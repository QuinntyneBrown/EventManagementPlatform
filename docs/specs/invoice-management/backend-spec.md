# Invoice & Financial Management - Backend Specification

## 1. Executive Summary

### 1.1 Purpose
This Software Requirements Specification (SRS) document describes the backend architecture and implementation requirements for the Invoice & Financial Management feature of the Event Management Platform. The system manages invoices, payments, refunds, and financial tracking using .NET 8, Azure services, and Azure AI.

### 1.2 Scope
The Invoice & Financial Management module provides comprehensive functionality for:
- Invoice lifecycle management (draft, finalize, void, correct)
- Multi-source fee aggregation (staff, invitations, prizes, equipment, additional)
- Payment processing and tracking
- Refund management
- Financial reporting and analytics
- Tax calculation and compliance
- Automated reminders and notifications

### 1.3 Target Audience
- Backend Developers
- Solution Architects
- DevOps Engineers
- QA Engineers
- Product Managers

## 2. System Architecture

### 2.1 Technology Stack

#### Core Framework
- **.NET 8**: Primary backend framework
- **ASP.NET Core Web API**: RESTful API implementation
- **C# 12**: Programming language

#### Azure Services
- **Azure App Service**: Application hosting
- **Azure SQL Database**: Primary data store
- **Azure Cosmos DB**: Event store for domain events
- **Azure Service Bus**: Message queue and event bus
- **Azure Functions**: Background processing and scheduled tasks
- **Azure Key Vault**: Secrets and configuration management
- **Azure Application Insights**: Monitoring and telemetry
- **Azure Blob Storage**: Document storage (PDF invoices)
- **Azure API Management**: API gateway and rate limiting

#### Azure AI Services
- **Azure OpenAI Service**: Invoice analysis and anomaly detection
- **Azure AI Document Intelligence**: OCR for receipt processing
- **Azure Cognitive Services**: Text analytics for payment notes

#### Additional Technologies
- **Entity Framework Core 8**: ORM
- **MediatR**: CQRS pattern implementation
- **FluentValidation**: Request validation
- **AutoMapper**: Object mapping
- **Polly**: Resilience and transient fault handling
- **Serilog**: Structured logging
- **Stripe API / PayPal SDK**: Payment gateway integration
- **QuestPDF**: PDF generation for invoices

### 2.2 Architectural Patterns

#### Domain-Driven Design (DDD)
- Bounded contexts for Invoice, Payment, and Financial Reporting
- Rich domain models with business logic
- Value objects for Money, InvoiceNumber, TaxRate
- Aggregate roots: Invoice, Payment, Refund

#### CQRS (Command Query Responsibility Segregation)
- Separate models for read and write operations
- Commands for state changes
- Queries for data retrieval
- Event sourcing for audit trail

#### Event-Driven Architecture
- Domain events for all state changes
- Event handlers for side effects
- Integration events for cross-bounded context communication
- Event store for complete audit trail

## 3. Domain Model

### 3.1 Core Entities

#### Invoice Aggregate
```
Invoice (Aggregate Root)
├── InvoiceId (GUID)
├── InvoiceNumber (Value Object)
├── EventId (Reference)
├── CustomerId (Reference)
├── Status (Draft, Finalized, Paid, Voided, WrittenOff, PastDue)
├── IssueDate (DateTime)
├── DueDate (DateTime)
├── FinalizedDate (DateTime?)
├── PaidDate (DateTime?)
├── Items (List<InvoiceItem>)
├── Subtotal (Money)
├── TaxAmount (Money)
├── DiscountAmount (Money)
├── TotalAmount (Money)
├── AmountPaid (Money)
├── AmountDue (Money)
├── Notes (string)
├── Terms (string)
├── BillingAddress (Address Value Object)
├── Version (int - for optimistic concurrency)
└── Methods:
    ├── AddItem(InvoiceItem)
    ├── RemoveItem(itemId)
    ├── UpdateItem(itemId, quantity, unitPrice)
    ├── ApplyDiscount(amount, type, reason)
    ├── RemoveDiscount()
    ├── CalculateTax(taxRate)
    ├── Recalculate()
    ├── Finalize()
    ├── Void(reason)
    ├── Correct(originalInvoiceId, reason)
    ├── RecordPayment(amount, method, reference)
    ├── RecordRefund(amount, reason)
    └── WriteOff(reason)
```

#### InvoiceItem (Entity)
```
InvoiceItem
├── ItemId (GUID)
├── InvoiceId (GUID)
├── FeeType (Staff, Invitation, Prize, Equipment, Additional)
├── Description (string)
├── ReferenceId (GUID? - references source entity)
├── Quantity (decimal)
├── UnitPrice (Money)
├── LineTotal (Money)
├── TaxRate (decimal?)
├── TaxAmount (Money)
├── DiscountAmount (Money)
└── SortOrder (int)
```

#### Payment Aggregate
```
Payment (Aggregate Root)
├── PaymentId (GUID)
├── InvoiceId (GUID)
├── Amount (Money)
├── PaymentMethod (CreditCard, BankTransfer, Check, Cash, PayPal, Stripe)
├── Status (Pending, Successful, Failed, Refunded, PartiallyRefunded)
├── TransactionId (string)
├── ProcessedDate (DateTime)
├── PaymentGateway (string)
├── PaymentDetails (PaymentDetails Value Object)
├── FailureReason (string?)
├── RetryCount (int)
├── Notes (string)
└── Methods:
    ├── Process()
    ├── Fail(reason)
    ├── Retry()
    ├── Refund(amount, reason)
    └── PartialRefund(amount, reason)
```

#### Refund (Entity)
```
Refund
├── RefundId (GUID)
├── PaymentId (GUID)
├── InvoiceId (GUID)
├── Amount (Money)
├── RefundMethod (ToOriginalPaymentMethod, StoreCredit, Check)
├── Status (Pending, Processed, Failed)
├── Reason (string)
├── ProcessedDate (DateTime?)
├── TransactionId (string?)
└── Notes (string)
```

### 3.2 Value Objects

