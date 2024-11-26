namespace Acme.Extensions;

/// <summary>
/// 枚举扩展
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// 获取枚举名称
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enum"></param>
    /// <returns></returns>
    public static string GetName<T>(this T @enum) where T : struct, Enum
    {
        return Enum.GetName(typeof(T), @enum)
            ?? throw new InvalidCastException("Could not get the enum name.");
    }

    /// <summary>
    /// 获取枚举值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="ignoreCase"></param>
    /// <returns></returns>
    public static T ToEnum<T>(this string value, bool ignoreCase = false) where T : struct, Enum
    {
        return (T)Enum.Parse(typeof(T), value, ignoreCase);
    }

    /// <summary>
    /// 获取枚举显示名称
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enum"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    public static string GetDisplayName<T>(this T @enum) where T : struct, Enum
    {
        var displayAttrs = EnumDispalyAttributeCache<T>.LoadDisplayAttributePairs();

        if (displayAttrs.TryGetValue(@enum, out var displayAttribute))
        {
            return displayAttribute.Name ?? throw new InvalidCastException("Not found name of the enum display attribute.");
        }
        else
        {
            return @enum.GetName();
        }
    }

    /// <summary>
    /// 通过显示名称获取枚举值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="displayName"></param>
    /// <param name="ignoreCase"></param>
    /// <returns></returns>
    public static T? ToEnumFromDisplayName<T>(this string displayName, bool ignoreCase = false) where T : struct, Enum
    {
        var displayAttrs = EnumDispalyAttributeCache<T>.LoadDisplayAttributePairs();

        foreach (var displayAttr in displayAttrs)
        {
            if (string.Equals(displayAttr.Value.Name, displayName,
                ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
            {
                return displayAttr.Key;
            }
        }

        return null;
    }

    /// <summary>
    /// 获取枚举显示描述
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enum"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    public static string GetDisplayDescription<T>(this T @enum) where T : struct, Enum
    {
        var displayAttr = EnumDispalyAttributeCache<T>.LoadDisplayAttributePairs();

        if (displayAttr.TryGetValue(@enum, out var displayAttribute))
        {
            return displayAttribute.Description ?? throw new InvalidCastException("Not found description of the enum display attribute.");
        }
        else
        {
            return @enum.GetName();
        }
    }

    /// <summary>
    /// 通过显示描述获取枚举值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="displayDescription"></param>
    /// <param name="ignoreCase"></param>
    /// <returns></returns>
    public static T? ToEnumFromDisplayDescription<T>(this string displayDescription, bool ignoreCase = false) where T : struct, Enum
    {
        var displayAttrs = EnumDispalyAttributeCache<T>.LoadDisplayAttributePairs();

        foreach (var displayAttr in displayAttrs)
        {
            if (string.Equals(displayAttr.Value.Description, displayDescription,
                ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
            {
                return displayAttr.Key;
            }
        }

        return null;
    }
}

/// <summary>
/// 枚举Display特性缓存
/// </summary>
/// <typeparam name="T"></typeparam>
public static class EnumDispalyAttributeCache<T> where T : struct, Enum
{
    private static IDictionary<T, DisplayAttribute>? _displayAttributes;

    /// <summary>
    /// 获取Display特性
    /// </summary>
    /// <returns></returns>
    public static IDictionary<T, DisplayAttribute> LoadDisplayAttributePairs()
    {
        if (_displayAttributes is null)
        {
            var fields = typeof(T).GetFields();

            var displayAttributes = new Dictionary<T, DisplayAttribute>(fields.Length);

            foreach (var field in fields)
            {
                var value = (T)field.GetValue(null)!;
                var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();

                if (displayAttribute is not null)
                {
                    displayAttributes.Add(value, displayAttribute);
                }
            }

            return _displayAttributes = displayAttributes;
        }

        return _displayAttributes;
    }
}
