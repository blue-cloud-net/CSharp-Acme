using Acme.Protocol.BouncyCastle;
using Acme.Protocol.Crypto;
using Acme.Protocol.Resources;

namespace Acme.Protocol.Jwk;

/// <summary>
/// 表示使用椭圆曲线算法（ECDSA）的 JSON Web Key (JWK) 实现
/// </summary>
/// <remarks>
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-6.2"/>
/// RFC 7518 定义了椭圆曲线密钥在 JWK 中的参数和编码方式
/// </remarks>
public class EcJsonWebKey : JsonWebKey
{
    [JsonConstructor]
    internal EcJsonWebKey()
    {
    }

    /// <summary>
    /// 获取或设置椭圆曲线标识符
    /// </summary>
    /// <remarks>
    /// 标识使用的椭圆曲线，如 "P-256"、"P-384"、"P-521" 等，
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-6.2.1.1"/>
    /// </remarks>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.Crv)]
    [JsonPropertyOrder(3)]
    public string Curve { get; protected set; } = String.Empty;

    /// <summary>
    /// 获取或设置公钥点的 X 坐标
    /// </summary>
    /// <remarks>
    /// 椭圆曲线公钥点的 X 坐标，参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-6.2.1.2"/>
    /// </remarks>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.X)]
    [JsonPropertyOrder(4)]
    public byte[] X { get; protected set; } = [];

    /// <summary>
    /// 获取或设置公钥点的 Y 坐标
    /// </summary>
    /// <remarks>
    /// 椭圆曲线公钥点的 Y 坐标，参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-6.2.1.3"/>
    /// </remarks>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.Y)]
    [JsonPropertyOrder(5)]
    public byte[] Y { get; protected set; } = [];

