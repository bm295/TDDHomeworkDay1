# El Gaucho Hanoi - FnB Management (Hexagonal Architecture)

This repository now includes a **Hexagonal Architecture (Ports and Adapters)** implementation for a restaurant management system sized for **80-120 seats**.

## Architecture

```text
/src
  /Domain          # Entities, value objects, domain services, business rules
  /Application     # Use cases, DTOs, ports (interfaces)
  /Adapters        # In-memory persistence and external adapter implementations
  /Infrastructure  # Dependency injection and composition support
  /Api             # Executable entrypoint that exercises operational flows
```

## Supported restaurant flows

1. Create order for a table
2. Add / remove items
3. Send order to kitchen
4. Process payment
5. Deduct inventory
6. Close order
7. Generate basic daily sales report

## Runtime target

All new projects target **.NET 10** (`net10.0`).

## Run sample flow

```bash
dotnet run --project src/Api/Api.csproj
```

The sample runs a full order lifecycle and prints a basic report.
