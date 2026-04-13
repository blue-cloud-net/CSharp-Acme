using Acme.Protocol.Json.Converts;

namespace Acme.Protocol.Enums;

/// <summary>
/// 挑战状态, 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.1.6"/>
/// </summary>
[JsonConverter(typeof(EnumDisplayNameJsonConverter))]
public enum ChallengeStatus
{
    /// <summary>
    /// 挑战已创建，等待客户端响应
    /// </summary>
    Pending = 1,

    /// <summary>
    /// 服务器正在验证客户端的挑战响应
    /// </summary>
    Processing = 2,

    /// <summary>
    /// 客户端已成功完成挑战验证
    /// </summary>
    Valid = 3,

    /// <summary>
    /// 客户端未能通过挑战验证
    /// </summary>
    Invalid = -1,
}