```csharp
public record Money(decimal Amount, string Currency)
{
    public static Money Zero(string currency = "USD") => new(0, currency);
    public Money Add(Money other) => new(Amount + other.Amount, Currency);
    public Money Subtract(Money other) => new(Amount - other.Amount, Currency);
    public Money Multiply(decimal factor) => new(Amount * factor, Currency);
}

public record InvoiceNumber(string Value)
{
    public static InvoiceNumber Generate(int year, int sequence)
        => new($"INV-{year}-{sequence:D6}");
}

public record Address(
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country
);

public record TaxRate(decimal Rate, string Description);

public record PaymentDetails(
    string? Last4Digits,
    string? CardType,
    string? AccountNumber,
    string? PayPalEmail
);
```

## 4. Domain Events

### 4.1 Invoice Creation & Modification Events

#### InvoiceDraftCreated
```csharp
public record InvoiceDraftCreated(
    Guid InvoiceId,
    Guid EventId,
    Guid CustomerId,
    DateTime IssueDate,
    DateTime DueDate,
    Address BillingAddress,
    DateTime OccurredAt
) : IDomainEvent;
```

#### InvoiceItemAdded
```csharp
public record InvoiceItemAdded(
    Guid InvoiceId,
    Guid ItemId,
    string FeeType,
    string Description,
    Guid? ReferenceId,
    decimal Quantity,
    Money UnitPrice,
    DateTime OccurredAt
) : IDomainEvent;
```

#### InvoiceItemRemoved
```csharp
public record InvoiceItemRemoved(
    Guid InvoiceId,
    Guid ItemId,
    DateTime OccurredAt
) : IDomainEvent;
```

#### InvoiceItemUpdated
```csharp
public record InvoiceItemUpdated(
    Guid InvoiceId,
    Guid ItemId,
    decimal NewQuantity,
    Money NewUnitPrice,
    DateTime OccurredAt
) : IDomainEvent;
```

#### InvoiceRecalculated
```csharp
public record InvoiceRecalculated(
    Guid InvoiceId,
    Money Subtotal,
    Money TaxAmount,
    Money DiscountAmount,
    Money TotalAmount,
    DateTime OccurredAt
) : IDomainEvent;
```

#### InvoiceFinalized
```csharp
public record InvoiceFinalized(
    Guid InvoiceId,
    string InvoiceNumber,
    Money TotalAmount,
    DateTime FinalizedDate,
    DateTime OccurredAt
) : IDomainEvent;
```

#### InvoiceVoided
```csharp
public record InvoiceVoided(
    Guid InvoiceId,
    string Reason,
    DateTime VoidedDate,
    DateTime OccurredAt
) : IDomainEvent;
```

#### InvoiceCorrected
```csharp
public record InvoiceCorrected(
    Guid NewInvoiceId,
    Guid OriginalInvoiceId,
    string Reason,
    DateTime OccurredAt
) : IDomainEvent;
```

### 4.2 Fee Addition Events

#### StaffFeesAddedToInvoice
```csharp
public record StaffFeesAddedToInvoice(
    Guid InvoiceId,
    List<Guid> StaffAssignmentIds,
    Money TotalStaffFees,
    DateTime OccurredAt
) : IDomainEvent;
```

#### InvitationFeesAddedToInvoice
```csharp
public record InvitationFeesAddedToInvoice(
    Guid InvoiceId,
    List<Guid> InvitationIds,
    Money TotalInvitationFees,
    DateTime OccurredAt
) : IDomainEvent;
```

#### PrizeFeesAddedToInvoice
```csharp
public record PrizeFeesAddedToInvoice(
    Guid InvoiceId,
    List<Guid> PrizeIds,
    Money TotalPrizeFees,
    DateTime OccurredAt
) : IDomainEvent;
```

#### EquipmentRentalFeesAddedToInvoice
```csharp
public record EquipmentRentalFeesAddedToInvoice(
    Guid InvoiceId,
    List<Guid> EquipmentRentalIds,
    Money TotalRentalFees,
    DateTime OccurredAt
) : IDomainEvent;
```

#### AdditionalFeesAddedToInvoice
```csharp
public record AdditionalFeesAddedToInvoice(
    Guid InvoiceId,
    List<Guid> AdditionalFeeIds,
    Money TotalAdditionalFees,
    DateTime OccurredAt
) : IDomainEvent;
```

### 4.3 Discount & Tax Events

#### DiscountAppliedToInvoice
```csharp
public record DiscountAppliedToInvoice(
    Guid InvoiceId,
    Money DiscountAmount,
    string DiscountType, // Percentage, FixedAmount
    string Reason,
    DateTime OccurredAt
) : IDomainEvent;
```

#### DiscountRemovedFromInvoice
```csharp
public record DiscountRemovedFromInvoice(
    Guid InvoiceId,
    DateTime OccurredAt
) : IDomainEvent;
```

#### TaxCalculated
```csharp
public record TaxCalculated(
    Guid InvoiceId,
    decimal TaxRate,
    Money TaxableAmount,
    Money TaxAmount,
    string TaxJurisdiction,
    DateTime OccurredAt
) : IDomainEvent;
```

### 4.4 Invoice Distribution Events

#### InvoiceSentToCustomer
```csharp
public record InvoiceSentToCustomer(
    Guid InvoiceId,
    string CustomerEmail,
    string DeliveryMethod, // Email, Mail, Portal
    DateTime SentDate,
    DateTime OccurredAt
) : IDomainEvent;
```

#### InvoiceViewed
```csharp
public record InvoiceViewed(
    Guid InvoiceId,
    Guid? UserId,
    string IpAddress,
    DateTime ViewedDate,
    DateTime OccurredAt
) : IDomainEvent;
```

#### InvoiceDownloaded
```csharp
public record InvoiceDownloaded(
    Guid InvoiceId,
    Guid? UserId,
    string FileFormat, // PDF, Excel, CSV
    DateTime DownloadedDate,
    DateTime OccurredAt
) : IDomainEvent;
```

