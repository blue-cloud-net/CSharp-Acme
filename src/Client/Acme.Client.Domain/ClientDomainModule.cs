using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Acme.Client;

[DependsOn(
    typeof(ClientDomainSharedModule),
    typeof(AbpDddDomainModule)
)]
public class ClientDomainModule : AbpModule
{

}
