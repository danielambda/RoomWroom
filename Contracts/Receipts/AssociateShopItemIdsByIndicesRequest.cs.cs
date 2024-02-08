namespace Contracts.Receipts;

public record AssociateShopItemIdsByIndicesRequest(
    IEnumerable<string?> AssociatedShopItemIds,
    bool SaveAssociations
);
