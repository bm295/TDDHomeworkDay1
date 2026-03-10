using Domain.Entities;

namespace Application.Models;

public sealed record OrderDto(Guid Id, int TableNumber, OrderStatus Status, decimal TotalAmount, int ItemCount);
