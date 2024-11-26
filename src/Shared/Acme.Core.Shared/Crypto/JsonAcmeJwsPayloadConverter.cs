using System.Reflection;

namespace Acme.Crypto;

/// <summary>
/// Acme的Jws的payload部分的Json转换器
/// </summary>
/// <typeparam name="TPayload"></typeparam>
public class JsonAcmeJwsPayloadConverter<TPayload> : JsonConverter<AcmeJwsPayload<TPayload>>
{
    /// <inheritdoc/>
    public override AcmeJwsPayload<TPayload> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var payload = JsonSerializer.Deserialize<TPayload>(ref reader, options)
            ?? throw new JsonException("Failed to deserialize the payload.");

        return new AcmeJwsPayload<TPayload>(payload);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, AcmeJwsPayload<TPayload> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.Value, options);
    }
}

/// <summary>
/// Acme的Jws的payload部分的Json转换器
/// </summary>
public class JsonAcmeJwsPayloadConverter : JsonConverterFactory
{
    /// <inheritdoc/>
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType)
        {
            return false;
        }

        if (typeToConvert.GetGenericTypeDefinition() != typeof(AcmeJwsPayload<>))
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type[] typeArguments = typeToConvert.GetGenericArguments();
        Type keyType = typeArguments[0];

        var converter = (JsonConverter)Activator.CreateInstance(
            typeof(AcmeJwsPayload<>).MakeGenericType([keyType]),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: [options],
            culture: null)!;

        return converter;
    }
}
