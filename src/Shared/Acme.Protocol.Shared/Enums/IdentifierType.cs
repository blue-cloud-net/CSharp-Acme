namespace Acme.Enums;

/// <summary>
/// 标识类型
/// </summary>
public enum IdentifierType
{
    /// <summary>
    /// DNS
    /// <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-9.7.7"/>
    /// </summary>
    [Display(Name = "dns")]
    Dns = 1,

    /// <summary>
    /// Ip地址
    /// <see href="https://datatracker.ietf.org/doc/html/rfc8738#name-identifier-types"/>
    /// </summary>
    [Display(Name = "ip")]
    Ip = 2,

    /// <summary>
    /// 邮箱
    /// <see href="https://datatracker.ietf.org/doc/html/rfc8823/#name-acme-identifier-type"/>
    /// </summary>
    [Display(Name = "email")]
    Email = 3,
}
