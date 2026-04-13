namespace Acme.Protocol.Json;

/// <summary>
/// System.Text.Json 封装实现
/// </summary>
public class SystemTextJsonSerializer : IJsonSerializer
{
    /// <summary>
    /// 默认序列化器实例
    /// </summary>
    public static readonly SystemTextJsonSerializer Default = new();

    /// <summary>
    /// 默认序列化配置
    /// </summary>
    public JsonSerializerOptions Options { get; }

    /// <summary>
    /// 初始化序列化器
    /// </summary>
    /// <param name="options">自定义配置，缺省使用 <see cref="JsonSerializerOptions.Default"/></param>
    public SystemTextJsonSerializer(JsonSerializerOptions? options = null)
    {
        this.Options = options ?? new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    /// <inheritdoc />
    public string Serialize<T>(T value) => JsonSerializer.Serialize(value, this.Options);

    /// <inheritdoc />
    public string Serialize(object value) => JsonSerializer.Serialize(value, value.GetType(), this.Options);

    /// <inheritdoc />
    public T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, this.Options);

    /// <inheritdoc />
    public object? Deserialize(string json, Type returnType)
    {
        ArgumentNullException.ThrowIfNull(returnType, nameof(returnType));

        return JsonSerializer.Deserialize(json, returnType, this.Options);
    }

    /// <inheritdoc />
    public Task SerializeAsync<T>(Stream utf8Json, T value, CancellationToken ct = default)
    {
        return JsonSerializer.SerializeAsync(utf8Json, value, this.Options, ct);
    }

    /// <inheritdoc />
    public Task SerializeAsync(Stream utf8Json, object value, Type inputType, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(inputType, nameof(inputType));

        return JsonSerializer.SerializeAsync(utf8Json, value, inputType, this.Options, ct);
    }

    /// <inheritdoc />
    public ValueTask<T?> DeserializeAsync<T>(Stream utf8Json, CancellationToken ct = default)
    {
        return JsonSerializer.DeserializeAsync<T>(utf8Json, this.Options, ct);
    }

    /// <inheritdoc />
    public ValueTask<object?> DeserializeAsync(Stream utf8Json, Type returnType, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(returnType, nameof(returnType));

        return JsonSerializer.DeserializeAsync(utf8Json, returnType, this.Options, ct);
    }
}