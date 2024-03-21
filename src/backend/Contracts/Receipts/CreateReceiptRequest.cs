namespace Contracts.Receipts;

public record CreateReceiptRequest(
    IList<ReceiptItemRequest> Items,
    string? Qr
);

public record ReceiptItemRequest(
    string Name,
    decimal PriceAmount,
    string PriceCurrency,
    decimal Quantity,
    string AssociatedShopItemId
);