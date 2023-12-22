using Telegram.Bot.Types;

namespace RoomWroom.Telegram;

public static class MessageExtensions
{
    public static string? GetTextOrCaption(this Message message)
    {
        if (message.Text is { } text)
            return text;

        if (message.Caption is { } caption)
            return caption;

        return null;
    }
}