using Acme.Protocol.Exceptions;
using Acme.Protocol.HttpModels.JsonConverters;
using Acme.Protocol.Resources;

namespace Acme.Protocol.HttpModels;

/// <summary>
/// 联系方式，用于账户通知和沟通
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.3"/>
/// </summary>
[JsonConverter(typeof(ContactJsonConverter))]
public readonly record struct Contact
{
    /// <summary>
    /// 从 URI 格式字符串初始化联系方式实例
    /// </summary>
    /// <param name="contact">联系方式 URI 字符串（格式："scheme:value"，如 "mailto:admin@example.com" 或 "tel:+1234567890"）</param>
    /// <exception cref="MalformedRequestException">当联系方式格式无效或类型不支持时抛出</exception>
    public Contact(
        string contact)
    {
        var parts = contact.Split(':');
        if (parts.Length != 2)
        {
            throw new MalformedRequestException(RS.InvalidContactFormat);
        }

        this.Type = parts[0].ToEnumFromDisplayName<ContactType>()
            ?? throw new MalformedRequestException(
                string.Format(RS.UnsupportedContactType, parts[0]));
        this.Value = parts[1].Trim();
    }

    /// <summary>
    /// 联系方式类型（邮箱、电话等）
    /// </summary>
    public ContactType Type { get; }

    /// <summary>
    /// 联系方式值（邮箱地址、电话号码等）
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// 转换为 URI 格式字符串
    /// </summary>
    /// <returns>格式为 "scheme:value" 的联系方式字符串</returns>
    public override string ToString() => $"{this.Type.GetName().ToLowerInvariant()}:{this.Value}";
}
