namespace Acme.Json;

/// <summary>
/// 枚举显示名称转换器创建器
/// </summary>
public class JsonDisplayNameEnumConverter : JsonConverterFactory
{
    /// <summary>
    /// 是否可以转换
    /// </summary>
    /// <param name="typeToConvert"></param>
    /// <returns></returns>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum;
    }

    /// <summary>
    /// 创建转换器
    /// </summary>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return (JsonConverter)(Activator.CreateInstance(
            typeof(JsonDisplayNameEnumConverter<>).MakeGenericType(typeToConvert),
            BindingFlags.Instance | BindingFlags.Public, null, null, null)
            ?? throw new JsonException(""));
    }
}

/// <summary>
/// 枚举显示名称转换器
/// </summary>
/// <typeparam name="T"></typeparam>
public class JsonDisplayNameEnumConverter<T> : JsonConverter<T> where T : struct, Enum
{
    /// <inheritdoc/>
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var enumValue = reader.GetString()?.ToEnumFromDisplayName<T>()
            ?? throw new InvalidCastException("");

        return enumValue;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var enumDisplayName = value.GetDisplayName();

        writer.WriteStringValue(enumDisplayName);
    }
}
