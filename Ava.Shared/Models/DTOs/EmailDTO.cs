namespace Ava.Shared.Models.DTOs;

public class EmailDTO
{
    [JsonPropertyName("email")]
    public required string Email { get; set; }
}
