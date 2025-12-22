# Invitation Management - Backend Specification

## Document Information
| Field | Value |
|-------|-------|
| Feature | Invitation Management |
| Version | 1.0 |
| Date | 2025-12-22 |
| Status | Final |

---

## 1. Overview

### 1.1 Purpose
The Invitation Management module handles the complete lifecycle of event invitations including ordering, design selection and approval, printing, quality control, and delivery tracking.

### 1.2 Scope
This specification covers all backend requirements for invitation orders, design management, printing coordination, quality control, and delivery tracking.

### 1.3 Technology Stack
- **Framework**: .NET 8.0
- **Database**: SQL Server Express (via Entity Framework Core)
- **Cloud**: Azure App Service, Azure SQL Database
- **AI Integration**: Azure AI Services for design generation and text suggestions
- **Messaging**: MediatR for CQRS pattern implementation
- **Integration**: Azure Service Bus for printer and shipper coordination

---

## 2. Domain Model

### 2.1 Aggregate: InvitationOrder
The InvitationOrder aggregate is the central entity in this bounded context.

#### 2.1.1 Entity Properties
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| InvitationOrderId | Guid | Yes | Unique identifier |
| EventId | Guid | Yes | Reference to Event aggregate |
| CustomerId | Guid | Yes | Reference to Customer aggregate |
| OrderNumber | string | Yes | Human-readable order number |
| OrderDate | DateTime | Yes | When order was created |
| DesignType | DesignType | Yes | Template or Custom |
| TemplateId | Guid | No | Reference to template (if selected) |
| CustomDesignUrl | string | No | URL to custom design file |
| InvitationText | string | Yes | Text content for invitation |
| Quantity | int | Yes | Number of invitations to print |
| Status | InvitationOrderStatus | Yes | Current lifecycle status |
| DesignApprovedAt | DateTime | No | When design was approved |
| DesignApprovedBy | Guid | No | User who approved design |
| PrinterId | Guid | No | Assigned printer |
| PrintJobId | Guid | No | Reference to print job |
| ShipperId | Guid | No | Assigned shipper |
| TrackingNumber | string | No | Delivery tracking number |
| CreatedAt | DateTime | Yes | Creation timestamp |
| ModifiedAt | DateTime | No | Last modification timestamp |
| CreatedBy | Guid | Yes | User who created the order |
| ModifiedBy | Guid | No | User who last modified |

#### 2.1.2 InvitationOrderStatus Enumeration
```csharp
public enum InvitationOrderStatus
{
    Draft = 0,
    PendingDesignApproval = 1,
    DesignApproved = 2,
    DesignRejected = 3,
    PrintQueued = 4,
    Printing = 5,
    QualityCheck = 6,
    QualityCheckPassed = 7,
    QualityCheckFailed = 8,
    Packaging = 9,
    Shipped = 10,
    Delivered = 11,
    Cancelled = 12
}
```

#### 2.1.3 DesignType Enumeration
```csharp
public enum DesignType
{
    Template = 0,
    Custom = 1
}
```

### 2.2 Entity: InvitationTemplate
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| InvitationTemplateId | Guid | Yes | Unique identifier |
| Name | string | Yes | Template name |
| Description | string | No | Template description |
| PreviewImageUrl | string | Yes | URL to preview image |
| DesignFileUrl | string | Yes | URL to design file |
| Category | string | No | Template category (wedding, corporate, etc.) |
| IsActive | bool | Yes | Whether template is available |
| CreatedAt | DateTime | Yes | Creation timestamp |

### 2.3 Entity: InvitationPrintJob
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| InvitationPrintJobId | Guid | Yes | Unique identifier |
| InvitationOrderId | Guid | Yes | Parent order reference |
| PrinterId | Guid | Yes | Assigned printer |
| StartedAt | DateTime | No | When printing started |
| CompletedAt | DateTime | No | When printing completed |
| Status | PrintJobStatus | Yes | Current status |
| Notes | string | No | Printer notes |

