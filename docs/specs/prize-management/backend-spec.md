# Prize Management - Backend Specification

## Document Information
| Field | Value |
|-------|-------|
| Feature | Prize Management |
| Version | 1.0 |
| Date | 2025-12-22 |
| Status | Final |

---

## 1. Overview

### 1.1 Purpose
The Prize Management module handles prize inventory, ordering for events, distribution tracking, and prize pack management across the EventManagementPlatform system.

### 1.2 Scope
This specification covers all backend requirements for prize item management, inventory tracking, event prize ordering, packing, shipping, distribution, and prize pack creation.

### 1.3 Technology Stack
- **Framework**: .NET 8.0
- **Database**: SQL Server Express (via Entity Framework Core)
- **Cloud**: Azure App Service, Azure SQL Database
- **AI Integration**: Azure AI Services for prize recommendations and demand forecasting
- **Messaging**: MediatR for CQRS pattern implementation
- **Storage**: Azure Blob Storage for prize item photos

---

## 2. Domain Model

### 2.1 Aggregate: PrizeItem
The PrizeItem aggregate represents individual prize items that can be ordered and distributed at events.

#### 2.1.1 Entity Properties
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| PrizeItemId | Guid | Yes | Unique identifier |
| Name | string | Yes | Prize item name (max 200 chars) |
| Description | string | No | Prize description (max 2000 chars) |
| SKU | string | Yes | Stock keeping unit (unique) |
| Category | string | Yes | Prize category (max 100 chars) |
| UnitCost | decimal | Yes | Cost per unit |
| CurrentStock | int | Yes | Current inventory quantity |
| MinimumStock | int | Yes | Low stock threshold |
| IsActive | bool | Yes | Whether item is active |
| PhotoUrl | string | No | Azure Blob Storage URL |
| CreatedAt | DateTime | Yes | Creation timestamp |
| ModifiedAt | DateTime | No | Last modification timestamp |
| CreatedBy | Guid | Yes | User who created |
| ModifiedBy | Guid | No | User who last modified |

### 2.2 Aggregate: PrizePack
The PrizePack aggregate represents predefined collections of prize items.

#### 2.2.1 Entity Properties
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| PrizePackId | Guid | Yes | Unique identifier |
| Name | string | Yes | Pack name (max 200 chars) |
| Description | string | No | Pack description (max 2000 chars) |
| IsActive | bool | Yes | Whether pack is active |
| CreatedAt | DateTime | Yes | Creation timestamp |
| ModifiedAt | DateTime | No | Last modification timestamp |

#### 2.2.2 Entity: PrizePackItem
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| PrizePackItemId | Guid | Yes | Unique identifier |
| PrizePackId | Guid | Yes | Parent pack reference |
| PrizeItemId | Guid | Yes | Prize item reference |
| Quantity | int | Yes | Number of items in pack |

### 2.3 Aggregate: EventPrizeOrder
The EventPrizeOrder aggregate manages prize orders for events.

#### 2.3.1 Entity Properties
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| EventPrizeOrderId | Guid | Yes | Unique identifier |
| EventId | Guid | Yes | Reference to Event |
| OrderStatus | OrderStatus | Yes | Current order status |
| OrderDate | DateTime | Yes | Order placement date |
| RequiredByDate | DateTime | Yes | Date prizes needed |
| PackedDate | DateTime | No | Date prizes were packed |
| ShippedDate | DateTime | No | Date prizes were shipped |
| DeliveredDate | DateTime | No | Date prizes were delivered |
| DistributedDate | DateTime | No | Date prizes were distributed |
| Notes | string | No | Order notes (max 4000 chars) |
| CreatedAt | DateTime | Yes | Creation timestamp |
| ModifiedAt | DateTime | No | Last modification timestamp |
| CreatedBy | Guid | Yes | User who created |
| ModifiedBy | Guid | No | User who last modified |

