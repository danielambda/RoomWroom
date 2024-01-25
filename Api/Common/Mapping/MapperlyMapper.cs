using Application.Receipts.Commands;
using Application.Receipts.Queries;
using Contracts.Receipts;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;
using Riok.Mapperly.Abstractions;

namespace Api.Common.Mapping;

[Mapper]
public partial class MapperlyMapper : IMapper
{
    public partial ReceiptResponse MapToResponse(Receipt receipt);

    public ReceiptItemResponse MapToResponse(ReceiptItem receiptItem) =>
        new(receiptItem.Name,
            receiptItem.Price.Amount,
            receiptItem.Price.Currency.ToString(),
            receiptItem.Quantity,
            receiptItem.AssociatedShopItemId);

    public partial CreateReceiptFromQrCommand MapToCommand(CreateReceiptFromQrRequest request);

    public partial GetReceiptQuery MapToQuery(GetReceiptRequest request);
}