using Volo.Abp.Application.Services;

namespace Acme.Client;

public abstract class ClientAppService : ApplicationService
{
    protected ClientAppService()
    {
        ObjectMapperContext = typeof(ClientApplicationModule);
    }
}
