namespace Acme.Protocol.Exceptions;

/// <summary>
/// CSR 不可接受异常（例如，密钥长度太短、算法不支持等）
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
/// </summary>
public class BadCSRException : AcmeException
{
    /// <summary>
    /// 初始化 CSR 不可接受异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public BadCSRException(string message)
        : base(AcmeErrorTypes.BadCSR, message)
    {
    }
}
