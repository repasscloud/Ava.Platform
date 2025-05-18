namespace Ava.Shared.Interfaces;

public interface IAvaApiService
{
    Task<TravelSearchRecordWrapper> PostFlightSearchQueryAsync(FlightOfferSearchRequestDTO criteria, string bearerToken);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest request, string avaToken);
    Task<AvaClientSupportedDomain> GetClientIdAsync(EmailDTO criteria, string bearerToken);

    // return the travel policy
    Task<TravelPolicy> GetTravelPolicyByIdAsync(string travelPolicyId, string bearerToken);

    Task<TravelPolicyBookingContextDTO> GetTravelPolicyInterResultByIdAsync(string travelPolicyId, string bearerToken);
    Task<AvaUserSysPreference> GetAvaUserSysPreferencesAsync(string aspNetUsersId, string bearerToken);
}
