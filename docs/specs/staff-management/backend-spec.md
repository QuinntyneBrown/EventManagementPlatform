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

## 2. Requirements

### 2.1 Staff CRUD Requirements

### REQ-STF-001: Register New Staff Member

**Requirement:** The system shall allow authorized users to register new staff members with their profile information.

**Acceptance Criteria:**
- [ ] System accepts first name, last name, email, phone number, role, hire date, hourly rate, and skills
- [ ] System generates unique StaffMemberId (Guid) for each new staff member
- [ ] System sets initial status to Active by default
- [ ] System captures CreatedAt timestamp and CreatedBy user ID
- [ ] System publishes StaffMemberRegistered domain event upon successful registration
- [ ] System returns the created staff member details in response

### REQ-STF-002: Retrieve Staff Member by ID

**Requirement:** The system shall provide the ability to retrieve a staff member's complete profile by their unique identifier.

**Acceptance Criteria:**
- [ ] System accepts StaffMemberId as input parameter
- [ ] System returns all staff member properties including computed fields (averageRating, totalAssignments, completedAssignments)
- [ ] System returns 404 Not Found if staff member does not exist
- [ ] Response includes related data summary (assignments count, ratings)

### REQ-STF-003: List All Staff Members with Pagination

**Requirement:** The system shall provide paginated list of staff members with filtering and sorting capabilities.

**Acceptance Criteria:**
- [ ] System supports pagination with configurable page size and page number
- [ ] System supports filtering by status, role, and skills
- [ ] System supports sorting by name, rating, and hire date
- [ ] System supports text search by name and email
- [ ] System returns total count of matching records
- [ ] Query performance meets requirement (< 200ms for 10,000+ staff)

### REQ-STF-004: Update Staff Profile

**Requirement:** The system shall allow authorized users to update staff member profile information.

**Acceptance Criteria:**
- [ ] System allows updating first name, last name, phone number, role, hourly rate, and skills
- [ ] System prevents updating email address (immutable after creation)
- [ ] System updates ModifiedAt timestamp and ModifiedBy user ID
- [ ] System validates all input data before persisting
- [ ] System publishes StaffProfileUpdated domain event with changed properties
- [ ] System returns updated staff member details

### REQ-STF-005: Soft Delete Staff Member

**Requirement:** The system shall support soft deletion of staff members by setting termination date.

**Acceptance Criteria:**
- [ ] System sets TerminationDate to current timestamp
- [ ] System changes Status to Terminated
- [ ] System publishes StaffMemberRemoved domain event
- [ ] System does not physically delete the record from database
- [ ] Terminated staff cannot be assigned to new events
- [ ] Historical data and assignments remain accessible

### REQ-STF-006: Activate Staff Profile

**Requirement:** The system shall allow activation of inactive staff profiles.

**Acceptance Criteria:**
- [ ] System changes Status from Inactive to Active
- [ ] System validates that staff is not terminated
- [ ] System publishes StaffProfileActivated domain event
- [ ] System records who activated the profile
- [ ] Activated staff can be assigned to events

### REQ-STF-007: Deactivate Staff Profile

**Requirement:** The system shall allow deactivation of active staff profiles with a reason.

**Acceptance Criteria:**
- [ ] System changes Status from Active to Inactive
- [ ] System accepts and stores deactivation reason
- [ ] System publishes StaffProfileDeactivated domain event
- [ ] System records who deactivated the profile
- [ ] Deactivated staff cannot be assigned to new events
- [ ] Existing assignments remain valid

### REQ-STF-008: Upload Staff Photo

**Requirement:** The system shall support uploading staff profile photos to Azure Blob Storage.

**Acceptance Criteria:**
- [ ] System accepts image files (JPG, PNG) up to 5MB
- [ ] System uploads photo to Azure Blob Storage
- [ ] System stores blob URL in PhotoUrl property
- [ ] System publishes StaffPhotoUploaded domain event
- [ ] System handles upload failures gracefully
- [ ] Upload completes within 3 seconds for 5MB file

