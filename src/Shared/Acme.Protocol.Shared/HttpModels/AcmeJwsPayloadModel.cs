namespace Acme.HttpModels;

/// <summary>
/// Acme的Jws的payload部分
/// </summary>
/// <typeparam name="TPayload"></typeparam>
public class AcmeJwsPayloadModel<TPayload>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="value"></param>
    public AcmeJwsPayloadModel(TPayload value)
    {
        Value = value;
    }

    /// <summary>
    /// 负载实际内容
    /// </summary>
    public TPayload Value { get; }
}
