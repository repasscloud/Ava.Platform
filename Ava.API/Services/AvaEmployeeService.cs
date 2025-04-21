using Microsoft.AspNetCore.Http.HttpResults;

namespace Ava.API.Services;

public class AvaEmployeeService : IAvaEmployeeService
{
    private readonly ApplicationDbContext _context;
    private readonly ILoggerService _logger;
    private readonly ICustomPasswordHasher _passwordHasher;

    public AvaEmployeeService(ApplicationDbContext context, ILoggerService logger, ICustomPasswordHasher passwordHasher)
    {
        _context = context;
        _logger = logger;
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
        return true;
    }

    public async Task<AvaEmployeeRecord?> UpdateAsync(string id, string password, string? firstName, string? lastName, string? email, bool? isActive, string? employeeType, InternalRole? role)
    {
        var user = await _context.AvaEmployees.FindAsync(id);
        if (user == null) return null;

        if (!string.IsNullOrWhiteSpace(firstName)) user.FirstName = firstName;
        if (!string.IsNullOrWhiteSpace(lastName)) user.LastName = lastName;
        if (!string.IsNullOrWhiteSpace(email)) user.Email = email;
        if (isActive.HasValue) user.IsActive = (bool)isActive;
        if (!string.IsNullOrWhiteSpace(employeeType)) user.EmployeeType = employeeType;
        if (role.HasValue) user.Role = role.Value;
        user.VerificationToken = null;
        user.PasswordHash = null;
        if (!string.IsNullOrWhiteSpace(password))
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
        }
        else
        {
            return null;
        }

        await _context.SaveChangesAsync();
        return user;
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

    //Task<bool> ResetPasswordAsync(string id);
    public async Task<bool> ResetPasswordAsync(string id)
    {
        var existing = await _context.AvaEmployees.FirstOrDefaultAsync(u =>
            u.Id.Equals(id, StringComparison.Ordinal) ||
            u.Email.Equals(id, StringComparison.OrdinalIgnoreCase)
        );

        if (existing is null)
            return false;

        existing.PasswordHash = null;
        existing.VerificationToken = Nanoid.Generate(size: 16);

        await _context.SaveChangesAsync();
        return true;
    }


    public Task<bool> UpdatePasswordAsync(string id, string newPassword, string oldPassword)
    {
        throw new NotImplementedException();
    }
}

