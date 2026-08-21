namespace Acme.Protocol.BouncyCastle;

/// <summary>
/// BouncyCastle <see cref="ISigner"/> 接口的扩展方法
/// </summary>
public static class ISignerExtensions
{
    /// <summary>
    /// 使用指定的输入数据生成签名
    /// </summary>
    /// <param name="signer">签名器实例</param>
    /// <param name="parameters">签名器参数</param>
    /// <param name="input">待签名的输入数据</param>
    /// <returns>生成的签名字节数组</returns>
    public static byte[] GenerateSignature(
        this ISigner signer,
        ICipherParameters parameters, 
        byte[] input)
    {
        signer.Init(true, parameters);
        signer.BlockUpdate(input, 0, input.Length);
        return signer.GenerateSignature();
    }

    /// <summary>
    /// 使用指定的输入数据验证签名
    /// </summary>
    /// <param name="signer">签名器实例</param>
    /// <param name="input">待验证的输入数据</param>
    /// <param name="signature">用于验证的签名字节数组</param>
    /// <returns>如果签名有效则返回 true，否则返回 false</returns>
    public static bool VerifySignature(
        this ISigner signer,
        ICipherParameters parameters, byte[] input, byte[] signature)
    {
        signer.Init(false, parameters);
        signer.BlockUpdate(input, 0, input.Length);
        return signer.VerifySignature(signature);
    }
}