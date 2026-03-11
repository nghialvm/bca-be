using Volo.Abp.Modularity;

namespace dvd.bca;

[DependsOn(
    typeof(bcaDomainModule),
    typeof(bcaTestBaseModule)
)]
public class bcaDomainTestModule : AbpModule
{

}
