namespace Acme.Exceptions;

/// <summary>
/// 错误CSR异常
/// </summary>
public class BadCSRException : AcmeException
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message"></param>
    public BadCSRException(string message)
        : base(AcmeErrorTypes.BadCSR, message)
    {
    }
}
