namespace Acme.Models;

/// <summary>
/// 重放随机数
/// </summary>
/// <param name="Token"></param>
/// <param name="CreationTime"></param>

public record struct Nonce(string Token, DateTimeOffset CreationTime);
