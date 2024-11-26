namespace Acme.Exceptions;

/// <summary>
/// 错误Nonce异常
/// </summary>
public class BadNonceException : AcmeException
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public BadNonceException() : base(AcmeErrorTypes.BadNonce)
    {
    }
}
