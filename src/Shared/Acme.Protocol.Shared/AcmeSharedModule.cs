using Acme.Localization;

using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Acme;

[DependsOn(typeof(AbpLocalizationModule))]
public class AcmeSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AcmeSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AcmeResource>("en")
                .AddVirtualJson("/Localization/Resources");
        });
    }
}
