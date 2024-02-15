using Domain.ShopItemAggregate.ValueObjects;

namespace Application.ShopItems.Commands;

public record DeleteShopItemCommand(ShopItemId ShopItemId) : IRequest<ErrorOr<Deleted>>;