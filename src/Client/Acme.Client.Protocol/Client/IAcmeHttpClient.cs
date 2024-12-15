namespace Acme.Client.Client;

/// <summary>
/// AcmeHTTP客户端
/// </summary>
public interface IAcmeHttpClient
{
    /// <summary>
    /// 目录URI
    /// </summary>
    public Uri DirectoryUri { get; }

    /// <summary>
    /// Http客户端
    /// </summary>
    public HttpClient HttpClient { get; }

    /// <summary>
    /// 发送HTTP请求前事件
    /// </summary>
    public Action<Uri, HttpRequestMessage>? BeforeHttpSend { get; set; }

    /// <summary>
    /// 发送HTTP请求后事件
    /// </summary>
    public Action<Uri, HttpResponseMessage>? AfterHttpSend { get; set; }

    /// <summary>
    /// 获取目录
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<DirectoryModel> GetDirectoryAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取资源URI
    /// </summary>
    /// <param name="getter"></param>
    /// <param name="optional"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<Uri?> GetResourceUriAsync(Func<DirectoryModel, string?> getter, bool optional = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取Nonce
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<string> ConsumeNonceAsync(CancellationToken cancellationToken);

    /// <summary>
    /// 发送GET请求
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="requestUri"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AcmeHttpResponse<T>> GetAsync<T>(Uri requestUri, CancellationToken cancellationToken = default);

    /// <summary>
    /// 发送POST请求
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="requestUri"></param>
    /// <param name="requestBody"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AcmeHttpResponse<T>> PostAsync<T>(Uri requestUri, JsonWebSignatureEncodeRawModel requestBody, CancellationToken cancellationToken = default);
}
