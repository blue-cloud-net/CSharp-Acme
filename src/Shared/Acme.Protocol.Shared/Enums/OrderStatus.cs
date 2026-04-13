using Acme.Protocol.Json.Converts;

namespace Acme.Protocol.Enums;

/// <summary>
/// 订单状态, 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.1.6"/>
/// </summary>
[JsonConverter(typeof(EnumDisplayNameJsonConverter))]
public enum OrderStatus
{
    /// <summary>
    /// 订单已创建，等待客户端完成所有授权验证
    /// </summary>
    Pending = 1,

    /// <summary>
    /// 所有授权已完成，订单已准备好接收 CSR 进行证书签发
    /// </summary>
    Ready = 2,

    /// <summary>
    /// 服务器正在处理 CSR 并签发证书
    /// </summary>
    Processing = 3,

    /// <summary>
    /// 证书已成功签发，可供客户端下载
    /// </summary>
    Valid = 4,

    /// <summary>
    /// 证书签发失败或订单已被服务器标记为无效
    /// </summary>
    Invalid = -1,
}