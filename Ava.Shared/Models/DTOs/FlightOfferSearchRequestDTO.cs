namespace Ava.Shared.Models.DTOs;

public class FlightOfferSearchRequestDTO
{
    [Required]
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("clientId")]
    [Required]
    public required string ClientId { get; set; }

    [JsonPropertyName("customerId")]
    [Required]
    public required string CustomerId { get; set; }

    [JsonPropertyName("travelPolicyId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [RegularExpression(@"^[0-9A-F]{10}$", ErrorMessage = "TravelPolicyId must be exactly 10 uppercase hexadecimal characters (0-9, A-F).")]
    [DefaultValue(null)]
    public string? TravelPolicyId { get; set; }

    [JsonPropertyName("currencyCode")]
    [Required]
    [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Currency code must be exactly 3 uppercase letters.")]
    public required string CurrencyCode { get; set; }

    [JsonPropertyName("originLocationCode")]
    [Required]
    [RegularExpression(@"^[A-Z0-9]{3}$", ErrorMessage = "Origin location code must be exactly 3 uppercase alphanumeric characters (A-Z, 0-9).")]
    public required string OriginLocationCode { get; set; }

    [JsonPropertyName("destinationLocationCode")]
    [Required]
    [RegularExpression(@"^[A-Z0-9]{3}$", ErrorMessage = "Destination location code must be exactly 3 uppercase alphanumeric characters (A-Z, 0-9).")]
    public required string DestinationLocationCode { get; set; }

    [JsonPropertyName("isOneWay")]
    public required bool IsOneWay { get; set; } = false;

    [JsonPropertyName("departureDate")]
    [Required]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Departure date must be in the format YYYY-MM-DD.")]
    public required string DepartureDate { get; set; }

    [JsonPropertyName("departureTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [RegularExpression(@"^\d{2}:\d{2}:\d{2}$", ErrorMessage = "Departure time must be in the format HH:mm:ss (24-hour time).")]
    public string? DepartureTime { get; set; }

    [JsonPropertyName("departureDateReturn")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Departure date must be in the format YYYY-MM-DD.")]
    [DefaultValue(null)] // ✅ Explicitly mark as optional
    public string? DepartureDateReturn { get; set; }

    [JsonPropertyName("departureTimeReturn")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [RegularExpression(@"^\d{2}:\d{2}:\d{2}$", ErrorMessage = "Departure time must be in the format HH:mm:ss (24-hour time).")]
    public string? DepartureTimeReturn { get; set; }

    [JsonPropertyName("adults")]
    [Range(1, 9, ErrorMessage = "Adults must be a single digit between 1 and 9.")]
    public required int Adults { get; set; }

    [JsonPropertyName("cabinClass")]
    [Required]
    [CabinTypeValidation]
    [DefaultValue("ECONOMY")]
    public required string CabinClass { get; set; } = "ECONOMY";

    [JsonPropertyName("cabinClassCoverage")]
    [Required]
    [CoverageTypeValidation]
    [DefaultValue("MOST_SEGMENTS")]
    public required string CabinClassCoverage { get; set; } = "MOST_SEGMENTS";

    [JsonPropertyName("excludedCarriers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExcludedCarrierCodes { get; set; }

    [JsonPropertyName("includedCarriers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IncludedCarrierCodes { get; set; }

    [JsonPropertyName("maxFlightPrice")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxFlightPrice { get; set; }

    [JsonPropertyName("nonStopFlight")]
    public bool NonStopFlight { get; set; } = false;

    [JsonPropertyName("maxFlightOffers")]
    [DefaultValue(10)]
    public int MaxFlightOffers { get; set; } = 20;
}
