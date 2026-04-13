namespace Acme.Protocol.Enums;

/// <summary>
/// 证书吊销原因码, 
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc5280#section-5.3.1"/> 
/// 和 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.6"/>
/// </summary>
public enum CertificateRevokeReasonCode
{
    /// <summary>
    /// 未指定
    /// </summary>
    Unspecified = 0,

    /// <summary>
    /// 证书持有者的私钥已泄露或疑似泄露
    /// </summary>
    KeyCompromise = 1,

    /// <summary>
    /// 证书颁发机构的私钥已泄露或疑似泄露
    /// </summary>
    CaCompromise = 2,

    /// <summary>
    /// 证书主体的从属关系或其他信息已发生变更
    /// </summary>
    AffiliationChanged = 3,

    /// <summary>
    /// 证书已被新证书取代
    /// </summary>
    Superseded = 4,

    /// <summary>
    /// 证书主体已停止运营或服务已终止
    /// </summary>
    CessationOfOperation = 5,

    /// <summary>
    /// 证书被临时暂停使用（可恢复）
    /// </summary>
    CertificateHold = 6,

    /// <summary>
    /// 从证书吊销列表(CRL)中移除（取消 CertificateHold 状态）
    /// </summary>
    RemoveFromCrl = 8,

    /// <summary>
    /// 证书持有者的特权或权限已被撤销
    /// </summary>
    PrivilegeWithdrawn = 9,

    /// <summary>
    /// 属性授权机构(AA)的私钥已泄露或疑似泄露
    /// </summary>
    AACompromise = 10
}