### REQ-STF-009: Remove Staff Photo

**Requirement:** The system shall allow removal of staff profile photos.

**Acceptance Criteria:**
- [ ] System deletes photo from Azure Blob Storage
- [ ] System sets PhotoUrl to null
- [ ] System publishes StaffPhotoRemoved domain event
- [ ] System handles cases where photo file doesn't exist
- [ ] Operation is idempotent

### 2.2 Staff Availability Requirements

### REQ-STF-010: Declare Staff Availability

**Requirement:** The system shall allow staff members to declare their availability for specific days and times.

**Acceptance Criteria:**
- [ ] System accepts day of week, start time, end time, recurring flag, effective from and effective to dates
- [ ] System validates that end time is after start time
- [ ] System validates that effective from date is provided
- [ ] System generates unique StaffAvailabilityId
- [ ] System publishes StaffAvailabilityDeclared domain event
- [ ] System supports multiple availability slots per day

### REQ-STF-011: Update Staff Availability

**Requirement:** The system shall allow modification of existing availability entries.

**Acceptance Criteria:**
- [ ] System accepts availability ID and updated time slots
- [ ] System validates updated times
- [ ] System publishes StaffAvailabilityUpdated domain event
- [ ] System prevents updates that conflict with confirmed assignments
- [ ] Changes take effect immediately for future assignments

### REQ-STF-012: Delete Staff Availability

**Requirement:** The system shall allow removal of availability entries.

**Acceptance Criteria:**
- [ ] System deletes availability by ID
- [ ] System validates no confirmed assignments depend on this availability
- [ ] System publishes appropriate domain event
- [ ] Deletion is soft delete to maintain history

### REQ-STF-013: Set Recurring Availability

**Requirement:** The system shall support setting recurring weekly availability patterns.

**Acceptance Criteria:**
- [ ] System accepts weekly pattern with multiple day/time combinations
- [ ] System creates individual availability records for the pattern
- [ ] System links records as part of recurring schedule
- [ ] System publishes StaffRecurringAvailabilitySet domain event
- [ ] Pattern can span multiple weeks/months

### REQ-STF-014: Add Unavailable Date

**Requirement:** The system shall allow staff to mark specific dates or date ranges as unavailable.

**Acceptance Criteria:**
- [ ] System accepts date, reason, all-day flag, and optional time range
- [ ] System validates date is not in the past
- [ ] System generates unique StaffUnavailableDateId
- [ ] System publishes StaffUnavailableDateAdded domain event
- [ ] System prevents assignment to events on unavailable dates

### REQ-STF-015: Remove Unavailable Date

**Requirement:** The system shall allow removal of unavailable date entries.

**Acceptance Criteria:**
- [ ] System deletes unavailable date by ID
- [ ] System validates no confirmed assignments conflict
- [ ] System publishes StaffUnavailableDateRemoved domain event
- [ ] Operation requires authorization

### REQ-STF-016: Retrieve Staff Availability

**Requirement:** The system shall provide ability to query staff availability for scheduling purposes.

**Acceptance Criteria:**
- [ ] System returns all availability entries for a staff member
- [ ] System returns unavailable dates
- [ ] System calculates available time slots for date range
- [ ] Query completes within 100ms
- [ ] Results are sorted chronologically

### 2.3 Staff Assignment Requirements

### REQ-STF-017: Assign Staff to Event

**Requirement:** The system shall allow assignment of staff members to events with specific roles.

**Acceptance Criteria:**
- [ ] System accepts staff ID, event ID, assigned role, and optional notes
- [ ] System validates staff is active
- [ ] System validates staff has availability for event time
- [ ] System validates no double booking conflicts
- [ ] System generates unique StaffAssignmentId
- [ ] System sets initial status to Requested
- [ ] System publishes StaffAssignedToEvent domain event
- [ ] System records who made the assignment

### REQ-STF-018: Update Staff Assignment

**Requirement:** The system shall allow modification of existing staff assignments.

