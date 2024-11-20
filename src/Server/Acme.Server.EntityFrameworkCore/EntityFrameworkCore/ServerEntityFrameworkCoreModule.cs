using Acme.Server;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace Acme.Server.EntityFrameworkCore;

[DependsOn(
    typeof(ServerDomainModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule)
)]
public class ServerEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ServerDbContext>(options =>
        {
            options.AddDefaultRepositories(includeAllEntities: true);
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
        });
        
        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure<ServerDbContext>(dbContext =>
            {
                dbContext.UseNpgsql();
            });
        });
    }
}
