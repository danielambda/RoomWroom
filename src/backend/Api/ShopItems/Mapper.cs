using Application.ShopItems.Commands;
using Contracts.ShopItems;
using Domain.Common.Enums;
using Domain.ShopItemAggregate;

namespace Api.ShopItems;

public static class Mapper
{
    public static ShopItemResponse ToResponse(this ShopItem shopItem) => 
        new(shopItem.Id!, shopItem.Name, shopItem.Quantity, shopItem.Units.ToString());

    public static CreateShopItemCommand ToCommand(this CreateShopItemRequest request) =>
        new(request.Name, request.Quantity, Enum.Parse<Units>(request.Units, ignoreCase: true));
}
