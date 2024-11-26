using Acme.Json;

namespace Acme.Crypto;

/// <summary>
/// Acme的Jws的Protected部分
/// </summary>
public class AcmeJwsProtected
{
    /// <summary>
    /// 重放随机数
    /// </summary>
    public string Nonce { get; set; } = String.Empty;

    /// <summary>
    /// 请求地址
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Uri? Url { get; set; }

    /// <summary>
    /// 算法
    /// </summary>
    [JsonPropertyName("alg")]
    public string Algorithm { get; set; } = String.Empty;

    /// <summary>
    /// 账号地址（Kid、Jwk不可同时存在）
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Uri? Kid { get; set; }

    /// <summary>
    /// Json Web Key
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JsonWebKey? Jwk { get; set; }

    /// <summary>
    /// 扩展数据
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>
    /// 设置扩展数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetExtensionData(string key, object value)
    {
        this.ExtensionData ??= new();

        this.ExtensionData[key] = JsonSerializer.SerializeToElement(value, JsonSerializerUtil.DefaultOptions);
    }

    /// <summary>
    /// 获取扩展数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public T? GetExtensionData<T>(string key)
    {
        if (this.ExtensionData is null
            || !this.ExtensionData.TryGetValue(key, out var value))
        {
            return default;
        }

        return value.Deserialize<T>(JsonSerializerUtil.DefaultOptions);
    }
}
