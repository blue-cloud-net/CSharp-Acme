namespace Acme.Protocol.Exceptions;

/// <summary>
/// 服务器遇到内部错误异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
/// </summary>
public class ServerInternalException : AcmeException
{
    /// <summary>
    /// 初始化服务器内部错误异常实例
    /// </summary>
    public ServerInternalException()
        : base(AcmeErrorTypes.ServerInternal)
    {
    }
    /// <summary>
    /// 初始化服务器内部异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public ServerInternalException(string message)
        : base(AcmeErrorTypes.ServerInternal, message)
    {
    }}
