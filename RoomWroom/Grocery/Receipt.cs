namespace RoomWroom.Grocery;

public class Receipt(IEnumerable<GroceryItem>? items)
{
    public override string ToString()
    {
        if (items == null)
            return "Receipt is empty";

        string result = "";
        float sum = 0;

        foreach (GroceryItem item in items)
        {
            result += item + Environment.NewLine;
            sum += item.Sum;
        }

        return result + Environment.NewLine + "Overall: " + sum / 100;
    }
}