using RoomWroom.CommandHandling;
using Telegram.Bot.Types.ReplyMarkups;

namespace RoomWroom.Telegram
{
    public static class ResponseExtensions
    {
        public static IEnumerable<MessageData> GetMessagesData(this Response response) 
            => response.ResponseUnits.Select(MessageData.FromResponseUnit);

        public static InlineKeyboardMarkup ToInlineKeyboardMarkup(
            this IEnumerable<ResponseCallbackButton> responseCallbackButtons)
        {
            return new(["TODO", "TODO DO"]);
        }
    }
}
