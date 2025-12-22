# Integration & External System Management - Backend Specification

## Document Information
| Field | Value |
|-------|-------|
| Feature | Integration & External System Management |
| Version | 1.0 |
| Date | 2025-12-22 |
| Status | Final |

---

## 1. Overview

### 1.1 Purpose
The Integration & External System Management module provides comprehensive capabilities for managing external system integrations, webhooks, API keys, and data synchronization within the EventManagementPlatform. This module enables seamless connectivity with payment gateways, email services, SMS providers, calendar systems, and custom third-party applications.

### 1.2 Scope
This specification covers all backend requirements for:
- External system integration management (payment gateways, email/SMS services, calendar sync)
- API key generation, rotation, and revocation
- Webhook configuration and delivery management
- Data synchronization orchestration and monitoring
- Integration health monitoring and alerting
- Secure credential storage and management

### 1.3 Technology Stack
- **Framework**: .NET 8.0
- **Database**: SQL Server Express (via Entity Framework Core)
- **Cloud**: Azure App Service, Azure SQL Database, Azure Key Vault
- **AI Integration**: Azure AI Services for intelligent integration recommendations and anomaly detection
- **Messaging**: MediatR for CQRS pattern implementation
- **Cache**: Azure Redis Cache for integration state management
- **Queue**: Azure Service Bus for webhook delivery and data sync jobs
- **Security**: Azure Key Vault for credential management

---

## 2. Domain Model

### 2.1 Aggregate: Integration
The Integration aggregate represents a configured connection to an external system.

#### 2.1.1 Entity Properties
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| IntegrationId | Guid | Yes | Unique identifier |
| Name | string | Yes | Integration name (max 200 chars) |
| IntegrationType | IntegrationType | Yes | Type of integration |
| Provider | string | Yes | Provider name (max 100 chars) |
| Status | IntegrationStatus | Yes | Current status |
| Configuration | string | Yes | JSON configuration (encrypted) |
| IsEnabled | bool | Yes | Whether integration is active |
| HealthStatus | HealthStatus | Yes | Current health state |
| LastHealthCheck | DateTime | No | Last health check timestamp |
| LastSyncTime | DateTime | No | Last successful sync |
| CreatedAt | DateTime | Yes | Creation timestamp |
| ModifiedAt | DateTime | No | Last modification timestamp |
| CreatedBy | Guid | Yes | User who created |
| ModifiedBy | Guid | No | User who last modified |

#### 2.1.2 IntegrationType Enumeration
```csharp
public enum IntegrationType
{
    PaymentGateway = 0,
    EmailService = 1,
    SMSService = 2,
    CalendarSync = 3,
    CustomAPI = 4,
    CRM = 5,
    Analytics = 6,
    Storage = 7
}
```

#### 2.1.3 IntegrationStatus Enumeration
```csharp
public enum IntegrationStatus
{
    Draft = 0,
    PendingValidation = 1,
    Active = 2,
    Suspended = 3,
    Disconnected = 4,
    Error = 5
}
```

#### 2.1.4 HealthStatus Enumeration
```csharp
public enum HealthStatus
{
    Healthy = 0,
    Degraded = 1,
    Unhealthy = 2,
    Unknown = 3
}
```

### 2.2 Entity: APIKey
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| APIKeyId | Guid | Yes | Unique identifier |
| KeyName | string | Yes | Descriptive name (max 100 chars) |
| KeyHash | string | Yes | SHA256 hash of the key |
| KeyPrefix | string | Yes | First 8 chars for identification |
| Scopes | string | Yes | JSON array of permitted scopes |
| ExpiresAt | DateTime | No | Expiration timestamp |
| IsActive | bool | Yes | Whether key is active |
| LastUsedAt | DateTime | No | Last usage timestamp |
| UsageCount | long | Yes | Total number of uses |
| RateLimitPerHour | int | Yes | Maximum requests per hour |
| CreatedAt | DateTime | Yes | Creation timestamp |
| CreatedBy | Guid | Yes | User who created the key |
| RevokedAt | DateTime | No | Revocation timestamp |
| RevokedBy | Guid | No | User who revoked the key |

