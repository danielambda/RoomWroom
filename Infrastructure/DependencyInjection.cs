using Application.Common.Interfaces;
using Application.Receipts.Interfaces;
using Application.Rooms.Interfaces;
using Infrastructure.Receipts;
using Infrastructure.Receipts.Perception;
using Infrastructure.Rooms.Perception;
using Infrastructure.ShopItems.Perception;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddScoped<IReceiptFromQrCreator, InnReceiptFromQrCreator>()
            .AddScoped<IReceiptRepository, FileReceiptRepository>()
            .AddScoped<IShopItemRepository, FileShopItemRepository>()
            .AddScoped<IRoomRepository, FileRoomRepository>();

        return services;
    }
}