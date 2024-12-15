using Acme.Models;

namespace Acme.Server.AppServices;

/// <summary>
/// Nonce应用服务接口
/// </summary>
public interface INonceAppService
{
    /// <summary>
    /// 创建Nonce
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Nonce> CreateNonceAsync(CancellationToken cancellationToken);

    /// <summary>
    /// 消费Nonce
    /// </summary>
    /// <param name="nonce"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ConsumeNonceAsync(string nonce, CancellationToken cancellationToken);
}
