namespace Acme.Protocol.Exceptions;

/// <summary>
/// 请求必须包含 "externalAccountBinding" 字段的值异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.3.4"/>
/// </summary>
public class ExternalAccountRequiredException : AcmeException
{
    /// <summary>
    /// 初始化需要外部账户绑定异常实例
    /// </summary>
    public ExternalAccountRequiredException()
        : base(AcmeErrorTypes.ExternalAccountRequired)
    {
    }
    /// <summary>
    /// 初始化外部账户必填异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public ExternalAccountRequiredException(string message)
        : base(AcmeErrorTypes.ExternalAccountRequired, message)
    {
    }}
