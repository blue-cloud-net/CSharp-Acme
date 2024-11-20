using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Acme.Client;

[DependsOn(
    typeof(AbpAuthorizationModule),
    typeof(AbpDddApplicationContractsModule)
)]
public class ClientApplicationContractsModule : AbpModule
{

}
