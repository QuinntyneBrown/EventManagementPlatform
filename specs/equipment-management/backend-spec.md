# Equipment Management - Backend Specification

## Document Information
| Field | Value |
|-------|-------|
| Feature | Equipment Management |
| Version | 1.0 |
| Date | 2025-12-22 |
| Status | Final |

---

## 1. Overview

### 1.1 Purpose
The Equipment Management module handles the complete lifecycle of equipment items (tables, games, etc.), their booking for events, logistics coordination (delivery/pickup), and maintenance tracking.

### 1.2 Scope
This specification covers all backend requirements for:
- Equipment item management (CRUD, activation, specifications)
- Equipment reservations and booking conflicts
- Logistics tracking (packing, delivery, setup, return)
- Maintenance scheduling and tracking
- Equipment condition monitoring

### 1.3 Technology Stack
- **Framework**: .NET 8.0
- **Database**: SQL Server Express (via Entity Framework Core)
- **Cloud**: Azure App Service, Azure SQL Database
- **AI Integration**: Azure AI Services for predictive maintenance and smart recommendations
- **Messaging**: MediatR for CQRS pattern implementation
- **Storage**: Azure Blob Storage for equipment photos

---

## 2. Domain Model

### 2.1 Aggregate: EquipmentItem
The EquipmentItem aggregate is the central entity managing equipment lifecycle.

#### 2.1.1 Entity Properties
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| EquipmentItemId | Guid | Yes | Unique identifier |
| Name | string | Yes | Equipment name (max 200 chars) |
| Description | string | No | Detailed description (max 2000 chars) |
| Category | EquipmentCategory | Yes | Category (Table, Game, AudioVisual, etc.) |
| Condition | EquipmentCondition | Yes | Current condition |
| Status | EquipmentStatus | Yes | Current status |
| PurchaseDate | DateTime | Yes | Date purchased |
| PurchasePrice | decimal | Yes | Original cost |
| CurrentValue | decimal | No | Estimated current value |
| Manufacturer | string | No | Manufacturer name |
| Model | string | No | Model number/name |
| SerialNumber | string | No | Serial number |
| WarehouseLocation | string | No | Storage location code |
| IsActive | bool | Yes | Whether item is active |
| CreatedAt | DateTime | Yes | Creation timestamp |
| ModifiedAt | DateTime | No | Last modification timestamp |
| CreatedBy | Guid | Yes | User who created |
| ModifiedBy | Guid | No | User who last modified |

#### 2.1.2 EquipmentCategory Enumeration
```csharp
public enum EquipmentCategory
{
    Table = 0,
    Game = 1,
    AudioVisual = 2,
    Decoration = 3,
    Seating = 4,
    Lighting = 5,
    Other = 99
}
```

#### 2.1.3 EquipmentCondition Enumeration
```csharp
public enum EquipmentCondition
{
    Excellent = 0,
    Good = 1,
    Fair = 2,
    Poor = 3,
    NeedsRepair = 4,
    OutOfService = 5
}
```

#### 2.1.4 EquipmentStatus Enumeration
```csharp
public enum EquipmentStatus
{
    Available = 0,
    Reserved = 1,
    InTransit = 2,
    AtVenue = 3,
    InMaintenance = 4,
    Retired = 5
}
```

### 2.2 Entity: EquipmentSpecification
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| SpecificationId | Guid | Yes | Unique identifier |
| EquipmentItemId | Guid | Yes | Parent equipment reference |
| SpecificationKey | string | Yes | Specification name (e.g., "Dimensions", "Weight") |
| SpecificationValue | string | Yes | Specification value |
| Unit | string | No | Measurement unit |
| CreatedAt | DateTime | Yes | Creation timestamp |
| ModifiedAt | DateTime | No | Last modification timestamp |

