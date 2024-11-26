namespace Acme.HttpModels;

/// <summary>
/// 账户创建模型
/// </summary>
public class AccountCreateModel
{
    /// <summary>
    /// 联系方式
    /// </summary>
    [JsonPropertyOrder(51)]
    [JsonPropertyName("contact")]
    public string[]? Contacts { get; set; } = [];

    /// <summary>
    /// 是否同意服务协议
    /// </summary>
    [JsonPropertyOrder(52)]
    public bool? TermsOfServiceAgreed { get; set; }

    /// <summary>
    /// 绑定Acme外部账户（CA账户Id等）的Jws
    /// 可参照<see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.3.4"/>中的案例
    /// </summary>
    [JsonPropertyOrder(53)]
    public string? ExternalAccountBinding { get; set; }
}
