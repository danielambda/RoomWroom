using Domain.ShopItemAggregate.ValueObjects;
using ErrorOr;

namespace Domain.Common.Errors;

public partial class Errors
{
    public static class ShopItem
    {
        public static Error NotFound(ShopItemId id) =>
            Error.NotFound($"ShopItem.{nameof(NotFound)}", $"ShopItem with id '{id}' was not found");
    }
}