namespace Acme.Protocol.X509;

/// <summary>
/// 公钥信息 (PKCS#1/PKCS#8)
/// </summary>
public sealed class PublicKey : Key
{
    /// <summary>
    /// 从非对称密钥参数初始化
    /// </summary>
    /// <param name="keyParameter">BouncyCastle 非对称密钥参数</param>
    public PublicKey(AsymmetricKeyParameter keyParameter) : base(keyParameter)
    {
        ArgumentNullException.ThrowIfNull(keyParameter, nameof(keyParameter));
        
        if (keyParameter.IsPrivate)
            throw new ArgumentException("密钥参数必须是公钥", nameof(keyParameter));
    }

    /// <summary>
    /// 从 PEM 格式公钥字符串或 DER 格式字节数组解析
    /// </summary>
    /// <param name="pemKey">PEM 格式公钥字符串</param>
    /// <returns>PublicKey 实例</returns>
    public static PublicKey Parse(string pemKey)
    {
        ArgumentNullException.ThrowIfNull(pemKey, nameof(pemKey));
        
        var derBytes = PemFormatter.GetKeyBytes(pemKey);
        var keyParameter = ParseDerPublicKey(derBytes);
        return new PublicKey(keyParameter);
    }

    /// <summary>
    /// 从 DER 格式公钥字节数组解析
    /// </summary>
    /// <param name="derBytes">DER 格式公钥字节数组</param>
    /// <returns>PublicKey 实例</returns>
    public static PublicKey Parse(byte[] derBytes)
    {
        ArgumentNullException.ThrowIfNull(derBytes, nameof(derBytes));
        
        var keyParameter = ParseDerPublicKey(derBytes);
        return new PublicKey(keyParameter);
    }

    /// <summary>
    /// 解析 DER 格式公钥
    /// </summary>
    private static AsymmetricKeyParameter ParseDerPublicKey(byte[] derBytes)
    {
        try
        {
            var asn1Object = Asn1Object.FromByteArray(derBytes);
            var publicKeyInfo = SubjectPublicKeyInfo.GetInstance(asn1Object);
            var keyParameter = PublicKeyFactory.CreateKey(publicKeyInfo);
            return keyParameter;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("无法解析公钥", ex);
        }
    }

    /// <summary>
    /// 获取公钥的 PEM 格式字符串
    /// </summary>
    /// <returns>PEM 格式的公钥</returns>
    public string ToPem()
    {
        var publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(KeyParameter);
        var keyBytes = publicKeyInfo.GetEncoded();
        return PemFormatter.Pkcs8PublicKeyBytesToPem(keyBytes);
    }

    /// <summary>
    /// 获取公钥的字节数组
    /// </summary>
    /// <returns>DER 格式的公钥字节数组</returns>
    public byte[] ToBytes()
    {
        var publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(KeyParameter);
        return publicKeyInfo.GetEncoded();
    }

}
