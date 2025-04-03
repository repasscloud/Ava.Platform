namespace Ava.Shared.Models.Results.Flights;

public class FlightResultV1
{
    [Required]
    public required string CarrierLogoUrl { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OperatingCarrierLogo { get; set; }

    [Required]
    public required string FlightStartTime { get; set; }
    
    [Required]
    public required string FlightEndTime { get; set;}
    
    [Required]
    public required string DepartCityIATACode { get; set; }
    
    [Required]
    public required string ArrivalCityIATACode { get; set; }
    public string? AdditionalText { get; set; }

    // included items goes here
    
    [Required]
    public required int LayoverCount { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Layover1 { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OperatingCarrierLogoLayover1 { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Layover2 { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OperatingCarrierLogoLayover2 { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Layover3 { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OperatingCarrierLogoLayover3 { get; set; }
    
    [Required]
    public required string TravelTime { get; set; }
    
    public int Price { get; set; }

    [Required]
    [Alpha2Validation]
    public required string CurrencyCode { get; set; }
}
