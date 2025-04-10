---
title: "Travel Search Record"
description: "Reference pages are ideal for outlining how things work in terse and clear terms."
summary: ""
date: 2023-09-07T16:13:18+02:00
lastmod: 2023-09-07T16:13:18+02:00
draft: false
weight: 910
toc: true
seo:
  title: "" # custom title (optional)
  description: "" # custom description (recommended)
  canonical: "" # custom canonical URL (optional)
  robots: "" # custom robot tags (optional)
---

Represents a modular travel search record with unique identifier supporting combinations of flights, hotels, cars, etc., and their types.

```csharp
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
```

When `TravelType` is equal to `TravelComponentType.Flight` _(2)_, it's suggested that there is only a Flight. They are not bitwise combinations, but rather unique values.

Full CSV download can be obtained [here](/files/Enum_Values.csv).

Each `xSubComponentType` has their own enum values.

For `FlightSubComponentType` the values are:

```csharp
public enum FlightSubComponentType
{
    None = 0,
    Flight_OneWay = 100,
    Flight_Return = 101,
    Flight_MultiCity = 102,
    Flight_OneWayWithStopOver = 103,
    Flight_OpenJaw = 104,
    Flight_MixedCabin = 105,
    Flight_FlexibleDates = 106,
    Flight_AlternateAirports = 107,
    Flight_WithPerks = 108
}
```

It's most common for the initial deployment of Ava.Platform to use `100` or `101`.

## FlightSubComponentType

### Flight_OneWay

When a `Flight_OneWay` value is selected, the `.Payload` will contain a single object that can be converted from `string` back to  the object type of `AmadeusFlightOfferSearchResult`. This needs to be deserialized back from `string` to object in-memory to be of use. This will only happen in the `Ava.WebApp` as it has private functions to that domain that manage this. It's part of the blazor services.

To obtain the first (and only) flight result, use the `TravelComponentExtension` class.

Use example:

```csharp
// convert the payload to a single result (one way flight)
_storedResults = _travelSearchRecord.Payload.DeserializeTo<AmadeusFlightOfferSearchResult>();
```

### Flight_Return

A `Flight_Return` has separate extension methods, whether it's the record at index `[0]` or `[1]`. There can only be 2 records. No more.

```csharp
_storedResults = _travelSearchRecord.Payload.GetFirstFlightOffer();

// or

_storedResults = _travelSearchRecord.Payload.GetSecondFlightOffer();
```
