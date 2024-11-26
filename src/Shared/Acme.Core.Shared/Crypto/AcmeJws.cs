using Acme.Json;

namespace Acme.Crypto;

/// <summary>
/// Acme的Jws
/// </summary>
/// <typeparam name="TPayload"></typeparam>
[JsonConverter(typeof(JsonAcmeJwsPayloadConverter))]
public class AcmeJws<TPayload> : AcmeJws
{
    /// <summary>
    /// 负载
    /// </summary>
    public required AcmeJwsPayload<TPayload> Payload { get; set; }

    /// <inheritdoc/>
    protected override JsonWebSignatureModel SerializeToModel()
    {
        var jws = new JsonWebSignatureModel
        {
            Protected = JsonSerializer.Serialize(base.Protected, JsonSerializerUtil.DefaultOptions),
            Payload = JsonSerializer.Serialize(this.Payload.Value, JsonSerializerUtil.DefaultOptions),
        };
        return jws;
    }
}

/// <summary>
/// Acme的Jws
/// </summary>
public class AcmeJws
{
    // https://datatracker.ietf.org/doc/html/rfc8555#section-6.2
    // The JWS Unprotected Header [RFC7515] MUST NOT be used

    /// <summary>
    /// 保护部分
    /// </summary>
    public required AcmeJwsProtected Protected { get; set; }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <returns></returns>
    protected virtual JsonWebSignatureModel SerializeToModel()
    {
        var jws = new JsonWebSignatureModel
        {
            Protected = JsonSerializer.Serialize(this.Protected, JsonSerializerUtil.DefaultOptions),
            Payload = "{}",
        };
        return jws;
    }

    public virtual async ValueTask<JsonWebSignatureEncodeRawModel> SignedAndToRawModelAsync(
        Func<byte[], CancellationToken, ValueTask<byte[]>> signFunc, CancellationToken cancellationToken = default)
    {
        var jws = this.SerializeToModel();

        var protectedBase64Url = Base64UrlEncoder.Encode(jws.Protected);
        var payloadBase64Url = Base64UrlEncoder.Encode(jws.Payload);
        var signingInput = $"{protectedBase64Url}.{payloadBase64Url}";
        //var signingData = Encoding.ASCII.GetBytes(signingInput);
        var signingData = Encoding.UTF8.GetBytes(signingInput);

        jws.Signature = await signFunc(signingData, cancellationToken);

        var rawModel = jws.ParseToRawModel();

        return rawModel;
    }
}