#### InvoiceReminderSent
```csharp
public record InvoiceReminderSent(
    Guid InvoiceId,
    string ReminderType, // FirstReminder, SecondReminder, FinalNotice
    string CustomerEmail,
    DateTime SentDate,
    DateTime OccurredAt
) : IDomainEvent;
```

#### InvoicePrintRequested
```csharp
public record InvoicePrintRequested(
    Guid InvoiceId,
    Guid RequestedByUserId,
    DateTime RequestedDate,
    DateTime OccurredAt
) : IDomainEvent;
```

### 4.5 Payment Events

#### PaymentReceived
```csharp
public record PaymentReceived(
    Guid PaymentId,
    Guid InvoiceId,
    Money Amount,
    string PaymentMethod,
    string TransactionId,
    DateTime ProcessedDate,
    DateTime OccurredAt
) : IDomainEvent;
```

#### PartialPaymentReceived
```csharp
public record PartialPaymentReceived(
    Guid PaymentId,
    Guid InvoiceId,
    Money AmountPaid,
    Money RemainingAmount,
    DateTime ProcessedDate,
    DateTime OccurredAt
) : IDomainEvent;
```

#### PaymentRefunded
```csharp
public record PaymentRefunded(
    Guid PaymentId,
    Guid RefundId,
    Guid InvoiceId,
    Money RefundAmount,
    string Reason,
    DateTime RefundedDate,
    DateTime OccurredAt
) : IDomainEvent;
```

#### PartialRefundIssued
```csharp
public record PartialRefundIssued(
    Guid PaymentId,
    Guid RefundId,
    Guid InvoiceId,
    Money RefundAmount,
    Money RemainingRefundableAmount,
    string Reason,
    DateTime IssuedDate,
    DateTime OccurredAt
) : IDomainEvent;
```

#### PaymentFailed
```csharp
public record PaymentFailed(
    Guid PaymentId,
    Guid InvoiceId,
    Money Amount,
    string PaymentMethod,
    string FailureReason,
    DateTime FailedDate,
    DateTime OccurredAt
) : IDomainEvent;
```

#### PaymentRetried
```csharp
public record PaymentRetried(
    Guid PaymentId,
    Guid InvoiceId,
    int RetryAttempt,
    DateTime RetriedDate,
    DateTime OccurredAt
) : IDomainEvent;
```

#### InvoiceFullyPaid
```csharp
public record InvoiceFullyPaid(
    Guid InvoiceId,
    Money TotalAmount,
    Money TotalPaid,
    List<Guid> PaymentIds,
    DateTime PaidDate,
    DateTime OccurredAt
) : IDomainEvent;
```

#### InvoiceOverpaid
```csharp
public record InvoiceOverpaid(
    Guid InvoiceId,
    Money TotalAmount,
    Money TotalPaid,
    Money OverpaymentAmount,
    DateTime OccurredAt
) : IDomainEvent;
```

#### OverpaymentRefunded
```csharp
public record OverpaymentRefunded(
    Guid InvoiceId,
    Guid RefundId,
    Money OverpaymentAmount,
    DateTime RefundedDate,
    DateTime OccurredAt
) : IDomainEvent;
```

#### InvoicePastDue
```csharp
public record InvoicePastDue(
    Guid InvoiceId,
    DateTime DueDate,
    Money AmountDue,
    int DaysPastDue,
    DateTime OccurredAt
) : IDomainEvent;
```

#### InvoiceWrittenOff
```csharp
public record InvoiceWrittenOff(
    Guid InvoiceId,
    Money Amount,
    string Reason,
    Guid ApprovedByUserId,
    DateTime WrittenOffDate,
    DateTime OccurredAt
) : IDomainEvent;
```

## 5. API Endpoints

### 5.1 Invoice Management

#### Create Draft Invoice
```
POST /api/v1/invoices
Authorization: Bearer {token}
Content-Type: application/json

Request:
{
  "eventId": "guid",
  "customerId": "guid",
  "dueDate": "2025-12-31T00:00:00Z",
  "billingAddress": {
    "street": "123 Main St",
    "city": "Seattle",
    "state": "WA",
    "postalCode": "98101",
    "country": "USA"
  },
  "notes": "Event invoice",
  "terms": "Net 30"
}

Response: 201 Created
{
  "invoiceId": "guid",
  "invoiceNumber": null,
  "status": "Draft",
  "issueDate": "2025-12-22T10:00:00Z",
  "dueDate": "2025-12-31T00:00:00Z",
  "totalAmount": {
    "amount": 0,
    "currency": "USD"
  }
}
```

#### Add Invoice Item
```
POST /api/v1/invoices/{invoiceId}/items
Authorization: Bearer {token}
Content-Type: application/json

Request:
{
  "feeType": "Staff",
  "description": "Event Staff - John Doe (8 hours)",
  "referenceId": "staff-assignment-guid",
  "quantity": 8,
  "unitPrice": {
    "amount": 50.00,
    "currency": "USD"
  }
}

Response: 200 OK
{
  "itemId": "guid",
  "lineTotal": {
    "amount": 400.00,
    "currency": "USD"
  }
}
```

#### Add Multiple Fee Sources to Invoice
```
POST /api/v1/invoices/{invoiceId}/fees/bulk
Authorization: Bearer {token}
Content-Type: application/json

Request:
{
  "staffAssignmentIds": ["guid1", "guid2"],
  "invitationIds": ["guid3"],
  "prizeIds": ["guid4", "guid5"],
  "equipmentRentalIds": ["guid6"],
  "additionalFeeIds": []
}

Response: 200 OK
{
  "itemsAdded": 6,
  "totalFeesAdded": {
    "amount": 2500.00,
    "currency": "USD"
  }
}
```

#### Apply Discount
```
POST /api/v1/invoices/{invoiceId}/discount
Authorization: Bearer {token}
Content-Type: application/json

Request:
{
  "discountType": "Percentage",
  "value": 10,
  "reason": "Early payment discount"
}

Response: 200 OK
{
  "discountAmount": {
    "amount": 250.00,
    "currency": "USD"
  },
  "newTotal": {
    "amount": 2250.00,
    "currency": "USD"
  }
}
```

