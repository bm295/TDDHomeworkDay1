# Restaurant Management System Architecture Review

## 1. Repository overview

- The repository is a small .NET solution with two projects: `Shop` (application code) and `UnitTest` (xUnit tests). The solution does **not** use the expected multi-project/layer folder layout (`/src/Domain`, `/src/Application`, `/src/Adapters`, `/src/Infrastructure`, `/src/Api`).
- Core business-relevant code is concentrated in `Shop/FnbManagement.cs`, which includes multiple concepts in one file: configuration, table, reservation, billing models, and a service.
- Additional legacy/example code (`SumCalculator`, `Validator`, and async console snippet in `Program`) is not aligned with an FnB restaurant management bounded context.

## 2. Architecture evaluation

- **Current architecture style:** Flat, monolithic class library + console entrypoint.
- There is no clear separation into Domain/Application/Adapters/Infrastructure boundaries.
- No API layer, no persistence abstractions, and no external integration adapters are present.
- The code is largely in-memory and synchronous service logic.

## 3. Hexagonal architecture compliance

### Required hexagonal elements vs current implementation

- **Domain layer**: Partially present as plain classes (`Table`, `Reservation`, `BillItem`, `BillSummary`) and domain-ish service logic (`FnbManagementService`) in `Shop/FnbManagement.cs`.
- **Application layer**: Missing (no use cases, command/query models, orchestrating services).
- **Ports**: Missing (no interfaces for repositories, payment gateways, inventory providers, kitchen messaging, reporting).
- **Adapters**: Missing (no persistence adapter, no HTTP/API adapter, no external integrations).
- **Infrastructure layer**: Missing (no database implementations, messaging bus integration, framework hosting concerns).

**Conclusion:** The repository does **not** follow hexagonal architecture.

## 4. Domain model evaluation

### Strengths

- Seat-capacity guardrail is explicitly modeled via `FnbConfiguration` with 80–120 seat validation.
- Table uniqueness and capacity checks exist in `ConfigureTables`.
- Reservation seat tracking and bill summary calculations are implemented.

### Gaps

- Domain entities are simplistic and mutable boundaries are not rigorously enforced as rich aggregates.
- No `Order` aggregate exists despite order lifecycle being a core requirement.
- No explicit payment domain model (payment method, transaction status, settlement, refund flow).
- No inventory domain model (stock items, deductions, replenishment, thresholds).
- No kitchen workflow model (ticket status, sent-to-kitchen timestamp/state).
- No reporting model.

## 5. FnB functionality coverage

Required operational flow vs implementation:

1. **Create order for a table** — ❌ Not implemented (reservation exists, but not order creation/association).
2. **Add / remove items** — ❌ Not implemented for order lifecycle.
3. **Send order to kitchen** — ❌ Not implemented.
4. **Process payment** — ⚠️ Partial only via `GenerateBill`; no payment transaction processing.
5. **Deduct inventory** — ❌ Not implemented.
6. **Close order** — ❌ Not implemented.

Additional required capabilities:

- **Order management**: Missing.
- **Payment processing**: Missing real processing.
- **Inventory tracking**: Missing.
- **Table/seat management**: Partially implemented.
- **Basic reporting**: Missing.

## 6. Dependency direction analysis

- With only one main project (`Shop`), there is no enforceable inward dependency flow.
- Domain concepts are not isolated from entrypoint concerns (`Program.cs` exists in same project).
- No interfaces/ports exist to invert dependencies from domain/application to adapters.
- Tests reference the monolithic `Shop` project directly; this is normal for tests but does not validate hexagonal boundaries.

## 7. Code quality review

- **Dependency Injection:** Not used (no DI container setup, no constructor injection of ports).
- **Asynchronous programming:** Present only in demo snippet `Program.cs`; not used in business flows where async I/O would be expected.
- **Testability:** Unit tests exist and cover current simple logic well, but they do not cover required restaurant workflows.
- **Separation of concerns:** Weak; multiple responsibilities mixed in same file/project.
- **Maintainability:** Limited for growth; missing bounded contexts, explicit contracts, and layering.
- **C#/.NET target compliance:** Targets `net10.0` correctly. No explicit C# 14 language features/configuration are present.

## 8. Identified architectural violations

1. **No hexagonal layering or ports/adapters structure**
   - Evidence: flat `Shop` project with domain + app + entrypoint mixed.
2. **Missing application/use-case layer**
   - No use-case orchestration for order lifecycle.
3. **Business capability mismatch to required FnB domain**
   - No order, inventory, payment transaction, reporting modules.
4. **Legacy/non-domain code co-located with restaurant logic**
   - `SumCalculator`, `Validator`, and discount/account status logic appear unrelated to required restaurant management capabilities.
5. **No DI or infrastructure abstractions**
   - No repository interfaces, payment port interfaces, inventory ports, or adapter implementations.

## 9. Recommended refactoring

### Target structure

Create a layered solution structure such as:

- `src/Domain` — Entities, VOs, domain services, policies.
- `src/Application` — Use cases, commands/queries, port interfaces.
- `src/Adapters` — Web API controllers, persistence adapters, external API adapters.
- `src/Infrastructure` — EF Core DbContext/repositories, messaging implementations, payment client wiring.
- `src/Api` — Composition root and DI registration.

### Priority backlog

1. Define core aggregates: `Order`, `OrderItem`, `Table`, `Payment`, `InventoryItem`.
2. Model order state machine: Draft → SentToKitchen → Ready/Served → Paid → Closed.
3. Introduce application use cases:
   - `CreateOrderForTable`
   - `AddOrderItem` / `RemoveOrderItem`
   - `SendOrderToKitchen`
   - `ProcessPayment`
   - `DeductInventory`
   - `CloseOrder`
4. Add ports (interfaces) in Application:
   - `IOrderRepository`, `ITableRepository`, `IInventoryRepository`, `IPaymentGateway`, `IKitchenNotifier`, `IReportingReadModel`.
5. Implement adapters in Infrastructure/Adapters.
6. Wire DI in API host (`Microsoft.Extensions.DependencyInjection`).
7. Make I/O use cases async (`Task`, cancellation tokens).
8. Add architecture tests to enforce dependency direction (Domain has zero refs to adapters/infrastructure).
9. Remove or isolate unrelated legacy classes (`SumCalculator`, `Validator`) from FnB bounded context.

## 10. Overall verdict

**FAIL** — The repository does not follow Hexagonal Architecture and does not satisfy the required restaurant management capabilities for El Gaucho Hanoi (80–120 seats), beyond partial seat/reservation and bill calculations.
