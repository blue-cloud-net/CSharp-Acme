using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Acme.Crypto.Jwk;

/// <summary>
/// Represents a JWK using RSA.
/// </summary>
public class RsaJsonWebKey : JsonWebKey
{
    [JsonConstructor]
    internal RsaJsonWebKey()
    {
    }

    /// <summary>
    /// Gets or sets the 'n' (RSA - Modulus).
    /// </summary>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.N)]
    [JsonPropertyOrder(3)]
    public byte[] Modulus { get; protected set; } = [];

    /// <summary>
    /// Gets or sets the 'e' (RSA - Exponent).
    /// </summary>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.E)]
    [JsonPropertyOrder(4)]
    public byte[] Exponent { get; protected set; } = [];

    /// <summary>
    /// Gets or sets the 'd' (RSA - Private Exponent).
    /// </summary>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.D)]
    [JsonPropertyOrder(5)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault)]
    public byte[]? D { get; protected set; }

    /// <summary>
    /// Gets or sets the 'p' (RSA - First Prime Factor)..
    /// </summary>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.P)]
    [JsonPropertyOrder(6)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault)]
    public byte[]? P { get; protected set; }

    /// <summary>
    /// Gets or sets the 'q' (RSA - Second  Prime Factor)..
    /// </summary>
    /// <remarks>Value is formatted as: Base64urlUInt</remarks>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.Q)]
    [JsonPropertyOrder(7)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault)]
    public byte[]? Q { get; protected set; }

    /// <summary>
    /// Gets or sets the 'dp' (RSA - First Factor CRT Exponent).
    /// </summary>
    /// <remarks>Value is formated as: Base64urlUInt</remarks>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.DP)]
    [JsonPropertyOrder(8)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault)]
    public byte[]? DP { get; protected set; }

    /// <summary>
    /// Gets or sets the 'dq' (RSA - Second Factor CRT Exponent).
    /// </summary>
    /// <remarks>Value is formated as: Base64urlUInt</remarks>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.DQ)]
    [JsonPropertyOrder(9)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault)]
    public byte[]? DQ { get; protected set; }

    /// <summary>
    /// Gets or sets the 'qi' (RSA - First CRT Coefficient)..
    /// </summary>
    /// <remarks>Value is formated as: Base64urlUInt</remarks>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.QI)]
    [JsonPropertyOrder(10)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault)]
    public byte[]? QI { get; protected set; }

    /// <summary>
    /// 密钥长度
    /// </summary>
    [JsonIgnore]
    public override int KeySize => this.Modulus.Length * 8;

    /// <inheritdoc/>
    public override byte[] ComputeThumbprint()
    {
        var canonicalJwk =
            $"{{\"{JsonWebKeyParameterNames.E}\":\"{Base64UrlEncoder.Encode(this.Exponent)}\"," +
            $"\"{JsonWebKeyParameterNames.Kty}\":\"{this.KeyType}\"," +
            $"\"{JsonWebKeyParameterNames.N}\":\"{Base64UrlEncoder.Encode(this.Modulus)}\"}}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(canonicalJwk));

        return hash;
    }

    /// <inheritdoc/>
    public override byte[] GenerateSignature(byte[] awaitSignData)
    {
        if (awaitSignData is null or { Length: 0 })
        {
            throw new ArgumentException("待签名数据不能为空。");
        }

        var privateKey = new RsaPrivateCrtKeyParameters(
            new(1, this.Modulus),
            new(1, this.Exponent),
            new(1, this.D),
            new(1, this.P),
            new(1, this.Q),
            new(1, this.DP),
            new(1, this.DQ),
            new(1, this.QI));

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
            throw new ArgumentException("待签名数据不能为空。");
        }
        else if (signature is null or { Length: 0 })
        {
            throw new ArgumentException("签名数据不能为空。");
        }
        else if (this.Modulus is null or { Length: 0 })
        {
            throw new ArgumentException("缺少密钥属性‘N’。");
        }
        else if (this.Exponent is null or { Length: 0 })
        {
            throw new ArgumentException("缺少密钥属性‘E’。");
        }

        var publicKey = new RsaKeyParameters(false, new(1, this.Modulus), new(1, this.Exponent));

        var signer = BCSignerFactory.GetSigner(this.Algorithm);
        signer.Init(false, publicKey);
        signer.BlockUpdate(awaitSignData);
        var result = signer.VerifySignature(signature);

        return result;
    }

    /// <summary>
    /// 到处公钥<see cref="JsonWebKey"/>
    /// </summary>
    /// <returns></returns>
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
    /// 创建RSA算法的<see cref="JsonWebKey"/>
    /// </summary>
    /// <param name="keySize">密钥长度必须大于等于2048<see href="https://datatracker.ietf.org/doc/html/rfc7518#autoid-13"/></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static RsaJsonWebKey Create(int keySize = 2048, int hashSize = 256)
    {
        if (keySize < 2048)
        {
            throw new NotSupportedException("The key size must be greater than or equal to 2048.");
        }

        if (hashSize is not 256 and not 384 and not 512)
        {
            throw new NotSupportedException("The hash size must be 256, 384, or 512.");
        }

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
        // 无法推断，返回空字符串
        => "";
}
