using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Expose private members to "friend" testing assemblies
[assembly: InternalsVisibleTo("Acme.Protocol.Shared.UnitTests")]
[assembly: InternalsVisibleTo("Acme.Protocol.Shared.IntegrationTests")]

[assembly: Guid("137504b6-44a9-4d75-9abd-4b608c9c7a84")]
