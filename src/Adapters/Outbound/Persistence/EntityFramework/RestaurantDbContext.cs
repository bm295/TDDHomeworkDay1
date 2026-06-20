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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
