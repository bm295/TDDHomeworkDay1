using Application.Models;
using Domain.Entities;
using Domain.Services;

namespace Application.UseCases;

internal static class OperationsDashboardMapper
{
    public static OperationsDashboardDto ToDto(
        DateOnly day,
        SalesReportDto salesReport,
        IReadOnlyCollection<Table> tables,
        IReadOnlyCollection<Order> activeOrders,
        IReadOnlyCollection<InventoryItem> inventoryItems,
        IMenuPricingPolicy pricingPolicy)
    {
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
            .Select(item => new MenuItemDto(item.Sku, item.Name, item.QuantityOnHand, pricingPolicy.GetSuggestedPrice(item.Sku)))
            .ToList()
            .AsReadOnly();

        return new OperationsDashboardDto(day, salesReport, tableDtos, activeOrderDtos, menuItems);
    }
}
