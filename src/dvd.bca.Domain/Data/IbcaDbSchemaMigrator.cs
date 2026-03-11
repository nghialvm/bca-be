using System.Threading.Tasks;

namespace dvd.bca.Data;

public interface IbcaDbSchemaMigrator
{
    Task MigrateAsync();
}
