using Application.Models;
using Application.Ports;

namespace Application.UseCases;

public sealed class SendOrderToKitchenUseCase
{
    private readonly IOrderRepository _orders;
    private readonly IKitchenNotifier _kitchenNotifier;

    public SendOrderToKitchenUseCase(IOrderRepository orders, IKitchenNotifier kitchenNotifier)
    {
        _orders = orders;
        _kitchenNotifier = kitchenNotifier;
    }

    public async Task<OrderDto> ExecuteAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _orders.GetByIdAsync(orderId, cancellationToken) ?? throw new InvalidOperationException("Order not found.");

        order.SendToKitchen();
        await _orders.UpdateAsync(order, cancellationToken);
        await _kitchenNotifier.NotifyOrderSentAsync(order.Id, order.TableNumber, cancellationToken);

        return new OrderDto(order.Id, order.TableNumber, order.Status, order.TotalAmount, order.Items.Count);
    }
}
