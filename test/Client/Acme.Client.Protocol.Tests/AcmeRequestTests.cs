using Acme.Client.Client;
using Acme.Client.Contexts;
using Acme.Crypto;
using Acme.Crypto.Jwk;
using Acme.Json;

using Microsoft.Extensions.DependencyInjection;

using System.Text.Json;

namespace Acme.Client;

public class AcmeRequestTests : AcmeTestBase<AcmeClientProtocolModule>
{
    public void GenerateAccountKey()
    {
    }

    [Test]
    public void AcmeRequestTests_CreateClientWithNewKey_Test()
    {
        var jwk = EcJsonWebKey.Create("P-256");
        var signer = new DefaultJwkSigner(jwk);
        var accountContext = new AcmeAccountContext(signer);
        var client = ActivatorUtilities.CreateInstance<AcmeProtocolClient>(base.ServiceProvider, (IAcmeAccountContext)accountContext);

        client.ShouldNotBeNull();
    }

    [Test]
    public async Task AcmeRequestTests_CreateAccountWithNewKey_TestAsync()
    {
        var jwk = EcJsonWebKey.Create("P-256");
        var signer = new DefaultJwkSigner(jwk);
        var accountContext = new AcmeAccountContext(signer);
        var client = ActivatorUtilities.CreateInstance<AcmeProtocolClient>(base.ServiceProvider, (IAcmeAccountContext)accountContext);

        client.AcmeHttpClient.BeforeHttpSend += (sender, args)
            => Console.WriteLine(args.Content?.ReadAsStringAsync().GetAwaiter().GetResult());

        client.ShouldNotBeNull();

        Console.WriteLine(JsonSerializer.Serialize(jwk, JsonSerializerUtil.DefaultOptions));

        var account = await client.CreateAccountAsync(
            new()
            {
                Contacts = ["mailto:651494458@qq.com"],
                TermsOfServiceAgreed = true
            });

        account.ShouldNotBeNull();
    }

    [Test]
    public async Task AcmeRequestTests_GetAccount_TestAsync()
    {
        Assert.Pass();
    }
}