#### 2.3.2 OrderStatus Enumeration
```csharp
public enum OrderStatus
{
    Draft = 0,
    Submitted = 1,
    Confirmed = 2,
    Allocated = 3,
    Packed = 4,
    Shipped = 5,
    Delivered = 6,
    Distributed = 7,
    Returned = 8,
    Cancelled = 9
}
```

#### 2.3.3 Entity: EventPrizeOrderItem
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| EventPrizeOrderItemId | Guid | Yes | Unique identifier |
| EventPrizeOrderId | Guid | Yes | Parent order reference |
| PrizeItemId | Guid | Yes | Prize item reference |
| Quantity | int | Yes | Quantity ordered |
| AllocatedQuantity | int | Yes | Quantity allocated from inventory |
| DistributedQuantity | int | Yes | Quantity actually distributed |
| ReturnedQuantity | int | Yes | Quantity returned to inventory |

### 2.4 Entity: InventoryTransaction
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| InventoryTransactionId | Guid | Yes | Unique identifier |
| PrizeItemId | Guid | Yes | Prize item reference |
| TransactionType | TransactionType | Yes | Type of transaction |
| Quantity | int | Yes | Quantity (positive or negative) |
| ReferenceId | Guid | No | Reference to order/adjustment |
| TransactionDate | DateTime | Yes | Transaction timestamp |
| Notes | string | No | Transaction notes |
| CreatedBy | Guid | Yes | User who created transaction |

#### 2.4.1 TransactionType Enumeration
```csharp
public enum TransactionType
{
    Received = 0,
    Allocated = 1,
    Deallocated = 2,
    Adjustment = 3,
    Distributed = 4,
    Returned = 5,
    Damaged = 6,
    Lost = 7
}
```

---

## 3. Domain Events

### 3.1 Prize Item Events
| Event | Trigger | Payload |
|-------|---------|---------|
| PrizeItemAdded | New prize item created | PrizeItemId, Name, SKU, Category |
| PrizeItemUpdated | Prize item modified | PrizeItemId, ChangedProperties |
| PrizeItemRemoved | Prize item deleted | PrizeItemId, Name |
| PrizeItemActivated | Prize item activated | PrizeItemId |
| PrizeItemDeactivated | Prize item deactivated | PrizeItemId |
| PrizeItemPhotoUploaded | Photo added to item | PrizeItemId, PhotoUrl |

### 3.2 Prize Pack Events
| Event | Trigger | Payload |
|-------|---------|---------|
| PrizePackCreated | New pack created | PrizePackId, Name |
| PrizePackUpdated | Pack modified | PrizePackId, ChangedProperties |

### 3.3 Event Prize Order Events
| Event | Trigger | Payload |
|-------|---------|---------|
| EventPrizesOrdered | New order created | EventPrizeOrderId, EventId, TotalItems |
| EventPrizeOrderUpdated | Order modified | EventPrizeOrderId, ChangedProperties |
| EventPrizeOrderCancelled | Order cancelled | EventPrizeOrderId, Reason |
| PrizeItemAddedToOrder | Item added to order | EventPrizeOrderId, PrizeItemId, Quantity |
| PrizeItemRemovedFromOrder | Item removed from order | EventPrizeOrderId, PrizeItemId |

### 3.4 Inventory Events
| Event | Trigger | Payload |
|-------|---------|---------|
| PrizeQuantityIncreased | Stock increased | PrizeItemId, Quantity, NewTotal |
| PrizeQuantityDecreased | Stock decreased | PrizeItemId, Quantity, NewTotal |
| PrizeInventoryReceived | New stock received | PrizeItemId, Quantity |
| PrizeInventoryAdjusted | Stock adjusted | PrizeItemId, OldQuantity, NewQuantity |
| PrizeInventoryAllocated | Stock allocated to order | PrizeItemId, EventPrizeOrderId, Quantity |
| PrizeInventoryDeallocated | Stock released from order | PrizeItemId, EventPrizeOrderId, Quantity |
| PrizeStockDepleted | Stock reached zero | PrizeItemId |
| PrizeLowStockWarningTriggered | Stock below minimum | PrizeItemId, CurrentStock, MinimumStock |
| PrizeInventoryReplenished | Stock replenished above minimum | PrizeItemId, NewStock |

