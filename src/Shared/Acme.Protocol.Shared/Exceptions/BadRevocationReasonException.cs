namespace Acme.Protocol.Exceptions;

/// <summary>
/// 服务器不允许提供的吊销原因异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.6"/>
/// </summary>
public class BadRevocationReasonException : AcmeException
{
    /// <summary>
    /// 初始化吊销原因不允许异常实例
    /// </summary>
    public BadRevocationReasonException()
        : base(AcmeErrorTypes.BadRevocationReason)
    {
    }
    /// <summary>
    /// 初始化撤销原因无效异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public BadRevocationReasonException(string message)
        : base(AcmeErrorTypes.BadRevocationReason, message)
    {
    }}
