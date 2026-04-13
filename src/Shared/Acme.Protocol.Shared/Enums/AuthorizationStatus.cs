using Acme.Protocol.Json.Converts;

namespace Acme.Protocol.Enums;

/// <summary>
/// 授权状态, 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.1.6"/>
/// </summary>
[JsonConverter(typeof(EnumDisplayNameJsonConverter))]
public enum AuthorizationStatus
{
    /// <summary>
    /// 授权已创建，等待客户端响应挑战
    /// </summary>
    Pending = 1,

    /// <summary>
    /// 服务器已成功验证客户端对标识符的控制权
    /// </summary>
    Valid = 2,

    /// <summary>
    /// 客户端未能通过挑战验证或授权已被服务器标记为无效
    /// </summary>
    Invalid = -1,

    /// <summary>
    /// 服务器已撤销授权
    /// </summary>
    Revoked = -2,

    /// <summary>
    /// 客户端已主动停用此授权
    /// </summary>
    Deactivated = -3,

    /// <summary>
    /// 授权已过期
    /// </summary>
    Expired  = -4,
}
