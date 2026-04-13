namespace Acme.Protocol.Const;

/// <summary>
/// HTTP Link 头的 rel 关系常量
/// </summary>
public static class LinkHeaderRelations
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    /// <summary>
    /// 指向目录或起始资源的链接
    /// </summary>
    public const string Index = "index";

    /// <summary>
    /// 服务条款链接
    /// </summary>
    public const string TermsOfService = "terms-of-service";

    /// <summary>
    /// 备用或等效资源链接
    /// </summary>
    public const string Alternate = "alternate";

    /// <summary>
    /// 上级资源链接
    /// </summary>
    public const string Up = "up";

    /// <summary>
    /// 下一资源或分页链接
    /// </summary>
    public const string Next = "next";
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
}
