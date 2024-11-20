using Volo.Abp.Authorization.Permissions;

namespace Acme.Client.Permissions;

public class ClientPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ClientPermissions.GroupName);
    }
}
