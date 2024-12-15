using Acme.Client.Exceptions;
using Acme.Client.Localization;
using Acme.Const;

using Microsoft.Extensions.Localization;

using System.Net.Http.Headers;
using System.Reflection;

using Volo.Abp.Json;

namespace Acme.Client.Client;

/// <inheritdoc/>
public class AcmeHttpClient : IAcmeHttpClient
{
    private readonly ILogger<AcmeHttpClient> _logger;
    private readonly IStringLocalizer<AcmeClientProtocolResource> _localizer;
    private readonly IJsonSerializer _jsonSerializer;

    private static readonly IList<ProductInfoHeaderValue> _userAgentHeaders = new[]
        {
            new ProductInfoHeaderValue("CSharp-Acme", Assembly.GetExecutingAssembly().GetName().Version?.ToString()),
            new ProductInfoHeaderValue(".NET", Environment.Version.ToString()),
        };

    /// <summary>
    /// 构造函数注入
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="directoryUri"></param>
    public AcmeHttpClient(
        ILogger<AcmeHttpClient> logger,
        IStringLocalizer<AcmeClientProtocolResource> localizer,
        IJsonSerializer jsonSerializer,
        Uri directoryUri)
    {
        _logger = logger;
        _localizer = localizer;
        _jsonSerializer = jsonSerializer;

        this.DirectoryUri = directoryUri;
        this.HttpClient = this.CreateHttpClient();
    }

    /// <summary>
    /// 目录地址
    /// </summary>
    public Uri DirectoryUri { get; }

    /// <summary>
    /// Http客户端
    /// </summary>
    public HttpClient HttpClient { get; }

    /// <inheritdoc/>
    public Action<Uri, HttpRequestMessage>? BeforeHttpSend { get; set; }

    /// <inheritdoc/>
    public Action<Uri, HttpResponseMessage>? AfterHttpSend { get; set; }

    /// <summary>
    /// 创建HttpClient
    /// </summary>
    /// <returns></returns>
    protected virtual HttpClient CreateHttpClient()
    {
        var httpClient = new HttpClient(
            new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
                MaxConnectionsPerServer = 20,
                EnableMultipleHttp2Connections = true
            });

        httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");

        httpClient.DefaultRequestHeaders.UserAgent.Clear();
        foreach (var header in _userAgentHeaders)
        {
            httpClient.DefaultRequestHeaders.UserAgent.Add(header);
        }

