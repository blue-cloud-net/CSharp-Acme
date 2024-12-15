using Acme.Client.Extensions;

using Volo.Abp.Modularity;

namespace Acme.Client;

public class ClientProtocolModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAcmeProtocolClient();
    }
}
