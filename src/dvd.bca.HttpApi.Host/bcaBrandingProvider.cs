using Microsoft.Extensions.Localization;
using dvd.bca.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace dvd.bca;

[Dependency(ReplaceServices = true)]
public class bcaBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<bcaResource> _localizer;

    public bcaBrandingProvider(IStringLocalizer<bcaResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
