namespace Ava.Shared.Interfaces;

public interface ICurrencyService
{
    bool IsZeroDecimalCurrency(string currencyCode);
    int GetCurrencyDecimalPlaces(string currencyCode);
    string FormatAmount(decimal amount, string currencyCode);
}
