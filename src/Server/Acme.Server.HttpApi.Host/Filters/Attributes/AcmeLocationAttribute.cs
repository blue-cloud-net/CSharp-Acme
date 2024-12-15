namespace Acme.Server.Filters.Attributes;

/// <summary>
/// Acme填充Location响应头
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class AcmeLocationAttribute : Attribute, IFilterMetadata
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="routeName"></param>
    public AcmeLocationAttribute(string routeName)
    {
        this.RouteName = routeName;
    }

    /// <summary>
    /// 路由名称
    /// </summary>
    public string RouteName { get; }
}
