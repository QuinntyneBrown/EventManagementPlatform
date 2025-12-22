# Scheduling & Calendar Management - Backend Specification

## Document Information
| Field | Value |
|-------|-------|
| Feature | Scheduling & Calendar Management |
| Version | 1.0 |
| Date | 2025-12-22 |
| Status | Final |

---

## 1. Overview

### 1.1 Purpose
The Scheduling & Calendar Management module manages event scheduling, calendar views, conflict detection, and resource availability for the EventManagementPlatform system.

### 1.2 Scope
This specification covers all backend requirements for scheduling events on calendars, detecting and resolving conflicts, managing resource availability, preventing overbooking, and generating calendar views and exports.

### 1.3 Technology Stack
- **Framework**: .NET 8.0
- **Database**: SQL Server Express (via Entity Framework Core)
- **Cloud**: Azure App Service, Azure SQL Database
- **AI Integration**: Azure AI Services for intelligent conflict prediction and scheduling optimization
- **Messaging**: MediatR for CQRS pattern implementation
- **Caching**: Azure Redis Cache for calendar view caching

---

## 2. Domain Model

### 2.1 Aggregate: ScheduledEvent
The ScheduledEvent aggregate manages the scheduling lifecycle of events.

#### 2.1.1 Entity Properties
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| ScheduledEventId | Guid | Yes | Unique identifier |
| EventId | Guid | Yes | Reference to Event aggregate |
| CalendarId | Guid | Yes | Reference to Calendar |
| StartDateTime | DateTime | Yes | Event start date and time |
| EndDateTime | DateTime | Yes | Event end date and time |
| TimeZone | string | Yes | IANA timezone identifier |
| RecurrencePattern | RecurrencePattern? | No | Recurrence configuration |
| Status | ScheduleStatus | Yes | Current schedule status |
| ConflictLevel | ConflictLevel | Yes | Conflict severity level |
| IsBlocked | bool | Yes | Whether scheduling is blocked |
| BlockReason | string? | No | Reason for blocking |
| CreatedAt | DateTime | Yes | Creation timestamp |
| ModifiedAt | DateTime? | No | Last modification timestamp |
| CreatedBy | Guid | Yes | User who created |
| ModifiedBy | Guid? | No | User who last modified |

#### 2.1.2 ScheduleStatus Enumeration
```csharp
public enum ScheduleStatus
{
    Tentative = 0,
    Scheduled = 1,
    Confirmed = 2,
    Rescheduling = 3,
    Cancelled = 4
}
```

#### 2.1.3 ConflictLevel Enumeration
```csharp
public enum ConflictLevel
{
    None = 0,
    Minor = 1,
    Moderate = 2,
    Severe = 3,
    Critical = 4
}
```

### 2.2 Entity: Calendar
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| CalendarId | Guid | Yes | Unique identifier |
| Name | string | Yes | Calendar name |
| Description | string? | No | Calendar description |
| CalendarType | CalendarType | Yes | Type of calendar |
| OwnerId | Guid | Yes | Owner reference (Venue, Staff, etc.) |
| OwnerType | OwnerType | Yes | Type of owner |
| IsActive | bool | Yes | Whether calendar is active |
| AllowOverbooking | bool | Yes | Whether overbooking is allowed |
| MaxConcurrentEvents | int | Yes | Max events at same time |
| TimeZone | string | Yes | Default timezone |
| CreatedAt | DateTime | Yes | Creation timestamp |

### 2.3 Entity: ScheduleConflict
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| ScheduleConflictId | Guid | Yes | Unique identifier |
| ScheduledEventId | Guid | Yes | Primary scheduled event |
| ConflictingEventId | Guid | Yes | Conflicting event |
| ConflictType | ConflictType | Yes | Type of conflict |
| ConflictLevel | ConflictLevel | Yes | Severity level |
| Status | ConflictStatus | Yes | Current status |
| DetectedAt | DateTime | Yes | When conflict was detected |
| ResolvedAt | DateTime? | No | When conflict was resolved |
| ResolutionMethod | ResolutionMethod? | No | How conflict was resolved |
| ResolvedBy | Guid? | No | Who resolved the conflict |
| EscalatedAt | DateTime? | No | When conflict was escalated |
| EscalatedTo | Guid? | No | Who it was escalated to |