### 2.4 Entity: InvitationQualityCheck
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| InvitationQualityCheckId | Guid | Yes | Unique identifier |
| InvitationPrintJobId | Guid | Yes | Print job reference |
| CheckedBy | Guid | Yes | User who performed check |
| CheckedAt | DateTime | Yes | When check was performed |
| Passed | bool | Yes | Whether check passed |
| Issues | string | No | Issues found (if failed) |
| Notes | string | No | Additional notes |

### 2.5 Entity: InvitationDelivery
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| InvitationDeliveryId | Guid | Yes | Unique identifier |
| InvitationOrderId | Guid | Yes | Parent order reference |
| ShipperId | Guid | Yes | Assigned shipper |
| TrackingNumber | string | Yes | Tracking number |
| ShippedAt | DateTime | No | When package shipped |
| DeliveredAt | DateTime | No | When package delivered |
| DeliveryAddress | string | Yes | Delivery address |
| Status | DeliveryStatus | Yes | Current status |

---

## 3. Domain Events

### 3.1 Invitation Order Events
| Event | Trigger | Payload |
|-------|---------|---------|
| InvitationOrderCreated | New order placed | InvitationOrderId, EventId, CustomerId, Quantity |
| InvitationOrderUpdated | Order details modified | InvitationOrderId, ChangedProperties |
| InvitationOrderCancelled | Order cancelled | InvitationOrderId, CancellationReason |

### 3.2 Design Events
| Event | Trigger | Payload |
|-------|---------|---------|
| InvitationTemplateSelected | Customer selects template | InvitationOrderId, TemplateId |
| CustomInvitationDesignRequested | Custom design requested | InvitationOrderId, DesignUrl |
| InvitationDesignApproved | Design approved by customer | InvitationOrderId, ApprovedBy |
| InvitationDesignRejected | Design rejected | InvitationOrderId, RejectedBy, Reason |
| InvitationDesignRevisionRequested | Revisions needed | InvitationOrderId, RevisionNotes |
| InvitationTextCustomized | Text content updated | InvitationOrderId, NewText |

### 3.3 Printing Events
| Event | Trigger | Payload |
|-------|---------|---------|
| InvitationPrintJobCreated | Print job created | PrintJobId, InvitationOrderId, PrinterId |
| InvitationPrintJobCompleted | Printing finished | PrintJobId, CompletedAt |
| InvitationQualityCheckPassed | QC passed | QualityCheckId, CheckedBy |
| InvitationQualityCheckFailed | QC failed | QualityCheckId, Issues |

### 3.4 Delivery Events
| Event | Trigger | Payload |
|-------|---------|---------|
| InvitationPackageShipped | Package shipped | DeliveryId, TrackingNumber, ShipperId |
| InvitationPackageDelivered | Package delivered | DeliveryId, DeliveredAt |

### 3.5 Other Events
| Event | Trigger | Payload |
|-------|---------|---------|
| InvitationQuantityChanged | Quantity updated | InvitationOrderId, OldQuantity, NewQuantity |
| InvitationFreePromoApplied | Free promo code applied | InvitationOrderId, PromoCode |
| InvitationDigitalVersionGenerated | Digital copy created | InvitationOrderId, DigitalFileUrl |

---

## 4. API Endpoints

### 4.1 Invitation Order Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/invitations/orders | List all invitation orders with pagination |
| GET | /api/invitations/orders/{orderId} | Get order by ID |
| POST | /api/invitations/orders | Create new invitation order |
| PUT | /api/invitations/orders/{orderId} | Update order details |
| DELETE | /api/invitations/orders/{orderId} | Cancel order |
| POST | /api/invitations/orders/{orderId}/approve-design | Approve design |
| POST | /api/invitations/orders/{orderId}/reject-design | Reject design |
| POST | /api/invitations/orders/{orderId}/request-revision | Request design revision |
| PUT | /api/invitations/orders/{orderId}/text | Update invitation text |
| PUT | /api/invitations/orders/{orderId}/quantity | Update quantity |

### 4.2 Template Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/invitations/templates | List available templates |
| GET | /api/invitations/templates/{templateId} | Get template by ID |
| POST | /api/invitations/templates | Create new template (admin) |
| PUT | /api/invitations/templates/{templateId} | Update template (admin) |
| DELETE | /api/invitations/templates/{templateId} | Deactivate template (admin) |