    /// <summary>
    /// 获取或设置私钥标量值
    /// </summary>
    /// <remarks>
    /// 椭圆曲线私钥的标量值 (Private Value)，仅在包含私钥的 JWK 中存在，
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-6.2.2"/>
    /// </remarks>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.D)]
    [JsonPropertyOrder(6)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public byte[]? D { get; protected set; }

    /// <summary>
    /// 获取椭圆曲线密钥的位数（单位：位）
    /// </summary>
    /// <remarks>
    /// 根据 X 坐标长度计算，公式：X 坐标字节长度 × 8
    /// </remarks>
    [JsonIgnore]
    public override int KeySize => this.X.Length * 8;

    /// <summary>
    /// 计算 JWK 指纹（Thumbprint）
    /// </summary>
    /// <param name="digest">用于计算指纹的摘要算法，默认为 SHA-256</param>
    /// <returns>JWK 指纹的摘要值</returns>
    /// <remarks>
    /// 根据 RFC 7638 规范，EC JWK 指纹由 Base64url 编码的公钥参数（Crv、Kty、X、Y）按字典序排列后的 JSON 对象计算
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7638"/>
    /// </remarks>
    /// <exception cref="ArgumentException">缺少必要的密钥参数时抛出</exception>
    public override byte[] ComputeThumbprint(IDigest? digest = null)
    {
        if (string.IsNullOrWhiteSpace(this.KeyType))
            throw new ArgumentException(RS.EcComputeThumbprintMissingKty);
        if (string.IsNullOrWhiteSpace(this.Curve))
            throw new ArgumentException(RS.EcComputeThumbprintMissingCrv);
        if (this.X is null or { Length: 0 })
            throw new ArgumentException(RS.EcComputeThumbprintMissingX);
        if (this.Y is null or { Length: 0 })
            throw new ArgumentException(RS.EcComputeThumbprintMissingY);

        digest ??= new Sha256Digest();

        var canonicalJwk =
            $"{{\"{JsonWebKeyParameterNames.Crv}\":\"{this.Curve}\"," +
            $"\"{JsonWebKeyParameterNames.Kty}\":\"{this.KeyType}\"," +
            $"\"{JsonWebKeyParameterNames.X}\":\"{Base64UrlEncoder.Encode(this.X)}\"," +
            $"\"{JsonWebKeyParameterNames.Y}\":\"{Base64UrlEncoder.Encode(this.Y)}\"}}";
        var hash = digest.ComputeHash(Encoding.UTF8.GetBytes(canonicalJwk));

        return hash;
    }

    /// <summary>
    /// 导出仅包含公钥的 JWK 副本
    /// </summary>
    /// <returns>仅包含公钥部分的 EC JWK，不包含私钥参数</returns>
    /// <remarks>
    /// 将本 JWK 的公钥参数（kty、alg、crv、x、y）复制到新的 JWK 实例，不包含私钥参数（d）
    /// </remarks>
    public override JsonWebKey ExportPublicKey()
        => new EcJsonWebKey()
        {
            KeyType = this.KeyType,
            Algorithm = this.Algorithm,
            Curve = this.Curve,
            X = this.X,
            Y = this.Y
        };

    private const string _algorithm = "ECDSA";

    /// <summary>
    /// 使用椭圆曲线私钥生成签名
    /// </summary>
    /// <param name="awaitSignData">待签名的数据</param>
    /// <returns>签名数据</returns>
    /// <remarks>
    /// 使用 ECDSA 私钥对数据进行签名，签名算法由 Algorithm 属性决定（如 ES256、ES384、ES512）
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-3.4"/>
    /// </remarks>
    /// <exception cref="ArgumentException">待签名数据或私钥为空时抛出</exception>
    public override byte[] GenerateSignature(byte[] awaitSignData)
    {
        if (awaitSignData is null or { Length: 0 })
            throw new ArgumentException(RS.EcSignDataCannotBeEmpty);
        if (this.D is null or { Length: 0 })
            throw new ArgumentException(RS.EcSignMissingD);

        var curveOid = NistNamedCurves.GetOid(this.Curve);
        var privateKey = new ECPrivateKeyParameters(_algorithm, new BigInteger(1, this.D), curveOid);

        var signer = BcSignerFactory.GetSigner(this.Algorithm);
        var signature = signer.GenerateSignature(privateKey, awaitSignData);

        return signature;
    }

    /// <summary>
    /// 使用椭圆曲线公钥验证签名
    /// </summary>
    /// <param name="awaitSignData">原始数据</param>
    /// <param name="signature">签名数据</param>
    /// <returns>签名是否有效</returns>
    /// <remarks>
    /// 使用 ECDSA 公钥验证数据签名，验证算法由 Algorithm 属性决定（如 ES256、ES384、ES512）
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-3.4"/>
    /// </remarks>
    /// <exception cref="ArgumentException">缺少必要的数据或密钥参数时抛出</exception>
    public override bool VerifySignature(byte[] awaitSignData, byte[] signature)
    {
        if (awaitSignData is null or { Length: 0 })
            throw new ArgumentException(RS.EcVerifyDataCannotBeEmpty);
        if (signature is null or { Length: 0 })
            throw new ArgumentException(RS.EcVerifySignatureCannotBeEmpty);
        if (this.X is null or { Length: 0 })
            throw new ArgumentException(RS.EcVerifyMissingX);
        if (this.Y is null or { Length: 0 })
            throw new ArgumentException(RS.EcVerifyMissingY);

        var curve = NistNamedCurves.GetByName(this.Curve);
        var curveOid = NistNamedCurves.GetOid(this.Curve);
        var x = new BigInteger(1, this.X);
        var y = new BigInteger(1, this.Y);
        var point = curve.Curve.CreatePoint(x, y);
        var publicKey = new ECPublicKeyParameters(_algorithm, point, curveOid);

        var signer = BcSignerFactory.GetSigner(this.Algorithm);
        var result = signer.VerifySignature(publicKey, awaitSignData, signature);

        return result;
    }

    private static SecureRandom _secureRandom = new();

    /// <summary>
    /// 生成新的 EC JWK 密钥对
    /// </summary>
    /// <param name="curve">椭圆曲线标识符，支持 "P-256"、"P-384"、"P-521"，默认为 "P-521"</param>
    /// <returns>包含公私钥的 EC JWK 实例</returns>
    /// <remarks>
    /// 生成指定椭圆曲线的密钥对，并自动设置相应的签名算法（ES256、ES384 或 ES512）
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-6.2"/>
    /// </remarks>
    /// <exception cref="ArgumentException">指定的曲线不受支持时抛出</exception>
    public static EcJsonWebKey Create(string? curve = null)
    {
        if (string.IsNullOrWhiteSpace(curve))
            curve = "P-256";

        var curveOid = NistNamedCurves.GetOid(curve)
            ?? throw new ArgumentException($"生成 EC 密钥失败：不支持的曲线标识符 '{curve}'。");

        var generator = new ECKeyPairGenerator(_algorithm);
        var generatorParameters = new ECKeyGenerationParameters(curveOid, _secureRandom);
        generator.Init(generatorParameters);
        var keyPair = generator.GenerateKeyPair();
        var privateKey = (ECPrivateKeyParameters)keyPair.Private;
        var publicKey = (ECPublicKeyParameters)keyPair.Public;

        return new EcJsonWebKey
        {
            KeyType = JsonWebAlgorithmsKeyTypes.EllipticCurve,
            Algorithm = GetAlgorithmNameFromCurveName(curve!),
            Curve = curve!,
            X = publicKey.Q.AffineXCoord.GetEncoded(),
            Y = publicKey.Q.AffineYCoord.GetEncoded(),
            D = privateKey.D.ToByteArrayUnsigned()
        };
    }

    /// <inheritdoc/>
    protected override string GetAlgorithmNameWithNoAlgParameter() => GetAlgorithmNameFromCurveName(this.Curve);

    /// <summary>
    /// 根据椭圆曲线名称获取对应的签名算法名称
    /// </summary>
    /// <param name="curveName">椭圆曲线标识符（"P-256"、"P-384" 或 "P-521"）</param>
    /// <returns>对应的签名算法名称（"ES256"、"ES384" 或 "ES512"）</returns>
    /// <remarks>
    /// P-256 对应 ES256（SHA-256 with ECDSA）
    /// P-384 对应 ES384（SHA-384 with ECDSA）
    /// P-521 对应 ES512（SHA-512 with ECDSA）
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-3.4"/>
    /// </remarks>
    /// <exception cref="NotSupportedException">不支持的曲线时抛出</exception>
    private static string GetAlgorithmNameFromCurveName(string curveName)
    {
        return NistNamedCurves.GetByName(curveName).Curve.FieldSize switch
        {
            256 => "ES256",
            384 => "ES384",
            521 => "ES512",
            _ => throw new NotSupportedException($"不支持的曲线 '{curveName}'。")
        };
    }

    /// <inheritdoc/>
    public override void SetAlgorithm(string algorithm)
    {
        // ECDSA算法不需要设置算法, 从曲线名称获取算法名称
        return;
    }
}
