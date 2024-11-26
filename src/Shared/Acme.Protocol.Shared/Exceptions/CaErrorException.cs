namespace Acme.Exceptions;

/// <summary>
/// CA错误异常
/// </summary>
public class CaErrorException : AcmeException
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message"></param>
    public CaErrorException(string message)
        : base(AcmeErrorTypes.BadNonce, message)
    {
    }
}
