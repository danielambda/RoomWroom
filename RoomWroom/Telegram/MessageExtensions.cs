using Telegram.Bot;
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

    public static async Task<Image?> GetImageAsync(this Message message,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        if (message.Photo?.Last() is not { } photo)
            return null;

        const string DESTINATION_FILE_PATH = "image.jpg";

        await using Stream fileStream = System.IO.File.Create(DESTINATION_FILE_PATH);

        await botClient.GetInfoAndDownloadFileAsync(photo.FileId, fileStream, cancellationToken);
        fileStream.Position = 0;

        return await Image.FromStreamAsync(fileStream);
    }
}