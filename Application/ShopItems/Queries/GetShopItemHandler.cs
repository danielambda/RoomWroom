using Application.Common.Interfaces;
using Domain.ShopItemAggregate;

namespace Application.ShopItems.Queries;

public class GetShopItemHandler(IShopItemRepository repository) : IRequestHandler<GetShopItemQuery, ErrorOr<ShopItem>>
{
    private readonly IShopItemRepository _repository = repository;
    
    public async Task<ErrorOr<ShopItem>> Handle(GetShopItemQuery request, CancellationToken cancellationToken)
    {
        ShopItem? shopItem = await _repository.GetAsync(request.Id!, cancellationToken);
        if (shopItem is null)
            return Error.NotFound();

        return shopItem;
    }
}