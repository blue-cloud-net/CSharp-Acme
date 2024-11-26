using Microsoft.IdentityModel.Tokens;

using Shouldly;

using System.Text;

namespace Acme.Crypto.Jwk;

[Parallelizable(ParallelScope.All)]
public class JsonWebKeyTests
{
    private const string _testEcJwk = "{\"kty\":\"EC\",\"crv\":\"P-256\",\"x\":\"f83OJ3D2xF1Bg8vub9tLe1gHMzV76e8Tus9uPHvRVEU\",\"y\":\"x_FEzRu9m36HLN_tue659LNpXW6pCyStikYjKIWI5a0\",\"d\":\"jpsQnnGQmL-YBIffH1136cspYG6-0iY7X1fCE9-E9LI\"}";
    private const string _testRsaJwk = "{\"kty\":\"RSA\",\"n\":\"ofgWCuLjybRlzo0tZWJjNiuSfb4p4fAkd_wWJcyQoTbji9k0l8W26mPddxHmfHQp-Vaw-4qPCJrcS2mJPMEzP1Pt0Bm4d4QlL-yRT-SFd2lZS-pCgNMsD1W_YpRPEwOWvG6b32690r2jZ47soMZo9wGzjb_7OMg0LOL-bSf63kpaSHSXndS5z5rexMdbBYUsLA9e-KXBdQOS-UTo7WTBEMa2R2CapHg665xsmtdVMTBQY4uDZlxvb3qCo5ZwKh9kG4LT6_I5IhlJH7aGhyxXFvUK-DWNmoudF8NAco9_h9iaGNj8q2ethFkMLs91kzk2PAcDTW9gb54h4FRWyuXpoQ\",\"e\":\"AQAB\",\"d\":\"Eq5xpGnNCivDflJsRQBXHx1hdR1k6Ulwe2JZD50LpXyWPEAeP88vLNO97IjlA7_GQ5sLKMgvfTeXZx9SE-7YwVol2NXOoAJe46sui395IW_GO-pWJ1O0BkTGoVEn2bKVRUCgu-GjBVaYLU6f3l9kJfFNS3E0QbVdxzubSu3Mkqzjkn439X0M_V51gfpRLI9JYanrC4D4qAdGcopV_0ZHHzQlBjudU2QvXt4ehNYTCBr6XCLQUShb1juUO1ZdiYoFaFQT5Tw8bGUl_x_jTj3ccPDVZFD9pIuhLhBOneufuBiB4cS98l2SR_RQyGWSeWjnczT0QU91p1DhOVRuOopznQ\",\"p\":\"4BzEEOtIpmVdVEZNCqS7baC4crd0pqnRH_5IB3jw3bcxGn6QLvnEtfdUdiYrqBdss1l58BQ3KhooKeQTa9AB0Hw_Py5PJdTJNPY8cQn7ouZ2KKDcmnPGBY5t7yLc1QlQ5xHdwW1VhvKn-nXqhJTBgIPgtldC-KDV5z-y2XDwGUc\",\"q\":\"uQPEfgmVtjL0Uyyx88GZFF1fOunH3-7cepKmtH4pxhtCoHqpWmT8YAmZxaewHgHAjLYsp1ZSe7zFYHj7C6ul7TjeLQeZD_YwD66t62wDmpe_HlB-TnBA-njbglfIsRLtXlnDzQkv5dTltRJ11BKBBypeeF6689rjcJIDEz9RWdc\",\"dp\":\"BwKfV3Akq5_MFZDFZCnW-wzl-CCo83WoZvnLQwCTeDv8uzluRSnm71I3QCLdhrqE2e9YkxvuxdBfpT_PI7Yz-FOKnu1R6HsJeDCjn12Sk3vmAktV2zb34MCdy7cpdTh_YVr7tss2u6vneTwrA86rZtu5Mbr1C1XsmvkxHQAdYo0\",\"dq\":\"h_96-mK1R_7glhsum81dZxjTnYynPbZpHziZjeeHcXYsXaaMwkOlODsWa7I9xXDoRwbKgB719rrmI2oKr6N3Do9U0ajaHF-NKJnwgjMd2w9cjz3_-kyNlxAr2v4IKhGNpmM5iIgOS1VZnOZ68m6_pbLBSp3nssTdlqvd0tIiTHU\",\"qi\":\"IYd7DHOhrWvxkwPQsRM2tOgrjbcrfvtQJipd-DlcxyVuuM9sQLdgjVk2oy26F0EmpScGLq2MowX7fhd_QJQ3ydy5cY7YIBi87w93IKLEdfnbJtoOPLUW0ITrJReOgo1cq9SbsxYawBgfp_gh6A5603k2-ZQwVK0JKSHuLFkuQ3U\"}";

