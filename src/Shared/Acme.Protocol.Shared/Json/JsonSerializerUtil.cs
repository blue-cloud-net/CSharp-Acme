namespace Acme.Json;

/// <summary>
/// Json序列化工具类
/// </summary>
public class JsonSerializerUtil
{
    /// <summary>
    /// 获取默认的Json序列化选项
    /// </summary>
    public static JsonSerializerOptions DefaultOptions => GetDefaultOptions();

    /// <summary>
    /// 获取默认的Json序列化选项
    /// </summary>
    /// <returns></returns>
    private static JsonSerializerOptions GetDefaultOptions()
    {
        var option = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        return option;
    }
}
