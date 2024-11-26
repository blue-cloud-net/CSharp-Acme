using Org.BouncyCastle.Crypto.Parameters;

namespace Acme.Crypto.Jwk;

public class OctJsonWebKey : JsonWebKey
{
    /// <summary>
    /// Gets or sets the 'k' (Octet Sequence).
    /// </summary>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.K)]
    [JsonPropertyOrder(3)]
    public byte[] K { get; protected set; } = [];

    public override byte[] ComputeThumbprint()
    {
        if (this.K is null or { Length: 0 })
        {
            throw new ArgumentException("缺少密钥属性‘K’。");
        }

        var canonicalJwk =
            $"{{\"{JsonWebKeyParameterNames.K}\":\"{this.K}\"," +
            $"\"{JsonWebKeyParameterNames.Kty}\":\"{this.KeyType}\"}}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(canonicalJwk));

        return hash;
    }

    public override JsonWebKey ExportPublicKey()
        => new OctJsonWebKey()
        {
            KeyType = this.KeyType,
            Algorithm = this.Algorithm,
            K = this.K
        };

    public override byte[] GenerateSignature(byte[] awaitSignData)
    {
        if (awaitSignData is null or { Length: 0 })
        {
            throw new ArgumentException("待签名数据不能为空。");
        }
        else if (this.K is null or { Length: 0 })
        {
            throw new ArgumentException("缺少密钥属性‘K’。");
        }

        var key = new KeyParameter(this.K);
        var signer = BCSignerFactory.GetSigner(this.Algorithm);
        signer.Init(forSigning: true, key);
        signer.BlockUpdate(awaitSignData);
        var signature = signer.GenerateSignature();

        return signature;
    }

    public override bool VerifySignature(byte[] awaitSignData, byte[] signature)
    {
        if (awaitSignData is null or { Length: 0 })
        {
            throw new ArgumentException("待签名数据不能为空。");
        }
        else if (signature is null or { Length: 0 })
        {
            throw new ArgumentException("签名数据不能为空。");
        }
        else if (this.K is null or { Length: 0 })
        {
            throw new ArgumentException("缺少密钥属性‘K’。");
        }

        var key = new KeyParameter(this.K);
        var signer = BCSignerFactory.GetSigner(this.Algorithm);
        signer.Init(forSigning: false, key);
        signer.BlockUpdate(awaitSignData);
        var result = signer.VerifySignature(signature);

        return result;
    }

    /// <inheritdoc/>
    protected override string GetAlgorithmNameWithNoAlgParameter()
        // 无法推断，返回空字符串
        => "";
}
