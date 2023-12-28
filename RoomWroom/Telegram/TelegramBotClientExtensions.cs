using System.Runtime.CompilerServices;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace RoomWroom.Telegram;

public static class TelegramBotClientExtensions
{
    public static async IAsyncEnumerable<Message> SendTextMessagesAsync(this ITelegramBotClient botClient,
        IEnumerable<MessageData> messagesData, long chatId, int? messageThreadId = null, ParseMode? parseMode = null,
        IEnumerable<MessageEntity>? entities = null, bool? disableWebPagePreview = null,
        bool? disableNotification = null, bool? protectContent = null, int? replyToMessageId = null, 
        bool? allowSendingWithoutReply = null, bool? allAreReplying = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        MessageEntity[]? entitiesArray = entities?.ToArray();

        foreach ((string? text, IReplyMarkup? replyMarkup) in messagesData)
        {
            if (text is null)
                continue;
            
            yield return await botClient.SendTextMessageAsync(
                chatId, text, messageThreadId, parseMode, entitiesArray, disableWebPagePreview, disableNotification,
                protectContent, replyToMessageId, allowSendingWithoutReply, replyMarkup, cancellationToken);

            if (allAreReplying == true)
                continue;
            
            replyToMessageId = null;
            allowSendingWithoutReply = null;
        }
    }
}