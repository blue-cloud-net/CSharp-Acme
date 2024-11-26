namespace Acme.HttpModels;

/// <summary>
/// Acme订单列表响应模型
/// </summary>
public class OrderListModel
{
    /// <summary>
    /// 订单列表地址
    /// </summary>
    public string[] Orders { get; set; } = Array.Empty<string>();
}