### 4.3 Print Job Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/invitations/print-jobs | List print jobs |
| GET | /api/invitations/print-jobs/{jobId} | Get print job by ID |
| POST | /api/invitations/print-jobs | Create print job |
| POST | /api/invitations/print-jobs/{jobId}/start | Start printing |
| POST | /api/invitations/print-jobs/{jobId}/complete | Mark print job complete |
| POST | /api/invitations/print-jobs/{jobId}/quality-check | Record quality check |

### 4.4 Delivery Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/invitations/deliveries/{deliveryId} | Get delivery by ID |
| POST | /api/invitations/deliveries | Create delivery record |
| PUT | /api/invitations/deliveries/{deliveryId}/ship | Mark as shipped |
| PUT | /api/invitations/deliveries/{deliveryId}/deliver | Mark as delivered |
| GET | /api/invitations/deliveries/{deliveryId}/tracking | Get tracking info |

---

## 5. Commands and Queries (CQRS)

### 5.1 Commands
```
EventManagementPlatform.Api/Features/Invitations/
├── CreateInvitationOrder/
│   ├── CreateInvitationOrderCommand.cs
│   ├── CreateInvitationOrderCommandHandler.cs
│   └── CreateInvitationOrderDto.cs
├── UpdateInvitationOrder/
│   ├── UpdateInvitationOrderCommand.cs
│   ├── UpdateInvitationOrderCommandHandler.cs
│   └── UpdateInvitationOrderDto.cs
├── CancelInvitationOrder/
│   ├── CancelInvitationOrderCommand.cs
│   └── CancelInvitationOrderCommandHandler.cs
├── ApproveDesign/
│   ├── ApproveDesignCommand.cs
│   └── ApproveDesignCommandHandler.cs
├── RejectDesign/
│   ├── RejectDesignCommand.cs
│   └── RejectDesignCommandHandler.cs
├── RequestDesignRevision/
│   ├── RequestDesignRevisionCommand.cs
│   └── RequestDesignRevisionCommandHandler.cs
├── UpdateInvitationText/
│   ├── UpdateInvitationTextCommand.cs
│   └── UpdateInvitationTextCommandHandler.cs
├── UpdateInvitationQuantity/
│   ├── UpdateInvitationQuantityCommand.cs
│   └── UpdateInvitationQuantityCommandHandler.cs
├── CreatePrintJob/
│   ├── CreatePrintJobCommand.cs
│   └── CreatePrintJobCommandHandler.cs
├── CompletePrintJob/
│   ├── CompletePrintJobCommand.cs
│   └── CompletePrintJobCommandHandler.cs
├── RecordQualityCheck/
│   ├── RecordQualityCheckCommand.cs
│   └── RecordQualityCheckCommandHandler.cs
├── CreateDelivery/
│   ├── CreateDeliveryCommand.cs
│   └── CreateDeliveryCommandHandler.cs
├── MarkAsShipped/
│   ├── MarkAsShippedCommand.cs
│   └── MarkAsShippedCommandHandler.cs
└── MarkAsDelivered/
    ├── MarkAsDeliveredCommand.cs
    └── MarkAsDeliveredCommandHandler.cs
```

### 5.2 Queries
```
EventManagementPlatform.Api/Features/Invitations/
├── GetInvitationOrders/
│   ├── GetInvitationOrdersQuery.cs
│   ├── GetInvitationOrdersQueryHandler.cs
│   └── InvitationOrderListDto.cs
├── GetInvitationOrderById/
│   ├── GetInvitationOrderByIdQuery.cs
│   ├── GetInvitationOrderByIdQueryHandler.cs
│   └── InvitationOrderDetailDto.cs
├── GetInvitationTemplates/
│   ├── GetInvitationTemplatesQuery.cs
│   ├── GetInvitationTemplatesQueryHandler.cs
│   └── InvitationTemplateDto.cs
├── GetPrintJobs/
│   ├── GetPrintJobsQuery.cs
│   ├── GetPrintJobsQueryHandler.cs
│   └── PrintJobDto.cs
└── GetDeliveryTracking/
    ├── GetDeliveryTrackingQuery.cs
    ├── GetDeliveryTrackingQueryHandler.cs
    └── DeliveryTrackingDto.cs
```

