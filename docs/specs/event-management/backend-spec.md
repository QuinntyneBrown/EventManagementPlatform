# Event Management - Backend Specification

## Document Information
| Field | Value |
|-------|-------|
| Feature | Event Management |
| Version | 1.0 |
| Date | 2025-12-22 |
| Status | Final |

---

## 1. Overview

### 1.1 Purpose
The Event Management module is the core component of the EventManagementPlatform system, responsible for managing the complete lifecycle of events from creation to archival.

### 1.2 Scope
This specification covers all backend requirements for event creation, modification, status management, and event-related notes and communications.

### 1.3 Technology Stack
- **Framework**: .NET 8.0
- **Database**: SQL Server Express (via Entity Framework Core)
- **Cloud**: Azure App Service, Azure SQL Database
- **AI Integration**: Azure AI Services for intelligent event recommendations
- **Messaging**: MediatR for CQRS pattern implementation

---

## 2. Domain Model

### 2.1 Aggregate: Event
The Event aggregate is the central entity in this bounded context.

#### 2.1.1 Entity Properties
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| EventId | Guid | Yes | Unique identifier |
| Title | string | Yes | Event title (max 200 chars) |
| Description | string | No | Event description (max 2000 chars) |
| EventDate | DateTime | Yes | Scheduled date and time |
| VenueId | Guid | Yes | Reference to Venue aggregate |
| EventTypeId | Guid | Yes | Reference to EventType |
| Status | EventStatus | Yes | Current lifecycle status |
| CustomerId | Guid | Yes | Reference to Customer aggregate |
| CreatedAt | DateTime | Yes | Creation timestamp |
| ModifiedAt | DateTime | No | Last modification timestamp |
| CreatedBy | Guid | Yes | User who created the event |
| ModifiedBy | Guid | No | User who last modified |

#### 2.1.2 EventStatus Enumeration
```csharp
public enum EventStatus
{
    Draft = 0,
    PendingApproval = 1,
    Approved = 2,
    Confirmed = 3,
    InProgress = 4,
    Completed = 5,
    Cancelled = 6,
    Archived = 7
}
```

### 2.2 Entity: EventNote
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| EventNoteId | Guid | Yes | Unique identifier |
| EventId | Guid | Yes | Parent event reference |
| Content | string | Yes | Note content (max 4000 chars) |
| NoteType | NoteType | Yes | Type of note |
| CreatedAt | DateTime | Yes | Creation timestamp |
| CreatedBy | Guid | Yes | User who created the note |

### 2.3 Entity: EventType
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| EventTypeId | Guid | Yes | Unique identifier |
| Name | string | Yes | Type name |
| Description | string | No | Type description |
| IsActive | bool | Yes | Whether type is active |

---

## 3. Domain Events

### 3.1 Event Lifecycle Events
| Event | Trigger | Payload |
|-------|---------|---------|
| EventCreated | New event registered | EventId, Title, CustomerId, EventDate |
| EventDetailsUpdated | Event info modified | EventId, ChangedProperties |
| EventDateChanged | Date/time rescheduled | EventId, OldDate, NewDate |
| EventVenueChanged | Location updated | EventId, OldVenueId, NewVenueId |
| EventTypeChanged | Type modified | EventId, OldTypeId, NewTypeId |
| EventCancelled | Event cancelled | EventId, CancellationReason, CancelledBy |
| EventReinstated | Cancelled event restored | EventId, ReinstatedBy |
| EventCompleted | Event marked complete | EventId, CompletedAt |
| EventArchived | Event archived | EventId, ArchivedAt |

### 3.2 Event Status Events
| Event | Trigger | Payload |
|-------|---------|---------|
| EventDraftCreated | Initial draft created | EventId, CustomerId |
| EventConfirmed | Booking confirmed | EventId, ConfirmedBy |
| EventPendingApproval | Submitted for approval | EventId, SubmittedBy |
| EventApproved | Event approved | EventId, ApprovedBy |
| EventRejected | Event rejected | EventId, RejectedBy, Reason |

