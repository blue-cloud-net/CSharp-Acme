using ISigner = Acme.Protocol.Crypto.ISigner;

namespace Acme.Protocol.Jws;

/// <summary>
/// Json Web Signature中间过度模型
/// </summary>
public class JsonWebSignatureModel
{
    /// <summary>
    /// 构造函数
    /// </summary>
    internal JsonWebSignatureModel()
    { }

    /// <summary>
    /// jws的header部分
    /// </summary>
    public string? Header { get; internal set; }

    /// <summary>
    /// jws的protected部分
    /// </summary>
    public string Protected { get; internal set; } = String.Empty;

    /// <summary>
    /// jws的payload部分
    /// </summary>
    public string Payload { get; internal set; } = String.Empty;

    /// <summary>
    /// jws的signature部分
    /// </summary>
    public byte[] Signature { get; internal set; } = [];

    /// <summary>
    /// 使用签名器签名
    /// </summary>
    /// <param name="signer"></param>
    /// <param name="ct"></param>
    public async ValueTask SignAsync(ISigner signer, CancellationToken ct)
    {
        var data = Encoding.UTF8.GetBytes($"{this.Protected}.{this.Payload}");
        this.Signature = await signer.SignAsync(data, ct);
    }
}
