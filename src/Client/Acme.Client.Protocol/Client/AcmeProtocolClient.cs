using Acme.Client.Contexts;
using Acme.Client.Options;
using Acme.Exceptions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Acme.Client.Client;

/// <inheritdoc/>
public class AcmeProtocolClient : IAcmeProtocolClient
{
    private readonly ILogger<AcmeProtocolClient> logger;

    /// <summary>
    /// 构造函数注入
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="serviceProvider"></param>
    /// <param name="options"></param>
    public AcmeProtocolClient(
        ILogger<AcmeProtocolClient> logger,
        IServiceProvider serviceProvider,
        IAcmeAccountContext accountContext,
        IOptions<AcmeClientProtocolOptions> options)
    {
        var directoryUri = new Uri(options.Value.DirectoryUri);
        this.logger = logger;
        this.AccountContext = accountContext;
        this.AcmeHttpClient = ActivatorUtilities.CreateInstance<AcmeHttpClient>(serviceProvider, [directoryUri]);
    }

    /// <summary>
    /// Acme账户上下文
    /// </summary>
    public IAcmeAccountContext AccountContext { get; }

    /// <summary>
    /// 签名器
    /// </summary>
    public ISigner Signer => this.AccountContext.Signer;

    /// <summary>
    /// AcmeHttp客户端
    /// </summary>
    public IAcmeHttpClient AcmeHttpClient { get; }

    /// <inheritdoc/>
    public ValueTask<DirectoryModel> GetDirectoryAsync(CancellationToken cancellationToken = default)
        => this.AcmeHttpClient.GetDirectoryAsync(cancellationToken);

    /// <inheritdoc/>
    public ValueTask<string> ConsumeNonceAsync(CancellationToken cancellationToken = default)
        => this.AcmeHttpClient.ConsumeNonceAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task<AccountModel> CreateAccountAsync(AccountCreateModel accountCreate, CancellationToken cancellationToken = default)
    {
        var accountUri = await this.AcmeHttpClient.GetResourceUriAsync(d => d.NewAccount, cancellationToken: cancellationToken);

        var rawModel = await this.ComputeAcmeSignedAsync(accountUri!, accountCreate, cancellationToken: cancellationToken);
        var response = await this.AcmeHttpClient.PostAsync<AccountModel>(accountUri!, rawModel, cancellationToken);
        await this.HandleAcmeErrorAsync(response, cancellationToken);

        var account = response.Resource!;
        account.Kid = response.Location;
        account.TosLink = response.Links?.FirstOrDefault(l => l.Key == "terms-of-service")?.FirstOrDefault();
        this.AccountContext.Account = account;

        return account;
    }

    /// <inheritdoc/>
    public Task<AccountModel> CheckAccountAsync(AccountModel account, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<OrderModel> CreateOrderAsync(OrderCreateModel orderCreate, CancellationToken cancellationToken = default)
    {
        var accountUri = await this.AcmeHttpClient.GetResourceUriAsync(d => d.NewOrder, cancellationToken: cancellationToken);

        var rawModel = await this.ComputeAcmeSignedAsync(accountUri!, orderCreate, cancellationToken: cancellationToken);
        var response = await this.AcmeHttpClient.PostAsync<OrderModel>(accountUri!, rawModel, cancellationToken);
        await this.HandleAcmeErrorAsync(response, cancellationToken);

        var order = response.Resource!;

        return order;
    }

    /// <inheritdoc/>
    public Task<OrderModel> GetOrderDetailsAsync(string orderUrl, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<AuthorizationModel> GetAuthorizationDetailAsync(string authzDetailUrl, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<ChallengeModel> GetChallengeDetailAsync(string challengeDetailUrl, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<ChallengeModel> AnswerChallengeAsync(string challengeDetailUrl, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<OrderModel> FinalizeOrderAsync(string orderFinalizeUrl, string csr, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<string> GetOrderCertificateAsync(string orderCertificateUrl, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<RenewalInfoModel?> GetRenewalInfoAsync(RenewalInfoModel renewalInfo, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 计算Acme签名
    /// </summary>
    /// <param name="requestUrl"></param>
    /// <param name="message"></param>
    /// <param name="includePublicKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async Task<JsonWebSignatureEncodeRawModel> ComputeAcmeSignedAsync(
        Uri requestUrl, object? message = null, bool includePublicKey = false, CancellationToken cancellationToken = default)
    {
        var @protected = new AcmeJwsProtected()
        {
            Nonce = await this.AcmeHttpClient.ConsumeNonceAsync(cancellationToken),
            Url = requestUrl,
            Algorithm = this.Signer.Algorithm,
        };

        if (includePublicKey
            || this.AccountContext.Account is null
            || this.AccountContext.Account.Kid is null)
        {
            @protected.Jwk = await this.Signer.ExportJwkAsync(cancellationToken: cancellationToken);
        }
        else
        {
            @protected.Kid = this.AccountContext.Account.Kid;
        }

        AcmeJws jws;
        if (message is null)
        {
            jws = new AcmeJws()
            {
                Protected = @protected,
            };
        }
        else
        {
            jws = new AcmeJws<object>()
            {
                Protected = @protected,
                Payload = new(message),
            };
        }

        var jwsRawModel = await jws.SignedAndToRawModelAsync(this.Signer.SignAsync, cancellationToken);

        return jwsRawModel;
    }

    /// <summary>
    /// 处理Acme错误
    /// </summary>
    /// <param name="response"></param>
    /// <exception cref="AcmeException"></exception>
    protected virtual ValueTask HandleAcmeErrorAsync(AcmeHttpResponse response, CancellationToken cancellationToken = default)
    {
        if (response.Error is not null)
        {
            throw new AcmeException(response.Error.Type, response.Error.Detail);
        }

        return ValueTask.CompletedTask;
    }
}
