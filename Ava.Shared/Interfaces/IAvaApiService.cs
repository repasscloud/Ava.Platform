namespace Ava.Shared.Interfaces;

public interface IAvaApiService
{
    Task<AmadeusFlightOfferSearchResult> PostFlightSearchQueryAsync(FlightOfferSearchRequestDTO criteria, string bearerToken);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest request, string avaToken);
    Task<AvaClientSupportedDomain> GetClientIdAsync(EmailDTO criteria, string bearerToken);

    // return the travel policy
    Task<TravelPolicy> GetTravelPolicyByIdAsync(string travelPolicyId, string bearerToken);
}
