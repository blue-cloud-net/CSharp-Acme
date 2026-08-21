namespace Acme.Protocol.X509;

/// <summary>
/// X.509 证书链
/// </summary>
public sealed class CertificateChain
{
    /// <summary>
    /// 证书链中的证书列表
    /// </summary>
    public IReadOnlyList<Certificate> Certificates { get; private set; }

    /// <summary>
    /// 链中的证书数量
    /// </summary>
    public int Count => Certificates.Count;

    /// <summary>
    /// 获取证书链的主证书（第一个证书）
    /// </summary>
    public Certificate? PrimaryCertificate => Count > 0 ? Certificates[0] : null;

    /// <summary>
    /// 从证书列表初始化证书链
    /// </summary>
    /// <param name="certificates">证书列表</param>
    public CertificateChain(params Certificate[] certificates)
    {
        ArgumentNullException.ThrowIfNull(certificates, nameof(certificates));

        if (certificates.Length == 0)
            throw new ArgumentException("证书链必须至少包含一个证书", nameof(certificates));

        Certificates = Array.AsReadOnly(certificates);
    }

    /// <summary>
    /// 从证书列表初始化证书链
    /// </summary>
    /// <param name="certificates">证书列表</param>
    public CertificateChain(IList<Certificate> certificates)
    {
        ArgumentNullException.ThrowIfNull(certificates, nameof(certificates));

        if (certificates.Count == 0)
            throw new ArgumentException("证书链必须至少包含一个证书", nameof(certificates));

        Certificates = Array.AsReadOnly(certificates.ToArray());
    }

    /// <summary>
    /// 从 PEM 格式证书链字符串解析
    /// 支持多个证书的 PEM 格式字符串
    /// </summary>
    /// <param name="pemCertificateChain">包含一个或多个证书的 PEM 格式字符串</param>
    /// <returns>CertificateChain 实例</returns>
    public static CertificateChain Parse(string pemCertificateChain)
    {
        if (string.IsNullOrWhiteSpace(pemCertificateChain))
            throw new ArgumentException("证书链 PEM 数据不能为空", nameof(pemCertificateChain));

        // 使用 PemFormatter 拆分证书链
        var pemCertificates = PemFormatter.SplitPemCertificateChain(pemCertificateChain);

        if (pemCertificates.Length == 0)
            throw new InvalidOperationException("无法从 PEM 数据中解析出任何证书");

        var certificates = pemCertificates
            .Select(pemCert => Certificate.Parse(pemCert))
            .ToList();

        return new CertificateChain(certificates);
    }

    /// <summary>
    /// 将证书链转换为 PEM 格式字符串
    /// </summary>
    /// <returns>PEM 格式的证书链（多个证书连接）</returns>
    public string ToPem()
    {
        // 注意：Certificate类只包含元数据，无法直接转换回完整的PEM
        // 此方法返回证书信息的文本表示而非标准PEM格式
        var pemCerts = new System.Text.StringBuilder();
        foreach (var cert in Certificates)
        {
            pemCerts.AppendLine("-----BEGIN CERTIFICATE-----");
            pemCerts.AppendLine($"Subject: {cert.Subject}");
            pemCerts.AppendLine($"Issuer: {cert.Issuer}");
            pemCerts.AppendLine($"SerialNumber: {cert.SerialNumber}");
            pemCerts.AppendLine($"NotBefore: {cert.NotBefore:O}");
            pemCerts.AppendLine($"NotAfter: {cert.NotAfter:O}");
            pemCerts.AppendLine("-----END CERTIFICATE-----");
        }
        return pemCerts.ToString();
    }

    /// <summary>
    /// 获取证书链的字节数组列表
    /// </summary>
    /// <returns>每个证书的元数据字节表示</returns>
    public IEnumerable<byte[]> ToBytes()
    {
        // 注意：由于Certificate类只包含元数据，此方法返回元数据的UTF-8编码
        return Certificates.Select(cert =>
            System.Text.Encoding.UTF8.GetBytes(
                $"Subject:{cert.Subject}|Issuer:{cert.Issuer}|SerialNumber:{cert.SerialNumber}"));
    }

    /// <summary>
    /// 获取特定索引的证书
    /// </summary>
    /// <param name="index">证书索引</param>
    /// <returns>Certificate 实例</returns>
    public Certificate GetCertificate(int index)
    {
        if (index < 0 || index >= Certificates.Count)
            throw new IndexOutOfRangeException($"证书索引 {index} 超出范围 [0, {Certificates.Count - 1}]");

        return Certificates[index];
    }

    /// <summary>
    /// 枚举证书链中的所有证书
    /// </summary>
    /// <returns>证书枚举</returns>
    public IEnumerator<Certificate> GetEnumerator()
    {
        return Certificates.GetEnumerator();
    }
}
