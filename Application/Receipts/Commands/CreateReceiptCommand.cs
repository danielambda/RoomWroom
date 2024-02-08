using Domain.Common.ValueObjects;
using Domain.ReceiptAggregate;
using Domain.ShopItemAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Application.Receipts.Commands;

public record CreateReceiptCommand(
    List<ReceiptItemCommand> Items,
    string? Qr,
    UserId CreatorId
) : IRequest<ErrorOr<Receipt>>;

public record ReceiptItemCommand(
    string Name,
    Money Prise,
    decimal Quantity,
    ShopItemId AssociatedShopItemId
);
    