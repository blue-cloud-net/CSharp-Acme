using System.ComponentModel;

namespace Acme.Client.Cli.Commands;

/// <summary>
/// 注册命令
/// </summary>
public class RegisterCommand : AsyncCommand<RegisterCommand.Settings>
{
    /// <inheritdoc/>
    public class Settings : CommandSettings
    {
        /// <summary>
        /// 用户
        /// </summary>
        [Description("Acme account username.")]
        [CommandOption("-u|--user <UserName>")]
        public string UserName { get; set; } = String.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        [Description("Acme account password.")]
        [CommandOption("-p|--password <UserName>")]
        public string Password { get; set; } = String.Empty;
    }

    /// <inheritdoc/>
    public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLine("[yellow]Issue command executed![/]");
        AnsiConsole.Write(new JsonText(JsonSerializer.Serialize(settings)));

        return Task.FromResult(0);
    }
}
