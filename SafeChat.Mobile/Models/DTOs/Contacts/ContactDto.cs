namespace SafeChat.Mobile.Models.DTOs.Contacts;

/// <summary>
/// DTO de contacto/utilizador devolvido pela API para pesquisa e listagem.
/// Expõe apenas dados públicos — sem informação sensível.
/// </summary>
public class ContactDto
{
    /// <summary>Identificador do utilizador.</summary>
    public int Id { get; set; }

    /// <summary>Nome de utilizador.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>Endereço de e-mail.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Indica se o utilizador está online (atualizado via SignalR).</summary>
    public bool IsOnline { get; set; }

    /// <summary>Data/hora UTC da última atividade, se offline.</summary>
    public DateTime? LastSeenAt { get; set; }

    /// <summary>Chave pública RSA-2048 em Base64 para troca de chaves AES.</summary>
    public string? PublicKey { get; set; }
}
