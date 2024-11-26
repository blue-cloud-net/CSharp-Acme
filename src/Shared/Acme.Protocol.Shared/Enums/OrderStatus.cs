namespace Acme.Enums;

/// <summary>
/// 订单状态
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// 订单已创建，等待授权
    /// </summary>
    Pending,

    /// <summary>
    /// 订单已准备好接收Csr
    /// </summary>
    Ready,

    /// <summary>
    /// 订单已推送Csr，到CA等待签发
    /// </summary>
    Processing,

    /// <summary>
    /// 证书已签发
    /// </summary>
    Valid,

    /// <summary>
    /// 证书签发失败
    /// </summary>
    Invalid
}
