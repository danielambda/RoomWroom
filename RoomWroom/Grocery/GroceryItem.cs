namespace RoomWroom.Grocery;

public sealed record GroceryItem(string Name, int Price, float Quantity, int Sum)
{
    public string? TranslatedName { get; private set; }

    public void TranslateName(string? translatedName) => TranslatedName = translatedName;
    
    public override string ToString() =>
        $"{TranslatedName ?? Name + "(!)"}.{Environment.NewLine} {Price / 100f} x {Quantity} = {Sum / 100f}";
}
