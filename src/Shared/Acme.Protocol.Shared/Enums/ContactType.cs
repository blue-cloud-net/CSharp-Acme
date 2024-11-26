namespace Acme.Enums;

/// <summary>
/// 联系方式类型
/// </summary>
public enum ContactType
{
    /// <summary>
    /// 邮箱
    /// </summary>
    [Display(Name = "mailto")]
    Email,

    /// <summary>
    /// 电话
    /// </summary>
    [Display(Name = "tel")]
    Phone,
}
