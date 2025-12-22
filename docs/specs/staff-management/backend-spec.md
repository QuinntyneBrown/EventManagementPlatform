# Staff Management - Backend Specification

## Document Information
| Field | Value |
|-------|-------|
| Feature | Staff Management |
| Version | 1.0 |
| Date | 2025-12-22 |
| Status | Final |

---

## 1. Overview

### 1.1 Purpose
The Staff Management module manages staff members, their profiles, availability, assignments to events, and performance tracking throughout the EventManagementPlatform system.

### 1.2 Scope
This specification covers all backend requirements for staff registration, profile management, availability scheduling, event assignments, check-in/out tracking, and performance evaluation.

### 1.3 Technology Stack
- **Framework**: .NET 8.0
- **Database**: SQL Server Express (via Entity Framework Core)
- **Cloud**: Azure App Service, Azure SQL Database
- **AI Integration**: Azure AI Services for staff recommendations and scheduling optimization
- **Messaging**: MediatR for CQRS pattern implementation
- **Storage**: Azure Blob Storage for staff photos

---

## 2. Domain Model

### 2.1 Aggregate: StaffMember
The StaffMember aggregate is the central entity in this bounded context.

#### 2.1.1 Entity Properties
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| StaffMemberId | Guid | Yes | Unique identifier |
| FirstName | string | Yes | First name (max 100 chars) |
| LastName | string | Yes | Last name (max 100 chars) |
| Email | string | Yes | Email address (unique) |
| PhoneNumber | string | Yes | Contact phone number |
| PhotoUrl | string | No | Azure Blob Storage URL for photo |
| Status | StaffStatus | Yes | Current status |
| HireDate | DateTime | Yes | Date of hire |
| TerminationDate | DateTime | No | Date of termination if applicable |
| Role | StaffRole | Yes | Primary role |
| Skills | List&lt;string&gt; | No | List of skills |
| HourlyRate | decimal | No | Hourly compensation rate |
| CreatedAt | DateTime | Yes | Creation timestamp |
| ModifiedAt | DateTime | No | Last modification timestamp |
| CreatedBy | Guid | Yes | User who created the record |
| ModifiedBy | Guid | No | User who last modified |

#### 2.1.2 StaffStatus Enumeration
```csharp
public enum StaffStatus
{
    Active = 0,
    Inactive = 1,
    OnLeave = 2,
    Terminated = 3
}
```

#### 2.1.3 StaffRole Enumeration
```csharp
public enum StaffRole
{
    EventCoordinator = 0,
    SetupCrew = 1,
    Server = 2,
    Bartender = 3,
    Chef = 4,
    Security = 5,
    Photographer = 6,
    DJ = 7,
    Manager = 8,
    Other = 9
}
```

### 2.2 Entity: StaffAvailability
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| StaffAvailabilityId | Guid | Yes | Unique identifier |
| StaffMemberId | Guid | Yes | Parent staff member reference |
| DayOfWeek | DayOfWeek | Yes | Day of the week |
| StartTime | TimeSpan | Yes | Start time of availability |
| EndTime | TimeSpan | Yes | End time of availability |
| IsRecurring | bool | Yes | Whether this is a recurring availability |
| EffectiveFrom | DateTime | Yes | Start date of this availability |
| EffectiveTo | DateTime | No | End date of this availability |

### 2.3 Entity: StaffUnavailableDate
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| StaffUnavailableDateId | Guid | Yes | Unique identifier |
| StaffMemberId | Guid | Yes | Parent staff member reference |
| Date | DateTime | Yes | Unavailable date |
| Reason | string | No | Reason for unavailability |
| IsAllDay | bool | Yes | Whether unavailable all day |
| StartTime | TimeSpan | No | Start time if not all day |
| EndTime | TimeSpan | No | End time if not all day |

### 2.4 Entity: StaffAssignment
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| StaffAssignmentId | Guid | Yes | Unique identifier |
| StaffMemberId | Guid | Yes | Staff member reference |
| EventId | Guid | Yes | Event reference |
| AssignedRole | StaffRole | Yes | Role for this assignment |
| AssignmentStatus | AssignmentStatus | Yes | Current status |
| RequestedAt | DateTime | Yes | When assignment was requested |
| ConfirmedAt | DateTime | No | When staff confirmed |
| AssignedBy | Guid | Yes | Who assigned the staff |
| Notes | string | No | Assignment notes |