### 2.3 Entity: EquipmentPhoto
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| PhotoId | Guid | Yes | Unique identifier |
| EquipmentItemId | Guid | Yes | Parent equipment reference |
| BlobUrl | string | Yes | Azure Blob Storage URL |
| ThumbnailUrl | string | No | Thumbnail URL |
| FileName | string | Yes | Original file name |
| FileSize | long | Yes | File size in bytes |
| IsPrimary | bool | Yes | Whether this is primary photo |
| UploadedAt | DateTime | Yes | Upload timestamp |
| UploadedBy | Guid | Yes | User who uploaded |

### 2.4 Entity: EquipmentReservation
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| ReservationId | Guid | Yes | Unique identifier |
| EquipmentItemId | Guid | Yes | Equipment reference |
| EventId | Guid | Yes | Event reference |
| Quantity | int | Yes | Number of items reserved |
| StartDate | DateTime | Yes | Reservation start date/time |
| EndDate | DateTime | Yes | Reservation end date/time |
| Status | ReservationStatus | Yes | Current status |
| Notes | string | No | Special instructions (max 1000 chars) |
| CreatedAt | DateTime | Yes | Creation timestamp |
| ModifiedAt | DateTime | No | Last modification timestamp |
| CreatedBy | Guid | Yes | User who created |
| ModifiedBy | Guid | No | User who last modified |

#### 2.4.1 ReservationStatus Enumeration
```csharp
public enum ReservationStatus
{
    Requested = 0,
    Confirmed = 1,
    Cancelled = 2,
    Fulfilled = 3
}
```

### 2.5 Entity: EquipmentLogistics
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| LogisticsId | Guid | Yes | Unique identifier |
| ReservationId | Guid | Yes | Reservation reference |
| PackedAt | DateTime | No | When packed for shipment |
| LoadedAt | DateTime | No | When loaded on truck |
| DispatchedAt | DateTime | No | When dispatched |
| DeliveredAt | DateTime | No | When delivered to venue |
| SetupCompletedAt | DateTime | No | When setup completed |
| PickedUpAt | DateTime | No | When picked up from venue |
| ReturnedAt | DateTime | No | When returned to warehouse |
| DeliveryTruckId | Guid | No | Truck/vehicle reference |
| DeliveryDriverId | Guid | No | Driver reference |
| Notes | string | No | Logistics notes (max 2000 chars) |

### 2.6 Entity: EquipmentMaintenance
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| MaintenanceId | Guid | Yes | Unique identifier |
| EquipmentItemId | Guid | Yes | Equipment reference |
| MaintenanceType | MaintenanceType | Yes | Type of maintenance |
| ScheduledDate | DateTime | Yes | When scheduled |
| StartedAt | DateTime | No | When started |
| CompletedAt | DateTime | No | When completed |
| Status | MaintenanceStatus | Yes | Current status |
| Description | string | Yes | Maintenance description (max 2000 chars) |
| Cost | decimal | No | Maintenance cost |
| TechnicianId | Guid | No | Assigned technician |
| Notes | string | No | Additional notes (max 2000 chars) |
| CreatedAt | DateTime | Yes | Creation timestamp |
| CreatedBy | Guid | Yes | User who created |

#### 2.6.1 MaintenanceType Enumeration
```csharp
public enum MaintenanceType
{
    Inspection = 0,
    PreventiveMaintenance = 1,
    Repair = 2,
    Cleaning = 3,
    Replacement = 4
}
```

#### 2.6.2 MaintenanceStatus Enumeration
```csharp
public enum MaintenanceStatus
{
    Scheduled = 0,
    InProgress = 1,
    Completed = 2,
    Cancelled = 3
}
```

### 2.7 Entity: EquipmentDamageReport
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| DamageReportId | Guid | Yes | Unique identifier |
| EquipmentItemId | Guid | Yes | Equipment reference |
| EventId | Guid | No | Related event (if applicable) |
| ReportedAt | DateTime | Yes | When damage reported |
| ReportedBy | Guid | Yes | User who reported |
| DamageSeverity | DamageSeverity | Yes | Severity level |
| Description | string | Yes | Damage description (max 2000 chars) |
| PhotoUrls | string | No | Comma-separated photo URLs |
| RepairRequired | bool | Yes | Whether repair needed |
| EstimatedRepairCost | decimal | No | Estimated cost |
| Status | DamageStatus | Yes | Current status |

