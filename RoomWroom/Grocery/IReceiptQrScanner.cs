namespace RoomWroom.Grocery;

internal interface IReceiptQrScanner
{
    Task<Receipt?> GetReceiptFromQrAsync(string qr);
}