### 2.4 Entity: ResourceAvailability
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| ResourceAvailabilityId | Guid | Yes | Unique identifier |
| ResourceId | Guid | Yes | Resource reference |
| ResourceType | ResourceType | Yes | Type of resource |
| StartDateTime | DateTime | Yes | Availability start |
| EndDateTime | DateTime | Yes | Availability end |
| IsAvailable | bool | Yes | Whether available |
| UnavailableReason | string? | No | Reason if unavailable |
| MarkedAt | DateTime | Yes | When availability was set |
| MarkedBy | Guid | Yes | Who set the availability |

### 2.5 Value Objects

#### 2.5.1 RecurrencePattern
```csharp
public record RecurrencePattern
{
    public RecurrenceType Type { get; init; }
    public int Interval { get; init; }
    public DayOfWeek[] DaysOfWeek { get; init; }
    public int? DayOfMonth { get; init; }
    public DateTime? EndDate { get; init; }
    public int? OccurrenceCount { get; init; }
}
```

#### 2.5.2 TimeSlot
```csharp
public record TimeSlot
{
    public DateTime StartDateTime { get; init; }
    public DateTime EndDateTime { get; init; }
    public TimeZoneInfo TimeZone { get; init; }
    public TimeSpan Duration => EndDateTime - StartDateTime;
}
```

### 2.6 Enumerations

#### 2.6.1 ConflictType
```csharp
public enum ConflictType
{
    TimeOverlap = 0,
    ResourceConflict = 1,
    VenueConflict = 2,
    StaffConflict = 3,
    EquipmentConflict = 4
}
```

#### 2.6.2 ConflictStatus
```csharp
public enum ConflictStatus
{
    Detected = 0,
    UnderReview = 1,
    Resolved = 2,
    Escalated = 3,
    Dismissed = 4
}
```

#### 2.6.3 ResolutionMethod
```csharp
public enum ResolutionMethod
{
    Rescheduled = 0,
    ResourceChanged = 1,
    EventCancelled = 2,
    OverrideApplied = 3,
    Split = 4
}
```

#### 2.6.4 CalendarType
```csharp
public enum CalendarType
{
    Venue = 0,
    Staff = 1,
    Equipment = 2,
    Master = 3,
    Department = 4
}
```

#### 2.6.5 OwnerType
```csharp
public enum OwnerType
{
    Venue = 0,
    Staff = 1,
    Equipment = 2,
    Department = 3,
    Organization = 4
}
```

#### 2.6.6 ResourceType
```csharp
public enum ResourceType
{
    Venue = 0,
    Staff = 1,
    Equipment = 2,
    Other = 3
}
```

#### 2.6.7 RecurrenceType
```csharp
public enum RecurrenceType
{
    None = 0,
    Daily = 1,
    Weekly = 2,
    Monthly = 3,
    Yearly = 4,
    Custom = 5
}
```

---

## 3. Domain Events

### 3.1 Scheduling Events
| Event | Trigger | Payload |
|-------|---------|---------|
| EventScheduledOnCalendar | Event added to calendar | ScheduledEventId, EventId, CalendarId, StartDateTime, EndDateTime |
| EventRescheduled | Event date/time changed | ScheduledEventId, OldStartDateTime, OldEndDateTime, NewStartDateTime, NewEndDateTime, RescheduledBy |
| EventRemovedFromCalendar | Event removed from schedule | ScheduledEventId, EventId, CalendarId, RemovedBy, Reason |

