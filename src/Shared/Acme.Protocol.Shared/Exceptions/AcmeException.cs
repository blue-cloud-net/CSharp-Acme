using Acme.Localization;

using Microsoft.Extensions.Localization;

namespace Acme.Exceptions;

/// <summary>
/// Acme异常
/// </summary>

public class AcmeException : Exception
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="errorType"></param>
    protected AcmeException(string errorType)
        : base()
    {
        this.ErrorType = errorType;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="errorType"></param>
    /// <param name="message"></param>
    public AcmeException(string errorType, string message)
        : base(message)
    {
        this.ErrorType = errorType;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="errorType"></param>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    public AcmeException(string errorType, string message, Exception exception)
        : base(message, exception)
    {
        this.ErrorType = errorType;
    }

    /// <summary>
    ///  ACME URN namespace
    /// </summary>
    public string UrnBase { get; protected set; } = "urn:ietf:params:acme:error";

    /// <summary>
    /// 错误类型
    /// </summary>
    public virtual string ErrorType { get; } = String.Empty;

    /// <summary>
    /// AcmeError <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
    /// </summary>
    /// <returns></returns>
    public virtual AcmeError GetHttpError(IStringLocalizer<AcmeResource> localizer)
    {
        if (String.IsNullOrWhiteSpace(this.Message))
        {
            return new AcmeError($"{this.UrnBase}:{this.ErrorType}", localizer[this.ErrorType]);
        }
        else
        {
            return new AcmeError($"{this.UrnBase}:{this.ErrorType}", localizer[this.Message]);
        }
    }
}
