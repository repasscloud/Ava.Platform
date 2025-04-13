$pages = @(
    "FlightBookingPage",
    "TravelPolicyPage",
    "ClientPage",
    "UserDirectoryPage",
    "PnrPage",
    "TicketingPage",
    "ReconciliationPage",
    "SupportPage",
    "SalesPage",
    "InventoryPage",
    "MapPage",
    "TransactionPage",
    "NotificationsPage",
    "EnvironmentPage",
    "DebugPage",
    "ActivityLogPage",
    "AuthPage",
    "ConfigPage",
    "PatchNotesPage"
)

$namespace = "AvaTerminal.Maui.Pages"

$pages | ForEach-Object {
    $pageName = $_
    $className = "$namespace.$pageName"
    $path1 = "${PSScriptRoot}/Pages/${pageName}.xaml"
    $path2 = "${PSScriptRoot}/Pages/${pageName}.xaml.cs"

    # Generate display name from PascalCase
    $pretty = [System.Text.RegularExpressions.Regex]::Replace(
        $pageName,
        '(?<!^)([A-Z])',
        { param($m) " $($m.Groups[1].Value)" }
    )

    # XAML content with substituted values
    $xaml1 = @"
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="$className"
             Title="$pretty">
    <VerticalStackLayout Padding="20">
        <Label Text="$pretty Placeholder"
               FontSize="20"
               HorizontalOptions="Center" />
    </VerticalStackLayout>
</ContentPage>
"@

    $xaml2 = @"
namespace AvaTerminal.Maui.Pages;

public partial class $pageName : ContentPage
{
    public $pageName()
    {
        InitializeComponent();
    }
}
"@

    # Create files and write XAML content
    New-Item -ItemType File -Path $path1 -Force | Out-Null
    New-Item -ItemType File -Path $path2 -Force | Out-Null
    Set-Content -Path $path1 -Value $xaml1
    Set-Content -Path $path2 -Value $xaml2

    Write-Host "Generated $path1"
    Write-Host "Generated $path2"
}