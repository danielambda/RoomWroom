namespace Contracts.Shopping;

public record ReceiptResponse(string Id, string Qr, List<ReceiptItemResponse> Items);

public record ReceiptItemResponse(
    string Name,
    float PriceAmount,
    string PriseCurrency,
    float Quantity,
    string? AssociatedShopItemId);