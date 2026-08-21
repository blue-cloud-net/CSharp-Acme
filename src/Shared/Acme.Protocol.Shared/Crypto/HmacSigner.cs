using Org.BouncyCastle.Crypto.Macs;

namespace Acme.Protocol.Crypto;

/// <summary>
/// HMAC 签名器
/// </summary>
public class HmacSigner : Org.BouncyCastle.Crypto.ISigner
{
    private readonly IDigest _digest;
    private readonly HMac _hmac;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="digest"></param>
    public HmacSigner(IDigest digest)
    {
        _digest = digest;
        _hmac = new HMac(digest);
    }

    /// <inheritdoc/>
    public virtual string AlgorithmName
    {
        get { return _digest.AlgorithmName + "withHMAC"; }
    }

    /// <inheritdoc/>
    public void BlockUpdate(byte[] input, int inOff, int inLen) => _hmac.BlockUpdate(input, inOff, inLen);

#if !NETSTANDARD2_0 && !NETSTANDARD2_1

    /// <inheritdoc/>
    public void BlockUpdate(ReadOnlySpan<byte> input) => _hmac.BlockUpdate(input);

#endif

    /// <inheritdoc/>
    public byte[] GenerateSignature()
    {
        var resBuf = new byte[_hmac.GetMacSize()];
        _hmac.DoFinal(resBuf, 0);
        return resBuf;
    }

    /// <inheritdoc/>
    public int GetMaxSignatureSize() => _hmac.GetMacSize();

    /// <inheritdoc/>
    public void Init(bool forSigning, ICipherParameters parameters) => _hmac.Init(parameters);

    /// <inheritdoc/>
    public void Reset() => _hmac.Reset();

    /// <inheritdoc/>
    public void Update(byte input) => _hmac.Update(input);

    /// <inheritdoc/>
    public bool VerifySignature(byte[] signature)
    {
        var resBuf = new byte[_hmac.GetMacSize()];
        _hmac.DoFinal(resBuf, 0);
        return resBuf.SequenceEqual(signature);
    }
}
