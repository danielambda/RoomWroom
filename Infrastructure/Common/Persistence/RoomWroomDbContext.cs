using Domain.RoomAggregate;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Common.Persistence;

public class RoomWroomDbContext : DbContext
{
    public DbSet<Room> Rooms { get; init; } = default!;

    public RoomWroomDbContext(DbContextOptions<RoomWroomDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RoomWroomDbContext).Assembly);
        
        modelBuilder.Model.GetEntityTypes()
            .SelectMany(entity => entity.GetProperties())
            .Where(property => property.IsPrimaryKey())
            .ToList()
            .ForEach(property =>
                property.ValueGenerated = ValueGenerated.Never
            );
        
        base.OnModelCreating(modelBuilder); 
    }
}