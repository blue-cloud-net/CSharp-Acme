namespace Acme.Protocol.HttpModels.JsonConverters;

/// <summary>
/// 订单标识符的 JSON 转换器
/// </summary>
public class OrderIdentifierJsonConverter : JsonConverter<OrderIdentifier>
{
    /// <inheritdoc/>
    public override OrderIdentifier Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token for OrderIdentifier.");
        }

        string? type = null;
        string? value = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();

                switch (propertyName?.ToLowerInvariant())
                {
                    case "type":
                        type = reader.GetString();
                        break;
                    case "value":
                        value = reader.GetString();
                        break;
                }
            }
        }

        if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(value))
        {
            throw new JsonException("Missing required properties: type and value.");
        }

        return new OrderIdentifier(type, value);
    }

    /// <inheritdoc/>
    public override void Write(
        Utf8JsonWriter writer,
        OrderIdentifier value,
        JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("type", value.Type.GetDisplayName());
        writer.WriteString("value", value.Value);
        writer.WriteEndObject();
    }
}