    private const string _testContent = "Hello, World!";
    private static readonly byte[] _testData = Encoding.UTF8.GetBytes(_testContent);

    [Test]
    public void JsonWebKeyTests_CreateRsa2048_256Jwk_Test()
    {
        var jwk = RsaJsonWebKey.Create(2048, 256);
        jwk.ShouldNotBeNull();
        Console.WriteLine(jwk);

        jwk.KeyType.ShouldBe(JsonWebAlgorithmsKeyTypes.RSA);
        jwk.Algorithm.ShouldBe("RS256");
        jwk.KeySize.ShouldBe(2048);
    }

    [Test]
    public void JsonWebKeyTests_CreateRsa2048_384Jwk_Test()
    {
        var jwk = RsaJsonWebKey.Create(2048, 384);
        jwk.ShouldNotBeNull();
        Console.WriteLine(jwk);

        jwk.KeyType.ShouldBe(JsonWebAlgorithmsKeyTypes.RSA);
        jwk.Algorithm.ShouldBe("RS384");
        jwk.KeySize.ShouldBe(2048);
    }

    [Test]
    public void JsonWebKeyTests_CreateRsa2048_512Jwk_Test()
    {
        var jwk = RsaJsonWebKey.Create(2048, 512);
        jwk.ShouldNotBeNull();
        Console.WriteLine(jwk);

        jwk.KeyType.ShouldBe(JsonWebAlgorithmsKeyTypes.RSA);
        jwk.Algorithm.ShouldBe("RS512");
        jwk.KeySize.ShouldBe(2048);
    }

    [Test]
    public void JsonWebKeyTests_CreateRsa4096_512Jwk_Test()
    {
        var jwk = RsaJsonWebKey.Create(4096, 512);
        jwk.ShouldNotBeNull();
        Console.WriteLine(jwk);

        jwk.KeyType.ShouldBe(JsonWebAlgorithmsKeyTypes.RSA);
        jwk.Algorithm.ShouldBe("RS512");
        jwk.KeySize.ShouldBe(4096);
    }

    [Test]
    public void JsonWebKeyTests_CreateEc256Jwk_Test()
    {
        var jwk = EcJsonWebKey.Create("P-256");
        jwk.ShouldNotBeNull();

        Console.WriteLine(jwk);

        jwk.KeyType.ShouldBe(JsonWebAlgorithmsKeyTypes.EllipticCurve);
        jwk.Curve.ShouldBe("P-256");
        jwk.X.ShouldNotBeEmpty();
        jwk.Y.ShouldNotBeEmpty();
        jwk.D.ShouldNotBeEmpty();
    }

    [Test]
    public void JsonWebKeyTests_CreateEc384Jwk_Test()
    {
        var jwk = EcJsonWebKey.Create("P-384");
        jwk.ShouldNotBeNull();

        Console.WriteLine(jwk);

        jwk.KeyType.ShouldBe(JsonWebAlgorithmsKeyTypes.EllipticCurve);
        jwk.Curve.ShouldBe("P-384");
        jwk.X.ShouldNotBeEmpty();
        jwk.Y.ShouldNotBeEmpty();
        jwk.D.ShouldNotBeEmpty();
    }

    [Test]
    public void JsonWebKeyTests_CreateEc521Jwk_Test()
    {
        var jwk = EcJsonWebKey.Create("P-521");
        jwk.ShouldNotBeNull();

        Console.WriteLine(jwk);

        jwk.KeyType.ShouldBe(JsonWebAlgorithmsKeyTypes.EllipticCurve);
        jwk.Curve.ShouldBe("P-521");
        jwk.X.ShouldNotBeEmpty();
        jwk.Y.ShouldNotBeEmpty();
        jwk.D.ShouldNotBeEmpty();
    }

    [Test]
    [Repeat(10)]
    [Parallelizable(ParallelScope.Self)]
    public void JsonWebKeyTests_CreateRsaJwk_Test()
    {
        var jwk = RsaJsonWebKey.Create();
        jwk.ShouldNotBeNull();
    }

