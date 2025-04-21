using AvaAITerminal.Modals;

namespace AvaAITerminal.Pages;

public partial class ClientPage : ContentPage
{
    private readonly IApiService _apiService;
    
    public ClientPage(IApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;

        Loaded += (_, _) => CodeEntry.Focus();
    }

    private async void CodeEntry_Completed(object sender, EventArgs e)
    {
        await HandleCommand();
    }

    private async void OnGoClicked(object sender, EventArgs e)
    {
        await HandleCommand();
    }

    private async Task HandleCommand()
    {
        var code = CodeEntry.Text?.Trim().ToUpperInvariant();

        if (string.IsNullOrWhiteSpace(code))
            return;

        CodeEntry.Text = string.Empty;

        switch (code)
        {
            case "EXT":
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                break;

            case "HLP":
                await Navigation.PushModalAsync(new HelpPopupPage(
                    "Client Management Help",
                    "SRC → Search client\n" +
                    "CLT → Refresh client list\n" +
                    "NEW → Create client\n" +
                    "EXT → Exit"
                ));
                break;

            case "CLT":
                await DisplayAlert("Client List", "Would refresh clients here.", "OK");
                break;

            case "TOP":
                await Shell.Current.GoToAsync("//main");
                break;

            case "SRC":
                await ShowSearchModalAsync();
                break;

            case "NEW":
                _currentClient = new AvaClient
                {
                    CliendID = string.Empty,
                    CompanyName = string.Empty,
                    ContactPersonFirstName = string.Empty,
                    ContactPersonLastName = string.Empty,
                    ContactPersonPhone = string.Empty,
                    ContactPersonEmail = string.Empty,
                    BillingPersonFirstName = string.Empty,
                    BillingPersonLastName = string.Empty,
                    BillingPersonPhone = string.Empty,
                    BillingPersonEmail = string.Empty,
                    AdminPersonFirstName = string.Empty,
                    AdminPersonLastName = string.Empty,
                    AdminPersonPhone = string.Empty,
                    AdminPersonEmail = string.Empty,
                    DefaultCurrency = "AUD"
                };

                UpdateUIWithClient(_currentClient);
                break;

            case "SAV":
                if (_currentClient != null)
                {
                await _apiService.SaveClientAsync(_currentClient); // You'll implement this
                await DisplayAlert("Saved", "Client data saved.", "OK");
                }
                break;


            default:
                await DisplayAlert("Unknown Command", $"'{code}' is not valid on this page.", "OK");
                break;
        }
    }

    private AvaClient? _currentClient;

    private void UpdateUIWithClient(AvaClient client)
    {
        // Replace this with proper UI binding
        // e.g. populate entry fields or label contents
    }

    private async Task ShowSearchModalAsync()
    {
        MessagingCenter.Subscribe<ClientSearchModal, AvaClient>(this, "ClientSelected", (sender, client) =>
        {
            _currentClient = client;
            UpdateUIWithClient(client);
            MessagingCenter.Unsubscribe<ClientSearchModal, AvaClient>(this, "ClientSelected");
        });

        await Navigation.PushModalAsync(new ClientSearchModal(_apiService));
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

}
