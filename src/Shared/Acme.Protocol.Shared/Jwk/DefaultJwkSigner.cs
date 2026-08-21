namespace Acme.Protocol.Jwk;

/// <summary>
/// 默认Jwk签名器
/// </summary>
public class DefaultJwkSigner : IJwkSigner
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="jwk"></param>
    public DefaultJwkSigner(JsonWebKey jwk)
    {
        this.Jwk = jwk;
    }

    /// <summary>
    /// JsonWebKey
    /// </summary>
    private JsonWebKey Jwk { get; }

    /// <inheritdoc/>
    public string Algorithm => this.Jwk.Algorithm;

    public ValueTask<JsonWebKey> ExportJwkAsync(bool hasPrivateKey = false, CancellationToken ct = default)
    {
        var publicJwk = this.Jwk.ExportPublicKey();
        return new(publicJwk);
    }

    /// <inheritdoc/>
    public ValueTask<byte[]> SignAsync(byte[] data, CancellationToken ct)
        => new(this.Jwk.GenerateSignature(data));

    /// <inheritdoc/>
    public ValueTask<bool> VerifyAsync(byte[] data, byte[] signature, CancellationToken ct)
        => new(this.Jwk.VerifySignature(data, signature));
}
