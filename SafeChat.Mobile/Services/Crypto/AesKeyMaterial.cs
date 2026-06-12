namespace SafeChat.Mobile.Services.Crypto;

/// <summary>Chave AES-256 e IV gerados para uma única mensagem.</summary>
public sealed class AesKeyMaterial
{
    public byte[] Key { get; }
    public byte[] Iv { get; }

    public AesKeyMaterial(byte[] key, byte[] iv)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(key.Length, 32, nameof(key));
        ArgumentOutOfRangeException.ThrowIfNotEqual(iv.Length, 16, nameof(iv));
        Key = key;
        Iv = iv;
    }
}
