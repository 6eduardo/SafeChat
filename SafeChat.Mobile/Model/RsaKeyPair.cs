namespace SafeChat.Mobile.Model;

/// <summary>
/// Par de chaves RSA-2048 gerado e mantido no dispositivo.
/// A chave privada nunca é transmitida — apenas armazenada via SecureStorage.
/// </summary>
public class RsaKeyPair
{
    /// <summary>Chave pública RSA-2048 em Base64 (enviada ao servidor no registo).</summary>
    public string PublicKey { get; set; } = string.Empty;

    /// <summary>Chave privada RSA-2048 em Base64 (permanece apenas no dispositivo).</summary>
    public string PrivateKey { get; set; } = string.Empty;
}
