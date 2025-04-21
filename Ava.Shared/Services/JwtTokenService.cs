namespace Ava.Shared.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public JwtTokenService(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string> GenerateTokenAsync(string userId, string username, string role, int expiryMinutes = 480)
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
            SigningCredentials = credentials
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

    public Task<bool> ValidateTokenAsync(string jwtToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveTokenToDbAsync(AvaJwtTokenResponse jwtToken)
    {
        await _context.AvaJwtTokenResponses.AddAsync(jwtToken);
        await _context.SaveChangesAsync();
        return true;
    }
}
