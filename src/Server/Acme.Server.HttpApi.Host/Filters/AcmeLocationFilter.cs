namespace Acme.Server.Filters;

/// <summary>
/// Acme填充Location响应头过滤器
/// </summary>
public class AcmeLocationFilter : IActionFilter
{
    private readonly IUrlHelperFactory _urlHelperFactory;

    public AcmeLocationFilter(IUrlHelperFactory urlHelperFactory)
    {
        _urlHelperFactory = urlHelperFactory;
    }

    /// <inheritdoc/>
    public void OnActionExecuted(ActionExecutedContext context)
    {
        var locationAttribute = context.ActionDescriptor.FilterDescriptors
                .Select(x => x.Filter)
                .OfType<AcmeLocationAttribute>()
                .FirstOrDefault();

        if (locationAttribute == null)
            return;

        var urlHelper = _urlHelperFactory.GetUrlHelper(context);

        var locationHeaderUrl = urlHelper.RouteUrl(locationAttribute.RouteName, context.RouteData.Values, "https");
        var locationHeader = $"{locationHeaderUrl}";

        context.HttpContext.Response.Headers.Append("Location", locationHeader);
    }

    /// <inheritdoc/>
    public void OnActionExecuting(ActionExecutingContext context)
    { }
}