### 2.3 Entity: Webhook
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| WebhookId | Guid | Yes | Unique identifier |
| IntegrationId | Guid | No | Associated integration (optional) |
| Url | string | Yes | Target URL (max 500 chars) |
| EventTypes | string | Yes | JSON array of subscribed events |
| Secret | string | Yes | Signing secret (encrypted) |
| IsActive | bool | Yes | Whether webhook is active |
| RetryPolicy | string | Yes | JSON retry configuration |
| MaxRetries | int | Yes | Maximum retry attempts |
| TimeoutSeconds | int | Yes | Request timeout |
| LastTriggeredAt | DateTime | No | Last trigger timestamp |
| SuccessCount | long | Yes | Successful deliveries |
| FailureCount | long | Yes | Failed deliveries |
| CreatedAt | DateTime | Yes | Creation timestamp |
| CreatedBy | Guid | Yes | User who created |
| ModifiedAt | DateTime | No | Last modification timestamp |

### 2.4 Entity: WebhookDelivery
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| WebhookDeliveryId | Guid | Yes | Unique identifier |
| WebhookId | Guid | Yes | Parent webhook reference |
| EventType | string | Yes | Triggered event type |
| Payload | string | Yes | JSON payload sent |
| HttpStatusCode | int | No | Response status code |
| ResponseBody | string | No | Response content |
| AttemptCount | int | Yes | Number of delivery attempts |
| DeliveryStatus | DeliveryStatus | Yes | Current status |
| ErrorMessage | string | No | Error details if failed |
| ScheduledAt | DateTime | Yes | Scheduled delivery time |
| DeliveredAt | DateTime | No | Actual delivery timestamp |
| NextRetryAt | DateTime | No | Next retry timestamp |

#### 2.4.1 DeliveryStatus Enumeration
```csharp
public enum DeliveryStatus
{
    Pending = 0,
    Sending = 1,
    Delivered = 2,
    Failed = 3,
    Retrying = 4,
    Expired = 5
}
```

### 2.5 Entity: DataSyncJob
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| DataSyncJobId | Guid | Yes | Unique identifier |
| IntegrationId | Guid | Yes | Source integration |
| SyncType | SyncType | Yes | Type of synchronization |
| Direction | SyncDirection | Yes | Data flow direction |
| Status | SyncStatus | Yes | Current status |
| RecordsTotal | int | Yes | Total records to sync |
| RecordsProcessed | int | Yes | Records completed |
| RecordsFailed | int | Yes | Records that failed |
| ErrorLog | string | No | JSON error details |
| StartedAt | DateTime | No | Sync start timestamp |
| CompletedAt | DateTime | No | Sync completion timestamp |
| ScheduledAt | DateTime | Yes | Scheduled run time |
| CreatedAt | DateTime | Yes | Creation timestamp |
| CreatedBy | Guid | Yes | User who initiated |

#### 2.5.1 SyncType Enumeration
```csharp
public enum SyncType
{
    Full = 0,
    Incremental = 1,
    Differential = 2
}
```

#### 2.5.2 SyncDirection Enumeration
```csharp
public enum SyncDirection
{
    Import = 0,
    Export = 1,
    Bidirectional = 2
}
```

#### 2.5.3 SyncStatus Enumeration
```csharp
public enum SyncStatus
{
    Scheduled = 0,
    Running = 1,
    Completed = 2,
    Failed = 3,
    Cancelled = 4,
    PartialSuccess = 5
}
```

### 2.6 Entity: IntegrationLog
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| IntegrationLogId | Guid | Yes | Unique identifier |
| IntegrationId | Guid | Yes | Related integration |
| LogLevel | LogLevel | Yes | Severity level |
| Message | string | Yes | Log message |
| Details | string | No | JSON additional details |
| Exception | string | No | Exception stack trace |
| RequestId | Guid | No | Correlation ID |
| CreatedAt | DateTime | Yes | Log timestamp |

---

## 3. Domain Events

### 3.1 Integration Events
| Event | Trigger | Payload |
|-------|---------|---------|
| PaymentGatewayConnected | Payment integration configured | IntegrationId, Provider, ConfiguredBy |
| PaymentGatewayDisconnected | Payment integration removed | IntegrationId, Provider, DisconnectedBy, Reason |
| EmailServiceConnected | Email service integrated | IntegrationId, Provider, ConfiguredBy |
| SMSServiceConnected | SMS service integrated | IntegrationId, Provider, ConfiguredBy |
| CalendarSyncEnabled | Calendar sync activated | IntegrationId, CalendarType, EnabledBy |
| CalendarSyncDisabled | Calendar sync deactivated | IntegrationId, CalendarType, DisabledBy |

