namespace Acme.HttpModels;

/// <summary>
/// Acme的Jws的Protected部分
/// </summary>
public class AcmeJwsProtectedModel
{
    /// <summary>
    /// 重放随机数
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Nonce { get; set; }

    /// <summary>
    /// 请求地址
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; set; }

    /// <summary>
    /// 算法
    /// </summary>
    [JsonPropertyName("alg")]
    public string Algorithm { get; set; } = string.Empty;

    /// <summary>
    /// 账号地址（Kid、Jwk不可同时存在）
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Kid { get; set; }

    /// <summary>
    /// Json Web Key
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JsonWebKey? Jwk { get; set; }
}
