using RoomWroom.CommandHandling;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

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

    public static ResponseUnit ToResponseUnit(this Message message) 
        => new(message.Text, message.ReplyMarkup?.InlineKeyboard.ToResponseCallbackButtons());

    public static IEnumerable<IEnumerable<ResponseCallbackButton>> ToResponseCallbackButtons(
        this IEnumerable<IEnumerable<InlineKeyboardButton>> buttons)
        => buttons.Select(row 
            => row.Select(button 
                => new ResponseCallbackButton(button.Text, button.CallbackData ?? 
                                                           throw new ArgumentNullException(nameof(buttons)))));
}