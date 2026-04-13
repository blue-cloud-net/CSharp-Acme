namespace Acme.Protocol.Const;

/// <summary>
/// 换行符常量
/// </summary>
public static class LineBreakConstant
{
    /// <summary>
    /// Windows 换行符 (CR+LF)
    /// </summary>
    public const string WindowsLineBreak = "\r\n";

    /// <summary>
    /// Unix 换行符 (LF)
    /// </summary>
    public const string UnixLineBreak = "\n";

    /// <summary>
    /// 旧版 Mac 换行符 (CR)
    /// </summary>
    public const string MacLineBreak = "\r";
}
