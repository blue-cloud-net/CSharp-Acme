using Acme.Exceptions;

namespace Acme.Crypto;

/// <summary>
/// Jws转换器
/// </summary>
public static class JsonWebSignatureExtensions
{
    /// <summary>
    /// 从原始模型解析
    /// </summary>
    /// <param name="rawModel"></param>
    /// <returns></returns>
    public static JsonWebSignatureModel ParseFromRawModel(this JsonWebSignatureEncodeRawModel rawModel)
    {
        return new()
        {
            Header = rawModel.Header.IsNullOrWhiteSpace() ? null : Base64UrlEncoder.Decode(rawModel.Header),
            Protected = rawModel.Protected.IsNullOrWhiteSpace() ? "{}" : Base64UrlEncoder.Decode(rawModel.Protected),
            Payload = rawModel.Payload.IsNullOrWhiteSpace() ? "{}" : Base64UrlEncoder.Decode(rawModel.Payload),
            Signature = rawModel.Payload.IsNullOrWhiteSpace()
                ? throw new MalformedRequestException("未签名无法认证，请签名。")
                : Base64UrlEncoder.DecodeBytes(rawModel.Signature),
        };
    }

    /// <summary>
    /// 转换为原始模型
    /// </summary>
    /// <param name="jws"></param>
    /// <returns></returns>
    public static JsonWebSignatureEncodeRawModel ParseToRawModel(this JsonWebSignatureModel jws)
    {
        if (jws.Signature is null or { Length: 0 })
        {
            throw new ArgumentException("Signature字段是必须得。");
        }

        return new()
        {
            Header = jws.Header.IsNullOrWhiteSpace() ? null : Base64UrlEncoder.Encode(jws.Header),
            Protected = jws.Protected.IsNullOrWhiteSpace() ? Base64UrlEncoder.Encode("{}") : Base64UrlEncoder.Encode(jws.Protected),
            Payload = jws.Payload.IsNullOrWhiteSpace() ? Base64UrlEncoder.Encode("{}") : Base64UrlEncoder.Encode(jws.Payload),
            Signature = jws.Signature is { Length: 0 } ? String.Empty : Base64UrlEncoder.Encode(jws.Signature),
        };
    }
}
