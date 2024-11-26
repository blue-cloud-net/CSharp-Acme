using System.Net.Http.Headers;

namespace Acme.Const;

/// <summary>
/// 媒体类型常量
/// </summary>
public class MediaTypeHeaderValues

{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public const string JsonContentType = "application/json";

    public const string JoseContentType = "application/jose+json";
    public static readonly MediaTypeHeaderValue JoseMediaTypeHeaderValue = new(JoseContentType);

    public const string JsonProblemContentType = "application/problem+json";

    public const string PemContentType = "application/pem-certificate-chain";
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
}