#### 2.7.1 DamageSeverity Enumeration
```csharp
public enum DamageSeverity
{
    Minor = 0,
    Moderate = 1,
    Severe = 2,
    TotalLoss = 3
}
```

#### 2.7.2 DamageStatus Enumeration
```csharp
public enum DamageStatus
{
    Reported = 0,
    UnderReview = 1,
    Approved = 2,
    RepairScheduled = 3,
    Repaired = 4,
    WriteOff = 5
}
```

---

## 3. Domain Events

### 3.1 Equipment Item Management Events
| Event | Trigger | Payload |
|-------|---------|---------|
| EquipmentItemAdded | New equipment registered | EquipmentItemId, Name, Category |
| EquipmentItemUpdated | Equipment info modified | EquipmentItemId, ChangedProperties |
| EquipmentItemRemoved | Equipment soft-deleted | EquipmentItemId, RemovedBy |
| EquipmentItemActivated | Equipment activated | EquipmentItemId, ActivatedBy |
| EquipmentItemDeactivated | Equipment deactivated | EquipmentItemId, DeactivatedBy |
| EquipmentPhotoUploaded | Photo added | EquipmentItemId, PhotoId, BlobUrl |
| EquipmentSpecificationsUpdated | Specs modified | EquipmentItemId, Specifications |

### 3.2 Reservation Events
| Event | Trigger | Payload |
|-------|---------|---------|
| EquipmentBookingRequested | Reservation request | ReservationId, EquipmentItemId, EventId |
| EquipmentReservedForEvent | Reservation confirmed | ReservationId, EquipmentItemId, EventId |
| EquipmentReservationUpdated | Reservation modified | ReservationId, ChangedProperties |
| EquipmentReservationCancelled | Reservation cancelled | ReservationId, CancellationReason |
| EquipmentDoubleBookingDetected | Conflict found | EquipmentItemId, ConflictingReservations |
| EquipmentDoubleBookingPrevented | Conflict blocked | EquipmentItemId, RequestedPeriod |
| EquipmentDoubleBookingOverridden | Conflict overridden | ReservationId, OverriddenBy, Reason |
| EquipmentAvailabilityChecked | Availability query | EquipmentItemId, DateRange |
| EquipmentAlternativeSuggested | Alternative offered | OriginalEquipmentId, SuggestedEquipmentIds |

### 3.3 Logistics Events
| Event | Trigger | Payload |
|-------|---------|---------|
| EquipmentPackedForShipment | Packing completed | LogisticsId, EquipmentItemId, PackedBy |
| EquipmentLoadedOnTruck | Loaded on vehicle | LogisticsId, TruckId, LoadedBy |
| EquipmentDispatchedToEvent | Dispatched | LogisticsId, DispatchedAt, DriverId |
| EquipmentDeliveredToVenue | Delivered | LogisticsId, VenueId, DeliveredAt |
| EquipmentSetupCompleted | Setup finished | LogisticsId, SetupCompletedAt |
| EquipmentPickedUpFromVenue | Picked up | LogisticsId, PickedUpAt |
| EquipmentReturnedToWarehouse | Returned | LogisticsId, ReturnedAt |

### 3.4 Maintenance Events
| Event | Trigger | Payload |
|-------|---------|---------|
| EquipmentMaintenanceScheduled | Maintenance planned | MaintenanceId, EquipmentItemId, ScheduledDate |
| EquipmentMaintenanceStarted | Maintenance begun | MaintenanceId, StartedAt, TechnicianId |
| EquipmentMaintenanceCompleted | Maintenance finished | MaintenanceId, CompletedAt, Cost |
| EquipmentInspectionPassed | Inspection successful | MaintenanceId, EquipmentItemId |
| EquipmentInspectionFailed | Inspection issues found | MaintenanceId, Issues |
| EquipmentDamageReported | Damage documented | DamageReportId, EquipmentItemId, Severity |
| EquipmentRepairRequested | Repair needed | MaintenanceId, EquipmentItemId, Description |
| EquipmentRepairCompleted | Repair finished | MaintenanceId, Cost |
| EquipmentReplacementRequired | Replacement needed | EquipmentItemId, Reason |
| EquipmentConditionDowngraded | Condition worsened | EquipmentItemId, OldCondition, NewCondition |
| EquipmentConditionUpgraded | Condition improved | EquipmentItemId, OldCondition, NewCondition |
| EquipmentRetired | Equipment retired | EquipmentItemId, RetiredBy, Reason |

