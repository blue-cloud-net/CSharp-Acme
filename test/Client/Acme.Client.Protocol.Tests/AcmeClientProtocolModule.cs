using Acme.Client.Options;

using Microsoft.Extensions.DependencyInjection;

using Volo.Abp.Modularity;

namespace Acme.Client;

[DependsOn(
    typeof(ClientProtocolModule),
    typeof(AcmeTestBaseModule)
)]
public class AcmeClientProtocolModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<AcmeClientProtocolOptions>(
            options =>
            {
                //options.DirectoryUri = "https://my.zotrus.com/yamuacme";
                options.DirectoryUri = "https://acme-staging-v02.api.letsencrypt.org/directory";
            });
    }
}