**Acceptance Criteria:**
- [ ] System allows updating assigned role and notes
- [ ] System validates changes don't create conflicts
- [ ] System publishes appropriate domain event
- [ ] System maintains assignment history
- [ ] Updates require authorization

### REQ-STF-019: Remove Staff Assignment

**Requirement:** The system shall support removal of staff assignments.

**Acceptance Criteria:**
- [ ] System changes status to Cancelled
- [ ] System accepts cancellation reason
- [ ] System publishes StaffUnassignedFromEvent domain event
- [ ] System notifies affected staff member
- [ ] Cancelled assignments remain in history

### REQ-STF-020: Confirm Staff Assignment

**Requirement:** The system shall allow staff members to confirm requested assignments.

**Acceptance Criteria:**
- [ ] System changes status from Requested to Confirmed
- [ ] System sets ConfirmedAt timestamp
- [ ] System publishes StaffBookingConfirmed domain event
- [ ] System validates assignment hasn't been cancelled
- [ ] Only the assigned staff member can confirm

### REQ-STF-021: Decline Staff Assignment

**Requirement:** The system shall allow staff members to decline requested assignments.

**Acceptance Criteria:**
- [ ] System changes status from Requested to Declined
- [ ] System accepts and stores decline reason
- [ ] System publishes StaffBookingDeclined domain event
- [ ] System notifies assignment creator
- [ ] Declined assignments can be reassigned

### REQ-STF-022: Find Available Staff for Event

**Requirement:** The system shall provide intelligent search for available staff matching event requirements.

**Acceptance Criteria:**
- [ ] System accepts event ID and optional role filter
- [ ] System filters staff by active status
- [ ] System checks availability for event date/time
- [ ] System excludes staff with conflicting assignments
- [ ] System excludes staff with unavailable dates
- [ ] System ranks results by rating and relevant skills
- [ ] Query completes within 200ms

### REQ-STF-023: Retrieve Staff Assignments

**Requirement:** The system shall provide querying of staff assignments with filtering.

**Acceptance Criteria:**
- [ ] System supports filtering by staff ID, event ID, status, and date range
- [ ] System supports pagination
- [ ] System returns assignment details with staff and event information
- [ ] System sorts by event date and assignment status
- [ ] Query supports performance requirements

### 2.4 Staff Check-in/Check-out Requirements

### REQ-STF-024: Check In Staff Member

**Requirement:** The system shall allow checking in staff when they arrive at an event.

**Acceptance Criteria:**
- [ ] System accepts assignment ID
- [ ] System validates assignment status is Confirmed
- [ ] System validates check-in time is within allowed window (1 hour before event start)
- [ ] System creates StaffCheckInOut record with check-in timestamp
- [ ] System records who performed the check-in
- [ ] System publishes StaffCheckedIn domain event
- [ ] System prevents duplicate check-ins

### REQ-STF-025: Check Out Staff Member

**Requirement:** The system shall allow checking out staff when they complete their work.

**Acceptance Criteria:**
- [ ] System accepts assignment ID
- [ ] System validates staff is checked in
- [ ] System sets CheckOutTime timestamp
- [ ] System calculates TotalHours worked
- [ ] System records who performed the check-out
- [ ] System publishes StaffCheckedOut domain event
- [ ] System validates check-out time is after check-in time

### REQ-STF-026: Mark Staff as No-Show

**Requirement:** The system shall support marking staff as no-show when they fail to arrive.

**Acceptance Criteria:**
- [ ] System changes assignment status to NoShow
- [ ] System validates staff was not checked in
- [ ] System validates event has started (30 minutes past start time)
- [ ] System publishes StaffNoShow domain event
- [ ] System updates staff reliability metrics
- [ ] Operation requires manager authorization

### REQ-STF-027: Retrieve Staff Timesheet

**Requirement:** The system shall generate timesheet reports showing staff work hours.

