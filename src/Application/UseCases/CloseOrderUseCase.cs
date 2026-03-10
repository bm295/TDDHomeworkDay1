using Application.Models;
using Application.Ports;

namespace Application.UseCases;

public sealed class CloseOrderUseCase
{
    private readonly IOrderRepository _orders;

    public CloseOrderUseCase(IOrderRepository orders)
    {
        _orders = orders;
    }

    public async Task<OrderDto> ExecuteAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _orders.GetByIdAsync(orderId, cancellationToken) ?? throw new InvalidOperationException("Order not found.");

        order.Close();
        await _orders.UpdateAsync(order, cancellationToken);

        return new OrderDto(order.Id, order.TableNumber, order.Status, order.TotalAmount, order.Items.Count);
    }
}
