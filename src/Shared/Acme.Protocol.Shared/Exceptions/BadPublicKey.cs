namespace Acme.Protocol.Exceptions;

/// <summary>
/// JWS 使用了服务器不支持的公钥进行签名异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
/// </summary>
public class BadPublicKeyException : AcmeException
{
    /// <summary>
    /// 初始化公钥不支持异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public BadPublicKeyException(string message)
        : base(AcmeErrorTypes.BadPublicKey, message)
    {
    }
}
