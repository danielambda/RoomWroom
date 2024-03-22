using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;
using Domain.ShopItemAggregate;

namespace Application.ShopItems.Queries;

public class GetShopItemHandler(
    IShopItemRepository repository
) : IRequestHandler<GetShopItemQuery, ErrorOr<ShopItem>>
{
    private readonly IShopItemRepository _repository = repository;
    
    public async Task<ErrorOr<ShopItem>> Handle(GetShopItemQuery query, CancellationToken cancellationToken)
    {
        var shopItemId = query.ShopItemId;
        
        ShopItem? shopItem = await _repository.GetAsync(shopItemId, cancellationToken);
        if (shopItem is null)
            return Errors.ShopItem.NotFound;

        return shopItem;
    }
}
