using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AvaTerminal.Maui;

public partial class MainPage : ContentPage
{
    public ObservableCollection<string> CommandTable { get; } = new()
    {
        "FLT        Flight Bookings",
        "POL        Travel Policies",
        "CLT        Client Management",
        "USR        User Directory",
        "PNR        Booking References",
        "TKT        Ticketing Dashboard",
        "RCM        Reconciliation Monitor",
        "SUP        Support Tickets",
        "SLS        Sales & Usage Stats",
        "INV        Inventory Viewer",
        "MAP        Route Map & Regions",
        "TRX        Transactions & Billing",
        "NTF        Notifications & Alerts",
        "ENV        Test Environments",
        "DBG        Debug Tools",
        "ACT        Activity Logs",
        "AUT        Authentication",
        "CNF        Configuration Center",
        "UPD        Update & Patch Notes",
        "EXT        Exit"
    };

    private readonly Dictionary<string, string> _routeMap = new()
    {
        { "FLT", "flight" },
        { "POL", "policy" },
        { "CLT", "client" },
        { "USR", "users" },
        { "PNR", "pnr" },
        { "TKT", "ticketing" },
        { "RCM", "reconciliation" },
        { "SUP", "support" },
        { "SLS", "sales" },
        { "INV", "inventory" },
        { "MAP", "map" },
        { "TRX", "transactions" },
        { "NTF", "notifications" },
        { "ENV", "environment" },
        { "DBG", "debug" },
        { "ACT", "activity" },
        { "AUT", "auth" },
        { "CNF", "config" },
        { "UPD", "updates" },
        { "EXT", "exit" }
    };

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public ICommand SubmitCommand => new Command(async () => await OnCodeSubmitted());

    // Called by both the Go button and Enter key
    private async Task OnCodeSubmitted()
    {
        var code = CodeEntry.Text?.Trim().ToUpperInvariant();

        if (string.IsNullOrWhiteSpace(code))
            return;

        if (_routeMap.TryGetValue(code, out var route))
        {
            if (route == "exit")
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            Console.WriteLine($"Navigating to: {route}");
            await Shell.Current.GoToAsync(route);
        }
        else
        {
            await DisplayAlert("Unknown Code", $"The code '{code}' is not recognized.", "OK");
        }
    }

    // Go button handler
    private async void OnGoClicked(object sender, EventArgs e)
    {
        await OnCodeSubmitted();
    }

    private void CodeEntry_Completed(object sender, EventArgs e)
{
    // mimic the Go button
    _ = OnCodeSubmitted();
}

}
