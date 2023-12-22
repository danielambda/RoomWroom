using System.Collections.Concurrent;
using RoomWroom.CommandHandling;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace RoomWroom.Telegram;

public class TelegramBot(string? token, Func<ResponseProvider> responseProviderFactoryMethod)
{
    private readonly TelegramBotClient _botClient = new(token ?? throw new ArgumentNullException(nameof(token)));
    private readonly Func<ResponseProvider> _responseProviderFactoryMethod = responseProviderFactoryMethod;
    
    private readonly ConcurrentDictionary<long, ResponseProvider> _responseProvidersByChatId = [];
    
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
        if (update.Message is not { } message)
            return;
        
        if (message.GetTextOrCaption() is not { } messageText)
            return;
        
        long chatId = message.Chat.Id;

        if (_responseProvidersByChatId.ContainsKey(chatId) == false)
        {
            //
        }

        ResponseProvider? responseProvider = _responseProvidersByChatId.GetOrAdd(
            chatId, l => _responseProviderFactoryMethod.Invoke());

        string? response = responseProvider(messageText);
        
        if (response is not null)
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, response, cancellationToken: cancellationToken);
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

    public TelegramBot(string? token) : this(token, null)
    {
    }
}