namespace SafeChat.Mobile.Views;

public partial class SplashPage : ContentPage
{
    public SplashPage()
    {
        InitializeComponent();
    }

    private async void OnCriarContaClicked(object sender, EventArgs e)
    {
        // Navegar para a página de registo
        // await Shell.Current.GoToAsync(nameof(RegisterPage));
    }

    private async void OnJaTenhoContaClicked(object sender, EventArgs e)
    {
        // Navegar para a página de login
        // await Shell.Current.GoToAsync(nameof(LoginPage));
    }
}