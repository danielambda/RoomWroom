using Application.Common.Interfaces;
using Application.Common.Interfaces.Perception;
using Application.Receipts.Interfaces;
using Domain.Common.Errors;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;

namespace Application.Receipts.Commands;

//This feature does not work due to ФНС
public class CreateReceiptFromFiscalHandler(
    IReceiptItemsFromQrCreator receiptFromQrCreatorProvider,
    IReceiptRepository receiptRepository,
    IShopItemAssociationsRepository shopItemAssociationsRepository,
    IDateTimeProvider dateTimeProvider
) : IRequestHandler<CreateReceiptFromFiscalCommand, ErrorOr<Receipt>>
{
    private readonly IReceiptItemsFromQrCreator _receiptFromQrCreatorProvider = receiptFromQrCreatorProvider;
    private readonly IReceiptRepository _receiptRepository = receiptRepository;
    private readonly IShopItemAssociationsRepository _shopItemAssociationsRepository = shopItemAssociationsRepository;

    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    
    public async Task<ErrorOr<Receipt>> Handle(
        CreateReceiptFromFiscalCommand command, CancellationToken cancellationToken)
    {
        var (fiscalDriveNumber, fiscalDocumentNumber, fiscalSign, creatorId) = command;

        DateTime now = _dateTimeProvider.Now;

        string qr = ConstructQr(now, fiscalDriveNumber, fiscalDocumentNumber, fiscalSign);
        
        bool receiptAlreadyExists = await _receiptRepository.CheckExistenceByQr(qr, cancellationToken);
        if (receiptAlreadyExists)
            return Errors.Receipt.QrDuplicate(qr);
        
        IEnumerable<ReceiptItem>? receiptItems = await _receiptFromQrCreatorProvider.CreateAsync(qr, cancellationToken);
        if (receiptItems is null)
            return Errors.Receipt.FromQrCreationFailure(qr);

        var receipt = Receipt.CreateNew(receiptItems, qr, creatorId);
        
        await receipt.AssociateShopItemIdsAsync(_shopItemAssociationsRepository.GetAsync, cancellationToken);
        await _receiptRepository.AddAsync(receipt, cancellationToken);
        
        return receipt;
    }

    private static string ConstructQr(DateTime now,
        string fiscalDriveNumber, string fiscalDocumentNumber, string fiscalSign) =>
        $"t={now.Year:0000}{now.Month:00}{now.Day:00}T{now.Hour:00}{now.Minute:00}{now.Second:00}&" +
        $"s=975.88&fn={fiscalDriveNumber}&" +
        $"i={fiscalDocumentNumber}&" +
        $"fp={fiscalSign}&n=1";
}