#### 2.4.1 AssignmentStatus Enumeration
```csharp
public enum AssignmentStatus
{
    Requested = 0,
    Confirmed = 1,
    Declined = 2,
    Completed = 3,
    NoShow = 4,
    Cancelled = 5
}
```

### 2.5 Entity: StaffCheckInOut
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| StaffCheckInOutId | Guid | Yes | Unique identifier |
| StaffAssignmentId | Guid | Yes | Assignment reference |
| CheckInTime | DateTime | Yes | Check-in timestamp |
| CheckOutTime | DateTime | No | Check-out timestamp |
| CheckedInBy | Guid | No | Who performed check-in |
| CheckedOutBy | Guid | No | Who performed check-out |
| TotalHours | decimal | No | Calculated hours worked |

### 2.6 Entity: StaffPerformanceReview
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| StaffPerformanceReviewId | Guid | Yes | Unique identifier |
| StaffMemberId | Guid | Yes | Staff member reference |
| EventId | Guid | No | Related event if applicable |
| Rating | int | Yes | Rating (1-5) |
| Feedback | string | No | Performance feedback |
| ReviewType | ReviewType | Yes | Type of review |
| ReviewedBy | Guid | Yes | Who provided the review |
| ReviewedAt | DateTime | Yes | When review was created |

#### 2.6.1 ReviewType Enumeration
```csharp
public enum ReviewType
{
    Feedback = 0,
    Complaint = 1,
    Compliment = 2,
    PerformanceReview = 3
}
```

---

## 3. Domain Events

### 3.1 Staff Profile Events
| Event | Trigger | Payload |
|-------|---------|---------|
| StaffMemberRegistered | New staff registered | StaffMemberId, FirstName, LastName, Email, Role |
| StaffProfileUpdated | Profile modified | StaffMemberId, ChangedProperties |
| StaffProfileActivated | Status changed to Active | StaffMemberId, ActivatedBy |
| StaffProfileDeactivated | Status changed to Inactive | StaffMemberId, DeactivatedBy, Reason |
| StaffPhotoUploaded | Photo added/updated | StaffMemberId, PhotoUrl |
| StaffPhotoRemoved | Photo deleted | StaffMemberId |
| StaffMemberRemoved | Staff terminated | StaffMemberId, TerminationDate, Reason |

### 3.2 Staff Availability Events
| Event | Trigger | Payload |
|-------|---------|---------|
| StaffAvailabilityDeclared | New availability created | StaffMemberId, DayOfWeek, StartTime, EndTime |
| StaffAvailabilityUpdated | Availability modified | StaffMemberId, AvailabilityId, Changes |
| StaffUnavailableDateAdded | Unavailable date added | StaffMemberId, Date, Reason |
| StaffUnavailableDateRemoved | Unavailable date removed | StaffMemberId, Date |
| StaffRecurringAvailabilitySet | Recurring schedule set | StaffMemberId, AvailabilityPattern |

### 3.3 Staff Assignment Events
| Event | Trigger | Payload |
|-------|---------|---------|
| StaffBookingRequested | Assignment requested | StaffMemberId, EventId, AssignedRole |
| StaffAssignedToEvent | Staff assigned to event | StaffMemberId, EventId, AssignmentId |
| StaffReassignedToEvent | Assignment changed | StaffMemberId, OldEventId, NewEventId |
| StaffUnassignedFromEvent | Assignment removed | StaffMemberId, EventId, Reason |
| StaffBookingConfirmed | Staff confirmed assignment | StaffMemberId, AssignmentId |
| StaffBookingDeclined | Staff declined assignment | StaffMemberId, AssignmentId, Reason |
| StaffDoubleBookingDetected | Conflict detected | StaffMemberId, ConflictingEventIds |
| StaffDoubleBookingPrevented | Conflict prevented | StaffMemberId, EventId, Reason |

### 3.4 Staff Check-In/Out Events
| Event | Trigger | Payload |
|-------|---------|---------|
| StaffCheckedIn | Staff checked in | StaffMemberId, AssignmentId, CheckInTime |
| StaffCheckedOut | Staff checked out | StaffMemberId, AssignmentId, CheckOutTime, TotalHours |
| StaffNoShow | Staff didn't show up | StaffMemberId, AssignmentId, EventId |

### 3.5 Staff Performance Events
| Event | Trigger | Payload |
|-------|---------|---------|
| StaffRatingRecorded | Performance rating added | StaffMemberId, Rating, ReviewedBy |
| StaffFeedbackReceived | Feedback submitted | StaffMemberId, Feedback, ReviewType |
| StaffComplaintReceived | Complaint filed | StaffMemberId, Complaint, FiledBy |
| StaffComplimentReceived | Compliment received | StaffMemberId, Compliment, GivenBy |

