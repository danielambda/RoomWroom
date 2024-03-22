using Domain.IngredientsAggregate.ValueObjects;
using Domain.ShopItemAggregate;
using Domain.ShopItemAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ShopItems.Configurations;

public class ShopItemConfigurations : IEntityTypeConfiguration<ShopItem>
{
    private const string SHOP_ITEMS_TABLE = "ShopItems";

    public void Configure(EntityTypeBuilder<ShopItem> builder)
        => ConfigureShopItemsTable(builder);

    private static void ConfigureShopItemsTable(EntityTypeBuilder<ShopItem> builder)
    {
        builder.ToTable(SHOP_ITEMS_TABLE);

        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id)
            .ValueGeneratedNever()
            .HasConversion
            (
                id => id.Value,
                value => ShopItemId.Create(value)
            );

        builder.Property(item => item.Name)
            .HasMaxLength(100);
        
        builder.Property(item => item.IngredientId)
            .HasConversion<Guid?>
            (
                id => id != null ? id.Value : null,
                value => value.HasValue ? IngredientId.Create(value.Value) : null
            );
    }
}