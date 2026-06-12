using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SafeChat.Mobile.Services.Auth;
using SafeChat.Mobile.Services.Interfaces;

namespace SafeChat.Mobile.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
  private readonly IProfileService _profileService;
  private readonly TokenService _tokenService;

  [ObservableProperty]
  private string _displayName = string.Empty;

  [ObservableProperty]
  private string _username = string.Empty;

  [ObservableProperty]
  private string _email = string.Empty;

  [ObservableProperty]
  private string _initials = "JS";

  [ObservableProperty]
  private string _publicKeySnippet = string.Empty;

  [ObservableProperty]
  private bool _isPrivateKeySecure;

  [ObservableProperty]
  private bool _isKeystoreActive;

  [ObservableProperty]
  private string _privateKeyStatusText = string.Empty;

  [ObservableProperty]
  private string _keystoreStatusText = string.Empty;

  [ObservableProperty]
  private bool _isLoading;

  public ProfileViewModel(IProfileService profileService, TokenService tokenService)
  {
    _profileService = profileService;
    _tokenService = tokenService;
  }

  [RelayCommand]
  private async Task LoadProfileAsync()
  {
    if (IsLoading)
      return;

    IsLoading = true;

    try
    {
      var profile = await _profileService.GetProfileAsync();
      DisplayName = profile.DisplayName;
      Username = profile.Username;
      Email = profile.Email;
      Initials = profile.Initials;
      PublicKeySnippet = profile.PublicKeySnippet;
      IsPrivateKeySecure = profile.IsPrivateKeySecure;
      IsKeystoreActive = profile.IsKeystoreActive;
      PrivateKeyStatusText = profile.PrivateKeyStatusText;
      KeystoreStatusText = profile.KeystoreStatusText;
    }
    finally
    {
      IsLoading = false;
    }
  }

  [RelayCommand]
  private async Task LogoutAsync()
  {
    var confirm = await Shell.Current.DisplayAlertAsync(
      "Terminar sessão",
      "Tens a certeza que queres sair?",
      "Sim",
      "Cancelar");

    if (!confirm)
      return;

    _tokenService.ClearSession();
    await Shell.Current.GoToAsync("//Login");
  }
}
