namespace Adapters.Outbound.Persistence.EntityFramework;

public sealed class TableRecord
{
    public int Number { get; set; }

    public int Seats { get; set; }

    public string DiningArea { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
