using Domain.ShopItemAggregate;
using Domain.ShopItemAggregate.ValueObjects;

namespace Application.ShopItems.Queries;

public record GetShopItemQuery(ShopItemId ShopItemId) : IRequest<ErrorOr<ShopItem>>;