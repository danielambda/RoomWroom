using Application.Receipts.Queries;
using Domain.ShopItemAggregate;

namespace Application.ShopItems.Queries;

public class GetShopItemHandler : IRequestHandler<GetShopItemQuery, ErrorOr<ShopItem>>
{
    public Task<ErrorOr<ShopItem>> Handle(GetShopItemQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}