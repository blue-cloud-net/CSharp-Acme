using Volo.Abp.Application.Services;

namespace Acme.Server;

public abstract class ServerAppService : ApplicationService
{
    protected ServerAppService()
    {
        this.ObjectMapperContext = typeof(ServerApplicationModule);
    }
}
