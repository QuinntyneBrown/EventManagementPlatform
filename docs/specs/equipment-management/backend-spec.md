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

## 2. Equipment CRUD Requirements

### REQ-EQP-001: Create Equipment Item

**Requirement:** The system shall allow authorized users to create new equipment items with all required properties.

**Acceptance Criteria:**
- [ ] Equipment can be created with name, category, purchase date, purchase price, and initial condition
- [ ] System assigns unique EquipmentItemId (Guid)
- [ ] System sets IsActive to true by default
- [ ] System sets initial Status to Available
- [ ] System records CreatedAt timestamp and CreatedBy user
- [ ] EquipmentItemAdded domain event is published
- [ ] API returns 201 Created with equipment details

### REQ-EQP-002: Update Equipment Item

**Requirement:** The system shall allow authorized users to update equipment item properties.

**Acceptance Criteria:**
- [ ] Name, description, manufacturer, model, serial number, and warehouse location can be updated
- [ ] Current value can be updated
- [ ] System records ModifiedAt timestamp and ModifiedBy user
- [ ] EquipmentItemUpdated domain event is published with changed properties
- [ ] API returns 200 OK with updated equipment details
- [ ] Equipment name uniqueness within category is validated

### REQ-EQP-003: Retrieve Equipment Item by ID

**Requirement:** The system shall allow users to retrieve a single equipment item by its unique identifier.

**Acceptance Criteria:**
- [ ] System retrieves equipment by EquipmentItemId
- [ ] Response includes all equipment properties, photos, and specifications
- [ ] API returns 200 OK with equipment details
- [ ] API returns 404 Not Found if equipment doesn't exist
- [ ] Soft-deleted equipment is not returned

### REQ-EQP-004: List Equipment Items

**Requirement:** The system shall allow users to retrieve a paginated list of equipment items with filtering and sorting.

**Acceptance Criteria:**
- [ ] System supports pagination with configurable page size
- [ ] System supports filtering by category, status, condition, and active status
- [ ] System supports sorting by name, purchase date, condition, and current value
- [ ] System supports full-text search by name, manufacturer, and model
- [ ] API returns 200 OK with paginated result including total count
- [ ] Only active equipment is returned by default unless explicitly requested

### REQ-EQP-005: Soft Delete Equipment Item

**Requirement:** The system shall allow authorized users to soft delete equipment items that have no active reservations.

**Acceptance Criteria:**
- [ ] System checks for active reservations before deletion
- [ ] System sets IsActive to false instead of hard deletion
- [ ] EquipmentItemRemoved domain event is published
- [ ] API returns 204 No Content on success
- [ ] API returns 409 Conflict if active reservations exist
- [ ] API returns 403 Forbidden if user lacks permission

### REQ-EQP-006: Activate Equipment Item

**Requirement:** The system shall allow authorized users to activate deactivated equipment items.

**Acceptance Criteria:**
- [ ] System sets IsActive to true
- [ ] EquipmentItemActivated domain event is published
- [ ] API returns 200 OK on success
- [ ] API returns 404 Not Found if equipment doesn't exist
- [ ] Equipment becomes available for reservations

### REQ-EQP-007: Deactivate Equipment Item

**Requirement:** The system shall allow authorized users to deactivate equipment items.

**Acceptance Criteria:**
- [ ] System sets IsActive to false
- [ ] EquipmentItemDeactivated domain event is published
- [ ] API returns 200 OK on success
- [ ] API returns 404 Not Found if equipment doesn't exist
- [ ] Deactivated equipment cannot be reserved

---

## 3. Equipment Category Requirements

### REQ-EQP-010: Equipment Categories

**Requirement:** The system shall support predefined equipment categories.

**Acceptance Criteria:**
- [ ] System supports Table, Game, AudioVisual, Decoration, Seating, Lighting, and Other categories
- [ ] Category is required when creating equipment
- [ ] Category is stored as enum value
- [ ] API validates category against allowed values
- [ ] API returns 400 Bad Request for invalid categories

### REQ-EQP-011: Filter by Category

**Requirement:** The system shall allow users to filter equipment by category.

**Acceptance Criteria:**
- [ ] API endpoint accepts category parameter
- [ ] System returns only equipment matching the specified category
- [ ] Multiple categories can be specified
- [ ] API returns 200 OK with filtered results

---

## 4. Equipment Booking Requirements

### REQ-EQP-020: Create Equipment Reservation

**Requirement:** The system shall allow users to create equipment reservations for events.

