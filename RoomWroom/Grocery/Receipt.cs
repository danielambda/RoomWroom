using System.Text;

namespace RoomWroom.Grocery;

public class Receipt(IEnumerable<GroceryItem>? items)
{
    public override string ToString()
    {
        if (items == null)
            return "Receipt is empty";

        StringBuilder result = new("");
        float sum = 0;

        foreach (GroceryItem item in items)
        {
            result.Append(item + Environment.NewLine);
            sum += item.Sum;
        }

        return result + Environment.NewLine + "Overall: " + sum / 100;
    }
}