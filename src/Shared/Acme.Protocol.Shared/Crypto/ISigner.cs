namespace Acme.Protocol.Crypto;

/// <summary>
/// 签名器
/// </summary>
public interface ISigner
{
    /// <summary>
    /// 签名算法
    /// </summary>
    public string Algorithm { get; }

    /// <summary>
    /// 签名
    /// </summary>
    /// <param name="data">数据</param>
    /// <returns></returns>
    ValueTask<byte[]> SignAsync(byte[] data, CancellationToken ct = default);

    /// <summary>
    /// 验证签名
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="signature">签名</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    ValueTask<bool> VerifyAsync(byte[] data, byte[] signature, CancellationToken ct = default);
}
