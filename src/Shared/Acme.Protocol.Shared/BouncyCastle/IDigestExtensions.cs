namespace Acme.Protocol.BouncyCastle;

/// <summary>
/// BouncyCastle <see cref="IDigest"/>接口的扩展方法
/// </summary>
public static class IDigestExtensions
{
    /// <summary>
    /// 计算输入数据的哈希值
    /// </summary>
    /// <param name="digest">哈希算法实例</param>
    /// <param name="input">待计算哈希的输入数据</param>
    /// <returns>计算得到的哈希摘要字节数组</returns>
    /// <remarks>
    /// 此方法为 BouncyCastle 的 IDigest 接口提供便利的哈希计算方法，
    /// 简化了调用 BlockUpdate 和 DoFinal 的过程。
    /// </remarks>
    public static byte[] ComputeHash(this IDigest digest, byte[] input)
    {
        var result = new byte[digest.GetDigestSize()];

        digest.BlockUpdate(input, 0, input.Length);
        digest.DoFinal(result, 0);

        return result;
    }
}
