using Application.ShopItems.Commands;
using Contracts.ShopItems;
using Domain.ShopItemAggregate;

namespace Api.ShopItems;

public static class Mapper
{
    public static ShopItemResponse ToResponse(this ShopItem shopItem) => new(shopItem.Id!, shopItem.Name);

    public static CreateShopItemCommand ToCommand(this CreateShopItemRequest request) => new(request.Name);
}