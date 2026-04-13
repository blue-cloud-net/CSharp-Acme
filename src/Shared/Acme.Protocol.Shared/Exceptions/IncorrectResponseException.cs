namespace Acme.Protocol.Exceptions;

/// <summary>
/// 收到的响应与挑战要求不匹配异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
/// </summary>
public class IncorrectResponseException : AcmeException
{
    /// <summary>
    /// 初始化响应不匹配挑战要求异常实例
    /// </summary>
    public IncorrectResponseException()
        : base(AcmeErrorTypes.IncorrectResponse)
    {
    }
    /// <summary>
    /// 初始化响应不正确异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public IncorrectResponseException(string message)
        : base(AcmeErrorTypes.IncorrectResponse, message)
    {
    }}
