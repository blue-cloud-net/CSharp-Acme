using Microsoft.IdentityModel.Tokens;

using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;

using Shouldly;

namespace Acme.Crypto.Jwk;

[Parallelizable(ParallelScope.All)]
public class JsonWebKeyRfcExampleTests
{
    /// <summary>
    /// <see href="https://www.rfc-editor.org/rfc/rfc7515.html#appendix-A.1.1"/>测试"/>
    /// </summary>
    [Test]
    public void JsonWebKeyPreTests_HmacSignTest()
    {
        var awaitingSignString = "eyJ0eXAiOiJKV1QiLA0KICJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJqb2UiLA0KICJleHAiOjEzMDA4MTkzODAsDQogImh0dHA6Ly9leGFtcGxlLmNvbS9pc19yb290Ijp0cnVlfQ";
        var awaitingSignBytes = Encoding.UTF8.GetBytes(awaitingSignString);

        awaitingSignBytes.ShouldNotBeEmpty();
        awaitingSignBytes.ShouldBe(
            [101, 121, 74, 48, 101, 88, 65, 105, 79, 105, 74, 75, 86, 49, 81,
            105, 76, 65, 48, 75, 73, 67, 74, 104, 98, 71, 99, 105, 79, 105, 74,
            73, 85, 122, 73, 49, 78, 105, 74, 57, 46, 101, 121, 74, 112, 99, 51,
            77, 105, 79, 105, 74, 113, 98, 50, 85, 105, 76, 65, 48, 75, 73, 67,
            74, 108, 101, 72, 65, 105, 79, 106, 69, 122, 77, 68, 65, 52, 77, 84,
            107, 122, 79, 68, 65, 115, 68, 81, 111, 103, 73, 109, 104, 48, 100,
            72, 65, 54, 76, 121, 57, 108, 101, 71, 70, 116, 99, 71, 120, 108, 76,
            109, 78, 118, 98, 83, 57, 112, 99, 49, 57, 121, 98, 50, 57, 48, 73,
            106, 112, 48, 99, 110, 86, 108, 102, 81]);

        var keyString = "AyM1SysPpbyDfgZld3umj1qzKObwVMkoqQ-EstJQLr_T-1qS0gZH75aKtMN3Yj0iPS4hcgUuTwjAzZr1Z9CAow";
        var keyBytes = Base64UrlEncoder.DecodeBytes(keyString);
        var key = new KeyParameter(keyBytes);
        var hmac = new HmacSigner(new Sha256Digest());
        hmac.Init(true, key);
        hmac.BlockUpdate(awaitingSignBytes, 0, awaitingSignBytes.Length);
        var signature = hmac.GenerateSignature();

        signature.ShouldNotBeEmpty();
        signature.ShouldBe(
            [116, 24, 223, 180, 151, 153, 224, 37, 79, 250, 96, 125, 216, 173,
            187, 186, 22, 212, 37, 77, 105, 214, 191, 240, 91, 88, 5, 88, 83,
            132, 141, 121]);
    }
}
