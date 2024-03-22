using ErrorOr;

namespace Domain.Common.Errors;

public partial class Errors
{
    public static class ShopItem
    {
        public static Error NotFound =>
            Error.NotFound($"ShopItem.{nameof(NotFound)}", $"ShopItem was not found");
    }
}