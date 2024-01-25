namespace Contracts.Receipts;

public record ReceiptResponse(
    string Id,
    string Qr,
    List<ReceiptItemResponse> Items);

public record ReceiptItemResponse(
    string Name,
    decimal PriceAmount,
    string PriseCurrency,
    decimal Quantity,
    string? AssociatedShopItemId);