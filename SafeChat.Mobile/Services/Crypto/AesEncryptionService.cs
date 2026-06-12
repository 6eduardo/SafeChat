using System.Security.Cryptography;
using System.Text;

namespace SafeChat.Mobile.Services.Crypto;

/// <summary>
/// Encriptação simétrica AES-256-CBC com PKCS7.
/// </summary>
public class AesEncryptionService
{
    public AesKeyMaterial GenerateKey()
    {
        var key = RandomNumberGenerator.GetBytes(32);
        var iv = RandomNumberGenerator.GetBytes(16);
        return new AesKeyMaterial(key, iv);
    }

    public string Encrypt(string plaintext, byte[] key, byte[] iv)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plaintext);

        var plainBytes = Encoding.UTF8.GetBytes(plaintext);
        var cipherBytes = Encrypt(plainBytes, key, iv);
        return Convert.ToBase64String(cipherBytes);
    }

    public string Decrypt(string ciphertextBase64, byte[] key, byte[] iv)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ciphertextBase64);

        var cipherBytes = Convert.FromBase64String(ciphertextBase64);
        var plainBytes = Decrypt(cipherBytes, key, iv);
        return Encoding.UTF8.GetString(plainBytes);
    }

    private static byte[] Encrypt(byte[] plainBytes, byte[] key, byte[] iv)
    {
        using var aes = CreateAes(key, iv);
        using var encryptor = aes.CreateEncryptor();
        return encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
    }

    private static byte[] Decrypt(byte[] cipherBytes, byte[] key, byte[] iv)
    {
        using var aes = CreateAes(key, iv);
        using var decryptor = aes.CreateDecryptor();
        return decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
    }

    private static Aes CreateAes(byte[] key, byte[] iv)
    {
        var aes = Aes.Create();
        aes.KeySize = 256;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = key;
        aes.IV = iv;
        return aes;
    }
}
