namespace Domain.Entities;

public sealed class InventoryItem
{
    public InventoryItem(string sku, string name, int quantityOnHand)
    {
        if (string.IsNullOrWhiteSpace(sku)) throw new ArgumentException("SKU is required.", nameof(sku));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        if (quantityOnHand < 0) throw new ArgumentOutOfRangeException(nameof(quantityOnHand));

        Sku = sku.Trim();
        Name = name.Trim();
        QuantityOnHand = quantityOnHand;
    }

    public string Sku { get; }
    public string Name { get; }
    public int QuantityOnHand { get; private set; }

    public void Deduct(int quantity)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        if (quantity > QuantityOnHand) throw new InvalidOperationException($"Insufficient inventory for {Sku}.");

        QuantityOnHand -= quantity;
    }
}
