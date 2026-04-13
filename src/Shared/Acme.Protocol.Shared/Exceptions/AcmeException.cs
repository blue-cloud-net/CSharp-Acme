using Acme.Protocol.HttpModels;
using Acme.Protocol.Resources;

namespace Acme.Protocol.Exceptions;

/// <summary>
/// ACME 协议异常基类，所有 ACME 相关异常的父类
/// 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
/// </summary>
public class AcmeException : Exception
{
    /// <summary>
    /// 初始化 ACME 异常实例
    /// </summary>
    /// <param name="errorType">ACME 错误类型标识符</param>
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
    /// 错误类型
    /// </summary>
    public virtual string ErrorType { get; protected set; }

    /// <summary>
    /// AcmeError <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
    /// </summary>
    /// <returns></returns>
    public virtual AcmeError ToAcmeError()
    {
        return String.IsNullOrWhiteSpace(this.Message)
            ? new AcmeError($"{AcmeErrorTypes.AcmeNamespace}:{this.ErrorType}", RS.ResourceManager.GetString(this.ErrorType) ?? this.ErrorType)
            : new AcmeError($"{AcmeErrorTypes.AcmeNamespace}:{this.ErrorType}", this.Message);
    }

    /// <summary>
    /// 将 AcmeError 转换为对应的异常
    /// </summary>
    /// <param name="error">ACME错误对象</param>
    /// <returns>转换后的 ACME 异常</returns>
    private static AcmeException ConvertToException(AcmeError error)
    {
        ArgumentNullException.ThrowIfNull(error, nameof(error));

        // 从URN格式提取错误类型
        var errorType = AcmeErrorTypes.FromUrn(error.Type);

        // 根据错误类型转换为对应的异常
        return errorType switch
        {
            AcmeErrorTypes.AccountDoesNotExist => new AccountDoesNotExistException(error.Detail),
            AcmeErrorTypes.AlreadyRevoked => new AlreadyRevokedException(error.Detail),
            AcmeErrorTypes.BadCSR => new BadCSRException(error.Detail),
            AcmeErrorTypes.BadNonce => new BadNonceException(error.Detail),
            AcmeErrorTypes.BadPublicKey => new BadPublicKeyException(error.Detail),
            AcmeErrorTypes.BadRevocationReason => new BadRevocationReasonException(error.Detail),
            AcmeErrorTypes.BadSignatureAlgorithm => new BadSignatureAlgorithmException(error.Detail),
            AcmeErrorTypes.Caa => new CaaException(error.Detail),
            AcmeErrorTypes.Compound => ConvertCompoundException(error),
            AcmeErrorTypes.Connection => new ConnectionException(error.Detail),
            AcmeErrorTypes.Dns => new DnsException(error.Detail),
            AcmeErrorTypes.ExternalAccountRequired => new ExternalAccountRequiredException(error.Detail),
            AcmeErrorTypes.IncorrectResponse => new IncorrectResponseException(error.Detail),
            AcmeErrorTypes.InvalidContact => new InvalidContactException(error.Detail),
            AcmeErrorTypes.Malformed => new MalformedRequestException(error.Detail),
            AcmeErrorTypes.OrderNotReady => new OrderNotReadyException(error.Detail),
            AcmeErrorTypes.RateLimited => new RateLimitedException(error.Detail),
            AcmeErrorTypes.RejectedIdentifier => new RejectedIdentifierException(error.Detail),
            AcmeErrorTypes.ServerInternal => new ServerInternalException(error.Detail),
            AcmeErrorTypes.Tls => new TlsException(error.Detail),
            AcmeErrorTypes.Unauthorized => new UnauthorizedException(error.Detail),
            AcmeErrorTypes.UnsupportedContact => new UnsupportedContactException(error.Detail),
            AcmeErrorTypes.UnsupportedIdentifier => new UnsupportedIdentifierException(error.Detail),
            AcmeErrorTypes.UserActionRequired => new UserActionRequiredException(error.Instance!, error.Detail),
            _ => new AcmeException(errorType, error.Detail)
        };
    }

    /// <summary>
    /// 递归转换复合异常，将子问题也转换为对应的异常
    /// </summary>
    /// <param name="error">包含子问题的复合异常</param>
    /// <returns>转换后的复合异常</returns>
    private static CompoundException ConvertCompoundException(AcmeError error)
    {
        var detail = error?.Detail ?? string.Empty;

        if (error?.Subproblems == null || error.Subproblems.Count == 0)
            return new CompoundException(detail);

        // 递归转换每个子问题
        var exceptions = error.Subproblems
            .Select(ConvertToException)
            .ToArray();

        return new CompoundException(detail, exceptions);
    }

    /// <summary>
    /// 从AcmeError解析并抛出对应的异常
    /// </summary>
    /// <param name="error">ACME错误对象</param>
    /// <exception cref="AcmeException">根据错误类型抛出对应的ACME异常</exception>
    public static void ThrowFromAcmeError(AcmeError error) => throw ConvertToException(error);
}
