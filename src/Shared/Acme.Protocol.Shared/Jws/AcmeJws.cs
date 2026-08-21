using Acme.Protocol.HttpModels;
using Acme.Protocol.Json;

namespace Acme.Protocol.Jws;

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
#if NET7_0_OR_GREATER
    public required AcmeJwsPayload<TPayload> Payload { get; set; }
#else
    public AcmeJwsPayload<TPayload> Payload { get; set; } = default!;
#endif

    /// <inheritdoc/>
    public override JsonWebSignatureModel SerializeToModel(IJsonSerializer? jsonSerializer = null)
    {
        jsonSerializer ??= JwsSystemTextJsonSerializer.Instance;

        var jws = new JsonWebSignatureModel
        {
            Protected = jsonSerializer.Serialize(base.Protected),
            Payload = jsonSerializer.Serialize(this.Payload.Value),
        };
        return jws;
    }
}

/// <summary>
/// Acme的Json Web Signature
/// <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.2">Json Web Signature</see>
/// <para>
/// The JWS Unencoded Payload Option <see href="https://datatracker.ietf.org/doc/html/rfc7797">RFC7797</see> MUST NOT be used
/// </para>
/// <para>
/// The JWS Unprotected Header <see href="https://datatracker.ietf.org/doc/html/rfc7515">RFC7515</see> MUST NOT be used
/// </para>
/// </summary>
public class AcmeJws
{
    // The JWS Unprotected Header MUST NOT be used

    /// <summary>
    /// 保护部分
    /// </summary>
#if NET7_0_OR_GREATER
    public required AcmeJwsProtected Protected { get; set; }
#else
    public AcmeJwsProtected Protected { get; set; } = default!;
#endif

    /// <summary>
    /// 序列化
    /// </summary>
    /// <returns></returns>
    public virtual JsonWebSignatureModel SerializeToModel(IJsonSerializer? jsonSerializer = null)
    {
        jsonSerializer ??= JwsSystemTextJsonSerializer.Instance;

        var jws = new JsonWebSignatureModel
        {
            Protected = jsonSerializer.Serialize(this.Protected),
            Payload = "{}",
        };
        return jws;
    }

    public virtual async ValueTask<JsonWebSignatureEncodeRawModel> SignedAndToRawModelAsync(
        Func<byte[], CancellationToken, ValueTask<byte[]>> signFunc, CancellationToken ct = default)
    {
        var jws = this.SerializeToModel();

        var protectedBase64Url = Base64UrlEncoder.Encode(jws.Protected);
        var payloadBase64Url = Base64UrlEncoder.Encode(jws.Payload);
        var signingInput = $"{protectedBase64Url}.{payloadBase64Url}";
        var signingData = Encoding.UTF8.GetBytes(signingInput);

        jws.Signature = await signFunc(signingData, ct);

        var rawModel = jws.ParseToRawModel();

        return rawModel;
    }
}
