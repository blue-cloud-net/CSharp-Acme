namespace Acme.Enums;

/// <summary>
/// 挑战类型
/// <see href="https://www.rfc-editor.org/rfc/rfc8555.html#section-9.7.8"/>
/// </summary>
public enum ChallengeType
{
    /// <summary>
    /// http挑战，需要在指定路径下放置指定内容
    /// </summary>
    [Display(Name = "http-01")]
    Http01 = 1,

    /// <summary>
    /// dns挑战，需要在DNS中添加TXT记录
    /// </summary>
    [Display(Name = "dns-01")]
    Dns01 = 2,

    /// <summary>
    /// tls-sni-01挑战，需要在TLS握手中添加指定内容
    /// </summary>
    [Obsolete("Do not use. Use in pre-RFC version.")]
    [Display(Name = "tls-sni-01")]
    TlsSni01 = 3,

    /// <summary>
    /// tls-sni-01挑战，需要在TLS握手中添加指定内容
    /// </summary>
    [Obsolete("Do not use. Use in pre-RFC version.")]
    [Display(Name = "tls-sni-02")]
    TlsSni02 = 4,

    /// <summary>
    /// tls-alpn-01挑战，需要在TLS握手中添加指定内容
    /// <see href="https://www.rfc-editor.org/rfc/rfc8737.html#name-acme-validation-method"/>
    /// </summary>
    [Display(Name = "dnstls-alpn-01")]
    TlsAlpn01 = 5,

    /// <summary>
    /// EMAIL挑战，需要在TLS握手中添加指定内容
    /// </summary>
    /// <see href="https://www.rfc-editor.org/rfc/rfc8823.html#name-acme-identifier-type"/>
    [Display(Name = "email-reply-00")]
    EmailReply00 = 6,
}
