using Domain.Entities;

namespace Application.Ports;

public interface IInventoryRepository
{
    Task<InventoryItem?> GetBySkuAsync(string sku, CancellationToken cancellationToken);
    Task UpdateAsync(InventoryItem item, CancellationToken cancellationToken);
}
