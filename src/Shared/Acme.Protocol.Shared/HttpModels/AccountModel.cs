namespace Acme.HttpModels;

/// <summary>
/// Acme账户响应模型
/// <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.1.2"/>
/// </summary>
public class AccountModel : AccountCreateModel
{
    /// <summary>
    /// 状态
    /// </summary>
    [JsonPropertyOrder(1)]
    public AccountStatus Status { get; set; }

    /// <summary>
    /// 订单服务地址
    /// </summary>
    [JsonPropertyOrder(101)]
    public string Orders { get; set; } = string.Empty;

    /// <summary>
    /// 账户Id
    /// </summary>
    [JsonIgnore]
    public Uri? Kid { get; set; }

    /// <summary>
    /// 服务协议地址
    /// </summary>
    [JsonIgnore]
    public Uri? TosLink { get; set; }
}
