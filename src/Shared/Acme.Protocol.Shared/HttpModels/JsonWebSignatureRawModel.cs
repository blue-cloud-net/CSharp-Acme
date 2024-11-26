namespace Acme.HttpModels;

/// <summary>
/// Json Web Signature 文本模型
/// </summary>
public class JsonWebSignatureRawModel
{
    /// <summary>
    /// jws的header部分
    /// </summary>
    public string? Header { get; set; }

    /// <summary>
    /// jws的protected部分
    /// </summary>
    public string Protected { get; set; } = string.Empty;

    /// <summary>
    /// jws的payload部分
    /// </summary>
    public string? Payload { get; set; }

    /// <summary>
    /// jws的signature部分
    /// </summary>
    public string Signature { get; set; } = string.Empty;
}