---

## 4. API Endpoints

### 4.1 Equipment Item Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/equipment | List all equipment with pagination |
| GET | /api/equipment/{equipmentId} | Get equipment by ID |
| POST | /api/equipment | Create new equipment item |
| PUT | /api/equipment/{equipmentId} | Update equipment details |
| DELETE | /api/equipment/{equipmentId} | Soft delete equipment |
| POST | /api/equipment/{equipmentId}/activate | Activate equipment |
| POST | /api/equipment/{equipmentId}/deactivate | Deactivate equipment |
| POST | /api/equipment/{equipmentId}/photos | Upload equipment photo |
| DELETE | /api/equipment/{equipmentId}/photos/{photoId} | Delete photo |
| PUT | /api/equipment/{equipmentId}/specifications | Update specifications |
| GET | /api/equipment/categories/{category} | Get equipment by category |

### 4.2 Reservation Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/equipment/reservations | List all reservations |
| GET | /api/equipment/{equipmentId}/reservations | Get reservations for equipment |
| GET | /api/events/{eventId}/equipment-reservations | Get reservations for event |
| POST | /api/equipment/reservations | Create reservation |
| PUT | /api/equipment/reservations/{reservationId} | Update reservation |
| DELETE | /api/equipment/reservations/{reservationId} | Cancel reservation |
| POST | /api/equipment/check-availability | Check equipment availability |
| POST | /api/equipment/suggest-alternatives | Get alternative suggestions |
| POST | /api/equipment/reservations/{reservationId}/override-conflict | Override double booking |

### 4.3 Logistics Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/equipment/logistics/{reservationId} | Get logistics for reservation |
| POST | /api/equipment/logistics/{reservationId}/pack | Mark as packed |
| POST | /api/equipment/logistics/{reservationId}/load | Mark as loaded |
| POST | /api/equipment/logistics/{reservationId}/dispatch | Mark as dispatched |
| POST | /api/equipment/logistics/{reservationId}/deliver | Mark as delivered |
| POST | /api/equipment/logistics/{reservationId}/setup-complete | Mark setup complete |
| POST | /api/equipment/logistics/{reservationId}/pickup | Mark as picked up |
| POST | /api/equipment/logistics/{reservationId}/return | Mark as returned |
| GET | /api/equipment/logistics/in-transit | Get equipment in transit |

### 4.4 Maintenance Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/equipment/{equipmentId}/maintenance | Get maintenance history |
| POST | /api/equipment/{equipmentId}/maintenance | Schedule maintenance |
| PUT | /api/equipment/maintenance/{maintenanceId} | Update maintenance record |
| POST | /api/equipment/maintenance/{maintenanceId}/start | Start maintenance |
| POST | /api/equipment/maintenance/{maintenanceId}/complete | Complete maintenance |
| POST | /api/equipment/{equipmentId}/damage-report | Report damage |
| GET | /api/equipment/maintenance/scheduled | Get scheduled maintenance |
| POST | /api/equipment/{equipmentId}/retire | Retire equipment |

---

## 5. Commands and Queries (CQRS)

