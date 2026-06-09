using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SafeChat.Mobile.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private bool _isLoading;

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlertAsync("Erro", "Por favor preenche todos os campos.", "OK");
                return;
            }

            IsLoading = true;

            try
            {
                // TODO: chamar o teu serviço de autenticação aqui
                await Task.Delay(1500); // simulação

                // Navegar para a página principal após login
                // await Shell.Current.GoToAsync("//MainPage");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task RegisterAsync()
        {
            await Shell.Current.GoToAsync("RegisterPage");
        }
    }
}