### 3.5 Prize Distribution Events
| Event | Trigger | Payload |
|-------|---------|---------|
| PrizesPackedForEvent | Prizes packed | EventPrizeOrderId, PackedDate, PackedBy |
| PrizesShippedToEvent | Prizes shipped | EventPrizeOrderId, ShippedDate, TrackingNumber |
| PrizesDeliveredToEvent | Prizes delivered | EventPrizeOrderId, DeliveredDate |
| PrizesDistributedAtEvent | Prizes distributed | EventPrizeOrderId, DistributedDate, DistributedQuantity |
| UnusedPrizesReturned | Prizes returned | EventPrizeOrderId, ReturnedDate, ReturnedQuantity |

---

## 4. API Endpoints

### 4.1 Prize Item Management Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/prizes | List all prize items with pagination |
| GET | /api/prizes/{prizeItemId} | Get prize item by ID |
| POST | /api/prizes | Create new prize item |
| PUT | /api/prizes/{prizeItemId} | Update prize item |
| DELETE | /api/prizes/{prizeItemId} | Soft delete prize item |
| POST | /api/prizes/{prizeItemId}/activate | Activate prize item |
| POST | /api/prizes/{prizeItemId}/deactivate | Deactivate prize item |
| POST | /api/prizes/{prizeItemId}/photo | Upload prize photo |
| GET | /api/prizes/low-stock | Get items with low stock |

### 4.2 Prize Pack Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/prize-packs | List all prize packs |
| GET | /api/prize-packs/{prizePackId} | Get prize pack by ID |
| POST | /api/prize-packs | Create new prize pack |
| PUT | /api/prize-packs/{prizePackId} | Update prize pack |
| DELETE | /api/prize-packs/{prizePackId} | Delete prize pack |
| POST | /api/prize-packs/{prizePackId}/items | Add item to pack |
| DELETE | /api/prize-packs/{prizePackId}/items/{itemId} | Remove item from pack |

### 4.3 Event Prize Order Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/events/{eventId}/prize-orders | Get prize orders for event |
| GET | /api/prize-orders/{orderId} | Get prize order by ID |
| POST | /api/events/{eventId}/prize-orders | Create prize order |
| PUT | /api/prize-orders/{orderId} | Update prize order |
| DELETE | /api/prize-orders/{orderId} | Cancel prize order |
| POST | /api/prize-orders/{orderId}/items | Add item to order |
| DELETE | /api/prize-orders/{orderId}/items/{itemId} | Remove item from order |
| POST | /api/prize-orders/{orderId}/pack | Mark prizes as packed |
| POST | /api/prize-orders/{orderId}/ship | Mark prizes as shipped |
| POST | /api/prize-orders/{orderId}/deliver | Mark prizes as delivered |
| POST | /api/prize-orders/{orderId}/distribute | Mark prizes as distributed |
| POST | /api/prize-orders/{orderId}/return | Process prize returns |

### 4.4 Inventory Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/prizes/{prizeItemId}/inventory | Get inventory history |
| POST | /api/prizes/{prizeItemId}/receive | Receive new inventory |
| POST | /api/prizes/{prizeItemId}/adjust | Adjust inventory |
| GET | /api/inventory/transactions | Get all inventory transactions |
| GET | /api/inventory/low-stock-report | Get low stock report |

---

## 5. Commands and Queries (CQRS)

