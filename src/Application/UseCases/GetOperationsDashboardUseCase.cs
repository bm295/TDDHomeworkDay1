using Application.Models;
using Application.Ports;
using Domain.Services;
using Domain.Entities;

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
        return OperationsDashboardMapper.ToDto(day, salesReport, tables, activeOrders, inventoryItems, _pricingPolicy);
    }
}
