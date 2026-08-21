using Acme.Protocol.Resources;

namespace Acme.Protocol.Utils;

/// <summary>
/// DirectoryUrl 标准化工具类
/// </summary>
public static class DirectoryUrlNormalizer
{
    /// <summary>
    /// 标准化目录URL
    /// </summary>
    /// <param name="url">待标准化的URL</param>
    /// <returns>标准化后的URL，如果输入为空则返回null</returns>
    public static string? NormalizeDirectoryUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return null;

        return url.Trim().TrimEnd('/');
    }

    /// <summary>
    /// 比较两个目录URL是否相等（忽略大小写和尾部斜杠）
    /// </summary>
    /// <param name="url1">第一个URL</param>
    /// <param name="url2">第二个URL</param>
    /// <returns>如果两个URL标准化后相等则返回true</returns>
    public static bool AreDirectoryUrlsEqual(string? url1, string? url2)
    {
        var normalizedUrl1 = NormalizeDirectoryUrl(url1);
        var normalizedUrl2 = NormalizeDirectoryUrl(url2);

        return string.Equals(normalizedUrl1, normalizedUrl2, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 验证URL格式是否正确
    /// </summary>
    /// <param name="url">待验证的URL</param>
    /// <returns>如果URL格式正确则返回true</returns>
    public static bool IsValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        return Uri.IsWellFormedUriString(url, UriKind.Absolute)
            && new Uri(url).Scheme is "http" or "https";
    }

    /// <summary>
    /// 标准化并验证目录URL
    /// </summary>
    /// <param name="url">待处理的URL</param>
    /// <returns>标准化后的URL</returns>
    /// <exception cref="ArgumentException">当URL格式不正确时抛出</exception>
    public static string NormalizeAndValidateDirectoryUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException(RS.UrlCannotBeEmpty, nameof(url));

        var normalizedUrl = NormalizeDirectoryUrl(url);

        if (!IsValidUrl(normalizedUrl))
            throw new ArgumentException(RS.InvalidUrlFormat, nameof(url));

        return normalizedUrl!;
    }
}