### 5.1 Commands
```
EventManagementPlatform.Api/Features/Prizes/
├── CreatePrizeItem/
│   ├── CreatePrizeItemCommand.cs
│   ├── CreatePrizeItemCommandHandler.cs
│   └── CreatePrizeItemDto.cs
├── UpdatePrizeItem/
│   ├── UpdatePrizeItemCommand.cs
│   ├── UpdatePrizeItemCommandHandler.cs
│   └── UpdatePrizeItemDto.cs
├── ActivatePrizeItem/
│   ├── ActivatePrizeItemCommand.cs
│   └── ActivatePrizeItemCommandHandler.cs
├── DeactivatePrizeItem/
│   ├── DeactivatePrizeItemCommand.cs
│   └── DeactivatePrizeItemCommandHandler.cs
├── UploadPrizePhoto/
│   ├── UploadPrizePhotoCommand.cs
│   └── UploadPrizePhotoCommandHandler.cs
├── CreatePrizePack/
│   ├── CreatePrizePackCommand.cs
│   ├── CreatePrizePackCommandHandler.cs
│   └── CreatePrizePackDto.cs
├── CreateEventPrizeOrder/
│   ├── CreateEventPrizeOrderCommand.cs
│   ├── CreateEventPrizeOrderCommandHandler.cs
│   └── CreateEventPrizeOrderDto.cs
├── UpdateEventPrizeOrder/
│   ├── UpdateEventPrizeOrderCommand.cs
│   └── UpdateEventPrizeOrderCommandHandler.cs
├── AddPrizeToOrder/
│   ├── AddPrizeToOrderCommand.cs
│   └── AddPrizeToOrderCommandHandler.cs
├── PackPrizesForEvent/
│   ├── PackPrizesForEventCommand.cs
│   └── PackPrizesForEventCommandHandler.cs
├── ShipPrizesToEvent/
│   ├── ShipPrizesToEventCommand.cs
│   └── ShipPrizesToEventCommandHandler.cs
├── DistributePrizes/
│   ├── DistributePrizesCommand.cs
│   └── DistributePrizesCommandHandler.cs
├── ReturnPrizes/
│   ├── ReturnPrizesCommand.cs
│   └── ReturnPrizesCommandHandler.cs
├── ReceiveInventory/
│   ├── ReceiveInventoryCommand.cs
│   └── ReceiveInventoryCommandHandler.cs
└── AdjustInventory/
    ├── AdjustInventoryCommand.cs
    └── AdjustInventoryCommandHandler.cs
```

### 5.2 Queries
```
EventManagementPlatform.Api/Features/Prizes/
├── GetPrizeItems/
│   ├── GetPrizeItemsQuery.cs
│   ├── GetPrizeItemsQueryHandler.cs
│   └── PrizeItemListDto.cs
├── GetPrizeItemById/
│   ├── GetPrizeItemByIdQuery.cs
│   ├── GetPrizeItemByIdQueryHandler.cs
│   └── PrizeItemDetailDto.cs
├── GetLowStockItems/
│   ├── GetLowStockItemsQuery.cs
│   ├── GetLowStockItemsQueryHandler.cs
│   └── LowStockItemDto.cs
├── GetPrizePacks/
│   ├── GetPrizePacksQuery.cs
│   ├── GetPrizePacksQueryHandler.cs
│   └── PrizePackListDto.cs
├── GetPrizePackById/
│   ├── GetPrizePackByIdQuery.cs
│   ├── GetPrizePackByIdQueryHandler.cs
│   └── PrizePackDetailDto.cs
├── GetEventPrizeOrders/
│   ├── GetEventPrizeOrdersQuery.cs
│   ├── GetEventPrizeOrdersQueryHandler.cs
│   └── EventPrizeOrderListDto.cs
├── GetEventPrizeOrderById/
│   ├── GetEventPrizeOrderByIdQuery.cs
│   ├── GetEventPrizeOrderByIdQueryHandler.cs
│   └── EventPrizeOrderDetailDto.cs
└── GetInventoryTransactions/
    ├── GetInventoryTransactionsQuery.cs
    ├── GetInventoryTransactionsQueryHandler.cs
    └── InventoryTransactionDto.cs
```

---

## 6. Business Rules

