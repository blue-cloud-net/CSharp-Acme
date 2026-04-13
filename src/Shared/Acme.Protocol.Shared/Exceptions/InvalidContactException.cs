namespace Acme.Protocol.Exceptions;

/// <summary>
/// 账户的联系人 URL 无效异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.3"/>
/// </summary>
public class InvalidContactException : AcmeException
{
    /// <summary>
    /// 初始化联系人 URL 无效异常实例
    /// </summary>
    public InvalidContactException(string message)
        : base(AcmeErrorTypes.InvalidContact,  message)
    {
    }
}
