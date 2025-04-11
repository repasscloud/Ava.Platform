
namespace Ava.WebApp.Services;

public class IataLookupService : IIataLookupService
{
    private readonly AppDbContext _context;

    public IataLookupService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<double?> GetLatitudeAsync(string iataCode)
    {
        var result = await _context.IATAAirportCodes
            .FirstOrDefaultAsync(x => x.IATACode == iataCode);
        
        return result?.Latitude is decimal lat ? (double?)lat : null;
    }

    public async Task<double?> GetLongitudeAsync(string iataCode)
    {
        var result = await _context.IATAAirportCodes
            .FirstOrDefaultAsync(x => x.IATACode == iataCode);
        
        return result?.Longitude is decimal lon ? (double?)lon : null;
    }
}