**Acceptance Criteria:**
- [ ] System accepts staff ID and date range
- [ ] System returns all check-in/out records for the period
- [ ] System calculates total hours worked
- [ ] System includes event details for each entry
- [ ] Report generation completes within 500ms
- [ ] Supports export to standard formats

### 2.5 Staff Performance Review Requirements

### REQ-STF-028: Record Staff Rating

**Requirement:** The system shall allow authorized users to record performance ratings for staff.

**Acceptance Criteria:**
- [ ] System accepts staff ID, rating (1-5), event ID (optional), and feedback
- [ ] System validates rating is within 1-5 range
- [ ] System validates reviewer cannot rate themselves
- [ ] System requires completed assignment for event-based reviews
- [ ] System publishes StaffRatingRecorded domain event
- [ ] System updates staff average rating

### REQ-STF-029: Submit Staff Feedback

**Requirement:** The system shall support submission of general feedback for staff members.

**Acceptance Criteria:**
- [ ] System accepts staff ID and feedback text
- [ ] System sets review type to Feedback
- [ ] System publishes StaffFeedbackReceived domain event
- [ ] System records timestamp and reviewer
- [ ] Feedback is visible to managers and admins

### REQ-STF-030: File Staff Complaint

**Requirement:** The system shall provide mechanism to file complaints against staff members.

**Acceptance Criteria:**
- [ ] System accepts staff ID and complaint details
- [ ] System sets review type to Complaint
- [ ] System publishes StaffComplaintReceived domain event
- [ ] System notifies management immediately
- [ ] Complaint tracking requires resolution workflow
- [ ] Only managers can mark complaints as resolved

### REQ-STF-031: Record Staff Compliment

**Requirement:** The system shall allow recording of compliments for staff members.

**Acceptance Criteria:**
- [ ] System accepts staff ID and compliment details
- [ ] System sets review type to Compliment
- [ ] System publishes StaffComplimentReceived domain event
- [ ] System notifies staff member
- [ ] Compliments contribute to performance metrics

### REQ-STF-032: Retrieve Staff Reviews

**Requirement:** The system shall provide access to performance review history.

**Acceptance Criteria:**
- [ ] System returns all reviews for a staff member
- [ ] System supports filtering by review type and date range
- [ ] System calculates ratings summary and distribution
- [ ] System includes recent feedback in response
- [ ] Query is optimized for performance

### REQ-STF-033: Generate Staff Ratings Summary

**Requirement:** The system shall calculate and provide staff performance metrics.

**Acceptance Criteria:**
- [ ] System calculates average rating across all reviews
- [ ] System provides rating distribution (count per rating 1-5)
- [ ] System returns total number of ratings
- [ ] System includes recent feedback
- [ ] Calculations update in real-time with new reviews
- [ ] Summary generation completes within 100ms

### 2.6 Double Booking Prevention Requirements

### REQ-STF-034: Detect Assignment Conflicts

**Requirement:** The system shall detect when staff assignment would create a scheduling conflict.

**Acceptance Criteria:**
- [ ] System checks for overlapping event times when creating assignment
- [ ] System compares against all confirmed and requested assignments
- [ ] System accounts for event setup and teardown times
- [ ] Detection completes within 150ms
- [ ] System identifies all conflicting assignments

### REQ-STF-035: Prevent Double Booking

**Requirement:** The system shall prevent creation of conflicting staff assignments.

**Acceptance Criteria:**
- [ ] System rejects assignment requests that create conflicts
- [ ] System returns 409 Conflict HTTP status
- [ ] System publishes StaffDoubleBookingPrevented domain event
- [ ] System provides details of conflicting assignments in error
- [ ] Prevention is enforced at database level with constraints

### REQ-STF-036: Validate Availability Before Assignment

**Requirement:** The system shall verify staff availability matches event requirements before assignment.

**Acceptance Criteria:**
- [ ] System checks staff has declared availability for event day/time
- [ ] System verifies staff is not marked unavailable for event date
- [ ] System validates staff status is Active
- [ ] All checks complete atomically before creating assignment
- [ ] Validation failures return clear error messages

