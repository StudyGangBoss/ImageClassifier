using System.Reflection;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureApplicationDi
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services, Configuration configuration)
    {
        services.AddMediatR(
            conf
                =>
            {
                conf.RegisterServicesFromAssemblies(typeof (AddMarkHandler).GetTypeInfo().Assembly);
            }
        );
        return services;
    }
}