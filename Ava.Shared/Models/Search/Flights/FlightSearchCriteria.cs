namespace Ava.Shared.Models.Search.Flights;

public class FlightSearchCriteria
{
    [Key]
    public string Id { get; set; } = Nanoid.Generate();

    [Required]
    [JsonPropertyName("_clientId")]
    public required string _clientId { get; set; }  // used by the system for logging purposes

    [Required]
    [JsonPropertyName("_customerId")]
    public required string _customerId { get; set; }  // used by the system for logging purposes

    [JsonPropertyName("_createdAt")]
    public DateTime _createdAt { get; set; } = DateTime.UtcNow;  // used by the system for logging purposes

    [Required]
    [JsonPropertyName("originLocationCode")]
    public required string OriginLocationCode { get; set; }

    [Required]
    [JsonPropertyName("destinationLocationCode")]
    public required string DestinationLocationCode { get; set; }

    [Required]
    [JsonPropertyName("departureDate")]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Departure date must be in the format YYYY-MM-DD.")]
    public required string DepartureDate { get; set; }

    [JsonPropertyName("returnDate")]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Return date must be in the format YYYY-MM-DD.")]
    public string? ReturnDate { get; set; }

    [Required]
    [JsonPropertyName("adults")]
    public required int Adults { get; set;}

    [JsonPropertyName("children")]
    public int? Children { get; set; }

    [JsonPropertyName("infants")]
    public int? Infants { get; set; }

    [JsonPropertyName("travelClass")]
    [EnumDataType(typeof(FlightTravelClassType), ErrorMessage = "Invalid travel class. Allowed values: ECONOMY, PREMIUM_ECONOMY, BUSINESS, FIRST.")]
    public FlightTravelClassType TravelClass { get; set; } = FlightTravelClassType.ECONOMY;

    [JsonPropertyName("includedAirlineCodes")]
    public string? IncludedAirlineCodes { get; set; }

    [JsonPropertyName("excludedAirlineCodes")]
    public string? ExcludedAirlineCodes { get; set; }

    [JsonPropertyName("nonStop")]
    public bool NonStop { get; set; } = false;

    [JsonPropertyName("currencyCode")]
    public string? CurrencyCode { get; set; }

    [JsonPropertyName("maxPrice")]
    public int? MaxPrice { get; set; }

    [JsonPropertyName("max")]
    public int? MaxResultCount { get; set; }
}
