namespace Ava.Shared.Models.Search.Travel;

/// <summary>
/// Represents a modular travel search record with unique identifier
/// supporting combinations of flights, hotels, cars, etc., and their types
/// </summary>
public class TravelSearchRecord
{
    [Key]
    public long Id { get; set; }

    [Required]
    public required string SearchId { get; set; }
    public TravelComponentType TravelType { get; set; } = TravelComponentType.None;
    public FlightSubComponentType FlightSubComponent { get; set; } = FlightSubComponentType.None;
    public HotelSubComponentType HotelSubComponent { get; set; } = HotelSubComponentType.None;
    public CarSubComponentType CarSubComponent { get; set; } = CarSubComponentType.None;
    public RailSubComponentType RailSubComponent { get; set; } = RailSubComponentType.None;
    public TransferSubComponentType TransferSubComponent { get; set; } = TransferSubComponentType.None;
    public ActivitySubComponentType ActivitySubComponent { get; set; } = ActivitySubComponentType.None;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(30);

    [Required]
    public required string Payload { get; set; }
}
