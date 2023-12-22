using System.Collections.Concurrent;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

using RoomWroom.CommandHandling;

namespace RoomWroom.Telegram;

public class TelegramBot(string token, Func<ResponseProvider> responseProviderFactoryMethod)
{
    private readonly TelegramBotClient _botClient = new(token);
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

        string? messageText = message.GetTextOrCaption();
        Image? image = await message.GetImageAsync(botClient, cancellationToken);
        
        long chatId = message.Chat.Id;

        ResponseProvider responseProvider = _responseProvidersByChatId.GetOrAdd(
            chatId, _ => _responseProviderFactoryMethod.Invoke());

        Task<string>? responseTask = responseProvider(messageText, image);
        if (responseTask == null)
            return;
        
        string response = await responseTask;
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
}