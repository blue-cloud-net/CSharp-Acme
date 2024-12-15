namespace Acme.Client.Cli.Commands.Context;

/// <summary>
/// 移除上下文命令
/// </summary>
public class RemoveContextCommand : AsyncCommand<RemoveContextCommand.Settings>
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
