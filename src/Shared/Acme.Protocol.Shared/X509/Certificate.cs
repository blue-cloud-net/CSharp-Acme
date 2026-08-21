using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Math.EC.Multiplier;

namespace Acme.Protocol.X509;

/// <summary>
/// X.509 证书信息
/// </summary>
public sealed class Certificate
{
    /// <summary>
    /// 主题
    /// </summary>
    public string Subject { get; private set; }

    /// <summary>
    /// 主题密钥标识
    /// </summary>
    public string SubjectKeyIdentifier { get; private set; }

    /// <summary>
    /// 颁发者
    /// </summary>
    public string Issuer { get; private set; }

    /// <summary>
    /// 颁发者密钥标识
    /// </summary>
    public string AuthorityKeyIdentifier { get; private set; }

    /// <summary>
    /// 序列号
    /// </summary>
    public string SerialNumber { get; private set; }

    /// <summary>
    /// 生效时间
    /// </summary>
    public DateTimeOffset NotBefore { get; private set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTimeOffset NotAfter { get; private set; }

    /// <summary>
    /// 证书有效期天数
    /// </summary>
    public long Duration => (long)Math.Ceiling((this.NotAfter - this.NotBefore).TotalDays);

    /// <summary>
    /// 是否已过期
    /// </summary>
    public bool IsExpired => DateTimeOffset.UtcNow > this.NotAfter;

    /// <summary>
    /// 剩余有效天数
    /// </summary>
    public double ExpiryDays => (this.NotAfter - DateTimeOffset.UtcNow).TotalDays;
    
    internal X509Certificate BcCertificate { get; set; }

    /// <summary>
    /// 从 X509Certificate 初始化证书信息
    /// </summary>
    /// <param name="x509Certificate">BouncyCastle X509Certificate</param>
    public Certificate(
        X509Certificate x509Certificate)
    {
        ArgumentNullException.ThrowIfNull(x509Certificate, nameof(x509Certificate));
        this.BcCertificate = x509Certificate;
        
        this.Subject = x509Certificate.SubjectDN.ToString();
        this.SubjectKeyIdentifier = ExtractSubjectKeyIdentifier(x509Certificate);
        this.Issuer = x509Certificate.IssuerDN.ToString();
        this.AuthorityKeyIdentifier = ExtractAuthorityKeyIdentifier(x509Certificate);
        this.SerialNumber = ExtractSerialNumber(x509Certificate);
        this.NotBefore = x509Certificate.NotBefore;
        this.NotAfter = x509Certificate.NotAfter;
    }

    public PublicKey GetPublicKey()
    {
        var publicKey = this.BcCertificate.GetPublicKey();
        return new PublicKey(publicKey);
    }

    /// <summary>
    /// 从 PEM 格式证书字符串或 DER 格式字节数组解析证书
    /// </summary>
    /// <param name="pemCert">PEM 格式证书字符串</param>
    /// <returns>Certificate 实例</returns>
    public static Certificate Parse(string pemCert)
    {
        ArgumentNullException.ThrowIfNull(pemCert, nameof(pemCert));
        
        var derBytes = PemFormatter.GetCertificateBytes(pemCert);
        var x509Certificate = new X509Certificate(derBytes);
        return new Certificate(x509Certificate);
    }

    /// <summary>
    /// 从 DER 格式证书字节数组解析证书
    /// </summary>
    /// <param name="derBytes">DER 格式证书字节数组</param>
    /// <returns>Certificate 实例</returns>
    public static Certificate Parse(byte[] derBytes)
    {
        ArgumentNullException.ThrowIfNull(derBytes, nameof(derBytes));
        
        var x509Certificate = new X509Certificate(derBytes);
        return new Certificate(x509Certificate);
    }

    /// <summary>
    /// 根据CSR和CA私钥签发新证书
    /// </summary>
    /// <param name="csr">证书签名请求</param>
    /// <param name="caPrivateKey">CA私钥</param>
    /// <param name="caSubject">CA主题名称</param>
    /// <param name="serialNumber">证书序列号</param>
    /// <param name="notBefore">证书生效时间</param>
    /// <param name="notAfter">证书过期时间</param>
    /// <returns>签发的Certificate实例</returns>
    public static Certificate IssueCertificate(
        CertificateSigningRequest csr,
        PrivateKey caPrivateKey,
        string caSubject,
        BigInteger serialNumber,
        DateTime notBefore,
        DateTime notAfter)
    {
        ArgumentNullException.ThrowIfNull(csr, nameof(csr));
        ArgumentNullException.ThrowIfNull(caPrivateKey, nameof(caPrivateKey));

        var bcCsr = csr.BcCsr;
        var caPrivateKeyParam = caPrivateKey.BcKeyParameter;

        // 获取CSR信息
        var certInfo = bcCsr.GetCertificationRequestInfo();
        var subject = certInfo.Subject;
        var publicKeyInfo = bcCsr.GetPublicKey();

        // 创建证书构建器
        var certBuilder = new X509V3CertificateGenerator();
        certBuilder.SetSerialNumber(serialNumber);
        certBuilder.SetIssuerDN(new X509Name(caSubject));
        certBuilder.SetSubjectDN(subject);
        certBuilder.SetNotBefore(notBefore);
        certBuilder.SetNotAfter(notAfter);
        certBuilder.SetPublicKey(publicKeyInfo);

        // 添加基本约束和密钥标识扩展
        certBuilder.AddExtension(X509Extensions.BasicConstraints, true, new BasicConstraints(false));
        certBuilder.AddExtension(X509Extensions.SubjectKeyIdentifier, false,
            X509ExtensionUtilities.CreateSubjectKeyIdentifier(publicKey.BcKeyParameter));
        certBuilder.AddExtension(X509Extensions.AuthorityKeyIdentifier, false,
            X509ExtensionUtilities.CreateSubjectKeyIdentifier(publicKey.BcKeyParameter));

        // 复制CSR中的扩展
        var csrExtensions = certInfo.GetAttributes();
        foreach (var attr in csrExtensions)
        {
            if (attr.AttrType.Equals(PkcsObjectIdentifiers.Pkcs9AtExtensionRequest))
            {
                var extensions = X509Extensions.GetInstance(attr.AttrValues[0]);
                foreach (var extensionOid in extensions.GetExtensionOids())
                {
                    var extension = extensions.GetExtension(extensionOid);
                    if (!extensionOid.Equals(X509Extensions.BasicConstraints) &&
                        !extensionOid.Equals(X509Extensions.SubjectKeyIdentifier) &&
                        !extensionOid.Equals(X509Extensions.AuthorityKeyIdentifier))
                    {
                        certBuilder.AddExtension(extensionOid, extension.IsCritical, extension.GetParsedValue());
                    }
                }
            }
        }

        // 选择签名算法
        var signatureAlgorithm = GetSignatureAlgorithmName(caPrivateKeyParam);

        // 生成证书
        var x509Cert = certBuilder.Generate(new Asn1SignatureFactory(signatureAlgorithm, caPrivateKeyParam));
        return new Certificate(x509Cert);
    }

    /// <summary>
    /// 生成自签证书
    /// </summary>
    /// <param name="privateKey">私钥</param>
    /// <param name="subjectName">主题名称</param>
    /// <param name="serialNumber">证书序列号</param>
    /// <param name="notBefore">证书生效时间</param>
    /// <param name="notAfter">证书过期时间</param>
    /// <returns>自签的Certificate实例</returns>
    public static Certificate GenerateSelfSigned(
        PrivateKey privateKey,
        string subjectName,
        BigInteger serialNumber,
        DateTime notBefore,
        DateTime notAfter)
    {
        ArgumentNullException.ThrowIfNull(privateKey, nameof(privateKey));
        if (string.IsNullOrWhiteSpace(subjectName))
            throw new ArgumentException("主题名称不能为空", nameof(subjectName));

        var subject = new X509Name(subjectName);

        // 获取公钥
        var publicKey = privateKey.ExtractPublicKey();

        // 创建证书构建器
        var certBuilder = new X509V3CertificateGenerator();
        certBuilder.SetSerialNumber(serialNumber);
        certBuilder.SetIssuerDN(subject);
        certBuilder.SetSubjectDN(subject);
        certBuilder.SetNotBefore(notBefore);
        certBuilder.SetNotAfter(notAfter);
        certBuilder.SetPublicKey(privateKey.ExtractPublicKey().BcKeyParameter);

        // 添加扩展
        certBuilder.AddExtension(X509Extensions.BasicConstraints, true, new BasicConstraints(true));
        certBuilder.AddExtension(X509Extensions.SubjectKeyIdentifier, false,
            X509ExtensionUtilities.CreateSubjectKeyIdentifier(publicKey.BcKeyParameter));
        certBuilder.AddExtension(X509Extensions.AuthorityKeyIdentifier, false,
            X509ExtensionUtilities.CreateSubjectKeyIdentifier(publicKey.BcKeyParameter));

        // 选择签名算法
        var privateKeyParam = privateKey.BcKeyParameter;
        var signatureAlgorithm = GetSignatureAlgorithmName(privateKeyParam);

        // 生成证书
        var x509Cert = certBuilder.Generate(new Asn1SignatureFactory(signatureAlgorithm, privateKeyParam));
        return new Certificate(x509Cert);
    }


    /// <summary>
    /// 获取证书的主题密钥标识
    /// </summary>
    private static string ExtractSubjectKeyIdentifier(X509Certificate certificate)
    {
        var asn1Object = certificate.GetExtensionValue(X509Extensions.SubjectKeyIdentifier);
        if (asn1Object is null)
            return string.Empty;

        var extensionValue = X509ExtensionUtilities.FromExtensionValue(asn1Object);
        var subjectKeyIdentifierExtension = Org.BouncyCastle.Asn1.X509.SubjectKeyIdentifier.GetInstance(extensionValue);
        var keyIdentifier = subjectKeyIdentifierExtension.GetKeyIdentifier();

        return HexConverter.ToHexString(keyIdentifier);
    }

    /// <summary>
    /// 获取证书的颁发者密钥标识
    /// </summary>
    private static string ExtractAuthorityKeyIdentifier(X509Certificate certificate)
    {
        var asn1Object = certificate.GetExtensionValue(X509Extensions.AuthorityKeyIdentifier);
        if (asn1Object is null)
            return string.Empty;

        var extensionValue = X509ExtensionUtilities.FromExtensionValue(asn1Object);
        var authorityKeyIdentifierExtension =
            Org.BouncyCastle.Asn1.X509.AuthorityKeyIdentifier.GetInstance(extensionValue);
        var keyIdentifier = authorityKeyIdentifierExtension.GetKeyIdentifier();

        return HexConverter.ToHexString(keyIdentifier);
    }

    /// <summary>
    /// 获取序列号
    /// </summary>
    private static string ExtractSerialNumber(X509Certificate certificate)
    {
        var serialNumber = certificate.SerialNumber.ToByteArrayUnsigned();
        return HexConverter.ToHexString(serialNumber);
    }

    /// <summary>
    /// 获取签名算法名称
    /// </summary>
    private static string GetSignatureAlgorithmName(AsymmetricKeyParameter keyParameter)
    {
        return keyParameter switch
        {
            RsaPrivateCrtKeyParameters => "SHA256WithRSA",
            ECPrivateKeyParameters => "SHA256WithECDSA",
            DsaPrivateKeyParameters => "SHA256WithDSA",
            _ => throw new NotSupportedException($"不支持的密钥类型: {keyParameter.GetType()}")
        };
    }


}