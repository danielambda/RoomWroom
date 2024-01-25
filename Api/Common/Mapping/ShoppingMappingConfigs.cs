/*using Contracts.Receipts;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;

namespace Api.Common.Mapping;

public class ShoppingMappingConfigs : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ReceiptItem, ReceiptItemResponse>()
            .Map(dest => dest.PriseCurrency, src => src.Price.Currency.ToString());
        config.NewConfig<Receipt, ReceiptResponse>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest, src => src);
    }
}*/