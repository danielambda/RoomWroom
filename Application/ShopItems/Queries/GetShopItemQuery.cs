using Domain.ShopItemAggregate;

namespace Application.ShopItems.Queries;

public record GetShopItemQuery(string ShopItemId) : IRequest<ErrorOr<ShopItem>>;