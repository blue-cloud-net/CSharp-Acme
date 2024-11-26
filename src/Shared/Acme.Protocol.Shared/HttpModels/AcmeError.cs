using Acme.Const;

using System.Net;

namespace Acme.HttpModels;

/// <summary>
/// Acme错误
/// AcmeError <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-6.7"/>
/// </summary>
public class AcmeError
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="type"></param>
    /// <param name="detail"></param>
    public AcmeError(string type, string detail)
    {
        Type = type;
        Detail = detail;
    }

    /// <summary>
    /// 错误类型
    /// 以 urn:ietf:params:acme:error: 开头
    /// 可参照<see cref="AcmeErrorTypes"/>
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// 详细错误信息
    /// </summary>
    public string Detail { get; set; }

    /// <summary>
    /// 子问题
    /// </summary>
    public List<AcmeError>? Subproblems { get; set; }

    /// <summary>
    /// 标识
    /// </summary>
    public IdentifierModel? Identifier { get; set; }

    /// <summary>
    /// 状态码
    /// </summary>
    [JsonIgnore]
    public HttpStatusCode StatusCode { get; set; }
}
