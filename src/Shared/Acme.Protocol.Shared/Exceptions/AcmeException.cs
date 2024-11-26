using Acme.Localization;

using Microsoft.Extensions.Localization;

namespace Acme.Exceptions;

/// <summary>
/// Acme 异常
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
        ErrorType = errorType;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="errorType"></param>
    /// <param name="message"></param>
    protected AcmeException(string errorType, string message)
        : base(message)
    {
        ErrorType = errorType;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="errorType"></param>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    protected AcmeException(string errorType, string message, Exception exception)
        : base(message, exception)
    {
        ErrorType = errorType;
    }

    /// <summary>
    ///  ACME URN namespace
    /// </summary>
    public string UrnBase { get; protected set; } = "urn:ietf:params:acme:error";

    /// <summary>
    /// 错误类型
    /// </summary>
    public virtual string ErrorType { get; } = string.Empty;

    /// <summary>
    /// AcmeError <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
    /// </summary>
    /// <returns></returns>
    public virtual AcmeError GetHttpError(IStringLocalizer<AcmeResource> localizer)
    {
        if (string.IsNullOrWhiteSpace(Message))
        {
            return new AcmeError($"{UrnBase}:{ErrorType}", localizer[ErrorType]);
        }
        else
        {
            return new AcmeError($"{UrnBase}:{ErrorType}", localizer[Message]);
        }
    }
}
