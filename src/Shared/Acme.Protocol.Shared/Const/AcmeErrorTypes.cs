namespace Acme.Protocol.Const;

/// <summary>
/// ACME 错误类型, 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-4.2.1.4"/> 定义的错误 URN
/// </summary>
public static class AcmeErrorTypes
{
    /// <summary>
    /// ACME 错误类型的命名空间
    /// </summary>
    public const string AcmeNamespace = "urn:ietf:params:acme:error";

    /// <summary>
    /// 请求指定的账户不存在
    /// </summary>
    public const string AccountDoesNotExist = "accountDoesNotExist";

    /// <summary>
    /// 请求指定要吊销的证书已经被吊销
    /// </summary>
    public const string AlreadyRevoked = "alreadyRevoked";

    /// <summary>
    /// CSR不可接受(例如,由于密钥太短)
    /// </summary>
    public const string BadCSR = "badCSR";

    /// <summary>
    /// 客户端发送的防重放随机数不可接受
    /// </summary>
    public const string BadNonce = "badNonce";

    /// <summary>
    /// JWS使用了服务器不支持的公钥进行签名
    /// </summary>
    public const string BadPublicKey = "badPublicKey";

    /// <summary>
    /// 服务器不允许提供的吊销原因
    /// </summary>
    public const string BadRevocationReason = "badRevocationReason";

    /// <summary>
    /// JWS使用了服务器不支持的签名算法
    /// </summary>
    public const string BadSignatureAlgorithm = "badSignatureAlgorithm";

    /// <summary>
    /// 证书颁发机构授权(CAA)记录禁止CA颁发证书
    /// </summary>
    public const string Caa = "caa";

    /// <summary>
    /// 具体的错误条件在"subproblems"数组中指示
    /// </summary>
    public const string Compound = "compound";

    /// <summary>
    /// 服务器无法连接到验证目标
    /// </summary>
    public const string Connection = "connection";

    /// <summary>
    /// 标识符验证期间DNS查询出现问题
    /// </summary>
    public const string Dns = "dns";

    /// <summary>
    /// 请求必须包含"externalAccountBinding"字段的值
    /// </summary>
    public const string ExternalAccountRequired = "externalAccountRequired";

    /// <summary>
    /// 收到的响应与挑战要求不匹配
    /// </summary>
    public const string IncorrectResponse = "incorrectResponse";

    /// <summary>
    /// 账户的联系人URL无效
    /// </summary>
    public const string InvalidContact = "invalidContact";

    /// <summary>
    /// 请求消息格式错误
    /// </summary>
    public const string Malformed = "malformed";

    /// <summary>
    /// 请求尝试完成尚未准备好完成的订单
    /// </summary>
    public const string OrderNotReady = "orderNotReady";

    /// <summary>
    /// 请求超出速率限制
    /// </summary>
    public const string RateLimited = "rateLimited";

    /// <summary>
    /// 服务器不会为该标识符颁发证书
    /// </summary>
    public const string RejectedIdentifier = "rejectedIdentifier";

    /// <summary>
    /// 服务器遇到内部错误
    /// </summary>
    public const string ServerInternal = "serverInternal";

    /// <summary>
    /// 服务器在验证期间收到TLS错误
    /// </summary>
    public const string Tls = "tls";

    /// <summary>
    /// 客户端缺少足够的授权
    /// </summary>
    public const string Unauthorized = "unauthorized";

    /// <summary>
    /// 账户的联系人URL使用了不支持的协议方案
    /// </summary>
    public const string UnsupportedContact = "unsupportedContact";

    /// <summary>
    /// 标识符的类型不受支持
    /// </summary>
    public const string UnsupportedIdentifier = "unsupportedIdentifier";

    /// <summary>
    /// 访问"instance"URL并执行其中指定的操作
    /// </summary>
    public const string UserActionRequired = "userActionRequired";

    /// <summary>
    /// 将错误类型转换为完整的URN格式, 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-4.2.1.4"/>
    /// </summary>
    /// <param name="errorType">错误类型,如 "userActionRequired"</param>
    /// <returns>完整的URN格式,如 "urn:ietf:params:acme:error:userActionRequired"</returns>
    /// <example>
    /// <code>
    /// string urn = AcmeErrorTypes.ToUrn(AcmeErrorTypes.UserActionRequired);
    /// // 结果: "urn:ietf:params:acme:error:userActionRequired"
    /// </code>
    /// </example>
    public static string ToUrn(string errorType) =>
        $"{AcmeNamespace}:{errorType}";

    /// <summary>
    /// 从完整的URN格式中提取错误类型, 参考 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-4.2.1.4"/>
    /// </summary>
    /// <param name="urn">完整的URN格式,如 "urn:ietf:params:acme:error:userActionRequired"</param>
    /// <returns>提取的错误类型,如 "userActionRequired"。如果URN格式不正确或为空,则返回原始值</returns>
    /// <example>
    /// <code>
    /// string errorType = AcmeErrorTypes.FromUrn("urn:ietf:params:acme:error:userActionRequired");
    /// // 结果: "userActionRequired"
    /// </code>
    /// </example>
    public static string FromUrn(string urn)
    {
        if (string.IsNullOrWhiteSpace(urn))
            return urn;

        if (urn.StartsWith(AcmeNamespace, StringComparison.OrdinalIgnoreCase))
            return urn.Substring(AcmeNamespace.Length + 1);

        return urn;
    }
}
