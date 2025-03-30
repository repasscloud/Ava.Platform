namespace Ava.Shared.Models.Custom;
public class QantasCustomBookingRequest
{
    [Key]
    [JsonPropertyName("_id")]
    public int Id { get; set; } = 0;

    [JsonPropertyName("_ref")]
    public string Reference { get; set; } = Nanoid.Generate(Nanoid.Alphabets.LowercaseLettersAndDigits, 10);

    [Required]
    [JsonPropertyName("_clientCode")]
    public required string ClientCode { get; set; }

    [Required]
    [JsonPropertyName("_employeeCode")]
    public required string EmployeeCode { get; set; }

    [Required]
    [JsonPropertyName("_numberOfPeople")]
    public required int PassengerCount { get; set; }

    [Required]
    [JsonPropertyName("_isOneWay")]
    public required bool IsOneWay { get; set; }

    [Required]
    [JsonPropertyName("_departCity")]
    public required string DepartCity { get; set; }

    [Required]
    [JsonPropertyName("_destinationCity")]
    public string? DestinationCity { get; set; }

    [Required]
    [JsonPropertyName("_date")]
    public required string Date { get; set; }

    [JsonPropertyName("_returnDate")]
    public string? ReturnDate { get; set; }

    [Required]
    [JsonPropertyName("_fareClass")]
    public required string _fareClass { get; set; } = "ECONOMY";

    [Required]
    [JsonPropertyName("_includeJetstar")]
    public required bool _includeJetstar { get; set; } = false;

    [Required]
    [JsonPropertyName("_currencyCode")]
    public required string _currencyCode { get; set; }

    [Required]
    [JsonPropertyName("_searchCreated")]
    private DateTime SearchCreated { get; set; } = DateTime.UtcNow;
}

// this temporary class is to be used to receive the request from
// the front-end application, it will then be dropped as it won't be
// used for anything more than just receiving the order data from
// the front-end, then the API will then use the QantasCustomIATACode
// class to look up the correct IATA codes, and use the date/s to
// find the return trip (if required) or just the one-way flight data
//
// the _id and _ref will be stored in the db for later, when I need
// to check what an order (or a search result, I should say) looked
// like to see how it was created/stored etc.
