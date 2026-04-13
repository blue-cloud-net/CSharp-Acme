namespace Acme.Protocol.Exceptions;

/// <summary>
/// 账户的联系人 URL 使用了不支持的协议方案异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.3"/>
/// </summary>
public class UnsupportedContactException : AcmeException
{
    /// <summary>
    /// 初始化不支持的联系人协议异常实例
    /// </summary>
    public UnsupportedContactException()
        : base(AcmeErrorTypes.UnsupportedContact)
    {
    }
    /// <summary>
    /// 初始化不支持的联系方式异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public UnsupportedContactException(string message)
        : base(AcmeErrorTypes.UnsupportedContact, message)
    {
    }}
