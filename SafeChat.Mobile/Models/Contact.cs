namespace SafeChat.Mobile.Models;

/// <summary>
/// Modelo local de contacto para listagens e seleção na UI.
/// </summary>
public class Contact
{
    /// <summary>Identificador do utilizador contacto.</summary>
    public int UserId { get; set; }

    /// <summary>Nome de utilizador.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>E-mail do contacto.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Indica se o contacto está online.</summary>
    public bool IsOnline { get; set; }

    /// <summary>Chave pública RSA-2048 em Base64.</summary>
    public string PublicKey { get; set; } = string.Empty;

    /// <summary>Nome apresentado na UI (por defeito, o username).</summary>
    public string DisplayName => Username;
}
