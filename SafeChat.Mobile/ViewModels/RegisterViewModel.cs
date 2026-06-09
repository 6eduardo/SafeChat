using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SafeChat.Mobile.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private static readonly Color StrengthActive = Color.FromArgb("#4CAF50");
    private static readonly Color StrengthInactive = Color.FromArgb("#E5E7EB");

    [ObservableProperty]
    private string _firstName = string.Empty;

    [ObservableProperty]
    private string _lastName = string.Empty;

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _confirmPassword = string.Empty;

    [ObservableProperty]
    private bool _acceptTerms;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private int _passwordStrength;

    public string PublicKeyPreview { get; } =
        "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA7xK9mN2pL4vR8wH3jF6sT1uY0zA5bC9dE2gI7kM4nO6qP3rS8tU1vW4xY7zA0bD3eF6gH9jK2lM5nO8pQ1rS4tU7vW0xY3zA6bC9dE2gH5jK8lM1nO4pQ7rS0tU3vW6xY9zA2bC5dE8gH1jK4lM7nO0pQ3rS6tU9vW2xY5zA8bC1dE4gH7jK0lM3nO6pQ9rS2tU5vW8xY1zA4bC7dE0gH3jK6lM9nO2pQ5rS8tU1vW4xY7zA0bC3dE6gH9jK2lM5nO8pQ1rS4tU7vW0xY3zA6bC9dE2gH5jK8lM1nO4pQ7rS0tU3vW6xY9zA2bC5dE8gH1jK4lM7nO0pQ3rS6tU9vW2xY5zA8bC1dE4gH7jK0lM3nO6pQ9rS2tU5vW8xY1zA4bC7dE0gH3jK6lM9nO2pQ5rS8tU1vW4xY7zA0bC3dE6gH9jK2lM5nO8pQ1rS4tU7vW0xY3zA6bC9dE2gH5jK8lM1nO4pQ7rS0tU3vW6xY9zA2bC5dE8gH1jK4lM7nO0pQ3rS6tU9vW2xY5zA8bC1dE4gH7jK0lM3nO6pQ9rS2tU5vW8xY1zA4bC7dE0gH3jK6lM9nO2pQ5rS8tU1vW4xY7zA0bC3dE6gH9jK2lM5nO8pQ1rS4tU7vW0xY3zA6bC9dE2gH5jK8lM1nO4pQ7rS0tU3vW6xY9zA2bC5dE8gH1jK4lM7nO0pQ3rS6tU9vW2xY5zA8bC1dE4gH7jK0lM3nO6pQ9rS2tU5vW8xY1zA4bC7dE0gH3jK6lM9nO2pQ5rS8tU1vW4xY7zA0bC3dE6gH9jK2lM5nO8pQIDAQAB";

    public bool ShowPasswordStrength => !string.IsNullOrEmpty(Password);

    public string PasswordStrengthText => PasswordStrength switch
    {
        0 => string.Empty,
        1 => "Fraca",
        2 => "Razoável",
        3 => "Boa",
        4 => "Forte – bom trabalho!",
        _ => string.Empty
    };

    public Color PasswordStrengthTextColor =>
        PasswordStrength >= 4 ? StrengthActive : Color.FromArgb("#6B7280");

    public Color Bar1Color => PasswordStrength >= 1 ? StrengthActive : StrengthInactive;
    public Color Bar2Color => PasswordStrength >= 2 ? StrengthActive : StrengthInactive;
    public Color Bar3Color => PasswordStrength >= 3 ? StrengthActive : StrengthInactive;
    public Color Bar4Color => PasswordStrength >= 4 ? StrengthActive : StrengthInactive;

    partial void OnPasswordChanged(string value)
    {
        PasswordStrength = CalculateStrength(value);
        NotifyStrengthProperties();
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (!await ValidateFormAsync())
            return;

        IsLoading = true;

        try
        {
            // TODO: chamar API de registo e gerar par RSA
            await Task.Delay(1500);
            await Shell.Current.DisplayAlertAsync("Sucesso", "Conta criada com sucesso!", "OK");
            await Shell.Current.GoToAsync("..");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task GoToLoginAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    private async Task<bool> ValidateFormAsync()
    {
        if (string.IsNullOrWhiteSpace(FirstName) ||
            string.IsNullOrWhiteSpace(LastName) ||
            string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password) ||
            string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            await Shell.Current.DisplayAlertAsync("Erro", "Por favor preenche todos os campos.", "OK");
            return false;
        }

        if (Password != ConfirmPassword)
        {
            await Shell.Current.DisplayAlertAsync("Erro", "As palavras-passe não coincidem.", "OK");
            return false;
        }

        if (!AcceptTerms)
        {
            await Shell.Current.DisplayAlertAsync("Erro", "Deves aceitar os Termos de Serviço e a Política de Privacidade.", "OK");
            return false;
        }

        return true;
    }

    private static int CalculateStrength(string password)
    {
        if (string.IsNullOrEmpty(password))
            return 0;

        var score = 0;
        if (password.Length >= 6) score++;
        if (password.Length >= 8) score++;
        if (password.Any(char.IsUpper) && password.Any(char.IsLower)) score++;
        if (password.Any(char.IsDigit)) score++;
        if (password.Any(c => !char.IsLetterOrDigit(c))) score++;

        return Math.Min(4, score);
    }

    private void NotifyStrengthProperties()
    {
        OnPropertyChanged(nameof(ShowPasswordStrength));
        OnPropertyChanged(nameof(PasswordStrengthText));
        OnPropertyChanged(nameof(PasswordStrengthTextColor));
        OnPropertyChanged(nameof(Bar1Color));
        OnPropertyChanged(nameof(Bar2Color));
        OnPropertyChanged(nameof(Bar3Color));
        OnPropertyChanged(nameof(Bar4Color));
    }
}
