using Acme.Protocol.Crypto;

namespace Acme.Protocol.Jwk;

public interface IJsonWebKey : IKey
{
    /// <summary>
    /// 导出公钥
    /// </summary>
    /// <returns></returns>
    JsonWebKey ExportPublicKey();
}