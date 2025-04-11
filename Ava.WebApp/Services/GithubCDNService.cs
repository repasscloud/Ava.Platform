namespace Ava.WebApp.Services;

public class GithubCDNService : IGithubCDNService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILoggerService _logger;

    public GithubCDNService(IHttpClientFactory httpClientFactory, ILoggerService logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<string> GetCarrierImageUrlAsync(FlightOffer flightOffer)
    {
        if (flightOffer == null)
        {
            return string.Empty;
        }

        if (flightOffer?.Itineraries == null || flightOffer.Itineraries.Count == 0)
        {
            return string.Empty;
        }

        var firstItinerary = flightOffer.Itineraries[0];
        
        if (firstItinerary?.Segments == null || firstItinerary.Segments.Count == 0)
        {
            return string.Empty;
        }

        //itineraries[0].segments[0].carrierCode
        var firstSegment = firstItinerary.Segments[0];
        var carrierCode = firstSegment.CarrierCode;

        if (!string.IsNullOrEmpty(carrierCode))
        {
            var client = _httpClientFactory.CreateClient("GithubCDN");
            client.DefaultRequestHeaders.UserAgent.ParseAdd("AirlineLogoFetcher/1.0 (+https://github.com/repasscloud/Ava.Platform)");

            string endpoint = $"/repasscloud/IATAScraper/main/airline_vectors/{carrierCode}.svg";

            var response = await client.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = $"API Error {response.StatusCode}: {response}";
    
                await _logger.LogErrorAsync(errorMessage);
                
                return $"https://raw.githubusercontent.com/repasscloud/IATAScraper/main/airline_vectors/__.svg";
            }
            else
            {
                return $"https://raw.githubusercontent.com/repasscloud/IATAScraper/main/airline_vectors/{carrierCode}.svg";
            }
        }
        else
        {
            return $"https://raw.githubusercontent.com/repasscloud/IATAScraper/main/airline_vectors/__.svg";
        }
    }

    public async Task<string> GetOperatingCarrierImageUrlAsync(string iataCarrierCode)
    {
        if (iataCarrierCode == null)
        {
            return string.Empty;
        }
            
        var client = _httpClientFactory.CreateClient("GithubCDN");
        client.DefaultRequestHeaders.UserAgent.ParseAdd("AirlineLogoFetcher/1.0 (+https://github.com/repasscloud/Ava.Platform)");

        string endpoint = $"/repasscloud/IATAScraper/main/airline_vectors/{iataCarrierCode}.svg";

        var response = await client.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = $"API Error {response.StatusCode}: {response}";
    
            await _logger.LogErrorAsync(errorMessage);
            
            return $"https://raw.githubusercontent.com/repasscloud/IATAScraper/main/airline_vectors/__.svg";
        }
        else
        {
            return $"https://raw.githubusercontent.com/repasscloud/IATAScraper/main/airline_vectors/{iataCarrierCode}.svg";
        }
    }
}
