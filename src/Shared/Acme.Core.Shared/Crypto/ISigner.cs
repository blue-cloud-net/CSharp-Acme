namespace Acme.Crypto;

/// <summary>
/// 签名器
/// </summary>
public interface ISigner
{
    /// <summary>
    /// 算法
    /// </summary>
    public string Algorithm { get; }

    /// <summary>
    /// 导出Jwk
    /// </summary>
    ValueTask<JsonWebKey> ExportJwkAsync(bool hasPrivateKey = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// 签名
    /// </summary>
    /// <param name="data">数据</param>
    /// <returns></returns>
    ValueTask<byte[]> SignAsync(byte[] data, CancellationToken cancellationToken = default);

    /// <summary>
    /// 验证签名
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="signature">签名</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<bool> VerifyAsync(byte[] data, byte[] signature, CancellationToken cancellationToken = default);
}
