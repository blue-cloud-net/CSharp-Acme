using Volo.Abp.Modularity;
using Volo.Abp.Domain;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Localization;
using Acme.Server.Localization;
using Volo.Abp.Validation.Localization;

namespace Acme.Server;

[DependsOn(
    typeof(AbpValidationModule),
    typeof(AbpDddDomainSharedModule)
)]
public class ServerDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ServerDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<ServerResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/Server");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Server", typeof(ServerResource));
        });
    }
}
