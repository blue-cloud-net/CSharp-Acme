namespace Acme.Protocol.Exceptions;

/// <summary>
/// 客户端发送的防重放随机数不可接受异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.5"/>
/// </summary>
public class BadNonceException : AcmeException
{
    /// <summary>
    /// 初始化防重放随机数不可接受异常实例
    /// </summary>
    public BadNonceException() : base(AcmeErrorTypes.BadNonce)
    {
    }

    /// <summary>
    /// 初始化防重放随机数不可接受异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public BadNonceException(string message) : base(AcmeErrorTypes.BadNonce, message)
    {
    }
}
