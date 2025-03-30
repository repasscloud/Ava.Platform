namespace Ava.Shared.Models.ExternalLib.IATA;

public class IATAAirportCodes
{
    [Key]
    [JsonPropertyName("id")]
    [Required]
    public required int Id { get; set; }

    [JsonPropertyName("ident")]
    [Required]
    public required string Identity { get; set; }

    [JsonPropertyName("type")]
    [Required]
    public required string Type { get; set; }

    [JsonPropertyName("name")]
    [Required]
    public required string Name { get; set; }

    [JsonPropertyName("latitude_deg")]
    [Required]
    public required decimal Latitude { get; set; }

    [JsonPropertyName("longitude_deg")]
    [Required]
    public decimal Longitude { get; set; }

    [JsonPropertyName("elevation_ft")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ElevationFt { get; set; }

    [JsonPropertyName("continent")]
    [Required]
    public required string Continent { get; set; }

    [JsonPropertyName("iso_country")]
    [Required]
    public required string IsoCountry { get; set; }

    [JsonPropertyName("iso_region")]
    [Required]
    public required string IsoRegion { get; set; }

    [JsonPropertyName("municipality")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Municipality { get; set; }

    [JsonPropertyName("scheduled_service")]
    [Required]
    public required string ScheduledService { get; set; }

    [JsonPropertyName("icao_code")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ICAOCode { get; set; }

    [JsonPropertyName("iata_code")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IATACode { get; set; }

    [JsonPropertyName("gps_code")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? GPSCode { get; set; }

    [JsonPropertyName("local_code")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LocalCode { get; set; }

    [JsonPropertyName("home_link")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? HomeLink { get; set; }

    [JsonPropertyName("wikipedia_link")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? WikipediaLink { get; set; }

    [JsonPropertyName("keywords")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Keywords { get; set; }

    [JsonPropertyName("_region")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? _Region { get; set; }
}
