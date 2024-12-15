using Acme.Client.Client;

using Microsoft.Extensions.DependencyInjection;

namespace Acme.Client.Extensions;

public static class AcmeProtocolClientServiceCollectionExtensions
{
    /// <summary>
    /// 注入ACME协议客户端
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureClient"></param>
    /// <returns></returns>
    public static IServiceCollection AddAcmeProtocolClient(
        this IServiceCollection services,
        Action<HttpClient>? configureClient = null)
    {
        services.AddScoped<IAcmeProtocolClient, AcmeProtocolClient>();

        return services;
    }
}
