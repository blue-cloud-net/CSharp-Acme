namespace Acme.Protocol.Exceptions;

/// <summary>
/// 服务器无法连接到验证目标异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
/// </summary>
public class ConnectionException : AcmeException
{
    /// <summary>
    /// 初始化连接验证目标失败异常实例
    /// </summary>
    public ConnectionException()
        : base(AcmeErrorTypes.Connection)
    {
    }
    /// <summary>
    /// 初始化连接异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public ConnectionException(string message)
        : base(AcmeErrorTypes.Connection, message)
    {
    }}