### 5.1 Commands
```
EventManagementPlatform.Api/Features/Equipment/
├── CreateEquipmentItem/
│   ├── CreateEquipmentItemCommand.cs
│   ├── CreateEquipmentItemCommandHandler.cs
│   └── CreateEquipmentItemDto.cs
├── UpdateEquipmentItem/
│   ├── UpdateEquipmentItemCommand.cs
│   ├── UpdateEquipmentItemCommandHandler.cs
│   └── UpdateEquipmentItemDto.cs
├── ActivateEquipmentItem/
│   ├── ActivateEquipmentItemCommand.cs
│   └── ActivateEquipmentItemCommandHandler.cs
├── DeactivateEquipmentItem/
│   ├── DeactivateEquipmentItemCommand.cs
│   └── DeactivateEquipmentItemCommandHandler.cs
├── UploadEquipmentPhoto/
│   ├── UploadEquipmentPhotoCommand.cs
│   ├── UploadEquipmentPhotoCommandHandler.cs
│   └── EquipmentPhotoDto.cs
├── UpdateSpecifications/
│   ├── UpdateSpecificationsCommand.cs
│   └── UpdateSpecificationsCommandHandler.cs
├── CreateReservation/
│   ├── CreateReservationCommand.cs
│   ├── CreateReservationCommandHandler.cs
│   └── CreateReservationDto.cs
├── UpdateReservation/
│   ├── UpdateReservationCommand.cs
│   └── UpdateReservationCommandHandler.cs
├── CancelReservation/
│   ├── CancelReservationCommand.cs
│   └── CancelReservationCommandHandler.cs
├── OverrideDoubleBooking/
│   ├── OverrideDoubleBookingCommand.cs
│   └── OverrideDoubleBookingCommandHandler.cs
├── UpdateLogistics/
│   ├── UpdateLogisticsCommand.cs
│   └── UpdateLogisticsCommandHandler.cs
├── ScheduleMaintenance/
│   ├── ScheduleMaintenanceCommand.cs
│   ├── ScheduleMaintenanceCommandHandler.cs
│   └── ScheduleMaintenanceDto.cs
├── CompleteMaintenance/
│   ├── CompleteMaintenanceCommand.cs
│   └── CompleteMaintenanceCommandHandler.cs
├── ReportDamage/
│   ├── ReportDamageCommand.cs
│   ├── ReportDamageCommandHandler.cs
│   └── DamageReportDto.cs
└── RetireEquipment/
    ├── RetireEquipmentCommand.cs
    └── RetireEquipmentCommandHandler.cs
```

### 5.2 Queries
```
EventManagementPlatform.Api/Features/Equipment/
├── GetEquipmentItems/
│   ├── GetEquipmentItemsQuery.cs
│   ├── GetEquipmentItemsQueryHandler.cs
│   └── EquipmentListDto.cs
├── GetEquipmentById/
│   ├── GetEquipmentByIdQuery.cs
│   ├── GetEquipmentByIdQueryHandler.cs
│   └── EquipmentDetailDto.cs
├── GetReservations/
│   ├── GetReservationsQuery.cs
│   ├── GetReservationsQueryHandler.cs
│   └── ReservationListDto.cs
├── CheckAvailability/
│   ├── CheckAvailabilityQuery.cs
│   ├── CheckAvailabilityQueryHandler.cs
│   └── AvailabilityResultDto.cs
├── GetAlternatives/
│   ├── GetAlternativesQuery.cs
│   ├── GetAlternativesQueryHandler.cs
│   └── AlternativeEquipmentDto.cs
├── GetLogistics/
│   ├── GetLogisticsQuery.cs
│   ├── GetLogisticsQueryHandler.cs
│   └── LogisticsDto.cs
└── GetMaintenanceHistory/
    ├── GetMaintenanceHistoryQuery.cs
    ├── GetMaintenanceHistoryQueryHandler.cs
    └── MaintenanceRecordDto.cs
```

---

## 6. Business Rules

### 6.1 Equipment Item Rules
| Rule ID | Description |
|---------|-------------|
| EQP-001 | Equipment name must be unique within category |
| EQP-002 | Equipment cannot be deleted if active reservations exist |
| EQP-003 | Purchase date cannot be in the future |
| EQP-004 | Current value cannot exceed purchase price by more than 20% |
| EQP-005 | Deactivated equipment cannot be reserved |
| EQP-006 | Equipment in "OutOfService" condition cannot be reserved |

