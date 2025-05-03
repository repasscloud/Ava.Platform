namespace Ava.Shared.Interfaces;

public interface ILateFeeConfigService
{
    Task<LateFeeConfig> CreateAsync(LateFeeConfig config);
    Task<LateFeeConfig> CreateForAgreementAsync(string licenseAgreementId, LateFeeConfig template);
    Task<string> QuickGenLateFeeConfigAsync(string licenseAgreementId);
    Task<IEnumerable<LateFeeConfig>> GetAllAsync();
    Task<LateFeeConfig?> GetByIdAsync(string id);
    Task<IEnumerable<LateFeeConfig>> GetByAgreementIdAsync(string licenseAgreementId);
    Task UpdateAsync(LateFeeConfig config);
    Task DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<int> CountAsync();
}