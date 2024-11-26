namespace Acme.Exceptions;

/// <summary>
/// 请求格式错误异常
/// </summary>
public class MalformedRequestException : AcmeException
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message"></param>
    public MalformedRequestException(string message)
        : base(AcmeErrorTypes.Malformed, message)
    {
    }
}

/// <summary>
/// 未找到异常
/// </summary>
public class NotFoundException : MalformedRequestException
{
    public NotFoundException()
        : base("The requested resource could not be found.")
    { }
}
