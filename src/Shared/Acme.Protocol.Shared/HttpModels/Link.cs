using Acme.Protocol.Resources;

using System.Diagnostics.CodeAnalysis;

namespace Acme.Protocol.HttpModels;

/// <summary>
/// HTTP Link 头模型，表示 HTTP 响应头中的 Link 关系
/// <para>包含 URL 和关系类型 (rel)，用于表示资源之间的关系</para>
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.1"/> 和 <see href="https://datatracker.ietf.org/doc/html/rfc8288"/>
/// </summary>
#if NET7_0_OR_GREATER
public partial class Link
#else
public class Link
#endif
{
    /// <summary>
    /// 初始化 Link 实例
    /// </summary>
    /// <param name="url">链接的目标 URL</param>
    /// <param name="rel">关系类型（如 "index"、"terms-of-service" 等）</param>
    public Link(Uri url, string rel)
    {
        this.Url = url;
        this.Relation = rel;
    }

    /// <summary>
    /// 从字符串初始化 Link 实例
    /// </summary>
    /// <param name="url">链接的目标 URL 字符串</param>
    /// <param name="rel">关系类型（如 "index"、"terms-of-service" 等）</param>
    public Link(string url, string rel) : this(new Uri(url), rel)
    {
    }

    /// <summary>
    /// 链接的目标 URL
    /// </summary>
    public Uri Url { get; set; }

    /// <summary>
    /// 关系类型，描述当前资源与目标资源的关系
    /// <para>常见值包括："index"（目录）、"terms-of-service"（服务条款）、"alternate"（备用资源）、"up"（上级资源）、"next"（下一资源）</para>
    /// </summary>
    public string Relation { get; set; }

    /// <summary>
    /// 转换为 HTTP Link 头格式字符串
    /// <para>格式：&lt;url&gt;;rel="relation"</para>
    /// </summary>
    /// <returns>符合 RFC 8288 规范的 HTTP Link 头字符串</returns>
    public override string ToString()
    {
        return $"<{this.Url}>;rel=\"{this.Relation}\"";
    }

    /// <summary>
    /// 从 HTTP Link 头字符串解析为 Link 对象集合
    /// <para>示例：&lt;https://example.com/directory&gt;;rel="index"</para>
    /// </summary>
    /// <param name="linkHeader">HTTP Link 头字符串，多个链接用逗号分隔</param>
    /// <returns>解析后的 Link 对象集合</returns>
    /// <exception cref="ArgumentException">当 Link 头格式无效时抛出</exception>
    public static IEnumerable<Link> Parse(string linkHeader)
    {
        if (string.IsNullOrWhiteSpace(linkHeader))
        {
            throw new ArgumentException(RS.LinkHeaderNullOrEmpty, nameof(linkHeader));
        }

        var links = new List<Link>(1);

        // 一个Link头可能包含多个链接,用逗号分隔
        var parts = linkHeader.Trim().Split(',');
        foreach (var part in parts)
        {
            var match = LinkHeaderRegex().Match(part.Trim());
            if (match.Success)
            {
                var url = new Uri(match.Groups[1].Value);
                var relation = match.Groups[2].Value;
                links.Add(new(url, relation));
            }
            else
            {
                throw new ArgumentException(
                    string.Format(RS.InvalidLinkHeaderFormat, parts), nameof(linkHeader));
            }
        }

        return links;
    }

    /// <summary>
    /// 尝试从 HTTP Link 头字符串解析，不抛出异常
    /// </summary>
    /// <param name="linkHeader">HTTP Link 头字符串</param>
    /// <param name="links">解析后的 Link 对象集合，解析失败时为 null</param>
    /// <returns>解析是否成功</returns>
#if !NETSTANDARD2_0 && !NETSTANDARD2_1
    public static bool TryParse(string linkHeader, [NotNullWhen(true)] out IEnumerable<Link>? links)
#else
    public static bool TryParse(string linkHeader, out IEnumerable<Link> links)
#endif
    {
        try
        {
            links = Parse(linkHeader);
            return true;
        }
        catch
        {
            links = null!;
            return false;
        }
    }

    /// <summary>
    /// 从多个 HTTP Link 头字符串解析为 Link 对象集合
    /// </summary>
    /// <param name="linkHeaders">HTTP Link 头字符串集合</param>
    /// <returns>所有成功解析的 Link 对象集合</returns>
    public static IEnumerable<Link> ParseMultiple(IEnumerable<string> linkHeaders)
    {
        var links = new List<Link>();

        foreach (var header in linkHeaders)
        {
            if (TryParse(header, out var parsedLinks))
            {
                links.AddRange(parsedLinks);
            }
        }

        return links;
    }

#if NET7_0_OR_GREATER
    /// <summary>
    /// 用于解析 Link 头的正则表达式（编译时生成）
    /// </summary>
    [GeneratedRegex("^<([^>]+)>\\s*;\\s*rel\\s*=\\s*[\"']([^\"']+)[\"']", RegexOptions.IgnoreCase)]
    private static partial Regex LinkHeaderRegex();
#else
    /// <summary>
    /// 用于解析 Link 头的正则表达式
    /// </summary>
    private static readonly Regex _linkHeaderRegex = new("^<([^>]+)>\\s*;\\s*rel\\s*=\\s*[\"']([^\"']+)[\"']", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    
    /// <summary>
    /// 用于解析 Link 头的正则表达式
    /// </summary>
    private static Regex LinkHeaderRegex() => _linkHeaderRegex;
#endif
}