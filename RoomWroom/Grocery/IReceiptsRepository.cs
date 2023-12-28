namespace RoomWroom.Grocery;

public interface IReceiptsRepository
{
    public Receipt? Get(string qrText);

    public void Add(string qrText, Receipt receipt);

    public string? GetTranslatedName(string name);

    public void AddTranslatedName(string name, string translatedName);
}