using Acme.Enums;

namespace Acme.HttpModels;

/// <summary>
/// Acme订单响应模型
/// <see href="https://tools.ietf.org/html/rfc8555#section-7.1.3"/>
/// </summary>
public class OrderModel
{
    /// <summary>
    /// 状态
    /// </summary>
    public OrderStatus Status { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTimeOffset? Expires { get; set; }

    /// <summary>
    /// 订单标识
    /// </summary>
    public IdentifierModel[] Identifiers { get; set; } = Array.Empty<IdentifierModel>();

    /// <summary>
    /// 证书开始时间
    /// </summary>
    public DateTimeOffset? NotBefore { get; set; }

    /// <summary>
    /// 证书结束时间
    /// </summary>
    public DateTimeOffset? NotAfter { get; set; }

    /// <summary>
    /// 错误
    /// </summary>
    public AcmeError? Error { get; set; }

    /// <summary>
    /// 订单授权地址
    /// </summary>
    public string[] Authorizations { get; set; } = Array.Empty<string>();

    /// <summary>
    /// 订单确认完成地址
    /// </summary>
    public string Finalize { get; set; } = string.Empty;

    /// <summary>
    /// 下载证书地址
    /// </summary>
    public string? Certificate { get; set; }
}
