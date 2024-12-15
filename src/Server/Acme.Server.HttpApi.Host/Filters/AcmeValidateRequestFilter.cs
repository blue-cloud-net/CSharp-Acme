using Acme.Server.Services.Request;

using Microsoft.AspNetCore.Http.Extensions;

namespace Acme.Server.Filters;

public class AcmeValidateRequestFilter : IAsyncActionFilter
{
    private readonly IAcmeRequestProvider _requestProvider;
    private readonly IRequestValidationService _validationService;

    public AcmeValidateRequestFilter(
        IAcmeRequestProvider requestProvider,
        IRequestValidationService validationService)
    {
        _requestProvider = requestProvider;
        _validationService = validationService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (HttpMethods.IsPost(context.HttpContext.Request.Method))
        {
            var acmeRequest = _requestProvider.GetRequestRawModel();
            var acmeHeader = _requestProvider.GetProtected();
            await _validationService.ValidateAsync(acmeRequest, acmeHeader,
                context.HttpContext.Request.GetDisplayUrl(), context.HttpContext.RequestAborted);
        }

        await next();
    }
}