### 6.2 Reservation Rules
| Rule ID | Description |
|---------|-------------|
| EQP-010 | Reservation start date must be before end date |
| EQP-011 | Reservation period must be at least 1 hour |
| EQP-012 | Reservation cannot overlap with existing confirmed reservations (unless overridden) |
| EQP-013 | Equipment must be "Available" or "Reserved" status to create reservation |
| EQP-014 | Manager approval required to override double booking |
| EQP-015 | Cancellation within 48 hours of event requires manager approval |

### 6.3 Logistics Rules
| Rule ID | Description |
|---------|-------------|
| EQP-020 | Equipment must be packed before loading |
| EQP-021 | Equipment must be loaded before dispatch |
| EQP-022 | Equipment must be dispatched before delivery |
| EQP-023 | Equipment must be delivered before setup completion |
| EQP-024 | Equipment must be picked up before return |
| EQP-025 | Logistics steps cannot be reversed |

### 6.4 Maintenance Rules
| Rule ID | Description |
|---------|-------------|
| EQP-030 | Preventive maintenance required every 6 months |
| EQP-031 | Equipment with "NeedsRepair" condition cannot be reserved |
| EQP-032 | Inspection required after every event usage |
| EQP-033 | Equipment status set to "InMaintenance" when maintenance starts |
| EQP-034 | Damage severity "TotalLoss" triggers automatic retirement |
| EQP-035 | Repair cost exceeding 50% of current value suggests replacement |

---

## 7. Azure Integration

### 7.1 Azure AI Services
| Service | Purpose |
|---------|---------|
| Azure Computer Vision | Analyze equipment photos for damage detection |
| Azure Machine Learning | Predictive maintenance based on usage patterns |
| Azure Cognitive Search | Intelligent equipment search and recommendations |
| Azure OpenAI | Smart alternative equipment suggestions |

### 7.2 Azure Infrastructure
| Service | Purpose |
|---------|---------|
| Azure App Service | Host API application |
| Azure SQL Database | Primary data storage |
| Azure Blob Storage | Equipment photos and documents |
| Azure Service Bus | Event publishing for integrations |
| Azure Application Insights | Monitoring and telemetry |
| Azure Key Vault | Secure configuration and secrets |

---

## 8. Data Transfer Objects (DTOs)

### 8.1 CreateEquipmentItemDto
```csharp
public record CreateEquipmentItemDto(
    string Name,
    string? Description,
    EquipmentCategory Category,
    DateTime PurchaseDate,
    decimal PurchasePrice,
    string? Manufacturer,
    string? Model,
    string? SerialNumber,
    string? WarehouseLocation
);
```

### 8.2 EquipmentDetailDto
```csharp
public record EquipmentDetailDto(
    Guid EquipmentItemId,
    string Name,
    string? Description,
    string Category,
    string Condition,
    string Status,
    DateTime PurchaseDate,
    decimal PurchasePrice,
    decimal? CurrentValue,
    string? Manufacturer,
    string? Model,
    string? SerialNumber,
    string? WarehouseLocation,
    bool IsActive,
    IEnumerable<EquipmentPhotoDto> Photos,
    IEnumerable<EquipmentSpecificationDto> Specifications,
    DateTime CreatedAt,
    DateTime? ModifiedAt
);
```

### 8.3 CreateReservationDto
```csharp
public record CreateReservationDto(
    Guid EquipmentItemId,
    Guid EventId,
    int Quantity,
    DateTime StartDate,
    DateTime EndDate,
    string? Notes
);
```

### 8.4 AvailabilityResultDto
```csharp
public record AvailabilityResultDto(
    Guid EquipmentItemId,
    bool IsAvailable,
    IEnumerable<ConflictingReservationDto> Conflicts,
    IEnumerable<AlternativeEquipmentDto> Alternatives
);
```

### 8.5 LogisticsDto
```csharp
public record LogisticsDto(
    Guid LogisticsId,
    Guid ReservationId,
    DateTime? PackedAt,
    DateTime? LoadedAt,
    DateTime? DispatchedAt,
    DateTime? DeliveredAt,
    DateTime? SetupCompletedAt,
    DateTime? PickedUpAt,
    DateTime? ReturnedAt,
    string CurrentStage,
    string? Notes
);
```

