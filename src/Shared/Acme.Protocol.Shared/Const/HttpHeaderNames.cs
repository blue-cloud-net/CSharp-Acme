namespace Acme.Protocol.Const;

/// <summary>
/// Http头名称常量
/// </summary>
public static class HttpHeaderNames
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    /// <summary>
    /// 客户端可接受的响应媒体类型
    /// </summary>
    public const string Accept = "Accept";

    /// <summary>
    /// 请求或响应主体的媒体类型
    /// </summary>
    public const string ContentType = "Content-Type";

    /// <summary>
    /// 关联资源的链接头
    /// </summary>
    public const string Link = "Link";

    /// <summary>
    /// 资源定位或重定向地址
    /// </summary>
    public const string Location = "Location";

    /// <summary>
    /// 建议客户端等待的时间或时间点
    /// </summary>
    public const string RetryAfter = "Retry-After";

    /// <summary>
    /// ACME 防重放随机数头
    /// </summary>
    public const string ReplayNonce = "Replay-Nonce";

    /// <summary>
    /// 客户端用户代理标识
    /// </summary>
    public const string UserAgent = "User-Agent";

    /// <summary>
    /// 反向代理传递的客户端真实 IP
    /// </summary>
    public const string X_Real_IP = "X-Real-IP";

    /// <summary>
    /// 代理链中记录的客户端 IP 列表
    /// </summary>
    public const string X_Forwarded_For = "X-Forwarded-For";
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
}
