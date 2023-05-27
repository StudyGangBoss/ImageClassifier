using Ardalis.Specification;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureInfrastructureDi
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, Configuration configuration)
    {
        // services.AddDbContext<ImageClassifierContext>(
        //     options =>
        //         options.UseNpgsql(configuration.ConnectionString));
        services.AddDbContext<ImageClassifierContext>(options => options.UseInMemoryDatabase(databaseName: "image_classifier"));
        services.AddSingleton(configuration);
        services.AddScoped(typeof (IRepositoryBase<>), typeof (Repository<>));
        services.AddScoped(typeof (Repository<>));
        return services;
    }
}