using Application.Common.Interfaces.Perception;
using Domain.Common.Enums;
using Domain.ShopItemAggregate;

namespace Application.ShopItems.Commands;

public class CreateShopItemHandler(
    IShopItemRepository repository
) : IRequestHandler<CreateShopItemCommand, ErrorOr<ShopItem>>
{
    private readonly IShopItemRepository _repository = repository;

    public async Task<ErrorOr<ShopItem>> Handle(CreateShopItemCommand command, CancellationToken cancellationToken)
    {
        var (name, quantity, units) = command;
        
        var shopItem = ShopItem.CreateNew(name, quantity, units);

        await _repository.AddAsync(shopItem, cancellationToken);

        return shopItem;
    }
}