### 3.3 Event Notes Events
| Event | Trigger | Payload |
|-------|---------|---------|
| EventNoteAdded | Note added | EventNoteId, EventId, Content |
| EventNoteUpdated | Note modified | EventNoteId, OldContent, NewContent |
| EventNoteDeleted | Note removed | EventNoteId, EventId |
| CustomerCommentRecorded | Customer feedback | EventId, Comment, CustomerId |

---

## 4. API Endpoints

### 4.1 Event Management Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/events | List all events with pagination |
| GET | /api/events/{eventId} | Get event by ID |
| POST | /api/events | Create new event |
| PUT | /api/events/{eventId} | Update event details |
| DELETE | /api/events/{eventId} | Soft delete event |
| POST | /api/events/{eventId}/cancel | Cancel event |
| POST | /api/events/{eventId}/reinstate | Reinstate cancelled event |
| POST | /api/events/{eventId}/complete | Mark event as complete |
| POST | /api/events/{eventId}/archive | Archive event |

### 4.2 Event Status Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/events/{eventId}/submit-for-approval | Submit for approval |
| POST | /api/events/{eventId}/approve | Approve event |
| POST | /api/events/{eventId}/reject | Reject event |
| POST | /api/events/{eventId}/confirm | Confirm booking |

### 4.3 Event Notes Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/events/{eventId}/notes | List event notes |
| POST | /api/events/{eventId}/notes | Add note to event |
| PUT | /api/events/{eventId}/notes/{noteId} | Update note |
| DELETE | /api/events/{eventId}/notes/{noteId} | Delete note |

---

## 5. Commands and Queries (CQRS)

### 5.1 Commands
```
EventManagementPlatform.Api/Features/Events/
├── CreateEvent/
│   ├── CreateEventCommand.cs
│   ├── CreateEventCommandHandler.cs
│   └── CreateEventDto.cs
├── UpdateEvent/
│   ├── UpdateEventCommand.cs
│   ├── UpdateEventCommandHandler.cs
│   └── UpdateEventDto.cs
├── CancelEvent/
│   ├── CancelEventCommand.cs
│   └── CancelEventCommandHandler.cs
├── ReinstateEvent/
│   ├── ReinstateEventCommand.cs
│   └── ReinstateEventCommandHandler.cs
├── CompleteEvent/
│   ├── CompleteEventCommand.cs
│   └── CompleteEventCommandHandler.cs
├── ArchiveEvent/
│   ├── ArchiveEventCommand.cs
│   └── ArchiveEventCommandHandler.cs
├── ApproveEvent/
│   ├── ApproveEventCommand.cs
│   └── ApproveEventCommandHandler.cs
├── RejectEvent/
│   ├── RejectEventCommand.cs
│   └── RejectEventCommandHandler.cs
└── AddEventNote/
    ├── AddEventNoteCommand.cs
    ├── AddEventNoteCommandHandler.cs
    └── AddEventNoteDto.cs
```

### 5.2 Queries
```
EventManagementPlatform.Api/Features/Events/
├── GetEvents/
│   ├── GetEventsQuery.cs
│   ├── GetEventsQueryHandler.cs
│   └── EventListDto.cs
├── GetEventById/
│   ├── GetEventByIdQuery.cs
│   ├── GetEventByIdQueryHandler.cs
│   └── EventDetailDto.cs
└── GetEventNotes/
    ├── GetEventNotesQuery.cs
    ├── GetEventNotesQueryHandler.cs
    └── EventNoteDto.cs
```

---

## 6. Business Rules

### 6.1 Event Creation Rules
| Rule ID | Description |
|---------|-------------|
| EVT-001 | Event date must be in the future |
| EVT-002 | Event title is required and must be unique per customer per date |
| EVT-003 | Customer must have an active account |
| EVT-004 | Venue must be available on the selected date |

### 6.2 Event Status Transition Rules
| Rule ID | From Status | To Status | Condition |
|---------|-------------|-----------|-----------|
| EVT-010 | Draft | PendingApproval | All required fields completed |
| EVT-011 | PendingApproval | Approved | Approved by authorized user |
| EVT-012 | PendingApproval | Rejected | Rejected with reason |
| EVT-013 | Approved | Confirmed | Customer confirms booking |
| EVT-014 | Confirmed | Cancelled | Cancellation request |
| EVT-015 | Cancelled | Confirmed | Reinstatement approved |
| EVT-016 | Confirmed | Completed | Event date passed |
| EVT-017 | Completed | Archived | Manual or automatic archival |

