using Acme.Enums;

namespace Acme.HttpModels;

/// <summary>
/// Acme授权响应模型
/// <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.1.4"/>
/// </summary>
public class AuthorizationModel
{
    /// <summary>
    /// 标识
    /// </summary>
    public IdentifierModel Identifier { get; set; } = default!;

    /// <summary>
    /// 状态
    /// </summary>
    public AuthorizationStatus Status { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTimeOffset? Expires { get; set; }

    /// <summary>
    /// 挑战地址
    /// </summary>
    public ChallengeStatus[] Challenges { get; set; } = Array.Empty<ChallengeStatus>();

    /// <summary>
    /// DNS标识符的值是否包含通配符
    /// </summary>
    public bool? Wildcard { get; set; }
}
