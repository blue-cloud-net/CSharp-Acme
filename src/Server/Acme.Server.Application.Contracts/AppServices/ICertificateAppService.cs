using Acme.HttpModels;

namespace Acme.Server.AppServices;

/// <summary>
/// 证书应用服务
/// </summary>
public interface ICertificateAppService
{
    /// <summary>
    /// 吊销证书
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RevokeAsync(CertificateRevokeModel model, CancellationToken cancellationToken = default);
}
