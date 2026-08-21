using System.Runtime.CompilerServices;

namespace Acme.Exceptions;

/// <summary>
/// 在初始化之前访问成员异常
/// </summary>
public class NotInitializedException : InvalidOperationException
{
    /// <summary>
    /// 初始化未初始化异常实例
    /// </summary>
    /// <param name="argment">参数说明</param>
    /// <param name="caller">调用成员名称</param>
    public NotInitializedException(string? argment = null, [CallerMemberName] string caller = null!)
        : base(string.IsNullOrWhiteSpace(argment)
            ? string.Format(Resources.AcmeCoreShared.NotInitialized, caller)
            : string.Format(Resources.AcmeCoreShared.NotInitializedWithArgument, caller, argment))
    {
    }
}
