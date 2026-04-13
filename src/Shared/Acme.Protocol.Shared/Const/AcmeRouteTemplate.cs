namespace Acme.Protocol.Const;

/// <summary>
/// 路由模板
/// </summary>
public class AcmeRouteTemplate
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    /// <summary>
    /// 获取 ACME 目录信息
    /// </summary>
    public const string GetDirectory = "directory";

    /// <summary>
    /// 获取新的防重放随机数
    /// </summary>
    public const string NewNonce = "new-nonce";

    /// <summary>
    /// 创建新账户
    /// </summary>
    public const string NewAccount = "new-account";

    /// <summary>
    /// 根据账户标识获取或更新账户信息
    /// </summary>
    public const string GetOrSetAccount = "acct/{accountId}";

    /// <summary>
    /// 账户密钥轮换
    /// </summary>
    public const string KeyChange = "key-change";

    /// <summary>
    /// 创建新订单
    /// </summary>
    public const string NewOrder = "new-order";

    /// <summary>
    /// 查询订单详情
    /// </summary>
    public const string GetOrder = "order/{orderId}";

    /// <summary>
    /// 查询指定账户的订单列表
    /// </summary>
    public const string GetOrders = "orders/{accountId}";

    /// <summary>
    /// 提交 CSR 完成订单
    /// </summary>
    public const string FinalizeOrder = "order/{orderId}/finalize";

    /// <summary>
    /// 创建新的授权
    /// </summary>
    public const string NewAuthorization = "new-authz";

    /// <summary>
    /// 获取或更新授权资源
    /// </summary>
    public const string GetOrSetAuthorization = "authz/{authzId}";

    /// <summary>
    /// 提交挑战应答
    /// </summary>
    public const string AcceptChallenge = "chall/{challId}";

    /// <summary>
    /// 下载颁发的证书链
    /// </summary>
    public const string GetCertificate = "cert/{orderId}";

    /// <summary>
    /// 吊销已颁发的证书
    /// </summary>
    public const string RevokeCertificate = "revoke-cert";

    /// <summary>
    /// 查询证书的续订信息
    /// </summary>
    public const string GetRenewalInfo = "renewal-info/{certId}";

    /// <summary>
    /// 更新续订偏好或状态
    /// </summary>
    public const string UpdateRenewalInfo = "renewal-info";
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
}