### 3.2 Calendar View Events
| Event | Trigger | Payload |
|-------|---------|---------|
| CalendarViewGenerated | Calendar view requested | CalendarId, ViewType, StartDate, EndDate, GeneratedAt |
| DailyScheduleExported | Daily schedule exported | CalendarId, Date, Format, ExportedBy |
| WeeklyScheduleExported | Weekly schedule exported | CalendarId, WeekStartDate, Format, ExportedBy |
| MonthlyScheduleExported | Monthly schedule exported | CalendarId, Month, Year, Format, ExportedBy |

### 3.3 Conflict Events
| Event | Trigger | Payload |
|-------|---------|---------|
| ScheduleConflictDetected | Conflict identified | ScheduleConflictId, ScheduledEventId, ConflictingEventId, ConflictType, ConflictLevel |
| ScheduleConflictResolved | Conflict resolved | ScheduleConflictId, ResolutionMethod, ResolvedBy, ResolvedAt |
| ScheduleConflictEscalated | Conflict escalated | ScheduleConflictId, EscalatedTo, EscalationReason, EscalatedAt |

### 3.4 Overbooking Events
| Event | Trigger | Payload |
|-------|---------|---------|
| OverbookingWarningTriggered | Approaching capacity | CalendarId, CurrentCount, MaxCapacity, WarningThreshold |
| OverbookingPreventedBySystem | Booking blocked at capacity | CalendarId, ScheduledEventId, AttemptedBy, PreventedAt |
| OverbookingAllowedByOverride | Manual override applied | CalendarId, ScheduledEventId, OverriddenBy, OverrideReason, OverriddenAt |

### 3.5 Resource Availability Events
| Event | Trigger | Payload |
|-------|---------|---------|
| ResourceAvailabilityChecked | Availability queried | ResourceId, ResourceType, StartDateTime, EndDateTime, IsAvailable |
| MultipleResourcesChecked | Batch availability check | ResourceIds, StartDateTime, EndDateTime, AvailabilityResults |
| ResourceMarkedUnavailable | Resource marked unavailable | ResourceAvailabilityId, ResourceId, StartDateTime, EndDateTime, Reason, MarkedBy |
| ResourceMarkedAvailable | Resource marked available | ResourceAvailabilityId, ResourceId, StartDateTime, EndDateTime, MarkedBy |

---

## 4. API Endpoints

### 4.1 Scheduling Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/schedules | List all scheduled events with filtering |
| GET | /api/schedules/{scheduleId} | Get scheduled event by ID |
| POST | /api/schedules | Schedule event on calendar |
| PUT | /api/schedules/{scheduleId} | Update scheduled event |
| DELETE | /api/schedules/{scheduleId} | Remove event from calendar |
| POST | /api/schedules/{scheduleId}/reschedule | Reschedule event to new time |
| POST | /api/schedules/{scheduleId}/confirm | Confirm schedule |

### 4.2 Calendar View Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/calendars/{calendarId}/view | Get calendar view |
| GET | /api/calendars/{calendarId}/daily | Get daily schedule |
| GET | /api/calendars/{calendarId}/weekly | Get weekly schedule |
| GET | /api/calendars/{calendarId}/monthly | Get monthly schedule |
| POST | /api/calendars/{calendarId}/export/daily | Export daily schedule |
| POST | /api/calendars/{calendarId}/export/weekly | Export weekly schedule |
| POST | /api/calendars/{calendarId}/export/monthly | Export monthly schedule |

### 4.3 Conflict Management Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/schedules/conflicts | List all conflicts |
| GET | /api/schedules/conflicts/{conflictId} | Get conflict details |
| POST | /api/schedules/conflicts/detect | Manually trigger conflict detection |
| POST | /api/schedules/conflicts/{conflictId}/resolve | Resolve conflict |
| POST | /api/schedules/conflicts/{conflictId}/escalate | Escalate conflict |
| POST | /api/schedules/conflicts/{conflictId}/dismiss | Dismiss conflict |

### 4.4 Resource Availability Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/resources/{resourceId}/availability | Get resource availability |
| POST | /api/resources/availability/check | Check availability for resources |
| POST | /api/resources/availability/check-multiple | Batch check multiple resources |
| POST | /api/resources/{resourceId}/availability/mark-unavailable | Mark resource unavailable |
| POST | /api/resources/{resourceId}/availability/mark-available | Mark resource available |
| GET | /api/resources/{resourceId}/availability/slots | Get available time slots |

