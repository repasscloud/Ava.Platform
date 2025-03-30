namespace Ava.Shared.Services;

public class AuthenticationInfoService : IAuthenticationInfoService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthenticationInfoService(AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    private async Task<ClaimsPrincipal> GetAuthenticatedUserAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        return authState.User;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var user = await GetAuthenticatedUserAsync();
        return user.Identity?.IsAuthenticated ?? false;
    }

    public async Task<string?> GetAuthenticationMethodAsync()
    {
        var user = await GetAuthenticatedUserAsync();
        return user.FindFirst(ClaimTypes.AuthenticationMethod)?.Value ?? user.Identity?.AuthenticationType ?? "Unknown";
    }

    public async Task<string?> GetUserIdAsync()
    {
        var user = await GetAuthenticatedUserAsync();
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public async Task<string?> GetUserNameAsync()
    {
        var user = await GetAuthenticatedUserAsync();
        return user.Identity?.Name;
    }

    public async Task<string?> GetEmailAsync()
    {
        var user = await GetAuthenticatedUserAsync();
        return user.FindFirst(ClaimTypes.Email)?.Value;
    }

    public async Task<string?> GetUserRolesAsync()
    {
        var user = await GetAuthenticatedUserAsync();
        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
        return roles.Any() ? string.Join(", ", roles) : "None";
    }

    public async Task<ClaimsPrincipal> GetUserAsync()
    {
        return await GetAuthenticatedUserAsync();
    }
}
