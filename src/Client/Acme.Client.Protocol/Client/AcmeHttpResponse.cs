namespace Acme.Client.Client;

/// <summary>
/// AcmeHTTP响应
/// </summary>
public class AcmeHttpResponse<T> : AcmeHttpResponse
{
    /// <summary>
    /// 实例化<see cref="AcmeHttpResponse{T}"/>
    /// </summary>
    /// <param name="location"></param>
    /// <param name="resource"></param>
    /// <param name="links"></param>
    /// <param name="error"></param>
    /// <param name="retryAfter"></param>
    public AcmeHttpResponse(
        Uri? location,
        T? resource,
        ILookup<string, Uri>? links,
        AcmeError? error,
        double? retryAfter)
        : base(location, links, error, retryAfter)
    {
        this.Resource = resource;
    }

    /// <summary>
    /// 响应资源
    /// </summary>
    public T? Resource { get; }
}

public class AcmeHttpResponse
{
    /// <summary>
    /// 实例化<see cref="AcmeHttpResponse"/>
    /// </summary>
    /// <param name="location"></param>
    /// <param name="links"></param>
    /// <param name="error"></param>
    /// <param name="retryAfter"></param>
    public AcmeHttpResponse(
        Uri? location,
        ILookup<string, Uri>? links,
        AcmeError? error,
        double? retryAfter)
    {
        this.Location = location;
        this.Links = links;
        this.Error = error;
        this.RetryAfter = retryAfter;
    }

    /// <summary>
    /// 响应头Location
    /// </summary>
    public Uri? Location { get; }

    /// <summary>
    /// 响应头Links
    /// </summary>
    public ILookup<string, Uri>? Links { get; }

    /// <summary>
    /// 错误
    /// </summary>
    public AcmeError? Error { get; }

    /// <summary>
    /// 响应头RetryAfter
    /// </summary>
    public double? RetryAfter { get; }
}
