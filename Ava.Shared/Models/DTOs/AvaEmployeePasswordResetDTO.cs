namespace Ava.Shared.Models.DTOs;
public class PasswordResetDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}