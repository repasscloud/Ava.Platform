namespace Ava.WebApp.Extensions;

public static class AmenityV1Extensions
{
    public static RenderFragment ToAmenityIcon(this AmenityV1 amenity) => builder =>
    {
        if (amenity == null || string.IsNullOrWhiteSpace(amenity.Description))
            return;

        var icon = amenity.Description.ToUpperInvariant() switch
        {
            "MEAL" => Icons.Material.Filled.Restaurant,
            "WIFI" => Icons.Material.Filled.Wifi,
            "POWER" => Icons.Material.Filled.Power,
            _ => Icons.Material.Filled.Info
        };

        // Base Icon
        builder.OpenComponent<MudIcon>(0);
        builder.AddAttribute(1, "Icon", icon);
        builder.AddAttribute(2, "Size", Size.Small);
        builder.CloseComponent();

        if (amenity.IsChargeable)
        {
            // Overlay Icon (Dollar Sign)
            builder.OpenComponent<MudIcon>(3);
            builder.AddAttribute(4, "Icon", Icons.Material.Filled.AttachMoney);
            builder.AddAttribute(5, "Size", Size.Small);
            builder.AddAttribute(6, "Color", Color.Error);
            builder.AddAttribute(7, "Style",
                "position: absolute; bottom: 1px; right: -4px; background-color: white; border-radius: 50%; padding: 0.5px; font-size: 0.8em;");
            builder.CloseComponent();
        }
    };
}
