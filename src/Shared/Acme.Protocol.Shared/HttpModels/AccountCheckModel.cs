namespace Acme.Protocol.HttpModels;

/// <summary>
/// Acme账户检查模型
/// <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.3.1"/>
/// </summary>
public class AccountCheckModel
{
    /// <summary>
    /// 是否仅返回已存在的账户信息，
    /// 如果为true且请求中指定的账户不存在，
    /// 则返回404 Not Found错误
    /// </summary>
    public bool OnlyReturnExisting { get; set; }
}
