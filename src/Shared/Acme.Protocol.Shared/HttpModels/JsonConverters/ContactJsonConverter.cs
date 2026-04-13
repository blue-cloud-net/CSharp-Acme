namespace Acme.Protocol.HttpModels.JsonConverters;

/// <summary>
/// 联系方式的 JSON 转换器
/// </summary>
public class ContactJsonConverter : JsonConverter<Contact>
{
    /// <inheritdoc/>
    public override Contact Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected String token for Contact.");
        }

        var contactString = reader.GetString();
        if (string.IsNullOrEmpty(contactString))
            throw new JsonException("Contact string cannot be null or empty.");

        return new Contact(contactString);
    }

    /// <inheritdoc/>
    public override void Write(
        Utf8JsonWriter writer,
        Contact value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
