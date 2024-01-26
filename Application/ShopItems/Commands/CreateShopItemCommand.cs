using Domain.ShopItemAggregate;

namespace Application.ShopItems.Commands;

public record CreateShopItemCommand(string Name) : IRequest<ErrorOr<ShopItem>>;