---

## 4. API Endpoints

### 4.1 Staff Management Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/staff | List all staff with pagination |
| GET | /api/staff/{staffId} | Get staff by ID |
| POST | /api/staff | Register new staff member |
| PUT | /api/staff/{staffId} | Update staff profile |
| DELETE | /api/staff/{staffId} | Soft delete staff |
| POST | /api/staff/{staffId}/activate | Activate staff profile |
| POST | /api/staff/{staffId}/deactivate | Deactivate staff profile |
| POST | /api/staff/{staffId}/photo | Upload staff photo |
| DELETE | /api/staff/{staffId}/photo | Remove staff photo |

### 4.2 Staff Availability Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/staff/{staffId}/availability | Get staff availability |
| POST | /api/staff/{staffId}/availability | Declare availability |
| PUT | /api/staff/{staffId}/availability/{availabilityId} | Update availability |
| DELETE | /api/staff/{staffId}/availability/{availabilityId} | Remove availability |
| POST | /api/staff/{staffId}/availability/recurring | Set recurring availability |
| GET | /api/staff/{staffId}/unavailable-dates | Get unavailable dates |
| POST | /api/staff/{staffId}/unavailable-dates | Add unavailable date |
| DELETE | /api/staff/{staffId}/unavailable-dates/{dateId} | Remove unavailable date |

### 4.3 Staff Assignment Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/staff/assignments | List all assignments |
| GET | /api/staff/{staffId}/assignments | Get staff assignments |
| POST | /api/staff/assignments | Assign staff to event |
| PUT | /api/staff/assignments/{assignmentId} | Update assignment |
| DELETE | /api/staff/assignments/{assignmentId} | Remove assignment |
| POST | /api/staff/assignments/{assignmentId}/confirm | Confirm assignment |
| POST | /api/staff/assignments/{assignmentId}/decline | Decline assignment |
| GET | /api/staff/available | Find available staff for date/time |

### 4.4 Staff Check-In/Out Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/staff/assignments/{assignmentId}/check-in | Check in staff |
| POST | /api/staff/assignments/{assignmentId}/check-out | Check out staff |
| POST | /api/staff/assignments/{assignmentId}/no-show | Mark as no-show |
| GET | /api/staff/{staffId}/timesheet | Get staff timesheet |

### 4.5 Staff Performance Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/staff/{staffId}/reviews | Get performance reviews |
| POST | /api/staff/{staffId}/reviews | Add performance review |
| POST | /api/staff/{staffId}/feedback | Submit feedback |
| POST | /api/staff/{staffId}/complaint | File complaint |
| POST | /api/staff/{staffId}/compliment | Give compliment |
| GET | /api/staff/{staffId}/ratings | Get staff ratings summary |

---

## 5. Commands and Queries (CQRS)

### 5.1 Commands
```
EventManagementPlatform.Api/Features/Staff/
├── RegisterStaff/
│   ├── RegisterStaffCommand.cs
│   ├── RegisterStaffCommandHandler.cs
│   └── RegisterStaffDto.cs
├── UpdateStaffProfile/
│   ├── UpdateStaffProfileCommand.cs
│   ├── UpdateStaffProfileCommandHandler.cs
│   └── UpdateStaffProfileDto.cs
├── ActivateStaff/
│   ├── ActivateStaffCommand.cs
│   └── ActivateStaffCommandHandler.cs
├── DeactivateStaff/
│   ├── DeactivateStaffCommand.cs
│   └── DeactivateStaffCommandHandler.cs
├── UploadStaffPhoto/
│   ├── UploadStaffPhotoCommand.cs
│   └── UploadStaffPhotoCommandHandler.cs
├── DeclareAvailability/
│   ├── DeclareAvailabilityCommand.cs
│   ├── DeclareAvailabilityCommandHandler.cs
│   └── DeclareAvailabilityDto.cs
├── AddUnavailableDate/
│   ├── AddUnavailableDateCommand.cs
│   └── AddUnavailableDateCommandHandler.cs
├── AssignStaffToEvent/
│   ├── AssignStaffToEventCommand.cs
│   ├── AssignStaffToEventCommandHandler.cs
│   └── AssignStaffDto.cs
├── ConfirmAssignment/
│   ├── ConfirmAssignmentCommand.cs
│   └── ConfirmAssignmentCommandHandler.cs
├── DeclineAssignment/
│   ├── DeclineAssignmentCommand.cs
│   └── DeclineAssignmentCommandHandler.cs
├── CheckInStaff/
│   ├── CheckInStaffCommand.cs
│   └── CheckInStaffCommandHandler.cs
├── CheckOutStaff/
│   ├── CheckOutStaffCommand.cs
│   └── CheckOutStaffCommandHandler.cs
└── RecordStaffRating/
    ├── RecordStaffRatingCommand.cs
    ├── RecordStaffRatingCommandHandler.cs
    └── StaffRatingDto.cs
```

