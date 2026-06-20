# Market Readiness Implementation Checklist

Use this checklist to turn El FnB from a technical MVP into sellable restaurant software. Each market-readiness theme is broken into smaller engineering tasks with suggested file names, class names, and acceptance checks.

## 1. Durable data storage

- [ ] Create project folder `src/Adapters/Outbound/Persistence/EntityFramework` for production persistence adapters.
- [ ] Add Entity Framework Core package references to `src/Adapters/Adapters.csproj` for the chosen database provider.
- [ ] Create `RestaurantDbContext` in `src/Adapters/Outbound/Persistence/EntityFramework/RestaurantDbContext.cs`.
- [ ] Add `DbSet<OrderRecord> Orders`, `DbSet<OrderItemRecord> OrderItems`, `DbSet<TableRecord> Tables`, `DbSet<InventoryItemRecord> InventoryItems`, and `DbSet<PaymentRecord> Payments` to `RestaurantDbContext`.
- [ ] Create persistence record class `OrderRecord` with columns for `Id`, `TableNumber`, `Status`, `CreatedAtUtc`, `ClosedAtUtc`, and `RowVersion`.
- [ ] Create persistence record class `OrderItemRecord` with columns for `Id`, `OrderId`, `Sku`, `Name`, `Quantity`, `UnitPrice`, and `LineTotal`.
- [ ] Create persistence record class `TableRecord` with columns for `Number`, `Seats`, `DiningArea`, and `IsActive`.
- [ ] Create persistence record class `InventoryItemRecord` with columns for `Sku`, `Name`, `QuantityOnHand`, `LowStockThreshold`, and `IsAvailable`.
- [ ] Create persistence record class `PaymentRecord` with columns for `Id`, `OrderId`, `Amount`, `Method`, `Status`, `PaidAtUtc`, and `TransactionId`.
- [ ] Create mapper class `OrderPersistenceMapper` to convert between `Domain.Entities.Order` and `OrderRecord`.
- [ ] Create mapper class `InventoryPersistenceMapper` to convert between `Domain.Entities.InventoryItem` and `InventoryItemRecord`.
- [ ] Implement `EfOrderRepository : IOrderRepository`.
- [ ] Implement `EfTableRepository : ITableRepository`.
- [ ] Implement `EfInventoryRepository : IInventoryRepository`.
- [ ] Implement `EfReportingReadModel : IReportingReadModel`.
- [ ] Add database connection string key `RestaurantDatabase` to production configuration documentation.
- [ ] Update `ServiceCollectionExtensions.AddRestaurantManagement` to use EF repositories when a database connection is configured.
- [ ] Create an initial database migration named `InitialRestaurantSchema`.
- [ ] Add seed data for tables and default menu inventory through a migration or explicit startup seed command.
- [ ] Acceptance check: orders, payments, inventory, and reports survive application restart.

## 2. Menu, pricing, and inventory management

- [ ] Create domain entity `MenuItem` in `src/Domain/Entities/MenuItem.cs`.
- [ ] Create domain entity `MenuModifier` in `src/Domain/Entities/MenuModifier.cs`.
- [ ] Create domain entity `MenuCategory` in `src/Domain/Entities/MenuCategory.cs`.
- [ ] Create application port `IMenuRepository` in `src/Application/Ports/IMenuRepository.cs`.
- [ ] Create DTO `MenuItemDetailsDto` in `src/Application/Models/MenuItemDetailsDto.cs`.
- [ ] Create use case `CreateMenuItemUseCase`.
- [ ] Create use case `UpdateMenuItemPriceUseCase`.
- [ ] Create use case `DeactivateMenuItemUseCase`.
- [ ] Create use case `CreateMenuModifierUseCase`.
- [ ] Create use case `MarkMenuItemOutOfStockUseCase`.
- [ ] Create use case `AdjustInventoryQuantityUseCase` with reason codes for count correction, waste, delivery, and comp.
- [ ] Create controller `MenuAdminController` in `src/Web/Controllers/MenuAdminController.cs`.
- [ ] Create Razor view `src/Web/Views/MenuAdmin/Index.cshtml` for menu and stock administration.
- [ ] Create Razor view `src/Web/Views/MenuAdmin/Edit.cshtml` for item editing.
- [ ] Add validation model `MenuItemInputModel` with required SKU, name, category, and price fields.
- [ ] Add validation model `InventoryAdjustmentInputModel` with SKU, quantity delta, and reason.
- [ ] Acceptance check: a manager can add, price, disable, and mark menu items out of stock without code changes.

