namespace Acme.Protocol.Json;

/// <summary>
/// JSON序列化接口
/// </summary>
public interface IJsonSerializer
{
    /// <summary>
    /// 序列化为字符串
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="value">目标对象</param>
    /// <returns>JSON字符串</returns>
    string Serialize<T>(T value);

    /// <summary>
    /// 序列化为字符串
    /// </summary>
    /// <param name="value">目标对象</param>
    /// <returns>JSON字符串</returns>
    string Serialize(object value);

    /// <summary>
    /// 反序列化为目标类型
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="json">JSON字符串</param>
    /// <returns>目标对象实例</returns>
    T? Deserialize<T>(string json);

    /// <summary>
    /// 反序列化为目标类型
    /// </summary>
    /// <param name="json">JSON字符串</param>
    /// <param name="returnType">目标类型</param>
    /// <returns>目标对象实例</returns>
    object? Deserialize(string json, Type returnType);

    /// <summary>
    /// 异步序列化为流
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="utf8Json">目标流</param>
    /// <param name="value">目标对象</param>
    /// <param name="ct">取消令牌</param>
    /// <returns>异步任务</returns>
    Task SerializeAsync<T>(Stream utf8Json, T value, CancellationToken ct = default);

    /// <summary>
    /// 异步序列化为流
    /// </summary>
    /// <param name="utf8Json">目标流</param>
    /// <param name="value">目标对象</param>
    /// <param name="inputType">对象类型</param>
    /// <param name="ct">取消令牌</param>
    /// <returns>异步任务</returns>
    Task SerializeAsync(Stream utf8Json, object value, Type inputType, CancellationToken ct = default);

    /// <summary>
    /// 异步从流反序列化为目标类型
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="utf8Json">源流</param>
    /// <param name="ct">取消令牌</param>
    /// <returns>目标对象实例</returns>
    ValueTask<T?> DeserializeAsync<T>(Stream utf8Json, CancellationToken ct = default);

    /// <summary>
    /// 异步从流反序列化为目标类型
    /// </summary>
    /// <param name="utf8Json">源流</param>
    /// <param name="returnType">目标类型</param>
    /// <param name="ct">取消令牌</param>
    /// <returns>目标对象实例</returns>
    ValueTask<object?> DeserializeAsync(Stream utf8Json, Type returnType, CancellationToken ct = default);
}