### 3.2 API Key Events
| Event | Trigger | Payload |
|-------|---------|---------|
| APIKeyGenerated | New API key created | APIKeyId, KeyName, Scopes, ExpiresAt, CreatedBy |
| APIKeyRevoked | API key invalidated | APIKeyId, KeyName, RevokedBy, RevokedAt, Reason |

### 3.3 Data Sync Events
| Event | Trigger | Payload |
|-------|---------|---------|
| DataSyncInitiated | Sync job started | DataSyncJobId, IntegrationId, SyncType, RecordsTotal |
| DataSyncCompleted | Sync job finished successfully | DataSyncJobId, RecordsProcessed, Duration |
| DataSyncFailed | Sync job encountered errors | DataSyncJobId, RecordsFailed, ErrorMessage |

### 3.4 Webhook Events
| Event | Trigger | Payload |
|-------|---------|---------|
| WebhookReceived | Incoming webhook processed | WebhookId, EventType, SourceIP, ReceivedAt |
| WebhookSent | Outgoing webhook delivered | WebhookDeliveryId, WebhookId, StatusCode, DeliveredAt |
| WebhookDeliveryFailed | Webhook delivery failed | WebhookDeliveryId, AttemptCount, ErrorMessage, NextRetryAt |

---

## 4. Use Cases

### 4.1 Integration Management

#### UC-IM-01: Configure Payment Gateway Integration
**Actor**: Administrator
**Preconditions**: User has admin privileges
**Flow**:
1. User selects payment gateway provider (Stripe, PayPal, Square)
2. User enters API credentials and configuration
3. System validates credentials with provider
4. System encrypts and stores configuration in Azure Key Vault
5. System performs test transaction
6. System activates integration
7. System publishes PaymentGatewayConnected event

**Postconditions**: Payment gateway is active and ready for transactions

#### UC-IM-02: Configure Email/SMS Service Integration
**Actor**: Administrator
**Preconditions**: User has admin privileges
**Flow**:
1. User selects service provider (SendGrid, Twilio, etc.)
2. User enters API credentials
3. System validates credentials
4. System sends test message
5. System stores configuration securely
6. System publishes EmailServiceConnected or SMSServiceConnected event

**Postconditions**: Communication service is active

#### UC-IM-03: Enable Calendar Synchronization
**Actor**: User
**Preconditions**: User has valid calendar account (Google, Outlook, iCal)
**Flow**:
1. User initiates OAuth flow for calendar provider
2. System redirects to provider authentication
3. User grants permissions
4. System receives and stores access token
5. System performs initial calendar sync
6. System publishes CalendarSyncEnabled event

**Postconditions**: Calendar events are synchronized bidirectionally

#### UC-IM-04: Monitor Integration Health
**Actor**: System
**Preconditions**: Active integrations exist
**Flow**:
1. System performs periodic health checks (every 5 minutes)
2. System sends test requests to integration endpoints
3. System measures response time and success rate
4. System updates health status
5. System triggers alerts if health degrades
6. System uses Azure AI to detect anomalies

**Postconditions**: Integration health is current and monitored

### 4.2 API Key Management

#### UC-AK-01: Generate API Key
**Actor**: Administrator, Developer
**Preconditions**: User has appropriate privileges
**Flow**:
1. User specifies key name and scopes
2. User sets optional expiration date
3. User configures rate limits
4. System generates cryptographically secure key
5. System hashes key for storage
6. System displays key (one-time only)
7. System publishes APIKeyGenerated event

**Postconditions**: API key is active and usable

#### UC-AK-02: Revoke API Key
**Actor**: Administrator
**Preconditions**: API key exists
**Flow**:
1. User selects API key to revoke
2. User provides revocation reason
3. System confirms revocation
4. System marks key as inactive
5. System publishes APIKeyRevoked event
6. Active sessions using the key are terminated

**Postconditions**: API key is no longer valid

#### UC-AK-03: Rotate API Key
**Actor**: Administrator, Developer
**Preconditions**: Existing API key
**Flow**:
1. User initiates key rotation
2. System generates new key
3. System provides grace period with both keys active
4. User updates integrations with new key
5. User confirms rotation complete
6. System revokes old key

**Postconditions**: New key is active, old key is revoked

### 4.3 Webhook Management

#### UC-WH-01: Create Webhook Subscription
**Actor**: Administrator, Developer
**Preconditions**: User has appropriate privileges
**Flow**:
1. User provides webhook URL
2. User selects event types to subscribe
3. System generates signing secret
4. User configures retry policy
5. System validates URL accessibility
6. System sends verification request
7. System activates webhook