## 3. Order workflow improvements

- [ ] Create domain entity `DiningArea` in `src/Domain/Entities/DiningArea.cs`.
- [ ] Add order note support by creating value object `OrderNote` in `src/Domain/Entities/OrderNote.cs`.
- [ ] Add line-item note support by extending `OrderItem` with a `SpecialInstructions` property.
- [ ] Create use case `MoveOrderToTableUseCase`.
- [ ] Create use case `MergeOrdersUseCase`.
- [ ] Create use case `SplitOrderUseCase`.
- [ ] Create use case `ApplyDiscountUseCase`.
- [ ] Create use case `VoidOrderItemUseCase`.
- [ ] Create use case `CompOrderItemUseCase`.
- [ ] Create DTO `OrderAdjustmentDto` for discounts, comps, and voids.
- [ ] Create input model `MoveOrderInputModel`.
- [ ] Create input model `SplitOrderInputModel`.
- [ ] Create input model `DiscountInputModel`.
- [ ] Add manager-only checks to discount, void, comp, merge, and split actions.
- [ ] Add UI controls to the operations dashboard for notes, discounts, move table, split, merge, void, and comp workflows.
- [ ] Acceptance check: staff can handle common dine-in exceptions without deleting and recreating orders.

## 4. Kitchen display and printing

- [ ] Create domain entity `KitchenTicket` in `src/Domain/Entities/KitchenTicket.cs`.
- [ ] Create enum `KitchenTicketStatus` with `Sent`, `Accepted`, `Preparing`, `Ready`, `Served`, `Cancelled`, and `Delayed`.
- [ ] Create application port `IKitchenTicketRepository`.
- [ ] Create application port `IKitchenDisplayNotifier`.
- [ ] Create use case `CreateKitchenTicketUseCase` that runs when an order is sent to kitchen.
- [ ] Create use case `UpdateKitchenTicketStatusUseCase`.
- [ ] Create use case `RecallKitchenTicketUseCase` for corrected tickets.
- [ ] Implement `EfKitchenTicketRepository`.
- [ ] Implement `SignalRKitchenDisplayNotifier` or an equivalent real-time notifier.
- [ ] Create `KitchenHub` in `src/Web/Hubs/KitchenHub.cs` if SignalR is selected.
- [ ] Create controller `KitchenController`.
- [ ] Create view `src/Web/Views/Kitchen/Index.cshtml` for kitchen staff.
- [ ] Create print adapter `ReceiptPrinterKitchenNotifier` for physical kitchen printers if printing is required.
- [ ] Acceptance check: kitchen staff can see, update, and complete tickets without watching server logs.

## 5. Payments, receipts, and fiscal behavior

