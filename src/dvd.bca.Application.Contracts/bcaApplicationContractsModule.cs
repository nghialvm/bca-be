using Volo.Abp.Account;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace dvd.bca;

[DependsOn(
    typeof(bcaDomainSharedModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
)]
public class bcaApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        bcaDtoExtensions.Configure();
    }
}