**Acceptance Criteria:**
- [ ] Reservation can be created with equipment ID, event ID, quantity, start date, end date, and optional notes
- [ ] System assigns unique ReservationId (Guid)
- [ ] System sets initial status to Requested
- [ ] System records CreatedAt timestamp and CreatedBy user
- [ ] EquipmentBookingRequested domain event is published
- [ ] API returns 201 Created with reservation details

### REQ-EQP-021: Update Equipment Reservation

**Requirement:** The system shall allow users to update equipment reservations.

**Acceptance Criteria:**
- [ ] Start date, end date, quantity, notes, and status can be updated
- [ ] System records ModifiedAt timestamp and ModifiedBy user
- [ ] EquipmentReservationUpdated domain event is published
- [ ] API returns 200 OK with updated reservation
- [ ] API returns 404 Not Found if reservation doesn't exist
- [ ] Availability is re-checked when dates are updated

### REQ-EQP-022: Cancel Equipment Reservation

**Requirement:** The system shall allow users to cancel equipment reservations.

**Acceptance Criteria:**
- [ ] System sets reservation status to Cancelled
- [ ] System frees up equipment availability
- [ ] EquipmentReservationCancelled domain event is published
- [ ] API returns 204 No Content on success
- [ ] Cancellation within 48 hours of event requires manager approval
- [ ] API returns 403 Forbidden if user lacks approval rights

### REQ-EQP-023: Confirm Equipment Reservation

**Requirement:** The system shall allow authorized users to confirm requested reservations.

**Acceptance Criteria:**
- [ ] System changes reservation status from Requested to Confirmed
- [ ] System updates equipment status to Reserved
- [ ] EquipmentReservedForEvent domain event is published
- [ ] API returns 200 OK on success
- [ ] Only users with Staff role or higher can confirm

### REQ-EQP-024: List Equipment Reservations

**Requirement:** The system shall allow users to retrieve equipment reservations with filtering.

**Acceptance Criteria:**
- [ ] System supports filtering by equipment ID, event ID, date range, and status
- [ ] System supports pagination
- [ ] API returns 200 OK with paginated reservation list
- [ ] Each reservation includes equipment name and event title
- [ ] Cancelled reservations are excluded by default

---

## 5. Equipment Availability Requirements

### REQ-EQP-030: Check Equipment Availability

**Requirement:** The system shall allow users to check equipment availability for a specific date range.

**Acceptance Criteria:**
- [ ] System checks for overlapping confirmed reservations
- [ ] System returns availability status (true/false)
- [ ] System returns list of conflicting reservations if unavailable
- [ ] System returns list of alternative equipment suggestions
- [ ] EquipmentAvailabilityChecked domain event is published
- [ ] API returns 200 OK with availability result
- [ ] Response time is under 150ms

### REQ-EQP-031: Prevent Double Booking

**Requirement:** The system shall prevent double booking of equipment unless explicitly overridden.

**Acceptance Criteria:**
- [ ] System detects overlapping reservation periods
- [ ] EquipmentDoubleBookingDetected event is published when conflict found
- [ ] EquipmentDoubleBookingPrevented event is published when reservation blocked
- [ ] API returns 409 Conflict with details of conflicting reservations
- [ ] Conflict check is performed before creating or updating reservations
- [ ] Equipment in InMaintenance or OutOfService status cannot be reserved

### REQ-EQP-032: Override Double Booking

**Requirement:** The system shall allow managers to override double booking conflicts with justification.

**Acceptance Criteria:**
- [ ] Only users with Manager role can override conflicts
- [ ] Override requires a reason/justification
- [ ] EquipmentDoubleBookingOverridden event is published with reason
- [ ] System records who overrode and why
- [ ] API returns 200 OK after successful override
- [ ] API returns 403 Forbidden for non-managers

### REQ-EQP-033: Suggest Alternative Equipment

**Requirement:** The system shall suggest alternative equipment when requested equipment is unavailable.

**Acceptance Criteria:**
- [ ] System finds equipment in the same category
- [ ] System checks availability of alternatives for the same period
- [ ] System prioritizes equipment with similar specifications
- [ ] EquipmentAlternativeSuggested event is published
- [ ] API returns list of available alternatives with details
- [ ] Response includes why each alternative is suggested

---

## 6. Equipment Logistics Requirements

### REQ-EQP-040: Track Equipment Packing

**Requirement:** The system shall allow users to mark equipment as packed for shipment.

