namespace Acme.Protocol.Exceptions;

/// <summary>
/// 服务器不会为该标识符颁发证书异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
/// </summary>
public class RejectedIdentifierException : AcmeException
{
    /// <summary>
    /// 初始化标识符被拒绝异常实例
    /// </summary>
    public RejectedIdentifierException()
        : base(AcmeErrorTypes.RejectedIdentifier)
    {
    }
    /// <summary>
    /// 初始化标识符被拒异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public RejectedIdentifierException(string message)
        : base(AcmeErrorTypes.RejectedIdentifier, message)
    {
    }}
