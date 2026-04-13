namespace Acme.Protocol.Exceptions;

/// <summary>
/// 标识符的类型不受支持异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
/// </summary>
public class UnsupportedIdentifierException : AcmeException
{
    /// <summary>
    /// 初始化不支持的标识符类型异常实例
    /// </summary>
    public UnsupportedIdentifierException()
        : base(AcmeErrorTypes.UnsupportedIdentifier)
    {
    }

    /// <summary>
    /// 初始化不支持的标识符类型异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public UnsupportedIdentifierException(string message)
        : base(AcmeErrorTypes.UnsupportedIdentifier, message)
    {
    }
}
