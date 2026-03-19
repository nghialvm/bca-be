using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.Emailing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.MailKit;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace dvd.bca;

[DependsOn(
    typeof(bcaDomainModule),
    typeof(bcaApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpEmailingModule),
    typeof(AbpMailKitModule)
)]
public class bcaApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<bcaApplicationModule>();
        });
    }
}
