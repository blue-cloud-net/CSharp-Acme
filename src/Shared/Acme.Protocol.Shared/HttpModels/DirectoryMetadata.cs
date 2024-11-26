namespace Acme.HttpModels;

/// <summary>
/// Acme目录元数据响应模型
/// https://tools.ietf.org/html/rfc8555#section-7.1.1
/// </summary>
public class DirectoryMetadata
{
    /// <summary>
    /// 服务协议地址
    /// </summary>
    public string? TermsOfService { get; set; }

    /// <summary>
    /// 关于Acme服务的信息地址
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// CAA标识填写标识符
    /// </summary>
    public string[]? CAAIdentities { get; set; }

    /// <summary>
    /// 是否需要提供外部账户
    /// </summary>
    public bool? ExternalAccountRequired { get; set; }
}