### REQ-STF-037: Detect Double Booking from Availability Changes

**Requirement:** The system shall detect when availability changes would affect existing assignments.

**Acceptance Criteria:**
- [ ] System checks confirmed assignments when availability is removed
- [ ] System prevents removal if assignments would be affected
- [ ] System publishes StaffDoubleBookingDetected event if conflicts found
- [ ] System provides list of affected assignments
- [ ] Managers can override with explicit confirmation

### 2.7 Validation Requirements

### REQ-STF-038: Validate Staff Registration Input

**Requirement:** The system shall enforce validation rules for staff registration.

**Acceptance Criteria:**
- [ ] First name is required, 1-100 characters
- [ ] Last name is required, 1-100 characters
- [ ] Email is required, valid email format, unique across all staff
- [ ] Phone number is required, valid phone format
- [ ] Hire date is required, cannot be future date
- [ ] At least one role must be assigned
- [ ] Hourly rate must be >= 0 if provided
- [ ] Validation implemented using FluentValidation
- [ ] Validation errors return 400 Bad Request with details

### REQ-STF-039: Validate Availability Time Ranges

**Requirement:** The system shall validate availability time specifications.

**Acceptance Criteria:**
- [ ] End time must be after start time
- [ ] Times must be valid TimeSpan values
- [ ] Effective from date is required for all availability
- [ ] Cannot declare availability for past dates
- [ ] Recurring availability requires valid pattern

### REQ-STF-040: Validate Assignment Prerequisites

**Requirement:** The system shall validate all prerequisites before creating assignments.

**Acceptance Criteria:**
- [ ] Staff must exist and be active
- [ ] Event must exist and not be completed
- [ ] Assigned role must be valid StaffRole enum value
- [ ] Staff must have availability for event time
- [ ] No existing assignments conflict with event time
- [ ] Validation failures provide specific error messages

### REQ-STF-041: Validate Check-in/Check-out Rules

**Requirement:** The system shall enforce check-in and check-out business rules.

**Acceptance Criteria:**
- [ ] Check-in only allowed within 1 hour before event start
- [ ] Cannot check in if already checked in
- [ ] Check-out requires prior check-in
- [ ] Check-out time must be after check-in time
- [ ] No-show only marked if not checked in within 30 minutes of event start

### REQ-STF-042: Validate Performance Review Data

**Requirement:** The system shall validate performance review submissions.

**Acceptance Criteria:**
- [ ] Rating must be integer between 1 and 5 inclusive
- [ ] Staff member must exist
- [ ] Reviewer cannot be the same as staff being reviewed
- [ ] Event-based reviews require completed assignment
- [ ] Review type must be valid ReviewType enum value

### 2.8 Authorization Requirements

### REQ-STF-043: Enforce Role-Based Access Control

**Requirement:** The system shall implement role-based authorization for all staff operations.

**Acceptance Criteria:**
- [ ] All endpoints require JWT Bearer token authentication
- [ ] Staff role can view own profile, manage own availability, confirm/decline assignments
- [ ] Manager role can perform all staff operations, assign staff, check-in/out, view all staff
- [ ] Admin role has full access including termination and data deletion
- [ ] Customer role can view assigned staff for their events (read-only)
- [ ] Unauthorized access returns 401 Unauthorized
- [ ] Insufficient permissions return 403 Forbidden

### REQ-STF-044: Implement Resource-Based Authorization

**Requirement:** The system shall enforce ownership-based access control where applicable.

**Acceptance Criteria:**
- [ ] Staff can only modify their own profile (except protected fields)
- [ ] Staff can only manage their own availability
- [ ] Staff can only confirm/decline their own assignments
- [ ] Managers can override staff-level restrictions
- [ ] Authorization checks integrated with domain layer

### REQ-STF-045: Secure Sensitive Operations

**Requirement:** The system shall require elevated permissions for sensitive operations.

