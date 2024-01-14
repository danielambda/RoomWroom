using Application.Shopping.Queries;
using Contracts.Shopping;
using Domain.ReceiptAggregate.ValueObjects;

namespace Api.Common.Mapping;

public class ShoppingMappingConfigs : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ReceiptItem, ReceiptItemResponse>()
            .Map(dest => dest.PriseCurrency, src => src.Price.Currency.ToString());
        config.NewConfig<ReceiptResult, ReceiptResponse>()
            .Map(dest => dest.Id, src => src.Receipt.Id.Value.ToString())
            .Map(dest => dest, src => src.Receipt);
    }
}