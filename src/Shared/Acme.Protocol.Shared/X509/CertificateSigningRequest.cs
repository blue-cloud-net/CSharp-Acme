namespace Acme.Protocol.X509;

/// <summary>
/// X.509 证书签名请求 (CSR/PKCS#10)
/// </summary>
public sealed class CertificateSigningRequest
{
    /// <summary>
    /// 主题
    /// </summary>
    public string Subject { get; private set; }

    /// <summary>
    /// 公钥信息
    /// </summary>
    public string PublicKeyInfo { get; private set; }

    /// <summary>
    /// 原始 PKCS#10 请求对象
    /// </summary>
    internal readonly Pkcs10CertificationRequest BcCsr;

    /// <summary>
    /// 从 PKCS#10 请求初始化
    /// </summary>
    /// <param name="csr">BouncyCastle PKCS#10 请求对象</param>
    public CertificateSigningRequest(Pkcs10CertificationRequest csr)
    {
        ArgumentNullException.ThrowIfNull(csr, nameof(csr));
        this.BcCsr = csr;

        this.Subject = csr.GetCertificationRequestInfo().Subject.ToString();
        this.PublicKeyInfo = ExtractPublicKeyInfo(csr);
    }

    /// <summary>
    /// 从 PEM 格式 CSR 字符串或 DER 格式字节数组解析
    /// </summary>
    /// <param name="pemCsr">PEM 格式 CSR 字符串</param>
    /// <returns>CertificateSigningRequest 实例</returns>
    public static CertificateSigningRequest Parse(string pemCsr)
    {
        ArgumentNullException.ThrowIfNull(pemCsr, nameof(pemCsr));
        
        var derBytes = PemFormatter.GetCsrBytes(pemCsr);
        var request = new Pkcs10CertificationRequest(derBytes);
        return new CertificateSigningRequest(request);
    }

    /// <summary>
    /// 从 DER 格式 CSR 字节数组解析
    /// </summary>
    /// <param name="derBytes">DER 格式 CSR 字节数组</param>
    /// <returns>CertificateSigningRequest 实例</returns>
    public static CertificateSigningRequest Parse(byte[] derBytes)
    {
        ArgumentNullException.ThrowIfNull(derBytes, nameof(derBytes));
        
        var request = new Pkcs10CertificationRequest(derBytes);
        return new CertificateSigningRequest(request);
    }

    /// <summary>
    /// 根据私钥生成证书签名请求 (CSR)
    /// </summary>
    /// <param name="privateKey">私钥</param>
    /// <param name="subjectName">主题名称 (例如: "CN=example.com,O=Example Org,C=US")</param>
    /// <param name="subjectAlternativeNames">主题备用名称列表 (可选)</param>
    /// <returns>CertificateSigningRequest 实例</returns>
    public static CertificateSigningRequest Generate(
        PrivateKey privateKey,
        string subjectName,
        string? signatureAlgorithmName = null,
        string[]? subjectAlternativeNames = null)
    {
        ArgumentNullException.ThrowIfNull(privateKey, nameof(privateKey));

        if (string.IsNullOrWhiteSpace(subjectName))
            throw new ArgumentException("主题名称不能为空", nameof(subjectName));

        // 获取公钥
        var publicKey = privateKey.ExtractPublicKey();

        // 获取签名算法
        signatureAlgorithmName ??= GetDefaultSignatureAlgorithmName(privateKey);

        // 创建主题DN
        var subject = new X509Name(subjectName);

        // 生成特性
        var x509ExtensionsGenerator = new X509ExtensionsGenerator();
        
        // 使用者可选名称 (SAN)
        if (subjectAlternativeNames is { Length: > 0 })
        {
            var subjectAltNames = new GeneralNames(
                subjectAlternativeNames
                    .Select(d =>
                        new GeneralName(
                            System.Net.IPAddress.TryParse(d, out _) ? GeneralName.IPAddress : GeneralName.DnsName,
                            d))
                    .ToArray());
            x509ExtensionsGenerator.AddExtension(
                X509Extensions.SubjectAlternativeName,
                false,
                subjectAltNames);
        }

        var attributes = GenerateAttributes(x509ExtensionsGenerator.Generate());

        // 创建BC CSR对象
        var bcCsr = new Pkcs10CertificationRequest(
            signatureAlgorithmName,
            subject,
            publicKey.BcKeyParameter,
            attributes,
            privateKey.BcKeyParameter
        );

        return new CertificateSigningRequest(bcCsr);
    }

    /// <summary>
    /// 验证CSR的有效性
    /// </summary>
    /// <returns>CSR是否有效</returns>
    public bool Verify()
    {
        try
        {
            return this.BcCsr.Verify();
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 获取公钥信息
    /// </summary>
    private static string ExtractPublicKeyInfo(Pkcs10CertificationRequest request)
    {
        var publicKeyInfo = request.GetPublicKey();
        var encoded = publicKeyInfo.GetEncoded();
        return HexConverter.ToHexString(encoded);
    }

    /// <summary>
    /// 获取签名算法字符串
    /// </summary>
    private static string GetDefaultSignatureAlgorithmName(PrivateKey privateKey)
    {
        return privateKey.BcKeyParameter switch
        {
            RsaPrivateCrtKeyParameters => "SHA256WithRSA",
            ECPrivateKeyParameters => "SHA256WithECDSA",
            DsaPrivateKeyParameters => "SHA256WithDSA",
            Ed25519PrivateKeyParameters => "Ed25519",
            _ => throw new NotSupportedException($"不支持的密钥类型: {privateKey.BcKeyParameter.GetType()}")
        };
    }

    /// <summary>
    /// 生成属性集合
    /// </summary>
    private static Asn1Set GenerateAttributes(X509Extensions? x509Extensions = null)
    {
        if (x509Extensions is not null)
        {
            var attribute = new AttributePkcs(
                PkcsObjectIdentifiers.Pkcs9AtExtensionRequest,
                new DerSet(x509Extensions));

            return new DerSet(attribute);
        }
        else
        {
            return new DerSet();
        }
    }
}
