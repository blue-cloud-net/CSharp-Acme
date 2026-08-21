using Acme.Protocol.BouncyCastle;
using Acme.Protocol.Crypto;
using Acme.Protocol.Resources;

namespace Acme.Protocol.Jwk;

/// <summary>
/// 表示使用对称密钥（八位字符序列）的 JSON Web Key (JWK) 实现
/// </summary>
/// <remarks>
/// 对称密钥用于 HMAC 和其他对称加密算法
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-6.4"/>
/// </remarks>
public class OctJsonWebKey : JsonWebKey
{
    /// <summary>
    /// 获取或设置对称密钥值
    /// </summary>
    /// <remarks>
    /// 对称密钥的密钥值，Base64url 编码，参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-6.4.1"/>
    /// </remarks>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.K)]
    [JsonPropertyOrder(3)]
    public byte[] K { get; protected set; } = [];

    /// <summary>
    /// 计算 JWK 指纹（Thumbprint）
    /// </summary>
    /// <param name="digest">用于计算指纹的摘要算法，默认为 SHA-256</param>
    /// <returns>JWK 指纹的摘要值</returns>
    /// <remarks>
    /// 根据 RFC 7638 规范，对称 JWK 指纹由 Base64url 编码的参数（K、Kty）按字典序排列后的 JSON 对象计算
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7638"/>
    /// </remarks>
    /// <exception cref="ArgumentException">缺少密钥参数时抛出</exception>
    public override byte[] ComputeThumbprint(IDigest? digest = null)
    {
        if (string.IsNullOrWhiteSpace(this.KeyType))
            throw new ArgumentException(RS.ComputeThumbprintMissingKty);
        if (this.K is null or { Length: 0 })
            throw new ArgumentException(RS.ComputeThumbprintMissingK);

        digest ??= new Sha256Digest();

        var canonicalJwk =
            $"{{\"{JsonWebKeyParameterNames.K}\":\"{this.K}\"," +
            $"\"{JsonWebKeyParameterNames.Kty}\":\"{this.KeyType}\"}}";
        var hash = digest.ComputeHash(Encoding.UTF8.GetBytes(canonicalJwk));

        return hash;
    }

    /// <summary>
    /// 导出公钥（对于对称密钥，返回密钥本身的副本）
    /// </summary>
    /// <returns>对称密钥副本</returns>
    /// <remarks>
    /// 对于对称密钥，"公钥" 实际上等同于密钥本身，因为对称密钥不区分公钥和私钥
    /// </remarks>
    public override JsonWebKey ExportPublicKey()
        => new OctJsonWebKey()
        {
            KeyType = this.KeyType,
            Algorithm = this.Algorithm,
            K = this.K
        };

    /// <summary>
    /// 使用对称密钥生成 HMAC 签名
    /// </summary>
    /// <param name="awaitSignData">待签名的数据</param>
    /// <returns>HMAC 签名数据</returns>
    /// <remarks>
    /// 使用对称密钥对数据进行 HMAC 签名，算法由 Algorithm 属性决定（如 HS256、HS384、HS512）
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-3.2"/>
    /// </remarks>
    /// <exception cref="ArgumentException">待签名数据或密钥为空时抛出</exception>
    public override byte[] GenerateSignature(byte[] awaitSignData)
    {
        if (awaitSignData is null or { Length: 0 })
            throw new ArgumentException(RS.SignDataCannotBeEmpty);
        else if (this.K is null or { Length: 0 })
            throw new ArgumentException(RS.SignMissingK);

        var key = new KeyParameter(this.K);
        var signer = BcSignerFactory.GetSigner(this.Algorithm);
        var signature = signer.GenerateSignature(key, awaitSignData);

        return signature;
    }

    /// <summary>
    /// 使用对称密钥验证 HMAC 签名
    /// </summary>
    /// <param name="awaitSignData">原始数据</param>
    /// <param name="signature">HMAC 签名数据</param>
    /// <returns>签名是否有效</returns>
    /// <remarks>
    /// 使用对称密钥验证数据的 HMAC 签名，算法由 Algorithm 属性决定（如 HS256、HS384、HS512）
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-3.2"/>
    /// </remarks>
    /// <exception cref="ArgumentException">缺少必要的数据或密钥参数时抛出</exception>
    public override bool VerifySignature(byte[] awaitSignData, byte[] signature)
    {
        if (awaitSignData is null or { Length: 0 })
            throw new ArgumentException(RS.VerifyDataCannotBeEmpty);
        if (signature is null or { Length: 0 })
            throw new ArgumentException(RS.VerifySignatureCannotBeEmpty);
        if (this.K is null or { Length: 0 })
            throw new ArgumentException(RS.VerifyMissingK);

        var key = new KeyParameter(this.K);
        var signer = BcSignerFactory.GetSigner(this.Algorithm);
        var result = signer.VerifySignature(key, awaitSignData, signature);

        return result;
    }

    /// <inheritdoc/>
    protected override string GetAlgorithmNameWithNoAlgParameter()
        // 无法从对称密钥参数推断算法名称，必须通过 Algorithm 属性显式设置
        => "";
}
