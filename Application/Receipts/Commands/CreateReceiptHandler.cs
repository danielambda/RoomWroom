using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;

namespace Application.Receipts.Commands;

public class CreateReceiptHandler(
    IReceiptRepository repository) 
    : IRequestHandler<CreateReceiptCommand, ErrorOr<Receipt>>
{
    private readonly IReceiptRepository _repository = repository;
    
    public async Task<ErrorOr<Receipt>> Handle(CreateReceiptCommand command, CancellationToken cancellationToken)
    {
        var receipt = Receipt.CreateNew(command.Items.ConvertAll(item =>
                new ReceiptItem(item.Name,
                    item.Prise,
                    item.Quantity,
                    item.AssociatedShopItemId)),
                command.Qr,
                command.CreatorId);

        if (await _repository.CheckExistenceByQr(receipt.Qr, cancellationToken))
            return Errors.Receipt.QrDuplicate(receipt.Qr!);

        await _repository.AddAsync(receipt, cancellationToken);

        return receipt;
    }
}