### 6.1 Prize Item Rules
| Rule ID | Description |
|---------|-------------|
| PRI-001 | Prize item SKU must be unique |
| PRI-002 | Prize item name is required (max 200 chars) |
| PRI-003 | Unit cost must be greater than zero |
| PRI-004 | Current stock cannot be negative |
| PRI-005 | Minimum stock must be non-negative |
| PRI-006 | Cannot delete prize item if in active orders |
| PRI-007 | Cannot deactivate prize item if allocated to orders |

### 6.2 Prize Pack Rules
| Rule ID | Description |
|---------|-------------|
| PRI-010 | Prize pack must contain at least one item |
| PRI-011 | Cannot add inactive prize items to pack |
| PRI-012 | Prize pack name is required and unique |
| PRI-013 | Cannot delete pack if used in recent orders |

### 6.3 Event Prize Order Rules
| Rule ID | Description |
|---------|-------------|
| PRI-020 | Order required date must be before event date |
| PRI-021 | Cannot modify order after status is Packed |
| PRI-022 | Cannot pack order if insufficient inventory |
| PRI-023 | Order must have at least one item |
| PRI-024 | Cannot ship before packing |
| PRI-025 | Cannot deliver before shipping |
| PRI-026 | Cannot distribute before delivery |

### 6.4 Inventory Rules
| Rule ID | Description |
|---------|-------------|
| PRI-030 | Inventory adjustment requires reason/notes |
| PRI-031 | Cannot allocate more than current stock |
| PRI-032 | Low stock warning triggers at minimum threshold |
| PRI-033 | Returned quantity cannot exceed distributed quantity |
| PRI-034 | Inventory transactions are immutable (append-only) |

---

## 7. Azure Integration

### 7.1 Azure AI Services
| Service | Purpose |
|---------|---------|
| Azure OpenAI | Prize demand forecasting based on event type |
| Azure Cognitive Services | Image recognition for prize photo validation |
| Azure Anomaly Detector | Detect unusual inventory movements |

### 7.2 Azure Infrastructure
| Service | Purpose |
|---------|---------|
| Azure App Service | Host API application |
| Azure SQL Database | Primary data storage |
| Azure Blob Storage | Prize item photos and documents |
| Azure Service Bus | Event publishing for integrations |
| Azure Application Insights | Monitoring and telemetry |
| Azure Functions | Scheduled low stock notifications |

---

## 8. Data Transfer Objects (DTOs)

### 8.1 CreatePrizeItemDto
```csharp
public record CreatePrizeItemDto(
    string Name,
    string? Description,
    string SKU,
    string Category,
    decimal UnitCost,
    int CurrentStock,
    int MinimumStock
);
```

### 8.2 PrizeItemDetailDto
```csharp
public record PrizeItemDetailDto(
    Guid PrizeItemId,
    string Name,
    string? Description,
    string SKU,
    string Category,
    decimal UnitCost,
    int CurrentStock,
    int MinimumStock,
    bool IsActive,
    string? PhotoUrl,
    DateTime CreatedAt,
    DateTime? ModifiedAt
);
```

### 8.3 CreateEventPrizeOrderDto
```csharp
public record CreateEventPrizeOrderDto(
    Guid EventId,
    DateTime RequiredByDate,
    IEnumerable<OrderItemDto> Items,
    string? Notes
);

public record OrderItemDto(
    Guid PrizeItemId,
    int Quantity
);
```

### 8.4 EventPrizeOrderDetailDto
```csharp
public record EventPrizeOrderDetailDto(
    Guid EventPrizeOrderId,
    Guid EventId,
    string EventTitle,
    string OrderStatus,
    DateTime OrderDate,
    DateTime RequiredByDate,
    DateTime? PackedDate,
    DateTime? ShippedDate,
    DateTime? DeliveredDate,
    DateTime? DistributedDate,
    string? Notes,
    IEnumerable<EventPrizeOrderItemDto> Items,
    decimal TotalCost
);

public record EventPrizeOrderItemDto(
    Guid EventPrizeOrderItemId,
    Guid PrizeItemId,
    string PrizeItemName,
    int Quantity,
    int AllocatedQuantity,
    int DistributedQuantity,
    int ReturnedQuantity,
    decimal UnitCost
);
```

