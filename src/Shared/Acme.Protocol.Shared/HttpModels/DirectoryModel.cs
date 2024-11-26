namespace Acme.HttpModels;

/// <summary>
/// Acme目录响应模型
/// <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.1.1"/>
/// </summary>
public class DirectoryModel
{
    /// <summary>
    /// 获取新的Nonce地址
    /// </summary>
    public string? NewNonce { get; set; }

    /// <summary>
    /// 创建新的账户地址
    /// </summary>
    public string? NewAccount { get; set; }

    /// <summary>
    /// 创建新的订单地址
    /// </summary>
    public string? NewOrder { get; set; }

    /// <summary>
    /// 创建授权地址
    /// </summary>
    public string? NewAuthz { get; set; }

    /// <summary>
    /// 吊销证书地址
    /// </summary>
    public string? RevokeCert { get; set; }

    /// <summary>
    /// 密钥变更地址
    /// </summary>
    public string? KeyChange { get; set; }

    /// <summary>
    /// 目录元数据
    /// </summary>
    public DirectoryMetadata? Meta { get; set; }
}
