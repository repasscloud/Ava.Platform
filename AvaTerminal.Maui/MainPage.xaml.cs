using System.Collections.ObjectModel;
using System.Windows.Input;
using AvaTerminal.Maui.Pages;


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
        { "EXT", "exit" },
        { "HLP", "help" } // this won’t be a real page route, but it lets us detect the input
    };

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;

        Loaded += (_, _) => CodeEntry.Focus();
    }

    public ICommand SubmitCommand => new Command(async () => await OnCodeSubmitted());

    private async Task OnCodeSubmitted()
    {
        var code = CodeEntry.Text?.Trim().ToUpperInvariant();

        if (string.IsNullOrWhiteSpace(code))
            return;

        if (code == "HLP")
        {
            await ShowHelpPopup();
            CodeEntry.Text = string.Empty;
            return;
        }

        if (_routeMap.TryGetValue(code, out var route))
        {
            if (route == "exit")
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            Console.WriteLine($"Navigating to: {route}");
            CodeEntry.Text = string.Empty;
            CodeEntry.Unfocus();
            await Shell.Current.GoToAsync($"//{route}");
        }
        else
        {
            await DisplayAlert("Unknown Code", $"The code '{code}' is not recognized.", "OK");
        }
    }


    private async void OnGoClicked(object sender, EventArgs e)
    {
        await OnCodeSubmitted();
    }

    private void CodeEntry_Completed(object sender, EventArgs e)
    {
        _ = OnCodeSubmitted();
    }

    public async Task ShowHelpPopup()
    {
        await Navigation.PushModalAsync(new HelpPopupPage(
            "User Management Help",
            "This page allows you to view and manage user accounts.\n\n" +
            "• USR → View all users\n" +
            "• NEW → Create a new user\n" +
            "• EXT → Exit"
        ));
    }
}
