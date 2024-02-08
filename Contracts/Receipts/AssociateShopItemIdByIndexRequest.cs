namespace Contracts.Receipts;

public record AssociateShopItemIdByIndexRequest(
    string AssociatedShopItemId,
    int Index,
    bool SaveAssociation
);
