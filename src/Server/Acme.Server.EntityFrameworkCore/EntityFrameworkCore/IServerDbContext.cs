using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Acme.Server.EntityFrameworkCore;

[ConnectionStringName(ServerDbContext.ConnectionStringName)]
public interface IServerDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
