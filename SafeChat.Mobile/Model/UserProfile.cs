namespace SafeChat.Mobile.Model;

/// <summary>
/// Perfil do utilizador autenticado para o ecrã de definições.
/// </summary>
public class UserProfile
{
  public string DisplayName { get; set; } = string.Empty;

  public string Username { get; set; } = string.Empty;

  public string Email { get; set; } = string.Empty;

  public string Initials { get; set; } = string.Empty;

  public string PublicKeySnippet { get; set; } = string.Empty;

  public bool IsPrivateKeySecure { get; set; }

  public bool IsKeystoreActive { get; set; }

  public string PrivateKeyStatusText => IsPrivateKeySecure ? "Segura" : "Indisponível";

  public string KeystoreStatusText => IsKeystoreActive ? "Activo" : "Inactivo";
}
