namespace AvaTerminal.Maui.Pages;

public partial class HelpPopupPage : ContentPage
{
    public HelpPopupPage() : this("Help", "No help available.") { }

    public HelpPopupPage(string title, string helpText)
    {
        InitializeComponent();
        HelpTitle.Text = title;
        HelpLabel.Text = helpText;
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
