using Spectre.Console.Cli.Help;

namespace Acme.Client.Cli.Help;

/// <summary>
/// Acme 帮助提供程序
/// </summary>
public class AcmeHelpProvider : HelpProvider
{
    /// <summary>
    /// 构造函数注入
    /// </summary>
    /// <param name="settings"></param>
    public AcmeHelpProvider(
        ICommandAppSettings settings) : base(settings)
    {
    }

    /// <inheritdoc/>
    //public override IEnumerable<IRenderable> GetCommands(ICommandModel model, ICommandInfo? command)
    //{
    //    var commandContainer = command ?? (ICommandContainer)model;
    //    bool isDefaultCommand = command?.IsDefaultCommand ?? false;

    //    var commands = isDefaultCommand ? model.Commands : commandContainer.Commands;
    //    commands = commands.Where(x => !x.IsHidden).ToList();

    //    if (commands.Count == 0)
    //    {
    //        return Array.Empty<IRenderable>();
    //    }

    //    var result = new List<IRenderable>
    //    {
    //        NewComposer().LineBreak().Style(helpStyles?.Commands?.Header ?? Style.Plain, $"{resources.Commands}:").LineBreak(),
    //    };

    //    var grid = new Grid();
    //    grid.AddColumn(new GridColumn { Padding = new Padding(4, 4), NoWrap = true });
    //    grid.AddColumn(new GridColumn { Padding = new Padding(0, 0) });

    //    foreach (var child in commands)
    //    {
    //        var arguments = NewComposer();
    //        arguments.Style(helpStyles?.Commands?.ChildCommand ?? Style.Plain, child.Name);
    //        arguments.Space();

    //        foreach (var argument in HelpArgument.Get(child).Where(a => a.Required))
    //        {
    //            arguments.Style(helpStyles?.Commands?.RequiredArgument ?? Style.Plain, $"<{argument.Name}>");
    //            arguments.Space();
    //        }

    //        if (this.TrimTrailingPeriod)
    //        {
    //            grid.AddRow(
    //                NewComposer().Text(arguments.ToString().TrimEnd()),
    //                NewComposer().Text(child.Description?.TrimEnd('.') ?? " "));
    //        }
    //        else
    //        {
    //            grid.AddRow(
    //                NewComposer().Text(arguments.ToString().TrimEnd()),
    //                NewComposer().Text(child.Description ?? " "));
    //        }
    //    }

    //    result.Add(grid);

    //    return result;
    //}
}
