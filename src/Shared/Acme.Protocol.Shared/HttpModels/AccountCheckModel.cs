namespace Acme.HttpModels;

/// <summary>
/// Acme账户检查模型
/// <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.3.1"/>
/// </summary>
public class AccountCheckModel
{
    /// <summary>
    ///
    /// </summary>
    public bool OnlyReturnExisting { get; set; }
}
