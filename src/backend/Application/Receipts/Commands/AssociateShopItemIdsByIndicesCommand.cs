using Domain.ReceiptAggregate.ValueObjects;
using Domain.ShopItemAggregate.ValueObjects;

namespace Application.Receipts.Commands;

public record AssociateShopItemIdsByIndicesCommand(
    IEnumerable<ShopItemId?> AssociatedShopItemIds, bool SaveAssociations, ReceiptId ReceiptId
) : IRequest<ErrorOr<Success>>;