using Acme.Const;
using Acme.Server.AppServices;

namespace Acme.Server.Filters;

/// <summary>
/// 添加下一个Nonce过滤器
/// </summary>
public class AcmeAddNextNonceFilter : IAsyncActionFilter, IAsyncExceptionFilter
{
    private readonly INonceAppService _nonceService;
    private readonly ILogger<AcmeAddNextNonceFilter> _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="nonceService"></param>
    /// <param name="logger"></param>
    public AcmeAddNextNonceFilter(
        INonceAppService nonceService,
        ILogger<AcmeAddNextNonceFilter> logger)
    {
        _nonceService = nonceService;
        _logger = logger;
    }

    /// <summary>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await next.Invoke();
        await this.AddNonceHeader(context.HttpContext);
    }

    public async Task OnExceptionAsync(ExceptionContext context)
    {
        await this.AddNonceHeader(context.HttpContext);
    }

    private async Task AddNonceHeader(HttpContext httpContext)
    {
        if (httpContext.Response.Headers.ContainsKey(HttpHeaderNames.ReplayNonce))
            return;

        var newNonce = await _nonceService.CreateNonceAsync(httpContext.RequestAborted);
        httpContext.Response.Headers.Append(HttpHeaderNames.ReplayNonce, newNonce.Token);

        _logger.LogInformation($"Response Replay-Nonce: {newNonce.Token}");
    }
}
