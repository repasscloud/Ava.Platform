namespace Ava.Shared.Models.DTOs;

public class EmailRequestDTO
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}
