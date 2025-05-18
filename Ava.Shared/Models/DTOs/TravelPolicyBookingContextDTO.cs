namespace Ava.Shared.Models.DTOs;

public class TravelPolicyBookingContextDTO
{
    public string? Id { get; set; }
    public string? PolicyName { get; set; }
    public int? AvaClientId { get; set; }
    public string? Currency { get; set; }
    public decimal? MaxFlightPrice { get; set; }
    public string? DefaultFlightSeating { get; set; }
    public string? MaxFlightSeating { get; set; }
    public string? CabinClassCoverage { get; set; }
    public string? FlightBookingTimeAvailableFrom { get; set; }
    public string? FlightBookingTimeAvailableTo { get; set; }
    public string? IncludedAirlineCodes { get; set; }
    public string? ExcludedAirlineCodes { get; set; }
}
