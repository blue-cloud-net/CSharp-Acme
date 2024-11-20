using Acme.Client.Protocol;

using Microsoft.Extensions.DependencyInjection;

using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Acme.Client;

[DependsOn(

    typeof(ClientProtocolModule),
    typeof(ClientDomainModule),
    typeof(ClientApplicationContractsModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpDddApplicationModule)
)]
public class ClientApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<ClientApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<ClientApplicationModule>(validate: true);
        });
    }
}