**Acceptance Criteria:**
- [ ] System records PackedAt timestamp
- [ ] System records user who packed
- [ ] EquipmentPackedForShipment domain event is published
- [ ] API returns 200 OK on success
- [ ] API returns 404 Not Found if reservation doesn't exist
- [ ] Equipment must be in Reserved status

### REQ-EQP-041: Track Equipment Loading

**Requirement:** The system shall allow users to mark equipment as loaded on delivery truck.

**Acceptance Criteria:**
- [ ] System records LoadedAt timestamp
- [ ] System records truck ID and user who loaded
- [ ] Equipment must be packed before loading
- [ ] EquipmentLoadedOnTruck domain event is published
- [ ] API returns 200 OK on success
- [ ] API returns 400 Bad Request if not yet packed

### REQ-EQP-042: Track Equipment Dispatch

**Requirement:** The system shall allow users to mark equipment as dispatched to event venue.

**Acceptance Criteria:**
- [ ] System records DispatchedAt timestamp
- [ ] System records driver ID
- [ ] Equipment must be loaded before dispatch
- [ ] System updates equipment status to InTransit
- [ ] EquipmentDispatchedToEvent domain event is published
- [ ] API returns 200 OK on success

### REQ-EQP-043: Track Equipment Delivery

**Requirement:** The system shall allow users to mark equipment as delivered to venue.

**Acceptance Criteria:**
- [ ] System records DeliveredAt timestamp
- [ ] Equipment must be dispatched before delivery
- [ ] System updates equipment status to AtVenue
- [ ] EquipmentDeliveredToVenue domain event is published
- [ ] API returns 200 OK on success

### REQ-EQP-044: Track Equipment Setup Completion

**Requirement:** The system shall allow users to mark equipment setup as complete.

**Acceptance Criteria:**
- [ ] System records SetupCompletedAt timestamp
- [ ] Equipment must be delivered before setup completion
- [ ] EquipmentSetupCompleted domain event is published
- [ ] API returns 200 OK on success

### REQ-EQP-045: Track Equipment Pickup

**Requirement:** The system shall allow users to mark equipment as picked up from venue.

**Acceptance Criteria:**
- [ ] System records PickedUpAt timestamp
- [ ] Equipment must be at venue before pickup
- [ ] System updates equipment status to InTransit
- [ ] EquipmentPickedUpFromVenue domain event is published
- [ ] API returns 200 OK on success

### REQ-EQP-046: Track Equipment Return

**Requirement:** The system shall allow users to mark equipment as returned to warehouse.

**Acceptance Criteria:**
- [ ] System records ReturnedAt timestamp
- [ ] Equipment must be picked up before return
- [ ] System updates equipment status to Available
- [ ] System updates reservation status to Fulfilled
- [ ] EquipmentReturnedToWarehouse domain event is published
- [ ] API returns 200 OK on success

### REQ-EQP-047: Retrieve In-Transit Equipment

**Requirement:** The system shall allow users to retrieve all equipment currently in transit.

**Acceptance Criteria:**
- [ ] System returns equipment with InTransit status
- [ ] Response includes current logistics stage
- [ ] API returns 200 OK with list of in-transit equipment
- [ ] List includes dispatched but not yet delivered equipment
- [ ] List includes picked up but not yet returned equipment

### REQ-EQP-048: Enforce Logistics Sequence

**Requirement:** The system shall enforce the correct sequence of logistics stages.

**Acceptance Criteria:**
- [ ] Equipment must be packed before loading
- [ ] Equipment must be loaded before dispatch
- [ ] Equipment must be dispatched before delivery
- [ ] Equipment must be delivered before setup completion
- [ ] Equipment must be picked up before return
- [ ] Logistics steps cannot be reversed
- [ ] API returns 400 Bad Request if sequence is violated

---

## 7. Equipment Maintenance Requirements

### REQ-EQP-050: Schedule Equipment Maintenance

**Requirement:** The system shall allow users to schedule maintenance for equipment.

**Acceptance Criteria:**
- [ ] Maintenance can be scheduled with type, scheduled date, and description
- [ ] System assigns unique MaintenanceId (Guid)
- [ ] System sets initial status to Scheduled
- [ ] Maintenance types include Inspection, PreventiveMaintenance, Repair, Cleaning, and Replacement
- [ ] EquipmentMaintenanceScheduled domain event is published
- [ ] API returns 201 Created with maintenance details

### REQ-EQP-051: Start Equipment Maintenance

**Requirement:** The system shall allow technicians to start scheduled maintenance.

