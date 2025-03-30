namespace Ava.Shared.Interfaces;

public interface IAuthenticationInfoService
{
    Task<bool> IsAuthenticatedAsync();
    Task<string?> GetAuthenticationMethodAsync();
    Task<string?> GetUserIdAsync();
    Task<string?> GetUserNameAsync();
    Task<string?> GetEmailAsync();
    Task<string?> GetUserRolesAsync();
    Task<ClaimsPrincipal> GetUserAsync();
}
