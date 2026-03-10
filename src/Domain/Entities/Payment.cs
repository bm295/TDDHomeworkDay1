namespace Domain.Entities;

public sealed class Payment
{
    public Payment(decimal amount, string method, DateTimeOffset paidAtUtc, string transactionId)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (string.IsNullOrWhiteSpace(method)) throw new ArgumentException("Method is required.", nameof(method));
        if (string.IsNullOrWhiteSpace(transactionId)) throw new ArgumentException("Transaction id is required.", nameof(transactionId));

        Amount = amount;
        Method = method.Trim();
        PaidAtUtc = paidAtUtc;
        TransactionId = transactionId.Trim();
    }

    public decimal Amount { get; }
    public string Method { get; }
    public DateTimeOffset PaidAtUtc { get; }
    public string TransactionId { get; }
}