**Acceptance Criteria:**
- [ ] System records StartedAt timestamp
- [ ] System records assigned technician ID
- [ ] System updates maintenance status to InProgress
- [ ] System updates equipment status to InMaintenance
- [ ] EquipmentMaintenanceStarted domain event is published
- [ ] API returns 200 OK on success

### REQ-EQP-052: Complete Equipment Maintenance

**Requirement:** The system shall allow technicians to complete maintenance work.

**Acceptance Criteria:**
- [ ] System records CompletedAt timestamp
- [ ] System records maintenance cost
- [ ] System updates maintenance status to Completed
- [ ] System returns equipment status to Available (if condition allows)
- [ ] EquipmentMaintenanceCompleted domain event is published
- [ ] API returns 200 OK on success

### REQ-EQP-053: Retrieve Maintenance History

**Requirement:** The system shall allow users to retrieve maintenance history for equipment.

**Acceptance Criteria:**
- [ ] System returns all maintenance records for specified equipment
- [ ] Records are sorted by scheduled date (most recent first)
- [ ] Response includes maintenance type, status, dates, cost, and technician
- [ ] API returns 200 OK with maintenance history
- [ ] API returns 404 Not Found if equipment doesn't exist

### REQ-EQP-054: Retrieve Scheduled Maintenance

**Requirement:** The system shall allow users to retrieve upcoming scheduled maintenance.

**Acceptance Criteria:**
- [ ] System returns maintenance with Scheduled status
- [ ] System supports filtering by equipment, date range, and maintenance type
- [ ] System highlights overdue maintenance (scheduled date in past)
- [ ] API returns 200 OK with list of scheduled maintenance
- [ ] Results are sorted by scheduled date (earliest first)

### REQ-EQP-055: Enforce Preventive Maintenance Schedule

**Requirement:** The system shall require preventive maintenance every 6 months.

**Acceptance Criteria:**
- [ ] System calculates next preventive maintenance due date
- [ ] System flags equipment overdue for preventive maintenance
- [ ] System prevents reservation of equipment with overdue preventive maintenance
- [ ] Automated notifications are sent for upcoming preventive maintenance
- [ ] Business rule EQP-030 is enforced

### REQ-EQP-056: Require Post-Event Inspection

**Requirement:** The system shall require inspection after every event usage.

**Acceptance Criteria:**
- [ ] System automatically schedules inspection when equipment is returned
- [ ] Inspection type is set to Inspection
- [ ] Equipment cannot be reserved again until inspection is complete
- [ ] EquipmentInspectionScheduled event is published
- [ ] Business rule EQP-032 is enforced

---

## 8. Equipment History Requirements

### REQ-EQP-060: Report Equipment Damage

**Requirement:** The system shall allow users to report equipment damage.

**Acceptance Criteria:**
- [ ] Damage report includes equipment ID, severity, description, and optional event ID
- [ ] System assigns unique DamageReportId (Guid)
- [ ] System records ReportedAt timestamp and ReportedBy user
- [ ] Severity levels include Minor, Moderate, Severe, and TotalLoss
- [ ] Photo URLs can be attached to damage report
- [ ] EquipmentDamageReported domain event is published
- [ ] API returns 201 Created with damage report details

### REQ-EQP-061: Update Equipment Condition

**Requirement:** The system shall allow authorized users to update equipment condition.

**Acceptance Criteria:**
- [ ] Condition can be set to Excellent, Good, Fair, Poor, NeedsRepair, or OutOfService
- [ ] System publishes EquipmentConditionDowngraded event when condition worsens
- [ ] System publishes EquipmentConditionUpgraded event when condition improves
- [ ] Equipment with NeedsRepair or OutOfService cannot be reserved
- [ ] API returns 200 OK with updated equipment details

### REQ-EQP-062: Trigger Automatic Retirement

**Requirement:** The system shall automatically trigger retirement for total loss damage.

**Acceptance Criteria:**
- [ ] When damage severity is TotalLoss, system triggers retirement workflow
- [ ] Equipment status is set to Retired
- [ ] EquipmentReplacementRequired event is published
- [ ] Business rule EQP-034 is enforced
- [ ] Retired equipment cannot be reserved

### REQ-EQP-063: Retire Equipment

**Requirement:** The system shall allow managers to manually retire equipment.

**Acceptance Criteria:**
- [ ] Only users with Manager role can retire equipment
- [ ] Retirement requires a reason
- [ ] System updates equipment status to Retired
- [ ] System sets IsActive to false
- [ ] EquipmentRetired domain event is published with reason
- [ ] API returns 200 OK on success
- [ ] API returns 403 Forbidden for non-managers

