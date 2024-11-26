using Acme.Enums;

namespace Acme.HttpModels;

/// <summary>
/// Acme挑战响应模型
/// <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.1.5"/>
/// </summary>
public class ChallengeModel
{
    /// <summary>
    /// 类型
    /// </summary>
    public ChallengeType Type { get; set; }

    /// <summary>
    /// 地址，用于激活验证挑战
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// 状态
    /// </summary>
    public ChallengeStatus Status { get; set; }

    /// <summary>
    /// 验证时间
    /// </summary>
    public DateTimeOffset? Validated { get; set; }

    /// <summary>
    /// 挑战Token
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// 挑战失败时的错误信息
    /// </summary>
    public AcmeError? Error { get; }
}
