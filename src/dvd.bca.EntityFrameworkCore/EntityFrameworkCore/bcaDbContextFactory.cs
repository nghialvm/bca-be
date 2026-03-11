using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace dvd.bca.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class bcaDbContextFactory : IDesignTimeDbContextFactory<bcaDbContext>
{
    public bcaDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        bcaEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<bcaDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new bcaDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../dvd.bca.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
