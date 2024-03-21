using Domain.ReceiptAggregate.ValueObjects;
using Domain.ShopItemAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Receipts.Configurations;

public class ShopItemAssociationConfiguration : IEntityTypeConfiguration<ShopItemAssociation>
{
    private const string SHOP_ITEMS_ASSOCIATION_TABLE = "ShopItemAssociation";
    
    public void Configure(EntityTypeBuilder<ShopItemAssociation> builder)
    {
        builder.ToTable(SHOP_ITEMS_ASSOCIATION_TABLE);

        builder.HasKey(association => association.OriginalName);

        builder.Property(association => association.ShopItemId)
            .ValueGeneratedNever()
            .HasConversion
            (
                id => id.Value,
                value => ShopItemId.Create(value)
            );
    }
}