namespace Acme.Server.Filters;

/// <summary>
/// Acme填充index响应头过滤器
/// </summary>
public class AcmeIndexLinkFilter : IActionFilter
{
    private readonly IUrlHelperFactory _urlHelperFactory;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="urlHelperFactory"></param>
    public AcmeIndexLinkFilter(IUrlHelperFactory urlHelperFactory)
    {
        _urlHelperFactory = urlHelperFactory;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    { }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var urlHelper = _urlHelperFactory.GetUrlHelper(context);

        var linkHeaderUrl = urlHelper.RouteUrl("Directory", null, "https");
        var linkHeader = $"<{linkHeaderUrl}>;rel=\"index\"";

        context.HttpContext.Response.Headers.Link = linkHeader;
    }
}
