using IronBarCode;
using RoomWroom.CommandHandling;

namespace RoomWroom.Grocery;

public class GroceryCommandProvider(IReceiptQrScanner receiptQrScanner) : ICommandProvider
{
    private readonly IReceiptQrScanner _receiptQrScanner = receiptQrScanner;

    public IEnumerable<Command> GetCommands() => [new Command<Image>("/scan", Scan, "Send me a photo of the QR code")];

    private async Task<string> Scan(Image image)
    {
        BarcodeResults results = await BarcodeReader.ReadAsync(image);

        if (!results.Any())
            throw new ArgumentException(nameof(image));

        string qrText = results.First().Value;
        string text = (await _receiptQrScanner.GetReceiptFromQrAsync(qrText)).ToString();

        return text;
    }
}