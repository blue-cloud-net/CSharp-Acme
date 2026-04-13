namespace Acme.Protocol.X509;

/// <summary>
/// PEM头部常量
/// </summary>
public static class PemHeader
{
    #region CERTIFICATE

    /// <summary>
    /// 证书头部
    /// </summary>
    public const string CertHeader = "-----BEGIN CERTIFICATE-----";

    /// <summary>
    /// 证书尾部
    /// </summary>
    public const string CertFooter = "-----END CERTIFICATE-----";

    #endregion

    #region CSR

    /// <summary>
    /// CSR头部
    /// </summary>
    public const string CsrHeader = "-----BEGIN CERTIFICATE REQUEST-----";

    /// <summary>
    /// CSR尾部
    /// </summary>
    public const string CsrFooter = "-----END CERTIFICATE REQUEST-----";

    #endregion

    #region CRL

    /// <summary>
    /// CRL头部
    /// </summary>
    public const string CrlHeader = "-----BEGIN X509 CRL-----";

    /// <summary>
    /// CRL尾部
    /// </summary>
    public const string CrlFooter = "-----END X509 CRL-----";

    #endregion

    #region KEY

    /// <summary>
    /// 公钥头部
    /// </summary>
    public const string PublicKeyHeader = "-----BEGIN PUBLIC KEY-----";

    /// <summary>
    /// 公钥尾部
    /// </summary>
    public const string PublicKeyFooter = "-----END PUBLIC KEY-----";

    /// <summary>
    /// PKCS#8通用私钥头部
    /// </summary>
    public const string Pkcs8PrivateKeyHeader = "-----BEGIN PRIVATE KEY-----";

    /// <summary>
    /// PKCS#8通用私钥尾部
    /// </summary>
    public const string Pkcs8PrivateKeyFooter = "-----END PRIVATE KEY-----";

    /// <summary>
    /// 公钥头部
    /// </summary>
    public const string RsaPublicKeyHeader = "-----BEGIN RSA PUBLIC KEY-----";

    /// <summary>
    /// 公钥尾部
    /// </summary>
    public const string RsaPublicKeyFooter = "-----END RSA PUBLIC KEY-----";

    /// <summary>
    /// EC私钥头部
    /// </summary>
    public const string EcPrivateKeyHeader = "-----BEGIN EC PRIVATE KEY-----";

    /// <summary>
    /// EC私钥尾部
    /// </summary>
    public const string EcPrivateKeyFooter = "-----END EC PRIVATE KEY-----";

    /// <summary>
    /// RSA私钥头部
    /// </summary>
    public const string RsaPrivateKeyHeader = "-----BEGIN RSA PRIVATE KEY-----";

    /// <summary>
    /// RSA私钥尾部
    /// </summary>
    public const string RsaPrivateKeyFooter = "-----END RSA PRIVATE KEY-----";

    /// <summary>
    /// DSA私钥头部
    /// </summary>
    public const string DsaPrivateKeyHeader = "-----BEGIN DSA PRIVATE KEY-----";

    /// <summary>
    /// DSA私钥尾部
    /// </summary>
    public const string DsaPrivateKeyFooter = "-----END DSA PRIVATE KEY-----";

    #endregion
}
