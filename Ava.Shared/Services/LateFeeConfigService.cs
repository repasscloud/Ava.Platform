namespace Ava.Shared.Services;

public class LateFeeConfigService : ILateFeeConfigService
{
    private readonly ApplicationDbContext _context;
    private readonly ILoggerService _logger;

    public LateFeeConfigService(ApplicationDbContext context, ILoggerService logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<LateFeeConfig> CreateAsync(LateFeeConfig config)
    {
        await _logger.LogTraceAsync($"[LateFeeConfigService.CreateAsync] Entering with LicenseAgreementId={config.LicenseAgreementId}");
        // ensure parent exists
        var parentExists = await _context.LicenseAgreements
                                    .AsNoTracking()
                                    .AnyAsync(a => a.Id == config.LicenseAgreementId);
        if (!parentExists)
        {
            await _logger.LogWarningAsync(
                $"[LateFeeConfigService.CreateAsync] LicenseAgreement '{config.LicenseAgreementId}' not found, cannot create config");
            throw new KeyNotFoundException($"LicenseAgreement '{config.LicenseAgreementId}' not found.");
        }

        await _context.LateFeeConfigs.AddAsync(config);
        await _context.SaveChangesAsync();

        await _logger.LogInfoAsync(
            $"[LateFeeConfigService.CreateAsync] Created LateFeeConfig '{config.Id}' for LicenseAgreement '{config.LicenseAgreementId}'");
        return config;
    }

    public async Task<LateFeeConfig> CreateForAgreementAsync(string licenseAgreementId, LateFeeConfig template)
    {
        await _logger.LogTraceAsync(
            $"[LateFeeConfigService.CreateForAgreementAsync] Entering for LicenseAgreementId={licenseAgreementId}");

        var agreement = await _context.LicenseAgreements
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(a => a.Id == licenseAgreementId);
        if (agreement is null)
        {
            await _logger.LogWarningAsync(
                $"[LateFeeConfigService.CreateForAgreementAsync] LicenseAgreement '{licenseAgreementId}' not found");
            throw new KeyNotFoundException($"LicenseAgreement '{licenseAgreementId}' not found.");
        }

        var cfg = new LateFeeConfig
        {
            LicenseAgreementId = licenseAgreementId,
            GracePeriodDays    = template.GracePeriodDays,
            UseFixedAmount     = template.UseFixedAmount,
            FixedAmount        = template.FixedAmount,
            PercentOfInvoice   = template.PercentOfInvoice,
            RecurringOption    = template.RecurringOption,
            MaxLateFeeCap      = template.MaxLateFeeCap
        };

        await _context.LateFeeConfigs.AddAsync(cfg);
        await _context.SaveChangesAsync();

        await _logger.LogInfoAsync(
            $"[LateFeeConfigService.CreateForAgreementAsync] Created LateFeeConfig '{cfg.Id}' from template for Agreement '{licenseAgreementId}'");
        return cfg;
    }

    public async Task<string> QuickGenLateFeeConfigAsync(string licenseAgreementId)
    {
        await _logger.LogTraceAsync("");

        var cfg = new LateFeeConfig
        {
            LicenseAgreementId = licenseAgreementId,
        };

        await _context.LateFeeConfigs.AddAsync(cfg);
        await _context.SaveChangesAsync();

        await _logger.LogInfoAsync(
            $"[LateFeeConfigService.QuickGenLateFeeConfigAsync] Created LateFeeConfig '{cfg.Id}' with defaults for Agreement '{licenseAgreementId}'");
        return cfg.Id;
    }

    public async Task<IEnumerable<LateFeeConfig>> GetAllAsync()
    {
        await _logger.LogTraceAsync("[LateFeeConfigService.GetAllAsync] Retrieving all LateFeeConfigs");
        var list = await _context.LateFeeConfigs
                            .AsNoTracking()
                            .ToListAsync();
        await _logger.LogDebugAsync(
            $"[LateFeeConfigService.GetAllAsync] Retrieved {list.Count} items");
        return list;
    }

    public async Task<LateFeeConfig?> GetByIdAsync(string id)
    {
        await _logger.LogTraceAsync($"[LateFeeConfigService.GetByIdAsync] Fetching LateFeeConfig '{id}'");
        var cfg = await _context.LateFeeConfigs
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == id);
        if (cfg is null)
            await _logger.LogWarningAsync(
                $"[LateFeeConfigService.GetByIdAsync] LateFeeConfig '{id}' not found");
        else
            await _logger.LogDebugAsync(
                $"[LateFeeConfigService.GetByIdAsync] Found LateFeeConfig '{id}'");
        return cfg;
    }

    public async Task<IEnumerable<LateFeeConfig>> GetByAgreementIdAsync(string licenseAgreementId)
    {
        await _logger.LogTraceAsync(
            $"[LateFeeConfigService.GetByAgreementIdAsync] Fetching configs for Agreement '{licenseAgreementId}'");
        var list = await _context.LateFeeConfigs
                            .AsNoTracking()
                            .Where(x => x.LicenseAgreementId == licenseAgreementId)
                            .ToListAsync();
        await _logger.LogDebugAsync(
            $"[LateFeeConfigService.GetByAgreementIdAsync] Retrieved {list.Count} items for Agreement '{licenseAgreementId}'");
        return list;
    }

    public async Task UpdateAsync(LateFeeConfig config)
    {
        await _logger.LogTraceAsync(
            $"[LateFeeConfigService.UpdateAsync] Updating LateFeeConfig '{config.Id}'");
        _context.LateFeeConfigs.Update(config);
        await _context.SaveChangesAsync();
        await _logger.LogInfoAsync(
            $"[LateFeeConfigService.UpdateAsync] Successfully updated LateFeeConfig '{config.Id}'");
    }

    public async Task DeleteAsync(string id)
    {
        await _logger.LogTraceAsync($"[LateFeeConfigService.DeleteAsync] Deleting LateFeeConfig '{id}'");
        var existing = await _context.LateFeeConfigs.FindAsync(id);
        if (existing is null)
        {
            await _logger.LogWarningAsync(
                $"[LateFeeConfigService.DeleteAsync] LateFeeConfig '{id}' not found");
            throw new KeyNotFoundException($"LateFeeConfig '{id}' not found.");
        }

        _context.LateFeeConfigs.Remove(existing);
        await _context.SaveChangesAsync();
        await _logger.LogInfoAsync(
            $"[LateFeeConfigService.DeleteAsync] Deleted LateFeeConfig '{id}'");
    }

    public async Task<bool> ExistsAsync(string id)
    {
        await _logger.LogTraceAsync(
            $"[LateFeeConfigService.ExistsAsync] Checking existence of '{id}'");
        var exists = await _context.LateFeeConfigs
                                .AsNoTracking()
                                .AnyAsync(x => x.Id == id);
        await _logger.LogDebugAsync(
            $"[LateFeeConfigService.ExistsAsync] Exists('{id}') = {exists}");
        return exists;
    }

    public async Task<int> CountAsync()
    {
        await _logger.LogTraceAsync("[LateFeeConfigService.CountAsync] Counting all configs");
        var total = await _context.LateFeeConfigs
                                .AsNoTracking()
                                .CountAsync();
        await _logger.LogDebugAsync(
            $"[LateFeeConfigService.CountAsync] Total LateFeeConfigs = {total}");
        return total;
    }
}
