using Application.Common.Interfaces.Perception;
using Domain.ShopItemAggregate;

namespace Application.ShopItems.Queries;

public class GetAllShopItemsHandler(
    IShopItemRepository repository
) : IRequestHandler<GetAllShopItemsQuery, IQueryable<ShopItem>>
{
    private readonly IShopItemRepository _repository = repository;
    
    public async Task<IQueryable<ShopItem>> Handle(GetAllShopItemsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}