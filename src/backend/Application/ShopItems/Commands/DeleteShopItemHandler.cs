using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;

namespace Application.ShopItems.Commands;

public class DeleteShopItemHandler(
    IShopItemRepository repository
) : IRequestHandler<DeleteShopItemCommand, ErrorOr<Deleted>>
{
    private readonly IShopItemRepository _repository = repository;
    
    public async Task<ErrorOr<Deleted>> Handle(DeleteShopItemCommand command, CancellationToken cancellationToken)
    {
        var shopItemId = command.ShopItemId;
        
        bool deleted = await _repository.DeleteAsync(shopItemId, cancellationToken);

        return deleted ? Result.Deleted : Errors.ShopItem.NotFound;
    }
}