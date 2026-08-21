using Acme.Protocol.Json;
using Acme.Protocol.Json.Converts;

namespace Acme.Protocol.Jws;

/// <summary>
/// 基于 <see cref="System.Text.Json"/> 的 JWS 场景专用 JSON 序列化器。
/// </summary>
/// <remarks>
/// 提供预配置的 <see cref="JsonSerializerOptions"/>，使用 CamelCase 命名策略，忽略空值且不缩进。
/// </remarks>
public class JwsSystemTextJsonSerializer : SystemTextJsonSerializer
{
    /// <summary>
    /// 预配置的 JWS JSON 序列化选项。
    /// </summary>
    public static readonly JsonSerializerOptions DefaultOptions =
        new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            Converters =
            {
                new EnumDisplayNameJsonConverter(),
                new ByteArrayBase64UrlStringJsonConverter(),
            }
        };

    /// <summary>
    /// 共享的单例实例，便于复用。
    /// </summary>
    public static readonly IJsonSerializer Instance = new JwsSystemTextJsonSerializer();

    /// <summary>
    /// 使用预配置的选项初始化序列化器。
    /// </summary>
    public JwsSystemTextJsonSerializer()
        : base(DefaultOptions)
    {
    }
}