using Volo.Abp.Modularity;

namespace dvd.bca;

public abstract class bcaApplicationTestBase<TStartupModule> : bcaTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
