using Application.Common.Interfaces.Perception;
using Application.Receipts.Interfaces;
using Domain.Common.Errors;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;

namespace Application.Receipts.Commands;

public class CreateReceiptFromQrHandler(
    IReceiptItemsFromQrCreator receiptFromQrCreatorProvider,
    IReceiptRepository receiptRepository,
    IShopItemAssociationsRepository shopItemAssociationsRepository
) : IRequestHandler<CreateReceiptFromQrCommand, ErrorOr<Receipt>>
{
    private readonly IReceiptItemsFromQrCreator _receiptFromQrCreatorProvider = receiptFromQrCreatorProvider;
    private readonly IReceiptRepository _receiptRepository = receiptRepository;
    private readonly IShopItemAssociationsRepository _shopItemAssociationsRepository = shopItemAssociationsRepository;
    
    public async Task<ErrorOr<Receipt>> Handle(
        CreateReceiptFromQrCommand command, CancellationToken cancellationToken)
    {
        var (qr, creatorId) = command;
        
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
}