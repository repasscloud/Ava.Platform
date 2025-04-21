namespace Ava.Shared.Models.Kernel.BoH;

public class AvaEmployeeRegisterDTO
{
    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    public bool IsActive { get; set; } = true;

    [Required]
    [Column(TypeName = "varchar(20)")] // Ensures string storage
    [RegularExpression("^(AvaEmployee|AvaAgent|AvaExternal|AvaContractor|System)$", 
        ErrorMessage = "EmployeeType must be AvaEmployee, AvaAgent, AvaExternal, or AvaContractor.")]
    public required string EmployeeType { get; set; }

    public InternalRole Role { get; set; }
}
