namespace Acme.Client;

public static class ClientDbProperties
{
    public static string DbTablePrefix { get; set; } = "Client";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Client";
}
