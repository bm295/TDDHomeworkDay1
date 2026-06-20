namespace Adapters.Outbound.Persistence.EntityFramework;

public sealed class OrderRecord
{
    public Guid Id { get; set; }

    public int TableNumber { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? ClosedAtUtc { get; set; }

    public byte[] RowVersion { get; set; } = [];
}
