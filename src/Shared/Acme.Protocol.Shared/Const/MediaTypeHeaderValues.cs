namespace Acme.Protocol.Const;

/// <summary>
/// 媒体类型常量
/// </summary>
public class MediaTypeHeaderValues
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    /// <summary>
    /// 标准 JSON 内容类型
    /// </summary>
    public const string JsonContentType = "application/json";

    /// <summary>
    /// JOSE(JWS) 封装的 JSON 内容类型
    /// </summary>
    public const string JoseContentType = "application/jose+json";

    /// <summary>
    /// RFC 7807 Problem Details 错误内容类型
    /// </summary>
    public const string JsonProblemContentType = "application/problem+json";

    /// <summary>
    /// PEM 编码的证书链
    /// </summary>
    public const string PemCertificateChainContentType = "application/pem-certificate-chain";

    /// <summary>
    /// DER/PKIX 单张证书
    /// </summary>
    public const string PkixCertContentType = "application/pkix-cert";

    /// <summary>
    /// PKCS #7 证书链容器
    /// </summary>
    public const string Pkcs7MimeContentType = "application/pkcs7-mime";

    /// <summary>
    /// 通用二进制流内容类型
    /// </summary>
    public const string OctContentType = "application/octet-stream";
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
}