### REQ-EQP-064: Suggest Replacement

**Requirement:** The system shall suggest replacement when repair cost exceeds 50% of current value.

**Acceptance Criteria:**
- [ ] System calculates ratio of estimated repair cost to current value
- [ ] When ratio exceeds 50%, EquipmentReplacementRequired event is published
- [ ] Notification is sent to manager
- [ ] Business rule EQP-035 is enforced
- [ ] Suggestion includes cost analysis

---

## 9. Equipment Photo Requirements

### REQ-EQP-070: Upload Equipment Photo

**Requirement:** The system shall allow users to upload photos to Azure Blob Storage for equipment.

**Acceptance Criteria:**
- [ ] System accepts JPG, PNG, and WEBP formats
- [ ] Maximum file size is 10MB
- [ ] System uploads to Azure Blob Storage
- [ ] System generates thumbnail
- [ ] System stores BlobUrl and ThumbnailUrl
- [ ] EquipmentPhotoUploaded domain event is published
- [ ] API returns 201 Created with photo details
- [ ] Upload completes in under 5 seconds for 10MB file

### REQ-EQP-071: Delete Equipment Photo

**Requirement:** The system shall allow users to delete equipment photos.

**Acceptance Criteria:**
- [ ] System deletes photo from Azure Blob Storage
- [ ] System deletes thumbnail from Azure Blob Storage
- [ ] System removes photo record from database
- [ ] If photo was primary, another photo is set as primary (if available)
- [ ] API returns 204 No Content on success
- [ ] API returns 404 Not Found if photo doesn't exist

### REQ-EQP-072: Set Primary Photo

**Requirement:** The system shall allow users to set a primary photo for equipment.

**Acceptance Criteria:**
- [ ] Only one photo can be primary per equipment
- [ ] System sets IsPrimary to true for selected photo
- [ ] System sets IsPrimary to false for all other photos
- [ ] API returns 200 OK on success
- [ ] Primary photo is displayed first in galleries

---

## 10. Equipment Specifications Requirements

### REQ-EQP-080: Update Equipment Specifications

**Requirement:** The system shall allow users to add and update equipment specifications.

**Acceptance Criteria:**
- [ ] Specifications are key-value pairs with optional unit
- [ ] Multiple specifications can be updated in one operation
- [ ] Existing specifications are replaced with new values
- [ ] EquipmentSpecificationsUpdated domain event is published
- [ ] API returns 200 OK on success
- [ ] Common specifications include Dimensions, Weight, Color, Material, Capacity

---

## 11. Validation Requirements

### REQ-EQP-090: Validate Equipment Name

**Requirement:** The system shall validate equipment name uniqueness within category.

**Acceptance Criteria:**
- [ ] Name is required and must be 1-200 characters
- [ ] Name must be unique within the same category
- [ ] Validation is performed on create and update
- [ ] API returns 400 Bad Request with validation error if duplicate
- [ ] Business rule EQP-001 is enforced

### REQ-EQP-091: Validate Purchase Information

**Requirement:** The system shall validate equipment purchase information.

**Acceptance Criteria:**
- [ ] Purchase date is required and cannot be in the future
- [ ] Purchase price is required and must be greater than 0
- [ ] Current value cannot exceed purchase price by more than 20%
- [ ] API returns 400 Bad Request with validation errors
- [ ] Business rules EQP-003 and EQP-004 are enforced

### REQ-EQP-092: Validate Reservation Dates

**Requirement:** The system shall validate reservation date ranges.

**Acceptance Criteria:**
- [ ] Start date is required and must be before end date
- [ ] Reservation period must be at least 1 hour
- [ ] End date must be after start date plus 1 hour minimum
- [ ] API returns 400 Bad Request with validation errors
- [ ] Business rules EQP-010 and EQP-011 are enforced

### REQ-EQP-093: Validate Reservation Quantity

**Requirement:** The system shall validate reservation quantity.

**Acceptance Criteria:**
- [ ] Quantity is required and must be greater than 0
- [ ] Quantity must not exceed available inventory
- [ ] API returns 400 Bad Request if validation fails

### REQ-EQP-094: Validate Equipment Status Transitions

**Requirement:** The system shall validate equipment status transitions.

**Acceptance Criteria:**
- [ ] Equipment cannot be deleted if active reservations exist (EQP-002)
- [ ] Deactivated equipment cannot be reserved (EQP-005)
- [ ] Equipment in OutOfService condition cannot be reserved (EQP-006)
- [ ] Equipment must be Available or Reserved to create reservation (EQP-013)
- [ ] Equipment with NeedsRepair condition cannot be reserved (EQP-031)
- [ ] API returns 409 Conflict for invalid state transitions

