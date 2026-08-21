global using Acme.Protocol.Const;
global using Acme.Protocol.Enums;
global using Acme.Protocol.Extensions;
global using Acme.Protocol.Utils;

global using Microsoft.IdentityModel.Tokens;

global using Org.BouncyCastle.Asn1;
global using Org.BouncyCastle.Asn1.Nist;
global using Org.BouncyCastle.Asn1.Pkcs;
global using Org.BouncyCastle.Asn1.Sec;
global using Org.BouncyCastle.Asn1.X509;
global using Org.BouncyCastle.Asn1.X9;
global using Org.BouncyCastle.Crypto;
global using Org.BouncyCastle.Crypto.Digests;
global using Org.BouncyCastle.Crypto.Engines;
global using Org.BouncyCastle.Crypto.Generators;
global using Org.BouncyCastle.Crypto.Parameters;
global using Org.BouncyCastle.Crypto.Signers;
global using Org.BouncyCastle.Math;
global using Org.BouncyCastle.OpenSsl;
global using Org.BouncyCastle.Pkcs;
global using Org.BouncyCastle.Security;
global using Org.BouncyCastle.X509;
global using Org.BouncyCastle.X509.Extension;

global using System.ComponentModel.DataAnnotations;
global using System.Net;
global using System.Reflection;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Text.RegularExpressions;

global using JsonWebKey = Acme.Protocol.Jwk.JsonWebKey;
global using RS = Acme.Protocol.Resources.AcmeProtocolShared;