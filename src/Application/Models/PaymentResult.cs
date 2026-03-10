namespace Application.Models;

public sealed record PaymentResult(bool Success, string TransactionId, string Message);