---

## 12. Authorization Requirements

### REQ-EQP-100: Authenticate API Requests

**Requirement:** The system shall require JWT Bearer token authentication for all API endpoints.

**Acceptance Criteria:**
- [ ] All endpoints require valid JWT token
- [ ] Tokens are issued by Azure AD B2C or internal identity service
- [ ] API returns 401 Unauthorized if token is missing or invalid
- [ ] Token includes user ID and roles

### REQ-EQP-101: Authorize Viewer Role

**Requirement:** The system shall allow Viewer role to view equipment and reservations.

**Acceptance Criteria:**
- [ ] Viewers can call GET endpoints for equipment and reservations
- [ ] Viewers cannot create, update, or delete resources
- [ ] API returns 403 Forbidden for unauthorized operations

### REQ-EQP-102: Authorize Staff Role

**Requirement:** The system shall allow Staff role to create reservations and update logistics.

**Acceptance Criteria:**
- [ ] Staff can create and update reservations
- [ ] Staff can update logistics status
- [ ] Staff can report damage
- [ ] Staff cannot manage equipment or override conflicts
- [ ] API returns 403 Forbidden for unauthorized operations

### REQ-EQP-103: Authorize Warehouse Manager Role

**Requirement:** The system shall allow Warehouse Manager role to manage equipment.

**Acceptance Criteria:**
- [ ] Warehouse Managers can create, update, and deactivate equipment
- [ ] Warehouse Managers can upload photos
- [ ] Warehouse Managers can update specifications
- [ ] Warehouse Managers can report damage
- [ ] Warehouse Managers cannot retire equipment or override conflicts
- [ ] API returns 403 Forbidden for unauthorized operations

### REQ-EQP-104: Authorize Maintenance Technician Role

**Requirement:** The system shall allow Maintenance Technician role to manage maintenance.

**Acceptance Criteria:**
- [ ] Technicians can schedule, start, and complete maintenance
- [ ] Technicians can update equipment condition
- [ ] Technicians can view maintenance history
- [ ] Technicians cannot manage equipment or reservations
- [ ] API returns 403 Forbidden for unauthorized operations

### REQ-EQP-105: Authorize Manager Role

**Requirement:** The system shall allow Manager role to override conflicts and approve cancellations.

**Acceptance Criteria:**
- [ ] Managers can override double booking conflicts
- [ ] Managers can approve late cancellations (within 48 hours of event)
- [ ] Managers can retire equipment
- [ ] Managers can perform all Staff and Warehouse Manager operations
- [ ] API returns 403 Forbidden for unauthorized operations

### REQ-EQP-106: Authorize Admin Role

**Requirement:** The system shall allow Admin role full access to all operations.

**Acceptance Criteria:**
- [ ] Admins can perform all operations including hard deletion
- [ ] Admins can manage all resources regardless of creator
- [ ] No operations return 403 Forbidden for Admin role

---

## 13. Azure Integration Requirements

### REQ-EQP-110: Store Photos in Azure Blob Storage

**Requirement:** The system shall store equipment photos in Azure Blob Storage.

**Acceptance Criteria:**
- [ ] Photos are uploaded to dedicated container
- [ ] System generates SAS tokens for secure access
- [ ] Thumbnails are generated and stored
- [ ] Storage connection uses Azure Key Vault for credentials
- [ ] Failed uploads are retried up to 3 times

### REQ-EQP-111: Publish Events to Azure Service Bus

**Requirement:** The system shall publish domain events to Azure Service Bus for integration.

**Acceptance Criteria:**
- [ ] All domain events are published to Service Bus topic
- [ ] Events include correlation ID for tracing
- [ ] Event schema includes event type, timestamp, and payload
- [ ] Failed publishes are retried with exponential backoff
- [ ] Service Bus connection uses Azure Key Vault for credentials

### REQ-EQP-112: Log Telemetry to Application Insights

**Requirement:** The system shall log telemetry and errors to Azure Application Insights.

**Acceptance Criteria:**
- [ ] All API requests are logged with duration
- [ ] Exceptions are logged with stack traces
- [ ] Custom events are logged for business operations
- [ ] Performance counters are tracked
- [ ] Application Insights connection uses instrumentation key from Key Vault

### REQ-EQP-113: Use Azure AI for Damage Detection

