using dvd.bca.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace dvd.bca.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(bcaEntityFrameworkCoreModule),
    typeof(bcaApplicationContractsModule)
)]
public class bcaDbMigratorModule : AbpModule
{
}
