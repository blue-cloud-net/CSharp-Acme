using Acme.Protocol.BouncyCastle;
using Acme.Protocol.Crypto;
using Acme.Protocol.Resources;

namespace Acme.Protocol.Jwk;

/// <summary>
/// 表示使用 RSA 算法的 JSON Web Key (JWK) 实现
/// </summary>
/// <remarks>
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-6.3"/>
/// RFC 7518 定义了 RSA 密钥在 JWK 中的参数和编码方式
/// </remarks>
public class RsaJsonWebKey : JsonWebKey
{
    [JsonConstructor]
    internal RsaJsonWebKey()
    {
    }

    /// <summary>
    /// 获取或设置 'n'（RSA 模数）
    /// </summary>
    /// <remarks>
    /// RSA 公钥的模数，参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-6.3.1.1"/>
    /// </remarks>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.N)]
    [JsonPropertyOrder(3)]
    public byte[] Modulus { get; protected set; } = [];

    /// <summary>
    /// 获取或设置 'e'（RSA 公钥指数）
    /// </summary>
    /// <remarks>
    /// RSA 公钥的指数，参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-6.3.1.2"/>
    /// </remarks>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.E)]
    [JsonPropertyOrder(4)]
    public byte[] Exponent { get; protected set; } = [];

    /// <summary>
    /// 获取或设置 'd'（RSA 私钥指数）
    /// </summary>
    /// <remarks>
    /// RSA 私钥的私有指数，参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-6.3.2.1"/>
    /// </remarks>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.D)]
    [JsonPropertyOrder(5)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public byte[]? D { get; protected set; }

    /// <summary>
    /// 获取或设置 'p'（RSA 第一个素数因子）
    /// </summary>
    /// <remarks>
    /// RSA 私钥的第一个素数因子，参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-6.3.2.2"/>
    /// </remarks>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.P)]
    [JsonPropertyOrder(6)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public byte[]? P { get; protected set; }

    /// <summary>
    /// 获取或设置 'q'（RSA 第二个素数因子）
    /// </summary>
    /// <remarks>
    /// RSA 私钥的第二个素数因子，值格式为 Base64url 编码的整数
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-6.3.2.3"/>
    /// </remarks>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.Q)]
    [JsonPropertyOrder(7)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public byte[]? Q { get; protected set; }

    /// <summary>
    /// 获取或设置 'dp'（RSA 第一个 CRT 指数）
    /// </summary>
    /// <remarks>
    /// RSA 私钥的第一个中国剩余定理 (CRT) 指数，值格式为 Base64url 编码的整数
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-6.3.2.4"/>
    /// </remarks>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.DP)]
    [JsonPropertyOrder(8)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public byte[]? DP { get; protected set; }

    /// <summary>
    /// 获取或设置 'dq'（RSA 第二个 CRT 指数）
    /// </summary>
    /// <remarks>
    /// RSA 私钥的第二个中国剩余定理 (CRT) 指数，值格式为 Base64url 编码的整数
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-6.3.2.5"/>
    /// </remarks>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.DQ)]
    [JsonPropertyOrder(9)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public byte[]? DQ { get; protected set; }

    /// <summary>
    /// 获取或设置 'qi'（RSA 第一个 CRT 系数）
    /// </summary>
    /// <remarks>
    /// RSA 私钥的第一个中国剩余定理 (CRT) 系数，值格式为 Base64url 编码的整数
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-6.3.2.6"/>
    /// </remarks>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.QI)]
    [JsonPropertyOrder(10)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public byte[]? QI { get; protected set; }

    /// <summary>
    /// 获取 RSA 密钥的位数（单位：位）
    /// </summary>
    /// <remarks>
    /// 根据模数长度计算，公式：模数字节长度 × 8
    /// </remarks>
    [JsonIgnore]
    public override int KeySize => this.Modulus.Length * 8;

    /// <summary>
    /// 计算 JWK 指纹（Thumbprint）
    /// </summary>
    /// <param name="digest">用于计算指纹的摘要算法，默认为 SHA-256</param>
    /// <returns>JWK 指纹的摘要值</returns>
    /// <remarks>
    /// 根据 RFC 7638 规范，JWK 指纹由 Base64url 编码的公钥参数（E、KTY、N）按字典序排列后的 JSON 对象计算
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7638"/>
    /// </remarks>
    /// <exception cref="ArgumentException">缺少必要的密钥参数时抛出</exception>
    public override byte[] ComputeThumbprint(IDigest? digest = null)
    {
        if (string.IsNullOrWhiteSpace(this.KeyType))
            throw new ArgumentException(RS.RsaComputeThumbprintMissingKty);
        if (this.Modulus is null or { Length: 0 })
            throw new ArgumentException(RS.RsaComputeThumbprintMissingN);
        if (this.Exponent is null or { Length: 0 })
            throw new ArgumentException(RS.RsaComputeThumbprintMissingE);

        digest ??= new Sha256Digest();

        var canonicalJwk =
            $"{{\"{JsonWebKeyParameterNames.E}\":\"{Base64UrlEncoder.Encode(this.Exponent)}\"," +
            $"\"{JsonWebKeyParameterNames.Kty}\":\"{this.KeyType}\"," +
            $"\"{JsonWebKeyParameterNames.N}\":\"{Base64UrlEncoder.Encode(this.Modulus)}\"}}";
        var hash = digest.ComputeHash(Encoding.UTF8.GetBytes(canonicalJwk));

        return hash;
    }

    /// <inheritdoc/>
    public override byte[] GenerateSignature(byte[] awaitSignData)
    {
        if (awaitSignData is null or { Length: 0 })
            throw new ArgumentException(RS.RsaSignDataCannotBeEmpty);
        if (this.Modulus is null or { Length: 0 })
            throw new ArgumentException(RS.RsaSignMissingN);
        if (this.Exponent is null or { Length: 0 })
            throw new ArgumentException(RS.RsaSignMissingE);
        if (this.D is null or { Length: 0 })
            throw new ArgumentException(RS.RsaSignMissingD);
        if (this.P is null or { Length: 0 })
            throw new ArgumentException(RS.RsaSignMissingP);
        if (this.Q is null or { Length: 0 })
            throw new ArgumentException(RS.RsaSignMissingQ);
        if (this.DP is null or { Length: 0 })
            throw new ArgumentException(RS.RsaSignMissingDp);
        if (this.DQ is null or { Length: 0 })
            throw new ArgumentException(RS.RsaSignMissingDq);
        if (this.QI is null or { Length: 0 })
            throw new ArgumentException(RS.RsaSignMissingQi);

        var privateKey = new RsaPrivateCrtKeyParameters(
            new(1, this.Modulus),
            new(1, this.Exponent),
            new(1, this.D),
            new(1, this.P),
            new(1, this.Q),
            new(1, this.DP),
            new(1, this.DQ),
            new(1, this.QI));

        var signer = BcSignerFactory.GetSigner(this.Algorithm);
        var signature = signer.GenerateSignature(privateKey, awaitSignData);

        return signature;
    }

    /// <summary>
    /// 使用 RSA 公钥验证签名
    /// </summary>
    /// <param name="awaitSignData">原始数据</param>
    /// <param name="signature">签名数据</param>
    /// <returns>签名是否有效</returns>
    /// <remarks>
    /// 使用公钥验证数据签名，验证算法由 Algorithm 属性决定（如 RS256、RS384、RS512）
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-3.5"/>
    /// </remarks>
    /// <exception cref="ArgumentException">缺少必要的数据或密钥参数时抛出</exception>
    public override bool VerifySignature(byte[] awaitSignData, byte[] signature)
    {
        if (awaitSignData is null or { Length: 0 })
            throw new ArgumentException(RS.RsaVerifyDataCannotBeEmpty);
        if (signature is null or { Length: 0 })
            throw new ArgumentException(RS.RsaVerifySignatureCannotBeEmpty);
        if (this.Modulus is null or { Length: 0 })
            throw new ArgumentException(RS.RsaVerifyMissingN);
        if (this.Exponent is null or { Length: 0 })
            throw new ArgumentException(RS.RsaVerifyMissingE);

        var publicKey = new RsaKeyParameters(
            false,
            new(1, this.Modulus),
            new(1, this.Exponent));

        var signer = BcSignerFactory.GetSigner(this.Algorithm);
        var result = signer.VerifySignature(publicKey, awaitSignData, signature);

        return result;
    }

    /// <summary>
    /// 导出仅包含公钥的 JWK 副本<see cref="JsonWebKey"/>
    /// </summary>
    /// <returns>仅包含公钥部分的 RSA JWK，不包含私钥参数</returns>
    /// <remarks>
    /// 将本 JWK 的公钥参数（kty、alg、n、e）复制到新的 JWK 实例，不包含私钥参数（d、p、q、dp、dq、qi）
    /// </remarks>
    public override JsonWebKey ExportPublicKey()
        => new RsaJsonWebKey()
        {
            KeyType = this.KeyType,
            Algorithm = this.Algorithm,
            Modulus = this.Modulus,
            Exponent = this.Exponent
        };

    private static SecureRandom _secureRandom = new();

    /// <summary>
    /// 创建 RSA 算法的 <see cref="JsonWebKey"/>
    /// </summary>
    /// <param name="keySize">密钥长度（必须 >= 2048，参见 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-3.3"/>）</param>
    /// <param name="hashSize">哈希长度，允许 256/384/512</param>
    /// <returns>包含公私钥的 RSA JWK</returns>
    /// <exception cref="NotSupportedException">当密钥长度或哈希长度不受支持时抛出</exception>
    public static RsaJsonWebKey Create(int keySize = 2048, int hashSize = 256)
    {
        if (keySize < 2048)
            throw new NotSupportedException("密钥长度必须大于或等于 2048 位。");

        if (hashSize is not 256 and not 384 and not 512)
            throw new NotSupportedException("哈希长度必须是 256、384 或 512。");

        var generator = new RsaKeyPairGenerator();
        var generatorParameters = new KeyGenerationParameters(_secureRandom, keySize);
        generator.Init(generatorParameters);
        var keyPair = generator.GenerateKeyPair();
        var privateKey = (RsaPrivateCrtKeyParameters)keyPair.Private;

        return new RsaJsonWebKey()
        {
            KeyType = JsonWebAlgorithmsKeyTypes.RSA,
            Algorithm = $"RS{hashSize}",
            Modulus = privateKey.Modulus.ToByteArrayUnsigned(),
            Exponent = privateKey.PublicExponent.ToByteArrayUnsigned(),
            D = privateKey.Exponent.ToByteArrayUnsigned(),
            P = privateKey.P.ToByteArrayUnsigned(),
            Q = privateKey.Q.ToByteArrayUnsigned(),
            DP = privateKey.DP.ToByteArrayUnsigned(),
            DQ = privateKey.DQ.ToByteArrayUnsigned(),
            QI = privateKey.QInv.ToByteArrayUnsigned(),
        };
    }

    /// <inheritdoc/>
    protected override string GetAlgorithmNameWithNoAlgParameter()
        // 无法从 RSA 密钥参数推断算法名称，必须通过 Algorithm 属性显式设置
        => "";
}
