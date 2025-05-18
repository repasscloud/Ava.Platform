namespace Ava.WebApp.Utilities;

public static class CitySearchHelper
{
    public static Task<IEnumerable<string>> SearchCityCodes(string value, Dictionary<string, string> cities, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Task.FromResult(Enumerable.Empty<string>());
        }

        var results = cities
            .Where(c => c.Key.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0) // Case-insensitive search
            .Select(c => c.Value); // ✅ Returns the airport code (SYD, MEL, etc.)

        return Task.FromResult(results);
    }
}
