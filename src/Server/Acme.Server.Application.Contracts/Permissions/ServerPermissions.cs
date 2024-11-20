using Volo.Abp.Reflection;

namespace Acme.Server.Permissions;

public class ServerPermissions
{
    public const string GroupName = "Server";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ServerPermissions));
    }
}
