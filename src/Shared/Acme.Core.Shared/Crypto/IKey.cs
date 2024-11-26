namespace Acme.Crypto;

/// <summary>
/// 密钥
/// </summary>
public interface IKey
{
    /// <summary>
    /// 计算指纹
    /// </summary>
    /// <returns></returns>
    byte[] ComputeThumbprint();

    /// <summary>
    /// 导出公钥
    /// </summary>
    /// <returns></returns>
    JsonWebKey ExportPublicKey();

    /// <summary>
    /// 生成签名
    /// </summary>
    /// <param name="awaitSignData"></param>
    /// <returns></returns>
    byte[] GenerateSignature(byte[] awaitSignData);

    /// <summary>
    /// 验证签名
    /// </summary>
    /// <param name="awaitSignData"></param>
    /// <param name="signature"></param>
    /// <returns></returns>
    bool VerifySignature(byte[] awaitSignData, byte[] signature);
}
