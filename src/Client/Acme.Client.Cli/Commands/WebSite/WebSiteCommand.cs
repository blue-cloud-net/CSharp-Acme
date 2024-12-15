namespace Acme.Client.Cli.Commands.WebSite;

/// <summary>
/// 网站管理命令
/// </summary>
public class WebSiteCommand : AsyncCommand<WebSiteCommand.Settings>
{
    /// <inheritdoc/>
    public class Settings : CommandSettings
    {
    }

    /// <inheritdoc/>
    public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        throw new NotImplementedException();
    }
}
