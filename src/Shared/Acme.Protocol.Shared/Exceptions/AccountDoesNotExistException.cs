namespace Acme.Exceptions;

/// <summary>
/// 账户不存在异常
/// </summary>
public class AccountDoesNotExistException : AcmeException
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public AccountDoesNotExistException()
        : base(AcmeErrorTypes.AccountDoesNotExist)
    {
    }
}
