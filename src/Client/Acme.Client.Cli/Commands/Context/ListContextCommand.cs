namespace Acme.Client.Cli.Commands.Context;

/// <summary>
/// 列出上下文命令
/// </summary>
public class ListContextCommand : AsyncCommand<ListContextCommand.Settings>
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
