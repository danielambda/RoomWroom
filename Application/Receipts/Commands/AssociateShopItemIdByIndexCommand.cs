using Domain.ReceiptAggregate.ValueObjects;
using Domain.ShopItemAggregate.ValueObjects;

namespace Application.Receipts.Commands;

public record AssociateShopItemIdByIndexCommand(
    ShopItemId AssociatedShopItemId, int Index, bool SaveAssociation, ReceiptId ReceiptId)
    : IRequest<ErrorOr<Success>>;