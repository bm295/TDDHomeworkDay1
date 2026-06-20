namespace Adapters.Outbound.Persistence.EntityFramework;

public sealed class OrderItemRecord
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public string Sku { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal LineTotal { get; set; }
}
