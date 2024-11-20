using Volo.Abp.Modularity;
using Volo.Abp.Domain;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Localization;
using Acme.Client.Localization;
using Volo.Abp.Validation.Localization;

namespace Acme.Client;

[DependsOn(
    typeof(AbpValidationModule),
    typeof(AbpDddDomainSharedModule)
)]
public class ClientDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ClientDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<ClientResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/Client");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Client", typeof(ClientResource));
        });
    }
}
