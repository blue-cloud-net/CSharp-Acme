namespace Acme.Protocol.Utils;

/// <summary>
/// Hex 转换工具类
/// </summary>
public static class HexConverter
{
    /// <summary>
    /// 将字节数组转换为十六进制字符串
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string ToHexString(byte[] bytes)
    {
#if NETSTANDARD2_0 || NETSTANDARD2_1
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));

        var sb = new StringBuilder(bytes.Length * 2);
        foreach (var b in bytes)
            sb.AppendFormat("{0:X2}", b);

        return sb.ToString();
#else
        return Convert.ToHexString(bytes);
#endif
    }

    /// <summary>
    /// 将十六进制字符串转换为字节数组
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static byte[] FromHexString(string hex)
    {
#if NETSTANDARD2_0 || NETSTANDARD2_1
        ArgumentNullException.ThrowIfNull(hex, nameof(hex));
        if (hex.Length % 2 != 0)
            throw new ArgumentException("Hex string must have an even length.", nameof(hex));

        var len = hex.Length / 2;
        var bytes = new byte[len];
        for (var i = 0; i < len; i++)
        {
            var byteValue = hex.Substring(i * 2, 2);
            bytes[i] = Convert.ToByte(byteValue, 16);
        }

        return bytes;
#else
        return Convert.FromHexString(hex);
#endif
    }
}