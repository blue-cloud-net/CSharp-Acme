namespace Acme.Const;

/// <summary>
/// Http头名称常量
/// </summary>
public static class HttpHeaderNames
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public const string ReplayNonce = "Replay-Nonce";
    public const string X_Real_IP = "X-Real-IP";
    public const string X_Forwarded_For = "X-Forwarded-For";
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
}
