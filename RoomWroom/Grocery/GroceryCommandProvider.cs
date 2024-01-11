using IronBarCode;
using RoomWroom.CommandHandling;

namespace RoomWroom.Grocery;

internal class GroceryCommandProvider(IReceiptFromQrProvider receiptQrScanner, IReceiptsRepository receiptsRepository)
    : ICommandProvider
{
    private readonly IReceiptFromQrProvider _receiptFromQrProvider = receiptQrScanner;
    private readonly IReceiptsRepository _receiptsRepository = receiptsRepository;

    public IEnumerable<Command> GetCommands() => [new Command<Image>("/scan", Scan, "Send me a photo of the QR code")];

    private async Task<Response> Scan(Image image)
    {
        BarcodeResults results = await BarcodeReader.ReadAsync(image);

        if (!results.Any())
            return "Scanning failed, try another photo of QR code";

        string qrText = results.First().Value;
        if (await GetReceiptAsync(qrText) is not { } receipt)
            return "FNS (ФНС) did not reply properly, try again";

        IEnumerable<ResponseCallbackButton> responseCallbackButtons =
        [
            new("Mark all Shared", GroceryCallbackActionsProvider.MARK_ALL_SHARED_CALLBACK),
            new("Select Shared", GroceryCallbackActionsProvider.SELECT_SHARED_CALLBACK),
            new("Edit names", GroceryCallbackActionsProvider.EDIT_NAMES_CALLBACK)
        ];

        Response response = new(new ResponseUnit(receipt.ToString(), responseCallbackButtons));

        return response;
    }

    private async Task<Receipt?> GetReceiptAsync(string qrText)
    {
        Receipt? repositoryReceipt = await _receiptFromQrProvider.GetAsync(qrText);
        
        if (repositoryReceipt is not null)
        {
            repositoryReceipt.TranslateNames(_receiptsRepository.GetTranslatedName);
            return repositoryReceipt;
        }

        Receipt? receipt = _receiptsRepository.Get(qrText);

        if (receipt is null)
            return null;
        
        receipt.TranslateNames(_receiptsRepository.GetTranslatedName);
        _receiptsRepository.Add(qrText, receipt);

        return receipt;
    }
}