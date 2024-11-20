using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Acme.Server.EntityFrameworkCore;

[ConnectionStringName(ConnectionStringName)]
public class ServerDbContext : AbpDbContext<ServerDbContext>, IServerDbContext
{
    public const string ConnectionStringName = "AcmeServer";

    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public ServerDbContext(DbContextOptions<ServerDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureServer();
    }
}
