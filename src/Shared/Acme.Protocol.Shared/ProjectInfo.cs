namespace Acme.Protocol;

/// <summary>
/// 项目静态信息
/// </summary>
public sealed class ProjectInfo
{
    private static readonly Lazy<ProjectInfo> _instance = new(() => new ProjectInfo());

    /// <summary>
    /// 获取项目信息实例
    /// </summary>
    public static ProjectInfo Info => _instance.Value;

    private ProjectInfo()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyName = assembly.GetName();

        this.Name = assemblyName.Name ?? "CSharp-Acme";
        this.Version = assemblyName.Version?.ToString() ?? "1.0.0";
        this.FullVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? this.Version;
        this.Description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? "CSharp-ACME";
        this.Company = assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company ?? String.Empty;
        this.Copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright ?? String.Empty;
        this.DotNetVersion = Environment.Version.ToString();
    }

    /// <summary>
    /// 项目名称
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 版本号
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// 完整版本号（包含预发布标签）
    /// </summary>
    public string FullVersion { get; }

    /// <summary>
    /// 项目描述
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// 公司名称
    /// </summary>
    public string Company { get; }

    /// <summary>
    /// 版权信息
    /// </summary>
    public string Copyright { get; }

    /// <summary>
    /// .NET 运行时版本
    /// </summary>
    public string DotNetVersion { get; }

    /// <summary>
    /// 获取用户代理字符串
    /// </summary>
    public string UserAgent => $"{this.Name}/{this.Version}";

    /// <summary>
    /// 获取完整的用户代理字符串
    /// </summary>
    public string FullUserAgent => $"{this.Name}/{this.FullVersion} (.NET/{Environment.Version})";

    public override string ToString() => $"{this.Name} v{this.FullVersion}";
}
