namespace Ava.Shared.Models.DTOs;
public class AvaEmployeeVerifyAccountDTO
{
    public string VerificationCode { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