### 4.5 Calendar Management Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/calendars | List all calendars |
| GET | /api/calendars/{calendarId} | Get calendar by ID |
| POST | /api/calendars | Create new calendar |
| PUT | /api/calendars/{calendarId} | Update calendar |
| DELETE | /api/calendars/{calendarId} | Delete calendar |

---

## 5. Commands and Queries (CQRS)

### 5.1 Commands
```
EventManagementPlatform.Api/Features/Scheduling/
├── ScheduleEvent/
│   ├── ScheduleEventCommand.cs
│   ├── ScheduleEventCommandHandler.cs
│   └── ScheduleEventDto.cs
├── RescheduleEvent/
│   ├── RescheduleEventCommand.cs
│   ├── RescheduleEventCommandHandler.cs
│   └── RescheduleEventDto.cs
├── RemoveEventFromCalendar/
│   ├── RemoveEventFromCalendarCommand.cs
│   └── RemoveEventFromCalendarCommandHandler.cs
├── ConfirmSchedule/
│   ├── ConfirmScheduleCommand.cs
│   └── ConfirmScheduleCommandHandler.cs
├── ResolveConflict/
│   ├── ResolveConflictCommand.cs
│   ├── ResolveConflictCommandHandler.cs
│   └── ResolveConflictDto.cs
├── EscalateConflict/
│   ├── EscalateConflictCommand.cs
│   └── EscalateConflictCommandHandler.cs
├── MarkResourceUnavailable/
│   ├── MarkResourceUnavailableCommand.cs
│   ├── MarkResourceUnavailableCommandHandler.cs
│   └── MarkResourceUnavailableDto.cs
└── MarkResourceAvailable/
    ├── MarkResourceAvailableCommand.cs
    ├── MarkResourceAvailableCommandHandler.cs
    └── MarkResourceAvailableDto.cs
```

### 5.2 Queries
```
EventManagementPlatform.Api/Features/Scheduling/
├── GetScheduledEvents/
│   ├── GetScheduledEventsQuery.cs
│   ├── GetScheduledEventsQueryHandler.cs
│   └── ScheduledEventListDto.cs
├── GetScheduledEventById/
│   ├── GetScheduledEventByIdQuery.cs
│   ├── GetScheduledEventByIdQueryHandler.cs
│   └── ScheduledEventDetailDto.cs
├── GetCalendarView/
│   ├── GetCalendarViewQuery.cs
│   ├── GetCalendarViewQueryHandler.cs
│   └── CalendarViewDto.cs
├── GetDailySchedule/
│   ├── GetDailyScheduleQuery.cs
│   ├── GetDailyScheduleQueryHandler.cs
│   └── DailyScheduleDto.cs
├── GetWeeklySchedule/
│   ├── GetWeeklyScheduleQuery.cs
│   ├── GetWeeklyScheduleQueryHandler.cs
│   └── WeeklyScheduleDto.cs
├── GetMonthlySchedule/
│   ├── GetMonthlyScheduleQuery.cs
│   ├── GetMonthlyScheduleQueryHandler.cs
│   └── MonthlyScheduleDto.cs
├── GetConflicts/
│   ├── GetConflictsQuery.cs
│   ├── GetConflictsQueryHandler.cs
│   └── ConflictListDto.cs
├── GetConflictById/
│   ├── GetConflictByIdQuery.cs
│   ├── GetConflictByIdQueryHandler.cs
│   └── ConflictDetailDto.cs
└── CheckResourceAvailability/
    ├── CheckResourceAvailabilityQuery.cs
    ├── CheckResourceAvailabilityQueryHandler.cs
    └── ResourceAvailabilityDto.cs
```

---

## 6. Business Rules