**Acceptance Criteria:**
- [ ] Staff termination requires Admin role
- [ ] Photo deletion requires Manager or Admin role
- [ ] Marking no-show requires Manager role
- [ ] Filing complaints accessible to all authenticated users
- [ ] Resolving complaints requires Manager role
- [ ] Profile deactivation requires Manager or Admin role

### 2.9 Azure Integration Requirements

### REQ-STF-046: Integrate Azure Blob Storage for Photos

**Requirement:** The system shall use Azure Blob Storage for staff photo management.

**Acceptance Criteria:**
- [ ] Photos uploaded to dedicated staff photos container
- [ ] Blob names use staff ID for organization
- [ ] Public read access configured for photo URLs
- [ ] Upload uses Azure Storage SDK
- [ ] Failed uploads handled with retry logic
- [ ] Storage connection configured via Azure App Configuration

### REQ-STF-047: Implement Azure AI Services for Feedback Analysis

**Requirement:** The system shall use Azure Cognitive Services to analyze performance feedback.

**Acceptance Criteria:**
- [ ] Feedback text analyzed for sentiment
- [ ] Sentiment scores stored with reviews
- [ ] Patterns identified across multiple reviews
- [ ] Negative sentiment triggers management alerts
- [ ] AI service failures don't block review submission
- [ ] Results cached for performance

### REQ-STF-048: Utilize Azure OpenAI for Staff Recommendations

**Requirement:** The system shall use Azure OpenAI to provide intelligent staff recommendations.

**Acceptance Criteria:**
- [ ] Recommendations based on skills, performance, and past assignments
- [ ] Model considers event type and requirements
- [ ] Recommendations include confidence scores
- [ ] Response time < 500ms for recommendation query
- [ ] Fallback to rule-based matching if AI unavailable
- [ ] Recommendations logged for quality monitoring

### REQ-STF-049: Deploy to Azure Infrastructure

**Requirement:** The system shall be deployed on Azure cloud infrastructure.

**Acceptance Criteria:**
- [ ] API hosted on Azure App Service
- [ ] Database hosted on Azure SQL Database
- [ ] Azure Service Bus used for event publishing
- [ ] Azure Application Insights configured for monitoring
- [ ] Azure Functions handle automated scheduling tasks
- [ ] All services configured with managed identities

### 2.10 Performance Requirements

### REQ-STF-050: Achieve API Response Time Targets

**Requirement:** The system shall meet specified response time requirements for all API endpoints.

**Acceptance Criteria:**
- [ ] 95th percentile response time < 200ms for all endpoints
- [ ] Staff list query supports 10,000+ staff with pagination
- [ ] Availability lookup completes < 100ms
- [ ] Double booking check completes < 150ms
- [ ] Performance monitoring via Application Insights
- [ ] Load testing validates performance under expected load

### REQ-STF-051: Optimize Photo Upload Performance

**Requirement:** The system shall handle photo uploads efficiently.

**Acceptance Criteria:**
- [ ] Supports files up to 5MB
- [ ] Upload completes within 3 seconds for maximum file size
- [ ] Parallel uploads supported
- [ ] Progress tracking available
- [ ] Automatic retry on transient failures
- [ ] Bandwidth throttling prevents system overload

### REQ-STF-052: Implement Database Optimization

**Requirement:** The system shall optimize database queries and schema for performance.

**Acceptance Criteria:**
- [ ] Indexes on frequently queried columns (email, status, role)
- [ ] Composite indexes for common filter combinations
- [ ] Query execution plans reviewed and optimized
- [ ] N+1 query problems eliminated
- [ ] Database connection pooling configured
- [ ] Slow query logging enabled

### REQ-STF-053: Support Scalability Requirements

**Requirement:** The system shall scale to support growing user base.

**Acceptance Criteria:**
- [ ] Architecture supports horizontal scaling
- [ ] Stateless API design enables load balancing
- [ ] Database supports read replicas for query scaling
- [ ] Caching strategy reduces database load
- [ ] System tested with 10x expected load
- [ ] Auto-scaling configured in Azure

### 2.11 Testing Requirements

