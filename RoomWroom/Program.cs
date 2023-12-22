using RoomWroom.CommandHandling;
using RoomWroom.Telegram;
using RoomWroom.Grocery;

string? inn = Environment.GetEnvironmentVariable("INN", EnvironmentVariableTarget.User);
if (inn is null)
{
    Console.WriteLine("Environment variable INN is null");
    return;
}

string? password = Environment.GetEnvironmentVariable("INN_PASSWORD", EnvironmentVariableTarget.User);
if (password is null)
{
    Console.WriteLine("Environment variable INN_PASSWORD is null");
    return;
}

string? accessToken = Environment.GetEnvironmentVariable("ROOMWROOM_TG_BOT_TOKEN", EnvironmentVariableTarget.User);
if (accessToken is null)
{
    Console.WriteLine("Environment variable ROOMWROOM_TG_BOT_TOKEN is null");
    return;
}

IReceiptQrScanner receiptQrScanner = new InnReceiptQrScanner(inn, password);
ICommandProvider groceryCommandProvider = new GroceryCommandProvider(receiptQrScanner);

CommandHandler baseCommandHandler = new([groceryCommandProvider]);
ResponseProviderFactoryMethodProvider responseProviderFactoryMethodProvider = new(baseCommandHandler);

TelegramBot bot = new(accessToken, responseProviderFactoryMethodProvider.ResponseProviderFactoryMethod);
bot.Run();

Console.ReadLine();
