namespace Acme.Enums;

/// <summary>
/// 授权状态
/// </summary>
public enum AuthorizationStatus
{
    /// <summary>
    /// 授权已创建，等待验证
    /// </summary>
    Pending,

    /// <summary>
    /// 授权已验证
    /// </summary>
    Valid,

    /// <summary>
    /// 授权验证失败
    /// </summary>
    Invalid,

    /// <summary>
    /// 授权已注销
    /// </summary>
    Revoked,

    /// <summary>
    /// 授权已停用
    /// </summary>
    Deactivated,

    /// <summary>
    /// 授权已过期
    /// </summary>
    Expired
}
