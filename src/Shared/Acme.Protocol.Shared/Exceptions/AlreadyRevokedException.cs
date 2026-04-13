namespace Acme.Protocol.Exceptions;

/// <summary>
/// 请求吊销的证书已经被吊销异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
/// </summary>
public class AlreadyRevokedException : AcmeException
{
    /// <summary>
    /// 初始化证书已被吊销异常实例
    /// </summary>
    public AlreadyRevokedException()
        : base(AcmeErrorTypes.AlreadyRevoked)
    {
    }
    /// <summary>
    /// 初始化已撤销异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public AlreadyRevokedException(string message)
        : base(AcmeErrorTypes.AlreadyRevoked, message)
    {
    }}
