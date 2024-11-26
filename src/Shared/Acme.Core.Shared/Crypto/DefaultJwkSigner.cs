namespace Acme.Crypto;

/// <summary>
/// 默认Jwk签名器
/// </summary>
public class DefaultJwkSigner : ISigner
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="jwk"></param>
    public DefaultJwkSigner(JsonWebKey jwk)
    {
        Jwk = jwk;
    }

    /// <summary>
    /// JsonWebKey
    /// </summary>
    private JsonWebKey Jwk { get; }

    /// <inheritdoc/>
    public string Algorithm => this.Jwk.Algorithm;

    public ValueTask<JsonWebKey> ExportJwkAsync(bool hasPrivateKey = false, CancellationToken cancellationToken = default)
    {
        var publicJwk = this.Jwk.ExportPublicKey();
        return new(publicJwk);
    }

    /// <inheritdoc/>
    public ValueTask<byte[]> SignAsync(byte[] data, CancellationToken cancellationToken)
        => new(this.Jwk.GenerateSignature(data));

    /// <inheritdoc/>
    public ValueTask<bool> VerifyAsync(byte[] data, byte[] signature, CancellationToken cancellationToken)
        => new(this.Jwk.VerifySignature(data, signature));
}
