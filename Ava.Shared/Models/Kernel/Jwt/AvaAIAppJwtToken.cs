namespace Ava.Shared.Models.Kernel.Jwt;

public class AvaAIAppJwtToken
{
    [Key]
    public int Id { get; set; }
    public required string JwtToken { get; set; }
    public DateTime Expires { get; set; }
    public bool IsValid { get; set; }
}