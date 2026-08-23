using Application.Models;
using Application.Ports;
using Application.UseCases;
using Domain.Entities;
using Domain.Services;
using Xunit;

namespace Application.Tests;

public sealed class GetOperationsDashboardUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_maps_active_orders_tables_and_menu_items()
    {
        var order = new Order(Guid.Parse("11111111-1111-1111-1111-111111111111"), 2, new DateTimeOffset(2026, 8, 23, 8, 0, 0, TimeSpan.Zero));
        order.AddItem("coffee", "Coffee", 2, 3.5m);
        order.SendToKitchen();

        var useCase = new GetOperationsDashboardUseCase(
            new FakeOrderRepository([order]),
            new FakeTableRepository([new Table(1, 2), new Table(2, 4)]),
            new FakeInventoryRepository([new InventoryItem("coffee", "Coffee", 12)]),
            new FakeReportingReadModel(new SalesReportDto(new DateOnly(2026, 8, 23), 0, 0m)),
            new FakePricingPolicy());

        var result = await useCase.ExecuteAsync(new DateOnly(2026, 8, 23), CancellationToken.None);

        Assert.Equal(new DateOnly(2026, 8, 23), result.Day);
        Assert.Single(result.ActiveOrders);
        Assert.Equal(2, result.ActiveOrders.Single().TableNumber);
        Assert.True(result.Tables.Single(x => x.Number == 2).HasActiveOrder);
        Assert.Equal(1, result.MenuItems.Single().SuggestedPrice);
    }

    private sealed class FakeOrderRepository : IOrderRepository
    {
        private readonly IReadOnlyCollection<Order> _orders;

        public FakeOrderRepository(IReadOnlyCollection<Order> orders) => _orders = orders;

        public Task AddAsync(Order order, CancellationToken cancellationToken) => Task.CompletedTask;
        public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => Task.FromResult(_orders.SingleOrDefault(x => x.Id == id));
        public Task<IReadOnlyCollection<Order>> GetActiveAsync(CancellationToken cancellationToken) => Task.FromResult(_orders);
        public Task UpdateAsync(Order order, CancellationToken cancellationToken) => Task.CompletedTask;
        public Task<IReadOnlyCollection<Order>> GetClosedByDateAsync(DateOnly day, CancellationToken cancellationToken) => Task.FromResult<IReadOnlyCollection<Order>>([]);
    }

    private sealed class FakeTableRepository : ITableRepository
    {
        private readonly IReadOnlyCollection<Table> _tables;
        public FakeTableRepository(IReadOnlyCollection<Table> tables) => _tables = tables;
        public Task<IReadOnlyCollection<Table>> GetAllAsync(CancellationToken cancellationToken) => Task.FromResult(_tables);
        public Task<Table?> GetByNumberAsync(int tableNumber, CancellationToken cancellationToken) => Task.FromResult(_tables.SingleOrDefault(x => x.Number == tableNumber));
    }

    private sealed class FakeInventoryRepository : IInventoryRepository
    {
        private readonly IReadOnlyCollection<InventoryItem> _items;
        public FakeInventoryRepository(IReadOnlyCollection<InventoryItem> items) => _items = items;
        public Task<InventoryItem?> GetBySkuAsync(string sku, CancellationToken cancellationToken) => Task.FromResult(_items.SingleOrDefault(x => x.Sku == sku));
        public Task<IReadOnlyCollection<InventoryItem>> GetAllAsync(CancellationToken cancellationToken) => Task.FromResult(_items);
        public Task UpdateAsync(InventoryItem item, CancellationToken cancellationToken) => Task.CompletedTask;
    }

    private sealed class FakeReportingReadModel : IReportingReadModel
    {
        private readonly SalesReportDto _report;
        public FakeReportingReadModel(SalesReportDto report) => _report = report;
        public Task<SalesReportDto> BuildDailySalesReportAsync(DateOnly day, CancellationToken cancellationToken) => Task.FromResult(_report);
    }

    private sealed class FakePricingPolicy : IMenuPricingPolicy
    {
        public decimal GetSuggestedPrice(string sku) => 1m;
    }
}
