namespace Ava.Shared.Interfaces;

public interface ILicenseAgreementService
{
    /// <summary>
        /// Create a new LicenseAgreement.
        /// </summary>
        Task<LicenseAgreement> CreateAsync(LicenseAgreement agreement);

        /// <summary>
        /// Get all LicenseAgreements.
        /// </summary>
        Task<IEnumerable<LicenseAgreement>> GetAllAsync();

        /// <summary>
        /// Get a LicenseAgreement by its Id.
        /// </summary>
        Task<LicenseAgreement?> GetByIdAsync(string id);

        /// <summary>
        /// Get all LicenseAgreements for a given client.
        /// </summary>
        Task<IEnumerable<LicenseAgreement>> GetByClientIdAsync(string avaClientId);

        /// <summary>
        /// Update an existing LicenseAgreement.
        /// </summary>
        Task UpdateAsync(LicenseAgreement agreement);

        /// <summary>
        /// Delete a LicenseAgreement by its Id.
        /// </summary>
        Task DeleteAsync(string id);

        /// <summary>
        /// Does a LicenseAgreement with this Id exist?
        /// </summary>
        Task<bool> ExistsAsync(string id);

        /// <summary>
        /// How many LicenseAgreements are there?
        /// </summary>
        Task<int> CountAsync();
}