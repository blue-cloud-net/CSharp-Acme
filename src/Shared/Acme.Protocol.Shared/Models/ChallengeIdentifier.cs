using Acme.Enums;
using Acme.Exceptions;
using Acme.Extensions;

namespace Acme.Models;

/// <summary>
/// 挑战标识
/// </summary>
public readonly record struct ChallengeIdentifier
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <exception cref="MalformedRequestException"></exception>
    public ChallengeIdentifier(
        string type, string value)
    {
        Type = type.ToEnumFromDisplayName<ChallengeType>()
            ?? throw new MalformedRequestException($"Unsupported identifier type: {type}");
        Value = value.Trim();
    }

    /// <summary>
    /// 类型
    /// </summary>
    public ChallengeType Type { get; }

    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; }
}
