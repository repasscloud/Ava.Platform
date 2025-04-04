namespace Ava.WebApp.Extensions;

public static class AirlineImageExtensions
{
    private const string BaseImageUrl = "https://raw.githubusercontent.com/repasscloud/IATAScraper/main/airline_vectors";

    public static string ToAirlineImageUrl(this string? carrierCode)
    {
        if (string.IsNullOrWhiteSpace(carrierCode))
        {
            return $"{BaseImageUrl}/__.svg";
        }

        return $"{BaseImageUrl}/{carrierCode}.svg";
    }
}
