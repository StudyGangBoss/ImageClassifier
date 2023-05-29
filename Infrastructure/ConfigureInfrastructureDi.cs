using System.Data.Common;
using System.Runtime.InteropServices;
using Ardalis.Specification;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure;

public static class ConfigureInfrastructureDi
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, Configuration configuration)
    {
        var d = new NpgsqlConnectionStringBuilder();
        d.Host = "localhost";
        d.Password = "123";
        d.Port = 5432;
        d.Username = "postgres";
        d.Database = "postgres";

        var a = d.ConnectionString;
        services.AddDbContext<ImageClassifierContext>(
            options =>
                options.UseNpgsql(d.ConnectionString).EnableSensitiveDataLogging());
        // services.AddDbContext<ImageClassifierContext>(options => options.UseInMemoryDatabase(databaseName: "image_classifier"));
        services.AddSingleton(configuration);
        services.AddTransient(typeof (IReadRepository<>), typeof (Repository<>));
        services.AddTransient(typeof (Repository<>));
        return services;
    }
}