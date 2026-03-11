using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using dvd.bca.Data;
using Volo.Abp.DependencyInjection;

namespace dvd.bca.EntityFrameworkCore;

public class EntityFrameworkCorebcaDbSchemaMigrator
    : IbcaDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCorebcaDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the bcaDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<bcaDbContext>()
            .Database
            .MigrateAsync();
    }
}
