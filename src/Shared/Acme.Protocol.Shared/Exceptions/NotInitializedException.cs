using System.Runtime.CompilerServices;

namespace Acme.Exceptions;

/// <summary>
/// 未初始化异常
/// </summary>
public class NotInitializedException : InvalidOperationException
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="caller"></param>
    public NotInitializedException([CallerMemberName] string caller = default!)
        : base($"{caller} has been accessed before being initialized.")
    {
    }
}
