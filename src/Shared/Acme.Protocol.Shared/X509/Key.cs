namespace Acme.Protocol.X509;

/// <summary>
/// 密钥基类 (PKCS#1/PKCS#8)
/// </summary>
public abstract class Key
{
    /// <summary>
    /// 密钥类型
    /// </summary>
    public string KeyType { get; protected set; }

    /// <summary>
    /// 密钥大小（比特）
    /// </summary>
    public int KeySize { get; protected set; }

    /// <summary>
    /// 内部 BC 密钥参数（仅供内部使用）
    /// </summary>
    protected internal AsymmetricKeyParameter BcKeyParameter { get; protected set; }

    /// <summary>
    /// 初始化密钥基类
    /// </summary>
    /// <param name="keyParameter">BouncyCastle 非对称密钥参数</param>
    protected Key(AsymmetricKeyParameter keyParameter)
    {
        ArgumentNullException.ThrowIfNull(keyParameter, nameof(keyParameter));
        
        this.BcKeyParameter = keyParameter;
        
        this.KeyType = DetermineKeyType(keyParameter);
        this.KeySize = DetermineKeySize(keyParameter);
    }

    /// <summary>
    /// 确定密钥类型
    /// </summary>
    protected static string DetermineKeyType(AsymmetricKeyParameter keyParameter)
    {
        return keyParameter switch
        {
            RsaKeyParameters  => "RSA",
            ECKeyParameters or ECPublicKeyParameters  => "EC",
            DsaPrivateKeyParameters or DsaPublicKeyParameters => "DSA",
            Ed25519PrivateKeyParameters or Ed25519PrivateKeyParameters => "Ed25519",
            _ => throw new NotSupportedException("不支持的密钥类型"),
        };
    }

    /// <summary>
    /// 确定密钥大小
    /// </summary>
    protected static int DetermineKeySize(AsymmetricKeyParameter keyParameter)
    {
        if (keyParameter is RsaPrivateCrtKeyParameters rsaPrivateKey)
            return rsaPrivateKey.Modulus.BitLength;
        if (keyParameter is RsaKeyParameters rsaPublicKey)
            return rsaPublicKey.Modulus.BitLength;
        if (keyParameter is ECPrivateKeyParameters ecPrivateKey)
            return ecPrivateKey.Parameters.Curve.FieldSize;
        if (keyParameter is ECKeyParameters ecPublicKey)
            return ecPublicKey.Parameters.Curve.FieldSize;
        if (keyParameter is DsaPrivateKeyParameters dsaPrivateKey)
            return dsaPrivateKey.Parameters.P.BitLength;
        if (keyParameter is DsaPublicKeyParameters dsaPublicKey)
            return dsaPublicKey.Parameters.P.BitLength;
        if (keyParameter is Ed25519PrivateKeyParameters or Ed25519PublicKeyParameters)
            return 256;
        
        throw new NotSupportedException("不支持的密钥类型，无法确定密钥大小");
    }
}
