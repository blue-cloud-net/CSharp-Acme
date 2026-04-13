using Acme.Protocol.Json.Converts;

namespace Acme.Protocol.Enums;

/// <summary>
/// 账户状态, 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.1.2"/>
/// </summary>
[JsonConverter(typeof(EnumDisplayNameJsonConverter))]
public enum AccountStatus
{
    /// <summary>
    /// 有效
    /// </summary>
    [Display(Name = "valid")]
    Valid = 1,

    /// <summary>
    /// 已停用
    /// </summary>
    [Display(Name = "deactivated")]
    Deactivated = -1,

    /// <summary>
    /// 已注销
    /// </summary>
    [Display(Name = "revoked")]
    Revoked = -2,
}