### 6.3 Event Cancellation Rules
| Rule ID | Description |
|---------|-------------|
| EVT-020 | Cancelled events cannot be modified |
| EVT-021 | Cancellation within 24 hours requires manager approval |
| EVT-022 | Archived events cannot be cancelled |

---

## 7. Azure Integration

### 7.1 Azure AI Services
| Service | Purpose |
|---------|---------|
| Azure Cognitive Services | Sentiment analysis of customer comments |
| Azure OpenAI | Smart event suggestions and descriptions |
| Azure Anomaly Detector | Detect unusual booking patterns |

### 7.2 Azure Infrastructure
| Service | Purpose |
|---------|---------|
| Azure App Service | Host API application |
| Azure SQL Database | Primary data storage |
| Azure Blob Storage | Event attachments and documents |
| Azure Service Bus | Event publishing for integrations |
| Azure Application Insights | Monitoring and telemetry |

---

## 8. Data Transfer Objects (DTOs)

### 8.1 CreateEventDto
```csharp
public record CreateEventDto(
    string Title,
    string? Description,
    DateTime EventDate,
    Guid VenueId,
    Guid EventTypeId,
    Guid CustomerId
);
```

### 8.2 EventDetailDto
```csharp
public record EventDetailDto(
    Guid EventId,
    string Title,
    string? Description,
    DateTime EventDate,
    Guid VenueId,
    string VenueName,
    Guid EventTypeId,
    string EventTypeName,
    string Status,
    Guid CustomerId,
    string CustomerName,
    DateTime CreatedAt,
    DateTime? ModifiedAt,
    IEnumerable<EventNoteDto> Notes
);
```

### 8.3 EventListDto
```csharp
public record EventListDto(
    Guid EventId,
    string Title,
    DateTime EventDate,
    string VenueName,
    string EventTypeName,
    string Status,
    string CustomerName
);
```

---

## 9. Validation Requirements

### 9.1 Input Validation
| Field | Validation Rules |
|-------|-----------------|
| Title | Required, 1-200 characters |
| Description | Optional, max 2000 characters |
| EventDate | Required, must be future date |
| VenueId | Required, must exist |
| EventTypeId | Required, must exist |
| CustomerId | Required, must exist |

### 9.2 FluentValidation Implementation
```csharp
public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.EventDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Event date must be in the future");

        RuleFor(x => x.VenueId)
            .NotEmpty();

        RuleFor(x => x.EventTypeId)
            .NotEmpty();

        RuleFor(x => x.CustomerId)
            .NotEmpty();
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
| Customer | Create, View own events, Add comments |
| Staff | View assigned events, Update status |
| Manager | All operations, Approve/Reject events |
| Admin | Full access including archive |

---

## 11. Performance Requirements

| Metric | Requirement |
|--------|-------------|
| API Response Time | < 200ms for 95th percentile |
| Event List Query | Support 10,000+ events with pagination |
| Concurrent Users | Support 100 concurrent users |
| Event Creation | < 500ms including validation |

---

## 12. Error Handling

### 12.1 Error Response Format
```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "Bad Request",
    "status": 400,
    "detail": "Event date must be in the future",
    "instance": "/api/events",
    "errors": {
        "EventDate": ["Event date must be in the future"]
    }
}
```

### 12.2 Exception Types
| Exception | HTTP Status | Description |
|-----------|-------------|-------------|
| ValidationException | 400 | Input validation failed |
| NotFoundException | 404 | Event not found |
| ConflictException | 409 | Status transition not allowed |
| UnauthorizedException | 401 | Authentication required |
| ForbiddenException | 403 | Insufficient permissions |

---

## 13. Testing Requirements

### 13.1 Unit Tests
- Test all command handlers
- Test all query handlers
- Test domain event generation
- Test validation rules

### 13.2 Integration Tests
- Test API endpoints
- Test database operations
- Test event publishing

### 13.3 Test Coverage
- Minimum 80% code coverage
- 100% coverage for business rules

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
