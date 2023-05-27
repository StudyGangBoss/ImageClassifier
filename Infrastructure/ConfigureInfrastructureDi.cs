using System.Data.Common;
using System.Runtime.InteropServices;
using Ardalis.Specification;
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
        d.Port=5432;
        d.Username = "postgres";
        d.Database = "postgres";

       var a= d.ConnectionString;
        services.AddDbContext<ImageClassifierContext>(
            options =>
                options.UseNpgsql(d.ConnectionString));
       // services.AddDbContext<ImageClassifierContext>(options => options.UseInMemoryDatabase(databaseName: "image_classifier"));
        services.AddSingleton(configuration);
        services.AddScoped(typeof (IRepositoryBase<>), typeof (Repository<>));
        services.AddScoped(typeof (Repository<>));
        return services;
    }
}