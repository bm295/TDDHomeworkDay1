using Application.Models;
using Application.Ports;
using Domain.Entities;

namespace Application.UseCases;

public sealed class ProcessPaymentUseCase
{
    private readonly IOrderRepository _orders;
    private readonly IInventoryRepository _inventory;
    private readonly IPaymentGateway _paymentGateway;

    public ProcessPaymentUseCase(IOrderRepository orders, IInventoryRepository inventory, IPaymentGateway paymentGateway)
    {
        _orders = orders;
        _inventory = inventory;
        _paymentGateway = paymentGateway;
    }

    public async Task<OrderDto> ExecuteAsync(Guid orderId, string method, CancellationToken cancellationToken)
    {
        var order = await _orders.GetByIdAsync(orderId, cancellationToken) ?? throw new InvalidOperationException("Order not found.");

        var paymentResult = await _paymentGateway.ChargeAsync(order.TotalAmount, method, cancellationToken);
        if (!paymentResult.Success)
        {
            throw new InvalidOperationException(paymentResult.Message);
        }

        foreach (var item in order.Items)
        {
            var inventoryItem = await _inventory.GetBySkuAsync(item.Sku, cancellationToken)
                ?? throw new InvalidOperationException($"Inventory item {item.Sku} is missing.");

            inventoryItem.Deduct(item.Quantity);
            await _inventory.UpdateAsync(inventoryItem, cancellationToken);
        }

        var payment = new Payment(order.TotalAmount, method, DateTimeOffset.UtcNow, paymentResult.TransactionId);
        order.MarkAsPaid(payment);
        await _orders.UpdateAsync(order, cancellationToken);

        return new OrderDto(order.Id, order.TableNumber, order.Status, order.TotalAmount, order.Items.Count);
    }
}
