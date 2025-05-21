namespace Ava.Shared.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateTokenAsync(
        string userId,
        string username,
        string role,
        string audience,
        string issuer,
        int expiryMinutes = 480);

    Task<bool> ValidateTokenAsync(string jwtToken);
    Task<bool> SaveTokenToDbAsync(AvaJwtTokenResponse jwtToken);

    Task<(bool IsValid, IActionResult? ErrorResult, string Issuer, string Audience, string Role)> ValidateBearerTokenAsync(HttpContext context);


    Task<(string Issuer, string Audience, string Role)> ExtractClaimsFromBearerTokenAsync(HttpContext context);

    Task<bool> LogoutAsync(string jwtToken);

    Task ClearAllTokensAsync();
}