### 5.2 Queries
```
EventManagementPlatform.Api/Features/Staff/
├── GetStaff/
│   ├── GetStaffQuery.cs
│   ├── GetStaffQueryHandler.cs
│   └── StaffListDto.cs
├── GetStaffById/
│   ├── GetStaffByIdQuery.cs
│   ├── GetStaffByIdQueryHandler.cs
│   └── StaffDetailDto.cs
├── GetStaffAvailability/
│   ├── GetStaffAvailabilityQuery.cs
│   ├── GetStaffAvailabilityQueryHandler.cs
│   └── StaffAvailabilityDto.cs
├── GetStaffAssignments/
│   ├── GetStaffAssignmentsQuery.cs
│   ├── GetStaffAssignmentsQueryHandler.cs
│   └── StaffAssignmentDto.cs
├── FindAvailableStaff/
│   ├── FindAvailableStaffQuery.cs
│   ├── FindAvailableStaffQueryHandler.cs
│   └── AvailableStaffDto.cs
├── GetStaffTimesheet/
│   ├── GetStaffTimesheetQuery.cs
│   ├── GetStaffTimesheetQueryHandler.cs
│   └── StaffTimesheetDto.cs
└── GetStaffPerformance/
    ├── GetStaffPerformanceQuery.cs
    ├── GetStaffPerformanceQueryHandler.cs
    └── StaffPerformanceDto.cs
```

---

## 6. Business Rules

### 6.1 Staff Registration Rules
| Rule ID | Description |
|---------|-------------|
| STF-001 | Email must be unique across all staff members |
| STF-002 | Phone number is required and must be valid format |
| STF-003 | Hire date cannot be in the future |
| STF-004 | At least one role must be assigned |

### 6.2 Staff Availability Rules
| Rule ID | Description |
|---------|-------------|
| STF-010 | Availability end time must be after start time |
| STF-011 | Recurring availability requires effective from date |
| STF-012 | Unavailable dates cannot overlap with confirmed assignments |
| STF-013 | Cannot declare availability for past dates |

### 6.3 Staff Assignment Rules
| Rule ID | Description |
|---------|-------------|
| STF-020 | Staff must be active to be assigned |
| STF-021 | Cannot assign staff to overlapping event times (double booking prevention) |
| STF-022 | Staff must have availability for the event time |
| STF-023 | Staff must not be on an unavailable date for the event |
| STF-024 | Declined assignments cannot be reassigned without staff confirmation |

### 6.4 Check-In/Out Rules
| Rule ID | Description |
|---------|-------------|
| STF-030 | Staff can only check in within 1 hour before event start time |
| STF-031 | Check-out time must be after check-in time |
| STF-032 | Cannot check in if already checked in |
| STF-033 | No-show marked if not checked in within 30 minutes of event start |

### 6.5 Performance Review Rules
| Rule ID | Description |
|---------|-------------|
| STF-040 | Rating must be between 1 and 5 |
| STF-041 | Performance reviews require completed assignment |
| STF-042 | Only managers can mark complaints as resolved |
| STF-043 | Staff cannot review themselves |

---

## 7. Azure Integration

### 7.1 Azure AI Services
| Service | Purpose |
|---------|---------|
| Azure Cognitive Services | Analyze feedback sentiment and identify patterns |
| Azure OpenAI | Smart staff recommendations based on skills and performance |
| Azure Anomaly Detector | Detect unusual attendance patterns or no-show trends |

### 7.2 Azure Infrastructure
| Service | Purpose |
|---------|---------|
| Azure App Service | Host API application |
| Azure SQL Database | Primary data storage |
| Azure Blob Storage | Staff photos and documents |
| Azure Service Bus | Event publishing for integrations |
| Azure Application Insights | Monitoring and telemetry |
| Azure Functions | Automated scheduling and reminder processing |