---

## 6. Business Rules

### 6.1 Order Creation Rules
| Rule ID | Description |
|---------|-------------|
| INV-001 | Invitation orders can only be created for confirmed events |
| INV-002 | Quantity must be between 10 and 10,000 |
| INV-003 | Event must be at least 14 days in future |
| INV-004 | Customer must have valid payment method on file |

### 6.2 Design Approval Rules
| Rule ID | Description |
|---------|-------------|
| INV-010 | Only order creator or customer can approve/reject design |
| INV-011 | Design must be approved before printing can start |
| INV-012 | After 3 rejections, order requires manual review |
| INV-013 | Custom designs must be uploaded in PDF or AI format |

### 6.3 Printing Rules
| Rule ID | Description |
|---------|-------------|
| INV-020 | Print job can only start after design approval |
| INV-021 | Quality check required for orders over 500 invitations |
| INV-022 | Failed quality check requires reprinting |
| INV-023 | Print jobs must be assigned to active printers |

### 6.4 Delivery Rules
| Rule ID | Description |
|---------|-------------|
| INV-030 | Delivery can only start after QC passes |
| INV-031 | Tracking number must be provided when marking as shipped |
| INV-032 | Express delivery available for orders under 200 invitations |
| INV-033 | International delivery requires additional 7-14 days |

### 6.5 Cancellation Rules
| Rule ID | Description |
|---------|-------------|
| INV-040 | Orders in Draft or PendingDesignApproval can be cancelled freely |
| INV-041 | Orders in printing cannot be cancelled |
| INV-042 | Cancellation after design approval may incur fees |
| INV-043 | Shipped orders cannot be cancelled |

---

## 7. Azure Integration

### 7.1 Azure AI Services
| Service | Purpose |
|---------|---------|
| Azure OpenAI | Generate invitation text suggestions |
| Azure Computer Vision | Analyze custom design uploads for quality |
| Azure Form Recognizer | Extract text from uploaded designs |
| Azure Cognitive Search | Search templates by style, theme, color |

### 7.2 Azure Infrastructure
| Service | Purpose |
|---------|---------|
| Azure App Service | Host API application |
| Azure SQL Database | Primary data storage |
| Azure Blob Storage | Store invitation designs and templates |
| Azure Service Bus | Coordinate with printers and shippers |
| Azure Application Insights | Monitoring and telemetry |
| Azure CDN | Serve template previews |

---

## 8. Data Transfer Objects (DTOs)

### 8.1 CreateInvitationOrderDto
```csharp
public record CreateInvitationOrderDto(
    Guid EventId,
    Guid CustomerId,
    DesignType DesignType,
    Guid? TemplateId,
    string? CustomDesignUrl,
    string InvitationText,
    int Quantity
);
```

### 8.2 InvitationOrderDetailDto
```csharp
public record InvitationOrderDetailDto(
    Guid InvitationOrderId,
    string OrderNumber,
    Guid EventId,
    string EventTitle,
    Guid CustomerId,
    string CustomerName,
    DateTime OrderDate,
    DesignType DesignType,
    Guid? TemplateId,
    string? TemplateName,
    string? CustomDesignUrl,
    string InvitationText,
    int Quantity,
    InvitationOrderStatus Status,
    DateTime? DesignApprovedAt,
    string? PrinterName,
    string? TrackingNumber,
    DateTime CreatedAt,
    DateTime? ModifiedAt
);
```

### 8.3 InvitationOrderListDto
```csharp
public record InvitationOrderListDto(
    Guid InvitationOrderId,
    string OrderNumber,
    string EventTitle,
    string CustomerName,
    int Quantity,
    InvitationOrderStatus Status,
    DateTime OrderDate
);
```

### 8.4 InvitationTemplateDto
```csharp
public record InvitationTemplateDto(
    Guid InvitationTemplateId,
    string Name,
    string? Description,
    string PreviewImageUrl,
    string Category,
    bool IsActive
);
```

