namespace Ava.WebApp.Interfaces;

public interface IGithubCDNService
{
    Task<string> GetCarrierImageUrlAsync(FlightOffer flightOffer);
    Task<string> GetOperatingCarrierImageUrlAsync(string iataCarrierCode);
}