#### Calculate Tax
```
POST /api/v1/invoices/{invoiceId}/calculate-tax
Authorization: Bearer {token}
Content-Type: application/json

Request:
{
  "taxRate": 8.5,
  "taxJurisdiction": "WA State"
}

Response: 200 OK
{
  "taxableAmount": {
    "amount": 2250.00,
    "currency": "USD"
  },
  "taxAmount": {
    "amount": 191.25,
    "currency": "USD"
  },
  "totalWithTax": {
    "amount": 2441.25,
    "currency": "USD"
  }
}
```

#### Finalize Invoice
```
POST /api/v1/invoices/{invoiceId}/finalize
Authorization: Bearer {token}

Response: 200 OK
{
  "invoiceId": "guid",
  "invoiceNumber": "INV-2025-000123",
  "status": "Finalized",
  "finalizedDate": "2025-12-22T10:30:00Z",
  "totalAmount": {
    "amount": 2441.25,
    "currency": "USD"
  },
  "pdfUrl": "https://storage.blob.core.windows.net/invoices/INV-2025-000123.pdf"
}
```

#### Void Invoice
```
POST /api/v1/invoices/{invoiceId}/void
Authorization: Bearer {token}
Content-Type: application/json

Request:
{
  "reason": "Customer cancelled event"
}

Response: 200 OK
{
  "invoiceId": "guid",
  "status": "Voided",
  "voidedDate": "2025-12-22T11:00:00Z"
}
```

#### Create Corrective Invoice
```
POST /api/v1/invoices/{invoiceId}/correct
Authorization: Bearer {token}
Content-Type: application/json

Request:
{
  "reason": "Incorrect pricing on original invoice",
  "modifications": [
    {
      "itemId": "guid",
      "newUnitPrice": {
        "amount": 45.00,
        "currency": "USD"
      }
    }
  ]
}

Response: 201 Created
{
  "newInvoiceId": "guid",
  "newInvoiceNumber": "INV-2025-000124",
  "originalInvoiceId": "guid",
  "originalInvoiceNumber": "INV-2025-000123"
}
```

#### Send Invoice to Customer
```
POST /api/v1/invoices/{invoiceId}/send
Authorization: Bearer {token}
Content-Type: application/json

Request:
{
  "deliveryMethod": "Email",
  "customerEmail": "customer@example.com",
  "ccEmails": ["manager@example.com"],
  "customMessage": "Thank you for your business"
}

Response: 200 OK
{
  "sentDate": "2025-12-22T12:00:00Z",
  "deliveryMethod": "Email",
  "recipients": ["customer@example.com", "manager@example.com"]
}
```

#### Get Invoice Details
```
GET /api/v1/invoices/{invoiceId}
Authorization: Bearer {token}

Response: 200 OK
{
  "invoiceId": "guid",
  "invoiceNumber": "INV-2025-000123",
  "eventId": "guid",
  "customerId": "guid",
  "status": "Finalized",
  "issueDate": "2025-12-22T10:00:00Z",
  "dueDate": "2025-12-31T00:00:00Z",
  "finalizedDate": "2025-12-22T10:30:00Z",
  "items": [
    {
      "itemId": "guid",
      "feeType": "Staff",
      "description": "Event Staff - John Doe (8 hours)",
      "quantity": 8,
      "unitPrice": {"amount": 50.00, "currency": "USD"},
      "lineTotal": {"amount": 400.00, "currency": "USD"}
    }
  ],
  "subtotal": {"amount": 2500.00, "currency": "USD"},
  "discountAmount": {"amount": 250.00, "currency": "USD"},
  "taxAmount": {"amount": 191.25, "currency": "USD"},
  "totalAmount": {"amount": 2441.25, "currency": "USD"},
  "amountPaid": {"amount": 0.00, "currency": "USD"},
  "amountDue": {"amount": 2441.25, "currency": "USD"},
  "billingAddress": { ... },
  "pdfUrl": "https://...",
  "version": 3
}
```

#### List Invoices
```
GET /api/v1/invoices?status=Finalized&eventId={guid}&page=1&pageSize=20
Authorization: Bearer {token}

Response: 200 OK
{
  "items": [...],
  "totalCount": 45,
  "page": 1,
  "pageSize": 20,
  "totalPages": 3
}
```

### 5.2 Payment Management

#### Process Payment
```
POST /api/v1/payments
Authorization: Bearer {token}
Content-Type: application/json

Request:
{
  "invoiceId": "guid",
  "amount": {
    "amount": 2441.25,
    "currency": "USD"
  },
  "paymentMethod": "CreditCard",
  "paymentGateway": "Stripe",
  "paymentDetails": {
    "stripeToken": "tok_...",
    "saveCard": true
  },
  "notes": "Payment for event services"
}

Response: 200 OK
{
  "paymentId": "guid",
  "status": "Successful",
  "transactionId": "ch_...",
  "processedDate": "2025-12-22T14:00:00Z",
  "amount": {"amount": 2441.25, "currency": "USD"}
}
```

#### Record Manual Payment
```
POST /api/v1/payments/manual
Authorization: Bearer {token}
Content-Type: application/json

Request:
{
  "invoiceId": "guid",
  "amount": {
    "amount": 1200.00,
    "currency": "USD"
  },
  "paymentMethod": "Check",
  "checkNumber": "1234",
  "receivedDate": "2025-12-20T00:00:00Z",
  "notes": "Check received via mail"
}

Response: 200 OK
{
  "paymentId": "guid",
  "status": "Successful",
  "processedDate": "2025-12-22T14:15:00Z"
}
```

#### Retry Failed Payment
```
POST /api/v1/payments/{paymentId}/retry
Authorization: Bearer {token}

Response: 200 OK
{
  "paymentId": "guid",
  "retryAttempt": 2,
  "status": "Pending"
}
```

