namespace Acme.Protocol.Exceptions;

/// <summary>
/// 服务器在验证期间收到 TLS 错误异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
/// </summary>
public class TlsException : AcmeException
{
    /// <summary>
    /// 初始化 TLS 错误异常实例
    /// </summary>
    public TlsException()
        : base(AcmeErrorTypes.Tls)
    {
    }

    /// <summary>
    /// 初始化 TLS 错误异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public TlsException(string message)
        : base(AcmeErrorTypes.Tls, message)
    {
    }
}
