namespace Acme.Protocol.Exceptions;

/// <summary>
/// JWS 使用了服务器不支持的签名算法异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.2"/>
/// </summary>
public class BadSignatureAlgorithmException : AcmeException
{
    /// <summary>
    /// 初始化签名算法不支持异常实例
    /// </summary>
    public BadSignatureAlgorithmException() : base(AcmeErrorTypes.BadSignatureAlgorithm)
    {
    }

    /// <summary>
    /// 初始化签名算法不支持异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public BadSignatureAlgorithmException(string message) : base(AcmeErrorTypes.BadSignatureAlgorithm, message)
    {
    }
}
