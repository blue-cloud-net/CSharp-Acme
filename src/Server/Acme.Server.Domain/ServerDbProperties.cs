namespace Acme.Server;

public static class ServerDbProperties
{
    public static string DbTablePrefix { get; set; } = "Server";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Server";
}
