using Domain.RoomAggregate;
using Domain.RoomAggregate.ValueObjects;
using Domain.ShopItemAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Rooms.Configurations;

public class RoomConfigurations : IEntityTypeConfiguration<Room>
{
    private const string ROOMS_TABLE = "Rooms";
    private const string OWNED_SHOP_ITEMS_TABLE = "RoomsOwnedShopItems";
    private const string ROOM_USER_TABLE = "RoomUserIds";
    
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        ConfigureRoomsTable(builder);
        
        ConfigureOwnedShopItemsTable(builder);
        
        ConfigureUserIds(builder);
    }

    private static void ConfigureRoomsTable(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable(ROOMS_TABLE);

        builder.HasKey(room => room.Id);
        builder.Property(room => room.Id)
            .ValueGeneratedNever()
            .HasConversion
            (
                id => id.Value,
                value => RoomId.Create(value)
            );

        builder.Property(room => room.Name)
            .HasMaxLength(100);

        builder.OwnsOne(room => room.Budget, budgetBuilder =>
        {
            budgetBuilder.Property(money => money.Currency).HasColumnName("BudgetCurrency");
            budgetBuilder.Property(money => money.Amount).HasColumnName("BudgetAmount");
        });
    }
    
    private static void ConfigureOwnedShopItemsTable(EntityTypeBuilder<Room> builder)
    {
        builder.OwnsMany(room => room.OwnedShopItems, itemBuilder =>
        {
            itemBuilder.ToTable(OWNED_SHOP_ITEMS_TABLE);

            itemBuilder.WithOwner().HasForeignKey("RoomId");

            itemBuilder.Property<int>("Id")
                .ValueGeneratedOnAdd();
            itemBuilder.HasKey("Id");

            itemBuilder.OwnsOne(item => item.Price);

            itemBuilder.Property(item => item.ShopItemId)
                .HasColumnName("ShopItemId")
                .HasConversion
                (
                    id => id.Value,
                    value => ShopItemId.Create(value)
                );
        });
        
        builder.Metadata.FindNavigation(nameof(Room.OwnedShopItems))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
    
    private static void ConfigureUserIds(EntityTypeBuilder<Room> builder)
    {
        builder.OwnsMany(room => room.UserIds, userIdBuilder =>
        {
            userIdBuilder.ToTable(ROOM_USER_TABLE);

            userIdBuilder.WithOwner().HasForeignKey("RoomId");

            userIdBuilder.HasKey("Id", "RoomId");
            
            userIdBuilder.Property<int>("Id")
                .ValueGeneratedOnAdd();

            userIdBuilder.Property(id => id.Value)
                .HasColumnName("UserId");
        });
        
        builder.Metadata.FindNavigation(nameof(Room.UserIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}