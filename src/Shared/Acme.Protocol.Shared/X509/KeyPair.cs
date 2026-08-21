namespace Acme.Protocol.X509;

/// <summary>
/// 密钥对（包含公钥和私钥）
/// </summary>
public sealed class KeyPair
{
    /// <summary>
    /// 公钥
    /// </summary>
    public PublicKey PublicKey { get; private set; }

    /// <summary>
    /// 私钥
    /// </summary>
    public PrivateKey PrivateKey { get; private set; }

    /// <summary>
    /// 密钥类型
    /// </summary>
    public string KeyType => PrivateKey.KeyType;

    /// <summary>
    /// 密钥大小（比特）
    /// </summary>
    public int KeySize => PrivateKey.KeySize;

    /// <summary>
    /// 从公钥和私钥初始化密钥对
    /// </summary>
    /// <param name="publicKey">公钥</param>
    /// <param name="privateKey">私钥</param>
    public KeyPair(PublicKey publicKey, PrivateKey privateKey)
    {
        ArgumentNullException.ThrowIfNull(publicKey, nameof(publicKey));
        ArgumentNullException.ThrowIfNull(privateKey, nameof(privateKey));

        PublicKey = publicKey;
        PrivateKey = privateKey;
    }

    /// <summary>
    /// 生成RSA密钥对
    /// </summary>
    /// <param name="keySize">密钥长度 (默认2048位)</param>
    /// <returns>RSA密钥对</returns>
    public static KeyPair GenerateRsaKeyPair(int keySize = 2048)
    {
        var keyGenerationParameters = new KeyGenerationParameters(new SecureRandom(), keySize);
        var keyPairGenerator = new RsaKeyPairGenerator();
        keyPairGenerator.Init(keyGenerationParameters);

        var bcKeyPair = keyPairGenerator.GenerateKeyPair();
        var publicKey = new PublicKey(bcKeyPair.Public);
        var privateKey = new PrivateKey(bcKeyPair.Private);

        return new KeyPair(publicKey, privateKey);
    }

    /// <summary>
    /// 生成EC密钥对
    /// </summary>
    /// <param name="curveName">椭圆曲线名称 (默认P-256)</param>
    /// <returns>EC密钥对</returns>
    public static KeyPair GenerateEcKeyPair(string curveName = "P-256")
    {
        var curve = ECNamedCurveTable.GetByName(curveName) ??
                    throw new ArgumentException($"不支持的椭圆曲线: {curveName}");

        var domainParams = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
        var keyGenParams = new ECKeyGenerationParameters(domainParams, new SecureRandom());

        var keyPairGenerator = new ECKeyPairGenerator();
        keyPairGenerator.Init(keyGenParams);

        var bcKeyPair = keyPairGenerator.GenerateKeyPair();
        var publicKey = new PublicKey(bcKeyPair.Public);
        var privateKey = new PrivateKey(bcKeyPair.Private);

        return new KeyPair(publicKey, privateKey);
    }

    /// <summary>
    /// 生成DSA密钥对
    /// </summary>
    /// <param name="keySize">密钥长度 (默认2048位)</param>
    /// <returns>DSA密钥对</returns>
    public static KeyPair GenerateDsaKeyPair(int keySize = 2048)
    {
        var keyGenParams = new DsaParametersGenerator();
        keyGenParams.Init(keySize, 64, new SecureRandom());
        var dsaParams = keyGenParams.GenerateParameters();

        var keyGenParameters = new DsaKeyGenerationParameters(new SecureRandom(), dsaParams);
        var keyPairGenerator = new DsaKeyPairGenerator();
        keyPairGenerator.Init(keyGenParameters);

        var bcKeyPair = keyPairGenerator.GenerateKeyPair();
        var publicKey = new PublicKey(bcKeyPair.Public);
        var privateKey = new PrivateKey(bcKeyPair.Private);

        return new KeyPair(publicKey, privateKey);
    }
}
