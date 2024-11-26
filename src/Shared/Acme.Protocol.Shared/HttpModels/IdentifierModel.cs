namespace Acme.HttpModels;

/// <summary>
/// 订单标识，用于存放
/// </summary>
public class IdentifierModel
{
    /// <summary>
    /// 类型
    /// </summary>
    public string Type { get; } = string.Empty;

    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; } = string.Empty;
}
