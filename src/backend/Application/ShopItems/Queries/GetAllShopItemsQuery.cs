using Domain.ShopItemAggregate;

namespace Application.ShopItems.Queries;

public record GetAllShopItemsQuery : IRequest<IQueryable<ShopItem>>;