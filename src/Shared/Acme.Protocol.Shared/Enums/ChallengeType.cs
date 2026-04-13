using Acme.Protocol.Json.Converts;

namespace Acme.Protocol.Enums;

/// <summary>
/// 挑战类型, 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-8"/>
/// </summary>
[JsonConverter(typeof(EnumDisplayNameJsonConverter))]
public enum ChallengeType
{
    /// <summary>
    /// HTTP-01 挑战，需要在 HTTP 服务器指定路径下放置验证令牌
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-8.3"/>
    /// </summary>
    [Display(Name = "http-01")]
    Http01 = 1,

    /// <summary>
    /// DNS-01 挑战，需要在 DNS 区域中添加 TXT 记录以证明域名控制权
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-8.4"/>
    /// </summary>
    [Display(Name = "dns-01")]
    Dns01 = 2,

    /// <summary>
    /// TLS-SNI-01 挑战（已废弃），曾用于通过 TLS SNI 扩展验证域名控制权
    /// </summary>
    [Obsolete("已废弃，存在安全漏洞。仅用于 RFC 发布前的版本")]
    [Display(Name = "tls-sni-01")]
    TlsSni01 = 3,

    /// <summary>
    /// TLS-SNI-02 挑战（已废弃），TLS-SNI-01 的改进版本
    /// </summary>
    [Obsolete("已废弃，存在安全漏洞。仅用于 RFC 发布前的版本")]
    [Display(Name = "tls-sni-02")]
    TlsSni02 = 4,

    /// <summary>
    /// TLS-ALPN-01 挑战，通过 TLS ALPN 扩展验证域名控制权
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8737#section-3"/>
    /// </summary>
    [Display(Name = "tls-alpn-01")]
    TlsAlpn01 = 5,

    /// <summary>
    /// Email-Reply-00 挑战，通过回复验证邮件证明邮箱所有权
    /// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8823#section-3"/>
    /// </summary>
    [Display(Name = "email-reply-00")]
    EmailReply00 = 6,
}
