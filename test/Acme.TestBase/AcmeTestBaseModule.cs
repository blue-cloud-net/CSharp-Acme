using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace Acme.TestBase;

[DependsOn(
    typeof(AbpTestBaseModule),
    typeof(AbpBackgroundJobsAbstractionsModule)
)]
public class AcmeTestBaseModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        this.Configure<AbpBackgroundJobOptions>(options =>
        {
            options.IsJobExecutionEnabled = false;
        });
    }
}
