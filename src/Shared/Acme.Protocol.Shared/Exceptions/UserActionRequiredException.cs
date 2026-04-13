namespace Acme.Protocol.Exceptions;

/// <summary>
/// 需要访问 "instance" URL 并执行其中指定的操作异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
/// </summary>
public class UserActionRequiredException : AcmeException
{
    /// <summary>
    /// 初始化需要用户操作异常实例
    /// </summary>
    /// <param name="instance">用户需要访问的 URL</param>
    public UserActionRequiredException(string instance)
        : base(AcmeErrorTypes.UserActionRequired)
    {
        this.Instance = instance;
    }

    /// <summary>
    /// 初始化需要用户操作异常实例
    /// </summary>
    /// <param name="instance">用户需要访问的 URL</param>
    /// <param name="message">详细错误信息</param>
    public UserActionRequiredException(string instance, string message)
        : base(AcmeErrorTypes.UserActionRequired, message)
    {
        this.Instance = instance;
    }

    /// <summary>
    /// 用户需要访问的 URL 以完成所需操作
    /// </summary>
    public string Instance { get; set; }
}
