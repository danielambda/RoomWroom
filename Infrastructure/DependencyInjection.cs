using Application.Common.Interfaces;
using Application.Common.Interfaces.Perception;
using Application.Receipts.Interfaces;
using Infrastructure.Common;
using Infrastructure.Common.Persistence;
using Infrastructure.Receipts;
using Infrastructure.Receipts.Persistence;
using Infrastructure.Rooms.Perception;
using Infrastructure.ShopItems.Perception;
using Infrastructure.Users.Perception;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddPersistence();

        services
            .AddScoped<IReceiptItemsFromQrCreator, InnReceiptItemsFromQrCreator>()
            .AddTransient<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<RoomWroomDbContext>(options =>
            options.UseSqlite("DataSource=app.db"));
        
        services
            .AddScoped<IReceiptRepository, FileReceiptRepository>()
            .AddScoped<IShopItemRepository, FileShopItemRepository>()
            .AddScoped<IRoomRepository, FileRoomRepository>()
            .AddScoped<IShopItemAssociationsRepository, FileShopItemAssociationRepository>()
            .AddScoped<IUserRepository, FileUserRepository>();

        return services;
    }
}