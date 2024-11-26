using Acme.Enums;

namespace Acme.HttpModels;

/// <summary>
/// Acme账户响应模型
/// <see href="https://tools.ietf.org/html/rfc8555#section-7.1.2"/>
/// </summary>
public class AccountModel
{
    /// <summary>
    /// 状态
    /// </summary>
    public AccountStatus Status { get; set; }

    /// <summary>
    /// 联系方式
    /// </summary>
    [JsonPropertyName("contact")]
    public string[]? Contacts { get; set; } = Array.Empty<string>();

    /// <summary>
    /// 是否同意服务协议
    /// </summary>
    public bool? TermsOfServiceAgreed { get; set; }

    /// <summary>
    /// 绑定Acme外部账户（CA账户Id等）的Jws
    /// 可参照<see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.3.4"/>中的案例
    /// </summary>
    public string? ExternalAccountBinding { get; set; }

    /// <summary>
    /// 订单服务地址
    /// </summary>
    public string Orders { get; set; } = string.Empty;
}
