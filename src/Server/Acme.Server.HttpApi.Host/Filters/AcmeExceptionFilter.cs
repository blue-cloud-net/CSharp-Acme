using Microsoft.AspNetCore.Mvc.Filters;

namespace Acme.Server.Filters;

/// <summary>
/// Acme异常过滤器
/// </summary>
public class AcmeExceptionFilter : IExceptionFilter
{
    private readonly ILogger<AcmeExceptionFilter> _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="logger"></param>
    public AcmeExceptionFilter(
        ILogger<AcmeExceptionFilter> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public void OnException(ExceptionContext context)
    {
    }
}
