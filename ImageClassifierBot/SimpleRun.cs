using Infrastructure;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace ImageClassifierBot;

public class SimpleRun
{
    public async Task Run()
    {
        var sc = new ServiceCollection();
        var config = new Configuration();
        sc.ConfigureBot(config);
        var sp = sc.BuildServiceProvider();
        var updateHandler=sp.GetRequiredService<IUpdateHandler>();
        var bot=sp.GetRequiredService<ITelegramBotClient>();
        var db = sp.GetRequiredService<ImageClassifierContext>();


        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = {} // receive all update types
        };

        try
        {
            await bot.ReceiveAsync(
                updateHandler,
                receiverOptions,
                cancellationToken
            );
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }
    }
}