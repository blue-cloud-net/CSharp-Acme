using Acme.Protocol.Json;
using Acme.Protocol.Jws;

namespace Acme.Protocol.Jwk;

/// <summary>
/// Json Web Key
/// <see href="https://www.rfc-editor.org/rfc/rfc7515"/>
/// </summary>
[JsonDerivedType(typeof(EcJsonWebKey))]
[JsonDerivedType(typeof(RsaJsonWebKey))]
public abstract class JsonWebKey : IJsonWebKey
{
    /// <summary>
    /// 密钥类型
    /// Gets or sets the 'alg' (KeyType).
    /// EC;RSA;oct
    /// </summary>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.Kty)]
    [JsonPropertyOrder(1)]
    public string KeyType { get; protected set; } = String.Empty;

    /// <summary>
    /// 算法
    /// </summary>
    [JsonInclude]
    [JsonPropertyName(JsonWebKeyParameterNames.Alg)]
    [JsonPropertyOrder(2)]
    public string Algorithm { get; protected set; } = String.Empty;

    /// <summary>
    /// 密钥大小
    /// </summary>
    [JsonIgnore]
    public virtual int KeySize { get; }

    /// <summary>
    /// 哈希长度
    /// </summary>
    [JsonIgnore]
    public virtual int HashSize
        => string.IsNullOrWhiteSpace(this.Algorithm)
            ? throw new ArgumentException($"缺少算法类型属性‘{JsonWebKeyParameterNames.Alg}’。")
            : Int32.Parse(this.Algorithm.Substring(2));

    /// <inheritdoc/>
    public abstract JsonWebKey ExportPublicKey();

    /// <inheritdoc/>
    public abstract byte[] ComputeThumbprint(IDigest? digest = null);

    /// <inheritdoc/>
    public abstract byte[] GenerateSignature(byte[] awaitSignData);

    /// <inheritdoc/>
    public abstract bool VerifySignature(byte[] awaitSignData, byte[] signature);

    /// <summary>
    /// 获取算法名称
    /// </summary>
    /// <returns></returns>
    protected abstract string GetAlgorithmNameWithNoAlgParameter();

    /// <summary>
    /// 设置算法
    /// </summary>
    /// <param name="algorithm"></param>
    public virtual void SetAlgorithm(string algorithm) => this.Algorithm = algorithm;

    /// <inheritdoc/>
    public override string ToString() => JwsSystemTextJsonSerializer.Instance.Serialize(this);

    /// <summary>
    /// 解析Jwk字符串
    /// </summary>
    /// <param name="jwkString"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public static JsonWebKey Parse(string jwkString, IJsonSerializer? jsonSerializer = null)
    {
        var jsonDocument = JsonDocument.Parse(jwkString);

        var keyTypeObj = jsonDocument.RootElement
            .EnumerateObject()
            .FirstOrDefault(p =>
                String.Equals(p.Name, JsonWebKeyParameterNames.Kty, StringComparison.OrdinalIgnoreCase));
        if (string.IsNullOrWhiteSpace(keyTypeObj.Name))
            throw new Exception("Jwk解析失败：缺少密钥类型属性‘kty’。");
        var keyType = keyTypeObj.Value.GetString();

        jsonSerializer ??= JwsSystemTextJsonSerializer.Instance;

        JsonWebKey jwk = keyType switch
        {
            JsonWebAlgorithmsKeyTypes.RSA =>
                jsonSerializer.Deserialize<RsaJsonWebKey>(jwkString)
                    ?? throw new Exception("Jwk解析失败。"),
            JsonWebAlgorithmsKeyTypes.EllipticCurve =>
                jsonSerializer.Deserialize<EcJsonWebKey>(jwkString)
                    ?? throw new Exception("Jwk解析失败。"),
            JsonWebAlgorithmsKeyTypes.Octet =>
                jsonSerializer.Deserialize<OctJsonWebKey>(jwkString)
                    ?? throw new Exception("Jwk解析失败。"),
            _ => throw new NotSupportedException($"不受支持Jwk的密钥类型‘{keyType}’。"),
        };

        if (string.IsNullOrWhiteSpace(jwk.Algorithm))
            jwk.Algorithm = jwk.GetAlgorithmNameWithNoAlgParameter();

        if (!JsonWebKeyAlgorithms.IsSupported(jwk.Algorithm))
            throw new NotSupportedException($"不受支持Jwk的算法‘{jwk.Algorithm}’。");

        return jwk;
    }
}