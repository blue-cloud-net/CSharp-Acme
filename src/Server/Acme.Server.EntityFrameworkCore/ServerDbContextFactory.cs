using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Acme.Server.EntityFrameworkCore;

namespace Acme.Server;

public class ServerDbContextFactory : IDesignTimeDbContextFactory<ServerDbContext>
{
    public ServerDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ServerDbContext>()
            .UseNpgsql(GetConnectionString());

        return new ServerDbContext(builder.Options);
    }

    private static string GetConnectionString()
    {
        var configuration = BuildConfiguration();
        return configuration.GetConnectionString("AcmeServer")!;
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}