### 6.1 Scheduling Rules
| Rule ID | Description |
|---------|-------------|
| SCH-001 | Event start time must be before end time |
| SCH-002 | Minimum event duration is 15 minutes |
| SCH-003 | Maximum event duration is 30 days |
| SCH-004 | Events cannot be scheduled in the past |
| SCH-005 | Schedule changes within 2 hours of start require manager approval |
| SCH-006 | Recurring events cannot exceed 365 occurrences |

### 6.2 Conflict Detection Rules
| Rule ID | Description |
|---------|-------------|
| SCH-010 | System automatically detects time overlaps on same calendar |
| SCH-011 | Venue conflicts are Critical level |
| SCH-012 | Staff conflicts are Moderate level if staff is primary, Minor if secondary |
| SCH-013 | Equipment conflicts are Moderate level |
| SCH-014 | Conflicts detected automatically within 5 seconds of scheduling |
| SCH-015 | Conflicts must be resolved or escalated within 24 hours |

### 6.3 Overbooking Rules
| Rule ID | Description |
|---------|-------------|
| SCH-020 | Overbooking prevention is enforced by default |
| SCH-021 | Manager role can override overbooking prevention |
| SCH-022 | Warning triggered at 80% capacity |
| SCH-023 | System blocks scheduling at 100% capacity unless overridden |
| SCH-024 | All overbooking overrides must include a reason |
| SCH-025 | Overbooking overrides are logged for audit |

### 6.4 Resource Availability Rules
| Rule ID | Description |
|---------|-------------|
| SCH-030 | Resources default to available unless marked otherwise |
| SCH-031 | Unavailable periods must not overlap |
| SCH-032 | Availability checks include 15-minute buffer by default |
| SCH-033 | Bulk availability checks limited to 50 resources |
| SCH-034 | Availability slots generated in 30-minute increments |

### 6.5 Calendar View Rules
| Rule ID | Description |
|---------|-------------|
| SCH-040 | Daily view shows 6am to 11pm by default |
| SCH-041 | Weekly view starts on Monday by default |
| SCH-042 | Monthly view shows full calendar month |
| SCH-043 | Calendar views cached for 5 minutes |
| SCH-044 | Export formats: PDF, Excel, iCalendar |

---

## 7. Azure Integration

### 7.1 Azure AI Services
| Service | Purpose |
|---------|---------|
| Azure OpenAI | Intelligent conflict resolution suggestions |
| Azure Cognitive Services | Pattern recognition for recurring conflicts |
| Azure Machine Learning | Predictive scheduling optimization |
| Azure Anomaly Detector | Detect unusual scheduling patterns |

### 7.2 Azure Infrastructure
| Service | Purpose |
|---------|---------|
| Azure App Service | Host API application |
| Azure SQL Database | Primary data storage |
| Azure Redis Cache | Calendar view caching |
| Azure Service Bus | Event publishing and conflict notifications |
| Azure Application Insights | Monitoring and telemetry |
| Azure Functions | Background conflict detection jobs |
| Azure Blob Storage | Calendar export file storage |

---

## 8. Data Transfer Objects (DTOs)

### 8.1 ScheduleEventDto
```csharp
public record ScheduleEventDto(
    Guid EventId,
    Guid CalendarId,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string TimeZone,
    RecurrencePattern? RecurrencePattern
);
```

### 8.2 ScheduledEventDetailDto
```csharp
public record ScheduledEventDetailDto(
    Guid ScheduledEventId,
    Guid EventId,
    string EventTitle,
    Guid CalendarId,
    string CalendarName,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string TimeZone,
    string Status,
    string ConflictLevel,
    bool IsBlocked,
    string? BlockReason,
    RecurrencePattern? RecurrencePattern,
    IEnumerable<ConflictSummaryDto> Conflicts
);
```

### 8.3 CalendarViewDto
```csharp
public record CalendarViewDto(
    Guid CalendarId,
    string CalendarName,
    DateTime ViewStartDate,
    DateTime ViewEndDate,
    string ViewType,
    IEnumerable<ScheduledEventSummaryDto> Events,
    Dictionary<DateTime, int> EventCounts
);
```

