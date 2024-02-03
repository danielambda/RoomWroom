using Application.Common.Interfaces.Perception;

namespace Application.ShopItems.Commands;

public class DeleteShopItemHandler(
    IShopItemRepository repository)
    : IRequestHandler<DeleteShopItemCommand, ErrorOr<Success>>
{
    private readonly IShopItemRepository _repository = repository;
    
    public async Task<ErrorOr<Success>> Handle(DeleteShopItemCommand request, CancellationToken cancellationToken)
    {
        bool deleted = await _repository.DeleteAsync(request.ShopItemId, cancellationToken);

        return deleted ? new Success() : Error.NotFound(); //TODO ShopItemId тоже
    }
}