**Postconditions**: Webhook is active and will receive events

#### UC-WH-02: Process Webhook Delivery
**Actor**: System
**Preconditions**: Webhook is active, event occurs
**Flow**:
1. Domain event is raised
2. System identifies subscribed webhooks
3. System queues delivery to Azure Service Bus
4. System prepares payload with event data
5. System signs payload with webhook secret
6. System sends HTTP POST to webhook URL
7. System records delivery status
8. System publishes WebhookSent or WebhookDeliveryFailed event

**Postconditions**: Webhook delivery is attempted and logged

#### UC-WH-03: Retry Failed Webhook Delivery
**Actor**: System
**Preconditions**: Webhook delivery failed
**Flow**:
1. System checks retry policy
2. System calculates exponential backoff delay
3. System schedules retry
4. System re-attempts delivery at scheduled time
5. System updates attempt count
6. System marks as expired if max retries exceeded

**Postconditions**: Delivery is retried or marked as permanently failed

### 4.4 Data Synchronization

#### UC-DS-01: Initiate Data Sync
**Actor**: User, System (scheduled)
**Preconditions**: Integration is active
**Flow**:
1. User or scheduler initiates sync job
2. System validates integration credentials
3. System determines sync scope (full/incremental)
4. System publishes DataSyncInitiated event
5. System queues sync job to Azure Service Bus
6. Background worker processes sync

**Postconditions**: Sync job is queued for processing

#### UC-DS-02: Execute Data Sync
**Actor**: Background Worker
**Preconditions**: Sync job is queued
**Flow**:
1. Worker dequeues sync job
2. Worker retrieves data from external system
3. Worker transforms data to internal format
4. Worker validates and deduplicates records
5. Worker applies business rules
6. Worker imports data to database
7. Worker updates sync statistics
8. Worker publishes DataSyncCompleted or DataSyncFailed event

**Postconditions**: Data is synchronized and logged

#### UC-DS-03: Monitor Sync Progress
**Actor**: User
**Preconditions**: Sync job is running
**Flow**:
1. User requests sync job status
2. System retrieves current progress
3. System displays records processed, failed, remaining
4. System shows estimated completion time
5. System provides real-time updates via SignalR

**Postconditions**: User has visibility into sync progress

---

## 5. API Endpoints

### 5.1 Integration Endpoints

#### POST /api/integrations
Create new integration
```json
{
  "name": "Stripe Payment Gateway",
  "integrationType": "PaymentGateway",
  "provider": "Stripe",
  "configuration": {
    "apiKey": "sk_test_...",
    "webhookSecret": "whsec_...",
    "currency": "USD"
  }
}
```

#### GET /api/integrations
List all integrations with filters
- Query params: `type`, `status`, `provider`, `page`, `pageSize`

#### GET /api/integrations/{id}
Get integration details

#### PUT /api/integrations/{id}
Update integration configuration

#### DELETE /api/integrations/{id}
Disconnect integration

#### POST /api/integrations/{id}/test
Test integration connection

#### GET /api/integrations/{id}/health
Get current health status

### 5.2 API Key Endpoints

#### POST /api/apikeys
Generate new API key
```json
{
  "keyName": "Mobile App API Key",
  "scopes": ["events:read", "customers:read", "bookings:write"],
  "expiresAt": "2026-12-31T23:59:59Z",
  "rateLimitPerHour": 1000
}
```

#### GET /api/apikeys
List all API keys (hashed values only)

#### GET /api/apikeys/{id}
Get API key details

#### PUT /api/apikeys/{id}
Update API key scopes or rate limits

#### DELETE /api/apikeys/{id}
Revoke API key

#### POST /api/apikeys/{id}/rotate
Rotate API key

### 5.3 Webhook Endpoints

#### POST /api/webhooks
Create webhook subscription
```json
{
  "url": "https://example.com/webhooks/events",
  "eventTypes": [
    "EventCreated",
    "EventCancelled",
    "BookingConfirmed"
  ],
  "retryPolicy": {
    "maxRetries": 5,
    "backoffMultiplier": 2
  },
  "timeoutSeconds": 30
}
```

#### GET /api/webhooks
List webhooks

#### GET /api/webhooks/{id}
Get webhook details

#### PUT /api/webhooks/{id}
Update webhook configuration

#### DELETE /api/webhooks/{id}
Delete webhook

