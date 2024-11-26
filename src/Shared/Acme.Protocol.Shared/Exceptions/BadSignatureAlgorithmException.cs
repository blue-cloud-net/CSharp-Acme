namespace Acme.Exceptions;

/// <summary>
/// 错误签名算法异常
/// </summary>
public class BadSignatureAlgorithmException : AcmeException
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public BadSignatureAlgorithmException() : base(AcmeErrorTypes.BadSignatureAlgorithm)
    {
    }
}
