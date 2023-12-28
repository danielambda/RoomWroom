using Telegram.Bot.Types.ReplyMarkups;
using RoomWroom.CommandHandling;

namespace RoomWroom.Telegram
{
    public static class ResponseExtensions
    {
        public static IEnumerable<MessageData> GetMessagesData(this Response response) 
            => response.ResponseUnits.Select(MessageData.FromResponseUnit);

        public static InlineKeyboardMarkup ToInlineKeyboardMarkup(
            this IEnumerable<IEnumerable<ResponseCallbackButton>> responseCallbackButtons) =>
            new(responseCallbackButtons.Select(row => row.Select(ToInlineKeyboardButton)));

        public static InlineKeyboardButton ToInlineKeyboardButton(this ResponseCallbackButton responseCallbackButton) 
            => InlineKeyboardButton.WithCallbackData(responseCallbackButton.Text, responseCallbackButton.CallBackData);
    }
}