#### Refund Payment
```
POST /api/v1/payments/{paymentId}/refund
Authorization: Bearer {token}
Content-Type: application/json

Request:
{
  "amount": {
    "amount": 2441.25,
    "currency": "USD"
  },
  "reason": "Event cancelled",
  "refundMethod": "ToOriginalPaymentMethod"
}

Response: 200 OK
{
  "refundId": "guid",
  "status": "Processed",
  "refundAmount": {"amount": 2441.25, "currency": "USD"},
  "processedDate": "2025-12-22T15:00:00Z"
}
```

#### Partial Refund
```
POST /api/v1/payments/{paymentId}/refund
Authorization: Bearer {token}
Content-Type: application/json

Request:
{
  "amount": {
    "amount": 500.00,
    "currency": "USD"
  },
  "reason": "Partial service cancellation",
  "refundMethod": "ToOriginalPaymentMethod"
}

Response: 200 OK
{
  "refundId": "guid",
  "status": "Processed",
  "refundAmount": {"amount": 500.00, "currency": "USD"},
  "remainingRefundableAmount": {"amount": 1941.25, "currency": "USD"}
}
```

#### Get Payment History
```
GET /api/v1/invoices/{invoiceId}/payments
Authorization: Bearer {token}

Response: 200 OK
{
  "invoiceId": "guid",
  "totalAmount": {"amount": 2441.25, "currency": "USD"},
  "amountPaid": {"amount": 2441.25, "currency": "USD"},
  "amountRefunded": {"amount": 0.00, "currency": "USD"},
  "payments": [
    {
      "paymentId": "guid",
      "amount": {"amount": 2441.25, "currency": "USD"},
      "paymentMethod": "CreditCard",
      "status": "Successful",
      "processedDate": "2025-12-22T14:00:00Z",
      "transactionId": "ch_..."
    }
  ],
  "refunds": []
}
```

### 5.3 Financial Reporting

#### Get Financial Dashboard
```
GET /api/v1/financial/dashboard?startDate=2025-01-01&endDate=2025-12-31
Authorization: Bearer {token}

Response: 200 OK
{
  "period": {
    "startDate": "2025-01-01",
    "endDate": "2025-12-31"
  },
  "totalRevenue": {"amount": 125000.00, "currency": "USD"},
  "totalPaid": {"amount": 98000.00, "currency": "USD"},
  "totalOutstanding": {"amount": 27000.00, "currency": "USD"},
  "totalRefunded": {"amount": 3500.00, "currency": "USD"},
  "invoiceCount": 45,
  "paidInvoiceCount": 32,
  "pastDueInvoiceCount": 8,
  "averageInvoiceValue": {"amount": 2777.78, "currency": "USD"},
  "averageDaysToPayment": 18,
  "revenueByMonth": [...],
  "revenueByFeeType": {
    "Staff": {"amount": 45000.00, "currency": "USD"},
    "Equipment": {"amount": 30000.00, "currency": "USD"},
    "Prizes": {"amount": 25000.00, "currency": "USD"},
    "Invitations": {"amount": 15000.00, "currency": "USD"},
    "Additional": {"amount": 10000.00, "currency": "USD"}
  }
}
```

#### Get Aging Report
```
GET /api/v1/financial/aging-report
Authorization: Bearer {token}

Response: 200 OK
{
  "asOfDate": "2025-12-22",
  "current": {"amount": 15000.00, "currency": "USD", "count": 12},
  "days1to30": {"amount": 8000.00, "currency": "USD", "count": 5},
  "days31to60": {"amount": 3000.00, "currency": "USD", "count": 2},
  "days61to90": {"amount": 1000.00, "currency": "USD", "count": 1},
  "over90Days": {"amount": 0.00, "currency": "USD", "count": 0},
  "total": {"amount": 27000.00, "currency": "USD", "count": 20}
}
```

#### Export Financial Data
```
GET /api/v1/financial/export?format=csv&startDate=2025-01-01&endDate=2025-12-31
Authorization: Bearer {token}

Response: 200 OK
Content-Type: text/csv
Content-Disposition: attachment; filename=financial-report-2025.csv

[CSV data]
```

### 5.4 AI-Powered Features

#### Analyze Invoice Anomalies
```
POST /api/v1/invoices/analyze-anomalies
Authorization: Bearer {token}
Content-Type: application/json

Request:
{
  "invoiceIds": ["guid1", "guid2", "guid3"]
}

Response: 200 OK
{
  "anomalies": [
    {
      "invoiceId": "guid1",
      "anomalyType": "UnusualAmount",
      "severity": "Medium",
      "description": "Invoice amount is 3x higher than average for this event type",
      "suggestedAction": "Review pricing with customer"
    },
    {
      "invoiceId": "guid2",
      "anomalyType": "DuplicateItems",
      "severity": "High",
      "description": "Duplicate equipment rental items detected",
      "suggestedAction": "Remove duplicate items"
    }
  ]
}
```

#### Generate Payment Prediction
```
GET /api/v1/invoices/{invoiceId}/payment-prediction
Authorization: Bearer {token}

Response: 200 OK
{
  "invoiceId": "guid",
  "predictedPaymentDate": "2025-12-28",
  "confidenceScore": 0.85,
  "paymentProbability": 0.92,
  "riskFactors": [
    "Customer has 100% payment history",
    "Invoice amount is within normal range"
  ]
}
```

## 6. Data Storage

### 6.1 Azure SQL Database Schema

