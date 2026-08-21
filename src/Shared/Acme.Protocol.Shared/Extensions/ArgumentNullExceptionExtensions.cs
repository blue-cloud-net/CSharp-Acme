namespace Acme.Protocol.Extensions;

/// <summary>
/// <see cref="ArgumentNullException"/> 扩展方法
/// </summary>
public static class ArgumentNullExceptionExtensions
{
#if NETSTANDARD2_0 || NETSTANDARD2_1
    extension(ArgumentNullException)
    {
        /// <summary>
        /// 在 .NET Standard 2.0/2.1 中检查参数是否为 null
        /// </summary>
        /// <param name="argument">要验证的参数</param>
        /// <param name="paramName">参数名称</param>
        /// <exception cref="ArgumentNullException">当参数为 null 时抛出</exception>
        public static void ThrowIfNull(object? argument, string paramName)
        {
            if (argument is null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
#endif
}