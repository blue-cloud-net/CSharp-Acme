namespace Acme.Client.Contexts;

/// <summary>
/// ACME账户上下文
/// </summary>
public interface IAcmeAccountContext
{
    /// <summary>
    /// 签名器
    /// </summary>
    public ISigner Signer { get; set; }

    /// <summary>
    /// 账户信息
    /// </summary>
    public AccountModel? Account { get; set; }
}
