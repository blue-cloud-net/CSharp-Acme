namespace Acme.Protocol.Exceptions;

/// <summary>
/// 请求中指定的账户不存在异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
/// </summary>
public class AccountDoesNotExistException : AcmeException
{
    /// <summary>
    /// 初始化账户不存在异常实例
    /// </summary>
    public AccountDoesNotExistException()
        : base(AcmeErrorTypes.AccountDoesNotExist)
    {
    }

    /// <summary>
    /// 初始化账户不存在异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public AccountDoesNotExistException(string message)
        : base(AcmeErrorTypes.AccountDoesNotExist, message)
    {
    }
}
