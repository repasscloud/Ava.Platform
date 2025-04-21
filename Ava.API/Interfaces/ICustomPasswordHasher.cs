namespace Ava.API.Interfaces;

public interface ICustomPasswordHasher
{
    string HashPassword(object userObject, string password);
    bool VerifyPassword(object userObject, string password, string storedHash);
}
