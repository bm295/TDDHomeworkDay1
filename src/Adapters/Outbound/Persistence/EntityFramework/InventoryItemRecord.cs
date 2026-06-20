namespace Adapters.Outbound.Persistence.EntityFramework;

public sealed class InventoryItemRecord
{
    public string Sku { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int QuantityOnHand { get; set; }

    public int LowStockThreshold { get; set; }

    public bool IsAvailable { get; set; }
}
