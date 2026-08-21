namespace Acme.Protocol.X509;

/// <summary>
/// X.509 证书撤销列表 (CRL)
/// </summary>
public sealed class CertificateRevocationList
{
    /// <summary>
    /// 签发者
    /// </summary>
    public string Issuer { get; private set; }

    /// <summary>
    /// 此次更新时间
    /// </summary>
    public DateTimeOffset ThisUpdate { get; private set; }

    /// <summary>
    /// 下次更新时间
    /// </summary>
    public DateTimeOffset? NextUpdate { get; private set; }

    /// <summary>
    /// 撤销证书数量
    /// </summary>
    public int RevokedCertificateCount { get; private set; }

    /// <summary>
    /// 原始 CRL 对象
    /// </summary>
    internal readonly X509Crl BcCrl;

    /// <summary>
    /// 从 X509Crl 初始化
    /// </summary>
    /// <param name="crl">BouncyCastle X509Crl 对象</param>
    public CertificateRevocationList(X509Crl crl)
    {
        ArgumentNullException.ThrowIfNull(crl, nameof(crl));
        
        this.BcCrl = crl;
        this.Issuer = crl.IssuerDN.ToString();
        this.ThisUpdate = crl.ThisUpdate;
        this.NextUpdate = crl.NextUpdate;
        this.RevokedCertificateCount = crl.GetRevokedCertificates()?.Count ?? 0;
    }

    /// <summary>
    /// 从 PEM 格式 CRL 字符串或 DER 格式字节数组解析
    /// </summary>
    /// <param name="pemCrl">PEM 格式 CRL 字符串</param>
    /// <returns>CertificateRevocationList 实例</returns>
    public static CertificateRevocationList Parse(string pemCrl)
    {
        ArgumentNullException.ThrowIfNull(pemCrl, nameof(pemCrl));
        
        var derBytes = PemFormatter.GetCrlBytes(pemCrl);
        var crlParser = new X509CrlParser();
        var crl = crlParser.ReadCrl(derBytes);
        return new CertificateRevocationList(crl);
    }

    /// <summary>
    /// 从 DER 格式 CRL 字节数组解析
    /// </summary>
    /// <param name="derBytes">DER 格式 CRL 字节数组</param>
    /// <returns>CertificateRevocationList 实例</returns>
    public static CertificateRevocationList Parse(byte[] derBytes)
    {
        ArgumentNullException.ThrowIfNull(derBytes, nameof(derBytes));
        
        var crlParser = new X509CrlParser();
        var crl = crlParser.ReadCrl(derBytes);
        return new CertificateRevocationList(crl);
    }

    /// <summary>
    /// 获取所有撤销的证书信息
    /// </summary>
    /// <returns>已撤销证书的序列号和撤销日期</returns>
    public IEnumerable<(string SerialNumber, DateTimeOffset RevokedDate)> GetRevokedCertificates()
    {
        var revokedCerts = this._crl.GetRevokedCertificates();
        if (revokedCerts == null)
            yield break;

        foreach (X509CrlEntry entry in revokedCerts)
        {
            var serialNumber = HexConverter.ToHexString(entry.SerialNumber.ToByteArrayUnsigned());
            var revokedDate = entry.RevocationDate;
            yield return (serialNumber, revokedDate);
        }
    }
}
