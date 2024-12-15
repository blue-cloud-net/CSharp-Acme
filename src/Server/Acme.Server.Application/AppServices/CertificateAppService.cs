using Acme.HttpModels;

using Microsoft.IdentityModel.Tokens;

using Org.BouncyCastle.X509;

using Volo.Abp.DependencyInjection;

namespace Acme.Server.AppServices;

/// <inheritdoc cref="ICertificateAppService"/>
public class CertificateAppService : ICertificateAppService, IScopedDependency
{
    /// <inheritdoc/>
    public Task RevokeAsync(CertificateRevokeModel model, CancellationToken cancellationToken = default)
    {
        var certData = Base64UrlEncoder.DecodeBytes(model.Certificate);
        var cert = new X509Certificate(certData);
        var serialNumber = cert.SerialNumber;
    }
}
