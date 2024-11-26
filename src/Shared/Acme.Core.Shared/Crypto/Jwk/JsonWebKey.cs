using Acme.Json;

using Volo.Abp.Json;

namespace Acme.Crypto.Jwk;

/// <summary>
/// Json Web Key
/// <see href="https://www.rfc-editor.org/rfc/rfc7515"/>
/// </summary>
[JsonDerivedType(typeof(EcJsonWebKey))]
[JsonDerivedType(typeof(RsaJsonWebKey))]
public abstract class JsonWebKey : IKey
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
        => this.Algorithm.IsNullOrWhiteSpace()
        ? throw new ArgumentException($"缺少算法类型属性‘{JsonWebKeyParameterNames.Alg}’")
        : Int32.Parse(this.Algorithm[2..]);

    /// <inheritdoc/>
    public abstract JsonWebKey ExportPublicKey();

    /// <summary>
    /// 计算指纹
    /// <see href="https://www.rfc-editor.org/rfc/rfc7638#section-3.1"/>
    /// </summary>
    /// <returns></returns>
    public abstract byte[] ComputeThumbprint();

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
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, JsonSerializerUtil.DefaultOptions);
    }

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

        var keyType = jsonDocument.RootElement
            .EnumerateObject()
            .FirstOrDefault(p => String.Equals(p.Name, JsonWebKeyParameterNames.Kty, StringComparison.OrdinalIgnoreCase))
            .Value.GetString();

        JsonWebKey jwk = keyType switch
        {
            JsonWebAlgorithmsKeyTypes.RSA =>
                jsonSerializer is not null
                ? jsonSerializer.Deserialize<RsaJsonWebKey>(jwkString)
                : (JsonSerializer.Deserialize<RsaJsonWebKey>(jwkString, JsonSerializerUtil.DefaultOptions)
                ?? throw new Exception("Jwk解析失败。")),
            JsonWebAlgorithmsKeyTypes.EllipticCurve =>
                jsonSerializer is not null
                ? jsonSerializer.Deserialize<EcJsonWebKey>(jwkString)
                : (JsonSerializer.Deserialize<EcJsonWebKey>(jwkString, JsonSerializerUtil.DefaultOptions)
                ?? throw new Exception("Jwk解析失败。")),
            JsonWebAlgorithmsKeyTypes.Octet =>
                jsonSerializer is not null
                ? jsonSerializer.Deserialize<OctJsonWebKey>(jwkString)
                : (JsonSerializer.Deserialize<OctJsonWebKey>(jwkString, JsonSerializerUtil.DefaultOptions)
                ?? throw new Exception("Jwk解析失败。")),
            _ => throw new NotSupportedException($"不受支持Jwk的密钥类型‘{keyType}’。")
        };

        if (jwk.Algorithm.IsNullOrWhiteSpace())
        {
            jwk.Algorithm = jwk.GetAlgorithmNameWithNoAlgParameter();
        }
        else if (!JsonWebKeyAlgorithms.IsSupported(jwk.Algorithm))
        {
            throw new NotSupportedException($"不受支持Jwk的算法‘{jwk.Algorithm}’。");
        }

        return jwk;
    }
}