### 8.6 DamageReportDto
```csharp
public record DamageReportDto(
    Guid DamageReportId,
    Guid EquipmentItemId,
    string EquipmentName,
    Guid? EventId,
    DateTime ReportedAt,
    string ReportedByName,
    string Severity,
    string Description,
    IEnumerable<string> PhotoUrls,
    bool RepairRequired,
    decimal? EstimatedRepairCost,
    string Status
);
```

---

## 9. Validation Requirements

### 9.1 Input Validation
| Field | Validation Rules |
|-------|-----------------|
| Name | Required, 1-200 characters, unique per category |
| Category | Required, valid enum value |
| PurchaseDate | Required, not future date |
| PurchasePrice | Required, > 0 |
| StartDate | Required, before EndDate |
| EndDate | Required, after StartDate |
| Quantity | Required, > 0 |

### 9.2 FluentValidation Implementation
```csharp
public class CreateEquipmentItemCommandValidator : AbstractValidator<CreateEquipmentItemCommand>
{
    public CreateEquipmentItemCommandValidator(IEquipmentRepository repository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(async (cmd, name, cancellation) =>
                !await repository.NameExistsInCategoryAsync(name, cmd.Category, cancellation))
            .WithMessage("Equipment name must be unique within category");

        RuleFor(x => x.PurchaseDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Purchase date cannot be in the future");

        RuleFor(x => x.PurchasePrice)
            .GreaterThan(0)
            .WithMessage("Purchase price must be greater than zero");

        RuleFor(x => x.Category)
            .IsInEnum()
            .WithMessage("Invalid equipment category");
    }
}
```

```csharp
public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator(IReservationService reservationService)
    {
        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage("Start date must be before end date");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate.AddHours(1))
            .WithMessage("Reservation must be at least 1 hour");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero");

        RuleFor(x => x)
            .MustAsync(async (cmd, cancellation) =>
                await reservationService.CheckAvailabilityAsync(
                    cmd.EquipmentItemId, cmd.StartDate, cmd.EndDate, cancellation))
            .WithMessage("Equipment not available for requested period");
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
| Viewer | View equipment, reservations |
| Staff | Create reservations, update logistics |
| Warehouse Manager | Manage equipment, upload photos, report damage |
| Maintenance Technician | Manage maintenance records |
| Manager | Override double bookings, approve cancellations, retire equipment |
| Admin | Full access including deletion |

---

## 11. Performance Requirements

| Metric | Requirement |
|--------|-------------|
| API Response Time | < 200ms for 95th percentile |
| Equipment Search | < 100ms for 10,000+ items |
| Availability Check | < 150ms including conflict detection |
| Photo Upload | < 5s for files up to 10MB |
| Concurrent Users | Support 100 concurrent users |

---

## 12. Error Handling

### 12.1 Error Response Format
```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "Bad Request",
    "status": 400,
    "detail": "Equipment not available for requested period",
    "instance": "/api/equipment/reservations",
    "errors": {
        "Availability": ["Equipment already reserved from 2025-12-25 to 2025-12-26"]
    }
}
```

### 12.2 Exception Types
| Exception | HTTP Status | Description |
|-----------|-------------|-------------|
| ValidationException | 400 | Input validation failed |
| NotFoundException | 404 | Equipment/Reservation not found |
| ConflictException | 409 | Double booking or status conflict |
| UnauthorizedException | 401 | Authentication required |
| ForbiddenException | 403 | Insufficient permissions |
| StorageException | 500 | Azure Blob Storage error |

---

## 13. Testing Requirements

### 13.1 Unit Tests
- Test all command handlers
- Test all query handlers
- Test domain event generation
- Test validation rules
- Test business rule enforcement
- Test availability checking logic

### 13.2 Integration Tests
- Test API endpoints
- Test database operations
- Test Azure Blob Storage integration
- Test event publishing
- Test double booking prevention

### 13.3 Test Coverage
- Minimum 80% code coverage
- 100% coverage for business rules
- 100% coverage for availability checking

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
