using Application.Models;
using Application.Ports;

namespace Application.UseCases;

public sealed class GetDailySalesReportUseCase
{
    private readonly IReportingReadModel _readModel;

    public GetDailySalesReportUseCase(IReportingReadModel readModel)
    {
        _readModel = readModel;
    }

    public Task<SalesReportDto> ExecuteAsync(DateOnly day, CancellationToken cancellationToken)
    {
        return _readModel.BuildDailySalesReportAsync(day, cancellationToken);
    }
}