#### Invoices Table
```sql
CREATE TABLE Invoices (
    InvoiceId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    InvoiceNumber NVARCHAR(50) UNIQUE NULL,
    EventId UNIQUEIDENTIFIER NOT NULL,
    CustomerId UNIQUEIDENTIFIER NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    IssueDate DATETIME2 NOT NULL,
    DueDate DATETIME2 NOT NULL,
    FinalizedDate DATETIME2 NULL,
    PaidDate DATETIME2 NULL,
    VoidedDate DATETIME2 NULL,
    SubtotalAmount DECIMAL(18,2) NOT NULL,
    SubtotalCurrency NVARCHAR(3) NOT NULL DEFAULT 'USD',
    TaxAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    TaxCurrency NVARCHAR(3) NOT NULL DEFAULT 'USD',
    TaxRate DECIMAL(5,2) NULL,
    TaxJurisdiction NVARCHAR(100) NULL,
    DiscountAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    DiscountCurrency NVARCHAR(3) NOT NULL DEFAULT 'USD',
    DiscountType NVARCHAR(20) NULL,
    DiscountReason NVARCHAR(500) NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    TotalCurrency NVARCHAR(3) NOT NULL DEFAULT 'USD',
    AmountPaid DECIMAL(18,2) NOT NULL DEFAULT 0,
    AmountPaidCurrency NVARCHAR(3) NOT NULL DEFAULT 'USD',
    AmountDue DECIMAL(18,2) NOT NULL,
    AmountDueCurrency NVARCHAR(3) NOT NULL DEFAULT 'USD',
    BillingStreet NVARCHAR(200) NULL,
    BillingCity NVARCHAR(100) NULL,
    BillingState NVARCHAR(50) NULL,
    BillingPostalCode NVARCHAR(20) NULL,
    BillingCountry NVARCHAR(50) NULL,
    Notes NVARCHAR(MAX) NULL,
    Terms NVARCHAR(500) NULL,
    PdfBlobUrl NVARCHAR(500) NULL,
    Version INT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    ModifiedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ModifiedBy UNIQUEIDENTIFIER NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,

    CONSTRAINT FK_Invoices_Events FOREIGN KEY (EventId)
        REFERENCES Events(EventId),
    CONSTRAINT FK_Invoices_Customers FOREIGN KEY (CustomerId)
        REFERENCES Customers(CustomerId),
    CONSTRAINT CK_Invoices_Status CHECK (Status IN
        ('Draft', 'Finalized', 'Paid', 'PartiallyPaid', 'Voided', 'WrittenOff', 'PastDue'))
);

CREATE INDEX IX_Invoices_EventId ON Invoices(EventId);
CREATE INDEX IX_Invoices_CustomerId ON Invoices(CustomerId);
CREATE INDEX IX_Invoices_Status ON Invoices(Status);
CREATE INDEX IX_Invoices_DueDate ON Invoices(DueDate);
CREATE INDEX IX_Invoices_InvoiceNumber ON Invoices(InvoiceNumber);
```

#### InvoiceItems Table
```sql
CREATE TABLE InvoiceItems (
    ItemId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    InvoiceId UNIQUEIDENTIFIER NOT NULL,
    FeeType NVARCHAR(50) NOT NULL,
    Description NVARCHAR(500) NOT NULL,
    ReferenceId UNIQUEIDENTIFIER NULL,
    Quantity DECIMAL(18,4) NOT NULL,
    UnitPriceAmount DECIMAL(18,2) NOT NULL,
    UnitPriceCurrency NVARCHAR(3) NOT NULL DEFAULT 'USD',
    LineTotalAmount DECIMAL(18,2) NOT NULL,
    LineTotalCurrency NVARCHAR(3) NOT NULL DEFAULT 'USD',
    TaxRate DECIMAL(5,2) NULL,
    TaxAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    TaxCurrency NVARCHAR(3) NOT NULL DEFAULT 'USD',
    DiscountAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    DiscountCurrency NVARCHAR(3) NOT NULL DEFAULT 'USD',
    SortOrder INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT FK_InvoiceItems_Invoices FOREIGN KEY (InvoiceId)
        REFERENCES Invoices(InvoiceId) ON DELETE CASCADE,
    CONSTRAINT CK_InvoiceItems_FeeType CHECK (FeeType IN
        ('Staff', 'Invitation', 'Prize', 'Equipment', 'Additional'))
);

CREATE INDEX IX_InvoiceItems_InvoiceId ON InvoiceItems(InvoiceId);
CREATE INDEX IX_InvoiceItems_FeeType ON InvoiceItems(FeeType);
CREATE INDEX IX_InvoiceItems_ReferenceId ON InvoiceItems(ReferenceId);
```

#### Payments Table
```sql
CREATE TABLE Payments (
    PaymentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    InvoiceId UNIQUEIDENTIFIER NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Currency NVARCHAR(3) NOT NULL DEFAULT 'USD',
    PaymentMethod NVARCHAR(50) NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    TransactionId NVARCHAR(100) NULL,
    PaymentGateway NVARCHAR(50) NULL,
    ProcessedDate DATETIME2 NULL,
    FailureReason NVARCHAR(500) NULL,
    RetryCount INT NOT NULL DEFAULT 0,
    Last4Digits NVARCHAR(4) NULL,
    CardType NVARCHAR(20) NULL,
    AccountNumber NVARCHAR(100) NULL,
    PayPalEmail NVARCHAR(100) NULL,
    Notes NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    ModifiedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Payments_Invoices FOREIGN KEY (InvoiceId)
        REFERENCES Invoices(InvoiceId),
    CONSTRAINT CK_Payments_Method CHECK (PaymentMethod IN
        ('CreditCard', 'BankTransfer', 'Check', 'Cash', 'PayPal', 'Stripe')),
    CONSTRAINT CK_Payments_Status CHECK (Status IN
        ('Pending', 'Successful', 'Failed', 'Refunded', 'PartiallyRefunded'))
);

CREATE INDEX IX_Payments_InvoiceId ON Payments(InvoiceId);
CREATE INDEX IX_Payments_Status ON Payments(Status);
CREATE INDEX IX_Payments_TransactionId ON Payments(TransactionId);
```

