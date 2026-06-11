namespace SafeChat.Mobile.DTO.Contacts;

/// <summary>
/// Representação de um contacto devolvida pela API.
/// </summary>
public class ContactDto
{
    /// <summary>Identificador do utilizador contacto.</summary>
    public int UserId { get; set; }

    /// <summary>Nome de utilizador.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>E-mail do contacto.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Chave pública RSA-2048 em Base64 para troca de chaves AES.</summary>
    public string PublicKey { get; set; } = string.Empty;

    /// <summary>Indica se o contacto está online (sincronizado via SignalR).</summary>
    public bool IsOnline { get; set; }

    /// <summary>Última vez que o contacto esteve ativo (UTC).</summary>
    public DateTime? LastSeenAt { get; set; }
}
