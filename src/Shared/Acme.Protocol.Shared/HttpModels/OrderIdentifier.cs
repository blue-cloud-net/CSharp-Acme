using Acme.Protocol.Exceptions;
using Acme.Protocol.HttpModels.JsonConverters;
using Acme.Protocol.Resources;

namespace Acme.Protocol.HttpModels;

/// <summary>
/// 订单标识符，用于标识需要验证的资源类型和值
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.1.3"/>
/// </summary>
[JsonConverter(typeof(OrderIdentifierJsonConverter))]
public readonly record struct OrderIdentifier
{
    /// <summary>
    /// 初始化订单标识符实例
    /// </summary>
    /// <param name="type">标识符类型字符串（如 "dns"、"ip"、"email"）</param>
    /// <param name="value">标识符值（域名、IP 地址或邮箱地址）</param>
    /// <exception cref="MalformedRequestException">当标识符类型不支持时抛出</exception>
    public OrderIdentifier(
        string type, string value)
    {
        this.Type = type.ToEnumFromDisplayName<IdentifierType>()
            ?? throw new MalformedRequestException(
                string.Format(RS.UnsupportedIdentifierType, type));
        this.Value = value.Trim();

        if (this.Type is IdentifierType.Dns or IdentifierType.Ip)
            this.Value = this.Value.ToLowerInvariant();
    }

    /// <summary>
    /// 标识符类型（DNS 域名、IP 地址或邮箱地址）
    /// </summary>
    public IdentifierType Type { get; }

    /// <summary>
    /// 标识符值，具体含义取决于类型（域名字符串、IP 地址字符串或邮箱地址）
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// 是否为通配符域名（以 * 开头）
    /// </summary>
#if !NETSTANDARD2_0
    public bool IsWildcard => this.Value.StartsWith('*');
#else
    public bool IsWildcard => this.Value.StartsWith("*");
#endif

    /// <summary>
    /// 转换为字符串表示形式
    /// </summary>
    /// <returns>格式为 "type:value" 的标识符字符串</returns>
    public override string ToString()
        => $"{this.Type.GetName().ToLowerInvariant()}:{this.Value}";
}
