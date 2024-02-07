using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;

namespace Application.ShopItems.Commands;

public class DeleteShopItemHandler(
    IShopItemRepository repository)
    : IRequestHandler<DeleteShopItemCommand, ErrorOr<Success>>
{
    private readonly IShopItemRepository _repository = repository;
    
    public async Task<ErrorOr<Success>> Handle(DeleteShopItemCommand request, CancellationToken cancellationToken)
    {
        var shopItemId = request.ShopItemId;
        
        bool deleted = await _repository.DeleteAsync(shopItemId, cancellationToken);

        return deleted ? new Success() : Errors.ShopItem.NotFound(shopItemId);
    }
}