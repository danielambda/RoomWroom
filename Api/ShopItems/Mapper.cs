using Application.ShopItems.Commands;
using Application.ShopItems.Queries;
using Contracts.ShopItems;
using Domain.ShopItemAggregate;

namespace Api.ShopItems;

public static class Mapper
{
    public static GetShopItemQuery ToQuery(this (GetShopItemRequest request, string id) source) => new(source.id);

    public static ShopItemResponse ToResponse(this ShopItem shopItem) => new(shopItem.Id!, shopItem.Name);

    public static CreateShopItemCommand ToCommand(this CreateShopItemRequest request) => new(request.Id, request.Name);
}