#### Refunds Table
```sql
CREATE TABLE Refunds (
    RefundId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    PaymentId UNIQUEIDENTIFIER NOT NULL,
    InvoiceId UNIQUEIDENTIFIER NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Currency NVARCHAR(3) NOT NULL DEFAULT 'USD',
    RefundMethod NVARCHAR(50) NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    Reason NVARCHAR(500) NOT NULL,
    ProcessedDate DATETIME2 NULL,
    TransactionId NVARCHAR(100) NULL,
    Notes NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,

    CONSTRAINT FK_Refunds_Payments FOREIGN KEY (PaymentId)
        REFERENCES Payments(PaymentId),
    CONSTRAINT FK_Refunds_Invoices FOREIGN KEY (InvoiceId)
        REFERENCES Invoices(InvoiceId),
    CONSTRAINT CK_Refunds_Method CHECK (RefundMethod IN
        ('ToOriginalPaymentMethod', 'StoreCredit', 'Check')),
    CONSTRAINT CK_Refunds_Status CHECK (Status IN
        ('Pending', 'Processed', 'Failed'))
);

CREATE INDEX IX_Refunds_PaymentId ON Refunds(PaymentId);
CREATE INDEX IX_Refunds_InvoiceId ON Refunds(InvoiceId);
```

### 6.2 Azure Cosmos DB - Event Store

#### Container: invoice-events
```json
{
  "id": "unique-event-id",
  "aggregateId": "invoice-id",
  "aggregateType": "Invoice",
  "eventType": "InvoiceFinalized",
  "eventData": {
    "invoiceId": "guid",
    "invoiceNumber": "INV-2025-000123",
    "totalAmount": {
      "amount": 2441.25,
      "currency": "USD"
    },
    "finalizedDate": "2025-12-22T10:30:00Z"
  },
  "occurredAt": "2025-12-22T10:30:00.123Z",
  "version": 3,
  "userId": "user-guid",
  "correlationId": "correlation-guid",
  "metadata": {
    "ipAddress": "192.168.1.1",
    "userAgent": "..."
  }
}
```

Partition Key: `/aggregateId`

## 7. Background Processing

### 7.1 Azure Functions

#### Invoice Reminder Function
```csharp
[FunctionName("SendInvoiceReminders")]
public async Task SendReminders(
    [TimerTrigger("0 0 8 * * *")] TimerInfo timer, // Daily at 8 AM
    ILogger log)
{
    // Find invoices due within 3 days with no reminder sent
    var upcomingDueInvoices = await _invoiceRepository
        .GetUpcomingDueInvoices(daysAhead: 3);

    foreach (var invoice in upcomingDueInvoices)
    {
        await _invoiceService.SendReminderAsync(
            invoice.InvoiceId,
            ReminderType.FirstReminder);
    }

    // Find invoices past due
    var pastDueInvoices = await _invoiceRepository
        .GetPastDueInvoices();

    foreach (var invoice in pastDueInvoices)
    {
        var daysPastDue = (DateTime.UtcNow - invoice.DueDate).Days;

        var reminderType = daysPastDue switch
        {
            <= 7 => ReminderType.FirstReminder,
            <= 14 => ReminderType.SecondReminder,
            _ => ReminderType.FinalNotice
        };

        await _invoiceService.SendReminderAsync(
            invoice.InvoiceId,
            reminderType);
    }
}
```

#### Payment Retry Function
```csharp
[FunctionName("RetryFailedPayments")]
public async Task RetryPayments(
    [TimerTrigger("0 0 */4 * * *")] TimerInfo timer, // Every 4 hours
    ILogger log)
{
    var failedPayments = await _paymentRepository
        .GetRetryablePayments(maxRetries: 3);

    foreach (var payment in failedPayments)
    {
        try
        {
            await _paymentService.RetryPaymentAsync(payment.PaymentId);
        }
        catch (Exception ex)
        {
            log.LogError(ex,
                "Failed to retry payment {PaymentId}",
                payment.PaymentId);
        }
    }
}
```

#### Invoice PDF Generation Function
```csharp
[FunctionName("GenerateInvoicePDF")]
public async Task GeneratePDF(
    [ServiceBusTrigger("invoice-finalized", Connection = "ServiceBusConnection")]
    InvoiceFinalized @event,
    ILogger log)
{
    var invoice = await _invoiceRepository.GetByIdAsync(@event.InvoiceId);

    // Generate PDF using QuestPDF
    var pdfBytes = await _pdfGenerator.GenerateInvoicePdfAsync(invoice);

    // Upload to Azure Blob Storage
    var blobUrl = await _blobStorageService.UploadAsync(
        containerName: "invoices",
        fileName: $"{invoice.InvoiceNumber}.pdf",
        content: pdfBytes,
        contentType: "application/pdf");

    // Update invoice with PDF URL
    await _invoiceRepository.UpdatePdfUrlAsync(
        invoice.InvoiceId,
        blobUrl);
}
```

#### Aging Report Function
```csharp
[FunctionName("UpdateAgingReport")]
public async Task UpdateAging(
    [TimerTrigger("0 0 2 * * *")] TimerInfo timer, // Daily at 2 AM
    ILogger log)
{
    var agingData = await _invoiceRepository.CalculateAgingReport();

    // Cache in Redis for fast retrieval
    await _cache.SetAsync(
        "aging-report:latest",
        agingData,
        TimeSpan.FromHours(24));

    // Store in database for historical tracking
    await _reportRepository.SaveAgingReportAsync(agingData);
}
```

## 8. Integration Patterns

### 8.1 Payment Gateway Integration

#### Stripe Integration
```csharp
public class StripePaymentGateway : IPaymentGateway
{
    private readonly StripeClient _stripeClient;

    public async Task<PaymentResult> ProcessPaymentAsync(
        PaymentRequest request)
    {
        try
        {
            var chargeOptions = new ChargeCreateOptions
            {
                Amount = (long)(request.Amount.Amount * 100), // Convert to cents
                Currency = request.Amount.Currency.ToLower(),
                Source = request.Token,
                Description = $"Invoice {request.InvoiceNumber}",
                Metadata = new Dictionary<string, string>
                {
                    { "InvoiceId", request.InvoiceId.ToString() },
                    { "CustomerId", request.CustomerId.ToString() }
                }
            };

            var charge = await _stripeClient.Charges.CreateAsync(chargeOptions);

            return new PaymentResult
            {
                Success = charge.Status == "succeeded",
                TransactionId = charge.Id,
                ProcessedDate = DateTime.UtcNow,
                FailureReason = charge.FailureMessage
            };
        }
        catch (StripeException ex)
        {
            return new PaymentResult
            {
                Success = false,
                FailureReason = ex.StripeError.Message
            };
        }
    }

    public async Task<RefundResult> RefundPaymentAsync(
        RefundRequest request)
    {
        var refundOptions = new RefundCreateOptions
        {
            Charge = request.TransactionId,
            Amount = (long)(request.Amount.Amount * 100),
            Reason = RefundReasons.RequestedByCustomer
        };

        var refund = await _stripeClient.Refunds.CreateAsync(refundOptions);

        return new RefundResult
        {
            Success = refund.Status == "succeeded",
            TransactionId = refund.Id,
            ProcessedDate = DateTime.UtcNow
        };
    }
}
```

