namespace Ava.Shared.Models.Kernel.Jwt;

public class AvaJwtTokenResponse
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string JwtToken { get; set; }
    public DateTime Expires { get; set; }
    public bool IsValid { get; set; }
}
