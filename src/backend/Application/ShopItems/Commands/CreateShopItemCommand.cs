using Domain.Common.Enums;
using Domain.ShopItemAggregate;

namespace Application.ShopItems.Commands;

public record CreateShopItemCommand(string Name, decimal Quantity, Units Units) : IRequest<ErrorOr<ShopItem>>;