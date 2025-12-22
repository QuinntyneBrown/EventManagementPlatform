# Venue Management - Backend Specification

## 1. Introduction

### 1.1 Purpose
This Software Requirements Specification (SRS) document describes the backend implementation for the Venue Management feature of the Event Management Platform. This feature enables organizations to manage venue directories, venue details, contacts, and venue history.

### 1.2 Scope
The Venue Management backend provides:
- RESTful API endpoints for venue CRUD operations
- Event-driven architecture using domain events
- Integration with Azure services (Storage, AI, Cosmos DB)
- Real-time notifications and updates
- Search and filtering capabilities
- Venue rating and feedback system
- Venue blacklist/whitelist management

### 1.3 Technology Stack
- **Framework**: .NET 8 (C#)
- **Architecture**: Clean Architecture with CQRS and Event Sourcing
- **Cloud Platform**: Microsoft Azure
- **Database**: Azure Cosmos DB / Azure SQL Database
- **Storage**: Azure Blob Storage (for venue photos)
- **AI Services**: Azure AI Services (for image analysis, sentiment analysis)
- **Messaging**: Azure Service Bus
- **Caching**: Azure Redis Cache
- **Authentication**: Azure AD B2C / Entra ID
- **API**: ASP.NET Core Web API with OpenAPI/Swagger

## 2. System Architecture

### 2.1 Architectural Patterns
- **Clean Architecture**: Separation of concerns with distinct layers
- **CQRS**: Command Query Responsibility Segregation
- **Event Sourcing**: Domain events as the source of truth
- **Domain-Driven Design**: Rich domain model with aggregates
- **Repository Pattern**: Data access abstraction
- **Mediator Pattern**: Request handling with MediatR

### 2.2 Project Structure
```
VenueManagement/
├── VenueManagement.API/          # Web API layer
├── VenueManagement.Application/  # Application logic and CQRS
├── VenueManagement.Domain/       # Domain entities and events
├── VenueManagement.Infrastructure/ # External services and data access
└── VenueManagement.Tests/        # Unit and integration tests
```

## 3. Domain Model

### 3.1 Aggregates

#### Venue Aggregate Root
```csharp
Properties:
- VenueId (Guid, Primary Key)
- Name (string, required, max 200)
- Description (string, max 2000)
- Status (VenueStatus enum: Active, Inactive, Blacklisted)
- VenueType (VenueType enum: Conference, Hotel, Convention, Outdoor, etc.)
- Address (VenueAddress value object)
- Capacity (VenueCapacity value object)
- Amenities (List<Amenity>)
- Contacts (List<VenueContact>)
- AccessInstructions (string, max 1000)
- ParkingInfo (ParkingInfo value object)
- Photos (List<VenuePhoto>)
- Rating (VenueRating value object)
- IsActive (bool)
- CreatedAt (DateTime)
- CreatedBy (string)
- UpdatedAt (DateTime)
- UpdatedBy (string)
```

### 3.2 Value Objects

#### VenueAddress
```csharp
- Street1 (string, required, max 100)
- Street2 (string, optional, max 100)
- City (string, required, max 100)
- State (string, required, max 100)
- Country (string, required, max 100)
- PostalCode (string, required, max 20)
- Latitude (decimal, optional)
- Longitude (decimal, optional)
- TimeZone (string)
```

#### VenueCapacity
```csharp
- MaxCapacity (int, required)
- SeatedCapacity (int, optional)
- StandingCapacity (int, optional)
- ConfigurableLayouts (List<LayoutCapacity>)
```

#### VenueContact
```csharp
- ContactId (Guid)
- ContactType (ContactType enum: Primary, Booking, Technical, Catering)
- FirstName (string, required)
- LastName (string, required)
- Email (string, required)
- Phone (string, required)
- Position (string)
- Notes (string)
```

#### ParkingInfo
```csharp
- HasParking (bool)
- ParkingCapacity (int)
- ParkingType (ParkingType enum: Free, Paid, Valet, Street)
- ParkingInstructions (string)
```

#### VenuePhoto
```csharp
- PhotoId (Guid)
- Url (string)
- ThumbnailUrl (string)
- Caption (string)
- IsPrimary (bool)
- UploadedAt (DateTime)
- UploadedBy (string)
- AzureBlobPath (string)
```

#### VenueRating
```csharp
- AverageRating (decimal, 0-5)
- TotalRatings (int)
- RatingBreakdown (Dictionary<int, int>)
```

### 3.3 Entities

#### VenueHistory
```csharp
- HistoryId (Guid)
- VenueId (Guid)
- EventId (Guid)
- EventName (string)
- EventDate (DateTime)
- Rating (int, 1-5)
- Feedback (string)
- Issues (string)
- CreatedAt (DateTime)
```

#### VenueIssue
```csharp
- IssueId (Guid)
- VenueId (Guid)
- IssueType (IssueType enum)
- Description (string)
- Severity (Severity enum: Low, Medium, High, Critical)
- Status (IssueStatus enum: Open, InProgress, Resolved, Closed)
- ReportedBy (string)
- ReportedAt (DateTime)
- ResolvedAt (DateTime?)
- Resolution (string)
```

### 3.4 Enumerations

```csharp
public enum VenueStatus
{
    Active,
    Inactive,
    Blacklisted,
    PendingApproval
}

public enum VenueType
{
    ConferenceCenter,
    Hotel,
    ConventionCenter,
    Outdoor,
    Restaurant,
    Theater,
    Stadium,
    Other
}

public enum ContactType
{
    Primary,
    Booking,
    Technical,
    Catering,
    Emergency
}

public enum ParkingType
{
    Free,
    Paid,
    Valet,
    Street,
    None
}

public enum IssueType
{
    Facility,
    Equipment,
    Service,
    Safety,
    Cleanliness,
    Other
}

public enum Severity
{
    Low,
    Medium,
    High,
    Critical
}

public enum IssueStatus
{
    Open,
    InProgress,
    Resolved,
    Closed
}
```

## 4. Domain Events

### 4.1 Venue Lifecycle Events

#### VenueAdded
```csharp
{
    "VenueId": "guid",
    "Name": "string",
    "VenueType": "enum",
    "Address": "VenueAddress",
    "CreatedBy": "string",
    "CreatedAt": "datetime"
}
```

#### VenueDetailsUpdated
```csharp
{
    "VenueId": "guid",
    "UpdatedFields": "Dictionary<string, object>",
    "UpdatedBy": "string",
    "UpdatedAt": "datetime"
}
```

#### VenueRemoved
```csharp
{
    "VenueId": "guid",
    "RemovedBy": "string",
    "RemovedAt": "datetime",
    "Reason": "string"
}
```

#### VenueActivated
```csharp
{
    "VenueId": "guid",
    "ActivatedBy": "string",
    "ActivatedAt": "datetime"
}
```

#### VenueDeactivated
```csharp
{
    "VenueId": "guid",
    "DeactivatedBy": "string",
    "DeactivatedAt": "datetime",
    "Reason": "string"
}
```

### 4.2 Venue Contact Events

#### VenueContactAdded
```csharp
{
    "VenueId": "guid",
    "ContactId": "guid",
    "ContactType": "enum",
    "FullName": "string",
    "Email": "string",
    "AddedBy": "string",
    "AddedAt": "datetime"
}
```

#### VenueContactUpdated
```csharp
{
    "VenueId": "guid",
    "ContactId": "guid",
    "UpdatedFields": "Dictionary<string, object>",
    "UpdatedBy": "string",
    "UpdatedAt": "datetime"
}
```

#### VenueContactRemoved
```csharp
{
    "VenueId": "guid",
    "ContactId": "guid",
    "RemovedBy": "string",
    "RemovedAt": "datetime"
}
```

### 4.3 Venue Details Events

#### VenueAddressUpdated
```csharp
{
    "VenueId": "guid",
    "OldAddress": "VenueAddress",
    "NewAddress": "VenueAddress",
    "UpdatedBy": "string",
    "UpdatedAt": "datetime"
}
```

#### VenueCapacityUpdated
```csharp
{
    "VenueId": "guid",
    "OldCapacity": "VenueCapacity",
    "NewCapacity": "VenueCapacity",
    "UpdatedBy": "string",
    "UpdatedAt": "datetime"
}
```

#### VenueAmenitiesUpdated
```csharp
{
    "VenueId": "guid",
    "AddedAmenities": "List<string>",
    "RemovedAmenities": "List<string>",
    "UpdatedBy": "string",
    "UpdatedAt": "datetime"
}
```

#### VenueAccessInstructionsAdded
```csharp
{
    "VenueId": "guid",
    "Instructions": "string",
    "AddedBy": "string",
    "AddedAt": "datetime"
}
```

#### VenueParkingInfoUpdated
```csharp
{
    "VenueId": "guid",
    "ParkingInfo": "ParkingInfo",
    "UpdatedBy": "string",
    "UpdatedAt": "datetime"
}
```

#### VenuePhotoUploaded
```csharp
{
    "VenueId": "guid",
    "PhotoId": "guid",
    "Url": "string",
    "Caption": "string",
    "IsPrimary": "bool",
    "UploadedBy": "string",
    "UploadedAt": "datetime"
}
```

### 4.4 Venue History Events

#### VenueUsedForEvent
```csharp
{
    "VenueId": "guid",
    "EventId": "guid",
    "EventName": "string",
    "EventDate": "datetime",
    "RecordedBy": "string",
    "RecordedAt": "datetime"
}
```

#### VenueRatingRecorded
```csharp
{
    "VenueId": "guid",
    "EventId": "guid",
    "Rating": "int",
    "RatedBy": "string",
    "RatedAt": "datetime"
}
```

#### VenueFeedbackReceived
```csharp
{
    "VenueId": "guid",
    "EventId": "guid",
    "Feedback": "string",
    "SentimentScore": "decimal",
    "ProvidedBy": "string",
    "ProvidedAt": "datetime"
}
```

### 4.5 Venue Status Events

#### VenueIssueReported
```csharp
{
    "VenueId": "guid",
    "IssueId": "guid",
    "IssueType": "enum",
    "Severity": "enum",
    "Description": "string",
    "ReportedBy": "string",
    "ReportedAt": "datetime"
}
```

#### VenueBlacklisted
```csharp
{
    "VenueId": "guid",
    "Reason": "string",
    "BlacklistedBy": "string",
    "BlacklistedAt": "datetime"
}
```

#### VenueWhitelisted
```csharp
{
    "VenueId": "guid",
    "WhitelistedBy": "string",
    "WhitelistedAt": "datetime"
}
```

## 5. API Endpoints

### 5.1 Venue Management

#### Create Venue
```
POST /api/v1/venues
Authorization: Bearer {token}
Content-Type: application/json

Request Body:
{
    "name": "string",
    "description": "string",
    "venueType": "enum",
    "address": {
        "street1": "string",
        "street2": "string",
        "city": "string",
        "state": "string",
        "country": "string",
        "postalCode": "string"
    },
    "capacity": {
        "maxCapacity": 0,
        "seatedCapacity": 0,
        "standingCapacity": 0
    },
    "amenities": ["string"],
    "contacts": [
        {
            "contactType": "enum",
            "firstName": "string",
            "lastName": "string",
            "email": "string",
            "phone": "string",
            "position": "string"
        }
    ]
}

Response: 201 Created
{
    "venueId": "guid",
    "name": "string",
    "createdAt": "datetime"
}
```

#### Get Venue by ID
```
GET /api/v1/venues/{venueId}
Authorization: Bearer {token}

Response: 200 OK
{
    "venueId": "guid",
    "name": "string",
    "description": "string",
    "status": "enum",
    "venueType": "enum",
    "address": { ... },
    "capacity": { ... },
    "amenities": [...],
    "contacts": [...],
    "photos": [...],
    "rating": { ... },
    "createdAt": "datetime",
    "updatedAt": "datetime"
}
```

#### Get All Venues
```
GET /api/v1/venues
Authorization: Bearer {token}
Query Parameters:
- page (int, default: 1)
- pageSize (int, default: 20, max: 100)
- status (VenueStatus, optional)
- venueType (VenueType, optional)
- city (string, optional)
- country (string, optional)
- minCapacity (int, optional)
- amenities (string[], optional)
- searchTerm (string, optional)
- sortBy (string, default: "name")
- sortOrder (string, default: "asc")

Response: 200 OK
{
    "items": [...],
    "totalCount": 0,
    "page": 1,
    "pageSize": 20,
    "totalPages": 0
}
```

#### Update Venue
```
PUT /api/v1/venues/{venueId}
Authorization: Bearer {token}
Content-Type: application/json

Request Body: (partial update supported)
{
    "name": "string",
    "description": "string",
    "venueType": "enum",
    ...
}

Response: 200 OK
{
    "venueId": "guid",
    "updatedAt": "datetime"
}
```

#### Delete Venue
```
DELETE /api/v1/venues/{venueId}
Authorization: Bearer {token}
Query Parameters:
- reason (string, optional)

Response: 204 No Content
```

#### Activate Venue
```
POST /api/v1/venues/{venueId}/activate
Authorization: Bearer {token}

Response: 200 OK
```

#### Deactivate Venue
```
POST /api/v1/venues/{venueId}/deactivate
Authorization: Bearer {token}
Content-Type: application/json

Request Body:
{
    "reason": "string"
}

Response: 200 OK
```

### 5.2 Venue Contacts

#### Add Contact
```
POST /api/v1/venues/{venueId}/contacts
Authorization: Bearer {token}
Content-Type: application/json

Request Body:
{
    "contactType": "enum",
    "firstName": "string",
    "lastName": "string",
    "email": "string",
    "phone": "string",
    "position": "string",
    "notes": "string"
}

Response: 201 Created
{
    "contactId": "guid"
}
```

#### Update Contact
```
PUT /api/v1/venues/{venueId}/contacts/{contactId}
Authorization: Bearer {token}

Response: 200 OK
```

#### Remove Contact
```
DELETE /api/v1/venues/{venueId}/contacts/{contactId}
Authorization: Bearer {token}

Response: 204 No Content
```

### 5.3 Venue Details

#### Update Address
```
PUT /api/v1/venues/{venueId}/address
Authorization: Bearer {token}

Response: 200 OK
```

#### Update Capacity
```
PUT /api/v1/venues/{venueId}/capacity
Authorization: Bearer {token}

Response: 200 OK
```

#### Update Amenities
```
PUT /api/v1/venues/{venueId}/amenities
Authorization: Bearer {token}
Content-Type: application/json

Request Body:
{
    "amenities": ["WiFi", "Parking", "AC", "Projector", "Catering"]
}

Response: 200 OK
```

#### Update Access Instructions
```
PUT /api/v1/venues/{venueId}/access-instructions
Authorization: Bearer {token}
Content-Type: application/json

Request Body:
{
    "instructions": "string"
}

Response: 200 OK
```

#### Update Parking Info
```
PUT /api/v1/venues/{venueId}/parking-info
Authorization: Bearer {token}

Response: 200 OK
```

### 5.4 Venue Photos

#### Upload Photo
```
POST /api/v1/venues/{venueId}/photos
Authorization: Bearer {token}
Content-Type: multipart/form-data

Request Body:
- file (binary)
- caption (string, optional)
- isPrimary (bool, default: false)

Response: 201 Created
{
    "photoId": "guid",
    "url": "string",
    "thumbnailUrl": "string"
}
```

#### Get Photos
```
GET /api/v1/venues/{venueId}/photos
Authorization: Bearer {token}

Response: 200 OK
[
    {
        "photoId": "guid",
        "url": "string",
        "thumbnailUrl": "string",
        "caption": "string",
        "isPrimary": bool,
        "uploadedAt": "datetime"
    }
]
```

#### Delete Photo
```
DELETE /api/v1/venues/{venueId}/photos/{photoId}
Authorization: Bearer {token}

Response: 204 No Content
```

### 5.5 Venue History

#### Record Venue Usage
```
POST /api/v1/venues/{venueId}/history
Authorization: Bearer {token}
Content-Type: application/json

Request Body:
{
    "eventId": "guid",
    "eventName": "string",
    "eventDate": "datetime"
}

Response: 201 Created
```

#### Get Venue History
```
GET /api/v1/venues/{venueId}/history
Authorization: Bearer {token}
Query Parameters:
- startDate (datetime, optional)
- endDate (datetime, optional)

Response: 200 OK
[
    {
        "historyId": "guid",
        "eventId": "guid",
        "eventName": "string",
        "eventDate": "datetime",
        "rating": 0,
        "feedback": "string"
    }
]
```

#### Record Rating
```
POST /api/v1/venues/{venueId}/ratings
Authorization: Bearer {token}
Content-Type: application/json

Request Body:
{
    "eventId": "guid",
    "rating": 0,  // 1-5
    "feedback": "string"
}

Response: 201 Created
```

#### Submit Feedback
```
POST /api/v1/venues/{venueId}/feedback
Authorization: Bearer {token}
Content-Type: application/json

Request Body:
{
    "eventId": "guid",
    "feedback": "string"
}

Response: 201 Created
{
    "sentimentScore": 0.0  // -1.0 to 1.0
}
```

### 5.6 Venue Issues

#### Report Issue
```
POST /api/v1/venues/{venueId}/issues
Authorization: Bearer {token}
Content-Type: application/json

Request Body:
{
    "issueType": "enum",
    "severity": "enum",
    "description": "string"
}

Response: 201 Created
{
    "issueId": "guid"
}
```

#### Get Issues
```
GET /api/v1/venues/{venueId}/issues
Authorization: Bearer {token}
Query Parameters:
- status (IssueStatus, optional)
- severity (Severity, optional)

Response: 200 OK
```

#### Update Issue Status
```
PUT /api/v1/venues/{venueId}/issues/{issueId}/status
Authorization: Bearer {token}

Response: 200 OK
```

### 5.7 Venue Status Management

#### Blacklist Venue
```
POST /api/v1/venues/{venueId}/blacklist
Authorization: Bearer {token}
Content-Type: application/json

Request Body:
{
    "reason": "string"
}

Response: 200 OK
```

#### Whitelist Venue
```
POST /api/v1/venues/{venueId}/whitelist
Authorization: Bearer {token}

Response: 200 OK
```

### 5.8 Search and Analytics

#### Search Venues
```
GET /api/v1/venues/search
Authorization: Bearer {token}
Query Parameters:
- q (string, required)
- filters (JSON, optional)
- location (string, optional)
- radius (int, optional, in km)

Response: 200 OK
```

#### Get Venue Analytics
```
GET /api/v1/venues/{venueId}/analytics
Authorization: Bearer {token}
Query Parameters:
- startDate (datetime)
- endDate (datetime)

Response: 200 OK
{
    "totalEvents": 0,
    "averageRating": 0.0,
    "totalFeedback": 0,
    "issueCount": 0,
    "utilizationRate": 0.0
}
```

#### Get Top Rated Venues
```
GET /api/v1/venues/top-rated
Authorization: Bearer {token}
Query Parameters:
- limit (int, default: 10)
- venueType (VenueType, optional)
- city (string, optional)

Response: 200 OK
```

## 6. Application Layer (CQRS)

### 6.1 Commands

```csharp
// Venue Management
CreateVenueCommand
UpdateVenueCommand
DeleteVenueCommand
ActivateVenueCommand
DeactivateVenueCommand

// Contact Management
AddVenueContactCommand
UpdateVenueContactCommand
RemoveVenueContactCommand

// Details Management
UpdateVenueAddressCommand
UpdateVenueCapacityCommand
UpdateVenueAmenitiesCommand
UpdateAccessInstructionsCommand
UpdateParkingInfoCommand

// Photo Management
UploadVenuePhotoCommand
DeleteVenuePhotoCommand

// History Management
RecordVenueUsageCommand
RecordVenueRatingCommand
SubmitVenueFeedbackCommand

// Issue Management
ReportVenueIssueCommand
UpdateIssueStatusCommand

// Status Management
BlacklistVenueCommand
WhitelistVenueCommand
```

### 6.2 Queries

```csharp
GetVenueByIdQuery
GetAllVenuesQuery
SearchVenuesQuery
GetVenuesByLocationQuery
GetVenuesByTypeQuery
GetVenueContactsQuery
GetVenuePhotosQuery
GetVenueHistoryQuery
GetVenueIssuesQuery
GetVenueAnalyticsQuery
GetTopRatedVenuesQuery
GetBlacklistedVenuesQuery
```

### 6.3 Command Handlers

Each command handler should:
1. Validate the command
2. Load the aggregate (if updating)
3. Execute business logic
4. Persist changes
5. Publish domain events
6. Return result

Example:
```csharp
public class CreateVenueCommandHandler : IRequestHandler<CreateVenueCommand, Result<Guid>>
{
    private readonly IVenueRepository _repository;
    private readonly IEventBus _eventBus;
    private readonly IValidator<CreateVenueCommand> _validator;

    public async Task<Result<Guid>> Handle(CreateVenueCommand command, CancellationToken cancellationToken)
    {
        // Validate
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return Result<Guid>.Failure(validationResult.Errors);

        // Create aggregate
        var venue = Venue.Create(command.Name, command.VenueType, command.Address, command.Capacity);

        // Persist
        await _repository.AddAsync(venue, cancellationToken);

        // Publish events
        await _eventBus.PublishAsync(new VenueAdded { ... }, cancellationToken);

        return Result<Guid>.Success(venue.Id);
    }
}
```

## 7. Infrastructure Layer

### 7.1 Data Persistence

#### Repository Interface
```csharp
public interface IVenueRepository
{
    Task<Venue> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Venue>> GetAllAsync(VenueFilter filter, CancellationToken cancellationToken = default);
    Task<PagedResult<Venue>> GetPagedAsync(VenueFilter filter, int page, int pageSize, CancellationToken cancellationToken = default);
    Task AddAsync(Venue venue, CancellationToken cancellationToken = default);
    Task UpdateAsync(Venue venue, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
```

#### Database Schema (SQL)
```sql
-- Venues Table
CREATE TABLE Venues (
    VenueId UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(2000),
    Status NVARCHAR(50) NOT NULL,
    VenueType NVARCHAR(50) NOT NULL,
    Street1 NVARCHAR(100),
    Street2 NVARCHAR(100),
    City NVARCHAR(100),
    State NVARCHAR(100),
    Country NVARCHAR(100),
    PostalCode NVARCHAR(20),
    Latitude DECIMAL(9,6),
    Longitude DECIMAL(9,6),
    TimeZone NVARCHAR(50),
    MaxCapacity INT,
    SeatedCapacity INT,
    StandingCapacity INT,
    AccessInstructions NVARCHAR(1000),
    HasParking BIT,
    ParkingCapacity INT,
    ParkingType NVARCHAR(50),
    ParkingInstructions NVARCHAR(500),
    AverageRating DECIMAL(3,2),
    TotalRatings INT,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL,
    CreatedBy NVARCHAR(100) NOT NULL,
    UpdatedAt DATETIME2,
    UpdatedBy NVARCHAR(100),
    CONSTRAINT CK_Rating CHECK (AverageRating >= 0 AND AverageRating <= 5)
);

-- Venue Contacts Table
CREATE TABLE VenueContacts (
    ContactId UNIQUEIDENTIFIER PRIMARY KEY,
    VenueId UNIQUEIDENTIFIER NOT NULL,
    ContactType NVARCHAR(50) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(50) NOT NULL,
    Position NVARCHAR(100),
    Notes NVARCHAR(500),
    CreatedAt DATETIME2 NOT NULL,
    FOREIGN KEY (VenueId) REFERENCES Venues(VenueId) ON DELETE CASCADE
);

-- Venue Amenities Table
CREATE TABLE VenueAmenities (
    VenueAmenityId UNIQUEIDENTIFIER PRIMARY KEY,
    VenueId UNIQUEIDENTIFIER NOT NULL,
    Amenity NVARCHAR(100) NOT NULL,
    FOREIGN KEY (VenueId) REFERENCES Venues(VenueId) ON DELETE CASCADE
);

-- Venue Photos Table
CREATE TABLE VenuePhotos (
    PhotoId UNIQUEIDENTIFIER PRIMARY KEY,
    VenueId UNIQUEIDENTIFIER NOT NULL,
    Url NVARCHAR(500) NOT NULL,
    ThumbnailUrl NVARCHAR(500),
    Caption NVARCHAR(200),
    IsPrimary BIT NOT NULL DEFAULT 0,
    AzureBlobPath NVARCHAR(500) NOT NULL,
    UploadedAt DATETIME2 NOT NULL,
    UploadedBy NVARCHAR(100) NOT NULL,
    FOREIGN KEY (VenueId) REFERENCES Venues(VenueId) ON DELETE CASCADE
);

-- Venue History Table
CREATE TABLE VenueHistory (
    HistoryId UNIQUEIDENTIFIER PRIMARY KEY,
    VenueId UNIQUEIDENTIFIER NOT NULL,
    EventId UNIQUEIDENTIFIER NOT NULL,
    EventName NVARCHAR(200) NOT NULL,
    EventDate DATETIME2 NOT NULL,
    Rating INT,
    Feedback NVARCHAR(2000),
    Issues NVARCHAR(2000),
    CreatedAt DATETIME2 NOT NULL,
    FOREIGN KEY (VenueId) REFERENCES Venues(VenueId) ON DELETE CASCADE,
    CONSTRAINT CK_HistoryRating CHECK (Rating IS NULL OR (Rating >= 1 AND Rating <= 5))
);

-- Venue Issues Table
CREATE TABLE VenueIssues (
    IssueId UNIQUEIDENTIFIER PRIMARY KEY,
    VenueId UNIQUEIDENTIFIER NOT NULL,
    IssueType NVARCHAR(50) NOT NULL,
    Description NVARCHAR(2000) NOT NULL,
    Severity NVARCHAR(50) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    ReportedBy NVARCHAR(100) NOT NULL,
    ReportedAt DATETIME2 NOT NULL,
    ResolvedAt DATETIME2,
    Resolution NVARCHAR(2000),
    FOREIGN KEY (VenueId) REFERENCES Venues(VenueId) ON DELETE CASCADE
);

-- Domain Events Table (Event Sourcing)
CREATE TABLE VenueDomainEvents (
    EventId UNIQUEIDENTIFIER PRIMARY KEY,
    AggregateId UNIQUEIDENTIFIER NOT NULL,
    EventType NVARCHAR(100) NOT NULL,
    EventData NVARCHAR(MAX) NOT NULL,
    Version INT NOT NULL,
    OccurredAt DATETIME2 NOT NULL,
    ProcessedAt DATETIME2
);

-- Indexes
CREATE INDEX IX_Venues_Status ON Venues(Status);
CREATE INDEX IX_Venues_City ON Venues(City);
CREATE INDEX IX_Venues_VenueType ON Venues(VenueType);
CREATE INDEX IX_VenueHistory_VenueId ON VenueHistory(VenueId);
CREATE INDEX IX_VenueHistory_EventDate ON VenueHistory(EventDate);
CREATE INDEX IX_VenueIssues_Status ON VenueIssues(Status);
CREATE INDEX IX_DomainEvents_AggregateId ON VenueDomainEvents(AggregateId);
```

### 7.2 Azure Services Integration

#### Azure Blob Storage (Photo Management)
```csharp
public interface IVenuePhotoStorageService
{
    Task<string> UploadPhotoAsync(Stream photoStream, string fileName, CancellationToken cancellationToken);
    Task<string> GenerateThumbnailAsync(string photoUrl, CancellationToken cancellationToken);
    Task DeletePhotoAsync(string blobPath, CancellationToken cancellationToken);
    Task<string> GetPhotoUrlAsync(string blobPath, CancellationToken cancellationToken);
}

Configuration:
- Container Name: venue-photos
- Blob Naming: {venueId}/{photoId}/{filename}
- CDN Integration: Azure CDN for photo delivery
- Thumbnail Generation: Azure Functions
```

#### Azure AI Services

##### Computer Vision (Photo Analysis)
```csharp
public interface IPhotoAnalysisService
{
    Task<PhotoAnalysisResult> AnalyzePhotoAsync(string photoUrl, CancellationToken cancellationToken);
}

Features:
- Image quality assessment
- Inappropriate content detection
- Automatic tagging
- Dominant color extraction
```

##### Text Analytics (Sentiment Analysis)
```csharp
public interface ISentimentAnalysisService
{
    Task<SentimentResult> AnalyzeFeedbackAsync(string feedback, CancellationToken cancellationToken);
}

Features:
- Sentiment scoring (-1.0 to 1.0)
- Key phrase extraction
- Language detection
- Entity recognition
```

#### Azure Service Bus (Event Publishing)
```csharp
public interface IEventBus
{
    Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken) where T : IDomainEvent;
    Task PublishBatchAsync<T>(IEnumerable<T> domainEvents, CancellationToken cancellationToken) where T : IDomainEvent;
}

Configuration:
- Topic: venue-events
- Subscriptions: venue-analytics, venue-notifications, event-integration
- Message TTL: 14 days
- Dead Letter Queue: Enabled
```

#### Azure Redis Cache
```csharp
public interface IVenueCacheService
{
    Task<Venue> GetVenueAsync(Guid venueId, CancellationToken cancellationToken);
    Task SetVenueAsync(Venue venue, TimeSpan expiration, CancellationToken cancellationToken);
    Task InvalidateVenueAsync(Guid venueId, CancellationToken cancellationToken);
    Task<List<Venue>> GetTopRatedVenuesAsync(CancellationToken cancellationToken);
}

Cache Strategy:
- Cache-Aside Pattern
- TTL: 1 hour for venue details, 5 minutes for top-rated lists
- Invalidation: On venue updates
```

#### Azure Cognitive Search (Search Functionality)
```csharp
public interface IVenueSearchService
{
    Task<SearchResult<Venue>> SearchAsync(VenueSearchRequest request, CancellationToken cancellationToken);
    Task IndexVenueAsync(Venue venue, CancellationToken cancellationToken);
    Task DeleteFromIndexAsync(Guid venueId, CancellationToken cancellationToken);
}

Search Index Schema:
- venueId (Edm.String, key)
- name (Edm.String, searchable, sortable)
- description (Edm.String, searchable)
- city (Edm.String, filterable, facetable)
- country (Edm.String, filterable, facetable)
- venueType (Edm.String, filterable, facetable)
- amenities (Collection(Edm.String), filterable, facetable)
- capacity (Edm.Int32, sortable, filterable)
- rating (Edm.Double, sortable, filterable)
- location (Edm.GeographyPoint, filterable)
```

### 7.3 External Integrations

#### Geocoding Service
```csharp
public interface IGeocodingService
{
    Task<GeoLocation> GetCoordinatesAsync(VenueAddress address, CancellationToken cancellationToken);
    Task<TimeZoneInfo> GetTimeZoneAsync(decimal latitude, decimal longitude, CancellationToken cancellationToken);
}
```

#### Notification Service
```csharp
public interface INotificationService
{
    Task SendVenueBlacklistedNotificationAsync(Venue venue, string reason, CancellationToken cancellationToken);
    Task SendIssueReportedNotificationAsync(VenueIssue issue, CancellationToken cancellationToken);
    Task SendVenueRatingNotificationAsync(Venue venue, int rating, CancellationToken cancellationToken);
}
```

## 8. Security

### 8.1 Authentication
- Azure AD B2C / Entra ID integration
- JWT Bearer token authentication
- Role-based access control (RBAC)

### 8.2 Authorization

#### Roles and Permissions
```csharp
Roles:
- VenueAdmin: Full access to all venue operations
- VenueManager: Create, update, view venues
- VenueCoordinator: View venues, add contacts, upload photos
- EventPlanner: View venues, submit feedback and ratings
- Viewer: Read-only access

Permissions:
- venues.create
- venues.update
- venues.delete
- venues.view
- venues.activate
- venues.deactivate
- venues.blacklist
- venues.whitelist
- venues.contacts.manage
- venues.photos.upload
- venues.feedback.submit
- venues.issues.report
```

### 8.3 Data Protection
- Encryption at rest (Azure SQL TDE)
- Encryption in transit (TLS 1.2+)
- Sensitive data masking (contact information)
- GDPR compliance for personal data

### 8.4 API Security
- Rate limiting (Azure API Management)
- Request validation and sanitization
- CORS policy configuration
- API key management for service-to-service calls

## 9. Validation Rules

### 9.1 Venue Validation
```csharp
- Name: Required, 3-200 characters
- Description: Optional, max 2000 characters
- VenueType: Required, valid enum value
- Address: Required, all fields validated
- Capacity: MaxCapacity > 0, SeatedCapacity + StandingCapacity <= MaxCapacity
- Email: Valid email format
- Phone: Valid phone format
- URL: Valid URL format
```

### 9.2 Business Rules
```csharp
- Cannot delete venue if it has upcoming events
- Cannot blacklist venue if it has confirmed future bookings
- Primary photo must exist before setting additional photos
- Rating must be 1-5
- Cannot have duplicate contacts with same email for same venue
- Cannot activate already active venue
- Cannot deactivate already inactive venue
```

## 10. Error Handling

### 10.1 Error Response Format
```json
{
    "type": "https://api.events.com/errors/validation",
    "title": "One or more validation errors occurred",
    "status": 400,
    "traceId": "00-abc123-def456-00",
    "errors": {
        "Name": ["The Name field is required."],
        "Capacity.MaxCapacity": ["MaxCapacity must be greater than 0"]
    }
}
```

### 10.2 HTTP Status Codes
```
200 OK - Successful GET, PUT
201 Created - Successful POST
204 No Content - Successful DELETE
400 Bad Request - Validation errors
401 Unauthorized - Missing or invalid authentication
403 Forbidden - Insufficient permissions
404 Not Found - Resource not found
409 Conflict - Business rule violation
429 Too Many Requests - Rate limit exceeded
500 Internal Server Error - Unexpected error
503 Service Unavailable - Temporary unavailability
```

### 10.3 Exception Handling
```csharp
Custom Exceptions:
- VenueNotFoundException
- VenueAlreadyExistsException
- VenueBlacklistedException
- InvalidVenueStateException
- VenueValidationException
- VenuePhotoUploadException
```

## 11. Performance and Scalability

### 11.1 Performance Requirements
- API response time: < 200ms (95th percentile)
- Photo upload: < 5 seconds for 5MB image
- Search queries: < 500ms
- Concurrent users: 10,000+
- Database query optimization with proper indexing

### 11.2 Caching Strategy
- Redis cache for frequently accessed venues
- CDN for venue photos
- Output caching for top-rated venues list
- Query result caching with 5-minute TTL

### 11.3 Scalability
- Horizontal scaling with Azure App Service
- Database read replicas for query operations
- Azure Service Bus for async processing
- Azure Functions for background tasks

## 12. Monitoring and Logging

### 12.1 Application Insights Integration
```csharp
Metrics:
- API request count and duration
- Failed requests and exceptions
- Database query performance
- Cache hit/miss ratio
- Photo upload success rate
- Domain event processing time

Custom Events:
- VenueCreated
- VenueBlacklisted
- HighSeverityIssueReported
- PhotoUploadFailed
```

### 12.2 Structured Logging
```csharp
using Serilog;

Log Levels:
- Information: Normal operations, API calls
- Warning: Validation failures, business rule violations
- Error: Exceptions, external service failures
- Critical: System failures, data corruption

Log Enrichment:
- CorrelationId
- UserId
- VenueId
- Timestamp
- Environment
```

### 12.3 Health Checks
```csharp
Health Check Endpoints:
- /health - Overall health
- /health/ready - Readiness probe
- /health/live - Liveness probe

Dependencies:
- Database connectivity
- Azure Storage availability
- Service Bus connectivity
- Redis cache availability
- Azure AI Services status
```

## 13. Testing Strategy

### 13.1 Unit Tests
- Domain model tests
- Command/Query handler tests
- Validator tests
- Business logic tests
- Target: 80% code coverage

### 13.2 Integration Tests
- API endpoint tests
- Repository tests
- Event publishing tests
- Azure service integration tests

### 13.3 End-to-End Tests
- Complete user flows
- Multi-step scenarios
- Error handling scenarios

## 14. Deployment

### 14.1 CI/CD Pipeline
```yaml
Pipeline Stages:
1. Build
   - Restore NuGet packages
   - Compile code
   - Run static code analysis

2. Test
   - Run unit tests
   - Run integration tests
   - Generate code coverage report

3. Package
   - Create deployment artifacts
   - Version management

4. Deploy to Dev
   - Deploy to Azure App Service (Dev)
   - Run smoke tests

5. Deploy to Staging
   - Deploy to Azure App Service (Staging)
   - Run E2E tests

6. Deploy to Production
   - Manual approval required
   - Blue-green deployment
   - Automated rollback on failure
```

### 14.2 Infrastructure as Code
```
Tool: Azure Bicep / Terraform

Resources:
- Azure App Service Plan
- Azure App Service
- Azure SQL Database
- Azure Storage Account
- Azure Service Bus Namespace
- Azure Redis Cache
- Azure Cognitive Services
- Azure Application Insights
```

## 15. Compliance and Audit

### 15.1 Audit Trail
```csharp
All operations tracked:
- Who performed the action
- What action was performed
- When it was performed
- Original and new values (for updates)
- IP address and user agent

Storage: Azure Table Storage or SQL audit tables
Retention: 7 years
```

### 15.2 GDPR Compliance
- Right to access: Export venue contact data
- Right to erasure: Anonymize personal data
- Data portability: JSON export format
- Consent management for contact data

## 16. Documentation

### 16.1 API Documentation
- OpenAPI/Swagger specification
- Interactive API explorer
- Code samples in C#, JavaScript, Python
- Postman collection

### 16.2 Developer Documentation
- Architecture decision records (ADR)
- Domain model documentation
- Integration guides
- Troubleshooting guides

## Appendix A: Domain Event Schema

All domain events extend from base `DomainEvent`:
```csharp
public abstract class DomainEvent : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
    public string EventType { get; init; }
    public int Version { get; init; } = 1;
}
```

## Appendix B: Configuration

### Application Settings
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...",
    "RedisCache": "..."
  },
  "AzureStorage": {
    "ConnectionString": "...",
    "ContainerName": "venue-photos"
  },
  "AzureServiceBus": {
    "ConnectionString": "...",
    "TopicName": "venue-events"
  },
  "AzureAI": {
    "ComputerVision": {
      "Endpoint": "...",
      "Key": "..."
    },
    "TextAnalytics": {
      "Endpoint": "...",
      "Key": "..."
    }
  },
  "CognitiveSearch": {
    "ServiceName": "...",
    "IndexName": "venues",
    "ApiKey": "..."
  },
  "Authentication": {
    "Authority": "...",
    "ClientId": "...",
    "Audience": "..."
  }
}
```

## Version History
- v1.0.0 - Initial specification (2025-12-22)
