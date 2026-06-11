namespace SafeChat.Mobile.DTO.Messages;

/// <summary>
/// Pedido de envio de mensagem encriptada à API.
/// </summary>
public class SendMessageRequestDto
{
    /// <summary>Identificador da conversa de destino.</summary>
    public int ConversationId { get; set; }

    /// <summary>Conteúdo encriptado com AES-256-CBC em Base64.</summary>
    public string EncryptedContent { get; set; } = string.Empty;

    /// <summary>Chave AES encriptada com RSA-OAEP em Base64.</summary>
    public string EncryptedAesKey { get; set; } = string.Empty;

    /// <summary>IV aleatório de 16 bytes em Base64.</summary>
    public string AesIv { get; set; } = string.Empty;
}
