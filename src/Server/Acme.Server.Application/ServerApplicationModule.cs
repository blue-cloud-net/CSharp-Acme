using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;

namespace Acme.Server;

[DependsOn(

    typeof(ServerDomainModule),
    typeof(ServerApplicationContractsModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpDddApplicationModule)
)]
public class ServerApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<ServerApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<ServerApplicationModule>(validate: true);
        });
    }
}
