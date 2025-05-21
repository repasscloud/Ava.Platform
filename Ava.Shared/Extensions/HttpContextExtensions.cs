namespace Ava.Shared.Extensions;
public static class HttpContextExtensions
{
    public static string GetJwtToken(this HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("Authorization", out var authHeader) &&
            authHeader.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return authHeader.ToString()["Bearer ".Length..].Trim();
        }

        return string.Empty;
    }
}
