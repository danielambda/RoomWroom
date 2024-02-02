using Domain.ReceiptAggregate.ValueObjects;
using Domain.ShopItemAggregate.ValueObjects;

namespace Application.Receipts.Commands;

public record AssociateShopItemIdsByIndicesCommand(IEnumerable<ShopItemId?> AssociatedShopItemIds, ReceiptId ReceiptId)
    : IRequest<ErrorOr<Success>>;