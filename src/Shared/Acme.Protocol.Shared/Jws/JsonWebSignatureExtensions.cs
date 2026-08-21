using Acme.Protocol.Exceptions;
using Acme.Protocol.HttpModels;

namespace Acme.Protocol.Jws;

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
            Header = string.IsNullOrWhiteSpace(rawModel.Header) ? null : Base64UrlEncoder.Decode(rawModel.Header),
            Protected = string.IsNullOrWhiteSpace(rawModel.Protected) ? "{}" : Base64UrlEncoder.Decode(rawModel.Protected),
            Payload = string.IsNullOrWhiteSpace(rawModel.Payload) ? "{}" : Base64UrlEncoder.Decode(rawModel.Payload),
            Signature = string.IsNullOrWhiteSpace(rawModel.Payload)
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
            throw new ArgumentException("Signature字段是必须存在，请先签名。");

        return new()
        {
            Header = string.IsNullOrWhiteSpace(jws.Header) ? null : Base64UrlEncoder.Encode(jws.Header),
            Protected = string.IsNullOrWhiteSpace(jws.Protected) ? Base64UrlEncoder.Encode("{}") : Base64UrlEncoder.Encode(jws.Protected),
            Payload = string.IsNullOrWhiteSpace(jws.Payload) ? Base64UrlEncoder.Encode("{}") : Base64UrlEncoder.Encode(jws.Payload),
            Signature = jws.Signature is { Length: 0 } ? String.Empty : Base64UrlEncoder.Encode(jws.Signature),
        };
    }
}