**Requirement:** The system shall use Azure Computer Vision to analyze damage photos.

**Acceptance Criteria:**
- [ ] Uploaded damage photos are sent to Computer Vision API
- [ ] System extracts damage indicators (scratches, dents, breaks)
- [ ] AI confidence score is stored with damage report
- [ ] Analysis results help set initial damage severity
- [ ] Failed AI calls do not block damage report creation

### REQ-EQP-114: Use Azure ML for Predictive Maintenance

**Requirement:** The system shall use Azure Machine Learning to predict maintenance needs.

**Acceptance Criteria:**
- [ ] Equipment usage patterns are sent to ML model
- [ ] Model predicts optimal maintenance scheduling
- [ ] Predictions are stored and surfaced to maintenance technicians
- [ ] Model is retrained periodically with actual maintenance data
- [ ] Failed ML calls do not block normal operations

### REQ-EQP-115: Use Azure OpenAI for Equipment Recommendations

**Requirement:** The system shall use Azure OpenAI to suggest alternative equipment.

**Acceptance Criteria:**
- [ ] When equipment is unavailable, system queries OpenAI for smart suggestions
- [ ] Context includes equipment specifications, category, and event details
- [ ] OpenAI considers similarity in functionality and specifications
- [ ] Suggestions are ranked by relevance
- [ ] Fallback to basic category matching if OpenAI is unavailable

---

## 14. Performance Requirements

### REQ-EQP-120: API Response Time

**Requirement:** The system shall respond to API requests within performance thresholds.

**Acceptance Criteria:**
- [ ] 95th percentile response time is under 200ms
- [ ] Equipment search completes in under 100ms for 10,000+ items
- [ ] Availability check completes in under 150ms including conflict detection
- [ ] Photo upload completes in under 5 seconds for 10MB files
- [ ] Performance is measured and logged to Application Insights

### REQ-EQP-121: Concurrent Users

**Requirement:** The system shall support concurrent user operations.

**Acceptance Criteria:**
- [ ] System supports 100 concurrent users without degradation
- [ ] Database connection pooling is configured appropriately
- [ ] API rate limiting prevents abuse
- [ ] Load testing validates concurrent user support

### REQ-EQP-122: Database Query Optimization

**Requirement:** The system shall optimize database queries for performance.

**Acceptance Criteria:**
- [ ] Indexes are created on frequently queried columns
- [ ] Complex queries use appropriate joins and projections
- [ ] N+1 query problems are avoided using eager loading
- [ ] Query execution plans are reviewed and optimized
- [ ] Entity Framework Core is configured for optimal performance

---

## 15. Testing Requirements

### REQ-EQP-130: Unit Test Coverage

**Requirement:** The system shall have comprehensive unit test coverage.

**Acceptance Criteria:**
- [ ] All command handlers have unit tests
- [ ] All query handlers have unit tests
- [ ] All domain event generation is tested
- [ ] All validation rules are tested
- [ ] All business rule enforcement is tested
- [ ] Minimum 80% code coverage overall
- [ ] 100% coverage for business rules
- [ ] 100% coverage for availability checking logic

### REQ-EQP-131: Integration Test Coverage

**Requirement:** The system shall have integration tests for critical paths.

**Acceptance Criteria:**
- [ ] All API endpoints have integration tests
- [ ] Database operations are tested against real database
- [ ] Azure Blob Storage integration is tested
- [ ] Event publishing is tested
- [ ] Double booking prevention is tested end-to-end
- [ ] Authentication and authorization are tested

### REQ-EQP-132: Test Data Management

**Requirement:** The system shall support test data setup and teardown.

**Acceptance Criteria:**
- [ ] Test database is seeded with representative data
- [ ] Each test runs in isolation with clean state
- [ ] Test data includes edge cases and boundary conditions
- [ ] Cleanup is performed after test execution

---

## 16. Error Handling Requirements

### REQ-EQP-140: Return Standardized Error Responses

**Requirement:** The system shall return RFC 7231 compliant error responses.

**Acceptance Criteria:**
- [ ] Error responses include type, title, status, detail, and instance
- [ ] Validation errors include field-level error details
- [ ] Error format matches Problem Details specification
- [ ] Status codes are semantically correct (400, 401, 403, 404, 409, 500)

### REQ-EQP-141: Handle Validation Exceptions

**Requirement:** The system shall handle FluentValidation exceptions gracefully.

**Acceptance Criteria:**
- [ ] ValidationException returns 400 Bad Request
- [ ] Response includes all validation errors with field names
- [ ] Validation runs before command execution
- [ ] Multiple validation errors are returned together

