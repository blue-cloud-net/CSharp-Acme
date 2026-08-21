using Org.BouncyCastle.Math.EC.Multiplier;

namespace Acme.Protocol.X509;

/// <summary>
/// 私钥信息 (PKCS#1/PKCS#8)
/// </summary>
public sealed class PrivateKey : Key
{
    /// <summary>
    /// 从非对称密钥参数初始化
    /// </summary>
    /// <param name="keyParameter">BouncyCastle 非对称密钥参数</param>
    public PrivateKey(AsymmetricKeyParameter keyParameter) : base(keyParameter)
    {
        ArgumentNullException.ThrowIfNull(keyParameter, nameof(keyParameter));
        
        if (!keyParameter.IsPrivate)
            throw new ArgumentException("密钥参数必须是私钥", nameof(keyParameter));
    }

    /// <summary>
    /// 从私钥提取公钥
    /// </summary>
    /// <returns>对应的公钥</returns>
    public PublicKey ExtractPublicKey()
    {
        var publicKeyParam = ExtractPublicKeyParameter(this.BcKeyParameter);
        return new PublicKey(publicKeyParam);
    }

    /// <summary>
    /// 从 PEM 格式私钥字符串或 DER 格式字节数组解析
    /// </summary>
    /// <param name="pemKey">PEM 格式私钥字符串</param>
    /// <returns>PrivateKey 实例</returns>
    public static PrivateKey Parse(string pemKey)
    {
        ArgumentNullException.ThrowIfNull(pemKey, nameof(pemKey));
        
        var derBytes = PemFormatter.GetKeyBytes(pemKey);
        var keyParameter = ParseDerPrivateKey(derBytes);
        return new PrivateKey(keyParameter);
    }

    /// <summary>
    /// 从 DER 格式私钥字节数组解析
    /// </summary>
    /// <param name="derBytes">DER 格式私钥字节数组</param>
    /// <returns>PrivateKey 实例</returns>
    public static PrivateKey Parse(byte[] derBytes)
    {
        ArgumentNullException.ThrowIfNull(derBytes, nameof(derBytes));
        
        var keyParameter = ParseDerPrivateKey(derBytes);
        return new PrivateKey(keyParameter);
    }

    /// <summary>
    /// 解析 DER 格式私钥 (PKCS#1 或 PKCS#8)
    /// </summary>
    private static AsymmetricKeyParameter ParseDerPrivateKey(byte[] derBytes)
    {
        try
        {
            // 尝试解析为 PKCS#8
            var asn1 = Asn1Object.FromByteArray(derBytes);
            var keyParameter = PrivateKeyFactory.CreateKey(asn1);
            return keyParameter;
        }
        catch
        {
            // 如果失败，尝试解析为 PKCS#1 (RSA)
            try
            {
                var asn1 = Asn1Object.FromByteArray(derBytes);
                var rsaPrivateKeyStructure = RsaPrivateKeyStructure.GetInstance(asn1);
                return PrivateKeyFactory.CreateKey(rsaPrivateKeyStructure);
            }
            catch
            {
                throw new InvalidOperationException("无法解析私钥，既不是 PKCS#8 也不是 PKCS#1 格式");
            }
        }
    }

    /// <summary>
    /// 从私钥参数提取公钥参数
    /// </summary>
    private static AsymmetricKeyParameter ExtractPublicKeyParameter(AsymmetricKeyParameter privateKeyParam)
    {
        if (!privateKeyParam.IsPrivate)
            throw new ArgumentException("密钥参数必须是私钥", nameof(privateKeyParam));

        return privateKeyParam switch
        {
            RsaPrivateCrtKeyParameters rsaPrivate =>
                new RsaKeyParameters(false, rsaPrivate.Modulus, rsaPrivate.PublicExponent),
            ECPrivateKeyParameters ecPrivate =>
                new ECPublicKeyParameters(ecPrivate.AlgorithmName, 
                    new FixedPointCombMultiplier().Multiply(ecPrivate.Parameters.G, ecPrivate.D), 
                    ecPrivate.PublicKeyParamSet),
            DsaPrivateKeyParameters dsaPrivate => 
                new DsaPublicKeyParameters(dsaPrivate.X, dsaPrivate.Parameters),
            Ed25519PrivateKeyParameters ed25519Private =>
                ed25519Private.GeneratePublicKey(),
            _ => throw new NotSupportedException($"不支持的密钥类型: {privateKeyParam.GetType()}")
        };
    }
}
