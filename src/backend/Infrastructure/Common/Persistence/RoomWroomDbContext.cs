using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;
using Domain.RoomAggregate;
using Domain.UserAggregate;

namespace Infrastructure.Common.Persistence;

public class RoomWroomDbContext : DbContext
{
    public DbSet<Room> Rooms { get; init; } = default!;
    public DbSet<Receipt> Receipts { get; init; } = default!;
    public DbSet<User> Users { get; init; } = default!;
    public DbSet<ShopItemAssociation> ShopItemAssociations { get; init; } = default!;

    public RoomWroomDbContext(DbContextOptions<RoomWroomDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RoomWroomDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}