    [Test]
    [Repeat(10)]
    [Parallelizable(ParallelScope.Self)]
    public void JsonWebKeyTests_CreateEcJwk_Test()
    {
        var jwk = EcJsonWebKey.Create();
        jwk.ShouldNotBeNull();
    }

    //[Test]
    //public void Demo()
    //{
    //    var curve = NistNamedCurves.GetByName("P-521");
    //    var curveOid = NistNamedCurves.GetOid("P-521");
    //    var generator = new ECKeyPairGenerator("ECDSA");
    //    var generatorParameters = new ECKeyGenerationParameters(curveOid, new());
    //    generator.Init(generatorParameters);
    //    var keyPair = generator.GenerateKeyPair();
    //    var privateKey = (ECPrivateKeyParameters)keyPair.Private;
    //    var publicKey = (ECPublicKeyParameters)keyPair.Public;

    //    var signer = SignerUtilities.GetSigner("SHA-512withECDSA");
    //    signer.Init(true, privateKey);
    //    signer.BlockUpdate(_testData);
    //    var signature = signer.GenerateSignature();
    //    signature.ShouldNotBeNull()
    //        .ShouldNotBeEmpty();

    //    signer.Init(false, publicKey);
    //    signer.BlockUpdate(_testData);
    //    signer.VerifySignature(signature)
    //        .ShouldBeTrue();

    //    //var newPrivateKey = new ECPrivateKeyParameters("ECDSA", new BigInteger(privateKey.D.ToByteArrayUnsigned()), curveOid);
    //    //var newSigner = SignerUtilities.GetSigner("SHA-512withECDSA");
    //    //newSigner.Init(true, newPrivateKey);
    //    //newSigner.BlockUpdate(_testData);
    //    //var newSignature = newSigner.GenerateSignature();

    //    //signature.ShouldBeEquivalentTo(newSignature);

    //    //var point = curve.Curve.CreatePoint(new(publicKey.Q.AffineXCoord.ToBigInteger().ToByteArrayUnsigned()), new(publicKey.Q.AffineYCoord.ToBigInteger().ToByteArrayUnsigned()));
    //    var point = curve.Curve.CreatePoint(new(publicKey.Q.XCoord.ToBigInteger().ToByteArrayUnsigned()), new(publicKey.Q.YCoord.ToBigInteger().ToByteArrayUnsigned()));
    //    //var point = curve.Curve.CreatePoint(new(publicKey.Q.XCoord.ToBigInteger().ToByteArray()), new(publicKey.Q.YCoord.ToBigInteger().ToByteArray()));
    //    var newPublicKey = new ECPublicKeyParameters("ECDSA", point, curveOid);
    //    signer = SignerUtilities.GetSigner("SHA-512withECDSA");
    //    signer.Init(false, newPublicKey);
    //    signer.BlockUpdate(_testData);
    //    signer.VerifySignature(signature)
    //        .ShouldBeTrue();
    //}

    [Test]
    public void JsonWebKeyTests_CreateEcJwkAndExportThenImport_Test()
    {
        var jwk = EcJsonWebKey.Create();
        jwk.ShouldNotBeNull();

        var jwkString = jwk.ToString();
        jwkString.ShouldNotBeNullOrWhiteSpace();

        var newJwk = JsonWebKey.Parse(jwkString);
        newJwk.ShouldNotBeNull();
        newJwk.ShouldBeAssignableTo<EcJsonWebKey>();

        var ecJwk = (EcJsonWebKey)newJwk;

        jwk.KeyType.ShouldBe(ecJwk.KeyType);
        jwk.Curve.ShouldBe(ecJwk.Curve);
        jwk.X.ShouldBeEquivalentTo(ecJwk.X);
        jwk.Y.ShouldBeEquivalentTo(ecJwk.Y);
        jwk.D.ShouldBeEquivalentTo(ecJwk.D);
    }

    [Test]
    public void JsonWebKeyTests_CreateEcJwkAndExportThenImportSign_Test()
    {
        var jwk = EcJsonWebKey.Create();
        jwk.ShouldNotBeNull();

        var signed = jwk.GenerateSignature(_testData);

        var jwkString = jwk.ToString();
        jwkString.ShouldNotBeNullOrWhiteSpace();

        var newJwk = JsonWebKey.Parse(jwkString);
        newJwk.ShouldNotBeNull();

        newJwk.VerifySignature(_testData, signed)
            .ShouldBeTrue();
    }