### REQ-EQP-142: Handle Not Found Exceptions

**Requirement:** The system shall handle entity not found scenarios.

**Acceptance Criteria:**
- [ ] NotFoundException returns 404 Not Found
- [ ] Response includes which entity type and ID was not found
- [ ] Clear error message guides user

### REQ-EQP-143: Handle Conflict Exceptions

**Requirement:** The system shall handle business rule conflicts.

**Acceptance Criteria:**
- [ ] ConflictException returns 409 Conflict
- [ ] Double booking conflicts include details of conflicting reservations
- [ ] Status transition conflicts explain why transition is invalid
- [ ] Suggested resolutions are included when available

### REQ-EQP-144: Handle Storage Exceptions

**Requirement:** The system shall handle Azure Blob Storage errors.

**Acceptance Criteria:**
- [ ] StorageException returns 500 Internal Server Error
- [ ] Transient errors are retried automatically
- [ ] Error is logged to Application Insights
- [ ] User-friendly error message is returned

---

## 17. Domain Model

### 17.1 EquipmentItem Aggregate
The EquipmentItem aggregate is the central entity managing equipment lifecycle.

#### Properties
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

#### Enumerations
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

public enum EquipmentCondition
{
    Excellent = 0,
    Good = 1,
    Fair = 2,
    Poor = 3,
    NeedsRepair = 4,
    OutOfService = 5
}

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

### 17.2 Related Entities

#### EquipmentSpecification
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| SpecificationId | Guid | Yes | Unique identifier |
| EquipmentItemId | Guid | Yes | Parent equipment reference |
| SpecificationKey | string | Yes | Specification name |
| SpecificationValue | string | Yes | Specification value |
| Unit | string | No | Measurement unit |
| CreatedAt | DateTime | Yes | Creation timestamp |
| ModifiedAt | DateTime | No | Last modification timestamp |

#### EquipmentPhoto
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

#### EquipmentReservation
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

```csharp
public enum ReservationStatus
{
    Requested = 0,
    Confirmed = 1,
    Cancelled = 2,
    Fulfilled = 3
}
```

#### EquipmentLogistics
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

#### EquipmentMaintenance
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

```csharp
public enum MaintenanceType
{
    Inspection = 0,
    PreventiveMaintenance = 1,
    Repair = 2,
    Cleaning = 3,
    Replacement = 4
}

public enum MaintenanceStatus
{
    Scheduled = 0,
    InProgress = 1,
    Completed = 2,
    Cancelled = 3
}
```

#### EquipmentDamageReport
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

```csharp
public enum DamageSeverity
{
    Minor = 0,
    Moderate = 1,
    Severe = 2,
    TotalLoss = 3
}

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

## 18. API Endpoints

### 18.1 Equipment Item Endpoints
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

### 18.2 Reservation Endpoints
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

### 18.3 Logistics Endpoints
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

### 18.4 Maintenance Endpoints
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

## 19. CQRS Implementation

### 19.1 Commands Structure
```
EventManagementPlatform.Api/Features/Equipment/
├── CreateEquipmentItem/
│   ├── CreateEquipmentItemCommand.cs
│   ├── CreateEquipmentItemCommandHandler.cs
│   └── CreateEquipmentItemDto.cs
├── UpdateEquipmentItem/
├── ActivateEquipmentItem/
├── DeactivateEquipmentItem/
├── UploadEquipmentPhoto/
├── UpdateSpecifications/
├── CreateReservation/
├── UpdateReservation/
├── CancelReservation/
├── OverrideDoubleBooking/
├── UpdateLogistics/
├── ScheduleMaintenance/
├── CompleteMaintenance/
├── ReportDamage/
└── RetireEquipment/
```

### 19.2 Queries Structure
```
EventManagementPlatform.Api/Features/Equipment/
├── GetEquipmentItems/
├── GetEquipmentById/
├── GetReservations/
├── CheckAvailability/
├── GetAlternatives/
├── GetLogistics/
└── GetMaintenanceHistory/
```

---

## 20. Appendices

### 20.1 Related Documents
- [Frontend Specification](./frontend-spec.md)
- [Use Case Diagram](./diagrams/use-case.drawio)
- [C4 Diagrams](./diagrams/c4-context.drawio)
- [Class Diagram](./plantuml/class-diagram.plantuml)
- [Sequence Diagrams](./plantuml/sequence-diagram.plantuml)

### 20.2 Revision History
| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-22 | System | Initial specification |
