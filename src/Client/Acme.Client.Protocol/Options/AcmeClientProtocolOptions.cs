namespace Acme.Client.Options;

/// <summary>
/// ACME客户端协议选项
/// </summary>
public class AcmeClientProtocolOptions
{
    /// <summary>
    /// Gets or sets the ACME directory URI.
    /// </summary>
    public string DirectoryUri { get; set; } = "https://acme-v02.api.letsencrypt.org/directory";
}
