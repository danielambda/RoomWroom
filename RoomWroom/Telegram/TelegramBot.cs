using System.Collections.Concurrent;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

using RoomWroom.CommandHandling;

namespace RoomWroom.Telegram;

public class TelegramBot(
    string token, Func<long, IResponseProvider> responseProviderFactoryMethod, bool needsToReply = true)
{
    private readonly bool _needsToReply = needsToReply;

    private readonly TelegramBotClient _botClient = new(token);
   
    private readonly Func<long, IResponseProvider> _responseProviderFactoryMethod = responseProviderFactoryMethod;    
    private readonly ConcurrentDictionary<long, IResponseProvider> _responseProvidersByChatId = [];

    public async void Run()
    {
        using CancellationTokenSource cancellationTokenSource = new();

        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = []
        };

        _botClient.StartReceiving(
            HandleUpdateAsync, HandlePollingErrorAsync,
            receiverOptions, cancellationTokenSource.Token);

        User me = await _botClient.GetMeAsync(cancellationToken: cancellationTokenSource.Token);
        Console.WriteLine($"Hello! My user id is {me.Id}, and my name is {me.Username}.");
        Console.Read();

        await cancellationTokenSource.CancelAsync();
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is { } message)
            await HandleMessageAsync(botClient, message, cancellationToken);

        if (update.CallbackQuery is { } callbackQuery)
            await HandleCallbackQueryAsync(botClient, callbackQuery, cancellationToken); 
    }

    private async Task HandleMessageAsync(ITelegramBotClient botClient, Message message,
        CancellationToken cancellationToken)
    {
        string? messageText = message.GetTextOrCaption();
        Image? image = await message.GetImageAsync(botClient, cancellationToken);

        long chatId = message.Chat.Id;

        IResponseProvider responseProvider =
            _responseProvidersByChatId.GetOrAdd(chatId, _responseProviderFactoryMethod);

        Task<Response>? responseTask = responseProvider.GetResponseTask(messageText, image);
        if (responseTask == null)
            return;
        Response response = await responseTask;
        IEnumerable<MessageData> messagesData = response.GetMessagesData();

        await foreach (Message _ in 
                       botClient.SendTextMessagesAsync(
                           messagesData: messagesData,
                           chatId: chatId,
                           messageThreadId: message.MessageThreadId,
                           replyToMessageId: _needsToReply ? message.MessageId : null,
                           cancellationToken: cancellationToken));
    }

    private static async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        CancellationToken cancellationToken)
    {
        if (callbackQuery.Message is not { } message)
            return;

        if (callbackQuery.Data == "GroceryItemMarkAsShared")
        {
            await botClient.SendTextMessageAsync(
                message.Chat.Id,
                message.Text ?? "nothing there",
                cancellationToken: cancellationToken);
        }
    }

    private static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        string errorMessage = exception switch
        {
            ApiRequestException apiRequestException =>
                $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}