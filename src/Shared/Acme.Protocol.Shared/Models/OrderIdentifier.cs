using Acme.Enums;
using Acme.Exceptions;
using Acme.Extensions;

namespace Acme.Models;

/// <summary>
/// 订单标识
/// </summary>
public readonly record struct OrderIdentifier
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <param name="subdomainAuthAllowed"></param>
    /// <exception cref="MalformedRequestException"></exception>
    public OrderIdentifier(
        string type, string value, bool subdomainAuthAllowed)
    {
        Type = type.ToEnumFromDisplayName<IdentifierType>()
            ?? throw new MalformedRequestException($"Unsupported identifier type: {type}");
        Value = value.Trim();

        if (Type is IdentifierType.Dns or IdentifierType.Ip)
        {
            Value = Value.ToLowerInvariant();
        }

        SubdomainAuthAllowed = subdomainAuthAllowed;
    }

    /// <summary>
    /// 类型
    /// </summary>
    public IdentifierType Type { get; }

    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// 是否允许子域名授权
    /// <see href="https://www.rfc-editor.org/rfc/rfc9444.html#name-pre-authorization"/>
    /// </summary>
    public bool SubdomainAuthAllowed { get; }

    /// <summary>
    /// 是否为通配符
    /// </summary>
    public bool IsWildcard => Value.StartsWith('*');

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Type.GetName().ToLowerInvariant()}:{Value}";
    }
}
