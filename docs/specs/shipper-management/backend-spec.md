# Shipper & Logistics Management - Backend Specification

## Document Information
- **Version**: 1.0
- **Last Updated**: 2025-12-22
- **Technology Stack**: .NET 8, Azure Services, Azure AI
- **Architecture Pattern**: Clean Architecture, CQRS, Event Sourcing

---

## Table of Contents
1. [Introduction](#1-introduction)
2. [System Architecture](#2-system-architecture)
3. [Domain Model](#3-domain-model)
4. [API Specifications](#4-api-specifications)
5. [Data Management](#5-data-management)
6. [Azure Services Integration](#6-azure-services-integration)
7. [Azure AI Integration](#7-azure-ai-integration)
8. [Event-Driven Architecture](#8-event-driven-architecture)
9. [Security & Authentication](#9-security--authentication)
10. [Performance & Scalability](#10-performance--scalability)
11. [Monitoring & Logging](#11-monitoring--logging)
12. [Error Handling](#12-error-handling)
13. [Testing Strategy](#13-testing-strategy)

---

## 1. Introduction

### 1.1 Purpose
This document specifies the backend requirements for the Shipper & Logistics Management feature of the Event Management Platform. The system manages the complete lifecycle of event logistics including shipper list generation, shipment tracking, delivery management, and returns processing.

### 1.2 Scope
The backend system provides:
- RESTful APIs for shipper list management
- Real-time shipment tracking and monitoring
- Delivery and return workflow management
- Event-driven architecture for logistics events
- Integration with Azure AI for intelligent routing and predictions
- Integration with Azure services for scalability and reliability

### 1.3 Business Requirements
- Support multi-event logistics coordination
- Real-time shipment status tracking
- Driver assignment and route optimization
- Delivery signature capture and verification
- Exception handling and rescheduling
- Return management with damage tracking
- Comprehensive audit trail via event sourcing

---

## 2. System Architecture

### 2.1 Clean Architecture Layers

#### 2.1.1 Domain Layer
```
EventManagement.ShipperManagement.Domain/
├── Entities/
│   ├── ShipperList.cs
│   ├── ShipperListItem.cs
│   ├── Shipment.cs
│   ├── Delivery.cs
│   ├── ReturnItem.cs
│   └── Driver.cs
├── ValueObjects/
│   ├── Address.cs
│   ├── DeliveryWindow.cs
│   ├── ShipmentStatus.cs
│   ├── TrackingNumber.cs
│   └── Signature.cs
├── Aggregates/
│   ├── ShipperListAggregate.cs
│   └── ShipmentAggregate.cs
├── DomainEvents/
│   ├── ShipperListEvents.cs
│   ├── ShipmentEvents.cs
│   ├── DeliveryEvents.cs
│   └── ReturnEvents.cs
├── Interfaces/
│   ├── IShipperListRepository.cs
│   ├── IShipmentRepository.cs
│   └── IEventPublisher.cs
└── Exceptions/
    ├── DomainException.cs
    └── BusinessRuleException.cs
```

#### 2.1.2 Application Layer
```
EventManagement.ShipperManagement.Application/
├── Commands/
│   ├── ShipperList/
│   │   ├── GenerateShipperListCommand.cs
│   │   ├── UpdateShipperListCommand.cs
│   │   ├── FinalizeShipperListCommand.cs
│   │   ├── AddItemToShipperListCommand.cs
│   │   └── MarkItemAsPackedCommand.cs
│   ├── Shipment/
│   │   ├── CreateShipmentCommand.cs
│   │   ├── AssignDriverCommand.cs
│   │   ├── DepartWarehouseCommand.cs
│   │   └── UpdateShipmentStatusCommand.cs
│   ├── Delivery/
│   │   ├── DeliverItemCommand.cs
│   │   ├── CaptureSignatureCommand.cs
│   │   ├── ReportExceptionCommand.cs
│   │   └── RescheduleDeliveryCommand.cs
│   └── Return/
│       ├── SchedulePickupCommand.cs
│       ├── RecordPickupCommand.cs
│       └── RecordDamageCommand.cs
├── Queries/
│   ├── GetShipperListQuery.cs
│   ├── GetShipmentTrackingQuery.cs
│   ├── GetActiveShipmentsQuery.cs
│   ├── GetDeliveryStatusQuery.cs
│   └── GetReturnItemsQuery.cs
├── DTOs/
│   ├── ShipperListDto.cs
│   ├── ShipmentDto.cs
│   ├── DeliveryDto.cs
│   └── ReturnItemDto.cs
├── Handlers/
│   ├── CommandHandlers/
│   └── QueryHandlers/
├── Services/
│   ├── RouteOptimizationService.cs
│   ├── TrackingService.cs
│   └── NotificationService.cs
└── Validators/
    ├── ShipperListValidator.cs
    └── ShipmentValidator.cs
```

#### 2.1.3 Infrastructure Layer
```
EventManagement.ShipperManagement.Infrastructure/
├── Persistence/
│   ├── ShipperManagementDbContext.cs
│   ├── Repositories/
│   │   ├── ShipperListRepository.cs
│   │   ├── ShipmentRepository.cs
│   │   └── DeliveryRepository.cs
│   ├── Configurations/
│   │   ├── ShipperListConfiguration.cs
│   │   └── ShipmentConfiguration.cs
│   └── Migrations/
├── EventStore/
│   ├── AzureEventStoreRepository.cs
│   └── EventStoreDbContext.cs
├── Messaging/
│   ├── AzureServiceBusPublisher.cs
│   └── EventHandlers/
├── Azure/
│   ├── AzureAIService.cs
│   ├── AzureMapsService.cs
│   ├── AzureBlobStorageService.cs
│   └── AzureKeyVaultService.cs
├── ExternalServices/
│   ├── SMSService.cs
│   └── EmailService.cs
└── BackgroundJobs/
    ├── ShipmentTrackingJob.cs
    └── ReturnReminderJob.cs
```

#### 2.1.4 API Layer
```
EventManagement.ShipperManagement.API/
├── Controllers/
│   ├── ShipperListController.cs
│   ├── ShipmentController.cs
│   ├── DeliveryController.cs
│   └── ReturnController.cs
├── Middleware/
│   ├── ExceptionHandlingMiddleware.cs
│   ├── RequestLoggingMiddleware.cs
│   └── PerformanceMonitoringMiddleware.cs
├── Filters/
│   ├── ValidationFilter.cs
│   └── AuthorizationFilter.cs
└── Hubs/
    └── ShipmentTrackingHub.cs (SignalR)
```

### 2.2 Technology Stack

#### 2.2.1 Core Framework
- **.NET 8**: Latest LTS version with minimal APIs support
- **ASP.NET Core 8**: Web API framework
- **Entity Framework Core 8**: ORM for data access
- **MediatR**: CQRS implementation
- **FluentValidation**: Input validation
- **AutoMapper**: Object-to-object mapping

#### 2.2.2 Azure Services
- **Azure SQL Database**: Primary data store
- **Azure Cosmos DB**: Event store for event sourcing
- **Azure Service Bus**: Message broker for domain events
- **Azure Blob Storage**: Document and signature storage
- **Azure Key Vault**: Secrets management
- **Azure Application Insights**: Monitoring and logging
- **Azure SignalR Service**: Real-time updates
- **Azure Maps**: Geolocation and routing

#### 2.2.3 Azure AI Services
- **Azure OpenAI Service**: Intelligent routing suggestions
- **Azure Cognitive Services**: Document intelligence for POD
- **Azure Machine Learning**: Delivery time predictions

---

## 3. Domain Model

### 3.1 Core Entities

#### 3.1.1 ShipperList Entity
```csharp
public class ShipperList : AggregateRoot
{
    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    public string ListNumber { get; private set; }
    public ShipperListStatus Status { get; private set; }
    public DateTime GeneratedDate { get; private set; }
    public DateTime? FinalizedDate { get; private set; }
    public Guid GeneratedBy { get; private set; }
    public string Notes { get; private set; }

    private readonly List<ShipperListItem> _items;
    public IReadOnlyCollection<ShipperListItem> Items => _items.AsReadOnly();

    // Domain methods
    public void AddItem(Guid inventoryItemId, int quantity, string description);
    public void RemoveItem(Guid itemId);
    public void UpdateItemQuantity(Guid itemId, int newQuantity);
    public void MarkItemAsPacked(Guid itemId, Guid packedBy);
    public void MarkItemAsLoaded(Guid itemId, Guid loadedBy);
    public void Finalize(Guid finalizedBy);
    public void Export(ExportFormat format);
    public void Print();
}

public enum ShipperListStatus
{
    Draft,
    InProgress,
    Finalized,
    Printed,
    Exported,
    Shipped
}
```

#### 3.1.2 ShipperListItem Entity
```csharp
public class ShipperListItem : Entity
{
    public Guid Id { get; private set; }
    public Guid ShipperListId { get; private set; }
    public Guid InventoryItemId { get; private set; }
    public string ItemName { get; private set; }
    public string ItemCode { get; private set; }
    public int QuantityOrdered { get; private set; }
    public int QuantityPacked { get; private set; }
    public bool IsPacked { get; private set; }
    public bool IsLoaded { get; private set; }
    public DateTime? PackedDate { get; private set; }
    public DateTime? LoadedDate { get; private set; }
    public Guid? PackedBy { get; private set; }
    public Guid? LoadedBy { get; private set; }
    public string Notes { get; private set; }
}
```

#### 3.1.3 Shipment Entity
```csharp
public class Shipment : AggregateRoot
{
    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    public Guid ShipperListId { get; private set; }
    public TrackingNumber TrackingNumber { get; private set; }
    public ShipmentStatus Status { get; private set; }
    public Guid? DriverId { get; private set; }
    public Address OriginAddress { get; private set; }
    public Address DestinationAddress { get; private set; }
    public DateTime ScheduledDepartureDate { get; private set; }
    public DateTime? ActualDepartureDate { get; private set; }
    public DateTime ScheduledArrivalDate { get; private set; }
    public DateTime? ActualArrivalDate { get; private set; }
    public string VehicleId { get; private set; }
    public string Notes { get; private set; }

    private readonly List<ShipmentItem> _items;
    public IReadOnlyCollection<ShipmentItem> Items => _items.AsReadOnly();

    // Domain methods
    public void AssignDriver(Guid driverId);
    public void DepartWarehouse(DateTime departureTime);
    public void UpdateStatus(ShipmentStatus newStatus);
    public void ArriveAtVenue(DateTime arrivalTime);
    public void RecordException(string exceptionType, string description);
}

public enum ShipmentStatus
{
    Created,
    DriverAssigned,
    Departed,
    InTransit,
    ArrivedAtVenue,
    Delivering,
    Delivered,
    ReturningToWarehouse,
    Completed,
    Cancelled,
    ExceptionReported
}
```

#### 3.1.4 Delivery Entity
```csharp
public class Delivery : Entity
{
    public Guid Id { get; private set; }
    public Guid ShipmentId { get; private set; }
    public Guid EventId { get; private set; }
    public Address DeliveryAddress { get; private set; }
    public DeliveryWindow DeliveryWindow { get; private set; }
    public DeliveryStatus Status { get; private set; }
    public DateTime? DeliveredDate { get; private set; }
    public Signature RecipientSignature { get; private set; }
    public string RecipientName { get; private set; }
    public string RecipientTitle { get; private set; }
    public string DeliveryNotes { get; private set; }
    public string ExceptionReason { get; private set; }
    public DateTime? RescheduledDate { get; private set; }

    private readonly List<DeliveryItem> _items;
    public IReadOnlyCollection<DeliveryItem> Items => _items.AsReadOnly();

    // Domain methods
    public void DeliverItem(Guid itemId, int quantity);
    public void CaptureSignature(Signature signature, string recipientName);
    public void ReportException(string exceptionType, string reason);
    public void Reschedule(DateTime newDeliveryDate);
    public void CompleteDelivery();
}

public enum DeliveryStatus
{
    Scheduled,
    InProgress,
    PartiallyDelivered,
    Delivered,
    SignatureReceived,
    ExceptionReported,
    Rescheduled
}
```

#### 3.1.5 ReturnItem Entity
```csharp
public class ReturnItem : Entity
{
    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    public Guid InventoryItemId { get; private set; }
    public Guid ShipmentId { get; private set; }
    public ReturnStatus Status { get; private set; }
    public DateTime ScheduledPickupDate { get; private set; }
    public DateTime? ActualPickupDate { get; private set; }
    public DateTime? ReturnedToWarehouseDate { get; private set; }
    public int QuantityExpected { get; private set; }
    public int QuantityReturned { get; private set; }
    public bool IsDamaged { get; private set; }
    public bool IsLost { get; private set; }
    public string DamageDescription { get; private set; }
    public string DamagePhotos { get; private set; }
    public string Notes { get; private set; }

    // Domain methods
    public void SchedulePickup(DateTime pickupDate);
    public void RecordPickup(DateTime pickupTime, int quantity);
    public void ReturnToWarehouse(DateTime returnTime);
    public void RecordDamage(string description, string photoUrls);
    public void RecordLoss(string reason);
}

public enum ReturnStatus
{
    PickupScheduled,
    PickedUp,
    InTransit,
    ReturnedToWarehouse,
    Damaged,
    Lost,
    Completed
}
```

### 3.2 Value Objects

#### 3.2.1 Address
```csharp
public record Address
{
    public string Street { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string ZipCode { get; init; }
    public string Country { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
}
```

#### 3.2.2 TrackingNumber
```csharp
public record TrackingNumber
{
    public string Value { get; init; }

    public static TrackingNumber Generate()
    {
        return new TrackingNumber
        {
            Value = $"SHP-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}"
        };
    }
}
```

#### 3.2.3 DeliveryWindow
```csharp
public record DeliveryWindow
{
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }

    public bool IsWithinWindow(DateTime time)
    {
        return time >= StartTime && time <= EndTime;
    }
}
```

#### 3.2.4 Signature
```csharp
public record Signature
{
    public string Base64Data { get; init; }
    public string BlobStorageUrl { get; init; }
    public DateTime CapturedAt { get; init; }
    public string CapturedBy { get; init; }
}
```

---

## 4. API Specifications

### 4.1 RESTful API Endpoints

#### 4.1.1 Shipper List Management

**Generate Shipper List**
```http
POST /api/v1/shipper-lists
Authorization: Bearer {token}
Content-Type: application/json

{
  "eventId": "guid",
  "generatedBy": "guid",
  "notes": "string"
}

Response: 201 Created
{
  "id": "guid",
  "listNumber": "SHIP-20251222-ABC123",
  "eventId": "guid",
  "status": "Draft",
  "generatedDate": "2025-12-22T10:00:00Z",
  "generatedBy": "guid"
}
```

**Add Item to Shipper List**
```http
POST /api/v1/shipper-lists/{id}/items
Authorization: Bearer {token}
Content-Type: application/json

{
  "inventoryItemId": "guid",
  "quantity": 50,
  "itemName": "Folding Chairs",
  "itemCode": "CHAIR-001"
}

Response: 200 OK
{
  "itemId": "guid",
  "shipperListId": "guid",
  "inventoryItemId": "guid",
  "itemName": "Folding Chairs",
  "quantity": 50,
  "isPacked": false,
  "isLoaded": false
}
```

**Update Item Quantity**
```http
PATCH /api/v1/shipper-lists/{id}/items/{itemId}/quantity
Authorization: Bearer {token}
Content-Type: application/json

{
  "newQuantity": 75
}

Response: 200 OK
```

**Mark Item as Packed**
```http
POST /api/v1/shipper-lists/{id}/items/{itemId}/pack
Authorization: Bearer {token}
Content-Type: application/json

{
  "packedBy": "guid",
  "quantityPacked": 50,
  "notes": "Packed in 5 boxes"
}

Response: 200 OK
```

**Mark Item as Loaded**
```http
POST /api/v1/shipper-lists/{id}/items/{itemId}/load
Authorization: Bearer {token}
Content-Type: application/json

{
  "loadedBy": "guid",
  "notes": "Loaded onto truck #123"
}

Response: 200 OK
```

**Finalize Shipper List**
```http
POST /api/v1/shipper-lists/{id}/finalize
Authorization: Bearer {token}
Content-Type: application/json

{
  "finalizedBy": "guid"
}

Response: 200 OK
```

**Export Shipper List**
```http
GET /api/v1/shipper-lists/{id}/export?format={pdf|excel|csv}
Authorization: Bearer {token}

Response: 200 OK
Content-Type: application/pdf
[Binary PDF data]
```

**Print Shipper List**
```http
POST /api/v1/shipper-lists/{id}/print
Authorization: Bearer {token}

Response: 200 OK
```

**Get Shipper List**
```http
GET /api/v1/shipper-lists/{id}
Authorization: Bearer {token}

Response: 200 OK
{
  "id": "guid",
  "listNumber": "SHIP-20251222-ABC123",
  "eventId": "guid",
  "status": "Finalized",
  "generatedDate": "2025-12-22T10:00:00Z",
  "finalizedDate": "2025-12-22T12:00:00Z",
  "generatedBy": "guid",
  "items": [
    {
      "id": "guid",
      "inventoryItemId": "guid",
      "itemName": "Folding Chairs",
      "quantity": 50,
      "isPacked": true,
      "isLoaded": true
    }
  ]
}
```

#### 4.1.2 Shipment Management

**Create Shipment**
```http
POST /api/v1/shipments
Authorization: Bearer {token}
Content-Type: application/json

{
  "eventId": "guid",
  "shipperListId": "guid",
  "originAddress": {
    "street": "123 Warehouse St",
    "city": "Seattle",
    "state": "WA",
    "zipCode": "98101",
    "country": "USA"
  },
  "destinationAddress": {
    "street": "456 Venue Ave",
    "city": "Seattle",
    "state": "WA",
    "zipCode": "98102",
    "country": "USA"
  },
  "scheduledDepartureDate": "2025-12-23T08:00:00Z",
  "scheduledArrivalDate": "2025-12-23T10:00:00Z",
  "vehicleId": "TRUCK-001"
}

Response: 201 Created
{
  "id": "guid",
  "trackingNumber": "SHP-20251222-XYZ789",
  "status": "Created",
  "eventId": "guid",
  "shipperListId": "guid"
}
```

**Assign Driver**
```http
POST /api/v1/shipments/{id}/assign-driver
Authorization: Bearer {token}
Content-Type: application/json

{
  "driverId": "guid"
}

Response: 200 OK
```

**Depart Warehouse**
```http
POST /api/v1/shipments/{id}/depart
Authorization: Bearer {token}
Content-Type: application/json

{
  "departureTime": "2025-12-23T08:15:00Z",
  "notes": "All items loaded"
}

Response: 200 OK
```

**Update Shipment Status**
```http
PATCH /api/v1/shipments/{id}/status
Authorization: Bearer {token}
Content-Type: application/json

{
  "status": "InTransit",
  "notes": "On highway I-5"
}

Response: 200 OK
```

**Arrive at Venue**
```http
POST /api/v1/shipments/{id}/arrive
Authorization: Bearer {token}
Content-Type: application/json

{
  "arrivalTime": "2025-12-23T10:05:00Z"
}

Response: 200 OK
```

**Track Shipment**
```http
GET /api/v1/shipments/{id}/track
Authorization: Bearer {token}

Response: 200 OK
{
  "id": "guid",
  "trackingNumber": "SHP-20251222-XYZ789",
  "status": "InTransit",
  "currentLocation": {
    "latitude": 47.6062,
    "longitude": -122.3321,
    "address": "I-5 North, Seattle, WA"
  },
  "estimatedArrival": "2025-12-23T10:05:00Z",
  "timeline": [
    {
      "status": "Created",
      "timestamp": "2025-12-22T14:00:00Z"
    },
    {
      "status": "DriverAssigned",
      "timestamp": "2025-12-22T15:00:00Z"
    },
    {
      "status": "Departed",
      "timestamp": "2025-12-23T08:15:00Z"
    }
  ]
}
```

**Get Active Shipments**
```http
GET /api/v1/shipments?status=InTransit&eventId={guid}
Authorization: Bearer {token}

Response: 200 OK
{
  "items": [
    {
      "id": "guid",
      "trackingNumber": "SHP-20251222-XYZ789",
      "status": "InTransit",
      "driverName": "John Doe",
      "estimatedArrival": "2025-12-23T10:05:00Z"
    }
  ],
  "totalCount": 1,
  "pageSize": 20,
  "currentPage": 1
}
```

#### 4.1.3 Delivery Management

**Deliver Item**
```http
POST /api/v1/deliveries/{id}/items/{itemId}/deliver
Authorization: Bearer {token}
Content-Type: application/json

{
  "quantityDelivered": 50,
  "deliveredBy": "guid",
  "notes": "Delivered to loading dock"
}

Response: 200 OK
```

**Capture Delivery Signature**
```http
POST /api/v1/deliveries/{id}/signature
Authorization: Bearer {token}
Content-Type: application/json

{
  "signatureData": "base64-encoded-signature",
  "recipientName": "Jane Smith",
  "recipientTitle": "Event Manager",
  "capturedAt": "2025-12-23T10:30:00Z"
}

Response: 200 OK
{
  "signatureUrl": "https://storage.azure.com/signatures/guid.png"
}
```

**Report Delivery Exception**
```http
POST /api/v1/deliveries/{id}/exceptions
Authorization: Bearer {token}
Content-Type: application/json

{
  "exceptionType": "AccessRestricted",
  "description": "Venue gate locked, cannot access loading area",
  "reportedBy": "guid"
}

Response: 200 OK
```

**Reschedule Delivery**
```http
POST /api/v1/deliveries/{id}/reschedule
Authorization: Bearer {token}
Content-Type: application/json

{
  "newDeliveryDate": "2025-12-24T09:00:00Z",
  "reason": "Venue not ready",
  "rescheduledBy": "guid"
}

Response: 200 OK
```

**Get Delivery Status**
```http
GET /api/v1/deliveries/{id}
Authorization: Bearer {token}

Response: 200 OK
{
  "id": "guid",
  "shipmentId": "guid",
  "status": "Delivered",
  "deliveredDate": "2025-12-23T10:30:00Z",
  "recipientName": "Jane Smith",
  "signatureUrl": "https://storage.azure.com/signatures/guid.png",
  "items": [
    {
      "itemId": "guid",
      "itemName": "Folding Chairs",
      "quantityOrdered": 50,
      "quantityDelivered": 50,
      "status": "Delivered"
    }
  ]
}
```

#### 4.1.4 Return Management

**Schedule Pickup**
```http
POST /api/v1/returns/schedule-pickup
Authorization: Bearer {token}
Content-Type: application/json

{
  "eventId": "guid",
  "shipmentId": "guid",
  "scheduledPickupDate": "2025-12-26T14:00:00Z",
  "items": [
    {
      "inventoryItemId": "guid",
      "quantityExpected": 50
    }
  ]
}

Response: 201 Created
{
  "returnItems": [
    {
      "id": "guid",
      "status": "PickupScheduled",
      "scheduledPickupDate": "2025-12-26T14:00:00Z"
    }
  ]
}
```

**Record Pickup**
```http
POST /api/v1/returns/{id}/pickup
Authorization: Bearer {token}
Content-Type: application/json

{
  "pickupTime": "2025-12-26T14:15:00Z",
  "quantityPickedUp": 48,
  "pickedUpBy": "guid",
  "notes": "2 chairs missing"
}

Response: 200 OK
```

**Record Item Returned to Warehouse**
```http
POST /api/v1/returns/{id}/returned
Authorization: Bearer {token}
Content-Type: application/json

{
  "returnTime": "2025-12-26T16:00:00Z",
  "quantityReceived": 48,
  "receivedBy": "guid"
}

Response: 200 OK
```

**Record Damage**
```http
POST /api/v1/returns/{id}/damage
Authorization: Bearer {token}
Content-Type: multipart/form-data

{
  "damageDescription": "5 chairs have broken legs",
  "damagePhotos": [file1, file2, file3]
}

Response: 200 OK
{
  "photoUrls": [
    "https://storage.azure.com/damage-photos/guid-1.jpg",
    "https://storage.azure.com/damage-photos/guid-2.jpg"
  ]
}
```

**Record Loss**
```http
POST /api/v1/returns/{id}/loss
Authorization: Bearer {token}
Content-Type: application/json

{
  "quantityLost": 2,
  "reason": "Items not found at venue",
  "reportedBy": "guid"
}

Response: 200 OK
```

**Get Return Items**
```http
GET /api/v1/returns?eventId={guid}&status=PickupScheduled
Authorization: Bearer {token}

Response: 200 OK
{
  "items": [
    {
      "id": "guid",
      "eventId": "guid",
      "inventoryItemId": "guid",
      "itemName": "Folding Chairs",
      "status": "PickupScheduled",
      "scheduledPickupDate": "2025-12-26T14:00:00Z",
      "quantityExpected": 50
    }
  ],
  "totalCount": 1
}
```

### 4.2 SignalR Real-Time Hub

**ShipmentTrackingHub**
```csharp
public class ShipmentTrackingHub : Hub
{
    // Client subscribes to shipment updates
    public async Task SubscribeToShipment(Guid shipmentId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"shipment-{shipmentId}");
    }

    // Client unsubscribes from shipment updates
    public async Task UnsubscribeFromShipment(Guid shipmentId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"shipment-{shipmentId}");
    }

    // Server sends location update to subscribed clients
    public async Task SendLocationUpdate(Guid shipmentId, LocationUpdate update)
    {
        await Clients.Group($"shipment-{shipmentId}")
            .SendAsync("ReceiveLocationUpdate", update);
    }

    // Server sends status update to subscribed clients
    public async Task SendStatusUpdate(Guid shipmentId, StatusUpdate update)
    {
        await Clients.Group($"shipment-{shipmentId}")
            .SendAsync("ReceiveStatusUpdate", update);
    }
}
```

---

## 5. Data Management

### 5.1 Database Schema (Azure SQL)

#### 5.1.1 ShipperLists Table
```sql
CREATE TABLE ShipperLists (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    EventId UNIQUEIDENTIFIER NOT NULL,
    ListNumber NVARCHAR(50) NOT NULL UNIQUE,
    Status NVARCHAR(20) NOT NULL,
    GeneratedDate DATETIME2 NOT NULL,
    FinalizedDate DATETIME2 NULL,
    GeneratedBy UNIQUEIDENTIFIER NOT NULL,
    Notes NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Version ROWVERSION,
    CONSTRAINT FK_ShipperLists_Events FOREIGN KEY (EventId)
        REFERENCES Events(Id)
);

CREATE INDEX IX_ShipperLists_EventId ON ShipperLists(EventId);
CREATE INDEX IX_ShipperLists_Status ON ShipperLists(Status);
CREATE INDEX IX_ShipperLists_GeneratedDate ON ShipperLists(GeneratedDate DESC);
```

#### 5.1.2 ShipperListItems Table
```sql
CREATE TABLE ShipperListItems (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ShipperListId UNIQUEIDENTIFIER NOT NULL,
    InventoryItemId UNIQUEIDENTIFIER NOT NULL,
    ItemName NVARCHAR(200) NOT NULL,
    ItemCode NVARCHAR(50) NOT NULL,
    QuantityOrdered INT NOT NULL,
    QuantityPacked INT NOT NULL DEFAULT 0,
    IsPacked BIT NOT NULL DEFAULT 0,
    IsLoaded BIT NOT NULL DEFAULT 0,
    PackedDate DATETIME2 NULL,
    LoadedDate DATETIME2 NULL,
    PackedBy UNIQUEIDENTIFIER NULL,
    LoadedBy UNIQUEIDENTIFIER NULL,
    Notes NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_ShipperListItems_ShipperLists FOREIGN KEY (ShipperListId)
        REFERENCES ShipperLists(Id) ON DELETE CASCADE,
    CONSTRAINT FK_ShipperListItems_InventoryItems FOREIGN KEY (InventoryItemId)
        REFERENCES InventoryItems(Id)
);

CREATE INDEX IX_ShipperListItems_ShipperListId ON ShipperListItems(ShipperListId);
CREATE INDEX IX_ShipperListItems_InventoryItemId ON ShipperListItems(InventoryItemId);
```

#### 5.1.3 Shipments Table
```sql
CREATE TABLE Shipments (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    EventId UNIQUEIDENTIFIER NOT NULL,
    ShipperListId UNIQUEIDENTIFIER NOT NULL,
    TrackingNumber NVARCHAR(50) NOT NULL UNIQUE,
    Status NVARCHAR(30) NOT NULL,
    DriverId UNIQUEIDENTIFIER NULL,
    OriginStreet NVARCHAR(200) NOT NULL,
    OriginCity NVARCHAR(100) NOT NULL,
    OriginState NVARCHAR(50) NOT NULL,
    OriginZipCode NVARCHAR(20) NOT NULL,
    OriginCountry NVARCHAR(100) NOT NULL,
    OriginLatitude DECIMAL(10, 7) NULL,
    OriginLongitude DECIMAL(10, 7) NULL,
    DestinationStreet NVARCHAR(200) NOT NULL,
    DestinationCity NVARCHAR(100) NOT NULL,
    DestinationState NVARCHAR(50) NOT NULL,
    DestinationZipCode NVARCHAR(20) NOT NULL,
    DestinationCountry NVARCHAR(100) NOT NULL,
    DestinationLatitude DECIMAL(10, 7) NULL,
    DestinationLongitude DECIMAL(10, 7) NULL,
    ScheduledDepartureDate DATETIME2 NOT NULL,
    ActualDepartureDate DATETIME2 NULL,
    ScheduledArrivalDate DATETIME2 NOT NULL,
    ActualArrivalDate DATETIME2 NULL,
    VehicleId NVARCHAR(50) NULL,
    Notes NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Version ROWVERSION,
    CONSTRAINT FK_Shipments_Events FOREIGN KEY (EventId)
        REFERENCES Events(Id),
    CONSTRAINT FK_Shipments_ShipperLists FOREIGN KEY (ShipperListId)
        REFERENCES ShipperLists(Id)
);

CREATE INDEX IX_Shipments_EventId ON Shipments(EventId);
CREATE INDEX IX_Shipments_Status ON Shipments(Status);
CREATE INDEX IX_Shipments_DriverId ON Shipments(DriverId);
CREATE INDEX IX_Shipments_TrackingNumber ON Shipments(TrackingNumber);
```

#### 5.1.4 Deliveries Table
```sql
CREATE TABLE Deliveries (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ShipmentId UNIQUEIDENTIFIER NOT NULL,
    EventId UNIQUEIDENTIFIER NOT NULL,
    DeliveryStreet NVARCHAR(200) NOT NULL,
    DeliveryCity NVARCHAR(100) NOT NULL,
    DeliveryState NVARCHAR(50) NOT NULL,
    DeliveryZipCode NVARCHAR(20) NOT NULL,
    DeliveryCountry NVARCHAR(100) NOT NULL,
    WindowStartTime DATETIME2 NOT NULL,
    WindowEndTime DATETIME2 NOT NULL,
    Status NVARCHAR(30) NOT NULL,
    DeliveredDate DATETIME2 NULL,
    SignatureBase64 NVARCHAR(MAX) NULL,
    SignatureBlobUrl NVARCHAR(500) NULL,
    RecipientName NVARCHAR(200) NULL,
    RecipientTitle NVARCHAR(100) NULL,
    DeliveryNotes NVARCHAR(MAX) NULL,
    ExceptionReason NVARCHAR(MAX) NULL,
    RescheduledDate DATETIME2 NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Deliveries_Shipments FOREIGN KEY (ShipmentId)
        REFERENCES Shipments(Id) ON DELETE CASCADE
);

CREATE INDEX IX_Deliveries_ShipmentId ON Deliveries(ShipmentId);
CREATE INDEX IX_Deliveries_EventId ON Deliveries(EventId);
CREATE INDEX IX_Deliveries_Status ON Deliveries(Status);
```

#### 5.1.5 ReturnItems Table
```sql
CREATE TABLE ReturnItems (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    EventId UNIQUEIDENTIFIER NOT NULL,
    InventoryItemId UNIQUEIDENTIFIER NOT NULL,
    ShipmentId UNIQUEIDENTIFIER NOT NULL,
    Status NVARCHAR(30) NOT NULL,
    ScheduledPickupDate DATETIME2 NOT NULL,
    ActualPickupDate DATETIME2 NULL,
    ReturnedToWarehouseDate DATETIME2 NULL,
    QuantityExpected INT NOT NULL,
    QuantityReturned INT NOT NULL DEFAULT 0,
    IsDamaged BIT NOT NULL DEFAULT 0,
    IsLost BIT NOT NULL DEFAULT 0,
    DamageDescription NVARCHAR(MAX) NULL,
    DamagePhotos NVARCHAR(MAX) NULL, -- JSON array of URLs
    Notes NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_ReturnItems_Events FOREIGN KEY (EventId)
        REFERENCES Events(Id),
    CONSTRAINT FK_ReturnItems_Shipments FOREIGN KEY (ShipmentId)
        REFERENCES Shipments(Id)
);

CREATE INDEX IX_ReturnItems_EventId ON ReturnItems(EventId);
CREATE INDEX IX_ReturnItems_Status ON ReturnItems(Status);
CREATE INDEX IX_ReturnItems_ScheduledPickupDate ON ReturnItems(ScheduledPickupDate);
```

### 5.2 Event Store Schema (Azure Cosmos DB)

#### 5.2.1 Container Configuration
```json
{
  "id": "shipper-management-events",
  "partitionKey": {
    "paths": ["/aggregateId"],
    "kind": "Hash"
  },
  "indexingPolicy": {
    "indexingMode": "consistent",
    "automatic": true,
    "includedPaths": [
      {
        "path": "/*"
      }
    ]
  }
}
```

#### 5.2.2 Event Document Schema
```json
{
  "id": "unique-event-id",
  "aggregateId": "shipper-list-guid",
  "aggregateType": "ShipperList",
  "eventType": "ShipperListGenerated",
  "eventVersion": 1,
  "timestamp": "2025-12-22T10:00:00Z",
  "userId": "user-guid",
  "correlationId": "correlation-guid",
  "causationId": "causation-guid",
  "data": {
    "eventId": "event-guid",
    "listNumber": "SHIP-20251222-ABC123",
    "generatedBy": "user-guid",
    "notes": "Initial shipper list for corporate event"
  },
  "metadata": {
    "ipAddress": "192.168.1.1",
    "userAgent": "Mozilla/5.0...",
    "sessionId": "session-guid"
  }
}
```

### 5.3 Entity Framework Configuration

#### 5.3.1 DbContext
```csharp
public class ShipperManagementDbContext : DbContext
{
    public DbSet<ShipperList> ShipperLists { get; set; }
    public DbSet<ShipperListItem> ShipperListItems { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<ReturnItem> ReturnItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ShipperListConfiguration());
        modelBuilder.ApplyConfiguration(new ShipmentConfiguration());
        modelBuilder.ApplyConfiguration(new DeliveryConfiguration());
        modelBuilder.ApplyConfiguration(new ReturnItemConfiguration());
    }
}
```

#### 5.3.2 Entity Configurations
```csharp
public class ShipperListConfiguration : IEntityTypeConfiguration<ShipperList>
{
    public void Configure(EntityTypeBuilder<ShipperList> builder)
    {
        builder.ToTable("ShipperLists");

        builder.HasKey(sl => sl.Id);

        builder.Property(sl => sl.ListNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(sl => sl.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.HasMany(sl => sl.Items)
            .WithOne()
            .HasForeignKey(item => item.ShipperListId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(sl => sl.Version)
            .IsRowVersion();
    }
}
```

---

## 6. Azure Services Integration

### 6.1 Azure Service Bus

#### 6.1.1 Topics and Subscriptions
```csharp
// Topic: shipper-management-events
// Subscriptions:
// - notification-service (sends notifications)
// - analytics-service (tracks metrics)
// - audit-service (logs events)

public class AzureServiceBusPublisher : IEventPublisher
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSender _sender;

    public async Task PublishAsync<T>(T domainEvent) where T : DomainEvent
    {
        var message = new ServiceBusMessage
        {
            Body = BinaryData.FromObjectAsJson(domainEvent),
            ContentType = "application/json",
            Subject = domainEvent.GetType().Name,
            MessageId = domainEvent.Id.ToString(),
            CorrelationId = domainEvent.CorrelationId
        };

        message.ApplicationProperties.Add("EventType", domainEvent.GetType().Name);
        message.ApplicationProperties.Add("AggregateId", domainEvent.AggregateId);
        message.ApplicationProperties.Add("Timestamp", domainEvent.Timestamp);

        await _sender.SendMessageAsync(message);
    }
}
```

### 6.2 Azure Blob Storage

#### 6.2.1 Container Structure
```
shipper-management/
├── signatures/
│   ├── 2025/
│   │   └── 12/
│   │       └── {delivery-id}-{timestamp}.png
├── damage-photos/
│   ├── 2025/
│   │   └── 12/
│   │       └── {return-item-id}-{timestamp}.jpg
└── exports/
    ├── shipper-lists/
    │   └── {shipper-list-id}-{timestamp}.pdf
    └── reports/
        └── {report-type}-{date}.xlsx
```

#### 6.2.2 Blob Service Implementation
```csharp
public class AzureBlobStorageService : IStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public async Task<string> UploadSignatureAsync(
        Guid deliveryId,
        string base64Data)
    {
        var containerClient = _blobServiceClient
            .GetBlobContainerClient("shipper-management");

        var blobName = $"signatures/{DateTime.UtcNow:yyyy/MM}/{deliveryId}-{DateTime.UtcNow:yyyyMMddHHmmss}.png";
        var blobClient = containerClient.GetBlobClient(blobName);

        var imageBytes = Convert.FromBase64String(base64Data);
        using var stream = new MemoryStream(imageBytes);

        await blobClient.UploadAsync(stream, new BlobHttpHeaders
        {
            ContentType = "image/png"
        });

        return blobClient.Uri.ToString();
    }

    public async Task<string> UploadDamagePhotoAsync(
        Guid returnItemId,
        Stream photoStream)
    {
        var containerClient = _blobServiceClient
            .GetBlobContainerClient("shipper-management");

        var blobName = $"damage-photos/{DateTime.UtcNow:yyyy/MM}/{returnItemId}-{Guid.NewGuid()}.jpg";
        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.UploadAsync(photoStream, new BlobHttpHeaders
        {
            ContentType = "image/jpeg"
        });

        return blobClient.Uri.ToString();
    }
}
```

### 6.3 Azure Key Vault

#### 6.3.1 Secrets Management
```csharp
public class AzureKeyVaultService : ISecretsManager
{
    private readonly SecretClient _secretClient;

    public async Task<string> GetSecretAsync(string secretName)
    {
        var secret = await _secretClient.GetSecretAsync(secretName);
        return secret.Value.Value;
    }
}

// Secrets stored:
// - AzureSQLConnectionString
// - AzureServiceBusConnectionString
// - AzureCosmosDBConnectionString
// - AzureBlobStorageConnectionString
// - AzureMapsApiKey
// - AzureOpenAIApiKey
// - SendGridApiKey
// - TwilioApiKey
```

### 6.4 Azure Maps

#### 6.4.1 Geolocation and Routing
```csharp
public class AzureMapsService : IGeolocationService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public async Task<Coordinates> GeocodeAddressAsync(Address address)
    {
        var query = $"{address.Street}, {address.City}, {address.State} {address.ZipCode}";
        var url = $"https://atlas.microsoft.com/search/address/json?api-version=1.0&query={query}&subscription-key={_apiKey}";

        var response = await _httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<GeocodeResponse>();

        return new Coordinates
        {
            Latitude = result.Results[0].Position.Lat,
            Longitude = result.Results[0].Position.Lon
        };
    }

    public async Task<RouteInfo> CalculateRouteAsync(
        Coordinates origin,
        Coordinates destination)
    {
        var url = $"https://atlas.microsoft.com/route/directions/json?api-version=1.0&query={origin.Latitude},{origin.Longitude}:{destination.Latitude},{destination.Longitude}&subscription-key={_apiKey}";

        var response = await _httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<RouteResponse>();

        return new RouteInfo
        {
            DistanceInMeters = result.Routes[0].Summary.LengthInMeters,
            TravelTimeInSeconds = result.Routes[0].Summary.TravelTimeInSeconds,
            Geometry = result.Routes[0].Legs[0].Points
        };
    }
}
```

### 6.5 Azure SignalR Service

#### 6.5.1 Configuration
```csharp
public static class SignalRConfiguration
{
    public static IServiceCollection AddAzureSignalR(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSignalR()
            .AddAzureSignalR(options =>
            {
                options.ConnectionString = configuration["Azure:SignalR:ConnectionString"];
            });

        return services;
    }
}
```

---

## 7. Azure AI Integration

### 7.1 Azure OpenAI Service

#### 7.1.1 Route Optimization with GPT-4
```csharp
public class RouteOptimizationService : IRouteOptimizationService
{
    private readonly OpenAIClient _openAIClient;

    public async Task<OptimizedRoute> OptimizeDeliveryRouteAsync(
        List<Delivery> deliveries)
    {
        var prompt = BuildOptimizationPrompt(deliveries);

        var chatCompletionsOptions = new ChatCompletionsOptions
        {
            DeploymentName = "gpt-4",
            Messages =
            {
                new ChatRequestSystemMessage("You are a logistics optimization expert."),
                new ChatRequestUserMessage(prompt)
            },
            Temperature = 0.3f,
            MaxTokens = 2000
        };

        var response = await _openAIClient.GetChatCompletionsAsync(chatCompletionsOptions);
        var optimizedRoute = ParseOptimizationResponse(response.Value.Choices[0].Message.Content);

        return optimizedRoute;
    }

    private string BuildOptimizationPrompt(List<Delivery> deliveries)
    {
        return $@"
Given the following deliveries for today, optimize the route to minimize travel time and meet delivery windows:

{string.Join("\n", deliveries.Select((d, i) => $@"
Delivery {i + 1}:
- Address: {d.DeliveryAddress.Street}, {d.DeliveryAddress.City}
- Coordinates: {d.DeliveryAddress.Latitude}, {d.DeliveryAddress.Longitude}
- Delivery Window: {d.DeliveryWindow.StartTime:HH:mm} - {d.DeliveryWindow.EndTime:HH:mm}
- Priority: {d.Priority}
"))}

Provide the optimized delivery sequence and estimated times in JSON format.";
    }
}
```

#### 7.1.2 Delivery Exception Analysis
```csharp
public class DeliveryExceptionAnalyzer : IExceptionAnalyzer
{
    private readonly OpenAIClient _openAIClient;

    public async Task<ExceptionAnalysis> AnalyzeExceptionAsync(
        string exceptionDescription,
        List<string> historicalExceptions)
    {
        var prompt = $@"
Analyze the following delivery exception and provide recommendations:

Current Exception: {exceptionDescription}

Similar Past Exceptions:
{string.Join("\n", historicalExceptions)}

Provide:
1. Root cause analysis
2. Recommended immediate action
3. Prevention strategies
4. Estimated resolution time

Format the response as JSON.";

        var chatCompletionsOptions = new ChatCompletionsOptions
        {
            DeploymentName = "gpt-4",
            Messages =
            {
                new ChatRequestSystemMessage("You are a logistics problem-solving expert."),
                new ChatRequestUserMessage(prompt)
            },
            Temperature = 0.5f
        };

        var response = await _openAIClient.GetChatCompletionsAsync(chatCompletionsOptions);
        return JsonSerializer.Deserialize<ExceptionAnalysis>(
            response.Value.Choices[0].Message.Content);
    }
}
```

### 7.2 Azure Cognitive Services

#### 7.2.1 Document Intelligence (Form Recognizer)
```csharp
public class ProofOfDeliveryService : IDocumentIntelligenceService
{
    private readonly DocumentAnalysisClient _documentClient;

    public async Task<DeliveryDocumentData> ExtractDeliveryDataAsync(
        string documentUrl)
    {
        var operation = await _documentClient.AnalyzeDocumentFromUriAsync(
            WaitUntil.Completed,
            "prebuilt-document",
            new Uri(documentUrl));

        var result = operation.Value;

        return new DeliveryDocumentData
        {
            RecipientName = ExtractField(result, "RecipientName"),
            DeliveryDate = DateTime.Parse(ExtractField(result, "Date")),
            ItemsDelivered = ExtractTable(result, "ItemsTable"),
            Notes = ExtractField(result, "Notes")
        };
    }
}
```

### 7.3 Azure Machine Learning

#### 7.3.1 Delivery Time Prediction
```csharp
public class DeliveryTimePredictionService : IPredictionService
{
    private readonly HttpClient _httpClient;
    private readonly string _scoringEndpoint;
    private readonly string _apiKey;

    public async Task<TimeSpan> PredictDeliveryTimeAsync(
        DeliveryPredictionInput input)
    {
        var requestData = new
        {
            input_data = new
            {
                columns = new[]
                {
                    "distance_km",
                    "traffic_level",
                    "weather_condition",
                    "time_of_day",
                    "day_of_week",
                    "num_items"
                },
                data = new[]
                {
                    new object[]
                    {
                        input.DistanceKm,
                        input.TrafficLevel,
                        input.WeatherCondition,
                        input.TimeOfDay,
                        input.DayOfWeek,
                        input.NumItems
                    }
                }
            }
        };

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);

        var response = await _httpClient.PostAsJsonAsync(
            _scoringEndpoint,
            requestData);

        var result = await response.Content.ReadFromJsonAsync<PredictionResponse>();

        return TimeSpan.FromMinutes(result.Predictions[0]);
    }
}
```

---

## 8. Event-Driven Architecture

### 8.1 Domain Events

#### 8.1.1 Shipper List Events
```csharp
public record ShipperListGenerated : DomainEvent
{
    public Guid ShipperListId { get; init; }
    public Guid EventId { get; init; }
    public string ListNumber { get; init; }
    public Guid GeneratedBy { get; init; }
    public DateTime GeneratedDate { get; init; }
}

public record ShipperListUpdated : DomainEvent
{
    public Guid ShipperListId { get; init; }
    public DateTime UpdatedDate { get; init; }
}

public record ShipperListFinalized : DomainEvent
{
    public Guid ShipperListId { get; init; }
    public Guid FinalizedBy { get; init; }
    public DateTime FinalizedDate { get; init; }
}

public record ItemAddedToShipperList : DomainEvent
{
    public Guid ShipperListId { get; init; }
    public Guid ItemId { get; init; }
    public Guid InventoryItemId { get; init; }
    public int Quantity { get; init; }
}

public record ItemMarkedAsPacked : DomainEvent
{
    public Guid ShipperListId { get; init; }
    public Guid ItemId { get; init; }
    public Guid PackedBy { get; init; }
    public int QuantityPacked { get; init; }
    public DateTime PackedDate { get; init; }
}

public record ItemMarkedAsLoaded : DomainEvent
{
    public Guid ShipperListId { get; init; }
    public Guid ItemId { get; init; }
    public Guid LoadedBy { get; init; }
    public DateTime LoadedDate { get; init; }
}
```

#### 8.1.2 Shipment Events
```csharp
public record ShipmentCreatedForEvent : DomainEvent
{
    public Guid ShipmentId { get; init; }
    public Guid EventId { get; init; }
    public string TrackingNumber { get; init; }
    public DateTime ScheduledDepartureDate { get; init; }
}

public record ShipmentAssignedToDriver : DomainEvent
{
    public Guid ShipmentId { get; init; }
    public Guid DriverId { get; init; }
    public DateTime AssignedDate { get; init; }
}

public record ShipmentDepartedWarehouse : DomainEvent
{
    public Guid ShipmentId { get; init; }
    public DateTime DepartureTime { get; init; }
    public string VehicleId { get; init; }
}

public record ShipmentInTransit : DomainEvent
{
    public Guid ShipmentId { get; init; }
    public double CurrentLatitude { get; init; }
    public double CurrentLongitude { get; init; }
    public DateTime Timestamp { get; init; }
}

public record ShipmentArrivedAtVenue : DomainEvent
{
    public Guid ShipmentId { get; init; }
    public DateTime ArrivalTime { get; init; }
}
```

#### 8.1.3 Delivery Events
```csharp
public record ItemDeliveredToVenue : DomainEvent
{
    public Guid DeliveryId { get; init; }
    public Guid ItemId { get; init; }
    public int QuantityDelivered { get; init; }
    public DateTime DeliveredDate { get; init; }
}

public record AllItemsDeliveredToVenue : DomainEvent
{
    public Guid DeliveryId { get; init; }
    public Guid ShipmentId { get; init; }
    public DateTime CompletedDate { get; init; }
}

public record DeliverySignatureReceived : DomainEvent
{
    public Guid DeliveryId { get; init; }
    public string SignatureUrl { get; init; }
    public string RecipientName { get; init; }
    public DateTime CapturedAt { get; init; }
}

public record DeliveryExceptionReported : DomainEvent
{
    public Guid DeliveryId { get; init; }
    public string ExceptionType { get; init; }
    public string Description { get; init; }
    public DateTime ReportedDate { get; init; }
}

public record DeliveryRescheduled : DomainEvent
{
    public Guid DeliveryId { get; init; }
    public DateTime OriginalDate { get; init; }
    public DateTime NewDate { get; init; }
    public string Reason { get; init; }
}
```

#### 8.1.4 Return Events
```csharp
public record ItemPickupScheduled : DomainEvent
{
    public Guid ReturnItemId { get; init; }
    public Guid EventId { get; init; }
    public DateTime ScheduledPickupDate { get; init; }
}

public record ItemPickedUpFromVenue : DomainEvent
{
    public Guid ReturnItemId { get; init; }
    public DateTime PickupTime { get; init; }
    public int QuantityPickedUp { get; init; }
}

public record ItemReturnedToWarehouse : DomainEvent
{
    public Guid ReturnItemId { get; init; }
    public DateTime ReturnTime { get; init; }
    public int QuantityReceived { get; init; }
}

public record ItemDamagedDuringReturn : DomainEvent
{
    public Guid ReturnItemId { get; init; }
    public string DamageDescription { get; init; }
    public List<string> PhotoUrls { get; init; }
}

public record ItemLostDuringReturn : DomainEvent
{
    public Guid ReturnItemId { get; init; }
    public int QuantityLost { get; init; }
    public string Reason { get; init; }
}

public record AllItemsReturnedFromEvent : DomainEvent
{
    public Guid EventId { get; init; }
    public Guid ShipmentId { get; init; }
    public DateTime CompletedDate { get; init; }
    public int TotalItemsReturned { get; init; }
}
```

### 8.2 Event Handlers

#### 8.2.1 Notification Event Handler
```csharp
public class NotificationEventHandler :
    INotificationHandler<ShipmentDepartedWarehouse>,
    INotificationHandler<ShipmentArrivedAtVenue>,
    INotificationHandler<DeliveryExceptionReported>
{
    private readonly INotificationService _notificationService;

    public async Task Handle(
        ShipmentDepartedWarehouse notification,
        CancellationToken cancellationToken)
    {
        await _notificationService.SendSMSAsync(
            notification.DriverId,
            $"Shipment {notification.ShipmentId} has departed at {notification.DepartureTime}");
    }

    public async Task Handle(
        ShipmentArrivedAtVenue notification,
        CancellationToken cancellationToken)
    {
        await _notificationService.SendEmailAsync(
            notification.EventManagerEmail,
            "Shipment Arrived",
            $"Your shipment has arrived at {notification.ArrivalTime}");
    }

    public async Task Handle(
        DeliveryExceptionReported notification,
        CancellationToken cancellationToken)
    {
        await _notificationService.SendUrgentAlertAsync(
            notification.SupervisorId,
            $"Delivery exception: {notification.ExceptionType} - {notification.Description}");
    }
}
```

---

## 9. Security & Authentication

### 9.1 Authentication
- **Azure AD B2C**: User authentication
- **JWT Bearer Tokens**: API authentication
- **Role-Based Access Control (RBAC)**: Authorization

### 9.2 Authorization Policies
```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("CanGenerateShipperList", policy =>
        policy.RequireRole("EventManager", "LogisticsCoordinator"));

    options.AddPolicy("CanAssignDriver", policy =>
        policy.RequireRole("LogisticsCoordinator", "WarehouseManager"));

    options.AddPolicy("CanCaptureSignature", policy =>
        policy.RequireRole("Driver", "DeliveryPersonnel"));

    options.AddPolicy("CanManageReturns", policy =>
        policy.RequireRole("WarehouseManager", "LogisticsCoordinator"));
});
```

### 9.3 Data Encryption
- **At Rest**: Azure SQL TDE, Cosmos DB encryption
- **In Transit**: TLS 1.3
- **Sensitive Fields**: Field-level encryption for signature data

---

## 10. Performance & Scalability

### 10.1 Caching Strategy
```csharp
public class CachedShipperListRepository : IShipperListRepository
{
    private readonly IDistributedCache _cache;
    private readonly ShipperListRepository _repository;

    public async Task<ShipperList> GetByIdAsync(Guid id)
    {
        var cacheKey = $"shipperlist:{id}";
        var cached = await _cache.GetStringAsync(cacheKey);

        if (cached != null)
        {
            return JsonSerializer.Deserialize<ShipperList>(cached);
        }

        var shipperList = await _repository.GetByIdAsync(id);

        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(shipperList),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
            });

        return shipperList;
    }
}
```

### 10.2 Database Optimization
- **Indexes**: Created on frequently queried columns
- **Partitioning**: Cosmos DB partitioned by aggregateId
- **Connection Pooling**: Configured for optimal throughput

### 10.3 API Rate Limiting
```csharp
services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
        httpContext => RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? "anonymous",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
});
```

---

## 11. Monitoring & Logging

### 11.1 Application Insights
```csharp
services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = configuration["ApplicationInsights:ConnectionString"];
    options.EnableAdaptiveSampling = true;
    options.EnableQuickPulseMetricStream = true;
});
```

### 11.2 Structured Logging
```csharp
_logger.LogInformation(
    "Shipment {ShipmentId} departed warehouse at {DepartureTime}",
    shipmentId,
    departureTime);

_logger.LogWarning(
    "Delivery exception for shipment {ShipmentId}: {ExceptionType}",
    shipmentId,
    exceptionType);
```

### 11.3 Health Checks
```csharp
services.AddHealthChecks()
    .AddSqlServer(configuration["ConnectionStrings:AzureSQL"])
    .AddAzureServiceBusTopic(configuration["Azure:ServiceBus:ConnectionString"], "shipper-management-events")
    .AddAzureCosmosDB(configuration["Azure:CosmosDB:ConnectionString"]);
```

---

## 12. Error Handling

### 12.1 Global Exception Handling
```csharp
public class ExceptionHandlingMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            await HandleDomainExceptionAsync(context, ex);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleUnexpectedExceptionAsync(context, ex);
        }
    }
}
```

### 12.2 Error Response Format
```json
{
  "type": "https://api.example.com/errors/validation",
  "title": "Validation Error",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "instance": "/api/v1/shipper-lists",
  "errors": {
    "quantity": ["Quantity must be greater than 0"]
  },
  "traceId": "00-trace-id-00"
}
```

---

## 13. Testing Strategy

### 13.1 Unit Tests
```csharp
[Fact]
public void ShipperList_AddItem_ShouldAddItemToList()
{
    // Arrange
    var shipperList = new ShipperList(Guid.NewGuid(), Guid.NewGuid());

    // Act
    shipperList.AddItem(Guid.NewGuid(), 50, "Folding Chairs");

    // Assert
    Assert.Single(shipperList.Items);
}
```

### 13.2 Integration Tests
```csharp
[Fact]
public async Task CreateShipment_ShouldPublishShipmentCreatedEvent()
{
    // Arrange
    var command = new CreateShipmentCommand { ... };

    // Act
    var result = await _mediator.Send(command);

    // Assert
    var publishedEvent = await _testHarness.Published
        .SelectAsync<ShipmentCreatedForEvent>()
        .FirstOrDefault();

    Assert.NotNull(publishedEvent);
}
```

### 13.3 API Tests
```csharp
[Fact]
public async Task POST_CreateShipperList_ReturnsCreated()
{
    // Arrange
    var request = new { eventId = Guid.NewGuid() };

    // Act
    var response = await _client.PostAsJsonAsync("/api/v1/shipper-lists", request);

    // Assert
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
}
```

---

## Appendix A: Configuration Files

### appsettings.json
```json
{
  "ConnectionStrings": {
    "AzureSQL": "Server=tcp:eventmanagement.database.windows.net,1433;Database=ShipperManagement;",
    "CosmosDB": "AccountEndpoint=https://eventmanagement.documents.azure.com:443/;",
    "ServiceBus": "Endpoint=sb://eventmanagement.servicebus.windows.net/;",
    "BlobStorage": "DefaultEndpointsProtocol=https;AccountName=eventmanagementstorage;",
    "Redis": "eventmanagement.redis.cache.windows.net:6380,password=..."
  },
  "Azure": {
    "ApplicationInsights": {
      "ConnectionString": "InstrumentationKey=..."
    },
    "KeyVault": {
      "VaultUri": "https://eventmanagement-kv.vault.azure.net/"
    },
    "SignalR": {
      "ConnectionString": "Endpoint=https://eventmanagement-signalr.service.signalr.net;..."
    },
    "Maps": {
      "SubscriptionKey": "..."
    },
    "OpenAI": {
      "Endpoint": "https://eventmanagement-openai.openai.azure.com/",
      "ApiKey": "...",
      "DeploymentName": "gpt-4"
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

---

**End of Backend Specification Document**
