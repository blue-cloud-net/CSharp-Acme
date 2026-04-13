namespace Acme.Protocol.Exceptions;

/// <summary>
/// 请求消息格式错误异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
/// </summary>
public class MalformedRequestException : AcmeException
{
    /// <summary>
    /// 初始化请求格式错误异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public MalformedRequestException(string message)
        : base(AcmeErrorTypes.Malformed, message)
    {
    }
}

/// <summary>
/// 请求的资源未找到异常
/// </summary>
public class NotFoundException : MalformedRequestException
{
    /// <summary>
    /// 初始化资源未找到异常实例
    /// </summary>
    public NotFoundException()
        : base(RS.ResourceNotFound)
    { }
}
