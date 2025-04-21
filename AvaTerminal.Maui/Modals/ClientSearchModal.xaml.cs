namespace AvaAITerminal.Modals;

public partial class ClientSearchModal : ContentPage
{
    private readonly IApiService _apiService;

    public ClientSearchModal(IApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void OnSearchClicked(object sender, EventArgs e)
    {
        if (int.TryParse(IdEntry.Text, out var id))
        {
            var result = await _apiService.GetClientAsync(id);
            if (result is not null)
            {
                MessagingCenter.Send(this, "ClientSelected", result);
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Not Found", $"No client found with ID {id}", "OK");
            }
        }
        else
        {
            await DisplayAlert("Invalid ID", "Enter a valid integer ID", "OK");
        }
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
