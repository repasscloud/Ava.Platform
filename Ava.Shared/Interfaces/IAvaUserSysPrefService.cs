namespace Ava.Shared.Interfaces;

public interface IAvaUserSysPrefService
{
    Task<bool> CreateUserPrefRecordAsync(string aspNetUserId);
    Task<AvaUserSysPreference> GetUserPrefRecordAsync(string aspNetUserId);
    Task<bool> UpdateUserPrefRecordAsync(AvaUserSysPreference userSysPreference);
}
