namespace Acme.Server.Filters.Attributes;

/// <summary>
/// 添加下一个Nonce过滤器特性
/// </summary>
public class AddNextNonceAttribute : ServiceFilterAttribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public AddNextNonceAttribute()
        : base(typeof(AcmeAddNextNonceFilter))
    { }
}