### 8.5 InventoryTransactionDto
```csharp
public record InventoryTransactionDto(
    Guid InventoryTransactionId,
    Guid PrizeItemId,
    string PrizeItemName,
    string TransactionType,
    int Quantity,
    DateTime TransactionDate,
    string? Notes,
    string CreatedByName
);
```

---

## 9. Validation Requirements

### 9.1 Input Validation
| Field | Validation Rules |
|-------|-----------------|
| Name | Required, 1-200 characters |
| SKU | Required, unique, alphanumeric |
| Category | Required, 1-100 characters |
| UnitCost | Required, > 0 |
| CurrentStock | Required, >= 0 |
| MinimumStock | Required, >= 0 |
| Quantity | Required, > 0 |
| RequiredByDate | Required, future date |

### 9.2 FluentValidation Implementation
```csharp
public class CreatePrizeItemCommandValidator : AbstractValidator<CreatePrizeItemCommand>
{
    public CreatePrizeItemCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.SKU)
            .NotEmpty()
            .Matches(@"^[A-Z0-9-]+$")
            .WithMessage("SKU must be alphanumeric");

        RuleFor(x => x.UnitCost)
            .GreaterThan(0)
            .WithMessage("Unit cost must be greater than zero");

        RuleFor(x => x.CurrentStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Stock cannot be negative");

        RuleFor(x => x.MinimumStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum stock cannot be negative");
    }
}
```

---

## 10. Security Requirements

### 10.1 Authentication
- All endpoints require JWT Bearer token authentication
- Tokens issued by Azure AD B2C or internal identity service

### 10.2 Authorization
| Role | Permissions |
|------|-------------|
| Staff | View prizes, create orders, view inventory |
| Warehouse | Manage inventory, pack/ship orders |
| Manager | All operations, adjust inventory, delete items |
| Admin | Full access including system configuration |

---

## 11. Performance Requirements

| Metric | Requirement |
|--------|-------------|
| API Response Time | < 200ms for 95th percentile |
| Prize List Query | Support 10,000+ items with pagination |
| Concurrent Users | Support 100 concurrent users |
| Inventory Update | < 100ms with transaction integrity |
| Photo Upload | Support images up to 5MB |

---

## 12. Error Handling

### 12.1 Error Response Format
```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "Bad Request",
    "status": 400,
    "detail": "Insufficient inventory for allocation",
    "instance": "/api/prize-orders/123/pack",
    "errors": {
        "Inventory": ["Only 5 units available, requested 10"]
    }
}
```

### 12.2 Exception Types
| Exception | HTTP Status | Description |
|-----------|-------------|-------------|
| ValidationException | 400 | Input validation failed |
| NotFoundException | 404 | Prize/order not found |
| InsufficientInventoryException | 409 | Not enough stock |
| InvalidStatusTransitionException | 409 | Status change not allowed |
| DuplicateSkuException | 409 | SKU already exists |
| UnauthorizedException | 401 | Authentication required |
| ForbiddenException | 403 | Insufficient permissions |

---

## 13. Testing Requirements

### 13.1 Unit Tests
- Test all command handlers
- Test all query handlers
- Test domain event generation
- Test validation rules
- Test inventory calculations

### 13.2 Integration Tests
- Test API endpoints
- Test database operations
- Test event publishing
- Test Azure Blob Storage integration
- Test inventory transaction integrity

### 13.3 Test Coverage
- Minimum 80% code coverage
- 100% coverage for business rules
- 100% coverage for inventory operations

---

## 14. Appendices

### 14.1 Related Documents
- [Frontend Specification](./frontend-spec.md)
- [Use Case Diagram](./diagrams/use-case.drawio)
- [C4 Diagrams](./diagrams/c4-context.drawio)
- [Class Diagram](./plantuml/class-diagram.plantuml)
- [Sequence Diagrams](./plantuml/sequence-diagram.plantuml)

### 14.2 Revision History
| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-22 | System | Initial specification |