    [Test]
    public void JsonWebKeyTests_SelfSignAndSelfVerifyRsaKey_Test()
    {
        var jwk = JsonWebKey.Parse(_testRsaJwk);
        jwk.SetAlgorithm("RS256");
        var signed = jwk.GenerateSignature(_testData);

        signed.ShouldNotBeNull()
            .ShouldNotBeEmpty();

        jwk.VerifySignature(_testData, signed)
            .ShouldBeTrue();
    }

    [Test]
    public void JsonWebKeyTests_MicrosoftSignAndMicrosoftVerifyRsaKey_Test()
    {
        var jwk = Microsoft.IdentityModel.Tokens.JsonWebKey.Create(_testRsaJwk);
        using var signatureProvider = new AsymmetricSignatureProvider(jwk, "RS256");

        var signed = signatureProvider.Sign(_testData);

        signed.ShouldNotBeNull()
            .ShouldNotBeEmpty();

        signatureProvider.Verify(_testData, signed)
            .ShouldBeTrue();
    }

    [Test]
    public void JsonWebKeyTests_SelfSignAndMicrosoftVerifyRsaKey_Test()
    {
        var jwk = JsonWebKey.Parse(_testRsaJwk);
        jwk.SetAlgorithm("RS256");
        var signed = jwk.GenerateSignature(_testData);
        signed.ShouldNotBeNull()
            .ShouldNotBeEmpty();

        var otherJwk = Microsoft.IdentityModel.Tokens.JsonWebKey.Create(_testRsaJwk);
        using var signatureProvider = new AsymmetricSignatureProvider(otherJwk, "RS256");
        signatureProvider.Verify(_testData, signed)
            .ShouldBeTrue();
    }

    [Test]
    public void JsonWebKeyTests_MicrosoftSignAndSelfVerifyRsaKey_Test()
    {
        var otherJwk = Microsoft.IdentityModel.Tokens.JsonWebKey.Create(_testRsaJwk);
        using var signatureProvider = new AsymmetricSignatureProvider(otherJwk, "RS256");
        var signed = signatureProvider.Sign(_testData);
        signed.ShouldNotBeNull()
            .ShouldNotBeEmpty();

        var jwk = JsonWebKey.Parse(_testRsaJwk);
        jwk.SetAlgorithm("RS256");
        jwk.VerifySignature(_testData, signed)
            .ShouldBeTrue();
    }

    [Test]
    public void JsonWebKeyTests_SelfSignAndSelfVerifyEcKey_Test()
    {
        var jwk = JsonWebKey.Parse(_testEcJwk);
        var signed = jwk.GenerateSignature(_testData);
        signed.ShouldNotBeNull()
            .ShouldNotBeEmpty();

        jwk.VerifySignature(_testData, signed)
            .ShouldBeTrue();
    }

    [Test]
    public void JsonWebKeyTests_MicrosoftSignAndMicrosoftVerifyEcKey_Test()
    {
        var jwk = Microsoft.IdentityModel.Tokens.JsonWebKey.Create(_testEcJwk);
        using var signatureProvider = new AsymmetricSignatureProvider(jwk, "ES256");

        var signed = signatureProvider.Sign(_testData);

        signed.ShouldNotBeNull()
            .ShouldNotBeEmpty();

        signatureProvider.Verify(_testData, signed)
            .ShouldBeTrue();
    }

    [Test]
    public void JsonWebKeyTests_SelfSignAndMicrosoftVerifyEcKey_Test()
    {
        var jwk = JsonWebKey.Parse(_testEcJwk);
        var signed = jwk.GenerateSignature(_testData);
        signed.ShouldNotBeNull()
            .ShouldNotBeEmpty();

        var otherJwk = Microsoft.IdentityModel.Tokens.JsonWebKey.Create(_testEcJwk);
        using var signatureProvider = new AsymmetricSignatureProvider(otherJwk, "ES256");
        signatureProvider.Verify(_testData, signed)
            .ShouldBeTrue();
    }

    [Test]
    public void JsonWebKeyTests_MicrosoftSignAndSelfVerifyEcKey_Test()
    {
        var otherJwk = Microsoft.IdentityModel.Tokens.JsonWebKey.Create(_testEcJwk);
        using var signatureProvider = new AsymmetricSignatureProvider(otherJwk, "ES256");
        var signed = signatureProvider.Sign(_testData);
        signed.ShouldNotBeNull()
            .ShouldNotBeEmpty();

        var jwk = JsonWebKey.Parse(_testEcJwk);
        jwk.VerifySignature(_testData, signed)
            .ShouldBeTrue();
    }
}
