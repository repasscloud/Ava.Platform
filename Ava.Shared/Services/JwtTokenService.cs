namespace Ava.Shared.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILoggerService _loggerService;

    public JwtTokenService(ApplicationDbContext context, IConfiguration configuration, ILoggerService loggerService)
    {
        _context = context;
        _configuration = configuration;
        _loggerService = loggerService;
    }

    public async Task<string> GenerateTokenAsync(
        string userId,
        string username,
        string role,
        string audience,
        string issuer,
        int expiryMinutes = 480)
    {
        var secretKey = _configuration["JwtSettings:SecretKey"]
            ?? throw new InvalidOperationException("JwtSettings:SecretKey is missing in the configuration");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64) // Issued At
        };

        // set token expiration
        var tokenExpiry = DateTime.UtcNow.AddMinutes(expiryMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = tokenExpiry,
            SigningCredentials = credentials,
            Issuer = issuer,
            Audience = audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        // generate the token
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        // create the JwtToken in the Api for future verification purposes
        AvaAIAppJwtToken avaJwtToken = new AvaAIAppJwtToken
        {
            Id = 0,
            JwtToken = jwtToken,
            Expires = tokenExpiry,
            IsValid = true,
        };

        // save it in the DB
        await _context.AvaAIAppJwtTokens.AddAsync(avaJwtToken);
        await _context.SaveChangesAsync();

        return jwtToken;
    }

    public async Task<bool> ValidateTokenAsync(string jwtToken)
    {
        if (string.IsNullOrWhiteSpace(jwtToken))
            return false;

        var record = await _context.AvaJwtTokenResponses
            .FirstOrDefaultAsync(j => j.JwtToken == jwtToken);

        return record?.IsValid == true;
    }

    public async Task<bool> SaveTokenToDbAsync(AvaJwtTokenResponse jwtToken)
    {
        await _context.AvaJwtTokenResponses.AddAsync(jwtToken);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(bool IsValid, IActionResult? ErrorResult, string Issuer, string Audience, string Role)> ValidateBearerTokenAsync(HttpContext context)
    {
        await _loggerService.LogTraceAsync("Entering ValidateBearerTokenAsync");

        try
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader) || string.IsNullOrWhiteSpace(authHeader))
            {
                await _loggerService.LogWarningAsync("Missing Authorization header");
                return (false, new UnauthorizedResult(), string.Empty, string.Empty, string.Empty);
            }

            var bearerToken = authHeader.ToString();
            if (!bearerToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                await _loggerService.LogErrorAsync("Invalid token format in Authorization header");
                return (false, new UnauthorizedResult(), string.Empty, string.Empty, string.Empty);
            }

            bearerToken = bearerToken["Bearer ".Length..].Trim();

            if (!await ValidateTokenAsync(bearerToken))
            {
                await _loggerService.LogErrorAsync("Invalid or expired token");
                return (false, new UnauthorizedResult(), string.Empty, string.Empty, string.Empty);
            }

            var issuer = GetIssuerFromToken(bearerToken);
            var audience = GetAudienceFromToken(bearerToken);
            var role = GetRoleFromToken(bearerToken);

            await _loggerService.LogInfoAsync($"Token validated: Issuer={issuer}, Audience={audience}, Role={role}");

            return (true, null, issuer, audience, role);
        }
        catch (Exception ex)
        {
            await _loggerService.LogCriticalAsync($"Exception in ValidateBearerTokenAsync: {ex}");
            throw;
        }
    }


    public async Task<(string Issuer, string Audience, string Role)> ExtractClaimsFromBearerTokenAsync(HttpContext context)
    {
        await _loggerService.LogTraceAsync("Entering ExtractClaimsFromBearerTokenAsync");

        try
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader) || string.IsNullOrWhiteSpace(authHeader))
            {
                await _loggerService.LogWarningAsync("Missing Authorization header");
                throw new UnauthorizedAccessException("Missing Authorization header");
            }

            var bearerToken = authHeader.ToString();
            if (!bearerToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                await _loggerService.LogErrorAsync("Invalid token format in Authorization header");
                throw new UnauthorizedAccessException("Invalid token format");
            }

            bearerToken = bearerToken["Bearer ".Length..].Trim();

            var issuer = GetIssuerFromToken(bearerToken);
            var audience = GetAudienceFromToken(bearerToken);
            var role = GetRoleFromToken(bearerToken);

            await _loggerService.LogInfoAsync($"Token claims extracted - Issuer: {issuer}, Audience: {audience}, Role: {role}");

            return (issuer, audience, role);
        }
        catch (Exception ex)
        {
            await _loggerService.LogCriticalAsync($"Exception in ExtractClaimsFromBearerTokenAsync: {ex}");
            throw;
        }
    }

    public async Task<bool> LogoutAsync(string jwtToken)
    {
        var deletedCount = await _context.AvaAIAppJwtTokens
            .Where(j => j.JwtToken == jwtToken)
            .ExecuteDeleteAsync();

        return deletedCount > 0;
    }

    public async Task ClearAllTokensAsync()
    {
        await _context.AvaAIAppJwtTokens.ExecuteDeleteAsync();
    }

    private string GetIssuerFromToken(string jwtToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwtToken);
        return token.Issuer;
    }

    private string GetAudienceFromToken(string jwtToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwtToken);
        return token.Audiences.FirstOrDefault() ?? string.Empty;
    }

    private string GetRoleFromToken(string jwtToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwtToken);
        
        return token.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")
            ?.Value ?? string.Empty;
    }
}
