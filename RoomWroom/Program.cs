using RoomWroom.CommandHandling;
using RoomWroom.Database.Grocery;
using RoomWroom.Telegram;
using RoomWroom.Grocery;

string inn = TryGetEnvironmentVariable("INN");
string password = TryGetEnvironmentVariable("INN_PASSWORD");
string accessToken = TryGetEnvironmentVariable("ROOMWROOM_TG_BOT_TOKEN");

IReceiptFromQrProvider receiptQrScanner = new InnReceiptFromQrProvider(inn, password);

IReceiptsRepository receiptsRepository = new FileReceiptsRepository();
ICommandProvider groceryCommandProvider = new GroceryCommandProvider(receiptQrScanner, receiptsRepository);
ICallbackActionsProvider groceryCallbackActionsProvider = new GroceryCallbackActionsProvider();

CommandHandler baseCommandHandler = new([groceryCommandProvider]);
CallbackHandler baseCallbackHandler = new([groceryCallbackActionsProvider]);

ResponseProviderFactoryMethodProvider responseProviderFactoryMethodProvider = new(baseCommandHandler);
CallbackResponseProviderFactoryMethodProvider callbackResponseProviderFactoryMethodProvider = new(baseCallbackHandler);

TelegramBot bot = new(accessToken,
    responseProviderFactoryMethodProvider.ResponseProviderFactoryMethod,
    callbackResponseProviderFactoryMethodProvider.CallbackResponseProviderFactoryMethod);

await bot.Run();

return;

static string TryGetEnvironmentVariable(string key)
{
    string? value = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User);
    return value ?? throw new ArgumentException($"Environment variable {key} is null");
}