### 8.4 ConflictDetailDto
```csharp
public record ConflictDetailDto(
    Guid ScheduleConflictId,
    Guid ScheduledEventId,
    string ScheduledEventTitle,
    Guid ConflictingEventId,
    string ConflictingEventTitle,
    string ConflictType,
    string ConflictLevel,
    string Status,
    DateTime DetectedAt,
    DateTime? ResolvedAt,
    string? ResolutionMethod,
    IEnumerable<string> SuggestedResolutions
);
```

### 8.5 ResourceAvailabilityDto
```csharp
public record ResourceAvailabilityDto(
    Guid ResourceId,
    string ResourceName,
    string ResourceType,
    DateTime CheckStartTime,
    DateTime CheckEndTime,
    bool IsAvailable,
    string? UnavailableReason,
    IEnumerable<TimeSlotDto> AvailableSlots
);
```

### 8.6 TimeSlotDto
```csharp
public record TimeSlotDto(
    DateTime StartDateTime,
    DateTime EndDateTime,
    TimeSpan Duration,
    bool IsAvailable
);
```

---

## 9. Validation Requirements

### 9.1 Input Validation
| Field | Validation Rules |
|-------|-----------------|
| StartDateTime | Required, must be future date/time, timezone-aware |
| EndDateTime | Required, must be after StartDateTime, timezone-aware |
| CalendarId | Required, must exist and be active |
| EventId | Required, must exist |
| TimeZone | Required, must be valid IANA timezone |
| RecurrencePattern | Optional, must be valid if provided |

### 9.2 FluentValidation Implementation
```csharp
public class ScheduleEventCommandValidator : AbstractValidator<ScheduleEventCommand>
{
    public ScheduleEventCommandValidator(IEventManagementPlatformContext context)
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .MustAsync(async (eventId, ct) =>
                await context.Events.AnyAsync(e => e.EventId == eventId, ct))
            .WithMessage("Event does not exist");

        RuleFor(x => x.CalendarId)
            .NotEmpty()
            .MustAsync(async (calendarId, ct) =>
                await context.Calendars.AnyAsync(c => c.CalendarId == calendarId && c.IsActive, ct))
            .WithMessage("Calendar does not exist or is inactive");

        RuleFor(x => x.StartDateTime)
            .NotEmpty()
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Start time must be in the future");

        RuleFor(x => x.EndDateTime)
            .NotEmpty()
            .GreaterThan(x => x.StartDateTime)
            .WithMessage("End time must be after start time");

        RuleFor(x => x)
            .Must(x => (x.EndDateTime - x.StartDateTime).TotalMinutes >= 15)
            .WithMessage("Event duration must be at least 15 minutes");

        RuleFor(x => x)
            .Must(x => (x.EndDateTime - x.StartDateTime).TotalDays <= 30)
            .WithMessage("Event duration cannot exceed 30 days");

        RuleFor(x => x.TimeZone)
            .NotEmpty()
            .Must(tz => TimeZoneInfo.GetSystemTimeZones().Any(t => t.Id == tz))
            .WithMessage("Invalid timezone");
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
| Customer | View own events, Schedule events (limited), Check availability |
| Staff | View assigned calendars, Schedule events, Mark resources unavailable |
| Manager | All operations, Resolve conflicts, Override overbooking, Escalate conflicts |
| Admin | Full access including calendar configuration |

### 10.3 Data Access Rules
| Rule | Description |
|------|-------------|
| Row-level security | Users can only view calendars they have access to |
| Audit logging | All scheduling operations logged with user ID and timestamp |
| Sensitive data | Conflict details restricted to authorized users |

---

## 11. Performance Requirements

| Metric | Requirement |
|--------|-------------|
| Conflict Detection | < 5 seconds for single event |
| Calendar View Generation | < 1 second for monthly view |
| Availability Check | < 200ms for single resource |
| Bulk Availability Check | < 2 seconds for 50 resources |
| Schedule Query | < 500ms with pagination |
| Export Generation | < 5 seconds for monthly export |
| Concurrent Users | Support 200 concurrent users |
| Cache Hit Rate | > 80% for calendar views |

---

## 12. Caching Strategy

### 12.1 Cached Data
| Data Type | Cache Duration | Invalidation Trigger |
|-----------|----------------|---------------------|
| Calendar Views | 5 minutes | New schedule, reschedule, removal |
| Resource Availability | 2 minutes | Availability status change |
| Conflict List | 1 minute | New conflict, resolution |
| Calendar Metadata | 30 minutes | Calendar update |

### 12.2 Cache Keys
```csharp
public static class CacheKeys
{
    public const string CalendarViewPrefix = "calendar:view:";
    public const string ResourceAvailabilityPrefix = "resource:availability:";
    public const string ConflictListPrefix = "conflicts:";

