namespace Application.Ports;

public interface IKitchenNotifier
{
    Task NotifyOrderSentAsync(Guid orderId, int tableNumber, CancellationToken cancellationToken);
}
