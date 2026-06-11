namespace SafeChat.Mobile.Model;

/// <summary>
/// Payload encriptado produzido localmente antes do envio à API.
/// Combina conteúdo AES-256-CBC com chave AES protegida por RSA-2048.
/// </summary>
public class EncryptedPayload
{
    /// <summary>Conteúdo encriptado com AES-256-CBC em Base64.</summary>
    public string EncryptedContent { get; set; } = string.Empty;

    /// <summary>Chave AES encriptada com RSA-OAEP em Base64.</summary>
    public string EncryptedAesKey { get; set; } = string.Empty;

    /// <summary>IV aleatório de 16 bytes em Base64.</summary>
    public string AesIv { get; set; } = string.Empty;
}
