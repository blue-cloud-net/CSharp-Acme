using Acme.Enums;
using Acme.Exceptions;
using Acme.Extensions;

namespace Acme.Models;

/// <summary>
/// 联系方式
/// </summary>
public readonly record struct Contact
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="contact"></param>
    /// <exception cref="MalformedRequestException"></exception>
    public Contact(
        string contact)
    {
        var parts = contact.Split(':');
        if (parts.Length != 2)
        {
            throw new MalformedRequestException("Invalid contact format.");
        }

        Type = parts[0].ToEnumFromDisplayName<ContactType>()
            ?? throw new MalformedRequestException($"Unsupported contact type: {parts[0]}");
        Value = parts[1].Trim();
    }

    /// <summary>
    /// 类型
    /// </summary>
    public ContactType Type { get; }

    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Type.GetName().ToLowerInvariant()}:{Value}";
    }
}
