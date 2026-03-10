using Application.Models;

namespace Application.Ports;

public interface IReportingReadModel
{
    Task<SalesReportDto> BuildDailySalesReportAsync(DateOnly day, CancellationToken cancellationToken);
}
