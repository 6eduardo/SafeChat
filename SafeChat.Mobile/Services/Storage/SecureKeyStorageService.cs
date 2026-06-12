namespace SafeChat.Mobile.Services.Storage;

/// <summary>
/// Armazenamento seguro da chave privada RSA no dispositivo (SecureStorage / Keystore / Keychain).
/// </summary>
public class SecureKeyStorageService
{
    private const string PrivateKeyStorageKey = "safechat_rsa_private_key";

    public async Task SavePrivateKeyAsync(string privateKey, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(privateKey);
        cancellationToken.ThrowIfCancellationRequested();
        await SecureStorage.SetAsync(PrivateKeyStorageKey, privateKey);
    }

    public async Task<string?> GetPrivateKeyAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await SecureStorage.GetAsync(PrivateKeyStorageKey);
    }

    public async Task<bool> HasPrivateKeyAsync(CancellationToken cancellationToken = default)
    {
        var key = await GetPrivateKeyAsync(cancellationToken);
        return !string.IsNullOrEmpty(key);
    }

    public Task ClearPrivateKeyAsync()
    {
        SecureStorage.Remove(PrivateKeyStorageKey);
        return Task.CompletedTask;
    }
}
