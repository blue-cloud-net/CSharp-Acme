using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Signers;

namespace Acme.Crypto.Jwk;

/// <summary>
/// <see cref="Org.BouncyCastle.Crypto.ISigner">签名器工厂
/// </summary>
public class BCSignerFactory
{
    /// </summary>
    /// <param name="algorithm">算法</param>
    /// <returns></returns>
    public static Org.BouncyCastle.Crypto.ISigner GetSigner(string algorithm)
    {
        if (algorithm.IsNullOrWhiteSpace())
        {
            throw new ArgumentException("Algorithm cannot be null or whitespace.", nameof(algorithm));
        }

        if (!JsonWebKeyAlgorithms.IsSupported(algorithm))
        {
            throw new NotSupportedException($"Unsupported JWS algorithm: {algorithm}");
        }

        Org.BouncyCastle.Crypto.IDigest digest = algorithm[2..] switch
        {
            "256" => new Sha256Digest(),
            "384" => new Sha384Digest(),
            "512" => new Sha512Digest(),
            _ => throw new NotSupportedException($"Unsupported algorithm '{algorithm}'.")
        };

        Org.BouncyCastle.Crypto.ISigner signer = algorithm[..2] switch
        {
            "RS" => new RsaDigestSigner(digest),
            "ES" => new DsaDigestSigner(new ECDsaSigner(), digest, PlainDsaEncoding.Instance),
            //"ES" => new DsaDigestSigner(new ECDsaSigner(), digest),
            "PS" => new PssSigner(new RsaBlindedEngine(), digest),
            "HS" => new HmacSigner(digest),
            _ => throw new NotSupportedException($"Unsupported algorithm '{algorithm}'.")
        };

        return signer;
    }
}
