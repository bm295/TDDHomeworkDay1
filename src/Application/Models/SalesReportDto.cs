namespace Application.Models;

public sealed record SalesReportDto(DateOnly Day, int ClosedOrders, decimal GrossRevenue);
