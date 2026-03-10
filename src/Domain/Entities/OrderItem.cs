namespace Domain.Entities;

public sealed class OrderItem
{
    public OrderItem(string sku, string name, int quantity, decimal unitPrice)
    {
        if (string.IsNullOrWhiteSpace(sku)) throw new ArgumentException("SKU is required.", nameof(sku));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        if (unitPrice < 0) throw new ArgumentOutOfRangeException(nameof(unitPrice));

        Sku = sku.Trim();
        Name = name.Trim();
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public string Sku { get; }
    public string Name { get; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; }
    public decimal LineTotal => Quantity * UnitPrice;

    public void Increase(int quantity)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        Quantity += quantity;
    }

    public void Decrease(int quantity)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        if (quantity > Quantity) throw new InvalidOperationException("Cannot remove more than existing quantity.");
        Quantity -= quantity;
    }
}
