using System.Security.Cryptography;
using SafeChat.Mobile.Model;

namespace SafeChat.Mobile.Services.Crypto;

public class RsaKeyService
{
    public RsaKeyPair GenerateKeyPair()
    {
        using var rsa = RSA.Create(2048);

        var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
        var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());

        return new RsaKeyPair
        {
            PublicKey = publicKey,
            PrivateKey = privateKey
        };
    }
}
