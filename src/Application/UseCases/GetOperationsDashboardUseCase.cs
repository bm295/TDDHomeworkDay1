using Application.Models;
using Application.Ports;
using Domain.Services;

namespace Application.UseCases;

public sealed class GetOperationsDashboardUseCase
{
    private readonly IOrderRepository _orders;
    private readonly ITableRepository _tables;
    private readonly IInventoryRepository _inventory;
    private readonly IReportingReadModel _reporting;
    private readonly IMenuPricingPolicy _pricingPolicy;

    public GetOperationsDashboardUseCase(
        IOrderRepository orders,
        ITableRepository tables,
        IInventoryRepository inventory,
        IReportingReadModel reporting,
        IMenuPricingPolicy pricingPolicy)
    {
        _orders = orders;
        _tables = tables;
        _inventory = inventory;
        _reporting = reporting;
        _pricingPolicy = pricingPolicy;
    }

    public async Task<OperationsDashboardDto> ExecuteAsync(DateOnly day, CancellationToken cancellationToken)
    {
        var tables = await _tables.GetAllAsync(cancellationToken);
        var activeOrders = await _orders.GetActiveAsync(cancellationToken);
        var inventoryItems = await _inventory.GetAllAsync(cancellationToken);
        var salesReport = await _reporting.BuildDailySalesReportAsync(day, cancellationToken);

        var activeOrderDtos = activeOrders
            .OrderBy(x => x.TableNumber)
            .ThenBy(x => x.CreatedAtUtc)
            .Select(x => new ActiveOrderDto(
                x.Id,
                x.TableNumber,
                x.CreatedAtUtc,
                x.Status,
                x.TotalAmount,
                x.Items
                    .Select(item => new OrderLineDto(item.Sku, item.Name, item.Quantity, item.UnitPrice, item.LineTotal))
                    .ToList()
                    .AsReadOnly(),
                x.Payment is null
                    ? null
                    : new PaymentSummaryDto(x.Payment.Amount, x.Payment.Method, x.Payment.PaidAtUtc, x.Payment.TransactionId)))
            .ToList()
            .AsReadOnly();

        var activeOrdersByTable = activeOrderDtos.ToLookup(x => x.TableNumber);

        var tableDtos = tables
            .OrderBy(x => x.Number)
            .Select(table =>
            {
                var activeOrder = activeOrdersByTable[table.Number].FirstOrDefault();
                return new DiningTableDto(
                    table.Number,
                    table.Seats,
                    activeOrder is not null,
                    activeOrder?.Id,
                    activeOrder?.Status,
                    activeOrder?.TotalAmount);
            })
            .ToList()
            .AsReadOnly();

        var menuItems = inventoryItems
            .OrderBy(x => x.Name)
            .Select(item => new MenuItemDto(item.Sku, item.Name, item.QuantityOnHand, _pricingPolicy.GetSuggestedPrice(item.Sku)))
            .ToList()
            .AsReadOnly();

        return new OperationsDashboardDto(day, salesReport, tableDtos, activeOrderDtos, menuItems);
    }
}
