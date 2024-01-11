namespace RoomWroom.Grocery;

internal interface IReceiptFromQrProvider
{
    Task<Receipt?> GetAsync(string qrText);
}