### 8.2 Event Bus Integration

#### Publishing Domain Events
```csharp
public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceBusSender _serviceBusSender;
    private readonly IEventStore _eventStore;

    public async Task PublishAsync<TEvent>(TEvent @event)
        where TEvent : IDomainEvent
    {
        // Store in event store
        await _eventStore.AppendAsync(@event);

        // Publish to Service Bus
        var message = new ServiceBusMessage
        {
            MessageId = @event.EventId.ToString(),
            Subject = typeof(TEvent).Name,
            Body = BinaryData.FromObjectAsJson(@event),
            ContentType = "application/json"
        };

        message.ApplicationProperties.Add("EventType", typeof(TEvent).Name);
        message.ApplicationProperties.Add("AggregateId", @event.AggregateId.ToString());
        message.ApplicationProperties.Add("OccurredAt", @event.OccurredAt);

        await _serviceBusSender.SendMessageAsync(message);
    }
}
```

## 9. Security & Compliance

### 9.1 Authentication & Authorization
- JWT Bearer token authentication
- Role-based access control (RBAC)
- Permissions: Invoice.Create, Invoice.Read, Invoice.Update, Invoice.Void, Payment.Process, Payment.Refund

### 9.2 Data Protection
- Encryption at rest (Azure SQL TDE, Cosmos DB encryption)
- Encryption in transit (TLS 1.3)
- PCI DSS compliance for payment data
- Sensitive data masking (credit card numbers, bank accounts)

### 9.3 Audit Trail
- All domain events stored in event store
- User actions logged with timestamp, user ID, IP address
- Immutable audit log in Cosmos DB

## 10. Performance & Scalability

### 10.1 Caching Strategy
- Redis Cache for frequently accessed invoices
- Cache invalidation on invoice updates
- TTL: 1 hour for invoice details, 24 hours for reports

### 10.2 Database Optimization
- Indexed columns: InvoiceNumber, EventId, CustomerId, Status, DueDate
- Read replicas for reporting queries
- Partitioning strategy for large tables

### 10.3 Rate Limiting
- API Management policies: 1000 requests/minute per user
- Throttling on payment endpoints: 10 requests/minute

## 11. Monitoring & Observability

### 11.1 Application Insights
- Custom metrics: invoice creation rate, payment success rate, average payment time
- Dependency tracking: SQL, Cosmos DB, Service Bus, payment gateways
- Exception tracking and alerting

### 11.2 Health Checks
```csharp
services.AddHealthChecks()
    .AddSqlServer(connectionString)
    .AddAzureServiceBusTopic(serviceBusConnection, "invoice-events")
    .AddAzureBlobStorage(storageConnection);
```

### 11.3 Alerts
- Failed payments > 5% in 15 minutes
- Invoice processing time > 5 seconds
- Payment gateway errors
- Database connection failures

## 12. Testing Strategy

### 12.1 Unit Tests
- Domain model business logic
- Value object validation
- Command and query handlers

### 12.2 Integration Tests
- API endpoints
- Database operations
- Event publishing and handling
- Payment gateway integration (using test mode)

### 12.3 Load Tests
- 1000 concurrent invoice creations
- 500 concurrent payment processing
- Target: < 500ms p95 response time

## 13. Deployment

### 13.1 CI/CD Pipeline
- Azure DevOps / GitHub Actions
- Automated testing on PR
- Staging deployment on merge to develop
- Production deployment on release tag

### 13.2 Infrastructure as Code
- Bicep templates for Azure resources
- Terraform for multi-cloud resources

### 13.3 Database Migrations
- EF Core Migrations
- Versioned migration scripts
- Rollback procedures

## 14. API Versioning
- URL versioning: `/api/v1/invoices`
- Backward compatibility for 2 major versions
- Deprecation notices 6 months in advance

## 15. Error Handling

### 15.1 Error Response Format
```json
{
  "type": "https://api.eventplatform.com/errors/validation-error",
  "title": "Validation Error",
  "status": 400,
  "detail": "Invoice cannot be finalized with zero total amount",
  "instance": "/api/v1/invoices/abc123/finalize",
  "errors": {
    "totalAmount": ["Total amount must be greater than zero"]
  },
  "traceId": "00-abc123-def456-00"
}
```

### 15.2 HTTP Status Codes
- 200 OK: Successful GET/PUT
- 201 Created: Successful POST
- 204 No Content: Successful DELETE
- 400 Bad Request: Validation error
- 401 Unauthorized: Missing/invalid authentication
- 403 Forbidden: Insufficient permissions
- 404 Not Found: Resource not found
- 409 Conflict: Optimistic concurrency violation
- 422 Unprocessable Entity: Business rule violation
- 429 Too Many Requests: Rate limit exceeded
- 500 Internal Server Error: Unexpected error

## 16. Appendix

### 16.1 Glossary
- **Invoice**: A document requesting payment for goods or services
- **Payment**: A transaction where money is received
- **Refund**: Return of payment to customer
- **Write-off**: Accounting action to remove uncollectable debt

### 16.2 References
- PCI DSS Compliance: https://www.pcisecuritystandards.org/
- Stripe API Documentation: https://stripe.com/docs/api
- Azure OpenAI Service: https://azure.microsoft.com/en-us/products/ai-services/openai-service
