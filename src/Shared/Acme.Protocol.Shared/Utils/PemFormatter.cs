namespace Acme.Protocol.Utils;

/// <summary>
/// PEM格式化工具类
/// </summary>
#if NET7_0_OR_GREATER
public static partial class PemFormatter
#else
public static class PemFormatter
#endif
{
    #region 证书

    /// <summary>
    /// 清理证书PEM格式中的头尾信息，合并成一行，只保留证书内容
    /// </summary>
    /// <param name="pemCert"></param>
    /// <returns></returns>
    public static string ClearCertificate(string pemCert)
    {
        ArgumentNullException.ThrowIfNull(pemCert, nameof(pemCert));

        var sb = new StringBuilder(pemCert);
        // 清理证书PEM格式中的头尾信息
        sb.Replace(PemHeader.CertHeader, null);
        sb.Replace(PemHeader.CertFooter, null);
        // 清理换行符和空白，合并成一行
        sb.Replace(" ", null);
        sb.Replace(LineBreakConstant.WindowsLineBreak, null);
        sb.Replace(LineBreakConstant.UnixLineBreak, null);
        sb.Replace(LineBreakConstant.MacLineBreak, null);
        return sb.ToString();
    }

    /// <summary>
    /// 获取证书的字节数组
    /// </summary>
    /// <param name="pemCert"></param>
    /// <returns></returns>
    public static byte[] GetCertificateBytes(string pemCert)
    {
        var base64Cert = ClearCertificate(pemCert);
        var bytes = Convert.FromBase64String(base64Cert);
        return bytes;
    }

    /// <summary>
    /// 格式化证书PEM格式，添加头尾信息，每行64个字符
    /// </summary>
    /// <param name="base64Cert"></param>
    /// <returns></returns>
    public static string FormatCertificate(string base64Cert)
    {
        ArgumentNullException.ThrowIfNull(base64Cert, nameof(base64Cert));

        var pemCert = ClearCertificate(base64Cert);

        var sb = new StringBuilder();
        sb.AppendLine(PemHeader.CertHeader);
        for (var i = 0; i < pemCert.Length; i += 64)
        {
            sb.AppendLine(pemCert.Substring(i, Math.Min(64, pemCert.Length - i)));
        }

        sb.AppendLine(PemHeader.CertFooter);

        return sb.ToString();
    }

    public static string FormatCertificates(X509Certificate certificate)
    {
        ArgumentNullException.ThrowIfNull(certificate, nameof(certificate));
        return CertificateBytesToPem(certificate.GetEncoded());
    }

    /// <summary>
    /// 将证书字节数组转换为PEM格式
    /// </summary>
    /// <param name="certificateBytes">证书字节数组</param>
    /// <returns>PEM格式的证书</returns>
    public static string CertificateBytesToPem(byte[] certificateBytes)
    {
        ArgumentNullException.ThrowIfNull(certificateBytes, nameof(certificateBytes));
        var base64Cert = Convert.ToBase64String(certificateBytes);
        return FormatCertificate(base64Cert);
    }

    /// <summary>
    /// 拆分证书链（包含多个证书的PEM格式字符串）为单个证书列表
    /// </summary>
    /// <param name="certificateChain">包含一个或多个证书的PEM格式字符串</param>
    /// <returns>单个证书的PEM格式列表</returns>
    public static string[] SplitPemCertificateChain(string certificateChain)
    {
        ArgumentNullException.ThrowIfNull(certificateChain, nameof(certificateChain));

        var certificates = new List<string>();
        var pattern = CertificateChainPattern();

        var matches = pattern.Matches(certificateChain);
        foreach (Match match in matches)
        {
            var cert = match.Value.Trim();
            if (!string.IsNullOrWhiteSpace(cert))
            {
                certificates.Add(cert);
            }
        }

        return certificates.ToArray();
    }

#if NETSTANDARD2_0 || NETSTANDARD2_1
    /// <summary>
    /// 用于匹配PEM格式的证书的正则表达式
    /// </summary>
    private static readonly Regex _certificateChainPattern = new($@"{PemHeader.CertHeader}.*?{PemHeader.CertFooter}", RegexOptions.Singleline | RegexOptions.Compiled);

    private static Regex CertificateChainPattern() => _certificateChainPattern;
#else
    /// <summary>
    /// 生成的正则表达式模式，用于匹配PEM格式的证书
    /// </summary>
    [GeneratedRegex($@"{PemHeader.CertHeader}.*?{PemHeader.CertFooter}", RegexOptions.Singleline)]
    private static partial Regex CertificateChainPattern();
#endif

    #endregion

    #region CSR

    /// <summary>
    /// 清理CSR的PEM格式中的头尾信息，合并成一行，只保留CSR内容
    /// </summary>
    /// <param name="pemCsr"></param>
    /// <returns></returns>
    public static string CleanCsr(string pemCsr)
    {
        ArgumentNullException.ThrowIfNull(pemCsr, nameof(pemCsr));
        var sb = new StringBuilder(pemCsr);
        // 清理CSR的PEM格式中的头尾信息
        sb.Replace(PemHeader.CsrHeader, String.Empty);
        sb.Replace(PemHeader.CsrFooter, String.Empty);
        // 清理换行符和空白，合并成一行
        sb.Replace(" ", String.Empty);
        sb.Replace(LineBreakConstant.WindowsLineBreak, String.Empty);
        sb.Replace(LineBreakConstant.UnixLineBreak, String.Empty);
        sb.Replace(LineBreakConstant.MacLineBreak, String.Empty);
        return sb.ToString();
    }

    /// <summary>
    /// 获取CSR的字节数组
    /// </summary>
    /// <param name="pemCsr"></param>
    /// <returns></returns>
    public static byte[] GetCsrBytes(string pemCsr)
    {
        var base64Csr = CleanCsr(pemCsr);
        var bytes = Convert.FromBase64String(base64Csr);
        return bytes;
    }

    /// <summary>
    /// 格式化CSR的PEM格式，添加头尾信息，每行64个字符
    /// </summary>
    /// <param name="base64Csr"></param>
    /// <returns></returns>
    public static string FormatCsr(string base64Csr)
    {
        ArgumentNullException.ThrowIfNull(base64Csr, nameof(base64Csr));
        var pemCert = CleanCsr(base64Csr);

        var sb = new StringBuilder();
        sb.AppendLine(PemHeader.CsrHeader);
        for (var i = 0; i < pemCert.Length; i += 64)
        {
            sb.AppendLine(pemCert.Substring(i, Math.Min(64, pemCert.Length - i)));
        }

        sb.AppendLine(PemHeader.CsrFooter);

        return sb.ToString();
    }

    public static string FormatCsr(Pkcs10CertificationRequest csr)
    {
        ArgumentNullException.ThrowIfNull(csr, nameof(csr));
        return CsrBytesToPem(csr.GetEncoded());
    }

    /// <summary>
    /// 将CSR字节数组转换为PEM格式
    /// </summary>
    /// <param name="csrBytes">CSR字节数组</param>
    /// <returns>PEM格式的CSR</returns>
    public static string CsrBytesToPem(byte[] csrBytes)
    {
        ArgumentNullException.ThrowIfNull(csrBytes, nameof(csrBytes));
        var base64Csr = Convert.ToBase64String(csrBytes);
        return FormatCsr(base64Csr);
    }

    #endregion

    #region KEY

    /// <summary>
    /// 将RSA私钥字节数组转换为PEM格式
    /// </summary>
    /// <param name="privateKeyBytes">RSA私钥字节数组</param>
    /// <returns>PEM格式的RSA私钥</returns>
    public static string RsaPrivateKeyBytesToPem(byte[] privateKeyBytes)
    {
        ArgumentNullException.ThrowIfNull(privateKeyBytes, nameof(privateKeyBytes));
        var base64Key = Convert.ToBase64String(privateKeyBytes);
        return FormatKey(base64Key, PemHeader.RsaPrivateKeyHeader, PemHeader.RsaPrivateKeyFooter);
    }

    /// <summary>
    /// 将EC私钥字节数组转换为PEM格式
    /// </summary>
    /// <param name="privateKeyBytes">EC私钥字节数组</param>
    /// <returns>PEM格式的EC私钥</returns>
    public static string EcPrivateKeyBytesToPem(byte[] privateKeyBytes)
    {
        ArgumentNullException.ThrowIfNull(privateKeyBytes, nameof(privateKeyBytes));
        var base64Key = Convert.ToBase64String(privateKeyBytes);
        return FormatKey(base64Key, PemHeader.EcPrivateKeyHeader, PemHeader.EcPrivateKeyFooter);
    }

    /// <summary>
    /// 将DSA私钥字节数组转换为PEM格式
    /// </summary>
    /// <param name="privateKeyBytes">DSA私钥字节数组</param>
    /// <returns>PEM格式的DSA私钥</returns>
    public static string DsaPrivateKeyBytesToPem(byte[] privateKeyBytes)
    {
        ArgumentNullException.ThrowIfNull(privateKeyBytes, nameof(privateKeyBytes));
        var base64Key = Convert.ToBase64String(privateKeyBytes);
        return FormatKey(base64Key, PemHeader.DsaPrivateKeyHeader, PemHeader.DsaPrivateKeyFooter);
    }

    /// <summary>
    /// 将PKCS#8私钥字节数组转换为PEM格式
    /// </summary>
    /// <param name="privateKeyBytes"></param>
    /// <returns></returns>
    public static string Pkcs8PrivateKeyBytesToPem(byte[] privateKeyBytes)
    {
        ArgumentNullException.ThrowIfNull(privateKeyBytes, nameof(privateKeyBytes));
        var base64Key = Convert.ToBase64String(privateKeyBytes);
        return FormatKey(base64Key, PemHeader.Pkcs8PrivateKeyHeader, PemHeader.Pkcs8PrivateKeyFooter);
    }

    /// <summary>
    /// 将PKCS#8公钥字节数组转换为PEM格式
    /// </summary>
    /// <param name="publicKeyBytes">公钥字节数组</param>
    /// <returns>PEM格式的公钥</returns>
    public static string Pkcs8PublicKeyBytesToPem(byte[] publicKeyBytes)
    {
        ArgumentNullException.ThrowIfNull(publicKeyBytes, nameof(publicKeyBytes));
        var base64Key = Convert.ToBase64String(publicKeyBytes);
        return FormatKey(base64Key, PemHeader.PublicKeyHeader, PemHeader.PublicKeyFooter);
    }

    /// <summary>
    /// 将RSA公钥字节数组转换为PEM格式
    /// </summary>
    /// <param name="publicKeyBytes">RSA公钥字节数组</param>
    /// <returns>PEM格式的RSA公钥</returns>
    public static string RsaPublicKeyBytesToPem(byte[] publicKeyBytes)
    {
        ArgumentNullException.ThrowIfNull(publicKeyBytes, nameof(publicKeyBytes));
        var base64Key = Convert.ToBase64String(publicKeyBytes);
        return FormatKey(base64Key, PemHeader.RsaPublicKeyHeader, PemHeader.RsaPublicKeyFooter);
    }

    /// <summary>
    /// 清理密钥PEM格式中的头尾信息，合并成一行，只保留密钥内容
    /// </summary>
    /// <param name="pemKey"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static string CleanKey(string pemKey, out string header)
    {
        ArgumentNullException.ThrowIfNull(pemKey, nameof(pemKey));

        var sb = new StringBuilder(pemKey);

        if (pemKey.StartsWith(PemHeader.Pkcs8PrivateKeyHeader))
        {
            sb.Replace(PemHeader.Pkcs8PrivateKeyHeader, String.Empty);
            sb.Replace(PemHeader.Pkcs8PrivateKeyFooter, String.Empty);
            header = PemHeader.Pkcs8PrivateKeyHeader;
        }
        else if (pemKey.StartsWith(PemHeader.PublicKeyHeader))
        {
            sb.Replace(PemHeader.PublicKeyHeader, String.Empty);
            sb.Replace(PemHeader.PublicKeyFooter, String.Empty);
            header = PemHeader.PublicKeyHeader;
        }
        else if (pemKey.StartsWith(PemHeader.RsaPrivateKeyHeader))
        {
            sb.Replace(PemHeader.RsaPrivateKeyHeader, String.Empty);
            sb.Replace(PemHeader.RsaPrivateKeyFooter, String.Empty);
            header = PemHeader.RsaPrivateKeyHeader;
        }
        else if (pemKey.StartsWith(PemHeader.EcPrivateKeyHeader))
        {
            sb.Replace(PemHeader.EcPrivateKeyHeader, String.Empty);
            sb.Replace(PemHeader.EcPrivateKeyFooter, String.Empty);
            header = PemHeader.EcPrivateKeyHeader;
        }
        else if (pemKey.StartsWith(PemHeader.DsaPrivateKeyHeader))
        {
            sb.Replace(PemHeader.DsaPrivateKeyHeader, String.Empty);
            sb.Replace(PemHeader.DsaPrivateKeyFooter, String.Empty);
            header = PemHeader.DsaPrivateKeyHeader;
        }
        else if (pemKey.StartsWith(PemHeader.RsaPublicKeyHeader))
        {
            sb.Replace(PemHeader.RsaPublicKeyHeader, String.Empty);
            sb.Replace(PemHeader.RsaPublicKeyFooter, String.Empty);
            header = PemHeader.RsaPublicKeyHeader;
        }
        else
        {
            header = String.Empty;
        }

        // 清理换行符和空白，合并成一行
        sb.Replace(" ", String.Empty);
        sb.Replace(LineBreakConstant.WindowsLineBreak, String.Empty);
        sb.Replace(LineBreakConstant.UnixLineBreak, String.Empty);
        sb.Replace(LineBreakConstant.MacLineBreak, String.Empty);
        return sb.ToString();
    }

    /// <summary>
    /// 获取私钥的字节数组
    /// </summary>
    /// <param name="pemKey"></param>
    /// <returns></returns>
    public static byte[] GetKeyBytes(string pemKey)
    {
        var base64Key = CleanKey(pemKey, out _);
        var bytes = Convert.FromBase64String(base64Key);
        return bytes;
    }

    /// <summary>
    /// 格式化密钥为PEM格式，根据密钥类型和格式要求自动选择合适的PEM头尾
    /// </summary>
    /// <param name="key">密钥参数</param>
    /// <param name="pkcs8">是否使用PKCS#8格式，默认为true</param>
    /// <returns>PEM格式的密钥字符串</returns>
    public static string FormatKey(AsymmetricKeyParameter key, bool pkcs8 = true)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        if (key.IsPrivate)
        {
            // 私钥处理
            if (pkcs8)
            {
                // 使用PKCS#8格式
                var privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(key);
                var keyBytes = privateKeyInfo.GetEncoded();
                var base64Key = Convert.ToBase64String(keyBytes);
                return FormatKey(base64Key, PemHeader.Pkcs8PrivateKeyHeader, PemHeader.Pkcs8PrivateKeyFooter);
            }
            else
            {
                // 使用传统格式
                return key switch
                {
                    RsaPrivateCrtKeyParameters rsaKey =>
                        FormatRsaPrivateKeyTraditional(rsaKey),
                    ECPrivateKeyParameters ecKey =>
                        FormatEcPrivateKeyTraditional(ecKey),
                    DsaPrivateKeyParameters dsaKey =>
                        FormatDsaPrivateKeyTraditional(dsaKey),
                    _ => throw new NotSupportedException($"不支持的私钥类型: {key.GetType()}")
                };
            }
        }
        else
        {
            // 公钥处理
            var publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(key);
            var keyBytes = publicKeyInfo.GetEncoded();
            var base64Key = Convert.ToBase64String(keyBytes);

            if (key is RsaKeyParameters && !pkcs8)
            {
                // RSA公钥使用传统格式
                return FormatKey(base64Key, PemHeader.RsaPublicKeyHeader, PemHeader.RsaPublicKeyFooter);
            }
            else
            {
                // 其他公钥或要求PKCS#8格式
                return FormatKey(base64Key, PemHeader.PublicKeyHeader, PemHeader.PublicKeyFooter);
            }
        }
    }

    /// <summary>
    /// 格式化RSA私钥为传统PKCS#1格式
    /// </summary>
    /// <param name="rsaKey">RSA私钥参数</param>
    /// <returns>PKCS#1格式的RSA私钥PEM</returns>
    private static string FormatRsaPrivateKeyTraditional(RsaPrivateCrtKeyParameters rsaKey)
    {
        var rsaPrivateKey = new RsaPrivateKeyStructure(
            rsaKey.Modulus,
            rsaKey.PublicExponent,
            rsaKey.Exponent,
            rsaKey.P,
            rsaKey.Q,
            rsaKey.DP,
            rsaKey.DQ,
            rsaKey.QInv);

        var keyBytes = rsaPrivateKey.GetEncoded();
        var base64Key = Convert.ToBase64String(keyBytes);
        return FormatKey(base64Key, PemHeader.RsaPrivateKeyHeader, PemHeader.RsaPrivateKeyFooter);
    }

    /// <summary>
    /// 格式化EC私钥为传统SEC1格式
    /// </summary>
    /// <param name="ecKey">EC私钥参数</param>
    /// <returns>SEC1格式的EC私钥PEM</returns>
    private static string FormatEcPrivateKeyTraditional(ECPrivateKeyParameters ecKey)
    {
        // 直接使用PrivateKeyInfoFactory创建，然后提取内部的私钥部分
        var privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(ecKey);
        var keyBytes = privateKeyInfo.GetEncoded();
        var base64Key = Convert.ToBase64String(keyBytes);
        return FormatKey(base64Key, PemHeader.EcPrivateKeyHeader, PemHeader.EcPrivateKeyFooter);
    }

    /// <summary>
    /// 格式化DSA私钥为传统格式
    /// </summary>
    /// <param name="dsaKey">DSA私钥参数</param>
    /// <returns>传统格式的DSA私钥PEM</returns>
    private static string FormatDsaPrivateKeyTraditional(DsaPrivateKeyParameters dsaKey)
    {
        // DSA私钥的传统格式比较复杂，这里使用PKCS#8作为后备
        var privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(dsaKey);
        var keyBytes = privateKeyInfo.GetEncoded();
        var base64Key = Convert.ToBase64String(keyBytes);
        return FormatKey(base64Key, PemHeader.DsaPrivateKeyHeader, PemHeader.DsaPrivateKeyFooter);
    }

    /// <summary>
    /// 格式化密钥PEM格式的通用方法，添加头尾信息，每行64个字符
    /// </summary>
    /// <param name="base64Key">Base64编码的密钥</param>
    /// <param name="header">PEM头部</param>
    /// <param name="footer">PEM尾部</param>
    /// <returns>格式化后的PEM字符串</returns>
    private static string FormatKey(string base64Key, string header, string footer)
    {
        ArgumentNullException.ThrowIfNull(base64Key, nameof(base64Key));
        ArgumentNullException.ThrowIfNull(header, nameof(header));
        ArgumentNullException.ThrowIfNull(footer, nameof(footer));

        // 清理输入的base64字符串，移除换行符和空白
        var cleanBase64 = base64Key
            .Replace(" ", "")
            .Replace(LineBreakConstant.WindowsLineBreak, "")
            .Replace(LineBreakConstant.UnixLineBreak, "")
            .Replace(LineBreakConstant.MacLineBreak, "");

        var sb = new StringBuilder();
        sb.AppendLine(header);

        // 每行64个字符
        for (var i = 0; i < cleanBase64.Length; i += 64)
        {
            sb.AppendLine(cleanBase64.Substring(i, Math.Min(64, cleanBase64.Length - i)));
        }

        sb.AppendLine(footer);
        return sb.ToString();
    }

    /// <summary>
    /// 通用密钥字节数组转PEM格式，自动识别算法和PKCS格式
    /// </summary>
    /// <param name="keyBytes">密钥字节数组</param>
    /// <param name="pkcs8">是否为PKCS#8格式</param>
    /// <returns>PEM格式密钥</returns>
    public static string KeyBytesToPem(byte[] keyBytes, bool pkcs8 = true)
    {
        ArgumentNullException.ThrowIfNull(keyBytes, nameof(keyBytes));

        try
        {
            // 尝试解byte数组为私钥
            var privateKeyInfo = PrivateKeyInfo.GetInstance(keyBytes);

            if (pkcs8)
            {
                // PKCS#8通用私钥
                return FormatKey(
                    Convert.ToBase64String(keyBytes),
                    PemHeader.Pkcs8PrivateKeyHeader,
                    PemHeader.Pkcs8PrivateKeyFooter);
            }

            // 非pkcs8根据算法类型返回不同的PEM格式
            var algOid = privateKeyInfo.PrivateKeyAlgorithm.Algorithm.Id;
            if (algOid == PkcsObjectIdentifiers.RsaEncryption.Id)
            {
                // PKCS#1 RSA私钥
                return FormatKey(
                    Convert.ToBase64String(keyBytes),
                    PemHeader.RsaPrivateKeyHeader,
                    PemHeader.RsaPrivateKeyFooter);
            }
            else if (algOid == SecObjectIdentifiers.SecP256r1.Id
                     || algOid == X9ObjectIdentifiers.IdECPublicKey.Id)
            {
                // SEC1 EC私钥
                return FormatKey(
                    Convert.ToBase64String(keyBytes),
                    PemHeader.EcPrivateKeyHeader,
                    PemHeader.EcPrivateKeyFooter);
            }
            else if (algOid == X9ObjectIdentifiers.IdDsa.Id)
            {
                // DSA私钥
                return FormatKey(
                    Convert.ToBase64String(keyBytes),
                    PemHeader.DsaPrivateKeyHeader,
                    PemHeader.DsaPrivateKeyFooter);
            }
        }
        catch
        {
            /* 不是私钥，继续尝试其他格式 */
        }

        try
        {
            // 尝试解析为公钥
            var publicKeyInfo = SubjectPublicKeyInfo.GetInstance(keyBytes);
            var algOid = publicKeyInfo.Algorithm.Algorithm.Id;

            if (algOid == PkcsObjectIdentifiers.RsaEncryption.Id)
            {
                return FormatKey(
                    Convert.ToBase64String(keyBytes),
                    PemHeader.RsaPublicKeyHeader,
                    PemHeader.RsaPublicKeyFooter);
            }

            return FormatKey(
                Convert.ToBase64String(keyBytes),
                PemHeader.PublicKeyHeader,
                PemHeader.PublicKeyFooter);
        }
        catch
        {
            // 下面抛出异常
        }

        throw new InvalidCastException("无法识别的密钥格式或算法。");
    }

    #endregion
}