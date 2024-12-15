namespace Acme.Client.Cli.Commands;

/// <summary>
/// 证书签发命令
/// </summary>
public class IssueCommand : AsyncCommand<IssueCommand.Settings>
{
    /// <inheritdoc/>
    public class Settings : CommandSettings
    {
        /// <summary>
        /// 域名组
        /// </summary>
        [CommandOption("-d <DomainName>")]
        public string[] DomainNames { get; set; } = Array.Empty<string>();
    }

    /// <inheritdoc/>
    public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLine("[yellow]Issue command executed![/]");
        AnsiConsole.Write(new JsonText(JsonSerializer.Serialize(settings)));

        return Task.FromResult(0);
    }
}
