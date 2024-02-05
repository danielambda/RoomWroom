namespace Contracts.Receipts;

public record CreateReceiptFromQrRequest(string Qr, string CreatorUserId);