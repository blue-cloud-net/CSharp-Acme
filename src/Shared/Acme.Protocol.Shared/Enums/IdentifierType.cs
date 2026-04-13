namespace Acme.Protocol.Enums;

/// <summary>
/// 标识符类型，用于指定需要验证的资源类型
/// </summary>
public enum IdentifierType
{
    /// <summary>
    /// DNS 域名标识符，参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-9.7.7"/>
    /// </summary>
    [Display(Name = "dns")]
    Dns = 1,

    /// <summary>
    /// IP 地址标识符（IPv4 或 IPv6），参考 <see href="https://datatracker.ietf.org/doc/html/rfc8738#section-3"/>
    /// </summary>
    [Display(Name = "ip")]
    Ip = 2,

    /// <summary>
    /// 邮箱地址标识符，参考 <see href="https://datatracker.ietf.org/doc/html/rfc8823#section-2"/>
    /// </summary>
    [Display(Name = "email")]
    Email = 3,
}
