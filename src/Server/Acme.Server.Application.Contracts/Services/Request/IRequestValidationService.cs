using Acme.Crypto;
using Acme.HttpModels;

namespace Acme.Server.Services.Request;

/// <summary>
/// Acme请求验证器服务
/// </summary>
public interface IRequestValidationService
{
    /// <summary>
    /// 验证请求
    /// </summary>
    /// <param name="jwsRawModel"></param>
    /// <param name="protected"></param>
    /// <param name="requestUrl"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ValidateAsync(JsonWebSignatureEncodeRawModel jwsRawModel, AcmeJwsProtected @protected,
        string requestUrl, CancellationToken cancellationToken);
}
