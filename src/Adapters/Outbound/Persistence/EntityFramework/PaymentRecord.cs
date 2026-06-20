namespace Adapters.Outbound.Persistence.EntityFramework;

public sealed class PaymentRecord
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public decimal Amount { get; set; }

    public string Method { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime PaidAtUtc { get; set; }

    public string TransactionId { get; set; } = string.Empty;
}
