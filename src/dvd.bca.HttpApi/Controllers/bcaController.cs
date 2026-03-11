using dvd.bca.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace dvd.bca.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class bcaController : AbpControllerBase
{
    protected bcaController()
    {
        LocalizationResource = typeof(bcaResource);
    }
}
