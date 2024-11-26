namespace Acme.Const;

/// <summary>
/// 路由模板
/// </summary>
public class AcmeRouteTemplate
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public const string GetDirectory = "directory";

    public const string NewNonce = "new-nonce";

    public const string NewAccount = "new-account";
    public const string GetOrSetAccount = "acct/{accountId}";
    public const string KeyChange = "key-change";

    public const string NewOrder = "new-order";
    public const string GetOrder = "order/{orderId}";
    public const string GetOrders = "orders/{accountId}";
    public const string FinalizeOrder = "order/{orderId}/finalize";

    public const string NewAuthorization = "new-authz";
    public const string GetOrSetAuthorization = "authz/{authzId}";

    public const string AcceptChallenge = "chall/{challId}";

    public const string GetCertificate = "cert/{orderId}";
    public const string RevokeCertificate = "revoke-cert";

    public const string GetRenewalInfo = "renewal-info/{certId}";
    public const string UpdateRenewalInfo = "renewal-info";
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
}
