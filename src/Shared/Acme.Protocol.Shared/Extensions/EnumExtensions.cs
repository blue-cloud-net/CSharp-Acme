using Acme.Protocol.Resources;

namespace Acme.Protocol.Extensions;

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
    public static string GetName<T>(this T @enum) where T : struct, Enum =>
        Enum.GetName(typeof(T), @enum)
        ?? throw new InvalidCastException(RS.EnumNameNotFound);

    /// <summary>
    /// 获取枚举值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="ignoreCase"></param>
    /// <returns></returns>
    public static T ToEnum<T>(this string value, bool ignoreCase = false) where T : struct, Enum => 
        (T)Enum.Parse(typeof(T), value, ignoreCase);

    /// <summary>
    /// 获取枚举显示名称
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enum"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    public static string GetDisplayName<T>(this T @enum) where T : struct, Enum
    {
        var displayAttrMap = EnumDisplayAttributeCache<T>.Map.Value;

        var target = displayAttrMap.FirstOrDefault(x => x.Value.Equals(@enum));

        if (target is not null)
        {
            return target.DisplayAttribute.Name
                ?? throw new InvalidCastException(RS.EnumDisplayNameNotFound);
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
        var displayAttrMap = EnumDisplayAttributeCache<T>.Map.Value;

        foreach (var displayAttrItem in displayAttrMap)
        {
            if (String.Equals(displayAttrItem.DisplayAttribute.Name, displayName,
                ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
            {
                return displayAttrItem.Value;
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
        var displayAttrMap = EnumDisplayAttributeCache<T>.Map.Value;

        var target = displayAttrMap.FirstOrDefault(x => x.Value.Equals(@enum));

        if (target is not null)
        {
            return target.DisplayAttribute.Description
                ?? throw new InvalidCastException(RS.EnumDisplayDescriptionNotFound);
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
        var displayAttrMap = EnumDisplayAttributeCache<T>.Map.Value;

        foreach (var displayAttrItem in displayAttrMap)
        {
            if (String.Equals(displayAttrItem.DisplayAttribute.Description, displayDescription,
                ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
            {
                return displayAttrItem.Value;
            }
        }

        return null;
    }
}

/// <summary>
/// 枚举Display特性缓存
/// </summary>
/// <typeparam name="T"></typeparam>
public static class EnumDisplayAttributeCache<T> where T : struct, Enum
{
    /// <summary>
    /// Display特性缓存
    /// </summary>
    public static readonly Lazy<List<EnumDisplayAttributeCacheItem<T>>> Map = new(LoadDisplayAttributePairs);

    /// <summary>
    /// 获取Display特性
    /// </summary>
    /// <returns></returns>
    private static List<EnumDisplayAttributeCacheItem<T>> LoadDisplayAttributePairs()
    {
        var fields = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public);

        var displayAttributes = new List<EnumDisplayAttributeCacheItem<T>>(fields.Length);

        foreach (var field in fields)
        {
            var value = (T)field.GetValue(null)!;
            var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();

            if (displayAttribute is not null)
            {
                displayAttributes.Add(new(value, displayAttribute));
            }
        }

        return displayAttributes;
    }
}

#if !NETSTANDARD2_0 && !NETSTANDARD2_1

/// <summary>
/// 枚举Display特性缓存项
/// </summary>
/// <typeparam name="T">枚举类型</typeparam>
/// <param name="Value">枚举值</param>
/// <param name="DisplayAttribute">Display特性实例</param>
public record EnumDisplayAttributeCacheItem<T>(T Value, DisplayAttribute DisplayAttribute)
    where T : struct, Enum;

#else

/// <summary>
/// 枚举Display特性缓存项
/// </summary>
/// <typeparam name="T">枚举类型</typeparam>
public class EnumDisplayAttributeCacheItem<T>
    where T : struct, Enum
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="value">枚举值</param>
    /// <param name="displayAttribute">Display特性实例</param>
    public EnumDisplayAttributeCacheItem(
        T value,
        DisplayAttribute displayAttribute)
    {
        this.Value = value;
        this.DisplayAttribute = displayAttribute;
    }

    /// <summary>
    /// 枚举值
    /// </summary>
    public T Value { get; private set; }

    /// <summary>
    /// Display特性
    /// </summary>
    public DisplayAttribute DisplayAttribute { get; private set; }
}

#endif