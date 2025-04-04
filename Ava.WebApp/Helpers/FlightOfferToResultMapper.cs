namespace Ava.WebApp.Helpers;

public static class FlightOfferToResultMapper
{
    public static FlightResultV1 ToFlightResultV1(this FlightOffer offer)
    {
        if (offer is null)
        {
            throw new ArgumentNullException(nameof(offer));
        }

        var result = new FlightResultV1
        {
            IsOneWay = offer.OneWay,
            LastTicketingDate = DateTime.TryParse(offer.LastTicketingDate, out var ltd) ? ltd : DateTime.MinValue,
            NumberOfBookableSeats = offer.NumberOfBookableSeats,
            CurrencyCode = offer.Price?.Currency ?? string.Empty,
            GrandTotal = decimal.TryParse(offer.Price?.GrandTotal, out var total) ? Math.Round(total, 2) : 0m,
            TotalNumberOfItineraries = offer.Itineraries?.Count ?? 0,
            TotalNumberOfSegments = offer.Itineraries?.Sum(i => i.Segments?.Count ?? 0) ?? 0,
            ItineraryGroup = new List<ItineraryV1>()
        };

        var amenitiesBySegmentId = offer.TravelerPricings?
            .SelectMany(tp => tp.FareDetailsBySegment ?? Enumerable.Empty<FareDetailBySegment>())
            .Where(fd => !string.IsNullOrEmpty(fd.SegmentId))
            .GroupBy(fd => fd.SegmentId!)
            .ToDictionary(
                g => g.Key,
                g => g.SelectMany(fd => fd.Amenities ?? Enumerable.Empty<FareDetailBySegmentAmenity>()).ToList()
            ) ?? new Dictionary<string, List<FareDetailBySegmentAmenity>>();


        for (int i = 0; i < (offer.Itineraries?.Count ?? 0); i++)
        {
            var itinerary = offer.Itineraries![i];
            var itineraryResult = new ItineraryV1
            {
                ItineraryV1Position = i,
                NumberOfSegments = itinerary.Segments?.Count ?? 0,
                TravelTime = XmlConvert.ToTimeSpan(itinerary.Duration ?? "PT0H0M"),
                SegmentGroup = new List<SegmentV1>()
            };

            for (int j = 0; j < (itinerary.Segments?.Count ?? 0); j++)
            {
                var segment = itinerary.Segments![j];
                var segmentResult = new SegmentV1
                {
                    Departure = new DepartureV1
                    {
                        IataCode = segment.Departure?.IATACode ?? string.Empty,
                        Terminal = segment.Departure?.Terminal ?? string.Empty,
                        At = DateTime.TryParse(segment.Departure?.At, out var dep) ? dep : DateTime.MinValue
                    },
                    Arrival = new ArrivalV1
                    {
                        IataCode = segment.Arrival?.IATACode ?? string.Empty,
                        Terminal = segment.Arrival?.Terminal ?? string.Empty,
                        At = DateTime.TryParse(segment.Arrival?.At, out var arr) ? arr : DateTime.MinValue
                    },
                    CarrierCode = segment.CarrierCode ?? "__",
                    OperatingCarrierCode = segment.Operating?.CarrierCode ?? "__",
                    Number = segment.Number ?? string.Empty,
                    Aircraft = segment.Aircraft?.Code ?? string.Empty,
                    NumberOfStops = segment.numberOfStops,
                    Id = segment.Id ?? string.Empty,
                    AmenityGroup = amenitiesBySegmentId.TryGetValue(segment.Id ?? string.Empty, out var amenities)
                        ? amenities.Select(a => new AmenityV1
                        {
                            Description = a.Description ?? string.Empty,
                            IsChargeable = a.IsChargeable,
                            AmentiyType = a.AmenityType ?? string.Empty
                        }).ToList()
                        : null
                };

                itineraryResult.SegmentGroup.Add(segmentResult);
            }

            result.ItineraryGroup.Add(itineraryResult);
        }

        return result;
    }
}
