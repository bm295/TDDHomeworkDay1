using Application.Models;
using Application.UseCases;
using Domain.Entities;
using Domain.Services;
using Xunit;

namespace Application.Tests;

public sealed class OperationsDashboardMapperTests
{
    [Fact]
    public void ToDto_keeps_table_ordering_and_embeds_active_orders()
    {
        var day = new DateOnly(2026, 8, 23);
        var report = new SalesReportDto(day, 3, 42m);
        var tables = new[] { new Table(2, 4), new Table(1, 2) };

        var order = new Order(Guid.Parse("22222222-2222-2222-2222-222222222222"), 2, new DateTimeOffset(2026, 8, 23, 7, 0, 0, TimeSpan.Zero));
        order.AddItem("tea", "Tea", 1, 2m);
        order.SendToKitchen();

        var dashboard = OperationsDashboardMapper.ToDto(day, report, tables, [order], [new InventoryItem("tea", "Tea", 5)], new FixedPricingPolicy());

        Assert.Equal(1, dashboard.Tables.First().Number);
        Assert.True(dashboard.Tables.Single(x => x.Number == 2).HasActiveOrder);
        Assert.Equal(9m, dashboard.MenuItems.Single().SuggestedPrice);
    }

    private sealed class FixedPricingPolicy : IMenuPricingPolicy
    {
        public decimal GetSuggestedPrice(string sku) => 9m;
    }
}
