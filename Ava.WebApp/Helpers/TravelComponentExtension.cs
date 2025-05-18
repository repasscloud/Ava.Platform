namespace Ava.WebApp.Helpers;

public static class TravelComponentExtension
{
    public static AmadeusFlightOfferSearchResult? GetFirstFlightOffer(this string json)
    {
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0)
        {
            return root[0].Deserialize<AmadeusFlightOfferSearchResult>();
        }

        return null;
    }

    public static AmadeusFlightOfferSearchResult? GetSecondFlightOffer(this string json)
    {
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 1)
        {
            return root[1].Deserialize<AmadeusFlightOfferSearchResult>();
        }

        return null;
    }

    public static T? DeserializeTo<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json);
    }
}
