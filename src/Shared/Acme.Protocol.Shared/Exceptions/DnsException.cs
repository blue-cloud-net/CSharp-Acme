namespace Acme.Protocol.Exceptions;

/// <summary>
/// 标识符验证期间 DNS 查询出现问题异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
/// </summary>
public class DnsException : AcmeException
{
    /// <summary>
    /// 初始化 DNS 查询异常实例
    /// </summary>
    public DnsException()
        : base(AcmeErrorTypes.Dns)
    {
    }

    /// <summary>
    /// 初始化 DNS 查询异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public DnsException(string message)
        : base(AcmeErrorTypes.Dns, message)
    {
    }
}
