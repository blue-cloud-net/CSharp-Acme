namespace Acme.Protocol.HttpModels;

/// <summary>
/// 订单终结模型
/// </summary>
public class OrderFinalizeModel
{
    /// <summary>
    /// BASE64Url格式的CSR
    /// </summary>
#if NET7_0_OR_GREATER
    public required string Csr { get; set; }
#else
    public string Csr { get; set; } = default!;
#endif
}