namespace Acme.Protocol.Exceptions;

/// <summary>
/// 请求尝试完成尚未准备好完成的订单异常
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.4"/>
/// </summary>
public class OrderNotReadyException : AcmeException
{
    /// <summary>
    /// 初始化订单未准备好异常实例
    /// </summary>
    public OrderNotReadyException()
        : base(AcmeErrorTypes.OrderNotReady)
    {
    }

    /// <summary>
    /// 初始化订单未准备好异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    public OrderNotReadyException(string message)
        : base(AcmeErrorTypes.OrderNotReady, message)
    {
    }
}
