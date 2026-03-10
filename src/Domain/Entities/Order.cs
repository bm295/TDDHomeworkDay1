namespace Domain.Entities;

public enum OrderStatus
{
    Draft = 1,
    SentToKitchen = 2,
    Paid = 3,
    Closed = 4
}

public sealed class Order
{
    private readonly List<OrderItem> _items = [];

    public Order(Guid id, int tableNumber, DateTimeOffset createdAtUtc)
    {
        if (tableNumber <= 0) throw new ArgumentOutOfRangeException(nameof(tableNumber));

        Id = id;
        TableNumber = tableNumber;
        CreatedAtUtc = createdAtUtc;
        Status = OrderStatus.Draft;
    }

    public Guid Id { get; }
    public int TableNumber { get; }
    public DateTimeOffset CreatedAtUtc { get; }
    public OrderStatus Status { get; private set; }
    public Payment? Payment { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public decimal TotalAmount => _items.Sum(x => x.LineTotal);

    public void AddItem(string sku, string name, int quantity, decimal unitPrice)
    {
        EnsureMutable();

        var existing = _items.SingleOrDefault(x => x.Sku.Equals(sku, StringComparison.OrdinalIgnoreCase));
        if (existing is null)
        {
            _items.Add(new OrderItem(sku, name, quantity, unitPrice));
            return;
        }

        existing.Increase(quantity);
    }

    public void RemoveItem(string sku, int quantity)
    {
        EnsureMutable();

        var existing = _items.SingleOrDefault(x => x.Sku.Equals(sku, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException("Item not found in order.");

        existing.Decrease(quantity);
        if (existing.Quantity == 0)
        {
            _items.Remove(existing);
        }
    }

    public void SendToKitchen()
    {
        EnsureStatus(OrderStatus.Draft);
        if (_items.Count == 0) throw new InvalidOperationException("Cannot send empty order to kitchen.");

        Status = OrderStatus.SentToKitchen;
    }

    public void MarkAsPaid(Payment payment)
    {
        EnsureStatus(OrderStatus.SentToKitchen);
        Payment = payment ?? throw new ArgumentNullException(nameof(payment));
        Status = OrderStatus.Paid;
    }

    public void Close()
    {
        EnsureStatus(OrderStatus.Paid);
        Status = OrderStatus.Closed;
    }

    private void EnsureMutable()
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException("Order can only be modified while in draft status.");
    }

    private void EnsureStatus(OrderStatus required)
    {
        if (Status != required)
            throw new InvalidOperationException($"Invalid order state transition. Required: {required}, current: {Status}.");
    }
}
