using Volo.Abp.Reflection;

namespace Acme.Client.Permissions;

public class ClientPermissions
{
    public const string GroupName = "Client";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ClientPermissions));
    }
}
