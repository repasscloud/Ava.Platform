namespace Ava.Shared.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateTokenAsync(string userId, string username, string role, int expiryMinutes = 60);

    Task<bool> ValidateTokenAsync(string jwtToken);
    Task<bool> SaveTokenToDbAsync(AvaJwtTokenResponse jwtToken);
}
