namespace Ava.Shared.Services;

public class CurrencyService : ICurrencyService
{
    public bool IsZeroDecimalCurrency(string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
            return false;

        return ZeroDecimalCurrencies.Contains(currencyCode.ToUpperInvariant());
    }

    public int GetCurrencyDecimalPlaces(string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
            return 2;

        return ZeroDecimalCurrencies.Contains(currencyCode.ToUpperInvariant()) ? 0 : 2;
    }

    public string FormatAmount(decimal amount, string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
            currencyCode = "USD"; // fallback default

        int decimalPlaces = GetCurrencyDecimalPlaces(currencyCode);

        decimal roundedAmount = Math.Round(amount, decimalPlaces, MidpointRounding.AwayFromZero);

        string formatted = decimalPlaces switch
        {
            0 => $"{roundedAmount:0}",
            2 => $"{roundedAmount:0.00}",
            _ => $"{roundedAmount:0.##}"
        };

        return formatted;
    }

    /// <summary>
    /// List of known zero-decimal currencies (ISO 4217 standard).
    /// </summary>
    private static readonly HashSet<string> ZeroDecimalCurrencies = new()
    {
        "BIF", "CLP", "DJF", "GNF", "ISK", "JPY", "KMF",
        "KRW", "PYG", "RWF", "UGX", "UYI", "VND", "VUV",
        "XAF", "XOF", "XPF"
    };
}
