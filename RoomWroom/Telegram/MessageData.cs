using RoomWroom.CommandHandling;
using Telegram.Bot.Types.ReplyMarkups;

namespace RoomWroom.Telegram;

public record struct MessageData(string Text, IReplyMarkup? ReplyMarkup)
{
    public static MessageData FromResponseUnit(ResponseUnit responseUnit)
    {
        string text = responseUnit.Text;
        IReplyMarkup? replyMarkup = responseUnit.ResponseCallbackButtons?.ToInlineKeyboardMarkup();

        return new MessageData(text, replyMarkup);
    }
}