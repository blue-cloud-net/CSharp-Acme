namespace Acme.Client.Exceptions;

/// <summary>
/// Acme客户端异常
/// </summary>
public class AcmeClientException : Exception
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public AcmeClientException()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message"></param>
    public AcmeClientException(string message) : base(message)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public AcmeClientException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