        return httpClient;
    }

    /// <inheritdoc/>
    public async Task<AcmeHttpResponse<T>> GetAsync<T>(
        Uri requestUri, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

        this.BeforeHttpSend?.Invoke(requestUri, request);
        using var response = await this.HttpClient.SendAsync(request, cancellationToken);
        this.AfterHttpSend?.Invoke(requestUri, response);

        var acmeRespone = await this.ProcessResponseAsync<T>(response, requestUri, cancellationToken);

        return acmeRespone;
    }

    /// <inheritdoc/>
    public async Task<AcmeHttpResponse<T>> PostAsync<T>(
        Uri requestUri, JsonWebSignatureEncodeRawModel requestBody, CancellationToken cancellationToken = default)
    {
        using var response = await this.SendPostAsync(requestUri, requestBody, cancellationToken);

        var acmeRespone = await this.ProcessResponseAsync<T>(response, requestUri, cancellationToken);

        return acmeRespone;
    }

    /// <inheritdoc/>
    public async Task<AcmeHttpResponse> PostAsync(
        Uri requestUri, JsonWebSignatureEncodeRawModel requestBody, CancellationToken cancellationToken = default)
    {
        using var response = await this.SendPostAsync(requestUri, requestBody, cancellationToken);

        var acmeRespone = await this.ProcessResponseAsync(response, requestUri, cancellationToken);

        return acmeRespone;
    }

    /// <summary>
    /// 发送POST请求
    /// </summary>
    /// <param name="requestUri"></param>
    /// <param name="requestBody"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async Task<HttpResponseMessage> SendPostAsync(
        Uri requestUri, JsonWebSignatureEncodeRawModel requestBody, CancellationToken cancellationToken = default)
    {
        var content = _jsonSerializer.Serialize(requestBody);
        var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
        {
            Content = new StringContent(
                content,
                Encoding.UTF8,
                MediaTypeHeaderValues.JoseMediaTypeHeaderValue)
        };

        this.BeforeHttpSend?.Invoke(requestUri, request);
        var response = await this.HttpClient.SendAsync(request, cancellationToken);
        this.AfterHttpSend?.Invoke(requestUri, response);

        return response;
    }

    #region Directory和Nonce处理

    private DirectoryModel? _directory;

    /// <summary>
    /// 获取目录
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async ValueTask<DirectoryModel> GetDirectoryAsync(CancellationToken cancellationToken = default)
    {
        if (_directory is not null)
        {
            return _directory;
        }

        var response = await this.GetAsync<DirectoryModel>(this.DirectoryUri, cancellationToken);

        if (response.Resource is null)
        {
            throw new AcmeClientException(_localizer["ErrorLoadDirectory"]);
        }

        return _directory = response.Resource;
    }

    /// <summary>
    /// 获取资源URI
    /// </summary>
    /// <param name="getter">选择地址</param>
    /// <param name="optional">是否为可选项</param>
    /// <returns></returns>
    public async ValueTask<Uri?> GetResourceUriAsync(Func<DirectoryModel, string?> getter, bool optional = false, CancellationToken cancellationToken = default)
    {
        var directory = await this.GetDirectoryAsync(cancellationToken);

        var uri = getter(directory);
        if (optional && uri.IsNullOrWhiteSpace())
        {
            throw new NotSupportedException($"The acme operation not supported.");
        }

        return uri.IsNullOrWhiteSpace() ? null : new Uri(uri);
    }

    /// <summary>
    /// 获取Nonce
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="AcmeClientException"></exception>
    private async Task<string> FetchNonceAsync(CancellationToken cancellationToken)
    {
        var directory = await this.GetDirectoryAsync(cancellationToken);

        var request = new HttpRequestMessage(HttpMethod.Get, directory.NewNonce);

        using var response = await this.HttpClient.SendAsync(request, cancellationToken);

        if (!response.Headers.TryGetValues(HttpHeaderNames.ReplayNonce, out var values))
        {
            throw new AcmeClientException(_localizer["ErrorFetchNonce"]);
        }

        var nonce = values.First();
        return nonce;
    }

    private string? _nonce;

    /// <summary>
    /// 消费Nonce，如果没有则获取
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async ValueTask<string> ConsumeNonceAsync(CancellationToken cancellationToken)
    {
        var nonce = Interlocked.Exchange(ref _nonce, null);
        while (nonce == null)
        {
            _nonce = await this.FetchNonceAsync(cancellationToken);
            nonce = Interlocked.Exchange(ref _nonce, null);
        }

        return nonce;
    }

    #endregion

    #region 响应解析

    /// <summary>
    /// 处理响应
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="response"></param>
    /// <param name="requestedUri"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task<AcmeHttpResponse<T>> ProcessResponseAsync<T>(
        HttpResponseMessage response, Uri requestedUri, CancellationToken cancellationToken = default)
    {
        var result = default(T);
        AcmeError? error = null;
        var retryafter = this.ExtractRetryAfterHeaderFromResponse(response);
        var links = this.ExtractLinksFromResponse(response);

        if (response.IsSuccessStatusCode)
        {
            if (IsJsonMedia(response.Content!.Headers.ContentType?.MediaType))
            {
                var responseContent = await response.Content!.ReadAsStringAsync(cancellationToken);
                result = _jsonSerializer.Deserialize<T>(responseContent);
            }
            else if (typeof(T) == typeof(string))
            {
                var content = await response.Content!.ReadAsStringAsync(cancellationToken);
                result = (T)(object)content;
            }
        }
        else
        {
            if (IsJsonMedia(response.Content?.Headers?.ContentType?.MediaType))
            {
                var responseContent = await response.Content!.ReadAsStringAsync(cancellationToken);
                error = _jsonSerializer.Deserialize<AcmeError>(responseContent);
            }
            else
            {
                // propagate network errors, e.g. proxy, gateway, timeout, etc.
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    throw new AcmeClientException(_localizer["ErrorHttpRequest", requestedUri], ex);
                }
            }
        }

        return new AcmeHttpResponse<T>(response.Headers.Location, result, links, error, retryafter);
    }

    /// <summary>
    /// 处理响应
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="response"></param>
    /// <param name="requestedUri"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task<AcmeHttpResponse> ProcessResponseAsync(
        HttpResponseMessage response, Uri requestedUri, CancellationToken cancellationToken = default)
    {
        AcmeError? error = null;
        var retryafter = this.ExtractRetryAfterHeaderFromResponse(response);
        var links = this.ExtractLinksFromResponse(response);

        if (!response.IsSuccessStatusCode)
        {
            if (IsJsonMedia(response.Content?.Headers?.ContentType?.MediaType))
            {
                var responseContent = await response.Content!.ReadAsStringAsync(cancellationToken);
                error = _jsonSerializer.Deserialize<AcmeError>(responseContent);
            }
            else
            {
                // propagate network errors, e.g. proxy, gateway, timeout, etc.
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    throw new AcmeClientException(_localizer["ErrorHttpRequest", requestedUri], ex);
                }
            }
        }

        return new AcmeHttpResponse(response.Headers.Location, links, error, retryafter);
    }

    /// <summary>
    /// 从响应中提取Retry-After头
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    private double? ExtractRetryAfterHeaderFromResponse(HttpResponseMessage response)
    {
        if (response.Headers.RetryAfter is not null)
        {
            var date = response.Headers.RetryAfter.Date;
            var delta = response.Headers.RetryAfter.Delta;
            if (date.HasValue)
            {
                return Math.Abs((date.Value - DateTime.UtcNow).TotalSeconds);
            }
            else if (delta.HasValue)
            {
                return delta.Value.TotalSeconds;
            }
        }

        return null;
    }

    /// <summary>
    /// 从响应中提取Link头
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    private ILookup<string, Uri>? ExtractLinksFromResponse(HttpResponseMessage response)
    {
        ILookup<string, Uri>? links = null;
        if (response.Headers.Contains("Link"))
        {
            links = response.Headers.GetValues("Link")?
                .Select(h =>
                {
                    var segments = h.Split(';');
                    var url = segments[0][1..^1];
                    var rel = segments.Skip(1)
                        .Select(s => s.Trim())
                        .Where(s => s.StartsWith("rel=", StringComparison.OrdinalIgnoreCase))
                        .Select(r =>
                        {
                            var relType = r.Split('=')[1];
                            return relType[1..^1];
                        })
                        .First();

                    return (
                        Rel: rel,
                        Uri: new Uri(url)
                    );
                })
                .ToLookup(l => l.Rel, l => l.Uri);
        }

        return links;
    }

    /// <summary>
    /// 判断是否为Json媒体类型
    /// </summary>
    /// <param name="mediaType"></param>
    /// <returns></returns>
    private static bool IsJsonMedia(string? mediaType)
    {
        return mediaType switch
        {
            MediaTypeHeaderValues.JsonContentType
            or MediaTypeHeaderValues.JoseContentType
            or MediaTypeHeaderValues.JsonProblemContentType => true,
            _ => false
        };
    }

    #endregion
}