---

## 8. Data Transfer Objects (DTOs)

### 8.1 RegisterStaffDto
```csharp
public record RegisterStaffDto(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    StaffRole Role,
    DateTime HireDate,
    decimal? HourlyRate,
    List<string>? Skills
);
```

### 8.2 StaffDetailDto
```csharp
public record StaffDetailDto(
    Guid StaffMemberId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string? PhotoUrl,
    StaffStatus Status,
    StaffRole Role,
    DateTime HireDate,
    DateTime? TerminationDate,
    decimal? HourlyRate,
    List<string> Skills,
    decimal AverageRating,
    int TotalAssignments,
    int CompletedAssignments,
    DateTime CreatedAt
);
```

### 8.3 StaffListDto
```csharp
public record StaffListDto(
    Guid StaffMemberId,
    string FullName,
    string Email,
    StaffStatus Status,
    StaffRole Role,
    decimal? AverageRating,
    string? PhotoUrl
);
```

### 8.4 StaffAvailabilityDto
```csharp
public record StaffAvailabilityDto(
    Guid StaffAvailabilityId,
    DayOfWeek DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime,
    bool IsRecurring,
    DateTime EffectiveFrom,
    DateTime? EffectiveTo
);
```

### 8.5 StaffAssignmentDto
```csharp
public record StaffAssignmentDto(
    Guid StaffAssignmentId,
    Guid StaffMemberId,
    string StaffName,
    Guid EventId,
    string EventTitle,
    DateTime EventDate,
    StaffRole AssignedRole,
    AssignmentStatus Status,
    DateTime RequestedAt,
    DateTime? ConfirmedAt
);
```

---

## 9. Validation Requirements

### 9.1 Input Validation
| Field | Validation Rules |
|-------|-----------------|
| FirstName | Required, 1-100 characters |
| LastName | Required, 1-100 characters |
| Email | Required, valid email format, unique |
| PhoneNumber | Required, valid phone format |
| HireDate | Required, cannot be future date |
| HourlyRate | Optional, must be >= 0 |
| Rating | Required, 1-5 range |

### 9.2 FluentValidation Implementation
```csharp
public class RegisterStaffCommandValidator : AbstractValidator<RegisterStaffCommand>
{
    public RegisterStaffCommandValidator(IEventManagementPlatformContext context)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (email, cancellation) =>
            {
                return !await context.StaffMembers
                    .AnyAsync(s => s.Email == email, cancellation);
            })
            .WithMessage("Email address already exists");

        RuleFor(x => x.HireDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Hire date cannot be in the future");

        RuleFor(x => x.HourlyRate)
            .GreaterThanOrEqualTo(0)
            .When(x => x.HourlyRate.HasValue);
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
| Staff | View own profile, manage own availability, confirm/decline assignments |
| Manager | All staff operations, assign staff, check-in/out, view all staff |
| Admin | Full access including termination and data deletion |
| Customer | View assigned staff for their events (read-only) |

---

## 11. Performance Requirements

| Metric | Requirement |
|--------|-------------|
| API Response Time | < 200ms for 95th percentile |
| Staff List Query | Support 10,000+ staff with pagination |
| Availability Lookup | < 100ms for availability check |
| Double Booking Check | < 150ms for conflict detection |
| Photo Upload | Support up to 5MB files |

---

## 12. Error Handling

### 12.1 Error Response Format
```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "Bad Request",
    "status": 400,
    "detail": "Email address already exists",
    "instance": "/api/staff",
    "errors": {
        "Email": ["Email address already exists"]
    }
}
```

### 12.2 Exception Types
| Exception | HTTP Status | Description |
|-----------|-------------|-------------|
| ValidationException | 400 | Input validation failed |
| NotFoundException | 404 | Staff member not found |
| ConflictException | 409 | Double booking or status conflict |
| UnauthorizedException | 401 | Authentication required |
| ForbiddenException | 403 | Insufficient permissions |
| DoubleBookingException | 409 | Staff assignment conflict detected |

---

## 13. Testing Requirements

### 13.1 Unit Tests
- Test all command handlers
- Test all query handlers
- Test domain event generation
- Test validation rules
- Test double booking detection logic

### 13.2 Integration Tests
- Test API endpoints
- Test database operations
- Test event publishing
- Test photo upload to Azure Blob Storage
- Test availability calculations

### 13.3 Test Coverage
- Minimum 80% code coverage
- 100% coverage for business rules
- 100% coverage for double booking prevention

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
