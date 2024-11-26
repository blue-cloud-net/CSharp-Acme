namespace Acme.Crypto;

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
    public byte[] Signature { get; internal set; } = Array.Empty<byte>();

    /// <summary>
    /// 使用签名器签名
    /// </summary>
    /// <param name="signer"></param>
    /// <param name="cancellationToken"></param>
    public async ValueTask SignAsync(ISigner signer, CancellationToken cancellationToken)
    {
        this.SignBeforeCheck();

        var data = Encoding.UTF8.GetBytes($"{this.Protected}.{this.Payload}");
        this.Signature = await signer.SignAsync(data, cancellationToken);
    }

    private void SignBeforeCheck()
    {
        if (this.Signature is not null and { Length: > 0 })
        {
            throw new InvalidOperationException("已经签名过了。");
        }
    }
}
