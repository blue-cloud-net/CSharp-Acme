namespace Acme.Json;

/// <summary>
/// Jwk的Base64Url字符串转换器
/// </summary>
public class JsonConverterByteArrayBase64UrlString : JsonConverter<byte[]>
{
    /// <inheritdoc/>
    public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var baseUrlString = reader.GetString();
        var data = Base64UrlEncoder.DecodeBytes(baseUrlString);
        return data;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
    {
        var baseUrlString = Base64UrlEncoder.Encode(value);
        writer.WriteStringValue(baseUrlString);
    }
}
