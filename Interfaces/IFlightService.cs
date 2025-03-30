namespace Ava.Shared.Interfaces;

public interface IFlightService
{
    Task<AmadeusFlightOfferSearchResult> SearchFlightsAsync(FlightSearchCriteria criteria);
    // Task<FlightPricingResult> GetFlightPricingAsync(FlightPricingRequest request);
    // Task<FlightBookingResult> CreateFlightBookingAsync(FlightBookingRequest request);
    // Task<FlightBookingDetails> GetFlightBookingDetailsAsync(string bookingReference);
    // Task<bool> CancelFlightBookingAsync(string bookingReference);
}
