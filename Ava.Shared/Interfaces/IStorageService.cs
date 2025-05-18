namespace Ava.Shared.Interfaces;

public interface IStorageService
{
    Task<string> StoreDataAsync<T>(T data) where T : class;
    Task<T?> GetDataAsync<T>(string uniqueId) where T : class;
    Task RemoveDataAsync(string uniqueId);

    Task<bool> StoreAmadeusFlightOfferSearchResultAsync(AmadeusFlightOfferSearchResult data, string cacheKey);
    Task<AmadeusFlightOfferSearchResult?> GetAmadeusFlightOfferSearchResultAsync(string uniqueId);
    Task RemoveAmadeusFlightOfferSearchResultAsync(string uniqueId);

    Task<TravelSearchRecord?> GetTravelSearchRecordAsync(string uniqueId);
}
