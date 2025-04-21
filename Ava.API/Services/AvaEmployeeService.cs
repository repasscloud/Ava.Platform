using Microsoft.AspNetCore.Http.HttpResults;

namespace Ava.API.Services;

public class AvaEmployeeService : IAvaEmployeeService
{
    private readonly ApplicationDbContext _context;
    private readonly ILoggerService _loggerService;
    private readonly ICustomPasswordHasher _passwordHasher;

    public AvaEmployeeService(ApplicationDbContext context, ILoggerService logger, ICustomPasswordHasher passwordHasher)
    {
        _context = context;
        _loggerService = logger;
        _passwordHasher = passwordHasher;
    }

    public async Task<AvaEmployeeRecord?> GetByIdAsync(string id)
        => await _context.AvaEmployees.FindAsync(id);

    public async Task<List<AvaEmployeeRecord>> GetAllAsync()
        => await _context.AvaEmployees.ToListAsync();
    
    public async Task<AvaEmployeeRecord> CreateAsync(string firstName, string lastName, string email, bool isActive, string employeeType, string password, InternalRole role)
    {
        var employee = new AvaEmployeeRecord
        {
            Id = Nanoid.Generate(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PrivateKey = Nanoid.Generate(),
            CreatedAt = DateTime.UtcNow,
            IsActive = isActive,
            EmployeeType = employeeType,
            Role = role,
        };

        employee.PasswordHash = _passwordHasher.HashPassword(employee, password);
        _context.AvaEmployees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var user = await _context.AvaEmployees.FindAsync(id);
        if (user is null) return false;

        _context.AvaEmployees.Remove(user);
        await _context.SaveChangesAsync();
        await _loggerService.LogInfoAsync($"User '{user.Id}' has been deleted.");

        return true;
    }

    public async Task<bool> UpdateAsync(string id, AvaEmployeeUpdateDTO dto)
    {
        var user = await _context.AvaEmployees.FindAsync(id);
        if (user == null) return false;

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            if (!string.IsNullOrWhiteSpace(dto.Email)) user.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.FirstName)) user.FirstName = dto.FirstName;
            if (!string.IsNullOrWhiteSpace(dto.LastName)) user.LastName = dto.LastName;
            if (dto.IsActive.HasValue) user.IsActive = (bool)dto.IsActive;
            if (!string.IsNullOrWhiteSpace(dto.EmployeeType)) user.EmployeeType = dto.EmployeeType;
            if (dto.Role.HasValue) user.Role = dto.Role.Value;
            user.VerificationToken = null;
            user.PasswordHash = null;
            
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            await _loggerService.LogInfoAsync($"User '{user.Id}' was successfully updated.");
            await _context.SaveChangesAsync();
            return true;
        }
        
        return false;
    }

    public async Task<bool> SetNewPasswordAsync(string id, string newPassword, string verificationToken)
    {
        var user = await _context.AvaEmployees.FindAsync(id);
        if (user is null) return false;

        if (!string.IsNullOrWhiteSpace(verificationToken) && !string.IsNullOrWhiteSpace(newPassword))
        {
            if (user.VerificationToken != verificationToken)
            {
                return false;
            }
            
            // clear password and verification token on user object
            user.VerificationToken = null;
            user.PasswordHash = null;

            // generate new user password hash
            user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);

            await _context.SaveChangesAsync();
            return true;        
        }

        return false;
    }

    public Task<bool> ImpersonateAsRoleAsync(string role)
    {
        // implementation for impersonation use-case (dummy / mocked as per app logic)
        return Task.FromResult(Enum.TryParse(typeof(InternalRole), role, true, out _));
    }

    public async Task<bool> ResetPasswordAsync(string id)
    {
        var existing = await _context.AvaEmployees.FirstOrDefaultAsync(u =>
            u.Id.Equals(id, StringComparison.Ordinal) ||
            u.Email.Equals(id, StringComparison.OrdinalIgnoreCase)
        );

        if (existing is null)
        {
            await _loggerService.LogInfoAsync($"User with Id '{id}' was not found when requesting password reset.");
            return true;
        }
            
        existing.PasswordHash = null;
        existing.VerificationToken = Nanoid.Generate(size: 16);

        await _loggerService.LogInfoAsync($"User with Id '{id}' has requested password reset successfully.");

        await _context.SaveChangesAsync();
        return true;
    }

    public Task<bool> UpdatePasswordAsync(string id, string newPassword, string oldPassword)
    {
        throw new NotImplementedException();
    }
}
