namespace Ava.Shared.Models.DTOs;

public class AvaEmployeeUpdateDTO
{
    public string Id { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool? IsActive { get; set; }
    public string? EmployeeType { get; set; }
    public InternalRole? Role { get; set; }
}
