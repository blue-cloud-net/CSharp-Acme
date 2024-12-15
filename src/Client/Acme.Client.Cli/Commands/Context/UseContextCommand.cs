namespace Acme.Client.Cli.Commands.Context;

/// <summary>
/// 使用上下文命令
/// </summary>
public class UseContextCommand : AsyncCommand<UseContextCommand.Settings>
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