    public static string CalendarView(Guid calendarId, DateTime date, string viewType)
        => $"{CalendarViewPrefix}{calendarId}:{date:yyyy-MM-dd}:{viewType}";

    public static string ResourceAvailability(Guid resourceId, DateTime date)
        => $"{ResourceAvailabilityPrefix}{resourceId}:{date:yyyy-MM-dd}";
}
```

---

## 13. Background Jobs

### 13.1 Scheduled Jobs
| Job | Schedule | Purpose |
|-----|----------|---------|
| ConflictDetectionJob | Every 5 minutes | Detect new conflicts |
| AvailabilityCleanupJob | Daily at 2am | Remove expired availability records |
| CalendarCacheWarmupJob | Daily at 6am | Pre-populate cache for active calendars |
| ConflictEscalationJob | Every 30 minutes | Escalate unresolved critical conflicts |
| RecurrenceExpansionJob | Daily at 3am | Expand recurring events for next 90 days |

### 13.2 Azure Functions Implementation
```csharp
public class ConflictDetectionFunction
{
    private readonly IMediator _mediator;

    [FunctionName("ConflictDetection")]
    public async Task Run(
        [TimerTrigger("0 */5 * * * *")] TimerInfo timer,
        ILogger log)
    {
        var command = new DetectConflictsCommand();
        await _mediator.Send(command);
        log.LogInformation($"Conflict detection completed at {DateTime.UtcNow}");
    }
}
```

---

## 14. Error Handling

### 14.1 Error Response Format
```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "Scheduling Conflict",
    "status": 409,
    "detail": "The selected time slot conflicts with an existing event",
    "instance": "/api/schedules",
    "errors": {
        "ConflictingEvents": [
            {
                "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "eventTitle": "Annual Gala",
                "startDateTime": "2025-12-25T18:00:00Z",
                "conflictType": "VenueConflict"
            }
        ]
    }
}
```

### 14.2 Exception Types
| Exception | HTTP Status | Description |
|-----------|-------------|-------------|
| ValidationException | 400 | Input validation failed |
| NotFoundException | 404 | Schedule or calendar not found |
| ConflictException | 409 | Scheduling conflict detected |
| OverbookingException | 409 | Calendar at capacity |
| UnauthorizedException | 401 | Authentication required |
| ForbiddenException | 403 | Insufficient permissions |
| TimeSlotUnavailableException | 422 | Requested time slot unavailable |

---

## 15. Testing Requirements

### 15.1 Unit Tests
- Test all command handlers
- Test all query handlers
- Test conflict detection logic
- Test availability checking algorithms
- Test validation rules
- Test domain event generation

### 15.2 Integration Tests
- Test API endpoints
- Test database operations
- Test event publishing
- Test conflict detection integration
- Test cache operations
- Test Azure Functions

### 15.3 Performance Tests
- Load test with 1000 concurrent scheduling requests
- Test calendar view generation with 500+ events
- Test bulk availability checks
- Test conflict detection performance

### 15.4 Test Coverage
- Minimum 85% code coverage
- 100% coverage for conflict detection logic
- 100% coverage for business rules

---

## 16. Monitoring and Telemetry

### 16.1 Application Insights Metrics
| Metric | Type | Description |
|--------|------|-------------|
| scheduling.conflicts.detected | Counter | Number of conflicts detected |
| scheduling.conflicts.resolved | Counter | Number of conflicts resolved |
| scheduling.overbooking.prevented | Counter | Overbooking prevention count |
| scheduling.overbooking.overridden | Counter | Overbooking override count |
| calendar.view.generation.duration | Timer | Calendar view generation time |
| availability.check.duration | Timer | Availability check time |
| cache.hit.rate | Gauge | Cache hit percentage |

### 16.2 Custom Events
```csharp
public class SchedulingTelemetry
{
    private readonly TelemetryClient _telemetryClient;

