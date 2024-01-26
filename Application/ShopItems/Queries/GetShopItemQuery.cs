using Domain.ShopItemAggregate;

namespace Application.ShopItems.Queries;

public record GetShopItemQuery(string Id) : IRequest<ErrorOr<ShopItem>>;