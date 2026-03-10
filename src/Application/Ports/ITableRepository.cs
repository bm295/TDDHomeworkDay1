using Domain.Entities;

namespace Application.Ports;

public interface ITableRepository
{
    Task<IReadOnlyCollection<Table>> GetAllAsync(CancellationToken cancellationToken);
    Task<Table?> GetByNumberAsync(int tableNumber, CancellationToken cancellationToken);
}
