namespace SafeChat.Mobile.Views;

public partial class SplashPage : ContentPage
{
    public SplashPage()
    {
        InitializeComponent();
    }

    private async void OnCriarContaClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("RegisterPage");
    }

    private async void OnJaTenhoContaClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Login");
    }
}