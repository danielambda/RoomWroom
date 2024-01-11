using System.Text;

namespace RoomWroom.Grocery;

public class Receipt(IEnumerable<GroceryItem> items)
{
    public GroceryItem[] Items { get; } = items.OrderBy(item => item.TranslatedName ?? item.Name)
                                               .CombineSame()
                                               .Where(item => item is { Quantity: > 0, Sum: > 0 })
                                               .ToArray();

    public void TranslateNames(Func<string, string?> translationMethod)
    {
        if (Items is null)
            throw new NullReferenceException(nameof(Items));

        foreach (GroceryItem item in Items) 
            item.TranslateName(translationMethod(item.Name));
    }
    
    public override string ToString()
    {
        if (Items is null)
            throw new NullReferenceException(nameof(Items));

        StringBuilder result = new();
        int sum = 0;
        int currentIndex = 1;
        
        foreach (GroceryItem item in Items)
        {
            result.Append($"{currentIndex}. {item}\n");
            sum += item.Sum;
            currentIndex++;
        }

        return $"{result}\nOverall:{sum / 100f}";
    }

    public IEnumerable<string> ToStrings()
    {
        if (Items is null)
            throw new NullReferenceException(nameof(Items));

        foreach (GroceryItem item in Items)
            yield return item.ToString();
    }
}

file static class GroceryItemExtensions
{
    public static IEnumerable<GroceryItem> CombineSame(this IOrderedEnumerable<GroceryItem> items)
    {
        GroceryItem? previousItem = null;

        foreach (GroceryItem currentItem in items)
        {
            if (currentItem.Price == previousItem?.Price && currentItem.TranslatedName == previousItem.TranslatedName)
            {
                float quantitySum = previousItem.Quantity + currentItem.Quantity;
                int sumSum = previousItem.Sum + currentItem.Sum;
                previousItem = currentItem with { Quantity = quantitySum, Sum = sumSum };
                
                continue;
            }
            
            if (previousItem is not null)
                yield return previousItem;

            previousItem = currentItem;
        }

        if (previousItem is not null)
            yield return previousItem;
    }
}