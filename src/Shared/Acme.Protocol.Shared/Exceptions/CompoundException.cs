namespace Acme.Protocol.Exceptions;

/// <summary>
/// 复合错误异常，具体错误条件在 "subproblems" 数组中指示
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7.1"/>
/// </summary>
public class CompoundException : AcmeException
{
    /// <summary>
    /// 初始化复合错误异常实例
    /// </summary>
    public CompoundException(params AcmeException[] exceptions)
        : base(AcmeErrorTypes.Compound)
    {
        this.Exceptions = exceptions;
    }

    /// <summary>
    /// 初始化复合错误异常实例
    /// </summary>
    /// <param name="message">详细错误信息</param>
    /// <param name="exceptions">复合错误异常列表</param>
    public CompoundException(string message, params AcmeException[] exceptions)
        : base(AcmeErrorTypes.Compound, message)
    {
        this.Exceptions = exceptions;
    }
    
    /// <summary>
    /// 复合错误异常列表
    /// </summary>
    public AcmeException[] Exceptions { get; set; }
}