#### GET /api/webhooks/{id}/deliveries
Get delivery history

#### POST /api/webhooks/{id}/test
Send test webhook

#### POST /api/webhooks/incoming/{integrationId}
Receive incoming webhook from external system

### 5.4 Data Sync Endpoints

#### POST /api/datasyncs
Create sync job
```json
{
  "integrationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "syncType": "Incremental",
  "direction": "Import",
  "scheduledAt": "2025-12-22T10:00:00Z"
}
```

#### GET /api/datasyncs
List sync jobs with filters

#### GET /api/datasyncs/{id}
Get sync job details and progress

#### DELETE /api/datasyncs/{id}
Cancel sync job

#### GET /api/datasyncs/{id}/logs
Get sync execution logs

---

## 6. Business Rules

### 6.1 Integration Rules
- BR-IM-01: Only one active integration per provider type allowed (e.g., one payment gateway)
- BR-IM-02: Integration credentials must be validated before activation
- BR-IM-03: Integration health checks must run every 5 minutes
- BR-IM-04: Failed health checks trigger after 3 consecutive failures
- BR-IM-05: Integrations in error state must be manually reviewed
- BR-IM-06: Integration configuration must be encrypted at rest
- BR-IM-07: Integration credentials stored in Azure Key Vault only

### 6.2 API Key Rules
- BR-AK-01: API keys must be minimum 32 characters, cryptographically random
- BR-AK-02: API keys displayed only once at generation
- BR-AK-03: Maximum 100 active API keys per tenant
- BR-AK-04: Expired API keys automatically revoked
- BR-AK-05: Rate limits enforced per API key per hour
- BR-AK-06: API key scopes cannot be expanded, only restricted
- BR-AK-07: Revoked API keys cannot be reactivated

### 6.3 Webhook Rules
- BR-WH-01: Webhook URLs must be HTTPS only (except localhost for testing)
- BR-WH-02: Webhook signatures must be validated using HMAC-SHA256
- BR-WH-03: Maximum 5 retry attempts with exponential backoff
- BR-WH-04: Initial retry after 1 minute, doubling each attempt
- BR-WH-05: Webhook deliveries expire after 24 hours
- BR-WH-06: Failed webhooks after max retries marked as permanently failed
- BR-WH-07: Maximum 50 webhooks per tenant
- BR-WH-08: Webhook payload size limited to 1MB

### 6.4 Data Sync Rules
- BR-DS-01: Full sync maximum once per day per integration
- BR-DS-02: Incremental sync maximum once per hour
- BR-DS-03: Maximum 10,000 records per sync batch
- BR-DS-04: Failed records logged individually
- BR-DS-05: Sync jobs timeout after 60 minutes
- BR-DS-06: Duplicate records detected by external ID
- BR-DS-07: Sync conflicts resolved by last-modified timestamp

---

## 7. Data Validation Rules

### 7.1 Integration Validation
- Name: Required, 1-200 characters
- Provider: Required, 1-100 characters
- Configuration: Required, valid JSON, max 10KB
- URL endpoints: Valid HTTPS URLs

### 7.2 API Key Validation
- KeyName: Required, 1-100 characters
- Scopes: At least one scope required
- RateLimitPerHour: 1-10,000
- ExpiresAt: Must be future date if provided

### 7.3 Webhook Validation
- Url: Required, valid HTTPS URL, max 500 characters
- EventTypes: At least one event type required
- MaxRetries: 0-10
- TimeoutSeconds: 5-300

### 7.4 Data Sync Validation
- IntegrationId: Must reference active integration
- RecordsTotal: Non-negative integer
- ScheduledAt: Cannot be more than 30 days in past

---

## 8. Security Requirements

### 8.1 Authentication & Authorization
- All endpoints require authenticated user
- API key endpoints require `integration:manage` permission
- Integration configuration requires `integration:admin` permission
- Webhook management requires `webhook:manage` permission
- Public webhook receiver endpoint validates signature

### 8.2 Data Protection
- All credentials encrypted using Azure Key Vault
- API keys hashed using SHA256 before storage
- Webhook secrets encrypted at rest
- Integration configurations encrypted at rest
- TLS 1.3 required for all external communications

### 8.3 API Security
- Rate limiting per API key enforced
- Request signing for webhook deliveries
- IP whitelisting for incoming webhooks (optional)
- API key rotation without service interruption
- Audit logging for all sensitive operations

---

## 9. Performance Requirements

