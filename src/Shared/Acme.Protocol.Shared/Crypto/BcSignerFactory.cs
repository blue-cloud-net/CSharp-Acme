using Acme.Protocol.Jwk;
using Acme.Protocol.Resources;

namespace Acme.Protocol.Crypto;

/// <summary>
/// <see cref="Org.BouncyCastle.Crypto.ISigner"/> BouncyCastle签名器工厂
/// </summary>
public static class BcSignerFactory
{
    /// <summary>
    /// 获取签名器
    /// </summary>
    /// <param name="algorithm">算法</param>
    /// <returns></returns>
    public static Org.BouncyCastle.Crypto.ISigner GetSigner(string algorithm)
    {
        if (string.IsNullOrWhiteSpace(algorithm))
            throw new ArgumentException(RS.AlgorithmCannotBeEmpty, nameof(algorithm));

        if (!JsonWebKeyAlgorithms.IsSupported(algorithm))
            throw new NotSupportedException(string.Format(RS.UnsupportedJwsAlgorithm, algorithm));

        IDigest digest = algorithm.Substring(2) switch
        {
            "256" => new Sha256Digest(),
            "384" => new Sha384Digest(),
            "512" => new Sha512Digest(),
            _ => throw new NotSupportedException(string.Format(RS.UnsupportedAlgorithm, algorithm))
        };

        Org.BouncyCastle.Crypto.ISigner signer = algorithm.Substring(0, 2) switch
        {
            "RS" => new RsaDigestSigner(digest),
            "ES" => new DsaDigestSigner(new ECDsaSigner(), digest, PlainDsaEncoding.Instance),
            "PS" => new PssSigner(new RsaBlindedEngine(), digest),
            "HS" => new HmacSigner(digest),
            _ => throw new NotSupportedException(string.Format(RS.UnsupportedAlgorithm, algorithm))
        };

        return signer;
    }
}
