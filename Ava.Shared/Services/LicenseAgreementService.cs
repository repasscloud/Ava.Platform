namespace Ava.Shared.Services;

public class LicenseAgreementService : ILicenseAgreementService
{
    private readonly ApplicationDbContext _context;
    private readonly ILoggerService _logger;

    public LicenseAgreementService(ApplicationDbContext context, ILoggerService logger)
    {
        _context = context;
        _logger  = logger;
    }

    public async Task<LicenseAgreement> CreateAsync(LicenseAgreement agreement)
    {
        await _logger.LogTraceAsync($"[LicenseAgreementService.CreateAsync] Entering, AvaClientId={agreement.AvaClientId}");
        await _context.LicenseAgreements.AddAsync(agreement);
        await _context.SaveChangesAsync();
        await _logger.LogInfoAsync($"[LicenseAgreementService.CreateAsync] Created LicenseAgreement '{agreement.Id}'");
        return agreement;
    }

    public async Task<IEnumerable<LicenseAgreement>> GetAllAsync()
    {
        await _logger.LogTraceAsync("[LicenseAgreementService.GetAllAsync] Fetching all");
        var list = await _context.LicenseAgreements
                                    .AsNoTracking()
                                    .ToListAsync();
        await _logger.LogDebugAsync($"[LicenseAgreementService.GetAllAsync] Retrieved {list.Count} agreements");
        return list;
    }

    public async Task<LicenseAgreement?> GetByIdAsync(string id)
    {
        await _logger.LogTraceAsync($"[LicenseAgreementService.GetByIdAsync] Fetching Id={id}");
        var agreement = await _context.LicenseAgreements
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.Id == id);
        if (agreement is null)
            await _logger.LogWarningAsync($"[LicenseAgreementService.GetByIdAsync] Not found: {id}");
        else
            await _logger.LogDebugAsync($"[LicenseAgreementService.GetByIdAsync] Found '{id}'");
        return agreement;
    }

    public async Task<IEnumerable<LicenseAgreement>> GetByClientIdAsync(string avaClientId)
    {
        await _logger.LogTraceAsync($"[LicenseAgreementService.GetByClientIdAsync] Fetching for Client={avaClientId}");
        var list = await _context.LicenseAgreements
                                    .AsNoTracking()
                                    .Where(x => x.AvaClientId == avaClientId)
                                    .ToListAsync();
        await _logger.LogDebugAsync(
            $"[LicenseAgreementService.GetByClientIdAsync] Retrieved {list.Count} for Client {avaClientId}");
        return list;
    }

    public async Task UpdateAsync(LicenseAgreement agreement)
    {
        await _logger.LogTraceAsync($"[LicenseAgreementService.UpdateAsync] Updating '{agreement.Id}'");
        _context.LicenseAgreements.Update(agreement);
        await _context.SaveChangesAsync();
        await _logger.LogInfoAsync($"[LicenseAgreementService.UpdateAsync] Updated '{agreement.Id}'");
    }

    public async Task DeleteAsync(string id)
    {
        await _logger.LogTraceAsync($"[LicenseAgreementService.DeleteAsync] Deleting '{id}'");
        var existing = await _context.LicenseAgreements.FindAsync(id);
        if (existing is null)
        {
            await _logger.LogWarningAsync($"[LicenseAgreementService.DeleteAsync] Not found: {id}");
            throw new KeyNotFoundException($"LicenseAgreement '{id}' not found.");
        }

        _context.LicenseAgreements.Remove(existing);
        await _context.SaveChangesAsync();
        await _logger.LogInfoAsync($"[LicenseAgreementService.DeleteAsync] Deleted '{id}'");
    }

    public async Task<bool> ExistsAsync(string id)
    {
        await _logger.LogTraceAsync($"[LicenseAgreementService.ExistsAsync] Checking '{id}'");
        var exists = await _context.LicenseAgreements
                                    .AsNoTracking()
                                    .AnyAsync(x => x.Id == id);
        await _logger.LogDebugAsync($"[LicenseAgreementService.ExistsAsync] Exists({id}) = {exists}");
        return exists;
    }

    public async Task<int> CountAsync()
    {
        await _logger.LogTraceAsync("[LicenseAgreementService.CountAsync] Counting all");
        var total = await _context.LicenseAgreements
                                    .AsNoTracking()
                                    .CountAsync();
        await _logger.LogDebugAsync($"[LicenseAgreementService.CountAsync] Total = {total}");
        return total;
    }
}