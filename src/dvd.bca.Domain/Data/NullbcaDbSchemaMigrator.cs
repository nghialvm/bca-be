using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace dvd.bca.Data;

/* This is used if database provider does't define
 * IbcaDbSchemaMigrator implementation.
 */
public class NullbcaDbSchemaMigrator : IbcaDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
