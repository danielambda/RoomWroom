using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;
using Domain.ShopItemAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Receipts.Configurations;

public class ReceiptConfigurations : IEntityTypeConfiguration<Receipt>
{
    private const string RECEIPTS_TABLE = "Receipts";
    private const string RECEIPT_ITEMS_TABLE = "ReceiptItems";
    
    public void Configure(EntityTypeBuilder<Receipt> builder)
    {
        ConfigureReceiptsTable(builder);
        
        ConfigureReceiptItemsTable(builder);
    }

    private static void ConfigureReceiptsTable(EntityTypeBuilder<Receipt> builder)
    {
        builder.ToTable(RECEIPTS_TABLE);

        builder.HasKey(receipt => receipt.Id);
        
        builder.Property(receipt => receipt.Id)
            .ValueGeneratedNever()
            .HasConversion
            (
                id => id.Value,
                value => ReceiptId.Create(value)
            );

        builder.Property(receipt => receipt.Qr)
            .HasMaxLength(100);

        builder.Property(receipt => receipt.CreatorId)
            .HasConversion
            (
                id => id.Value,
                value => UserId.Create(value)
            );
    }
    
    private static void ConfigureReceiptItemsTable(EntityTypeBuilder<Receipt> builder)
    {
        builder.OwnsMany(receipt => receipt.Items, itemBuilder =>
        {
            itemBuilder.ToTable(RECEIPT_ITEMS_TABLE);

            itemBuilder.WithOwner().HasForeignKey("ReceiptId");

            itemBuilder.HasKey("Id", "ReceiptId");
            itemBuilder.Property<int>("Id")
                .ValueGeneratedOnAdd();
            
            itemBuilder.Property(item => item.Name)
                .HasMaxLength(100);
            
            itemBuilder.OwnsOne(item => item.Price);
            
            itemBuilder.OwnsOne(item => item.Sum);

            itemBuilder.Property(item => item.AssociatedShopItemId)
                .HasConversion<Guid?>
                (
                    id => id != null ? id.Value : null,
                    value => value.HasValue ? ShopItemId.Create(value.Value) : null
                );
        });
        
        builder.Metadata.FindNavigation(nameof(Receipt.Items))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}