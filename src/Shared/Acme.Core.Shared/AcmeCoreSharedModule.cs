using Acme.Protocol.Shared;
using Volo.Abp.Modularity;

namespace Acme.Core.Shared;

[DependsOn(
    typeof(AcmeProtocolSharedModule)
)]
public class AcmeCoreSharedModule : AbpModule
{

}