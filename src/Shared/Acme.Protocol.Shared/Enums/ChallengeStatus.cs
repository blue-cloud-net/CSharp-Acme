namespace Acme.Enums;

/// <summary>
/// 挑战状态
/// </summary>
public enum ChallengeStatus
{
    /// <summary>
    /// 挑战已创建，等待验证
    /// </summary>
    Pending,

    /// <summary>
    /// 挑战验证中
    /// </summary>
    Processing,

    /// <summary>
    /// 挑战验证成功
    /// </summary>
    Valid,

    /// <summary>
    /// 挑战验证失败
    /// </summary>
    Invalid
}
