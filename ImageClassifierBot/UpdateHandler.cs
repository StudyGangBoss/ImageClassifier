using Application;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ImageClassifierBot;

public class UpdateHandler : IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private ISender sender;

    public UpdateHandler(ITelegramBotClient botClient, ISender sender)
    {
        _botClient = botClient;
        this.sender = sender;
    }

    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ApiRequestException apiRequestException)
        {
            await botClient.SendTextMessageAsync(123, apiRequestException.ToString());
        }
    }

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken ct)
    {
        var handler = update switch
        {
            { Message: {} message } => BotOnMessageReceived(message, ct),
            { CallbackQuery: {} callbackQuery } => BotOnCallbackQueryReceived(callbackQuery, ct),
        };
        await handler;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken ct)
    {
        await sender.Send(new AddUserCommand(message!.Chat.Id), ct);

        if (message.Text is not {} messageText)
            return;

        var action = messageText.Split(' ')[0] switch
        {
            "/start" => GenerateImageMessage(_botClient, message, ct),
            "/help" => Usage(_botClient, message, ct)
        };
    }

    static async Task<Message> Usage(ITelegramBotClient botClient, Message message, CancellationToken ct)
    {
        const string usage = "Usage:\n" +
                             "/inline_keyboard - send inline keyboard\n";

        return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: usage,
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: ct);
    }

    private async Task<Message> GenerateImageMessage(ITelegramBotClient botClient, Message message, CancellationToken ct)
    {
        var chatId = message.Chat.Id;
        var classResult = await sender.Send(new GetClassAndImageToMarkCommand(chatId));

        await botClient.SendChatActionAsync(
            message.Chat.Id,
            ChatAction.UploadPhoto,
            cancellationToken: ct);

        var markup = new InlineKeyboardMarkup(GenerateKeyboard(classResult.ImageClassification.Id));
        var stream = new MemoryStream(classResult.Image.ImageData);
        return await botClient.SendPhotoAsync(
            chatId: message.Chat.Id,
            photo: new InputFileStream(stream, classResult.Image.Id + ".jpg"),
            caption: classResult.ImageClassification.ClassificationType.Question,
            replyMarkup: markup,
            cancellationToken: ct);
    }

    private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken ct)
    {
        var data = callbackQuery.Data;
        if (data is null)
            return;
        var classificationId = Guid.Parse(data.Split(' ')[0]);
        var mark = Int32.Parse(data.Split(' ')[1]);

        await sender.Send(new SetMarkCommand(classificationId, mark), ct);
        await _botClient.AnswerCallbackQueryAsync(
            callbackQuery.Id,
            $"Received {callbackQuery.Data}",
            cancellationToken: ct);

        await _botClient.SendTextMessageAsync(
            callbackQuery.Message!.Chat.Id,
            $"Received {callbackQuery.Data}",
            cancellationToken: ct);
    }

    private IEnumerable<InlineKeyboardButton> GenerateKeyboard(Guid classificationId)
    {
        for (var i = 0; i < 5; i++)
        {
            yield return InlineKeyboardButton.WithCallbackData(i.ToString(), $"{classificationId} {i.ToString()}");
        }
    }
}