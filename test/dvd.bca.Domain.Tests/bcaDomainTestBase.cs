using Volo.Abp.Modularity;

namespace dvd.bca;

/* Inherit from this class for your domain layer tests. */
public abstract class bcaDomainTestBase<TStartupModule> : bcaTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
