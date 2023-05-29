using Application;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ImageClassifierBot.Services;

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
            _ => Task.CompletedTask
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

        if (message.Photo is not null)
        {
            await AddImage(message, ct);
        }

        if (message.Text is not {} messageText)
            return;
        var action = messageText.Split(' ')[0] switch
        {
            "/start" => GenerateImageMessage(_botClient, message, ct),
            "/help" => Usage(_botClient, message, ct),
            "/class" => AddClass(_botClient, message, ct),
            _ => Usage(_botClient, message, ct)
        };
    }

    private async Task<Message> AddImage(Message message, CancellationToken ct)
    {
        var file = await _botClient.GetFileAsync(message.Photo.Last().FileId, ct);
        var imageStream = new MemoryStream();
        await _botClient.DownloadFileAsync(file.FilePath!, imageStream, ct);
        var addImageResult = await sender.Send(new AddImageCommand(message.Chat.Id, imageStream), ct);

        if (addImageResult.Image is not null)
            return await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Изображение добавлено с Id {addImageResult.Image.Id}",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: ct);
        return null;
    }

    async Task<Message> AddClass(ITelegramBotClient botClient, Message message, CancellationToken ct)
    {
        var request = string.Join(" ", message.Text!.Split(' ').Skip(1));
        var classType = request.Split(';')[0];
        var question = request.Split(';')[1];
        var addClassResult = await sender.Send(new AddClassCommand(message.Chat.Id, question, classType), ct);

        if (addClassResult.id is not null)
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Класс {classType} добавлен с Id {addClassResult.id}",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: ct);
        return null;
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
            $"Спасибо за оценку {callbackQuery.Message.Chat.Id}",
            cancellationToken: ct);
        await GenerateImageMessage(_botClient, callbackQuery.Message, ct);
    }

    private IEnumerable<InlineKeyboardButton> GenerateKeyboard(Guid classificationId)
    {
        for (var i = 0; i < 5; i++)
        {
            yield return InlineKeyboardButton.WithCallbackData(i.ToString(), $"{classificationId} {i.ToString()}");
        }
    }
}