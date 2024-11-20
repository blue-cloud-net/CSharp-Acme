using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Acme.Server;

[DependsOn(
    typeof(ServerDomainSharedModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpDddApplicationContractsModule)
)]
public class ServerApplicationContractsModule : AbpModule
{

}
