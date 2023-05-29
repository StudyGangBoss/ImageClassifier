using Application;
using ImageClassifierBot.Services;
using Infrastructure;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace ImageClassifierBot;

public static class ConfigureDi
{
    public static IServiceCollection ConfigureBot(this IServiceCollection services, Configuration configuration)
    {
        services.ConfigureInfrastructure(configuration);
        services.ConfigureApplication(configuration);
        services.AddScoped<ITelegramBotClient>(sp => new TelegramBotClient(sp.GetRequiredService<Configuration>().TelegramToken));
        services.AddScoped<IUpdateHandler,UpdateHandler>();

        return services;
    }
}