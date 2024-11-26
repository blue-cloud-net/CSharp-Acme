namespace Acme.Const;

/// <summary>
/// Acme错误类型
/// </summary>
public static class AcmeErrorTypes
{
    /// <summary>
    /// The request specified an account that does not exist
    /// </summary>
    public const string AccountDoesNotExist = "accountDoesNotExist";

    /// <summary>
    /// The request specified a certificate to be revoked that has already been revoked
    /// </summary>
    public const string AlreadyRevoked = "alreadyRevoked";

    /// <summary>
    /// The CSR is unacceptable (e.g., due to a short key)
    /// </summary>
    public const string BadCSR = "badCSR";

    /// <summary>
    /// The client sent an unacceptable antireplay nonce
    /// </summary>
    public const string BadNonce = "badNonce";

    /// <summary>
    /// The JWS was signed by a public key the server does not support
    /// </summary>
    public const string BadPublicKey = "badPublicKey";

    /// <summary>
    /// The revocation reason provided is not allowed by the server
    /// </summary>
    public const string BadRevocationReason = "badRevocationReason";

    /// <summary>
    /// The JWS was signed with an algorithm the server does not support
    /// </summary>
    public const string BadSignatureAlgorithm = "badSignatureAlgorithm";

    /// <summary>
    /// Certification Authority Authorization (CAA) records forbid the CA from issuing a certificate
    /// </summary>
    public const string Caa = "caa";

    /// <summary>
    /// Specific error conditions are indicated in the "subproblems" array
    /// </summary>
    public const string Compound = "compound";

    /// <summary>
    /// The server could not connect to validation target
    /// </summary>
    public const string Connection = "connection";

    /// <summary>
    /// There was a problem with a DNS query during identifier validation
    /// </summary>
    public const string Dns = "dns";

    /// <summary>
    /// The request must include a value for the "externalAccountBinding" field
    /// </summary>
    public const string ExternalAccountRequired = "externalAccountRequired";

    /// <summary>
    /// Response received didn't match the challenge's requirements
    /// </summary>
    public const string IncorrectResponse = "incorrectResponse";

    /// <summary>
    /// A contact URL for an account was invalid
    /// </summary>
    public const string InvalidContact = "invalidContact";

    /// <summary>
    /// The request message was malformed
    /// </summary>
    public const string Malformed = "malformed";

    /// <summary>
    /// The request attempted to finalize an order that is not ready to be finalized
    /// </summary>
    public const string OrderNotReady = "orderNotReady";

    /// <summary>
    /// The request exceeds a rate limit
    /// </summary>
    public const string RateLimited = "rateLimited";

    /// <summary>
    /// The server will not issue certificates for the identifier
    /// </summary>
    public const string RejectedIdentifier = "rejectedIdentifier";

    /// <summary>
    /// The server experienced an internal error
    /// </summary>
    public const string ServerInternal = "serverInternal";

    /// <summary>
    /// The server received a TLS error during validation
    /// </summary>
    public const string Tls = "tls";

    /// <summary>
    /// The client lacks sufficient authorization
    /// </summary>
    public const string Unauthorized = "unauthorized";

    /// <summary>
    /// A contact URL for an account used an unsupported protocol scheme
    /// </summary>
    public const string UnsupportedContact = "unsupportedContact";

    /// <summary>
    /// An identifier is of an unsupported type
    /// </summary>
    public const string UnsupportedIdentifier = "unsupportedIdentifier";

    /// <summary>
    /// Visit the "instance" URL and take actions specified there
    /// </summary>
    public const string UserActionRequired = "userActionRequired";

    /// <summary>
    /// The request message was syntactically incorrect
    /// </summary>
    public const string Unknown = "unknown";
}