### 9.1 Response Times
- Integration list: < 200ms
- Integration creation: < 1s (excluding external validation)
- API key generation: < 500ms
- Webhook delivery: < 5s
- Health check: < 30s

### 9.2 Throughput
- Support 100 concurrent webhook deliveries
- Process 1,000 API requests per minute per key
- Handle 50 concurrent data sync jobs
- Support 10,000 events per minute for webhook distribution

### 9.3 Scalability
- Support 1,000 active integrations per tenant
- Support 100 API keys per tenant
- Support 500 webhook subscriptions per tenant
- Support 100 concurrent sync jobs across all tenants

---

## 10. Azure AI Integration

### 10.1 Intelligent Integration Recommendations
- AI analyzes usage patterns to recommend optimal integrations
- Suggests provider based on event types and volume
- Predicts integration health issues before failures
- Recommends configuration optimizations

### 10.2 Anomaly Detection
- Azure AI monitors integration traffic patterns
- Detects unusual API usage indicating potential security issues
- Identifies data sync anomalies (unexpected volumes, patterns)
- Alerts on webhook delivery patterns suggesting problems

### 10.3 Smart Retry Logic
- AI learns optimal retry timing based on historical success rates
- Adjusts backoff strategies per webhook endpoint
- Predicts best sync times based on external system load

---

## 11. Error Handling

### 11.1 Integration Errors
- Connection failures: Retry with exponential backoff
- Authentication failures: Alert admin, suspend integration
- Configuration errors: Prevent activation until resolved
- Health check failures: Alert after 3 consecutive failures

### 11.2 API Key Errors
- Invalid key: Return 401 Unauthorized
- Expired key: Return 401 with expiration message
- Rate limit exceeded: Return 429 Too Many Requests
- Insufficient scopes: Return 403 Forbidden

### 11.3 Webhook Errors
- Delivery timeout: Retry according to policy
- HTTP 4xx: Log and don't retry (except 429)
- HTTP 5xx: Retry with backoff
- Network errors: Retry with backoff

### 11.4 Sync Errors
- Authentication failure: Alert and suspend sync
- Data validation errors: Log record and continue
- Batch failures: Retry batch with reduced size
- Timeout: Resume from last checkpoint

---

## 12. Monitoring & Logging

### 12.1 Metrics
- Integration health status (per integration)
- API key usage statistics (requests, rate limits)
- Webhook delivery success rate
- Data sync completion rate
- Average sync duration
- Error rates by category

### 12.2 Logging
- All integration configuration changes
- All API key generations and revocations
- All webhook deliveries (success and failure)
- All sync job executions
- All authentication attempts
- All errors with stack traces

### 12.3 Alerting
- Integration health degradation
- Webhook delivery failure rate > 20%
- Data sync failures
- API key usage anomalies
- Security violations

---

## 13. Database Schema

### 13.1 Tables

#### Integrations
```sql
CREATE TABLE Integrations (
    IntegrationId UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    IntegrationType INT NOT NULL,
    Provider NVARCHAR(100) NOT NULL,
    Status INT NOT NULL,
    Configuration NVARCHAR(MAX) NOT NULL, -- Encrypted JSON
    IsEnabled BIT NOT NULL DEFAULT 1,
    HealthStatus INT NOT NULL DEFAULT 3,
    LastHealthCheck DATETIME2 NULL,
    LastSyncTime DATETIME2 NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ModifiedAt DATETIME2 NULL,
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER NULL,
    CONSTRAINT FK_Integration_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(UserId),
    CONSTRAINT FK_Integration_ModifiedBy FOREIGN KEY (ModifiedBy) REFERENCES Users(UserId)
);

CREATE INDEX IX_Integration_Type ON Integrations(IntegrationType);
CREATE INDEX IX_Integration_Status ON Integrations(Status);
CREATE INDEX IX_Integration_Provider ON Integrations(Provider);
```

