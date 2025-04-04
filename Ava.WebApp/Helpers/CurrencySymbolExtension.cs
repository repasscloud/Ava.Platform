namespace Ava.WebApp.Helpers;

public static class CurrencySymbolExtension
{
    public static string ToCurrencySymbol(this string currencyCode)
    {
        return currencyCode.ToUpperInvariant() switch
        {
            "AUD" => "A$",
            "EUR" => "€",
            "USD" => "$",
            "GBP" => "£",
            "CAD" => "C$",
            "CHF" => "CHF",
            "CNH" => "¥",
            "HKD" => "HK$",
            "NZD" => "NZ$",
            _ => currencyCode ?? string.Empty
        };
    }
}
