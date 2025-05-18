namespace Ava.Shared.Models.ExternalLib.TravelPayouts;

public class TPCityIATACode
{
    [JsonPropertyName("name_translations")]
    public NameTranslations NameTranslations { get; set; } = new NameTranslations();

    [JsonPropertyName("city_code")]
    public string? CityCode { get; set; }

    [JsonPropertyName("country_code")]
    public string? CountryCode { get; set; }

    [Required]
    [JsonPropertyName("time_zone")]
    public required string TimeZone { get; set; }

    [Key]
    [Required]
    [JsonPropertyName("code")]
    public required string Code { get; set; }

    [JsonPropertyName("iata_type")]
    public string? IataType { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("coordinates")]
    public Coordinates Coordinates { get; set; } = new Coordinates();

    [JsonPropertyName("flightable")]
    public bool Flightable { get; set; }
}

public class NameTranslations
{
    [JsonPropertyName("en")]
    public string En { get; set; } = string.Empty;
}

public class Coordinates
{
    [JsonPropertyName("lat")]
    public double Lat { get; set; } // Changed from int to double

    [JsonPropertyName("lon")]
    public double Lon { get; set; }
}
