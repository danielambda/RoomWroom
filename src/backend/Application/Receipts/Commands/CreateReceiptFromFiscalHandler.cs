using Application.Common.Interfaces.Perception;
using Application.Receipts.Interfaces;
using Domain.Common.Errors;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;

namespace Application.Receipts.Commands;

public class CreateReceiptFromFiscalHandler(
    IReceiptItemsFromQrCreator receiptFromQrCreatorProvider,
    IReceiptRepository receiptRepository,
    IShopItemAssociationsRepository shopItemAssociationsRepository
) : IRequestHandler<CreateReceiptFromFiscalCommand, ErrorOr<Receipt>>
{
    private readonly IReceiptItemsFromQrCreator _receiptFromQrCreatorProvider = receiptFromQrCreatorProvider;
    private readonly IReceiptRepository _receiptRepository = receiptRepository;
    private readonly IShopItemAssociationsRepository _shopItemAssociationsRepository = shopItemAssociationsRepository;
    
    public async Task<ErrorOr<Receipt>> Handle(
        CreateReceiptFromFiscalCommand command, CancellationToken cancellationToken)
    {
        var (dateTime, sum, fiscalDriveNumber, fiscalDocumentNumber, fiscalSign, creatorId) = command;
        
        string qr = ConstructQr(dateTime, sum, fiscalDriveNumber, fiscalDocumentNumber, fiscalSign);
        
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

    private static string ConstructQr(DateTime dateTime, decimal sum, 
        string fiscalDriveNumber, string fiscalDocumentNumber, string fiscalSign) =>
        $"t={dateTime.Year:0000}{dateTime.Month:00}{dateTime.Day:00}T{dateTime.Hour:00}{dateTime.Minute:00}&" +
        $"s={sum:.00}&" +
        $"fn={fiscalDriveNumber}&" +
        $"i={fiscalDocumentNumber}&" +
        $"fp={fiscalSign}&n=1";
}