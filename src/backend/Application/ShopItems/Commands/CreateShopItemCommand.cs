using Domain.Common.Enums;
using Domain.IngredientsAggregate.ValueObjects;
using Domain.ShopItemAggregate;

namespace Application.ShopItems.Commands;

public record CreateShopItemCommand
(
    string Name,
    decimal Quantity,
    Units Units,
    IngredientId? IngredientId
) : IRequest<ErrorOr<ShopItem>>;