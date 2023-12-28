using Telegram.Bot.Types.ReplyMarkups;
using RoomWroom.CommandHandling;

namespace RoomWroom.Telegram;

public record struct MessageData(string? Text, InlineKeyboardMarkup? InlineKeyboardMarkup = null)
{
    public static MessageData FromResponseUnit(ResponseUnit responseUnit)
    {
        string? text = responseUnit.Text;
        InlineKeyboardMarkup? replyMarkup = responseUnit.ResponseCallbackButtons?.ToInlineKeyboardMarkup();

        return new MessageData(text, replyMarkup);
    }

    public static implicit operator MessageData(string text) => new(text);
}