#### APIKeys
```sql
CREATE TABLE APIKeys (
    APIKeyId UNIQUEIDENTIFIER PRIMARY KEY,
    KeyName NVARCHAR(100) NOT NULL,
    KeyHash NVARCHAR(64) NOT NULL, -- SHA256
    KeyPrefix NVARCHAR(8) NOT NULL,
    Scopes NVARCHAR(MAX) NOT NULL, -- JSON array
    ExpiresAt DATETIME2 NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    LastUsedAt DATETIME2 NULL,
    UsageCount BIGINT NOT NULL DEFAULT 0,
    RateLimitPerHour INT NOT NULL DEFAULT 1000,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    RevokedAt DATETIME2 NULL,
    RevokedBy UNIQUEIDENTIFIER NULL,
    CONSTRAINT FK_APIKey_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(UserId),
    CONSTRAINT FK_APIKey_RevokedBy FOREIGN KEY (RevokedBy) REFERENCES Users(UserId)
);

CREATE UNIQUE INDEX IX_APIKey_Hash ON APIKeys(KeyHash);
CREATE INDEX IX_APIKey_Prefix ON APIKeys(KeyPrefix);
CREATE INDEX IX_APIKey_Active ON APIKeys(IsActive) WHERE IsActive = 1;
```

#### Webhooks
```sql
CREATE TABLE Webhooks (
    WebhookId UNIQUEIDENTIFIER PRIMARY KEY,
    IntegrationId UNIQUEIDENTIFIER NULL,
    Url NVARCHAR(500) NOT NULL,
    EventTypes NVARCHAR(MAX) NOT NULL, -- JSON array
    Secret NVARCHAR(MAX) NOT NULL, -- Encrypted
    IsActive BIT NOT NULL DEFAULT 1,
    RetryPolicy NVARCHAR(MAX) NOT NULL, -- JSON
    MaxRetries INT NOT NULL DEFAULT 3,
    TimeoutSeconds INT NOT NULL DEFAULT 30,
    LastTriggeredAt DATETIME2 NULL,
    SuccessCount BIGINT NOT NULL DEFAULT 0,
    FailureCount BIGINT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    ModifiedAt DATETIME2 NULL,
    CONSTRAINT FK_Webhook_Integration FOREIGN KEY (IntegrationId) REFERENCES Integrations(IntegrationId),
    CONSTRAINT FK_Webhook_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(UserId)
);

CREATE INDEX IX_Webhook_Active ON Webhooks(IsActive) WHERE IsActive = 1;
CREATE INDEX IX_Webhook_Integration ON Webhooks(IntegrationId);
```

#### WebhookDeliveries
```sql
CREATE TABLE WebhookDeliveries (
    WebhookDeliveryId UNIQUEIDENTIFIER PRIMARY KEY,
    WebhookId UNIQUEIDENTIFIER NOT NULL,
    EventType NVARCHAR(100) NOT NULL,
    Payload NVARCHAR(MAX) NOT NULL, -- JSON
    HttpStatusCode INT NULL,
    ResponseBody NVARCHAR(MAX) NULL,
    AttemptCount INT NOT NULL DEFAULT 0,
    DeliveryStatus INT NOT NULL,
    ErrorMessage NVARCHAR(MAX) NULL,
    ScheduledAt DATETIME2 NOT NULL,
    DeliveredAt DATETIME2 NULL,
    NextRetryAt DATETIME2 NULL,
    CONSTRAINT FK_WebhookDelivery_Webhook FOREIGN KEY (WebhookId) REFERENCES Webhooks(WebhookId) ON DELETE CASCADE
);

CREATE INDEX IX_WebhookDelivery_Webhook ON WebhookDeliveries(WebhookId);
CREATE INDEX IX_WebhookDelivery_Status ON WebhookDeliveries(DeliveryStatus);
CREATE INDEX IX_WebhookDelivery_NextRetry ON WebhookDeliveries(NextRetryAt) WHERE NextRetryAt IS NOT NULL;
```

#### DataSyncJobs
```sql
CREATE TABLE DataSyncJobs (
    DataSyncJobId UNIQUEIDENTIFIER PRIMARY KEY,
    IntegrationId UNIQUEIDENTIFIER NOT NULL,
    SyncType INT NOT NULL,
    Direction INT NOT NULL,
    Status INT NOT NULL,
    RecordsTotal INT NOT NULL DEFAULT 0,
    RecordsProcessed INT NOT NULL DEFAULT 0,
    RecordsFailed INT NOT NULL DEFAULT 0,
    ErrorLog NVARCHAR(MAX) NULL, -- JSON
    StartedAt DATETIME2 NULL,
    CompletedAt DATETIME2 NULL,
    ScheduledAt DATETIME2 NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT FK_DataSync_Integration FOREIGN KEY (IntegrationId) REFERENCES Integrations(IntegrationId),
    CONSTRAINT FK_DataSync_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(UserId)
);

CREATE INDEX IX_DataSync_Integration ON DataSyncJobs(IntegrationId);
CREATE INDEX IX_DataSync_Status ON DataSyncJobs(Status);
CREATE INDEX IX_DataSync_Scheduled ON DataSyncJobs(ScheduledAt);
```

