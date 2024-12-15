using Acme.HttpModels;

namespace Acme.Server.Services;

/// <summary>
/// 证书颁发器
/// </summary>
public interface ICertificateIssuer
{
    Task<(byte[]? certificate, AcmeError? error)> IssueCertificate(string csr, CancellationToken cancellationToken);
}
