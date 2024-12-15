namespace Acme.Client.Contexts;

/// <inheritdoc/>
public class AcmeAccountContext : IAcmeAccountContext
{
    public AcmeAccountContext(
        ISigner signer)
    {
        Signer = signer;
    }

    /// <inheritdoc/>
    public ISigner Signer { get; set; }

    /// <inheritdoc/>
    public AccountModel? Account { get; set; }
}