- [ ] Create enum `PaymentStatus` with `Pending`, `Approved`, `Failed`, `Voided`, `Refunded`, and `PartiallyRefunded`.
- [ ] Create value object `Money` in `src/Domain/Entities/Money.cs` if multiple currencies or precise rounding rules are required.
- [ ] Create domain entity `Receipt` in `src/Domain/Entities/Receipt.cs`.
- [ ] Create application port `IReceiptRepository`.
- [ ] Create application port `IReceiptRenderer`.
- [ ] Create application port `IRefundGateway` or extend `IPaymentGateway` with refund and void operations.
- [ ] Create payment adapter class for the selected provider, for example `StripePaymentGateway`, `AdyenPaymentGateway`, or `LocalTerminalPaymentGateway`.
- [ ] Create provider options class, for example `PaymentGatewayOptions`, with endpoint, merchant ID, terminal ID, and secret references.
- [ ] Create use case `CreatePaymentIntentUseCase`.
- [ ] Create use case `ConfirmPaymentUseCase`.
- [ ] Create use case `RefundPaymentUseCase`.
- [ ] Create use case `VoidPaymentUseCase`.
- [ ] Create use case `GenerateReceiptUseCase`.
- [ ] Create domain service `TaxCalculationService`.
- [ ] Create domain service `RoundingPolicy`.
- [ ] Create DTO `ReceiptDto`.
- [ ] Add receipt view `src/Web/Views/Operations/Receipt.cshtml`.
- [ ] Add idempotency key field to payment commands to prevent duplicate charges.
- [ ] Acceptance check: payment retries do not double-charge customers or double-deduct inventory.

## 6. Authentication, authorization, and auditing

- [ ] Add identity package references to `src/Web/Web.csproj` or create a dedicated identity infrastructure project.
- [ ] Create domain entity `StaffUser` in `src/Domain/Entities/StaffUser.cs`.
- [ ] Create enum `StaffRole` with `Server`, `Cashier`, `Kitchen`, `Manager`, `Owner`, and `Support`.
- [ ] Create application port `IStaffUserRepository`.
- [ ] Create application port `IAuditLogRepository`.
- [ ] Create domain entity `AuditLogEntry`.
- [ ] Create service `AuditLogService`.
- [ ] Create authorization policy constants in `src/Web/Security/Policies.cs`.
- [ ] Add `UseAuthentication()` and `UseAuthorization()` to `src/Web/Program.cs`.
- [ ] Add `[Authorize]` to operational controllers.
- [ ] Add manager-only policies to payment refunds, discounts, voids, comps, menu changes, and staff management.
- [ ] Create controller `StaffAdminController`.
- [ ] Create view `src/Web/Views/StaffAdmin/Index.cshtml`.
- [ ] Create view `src/Web/Views/StaffAdmin/Edit.cshtml`.
- [ ] Create middleware or action filter `AuditActionFilter` for sensitive operations.
- [ ] Acceptance check: unauthenticated users cannot access the service board, and non-manager users cannot perform manager actions.

## 7. Configuration and environment hardening

- [ ] Create options class `RestaurantOptions` for location name, currency, timezone, seat count, and feature flags.
- [ ] Create options class `KitchenOptions` for display, printer, and routing settings.
- [ ] Create options class `SecurityOptions` for session timeout, password policy, and support access controls.
- [ ] Add `appsettings.Production.json` with placeholders only, not secrets.
- [ ] Add `.env.example` with required environment variable names and safe example values.
- [ ] Add startup validation class `OptionsValidationService` to fail fast on missing production configuration.
- [ ] Add health check endpoint `/health` for app and dependency status.
- [ ] Add readiness endpoint `/ready` for deployment orchestration.
- [ ] Add structured logging configuration for production.
- [ ] Add correlation ID middleware for request tracing.
- [ ] Acceptance check: production startup fails clearly when required database, payment, or security settings are missing.

## 8. Reliability, concurrency, and recovery

- [ ] Add optimistic concurrency tokens to order, inventory, payment, and kitchen ticket records.
- [ ] Create exception type `ConcurrencyConflictException`.
- [ ] Create retry-safe command model with `CommandId` for important state changes.
- [ ] Create table `ProcessedCommandRecord` for idempotent command tracking.
- [ ] Create service `IdempotencyService`.
- [ ] Update payment and closeout use cases to use idempotency keys.
- [ ] Add background job or hosted service `DailyCloseoutSnapshotService`.
- [ ] Add backup and restore runbook under `docs/runbooks/backup-and-restore.md`.
- [ ] Add incident response runbook under `docs/runbooks/incident-response.md`.
- [ ] Add deployment rollback runbook under `docs/runbooks/rollback.md`.
- [ ] Acceptance check: duplicate form submissions and network retries do not create duplicate charges, duplicate tickets, or incorrect inventory deductions.

