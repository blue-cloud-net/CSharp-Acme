namespace Acme.Protocol.Exceptions;

/// <summary>
/// 客户端缺少足够的授权异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
/// </summary>
public class UnauthorizedException : AcmeException
{
    /// <summary>
    /// 初始化未授权异常实例
    /// </summary>
    public UnauthorizedException()
        : base(AcmeErrorTypes.Unauthorized)
    {
    }

    /// <summary>
    /// 初始化未授权异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public UnauthorizedException(string message)
        : base(AcmeErrorTypes.Unauthorized, message)
    {
    }
}
