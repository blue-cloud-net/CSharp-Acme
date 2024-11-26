namespace Acme.Exceptions;

/// <summary>
/// 错误公钥异常
/// </summary>
public class BadPublicKey : AcmeException
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message"></param>
    public BadPublicKey(string message)
        : base(AcmeErrorTypes.BadPublicKey, message)
    {
    }
}
