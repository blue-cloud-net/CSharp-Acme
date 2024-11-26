using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Acme.Crypto.Jwk;

/// <summary>
/// Represents a JWK using Elliptic Curve.
/// </summary>
public class EcJsonWebKey : JsonWebKey
{
    [JsonConstructor]
    internal EcJsonWebKey()
    {
    }

    /// <summary>
    /// Gets or sets the 'd' (ECC - Private Key).
    /// </summary>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.Crv)]
    [JsonPropertyOrder(3)]
    public string Curve { get; protected set; } = String.Empty;

    /// <summary>
    /// Gets or sets the x coordinate of the key.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.X)]
    [JsonPropertyOrder(4)]
    public byte[] X { get; protected set; } = [];

    /// <summary>
    /// Gets or sets the y coordinate of the key.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.Y)]
    [JsonPropertyOrder(5)]
    public byte[] Y { get; protected set; } = [];

    /// <summary>
    /// Gets or sets the y coordinate of the key.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.D)]
    [JsonPropertyOrder(6)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault)]
    public byte[]? D { get; protected set; }

    /// <summary>
    /// 密钥长度
    /// </summary>
    [JsonIgnore]
    public override int KeySize => this.X.Length * 8;

    /// <inheritdoc/>
    public override byte[] ComputeThumbprint()
    {
        if (this.Curve.IsNullOrWhiteSpace())
        {
            throw new ArgumentException("");
        }

        if (this.X is null or { Length: 0 } || this.Y is null or { Length: 0 })
        {
            throw new ArgumentException("缺少公钥属性‘X’,‘Y’。");
        }

        var canonicalJwk =
            $"{{\"{JsonWebKeyParameterNames.Crv}\":\"{this.Curve}\"," +
            $"\"{JsonWebKeyParameterNames.Kty}\":\"{this.KeyType}\"," +
            $"\"{JsonWebKeyParameterNames.X}\":\"{Base64UrlEncoder.Encode(this.X)}\"," +
            $"\"{JsonWebKeyParameterNames.Y}\":\"{Base64UrlEncoder.Encode(this.Y)}\"}}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(canonicalJwk));

        return hash;
    }

    /// <summary>
    /// 导出公钥
    /// </summary>
    /// <returns></returns>
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

    /// <inheritdoc/>
    public override byte[] GenerateSignature(byte[] awaitSignData)
    {
        if (awaitSignData is null or { Length: 0 })
        {
            throw new ArgumentException("待签名内容不可为空。");
        }
        else if (this.D is null or { Length: 0 })
        {
            throw new ArgumentException("缺少私钥属性‘D’。");
        }

        var curveOid = NistNamedCurves.GetOid(this.Curve);
        var privateKey = new ECPrivateKeyParameters(_algorithm, new BigInteger(1, this.D), curveOid);

        var signer = BCSignerFactory.GetSigner(this.Algorithm);
        signer.Init(true, privateKey);
        signer.BlockUpdate(awaitSignData);
        var signature = signer.GenerateSignature();

        return signature;
    }

    /// <inheritdoc/>
    public override bool VerifySignature(byte[] awaitSignData, byte[] signature)
    {
        if (awaitSignData is null or { Length: 0 })
        {
            throw new ArgumentException("待验签内容不可为空。");
        }
        else if (signature is null or { Length: 0 })
        {
            throw new ArgumentException("签名不可为空。");
        }
        else if (this.X is null or { Length: 0 } || this.Y is null or { Length: 0 })
        {
            throw new ArgumentException("缺少公钥属性‘X’,‘Y’。");
        }

        var curve = NistNamedCurves.GetByName(this.Curve);
        var curveOid = NistNamedCurves.GetOid(this.Curve);
        var x = new BigInteger(1, this.X);
        var y = new BigInteger(1, this.Y);
        var point = curve.Curve.CreatePoint(x, y);
        var publicKey = new ECPublicKeyParameters(_algorithm, point, curveOid);

        var signer = BCSignerFactory.GetSigner(this.Algorithm);
        signer.Init(false, publicKey);
        signer.BlockUpdate(awaitSignData);
        var result = signer.VerifySignature(signature);

        return result;
    }

    private static SecureRandom _secureRandom = new();

    /// <summary>
    /// 创建EC算法的<see cref="JsonWebKey"/>
    /// </summary>
    /// <param name="curve"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public static EcJsonWebKey Create(string? curve = null)
    {
        if (curve.IsNullOrWhiteSpace())
        {
            curve = "P-521";
        }

        var curveOid = NistNamedCurves.GetOid(curve);
        if (curveOid is null)
        {
            throw new ArgumentException($"不存在curve别名:{curve}");
        }

        var generator = new ECKeyPairGenerator(_algorithm);
        var generatorParameters = new ECKeyGenerationParameters(curveOid, _secureRandom);
        generator.Init(generatorParameters);
        var keyPair = generator.GenerateKeyPair();
        var privateKey = (ECPrivateKeyParameters)keyPair.Private;
        var publicKey = (ECPublicKeyParameters)keyPair.Public;

        return new EcJsonWebKey
        {
            KeyType = JsonWebAlgorithmsKeyTypes.EllipticCurve,
            Algorithm = GetAlgorithmNameFromCurveName(curve),
            Curve = curve,
            X = publicKey.Q.AffineXCoord.GetEncoded(),
            Y = publicKey.Q.AffineYCoord.GetEncoded(),
            D = privateKey.D.ToByteArrayUnsigned()
        };
    }

    /// <inheritdoc/>
    protected override string GetAlgorithmNameWithNoAlgParameter() => GetAlgorithmNameFromCurveName(this.Curve);

    /// <summary>
    /// 从曲线获取算法名称
    /// </summary>
    /// <param name="curveName"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    private static string GetAlgorithmNameFromCurveName(string curveName)
    {
        return NistNamedCurves.GetByName(curveName).Curve.FieldSize switch
        {
            256 => "ES256",
            384 => "ES384",
            521 => "ES512",
            _ => throw new NotSupportedException(curveName)
        };
    }

    /// <inheritdoc/>
    public override void SetAlgorithm(string algorithm)
    {
        // ECDSA算法不需要设置算法, 从曲线名称获取算法名称
        return;
    }
}