### REQ-STF-054: Implement Comprehensive Unit Tests

**Requirement:** The system shall have thorough unit test coverage for all business logic.

**Acceptance Criteria:**
- [ ] All command handlers have unit tests
- [ ] All query handlers have unit tests
- [ ] Domain event generation tested
- [ ] Validation rules tested
- [ ] Double booking detection logic tested
- [ ] Test coverage minimum 80%
- [ ] Business rules have 100% coverage
- [ ] Tests follow AAA (Arrange, Act, Assert) pattern

### REQ-STF-055: Implement Integration Tests

**Requirement:** The system shall have integration tests for API endpoints and external dependencies.

**Acceptance Criteria:**
- [ ] All API endpoints tested
- [ ] Database operations tested with test database
- [ ] Event publishing tested
- [ ] Azure Blob Storage upload tested with emulator
- [ ] Availability calculations tested
- [ ] Authentication and authorization tested
- [ ] Integration tests run in CI/CD pipeline

### REQ-STF-056: Achieve Test Coverage Targets

**Requirement:** The system shall meet minimum test coverage requirements.

**Acceptance Criteria:**
- [ ] Overall code coverage minimum 80%
- [ ] Business rules coverage 100%
- [ ] Double booking prevention coverage 100%
- [ ] Critical paths covered by integration tests
- [ ] Coverage reports generated automatically
- [ ] Coverage metrics tracked over time
- [ ] Pull requests blocked if coverage decreases

### REQ-STF-057: Implement Performance Tests

**Requirement:** The system shall have automated performance testing.

**Acceptance Criteria:**
- [ ] Load tests simulate expected user load
- [ ] Stress tests identify breaking points
- [ ] Endurance tests validate stability over time
- [ ] Performance benchmarks established
- [ ] Automated alerts for performance regressions
- [ ] Results tracked in performance dashboard

---

## 3. Domain Model

### 3.1 Aggregate: StaffMember
The StaffMember aggregate is the central entity in this bounded context.

#### 3.1.1 Entity Properties
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

#### 3.1.2 StaffStatus Enumeration
```csharp
public enum StaffStatus
{
    Active = 0,
    Inactive = 1,
    OnLeave = 2,
    Terminated = 3
}
```

#### 3.1.3 StaffRole Enumeration
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

### 3.2 Entity: StaffAvailability
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

### 3.3 Entity: StaffUnavailableDate
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| StaffUnavailableDateId | Guid | Yes | Unique identifier |
| StaffMemberId | Guid | Yes | Parent staff member reference |
| Date | DateTime | Yes | Unavailable date |
| Reason | string | No | Reason for unavailability |
| IsAllDay | bool | Yes | Whether unavailable all day |
| StartTime | TimeSpan | No | Start time if not all day |
| EndTime | TimeSpan | No | End time if not all day |

### 3.4 Entity: StaffAssignment
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

#### 3.4.1 AssignmentStatus Enumeration
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

### 3.5 Entity: StaffCheckInOut
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| StaffCheckInOutId | Guid | Yes | Unique identifier |
| StaffAssignmentId | Guid | Yes | Assignment reference |
| CheckInTime | DateTime | Yes | Check-in timestamp |
| CheckOutTime | DateTime | No | Check-out timestamp |
| CheckedInBy | Guid | No | Who performed check-in |
| CheckedOutBy | Guid | No | Who performed check-out |
| TotalHours | decimal | No | Calculated hours worked |

### 3.6 Entity: StaffPerformanceReview
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

#### 3.6.1 ReviewType Enumeration
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

## 5. Appendices

### 5.1 Related Documents
- [Frontend Specification](./frontend-spec.md)
- [Use Case Diagram](./diagrams/use-case.drawio)
- [C4 Diagrams](./diagrams/c4-context.drawio)
- [Class Diagram](./plantuml/class-diagram.plantuml)
- [Sequence Diagrams](./plantuml/sequence-diagram.plantuml)

### 5.2 Revision History
| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-22 | System | Initial specification with requirements format |
