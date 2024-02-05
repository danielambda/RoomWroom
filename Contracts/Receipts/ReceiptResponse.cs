namespace Contracts.Receipts;

public record ReceiptResponse(
    string Id,
    List<ReceiptItemResponse> Items,
    string? Qr,
    string CreatorId);

public record ReceiptItemResponse(
    string Name,
    decimal PriceAmount,
    string PriseCurrency,
    decimal Quantity,
    string? AssociatedShopItemId);