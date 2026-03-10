using Application.UseCases;
using Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddRestaurantManagement();

await using var provider = services.BuildServiceProvider().CreateAsyncScope();

var createOrder = provider.ServiceProvider.GetRequiredService<CreateOrderForTableUseCase>();
var addItem = provider.ServiceProvider.GetRequiredService<AddOrderItemUseCase>();
var removeItem = provider.ServiceProvider.GetRequiredService<RemoveOrderItemUseCase>();
var sendToKitchen = provider.ServiceProvider.GetRequiredService<SendOrderToKitchenUseCase>();
var processPayment = provider.ServiceProvider.GetRequiredService<ProcessPaymentUseCase>();
var closeOrder = provider.ServiceProvider.GetRequiredService<CloseOrderUseCase>();
var report = provider.ServiceProvider.GetRequiredService<GetDailySalesReportUseCase>();

var order = await createOrder.ExecuteAsync(tableNumber: 1, CancellationToken.None);
order = await addItem.ExecuteAsync(order.Id, "RIBEYE", "Ribeye", quantity: 2, unitPrice: 950000m, CancellationToken.None);
order = await addItem.ExecuteAsync(order.Id, "WINE", "House Wine", quantity: 1, unitPrice: 1200000m, CancellationToken.None);
order = await removeItem.ExecuteAsync(order.Id, "WINE", quantity: 1, CancellationToken.None);
order = await addItem.ExecuteAsync(order.Id, "WINE", "House Wine", quantity: 1, unitPrice: 1200000m, CancellationToken.None);
order = await sendToKitchen.ExecuteAsync(order.Id, CancellationToken.None);
order = await processPayment.ExecuteAsync(order.Id, method: "Card", CancellationToken.None);
order = await closeOrder.ExecuteAsync(order.Id, CancellationToken.None);

var daily = await report.ExecuteAsync(DateOnly.FromDateTime(DateTime.UtcNow), CancellationToken.None);

Console.WriteLine($"Closed order {order.Id} for table {order.TableNumber} with total {order.TotalAmount:N0}.");
Console.WriteLine($"Daily report: {daily.Day} | Closed orders: {daily.ClosedOrders} | Revenue: {daily.GrossRevenue:N0}");
