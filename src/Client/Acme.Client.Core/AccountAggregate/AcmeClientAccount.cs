namespace Acme.Client.Core.AggregatesModel.AccountAggregate;

/// <summary>
/// 账户
/// </summary>
public class AcmeClientAccount : FullAuditedEntity
{
    /// <summary>
    /// 状态
    /// </summary>
    public AccountStatus Status { get; set; }
}
