using Volo.Abp.Authorization.Permissions;

namespace Acme.Server.Permissions;

public class ServerPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ServerPermissions.GroupName);
    }
}
