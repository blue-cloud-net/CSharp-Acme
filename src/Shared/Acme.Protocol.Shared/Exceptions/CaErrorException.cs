namespace Acme.Protocol.Exceptions;

/// <summary>
/// CAA (Certificate Authority Authorization) 异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
/// </summary>
public class CaErrorException : AcmeException
{
    /// <summary>
    /// 初始化 CAA 异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public CaErrorException(string message)
        : base(AcmeErrorTypes.Caa, message)
    {
    }
}