#### IntegrationLogs
```sql
CREATE TABLE IntegrationLogs (
    IntegrationLogId UNIQUEIDENTIFIER PRIMARY KEY,
    IntegrationId UNIQUEIDENTIFIER NOT NULL,
    LogLevel INT NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    Details NVARCHAR(MAX) NULL, -- JSON
    Exception NVARCHAR(MAX) NULL,
    RequestId UNIQUEIDENTIFIER NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_IntegrationLog_Integration FOREIGN KEY (IntegrationId) REFERENCES Integrations(IntegrationId)
);

CREATE INDEX IX_IntegrationLog_Integration ON IntegrationLogs(IntegrationId);
CREATE INDEX IX_IntegrationLog_CreatedAt ON IntegrationLogs(CreatedAt);
CREATE INDEX IX_IntegrationLog_Level ON IntegrationLogs(LogLevel);
```

---

## 14. Background Jobs

### 14.1 Scheduled Jobs
- **Integration Health Check**: Every 5 minutes
- **Webhook Retry Processor**: Every 1 minute
- **Data Sync Scheduler**: Every 15 minutes
- **API Key Expiration Check**: Every 1 hour
- **Integration Log Cleanup**: Daily at 2 AM UTC
- **Webhook Delivery Cleanup**: Daily at 3 AM UTC (>30 days old)

### 14.2 Event-Driven Jobs
- **Webhook Delivery Worker**: Triggered by domain events
- **Data Sync Executor**: Triggered by sync job creation
- **Integration Health Alert**: Triggered by health check failures

---

## 15. Testing Requirements

### 15.1 Unit Tests
- All domain event handlers
- All business rule validations
- All data transformations
- Webhook signature generation and validation
- API key hashing and validation

### 15.2 Integration Tests
- Integration configuration and validation
- API key generation and usage
- Webhook delivery and retry logic
- Data sync execution end-to-end
- Azure Key Vault integration
- Azure Service Bus integration

### 15.3 Load Tests
- 1,000 concurrent webhook deliveries
- 10,000 API requests per minute
- 100 concurrent data sync jobs
- Integration health checks at scale

---

## 16. Deployment Considerations

### 16.1 Azure Resources
- Azure App Service (Standard tier minimum)
- Azure SQL Database (Standard S2 minimum)
- Azure Key Vault (Standard tier)
- Azure Service Bus (Standard tier)
- Azure Redis Cache (Standard C1 minimum)
- Azure Application Insights
- Azure AI Services

### 16.2 Configuration
- Connection strings in Azure Key Vault
- Feature flags for integration types
- Rate limit configurations
- Webhook retry policies
- Sync job schedules

### 16.3 Scaling
- Horizontal scaling for webhook workers
- Separate worker pools for sync jobs
- Redis cache for rate limiting state
- Service Bus for reliable message delivery

---

## 17. Compliance & Audit

### 17.1 Audit Requirements
- All integration configuration changes logged
- All API key operations logged
- All webhook deliveries logged
- All data sync operations logged
- User actions tracked with timestamps

### 17.2 Data Retention
- Integration logs: 90 days
- Webhook deliveries: 30 days
- Data sync logs: 180 days
- API key audit trail: 1 year

### 17.3 Compliance
- GDPR: Personal data in sync jobs properly handled
- PCI DSS: Payment integration credentials secured
- SOC 2: Audit trails maintained
- HIPAA: Healthcare integrations properly isolated (if applicable)

---

## 18. Dependencies

### 18.1 Internal Dependencies
- User Management: User authentication and authorization
- Event Management: Event data for webhooks
- Notification Management: Alerts for integration failures

### 18.2 External Dependencies
- Azure Key Vault: Credential storage
- Azure Service Bus: Message queue
- Azure Redis Cache: Rate limiting and state
- Azure AI Services: Intelligent recommendations
- Third-party APIs: Payment gateways, email/SMS services

---

## 19. Future Enhancements

- OAuth 2.0 provider for third-party integrations
- GraphQL API for flexible data queries
- Real-time integration monitoring dashboard
- Advanced data transformation rules engine
- Multi-tenancy support for integration sharing
- Integration marketplace for pre-built connectors
- AI-powered integration testing and validation
- Blockchain integration for audit trails
