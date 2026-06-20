using Microsoft.EntityFrameworkCore;

namespace Adapters.Outbound.Persistence.EntityFramework;

/// <summary>
/// Entity Framework Core unit-of-work for durable restaurant persistence.
/// </summary>
public sealed class RestaurantDbContext : DbContext
{
    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
        : base(options)
    {
    }

    public DbSet<OrderRecord> Orders => Set<OrderRecord>();

    public DbSet<OrderItemRecord> OrderItems => Set<OrderItemRecord>();

    public DbSet<TableRecord> Tables => Set<TableRecord>();

    public DbSet<InventoryItemRecord> InventoryItems => Set<InventoryItemRecord>();

    public DbSet<PaymentRecord> Payments => Set<PaymentRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TableRecord>()
            .HasKey(table => table.Number);

        modelBuilder.Entity<InventoryItemRecord>()
            .HasKey(inventoryItem => inventoryItem.Sku);

        modelBuilder.Entity<OrderRecord>()
            .Property(order => order.RowVersion)
            .IsRowVersion();
    }
}
