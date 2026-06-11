using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SafeChat.Mobile.DTO.Auth;
using SafeChat.Mobile.Services.Interfaces;

namespace SafeChat.Mobile.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthenticationService _authService;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private bool _isLoading;

        public LoginViewModel(IAuthenticationService authService)
        {
            _authService = authService;
        }

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
                var request = new LoginRequestDto
                {
                    EmailOrUsername = Email,
                    Password = Password
                };

                await _authService.LoginAsync(request);

                await Shell.Current.GoToAsync("//Inicio");
            }
            catch (UnauthorizedAccessException ex)
            {
                await Shell.Current.DisplayAlertAsync("Erro", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Erro", $"Ocorreu um erro inesperado: {ex.Message}", "OK");
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