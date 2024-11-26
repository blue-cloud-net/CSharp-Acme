namespace Acme.Enums;

public enum CertificateRevokeReasonCode
{
    /// <summary>
    /// 未指定
    /// </summary>
    Unspecified = 0,

    /// <summary>
    /// 密钥泄露
    /// </summary>
    KeyCompromise = 1,

    /// <summary>
    /// CA证书私钥泄露
    /// </summary>
    CACompromise = 2,

    /// <summary>
    /// 证书信息变更
    /// </summary>
    AffiliationChanged = 3,

    /// <summary>
    /// 已取代
    /// </summary>
    Superseded = 4,

    /// <summary>
    /// 域名停止运营
    /// </summary>
    CessationOfOperation = 5,

    /// <summary>
    /// 证书被撤销
    /// </summary>
    CertificateHold = 6,

    /// <summary>
    /// 根据吊销列表吊销
    /// </summary>
    RemoveFromCRL = 8,

    /// <summary>
    /// 属性证书被吊销
    /// </summary>
    PrivilegeWithdrawn = 9,

    /// <summary>
    /// 证书AA被破坏
    /// </summary>
    AACompromise = 10
}
