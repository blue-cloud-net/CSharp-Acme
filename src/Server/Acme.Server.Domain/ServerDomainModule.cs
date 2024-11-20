using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Acme.Server;

[DependsOn(
    typeof(ServerDomainSharedModule),
    typeof(AbpDddDomainModule)
)]
public class ServerDomainModule : AbpModule
{

}
