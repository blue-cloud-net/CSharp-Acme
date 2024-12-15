using Acme.Client.Contexts;

namespace Acme.Client.Client;

/// <summary>
/// Acme协议客户端
/// </summary>
public interface IAcmeProtocolClient
{
    /// <summary>
    /// 获取或设置Acme账户上下文
    /// </summary>
    IAcmeAccountContext AccountContext { get; }

    /// <summary>
    /// 获取Acme目录
    /// </summary>
    /// <remarks>
    /// https://tools.ietf.org/html/draft-ietf-acme-acme-12#section-7.1.1
    /// </remarks>
    /// <param name="cancellationToken"></param>
    /// <returns>Acme目录</returns>
    ValueTask<DirectoryModel> GetDirectoryAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取Nonce
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Nonce</returns>
    ValueTask<string> ConsumeNonceAsync(CancellationToken cancellationToken);

    /// <summary>
    /// 创建新的账户
    /// </summary>
    /// <remarks>
    /// https://tools.ietf.org/html/draft-ietf-acme-acme-12#section-7.3
    /// </remarks>
    Task<AccountModel> CreateAccountAsync(AccountCreateModel accountCreate, CancellationToken cancellationToken = default);

    /// <summary>
    /// 查询服务器上是否存在帐户<br />
    /// 如果帐户不存在，则抛出异常。
    /// </summary>
    /// <remarks>
    /// https://tools.ietf.org/html/draft-ietf-acme-acme-12#section-7.3.1
    /// https://tools.ietf.org/html/draft-ietf-acme-acme-12#section-7.3.3
    /// </remarks>
    Task<AccountModel> CheckAccountAsync(AccountModel account, CancellationToken cancellationToken = default);

    /// <summary>
    /// 申请证书订单<br />
    /// 第一个标识符将被视为证书的主要主题，任何可选的后续标识符将被视为主题替代名称 (SAN) 条目<br />
    /// 可通过<see cref="QuickCreateOrderRequestDto"/>发送请求，以快速申请证书订单，向Acme服务器提交订单。
    /// 该方式不完全遵守Acme协议，跳过挑战和终结流程，直接进入等待签发流程，仅验证pkitoken。
    /// </summary>
    /// <param name="request">第一个标识符将被视为证书的主要主题，任何可选的后续标识符将被视为主题替代名称 (SAN) 条目</param>
    /// <param name="cancellationToken"></param>
    /// <remarks>
    /// https://tools.ietf.org/html/draft-ietf-acme-acme-12#section-7.4<br />
    /// https://tools.ietf.org/html/draft-ietf-acme-acme-12#section-7.1.3
    /// </remarks>
    Task<OrderModel> CreateOrderAsync(OrderCreateModel orderCreate, CancellationToken cancellationToken = default);

    /// <summary>
    /// 查询现有订单的当前状态和详情信息
    /// </summary>
    /// <param name="orderUrl"></param>
    /// <param name="cancellationToken"></param>
    /// <remarks>
    /// https://tools.ietf.org/html/draft-ietf-acme-acme-12#section-7.4
    /// https://tools.ietf.org/html/draft-ietf-acme-acme-12#section-7.1.3
    /// </remarks>
    /// <returns></returns>
    Task<OrderModel> GetOrderDetailsAsync(string orderUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// 检索与先前创建的订单关联的授权的详细信息。 授权详细信息 URL 作为订单响应的一部分返回
    /// </summary>
    /// <param name="authzDetailUrl"></param>
    /// <param name="cancellationToken"></param>
    /// <remarks>
    /// https://tools.ietf.org/html/draft-ietf-acme-acme-12#section-7.5
    /// https://tools.ietf.org/html/draft-ietf-acme-acme-12#section-7.1.4
    /// </remarks>
    /// <returns></returns>
    Task<AuthorizationModel> GetAuthorizationDetailAsync(string authzDetailUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取挑战明细
    /// </summary>
    /// <param name="challengeDetailUrl"></param>
    /// <param name="cancellationToken"></param>
    /// <remarks>
    /// https://tools.ietf.org/html/draft-ietf-acme-acme-12#section-7.5.1
    /// </remarks>
    /// <returns></returns>
    Task<ChallengeModel> GetChallengeDetailAsync(string challengeDetailUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// 回应激活挑战
    /// </summary>
    /// <param name="challengeDetailUrl"></param>
    /// <param name="cancellationToken"></param>
    /// <remarks>
    /// https://tools.ietf.org/html/draft-ietf-acme-acme-12#section-7.5.1
    /// </remarks>
    /// <returns></returns>
    Task<ChallengeModel> AnswerChallengeAsync(string challengeDetailUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// 终结订单
    /// </summary>
    /// <param name="orderFinalizeUrl"></param>
    /// <param name="csr"></param>
    /// <param name="cancellationToken"></param>
    /// <remarks>
    /// https://tools.ietf.org/html/draft-ietf-acme-acme-12#section-7.4
    /// </remarks>
    /// <returns></returns>
    Task<OrderModel> FinalizeOrderAsync(string orderFinalizeUrl, string csr, CancellationToken cancellationToken = default);

    /// <summary>
    /// 收集该订单的证书
    /// </summary>
    /// <param name="orderCertificateUrl"></param>
    /// <param name="cancellationToken"></param>
    /// <remarks>
    /// https://tools.ietf.org/html/draft-ietf-acme-acme-12#section-7.4.2
    /// </remarks>
    /// <returns></returns>
    Task<string> GetOrderCertificateAsync(string orderCertificateUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取续订信息
    /// </summary>
    /// <param name="renewalInfo"></param>
    /// <param name="cancellationToken"></param>
    /// <remarks>
    /// https://www.ietf.org/archive/id/draft-ietf-acme-ari-06.html#name-the-renewalinfo-resource
    /// </remarks>
    /// <returns></returns>
    Task<RenewalInfoModel?> GetRenewalInfoAsync(RenewalInfoModel renewalInfo, CancellationToken cancellationToken = default);
}
