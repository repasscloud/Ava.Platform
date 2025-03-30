namespace Ava.Shared.Models.Kernel.Jwt;

public class ArcadiaAppMBServerJwtTokenRequest
{
    public required string UserId { get; set; }
    public required string UserName { get; set; }
    public required string JwtToken { get; set; }
    public DateTime Expires { get; set; }
}
