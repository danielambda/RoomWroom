using Application.Common.Interfaces;
using Application.Common.Interfaces.Perception;
using Application.Receipts.Interfaces;
using Infrastructure.Common;
using Infrastructure.Receipts;
using Infrastructure.Receipts.Perception;
using Infrastructure.Rooms.Perception;
using Infrastructure.ShopItems.Perception;
using Infrastructure.Users.Perception;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddScoped<IReceiptItemsFromQrCreator, InnReceiptItemsFromQrCreator>()
            .AddScoped<IReceiptRepository, FileReceiptRepository>()
            .AddScoped<IShopItemRepository, FileShopItemRepository>()
            .AddScoped<IRoomRepository, FileRoomRepository>()
            .AddScoped<IShopItemAssociationsRepository, FileShopItemAssociationRepository>()
            .AddScoped<IUserRepository, FileUserRepository>();

        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}