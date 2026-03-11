using dvd.bca.Localization;
using Volo.Abp.Application.Services;

namespace dvd.bca;

/* Inherit your application services from this class.
 */
public abstract class bcaAppService : ApplicationService
{
    protected bcaAppService()
    {
        LocalizationResource = typeof(bcaResource);
    }
}