    public void TrackConflictDetected(ScheduleConflict conflict)
    {
        var properties = new Dictionary<string, string>
        {
            { "ConflictId", conflict.ScheduleConflictId.ToString() },
            { "ConflictType", conflict.ConflictType.ToString() },
            { "ConflictLevel", conflict.ConflictLevel.ToString() }
        };

        _telemetryClient.TrackEvent("ConflictDetected", properties);
    }
}
```

---

## 17. API Response Examples

### 17.1 Schedule Event Response
```json
{
    "scheduledEventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "eventId": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
    "eventTitle": "Corporate Fundraiser",
    "calendarId": "b9a3f8a3-7c42-4f89-8c95-7f7a9c8e5d2b",
    "calendarName": "Grand Ballroom Calendar",
    "startDateTime": "2025-12-25T18:00:00Z",
    "endDateTime": "2025-12-25T23:00:00Z",
    "timeZone": "America/New_York",
    "status": "Scheduled",
    "conflictLevel": "None",
    "isBlocked": false,
    "conflicts": []
}
```

### 17.2 Conflict Detection Response
```json
{
    "scheduleConflictId": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    "scheduledEventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "scheduledEventTitle": "Corporate Fundraiser",
    "conflictingEventId": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
    "conflictingEventTitle": "Wedding Reception",
    "conflictType": "VenueConflict",
    "conflictLevel": "Critical",
    "status": "Detected",
    "detectedAt": "2025-12-22T10:30:00Z",
    "suggestedResolutions": [
        "Reschedule Corporate Fundraiser to December 26",
        "Move Wedding Reception to Garden Pavilion",
        "Split event between two venues"
    ]
}
```

### 17.3 Resource Availability Response
```json
{
    "resourceId": "b9a3f8a3-7c42-4f89-8c95-7f7a9c8e5d2b",
    "resourceName": "Grand Ballroom",
    "resourceType": "Venue",
    "checkStartTime": "2025-12-25T00:00:00Z",
    "checkEndTime": "2025-12-25T23:59:59Z",
    "isAvailable": false,
    "unavailableReason": "Already booked for Wedding Reception",
    "availableSlots": [
        {
            "startDateTime": "2025-12-25T08:00:00Z",
            "endDateTime": "2025-12-25T12:00:00Z",
            "duration": "04:00:00",
            "isAvailable": true
        }
    ]
}
```

---

## 18. Appendices

### 18.1 Related Documents
- [Frontend Specification](./frontend-spec.md)
- [Use Case Diagram](./diagrams/use-case.drawio)
- [C4 Diagrams](./diagrams/c4-context.drawio)
- [Class Diagram](./plantuml/class-diagram.plantuml)
- [Sequence Diagrams](./plantuml/sequence-diagram.plantuml)

### 18.2 Timezone Handling
The system uses IANA timezone identifiers (e.g., "America/New_York") for all datetime operations. All dates stored in the database are in UTC, and conversions happen at the application layer.

### 18.3 Recurrence Pattern Examples
```json
{
    "type": "Weekly",
    "interval": 1,
    "daysOfWeek": ["Monday", "Wednesday", "Friday"],
    "endDate": "2026-12-31T23:59:59Z"
}
```

### 18.4 Revision History
| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-22 | System | Initial specification |
