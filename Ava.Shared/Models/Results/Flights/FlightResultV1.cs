namespace Ava.Shared.Models.Results.Flights;

public class FlightResultV1
{
    [Required]
    public required bool IsOneWay { get; set; }

    [Required]
    public required DateTime LastTicketingDate { get; set; }

    [Required]
    public int NumberOfBookableSeats { get; set; }

    [Required]
    public int TotalNumberOfItineraries { get; set; }

    [Required]
    public int TotalNumberOfSegments { get; set; }

    [Required]
    public required List<ItineraryV1> ItineraryGroup { get; set; }

    [Required]
    [Alpha3Validation]
    public required string CurrencyCode { get; set; }

    [Required]
    public decimal GrandTotal { get; set; }

    [Required]
    public required TimeSpan TotalTravelTime { get; set; }
}

public class ItineraryV1
{
    [Required]
    public int ItineraryV1Position { get; set; }

    [Required]
    public int NumberOfSegments { get; set; }

    [Required]
    public required TimeSpan TravelTime { get; set; }

    [Required]
    public required List<SegmentV1> SegmentGroup { get; set; }
}

public class SegmentV1
{
    [Required]
    public required DepartureV1 Departure { get; set; }

    [Required]
    public required ArrivalV1 Arrival { get; set; }
    
    [Required]
    public required string CarrierCode { get; set; }

    [Required]
    public required string OperatingCarrierCode { get; set; }
    
    [Required]
    public required string Number { get; set; }
    
    [Required]
    public required string Aircraft { get; set; }
    
    [Required]
    public int NumberOfStops { get; set; }

    [Required]
    public required string CabinClass { get; set; }
    
    [Required]
    public required string Id { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<AmenityV1>? AmenityGroup { get; set; }
}

public class DepartureV1
{
    [Required]
    public required string IataCode { get; set; }
    
    [Required]
    public required string Terminal { get; set; }
    
    [Required]
    public required DateTime At { get; set; }
}

public class ArrivalV1
{
    [Required]
    public required string IataCode { get; set; }
    
    [Required]
    public required string Terminal { get; set; }
    
    [Required]
    public required DateTime At { get; set; }
}

public class AmenityV1
{
    [Required]
    public required string Description { get; set; }
    
    [Required]
    public bool IsChargeable { get; set; }
    
    [Required]
    public required string AmentiyType { get; set; }
}
