namespace Acme.HttpModels;

/// <summary>
/// 订单创建模型
/// </summary>
public class OrderCreateModel
{
    /// <summary>
    /// 订单标识
    /// </summary>
    [JsonPropertyOrder(50)]
    public IdentifierModel[] Identifiers { get; set; } = Array.Empty<IdentifierModel>();

    /// <summary>
    /// 证书开始时间
    /// </summary>
    [JsonPropertyOrder(51)]
    public DateTimeOffset? NotBefore { get; set; }

    /// <summary>
    /// 证书结束时间
    /// </summary>
    [JsonPropertyOrder(52)]
    public DateTimeOffset? NotAfter { get; set; }
}
