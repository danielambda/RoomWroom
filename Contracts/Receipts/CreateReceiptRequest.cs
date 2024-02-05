namespace Contracts.Receipts;

public record CreateReceiptRequest(
    List<ReceiptItemRequest> Items,
    string? Qr,
    string CreatorUserId);

public record ReceiptItemRequest(
    string Name,
    decimal MoneyAmount,
    string MoneyCurrency,
    decimal Quantity,
    string AssociatedShopItemId);