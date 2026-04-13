namespace Acme.Protocol.HttpModels;

#if !NETSTANDARD2_0 && !NETSTANDARD2_1

/// <summary>
/// 防重放随机数（Nonce），用于防止请求重放攻击
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.5"/>
/// </summary>
/// <param name="Token">随机数令牌字符串</param>
/// <param name="CreationTime">随机数创建时间</param>
public record class Nonce(string Token, DateTimeOffset CreationTime);

#else

/// <summary>
/// 防重放随机数（Nonce），用于防止请求重放攻击
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.5"/>
/// </summary>
/// <param name="Token">随机数令牌字符串</param>
/// <param name="CreationTime">随机数创建时间</param>
public class Nonce
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="token"></param>
    /// <param name="creationTime"></param>
    public Nonce(string token, DateTimeOffset creationTime)
    {
        this.Token = token;
        this.CreationTime = creationTime;
    }

    /// <summary>
    /// 随机数令牌字符串
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// 随机数创建时间
    /// </summary>
    public DateTimeOffset CreationTime { get; set; }
}

#endif