### 8.5 PrintJobDto
```csharp
public record PrintJobDto(
    Guid InvitationPrintJobId,
    Guid InvitationOrderId,
    string OrderNumber,
    Guid PrinterId,
    string PrinterName,
    DateTime? StartedAt,
    DateTime? CompletedAt,
    PrintJobStatus Status,
    string? Notes
);
```

### 8.6 DeliveryTrackingDto
```csharp
public record DeliveryTrackingDto(
    Guid InvitationDeliveryId,
    Guid InvitationOrderId,
    string OrderNumber,
    string TrackingNumber,
    string ShipperName,
    DateTime? ShippedAt,
    DateTime? DeliveredAt,
    string DeliveryAddress,
    DeliveryStatus Status
);
```

---

## 9. Validation Requirements

### 9.1 Input Validation
| Field | Validation Rules |
|-------|-----------------|
| EventId | Required, must exist and be confirmed |
| Quantity | Required, between 10 and 10,000 |
| InvitationText | Required, max 1000 characters |
| DesignType | Required, valid enum value |
| TemplateId | Required if DesignType is Template |
| CustomDesignUrl | Required if DesignType is Custom, valid URL |

### 9.2 FluentValidation Implementation
```csharp
public class CreateInvitationOrderCommandValidator : AbstractValidator<CreateInvitationOrderCommand>
{
    public CreateInvitationOrderCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event ID is required");

        RuleFor(x => x.Quantity)
            .InclusiveBetween(10, 10000)
            .WithMessage("Quantity must be between 10 and 10,000");

        RuleFor(x => x.InvitationText)
            .NotEmpty()
            .MaximumLength(1000)
            .WithMessage("Invitation text is required and must not exceed 1000 characters");

        When(x => x.DesignType == DesignType.Template, () =>
        {
            RuleFor(x => x.TemplateId)
                .NotEmpty()
                .WithMessage("Template ID is required when using template design");
        });

        When(x => x.DesignType == DesignType.Custom, () =>
        {
            RuleFor(x => x.CustomDesignUrl)
                .NotEmpty()
                .Must(BeValidUrl)
                .WithMessage("Custom design URL is required and must be valid");
        });
    }

    private bool BeValidUrl(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
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
| Customer | Create orders, view own orders, approve/reject own designs |
| Staff | View all orders, update order status, assign print jobs |
| Printer | View assigned print jobs, update print status, record QC |
| Manager | All operations, manage templates, cancel any order |
| Admin | Full access including template management |

---

## 11. Performance Requirements

| Metric | Requirement |
|--------|-------------|
| API Response Time | < 200ms for 95th percentile |
| Order List Query | Support 50,000+ orders with pagination |
| Concurrent Users | Support 200 concurrent users |
| Order Creation | < 500ms including validation |
| Design Upload | Support files up to 50MB |
| Template Loading | < 100ms with CDN caching |

---

## 12. Error Handling

### 12.1 Error Response Format
```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "Bad Request",
    "status": 400,
    "detail": "Quantity must be between 10 and 10,000",
    "instance": "/api/invitations/orders",
    "errors": {
        "Quantity": ["Quantity must be between 10 and 10,000"]
    }
}
```

### 12.2 Exception Types
| Exception | HTTP Status | Description |
|-----------|-------------|-------------|
| ValidationException | 400 | Input validation failed |
| NotFoundException | 404 | Order/Template not found |
| ConflictException | 409 | Status transition not allowed |
| UnauthorizedException | 401 | Authentication required |
| ForbiddenException | 403 | Insufficient permissions |
| PaymentRequiredException | 402 | Payment method required |

---

## 13. Testing Requirements

### 13.1 Unit Tests
- Test all command handlers
- Test all query handlers
- Test domain event generation
- Test validation rules
- Test business rule enforcement

### 13.2 Integration Tests
- Test API endpoints
- Test database operations
- Test Azure Blob Storage integration
- Test Service Bus messaging
- Test external printer/shipper integrations

### 13.3 Test Coverage
- Minimum 80% code coverage
- 100% coverage for business rules
- 100% coverage for validation logic

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
