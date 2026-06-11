namespace SafeChat.Mobile.DTO.Messages;

/// <summary>
/// Mensagem encriptada tal como transmitida e armazenada no servidor.
/// O servidor nunca tem acesso ao texto claro.
/// </summary>
public class MessageDto
{
    /// <summary>Identificador da mensagem.</summary>
    public int Id { get; set; }

    /// <summary>Identificador da conversa.</summary>
    public int ConversationId { get; set; }

    /// <summary>Identificador do remetente.</summary>
    public int SenderId { get; set; }

    /// <summary>Data/hora UTC de envio.</summary>
    public DateTime SentAt { get; set; }

    /// <summary>Conteúdo encriptado com AES-256-CBC em Base64.</summary>
    public string EncryptedContent { get; set; } = string.Empty;

    /// <summary>Chave AES encriptada com RSA-OAEP (chave pública do destinatário) em Base64.</summary>
    public string EncryptedAesKey { get; set; } = string.Empty;

    /// <summary>IV aleatório de 16 bytes em Base64 (único por mensagem).</summary>
    public string AesIv { get; set; } = string.Empty;
}