## 9. Testing and quality gates

- [ ] Create test project `tests/Domain.Tests/Domain.Tests.csproj`.
- [ ] Create test project `tests/Application.Tests/Application.Tests.csproj`.
- [ ] Create test project `tests/Adapters.Tests/Adapters.Tests.csproj`.
- [ ] Create test project `tests/Web.Tests/Web.Tests.csproj`.
- [ ] Add all test projects to `ElFnB.sln`.
- [ ] Create `OrderTests` for add item, remove item, send to kitchen, pay, close, and invalid transitions.
- [ ] Create `InventoryItemTests` for low stock, deduction, and insufficient stock behavior.
- [ ] Create `ProcessPaymentUseCaseTests` for approved, failed, retried, and insufficient-stock payment paths.
- [ ] Create `CreateOrderForTableUseCaseTests` for duplicate active table orders.
- [ ] Create `EfOrderRepositoryTests` using a disposable test database.
- [ ] Create `OperationsControllerTests` for authorization and validation behavior.
- [ ] Add end-to-end test project or folder `tests/EndToEnd` for service-board browser tests.
- [ ] Add accessibility test checks for labels, focus order, and color contrast.
- [ ] Add CI workflow that runs restore, build, unit tests, integration tests, formatting checks, and security scans.
- [ ] Acceptance check: every pull request proves core restaurant workflows still work before merge.

## 10. UX, documentation, and onboarding

- [ ] Create onboarding controller `SetupController`.
- [ ] Create setup view `src/Web/Views/Setup/Restaurant.cshtml` for restaurant identity, currency, timezone, and tax settings.
- [ ] Create setup view `src/Web/Views/Setup/Tables.cshtml` for dining areas and tables.
- [ ] Create setup view `src/Web/Views/Setup/Menu.cshtml` for initial menu import or manual entry.
- [ ] Create setup view `src/Web/Views/Setup/Staff.cshtml` for initial staff accounts.
- [ ] Create import model `MenuImportRow` for CSV menu upload.
- [ ] Create sample CSV `docs/samples/menu-import-template.csv`.
- [ ] Add user guide `docs/user-guide/service-board.md`.
- [ ] Add manager guide `docs/user-guide/manager-operations.md`.
- [ ] Add admin guide `docs/admin-guide/configuration.md`.
- [ ] Add troubleshooting guide `docs/support/troubleshooting.md`.
- [ ] Add sales demo script `docs/sales/demo-script.md`.
- [ ] Acceptance check: a new restaurant can configure tables, staff, menu, taxes, and payments without developer help.

## 11. Commercial, legal, and launch operations

- [ ] Create pricing and packaging document `docs/business/pricing-and-packaging.md`.
- [ ] Create implementation checklist `docs/customer-onboarding/implementation-checklist.md`.
- [ ] Create customer data export plan `docs/compliance/data-export.md`.
- [ ] Create privacy and data handling notes `docs/compliance/privacy-readiness.md`.
- [ ] Create payment compliance notes `docs/compliance/payment-readiness.md`.
- [ ] Create support runbook `docs/support/support-runbook.md`.
- [ ] Create pilot feedback form `docs/customer-onboarding/pilot-feedback-form.md`.
- [ ] Create release checklist `docs/release/release-checklist.md`.
- [ ] Create launch go/no-go checklist `docs/release/launch-go-no-go.md`.
- [ ] Acceptance check: product, engineering, support, and business stakeholders have documented launch criteria and owner sign-off.
