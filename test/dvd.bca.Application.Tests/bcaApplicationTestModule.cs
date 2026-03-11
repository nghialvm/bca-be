using Volo.Abp.Modularity;

namespace dvd.bca;

[DependsOn(
    typeof(bcaApplicationModule),
    typeof(bcaDomainTestModule)
)]
public class bcaApplicationTestModule : AbpModule
{

}
