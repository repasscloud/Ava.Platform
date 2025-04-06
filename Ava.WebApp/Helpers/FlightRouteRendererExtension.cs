namespace Ava.WebApp.Helpers;

public static class FlightRouteRenderer
{
    public static MarkupString BuildRouteLine(string origin, IEnumerable<string> stops, string destination)
    {
        var stopsList = stops?.ToList() ?? new();

        var dotsHtml = stopsList.Count > 0
            ? $@"
            <div style='display: flex; justify-content: space-between; width: 100%; z-index: 1;'>
                {string.Join("", stopsList.Select(_ => @"
                    <div style='display: flex; justify-content: center; align-items: center; width: 100%;'>
                        <div style='width: 10px; height: 10px; background-color: #1976d2; border-radius: 50%;'></div>
                    </div>
                "))}
            </div>"
            : "";

        var html = $@"
<div style='display: flex; align-items: center; width: 100%;'>

    <!-- Origin -->
    <span style='font-weight: bold;'>{origin}</span>

    <!-- Line -->
    <div style='flex: 1; margin: 0 8px; position: relative; display: flex; align-items: center;'>
        <!-- Background line -->
        <div style='position: absolute; top: 50%; left: 0; right: 0; height: 2px; background-color: #ccc; transform: translateY(-50%);'></div>

        {dotsHtml}
    </div>

    <!-- Destination -->
    <span style='font-weight: bold;'>{destination}</span>
</div>";

        return new MarkupString(html);
    }
}


