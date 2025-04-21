namespace Ava.API.Interfaces;

public interface IAvaEmployeeService
{
    Task<AvaEmployeeRecord?> GetByIdAsync(string id);
    Task<List<AvaEmployeeRecord>> GetAllAsync();
    Task<AvaEmployeeRecord> CreateAsync(string firstName, string lastName, string email, bool isActive, string employeeType, string password, InternalRole role);
    Task<bool> DeleteAsync(string id);
    Task<AvaEmployeeRecord?> UpdateAsync(string id, string password, string? firstName, string? lastName, string? email, bool? isActive, string? employeeType, InternalRole? role);
    Task<bool> SetNewPasswordAsync(string id, string newPassword, string verificationToken);
    Task<bool> UpdatePasswordAsync(string id, string newPassword, string oldPassword);
    Task<bool> ResetPasswordAsync(string id);
    Task<bool> ImpersonateAsRoleAsync(string role);
}
