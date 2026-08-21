using ISigner = Acme.Protocol.Crypto.ISigner;

namespace Acme.Protocol.Jwk;

/// <summary>
/// JWK 签名器
/// </summary>
public interface IJwkSigner : ISigner
{
    /// <summary>
    /// 导出 JWK
    /// </summary>
    /// <param name="hasPrivateKey"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    ValueTask<JsonWebKey> ExportJwkAsync(bool hasPrivateKey = false, CancellationToken ct = default);
}