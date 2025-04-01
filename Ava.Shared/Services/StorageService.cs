namespace Ava.Shared.Services;

public class StorageService : IStorageService
{
    private readonly ConcurrentDictionary<string, object> _storage = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly ApplicationDbContext _context;

    public StorageService(IServiceProvider serviceProvider, ApplicationDbContext context)
    {
        _serviceProvider = serviceProvider;
        _context = context;
    }

    public Task<string> StoreDataAsync<T>(T data) where T : class
    {
        string cacheKey;

        // use existing "Id" if present
        var idProperty = typeof(T).GetProperty("Id");
        if (idProperty is not null && idProperty.GetValue(data) is string idValue && !string.IsNullOrEmpty(idValue))
        {
            cacheKey = idValue;
        }
        else
        {
            cacheKey = NanoidDotNet.Nanoid.GenerateAsync().Result;
        }

        _storage[cacheKey] = data;
        return Task.FromResult(cacheKey);
    }

    public Task<T?> GetDataAsync<T>(string cacheKey) where T : class
    {
        _storage.TryGetValue(cacheKey, out var data);
        return Task.FromResult(data as T);
    }

    public Task RemoveDataAsync(string cacheKey)
    {
        _storage.TryRemove(cacheKey, out _);
        return Task.CompletedTask;
    }

    public async Task<bool> StoreAmadeusFlightOfferSearchResultAsync(AmadeusFlightOfferSearchResult data, string cacheKey)
    {
        // Remove existing entry if it exists
        var existResult = await _context.StorageEntries.FirstOrDefaultAsync(c => c.Id == cacheKey);
        if (existResult != null)
        {
            _context.StorageEntries.Remove(existResult);
            await _context.SaveChangesAsync();
        }

        // Create new storage entry with serialized data
        var entry = new StorageEntry
        {
            Id = cacheKey,
            SerializedData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = false })
        };

        // Add and save the new entry
        await _context.AddAsync(entry);
        await _context.SaveChangesAsync();

        // Verify if the entry was successfully created
        var createdEntry = await _context.StorageEntries.FirstOrDefaultAsync(c => c.Id == cacheKey);
        return createdEntry != null;
    }

    public async Task<AmadeusFlightOfferSearchResult?> GetAmadeusFlightOfferSearchResultAsync(string uniqueId)
    {
        var result = await _context.StorageEntries.FirstOrDefaultAsync(c => c.Id == uniqueId);
        if (result != null)
        {
            // Deserialize using the SerializedData property
            return JsonSerializer.Deserialize<AmadeusFlightOfferSearchResult>(result.SerializedData);
        }
        return null;
    }

    public async Task RemoveAmadeusFlightOfferSearchResultAsync(string uniqueId)
    {
        var entry = await _context.StorageEntries.FirstOrDefaultAsync(c => c.Id == uniqueId);
        if (entry != null)
        {
            _context.StorageEntries.Remove(entry);
            await _context.SaveChangesAsync();
        }
    }
}
