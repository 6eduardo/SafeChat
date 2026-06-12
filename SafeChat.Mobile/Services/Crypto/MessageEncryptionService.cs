using System.Security.Cryptography;
using SafeChat.Mobile.Model;

namespace SafeChat.Mobile.Services.Crypto;

/// <summary>
/// Pipeline E2EE: AES-256-CBC para conteúdo + RSA-OAEP (SHA-256) para a chave AES.
/// </summary>
public class MessageEncryptionService
{
    private readonly AesEncryptionService _aesEncryptionService;

    public MessageEncryptionService(AesEncryptionService aesEncryptionService)
    {
        _aesEncryptionService = aesEncryptionService;
    }

    public EncryptedPayload EncryptMessage(string plaintext, string recipientPublicKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plaintext);
        ArgumentException.ThrowIfNullOrWhiteSpace(recipientPublicKey);

        var aesMaterial = _aesEncryptionService.GenerateKey();
        var encryptedContent = _aesEncryptionService.Encrypt(plaintext, aesMaterial.Key, aesMaterial.Iv);

        using var rsa = RsaKeyImportHelper.ImportPublicKey(recipientPublicKey);
        var encryptedAesKeyBytes = rsa.Encrypt(aesMaterial.Key, RSAEncryptionPadding.OaepSHA256);

        return new EncryptedPayload
        {
            EncryptedContent = encryptedContent,
            EncryptedAesKey = Convert.ToBase64String(encryptedAesKeyBytes),
            AesIv = Convert.ToBase64String(aesMaterial.Iv)
        };
    }

    public string DecryptMessage(
        string encryptedContent,
        string encryptedAesKey,
        string aesIv,
        string recipientPrivateKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(encryptedContent);
        ArgumentException.ThrowIfNullOrWhiteSpace(encryptedAesKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(aesIv);
        ArgumentException.ThrowIfNullOrWhiteSpace(recipientPrivateKey);

        using var rsa = RsaKeyImportHelper.ImportPrivateKey(recipientPrivateKey);
        var aesKeyBytes = rsa.Decrypt(
            Convert.FromBase64String(encryptedAesKey),
            RSAEncryptionPadding.OaepSHA256);

        var iv = Convert.FromBase64String(aesIv);
        return _aesEncryptionService.Decrypt(encryptedContent, aesKeyBytes, iv);
    }
}
