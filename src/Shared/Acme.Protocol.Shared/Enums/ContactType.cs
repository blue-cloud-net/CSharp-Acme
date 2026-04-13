namespace Acme.Protocol.Enums;

/// <summary>
/// 联系方式类型, 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.3"/>
/// </summary>
public enum ContactType
{
    /// <summary>
    /// 邮箱地址（使用 mailto: URI 方案）
    /// </summary>
    [Display(Name = "mailto")]
    Email = 1,

    /// <summary>
    /// 电话号码（使用 tel: URI 方案）
    /// </summary>
    [Display(Name = "tel")]
    Phone = 2,
}
