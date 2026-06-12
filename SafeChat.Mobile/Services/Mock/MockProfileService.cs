using SafeChat.Mobile.Model;
using SafeChat.Mobile.Services.Auth;
using SafeChat.Mobile.Services.Interfaces;

namespace SafeChat.Mobile.Services.Mock;

/// <summary>
/// Fornece dados de perfil fictícios para desenvolvimento da UI.
/// </summary>
public class MockProfileService : IProfileService
{
  private readonly TokenService _tokenService;

  private const string DefaultPublicKeySnippet = "MIIBIj...";

  public MockProfileService(TokenService tokenService)
  {
    _tokenService = tokenService;
  }

  public async Task<UserProfile> GetProfileAsync(CancellationToken cancellationToken = default)
  {
    await Task.Delay(250, cancellationToken);

    var session = _tokenService.GetSession();

    var displayName = session?.Username ?? "João Silva";
    var email = session?.Email ?? "joao.silva@email.com";

    return new UserProfile
    {
      DisplayName = FormatDisplayName(displayName),
      Username = displayName,
      Email = email,
      Initials = GetInitials(displayName),
      PublicKeySnippet = DefaultPublicKeySnippet,
      IsPrivateKeySecure = true,
      IsKeystoreActive = true
    };
  }

  private static string FormatDisplayName(string name)
  {
    if (name.Contains(' '))
      return name;

    return name.Equals("joao", StringComparison.OrdinalIgnoreCase) ||
           name.Equals("joaosilva", StringComparison.OrdinalIgnoreCase)
      ? "João Silva"
      : name;
  }

  private static string GetInitials(string name)
  {
    var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    return parts.Length >= 2
      ? $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant()
      : name.Length >= 2
        ? name[..2].ToUpperInvariant()
        : name.ToUpperInvariant();
  }
}
