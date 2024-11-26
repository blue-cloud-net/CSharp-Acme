namespace Acme.Crypto.Jwk;

public class JwsUtilities
{
    /// <summary>
    /// JWS算法名称
    /// <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-3.1"/>
    /// </summary>
    public class JwsAlgName
    {
        /// <summary>
        /// 转换为签名和MAC名称
        /// </summary>
        /// <param name="alg"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static string ParseToSignatureAndMACName(string alg)
        {
            return alg switch
            {
                "HS256" => "HMACSHA256",
                "HS384" => "HMACSHA384",
                "HS512" => "HMACSHA512",
                "RS256" => "SHA256withRSA",
                "RS384" => "SHA384withRSA",
                "RS512" => "SHA512withRSA",
                "ES256" => "SHA256withECDSA",
                "ES384" => "SHA384withECDSA",
                "ES512" => "SHA512withECDSA",
                "PS256" => "SHA256withRSAandMGF1",
                "PS384" => "SHA384withRSAandMGF1",
                "PS512" => "SHA512withRSAandMGF1",
                _ => throw new NotSupportedException($"Unsupported JWS algorithm: {alg}")
            };
        }

        /// <summary>
        /// 从签名和MAC名称转换
        /// </summary>
        /// <param name="signAndMAC"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static string ParseFromSignatureAndMACName(string signAndMAC)
        {
            return signAndMAC switch
            {
                "HMACSHA256" => "HS256",
                "HMACSHA384" => "HS384",
                "HMACSHA512" => "HS512",
                "SHA256withRSA" => "RS256",
                "SHA384withRSA" => "RS384",
                "SHA512withRSA" => "RS512",
                "SHA256withECDSA" => "ES256",
                "SHA384withECDSA" => "ES384",
                "SHA512withECDSA" => "ES512",
                "SHA256withRSAandMGF1" => "PS256",
                "SHA384withRSAandMGF1" => "PS384",
                "SHA512withRSAandMGF1" => "PS512",
                _ => throw new NotSupportedException($"Unsupported JWS signature and MAC name: {signAndMAC}")
            };
        }
    }
}
