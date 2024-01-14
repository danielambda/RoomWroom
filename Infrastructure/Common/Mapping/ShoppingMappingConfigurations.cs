using Domain.Common.Enums;
using Domain.ReceiptAggregate.ValueObjects;
using Infrastructure.Shopping.Dtos;

namespace Infrastructure.Common.Mapping;

public class ShoppingMappingConfigs : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ReceiptItemDto, ReceiptItem>()
            .MapWith(dto => new(dto.Name, new(dto.Price / 100m, Currency.RUB), dto.Quantity, null));
    }
}