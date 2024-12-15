namespace Acme.Client.Cli.Commands.Context;

/// <summary>
/// 添加上下文命令
/// </summary>
public class AddContextCommand : AsyncCommand<AddContextCommand.Settings>
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
