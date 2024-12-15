using Acme.Crypto;
using Acme.HttpModels;

namespace Acme.Server.Services.Request;

/// <summary>
/// Acme请求提供者
/// </summary>
public interface IAcmeRequestProvider
{
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="rawModel"></param>
    void Initialize(JsonWebSignatureEncodeRawModel rawModel);

    /// <summary>
    /// 获取请求
    /// </summary>
    /// <returns></returns>
    JsonWebSignatureEncodeRawModel GetRequestRawModel();

    /// <summary>
    /// 获取请求
    /// </summary>
    /// <returns></returns>
    JsonWebSignatureModel GetRequest();

    /// <summary>
    /// 获取保护部分
    /// </summary>
    /// <returns></returns>
    AcmeJwsProtected GetProtected();

    /// <summary>
    /// 获取负载部分
    /// </summary>
    /// <typeparam name="TPayload"></typeparam>
    /// <returns></returns>
    TPayload GetPayload<TPayload>();
}
