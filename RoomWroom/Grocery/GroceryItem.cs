using System.Text.Json.Serialization;

namespace RoomWroom.Grocery;

public readonly struct GroceryItem(string originalName, int price, float quantity, int sum, string translatedName)
{
    public string Name { get; } = originalName;

    /// <summary>
    /// Price in 0.01 of rubble
    /// </summary>
    public int Price { get; } = price;

    public float Quantity { get; } = quantity;
    public int Sum { get; } = sum;
    public string TranslatedName { get; } = translatedName;

    [JsonConstructor]
    public GroceryItem(string name, int price, float quantity, int sum) : this(name, price, quantity, sum, name + "(!).") { }

    public override string ToString() => $"{TranslatedName}.{Environment.NewLine} {Price / 100f} x {Quantity} = {Sum / 100f}";
}