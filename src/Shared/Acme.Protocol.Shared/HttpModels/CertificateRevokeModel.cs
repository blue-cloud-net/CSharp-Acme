namespace Acme.HttpModels;

public class CertificateRevokeModel
{
    /// <summary>
    /// 证书
    /// </summary>
    public string Certificate { get; set; } = String.Empty;

    /// <summary>
    /// 原因
    /// </summary>
    public CertificateRevokeReasonCode Reason { get; set; }
}
