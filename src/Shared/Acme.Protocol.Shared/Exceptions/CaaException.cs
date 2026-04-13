namespace Acme.Protocol.Exceptions;

/// <summary>
/// 证书颁发机构授权 (CAA) 记录禁止 CA 颁发证书异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/> 
// 和 <see href="https://datatracker.ietf.org/doc/html/rfc8657"/>
/// </summary>
public class CaaException : AcmeException
{
    /// <summary>
    /// 初始化 CAA 记录禁止颁发证书异常实例
    /// </summary>
    public CaaException()
        : base(AcmeErrorTypes.Caa)
    {
    }

    /// <summary>
    /// 初始化 CAA 记录禁止颁发证书异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public CaaException(string message)
        : base(AcmeErrorTypes.Caa, message)
    {
    }
}
