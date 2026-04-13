namespace Acme.Protocol.Exceptions;

/// <summary>
/// 请求超出速率限制异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.6"/>
/// </summary>
public class RateLimitedException : AcmeException
{
    /// <summary>
    /// 初始化超出速率限制异常实例
    /// </summary>
    public RateLimitedException()
        : base(AcmeErrorTypes.RateLimited)
    {
    }
    /// <summary>
    /// 初始化速率限制异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public RateLimitedException(string message)
        : base(AcmeErrorTypes.RateLimited, message)
    {
    }}
