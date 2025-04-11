namespace Ava.WebApp.Interfaces;

public interface IIataLookupService
{
    Task<double?> GetLatitudeAsync(string iataCode);
    Task<double?> GetLongitudeAsync(string iataCode);
}
