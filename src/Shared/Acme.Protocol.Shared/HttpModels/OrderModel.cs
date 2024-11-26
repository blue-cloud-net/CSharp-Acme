namespace Acme.HttpModels;

/// <summary>
/// Acme订单响应模型
/// <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.1.3"/>
/// </summary>
public class OrderModel : OrderCreateModel
{
    /// <summary>
    /// 状态
    /// </summary>
    [JsonPropertyOrder(1)]
    public OrderStatus Status { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    [JsonPropertyOrder(101)]
    public DateTimeOffset? Expires { get; set; }

    /// <summary>
    /// 错误
    /// </summary>
    [JsonPropertyOrder(102)]
    public AcmeError? Error { get; set; }

    /// <summary>
    /// 订单授权地址
    /// </summary>
    [JsonPropertyOrder(103)]
    public string[] Authorizations { get; set; } = Array.Empty<string>();

    /// <summary>
    /// 订单确认完成地址
    /// </summary>
    [JsonPropertyOrder(104)]
    public string Finalize { get; set; } = string.Empty;

    /// <summary>
    /// 下载证书地址
    /// </summary>
    [JsonPropertyOrder(105)]
    public string? Certificate { get; set; }
}
