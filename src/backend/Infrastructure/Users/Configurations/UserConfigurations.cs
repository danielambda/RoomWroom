using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Users.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    private const string USERS_TABLE = "Users";

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(USERS_TABLE);

        builder.HasKey(user => user.Id);
        builder.Property(user => user.Id)
            .ValueGeneratedNever()
            .HasConversion
            (
                id => id.Value,
                value => UserId.Create(value)
            );

        builder.Property(user => user.Name)
            .HasMaxLength(50);

        builder.Property(user => user.RoomId)
            .HasConversion<Guid?>
            (
                id => id != null ? id.Value : null,
                value => value.HasValue ? RoomId.Create(value.Value) : null
            );

        builder.Metadata.FindNavigation(nameof(User.ScannedReceiptsIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.OwnsMany(user => user.ScannedReceiptsIds, idBuilder =>
            idBuilder.Property(id => id.Value));
    }
}