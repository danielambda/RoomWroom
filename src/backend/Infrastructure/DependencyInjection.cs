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
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        string? dbConnectionString = null)
    {
        services.AddPersistence(dbConnectionString);

        services
            .AddScoped<IReceiptItemsFromQrCreator, InnReceiptItemsFromQrCreator>()
            .AddTransient<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    private static void AddPersistence(this IServiceCollection services, string? dbConnectionString)
    {
        services.AddDbContext<RoomWroomDbContext>(options =>
            options.UseNpgsql(dbConnectionString));
        
        services
            .AddScoped<IReceiptRepository, ReceiptRepository>()
            .AddScoped<IShopItemRepository, ShopItemRepository>()
            .AddScoped<IRoomRepository, RoomRepository>()
            .AddScoped<IShopItemAssociationsRepository, ShopItemAssociationRepository>()
            .AddScoped<IUserRepository, UserRepository>();
    }
}