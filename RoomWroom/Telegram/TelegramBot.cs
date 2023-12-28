using System.Collections.Concurrent;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using RoomWroom.CommandHandling;

namespace RoomWroom.Telegram;

public class TelegramBot(
    string token,
    Func<long, IResponseProvider> responseProviderFactoryMethod,
    Func<long, ICallbackResponseProvider> callbackResponseProviderFactoryMethod,
    bool needsToReply = true)
{
    private const ParseMode PARSE_MODE = ParseMode.Html;
    
    private readonly bool _needsToReply = needsToReply;

    private readonly TelegramBotClient _botClient = new(token);
   
    private readonly Func<long, IResponseProvider> _responseProviderFactoryMethod = responseProviderFactoryMethod;
    private readonly Func<long, ICallbackResponseProvider> _callbackResponseProviderFactoryMethod =
        callbackResponseProviderFactoryMethod;
    
    private readonly ConcurrentDictionary<long, IResponseProvider> _responseProvidersByChatId = [];
    private readonly ConcurrentDictionary<long, ICallbackResponseProvider> _callbackResponseProvidersByChatId = [];

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
        Console.WriteLine("Canceled");
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

        await foreach (Message _ in botClient.SendTextMessagesAsync(
                           messagesData: messagesData,
                           chatId: chatId,
                           parseMode: PARSE_MODE,
                           messageThreadId: message.MessageThreadId,
                           replyToMessageId: _needsToReply ? message.MessageId : null,
                           cancellationToken: cancellationToken));
    }

    private async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        CancellationToken cancellationToken)
    {
        if (callbackQuery.Data is not { } callback)
            return;
        
        if (callbackQuery.Message is not { } message)
            return;

        long chatId = message.Chat.Id;

        ICallbackResponseProvider callbackResponseProvider =
            _callbackResponseProvidersByChatId.GetOrAdd(chatId, _callbackResponseProviderFactoryMethod);
        
        (Task<Response>, CallbackActionType)? result = 
            callbackResponseProvider.GetCallbackResponseTask(callback, message.ToResponseUnit());
        
        if (result is null)
            return;

        (Task<Response> responseTask, CallbackActionType callbackActionType) = result.Value;
        
        Response response = await responseTask;
        IEnumerable<MessageData> messagesData = response.GetMessagesData();

        switch (callbackActionType)
        {
            case CallbackActionType.NewMessage:
                await foreach (Message _ in botClient.SendTextMessagesAsync(
                                   messagesData: messagesData,
                                   chatId: chatId,
                                   parseMode: PARSE_MODE,
                                   messageThreadId: message.MessageThreadId,
                                   replyToMessageId: _needsToReply ? message.MessageId : null,
                                   cancellationToken: cancellationToken));
                break;
            case CallbackActionType.ClearButtonsAndNewMessage:
                messagesData = messagesData.Select(messageData => messageData with { InlineKeyboardMarkup = null });
                await foreach (Message _ in botClient.SendTextMessagesAsync(
                                   messagesData: messagesData,
                                   chatId: chatId,
                                   parseMode: PARSE_MODE,
                                   messageThreadId: message.MessageThreadId,
                                   replyToMessageId: _needsToReply ? message.MessageId : null,
                                   cancellationToken: cancellationToken));
                break;
            case CallbackActionType.EditCurrentMessage:
                MessageData messageData = messagesData.First(); 
                
                if (messageData.Text is null)
                    return;
                
                await botClient.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: message.MessageId,
                    text: messageData.Text,
                    parseMode: PARSE_MODE,
                    replyMarkup: messageData.InlineKeyboardMarkup,
                    cancellationToken: cancellationToken);
                break;
            default:
                throw new ArgumentOutOfRangeException();
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