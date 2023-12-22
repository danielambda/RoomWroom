namespace RoomWroom.Grocery;

public interface IReceiptQrScanner
{
    public Task<Receipt> GetReceiptFromQrAsync(string qr);
}