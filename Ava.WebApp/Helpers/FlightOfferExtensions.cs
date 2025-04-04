namespace Ava.WebApp.Helpers;

public static class FlightOfferExtensions
{
    public static string GetFirstDepartureAt(this FlightOffer offer)
    {
        if (offer == null)
            return string.Empty;

        var itineraries = offer.Itineraries;
        if (itineraries == null || itineraries.Count == 0)
            return string.Empty;

        var firstItinerary = itineraries[0];
        if (firstItinerary?.Segments == null || firstItinerary.Segments.Count == 0)
            return string.Empty;

        var firstSegment = firstItinerary.Segments[0];
        return firstSegment?.Departure?.At ?? string.Empty;
    }

    public static string GetFirstArrivaleAt(this FlightOffer offer)
    {
        if (offer == null)
            return string.Empty;

        var itineraries = offer.Itineraries;
        if (itineraries == null || itineraries.Count == 0)
            return string.Empty;

        var firstItinerary = itineraries[0];
        if (firstItinerary?.Segments == null || firstItinerary.Segments.Count == 0)
            return string.Empty;

        var firstSegment = firstItinerary.Segments[0];
        return firstSegment?.Arrival?.At ?? string.Empty;
    }

    public static TimeSpan GetFirstItineraryDuration(this FlightOffer offer)
    {
        if (offer?.Itineraries != null &&
            offer.Itineraries.Count > 0 &&
            !string.IsNullOrEmpty(offer.Itineraries[0].Duration))
        {
            try
            {
                return XmlConvert.ToTimeSpan(offer.Itineraries[0].Duration!);
            }
            catch
            {
                // log here if needed
                return TimeSpan.Zero;
            }
        }

        return TimeSpan.Zero;
    }

    public static string GetCurrencyCode(this FlightOffer offer)
    {
        if (offer != null && offer.Price != null && !string.IsNullOrWhiteSpace(offer.Price.Currency))
        {
            return offer.Price.Currency;
        }

        return string.Empty;
    }

    public static decimal GetCalculatedTotal(
        this FlightOffer offer,
        decimal markupPercent,
        decimal flatFee,
        decimal taxMultiplier)
    {
        if (!decimal.TryParse(offer?.Price?.GrandTotal, out var baseTotal))
            baseTotal = 0m;

        decimal result = baseTotal;

        // Apply markup if applicable
        if (markupPercent > 0)
        {
            var markupAmount = baseTotal * (markupPercent / 100m);
            result += markupAmount;
        }

        // Apply flat fee if applicable
        if (flatFee > 0)
        {
            result += flatFee;
        }

        // Apply tax if applicable
        if (taxMultiplier > 0)
        {
            result *= taxMultiplier;
        }

        return Math.Round(result, 2);
    }
}
