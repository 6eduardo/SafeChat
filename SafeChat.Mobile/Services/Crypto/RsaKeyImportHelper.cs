using System.Security.Cryptography;
using System.Text;

namespace SafeChat.Mobile.Services.Crypto;

/// <summary>
/// Importa chaves RSA em PEM ou Base64 (formato PKCS#1/PKCS#8 usado pela app).
/// </summary>
internal static class RsaKeyImportHelper
{
    public static RSA ImportPublicKey(string publicKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(publicKey);

        var rsa = RSA.Create();

        if (publicKey.Contains("BEGIN", StringComparison.Ordinal))
        {
            rsa.ImportFromPem(publicKey);
            return rsa;
        }

        rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey.Trim()), out _);
        return rsa;
    }

    public static RSA ImportPrivateKey(string privateKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(privateKey);

        var rsa = RSA.Create();

        if (privateKey.Contains("BEGIN", StringComparison.Ordinal))
        {
            rsa.ImportFromPem(privateKey);
            return rsa;
        }

        rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey.Trim()), out _);
        return rsa;
    }

    public static string ExportPublicKeyPem(RSA rsa)
    {
        var builder = new StringBuilder();
        builder.AppendLine("-----BEGIN RSA PUBLIC KEY-----");
        builder.AppendLine(Convert.ToBase64String(rsa.ExportRSAPublicKey(), Base64FormattingOptions.InsertLineBreaks));
        builder.AppendLine("-----END RSA PUBLIC KEY-----");
        return builder.ToString();
    }
}
