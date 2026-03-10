using Application.Models;
using Application.Ports;

namespace Application.UseCases;

public sealed class AddOrderItemUseCase
{
    private readonly IOrderRepository _orders;

    public AddOrderItemUseCase(IOrderRepository orders)
    {
        _orders = orders;
    }

    public async Task<OrderDto> ExecuteAsync(Guid orderId, string sku, string name, int quantity, decimal unitPrice, CancellationToken cancellationToken)
    {
        var order = await _orders.GetByIdAsync(orderId, cancellationToken) ?? throw new InvalidOperationException("Order not found.");

        order.AddItem(sku, name, quantity, unitPrice);
        await _orders.UpdateAsync(order, cancellationToken);

        return new OrderDto(order.Id, order.TableNumber, order.Status, order.TotalAmount, order.Items.Count);
    }
}
