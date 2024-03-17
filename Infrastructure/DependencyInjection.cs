using Application.Common.Interfaces;
using Application.Common.Interfaces.Perception;
using Application.Receipts.Interfaces;
using Infrastructure.Common;
using Infrastructure.Common.Persistence;
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
        services.AddPersistence();
        
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<RoomWroomDbContext>(options =>
            options.UseSqlite("DataSource=app.db"));
        
        services
            .AddScoped<IReceiptItemsFromQrCreator, InnReceiptItemsFromQrCreator>()
            .AddScoped<IReceiptRepository, FileReceiptRepository>()
            .AddScoped<IShopItemRepository, FileShopItemRepository>()
            .AddScoped<IRoomRepository, RoomRepository>()
            .AddScoped<IShopItemAssociationsRepository, FileShopItemAssociationRepository>()
            .AddScoped<IUserRepository, FileUserRepository>();

        return services;
    }
}