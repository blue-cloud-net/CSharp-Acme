using Acme.Const;
using Acme.HttpModels;
using Acme.Server.AppServices;

namespace Acme.Server.Controllers;

/// <summary>
/// 证书控制器
/// </summary>
[ApiController]
public class CertificateController : AbpControllerBase
{
    private readonly ICertificateAppReService _certificateAppService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="certificateAppService"></param>
    public CertificateController(
        ICertificateAppService certificateAppService)
    {
        _certificateAppService = certificateAppService;
    }

    [HttpPost(AcmeRouteTemplate.GetCertificate)]
    [AcmeLocation(nameof(AcmeRouteTemplate.GetOrder))]
    public Task<IActionResult> GetAsync()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 吊销证书
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<IActionResult> RevokeAsync(CertificateRevokeModel model)
    {
        await _certificateAppService.RevokeAsync(model, this.HttpContext.RequestAborted);

        return this.Ok();
    }
}
