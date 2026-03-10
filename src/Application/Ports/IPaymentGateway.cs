using Application.Models;

namespace Application.Ports;

public interface IPaymentGateway
{
    Task<PaymentResult> ChargeAsync(decimal amount, string method, CancellationToken cancellationToken);
}
