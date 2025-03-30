namespace Ava.Shared.Models.DTOs;

public class CreateTravelPolicyDTO
{
    public required string PolicyName { get; set; }
    public int AvaClientId { get; set; }

    // financial details
    [Required]
    [CurrencyTypeValidation]
    [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Currency must be exactly 3 uppercase letters.")]
    [JsonPropertyName("currency")]
    [DefaultValue("AUD")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public required string DefaultCurrencyCode { get; set; } = "AUD";

    public int MaxFlightPrice { get; set; } = 0;

    // flight particulars
    [CabinTypeValidation]
    [DefaultValue("ECONOMY")]
    public string DefaultFlightSeating { get; set; } = "ECONOMY";

    [CabinTypeValidation]
    [DefaultValue("ECONOMY")]
    public string MaxFlightSeating { get; set; } = "ECONOMY";
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IncludedAirlineCodes { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExcludedAirlineCodes { get; set; }

    [CoverageTypeValidation]
    [DefaultValue("MOST_SEGMENTS")]
    public string CabinClassCoverage { get; set; } = "MOST_SEGMENTS";
    public bool NonStopFlight { get; set; } = false;

    // amadeus (and other system) specifics [meta]
    //public int MaxResults { get; set; } = 20;

    // times for bookings (business rules)
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [RegularExpression(@"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$", ErrorMessage = "Time must be in the format hh:mm:ss.")]
    public string? FlightBookingTimeAvailableFrom { get; set; }  // Local time. hh:mm:ss format, e.g 10:30:00

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [RegularExpression(@"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$", ErrorMessage = "Time must be in the format hh:mm:ss.")]
    public string? FlightBookingTimeAvailableTo { get; set; }  // Local time. hh:mm:ss format, e.g 10:30:00

    [DefaultValue(false)]
    public bool EnableSaturdayFlightBookings { get; set; } = false;

    [DefaultValue(false)]
    public bool EnableSundayFlightBookings { get; set; } = false;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? DefaultCalendarDaysInAdvanceForFlightBooking { get; set; }

    [JsonIgnore] // Prevent circular reference during serialization.
    public AvaClient? AvaClient { get; set; }

    // Allowed destinations (can be entire regions, continents, or specific countries)
    public List<int>? RegionIds { get; set; }
    public List<int>? ContinentIds { get; set; }
    public List<int>? CountryIds { get; set; }
    public List<int>? DisabledCountryIds { get; set; }
}
