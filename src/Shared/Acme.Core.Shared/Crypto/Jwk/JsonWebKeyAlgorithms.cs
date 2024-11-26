namespace Acme.Crypto.Jwk;

/// <summary>
/// Jws算法名称
/// <see href="https://datatracker.ietf.org/doc/html/rfc7518#section-3.1"/>
/// </summary>
public static class JsonWebKeyAlgorithms
{
    // HMAC算法源
    //// HMACSHA256
    //public const string HS256 = "HS256";

    //// HMACSHA384
    //public const string HS384 = "HS384";

    //// HMACSHA512
    //public const string HS512 = "HS512";

    // SHA256withRSA
    public const string RS256 = "RS256";

    // SHA384withRSA
    public const string RS384 = "RS384";

    // SHA512withRSA
    public const string RS512 = "RS512";

    // SHA256withECDSA
    public const string ES256 = "ES256";

    // SHA384withECDSA
    public const string ES384 = "ES384";

    // SHA512withECDSA
    public const string ES512 = "ES512";

    // SHA256withRSAandMGF1
    public const string PS256 = "PS256";

    // SHA384withRSAandMGF1
    public const string PS384 = "PS384";

    // SHA512withRSAandMGF1
    public const string PS512 = "PS512";

    public static string[] All =
    [
        //HS256, HS384, HS512,
        RS256, RS384, RS512,
        ES256, ES384, ES512,
        PS256, PS384, PS512,
    ];

    /// <summary>
    /// 是否支持算法
    /// </summary>
    /// <param name="algorithm"></param>
    /// <returns></returns>
    public static bool IsSupported(string algorithm)
    {
        return All.Contains(algorithm);
    }
}
