# Audit & Compliance - Backend Specification

## Document Information
| Field | Value |
|-------|-------|
| Feature | Audit & Compliance |
| Version | 1.0 |
| Date | 2025-12-22 |
| Status | Final |

---

## 1. Overview

### 1.1 Purpose
The Audit & Compliance module provides comprehensive audit trail management, compliance logging, data lifecycle management (exports, imports, backups), and GDPR/privacy compliance features for the EventManagementPlatform system.

### 1.2 Scope
This specification covers:
- Entity audit trail logging (create, update, delete operations)
- Bulk operation tracking
- Data export/import management
- System backup and restore operations
- Customer data access logging (GDPR compliance)
- Privacy policy and consent management
- Data retention and deletion policies

### 1.3 Technology Stack
- **Framework**: .NET 8.0
- **Database**: SQL Server Express (via Entity Framework Core)
- **Cloud**: Azure App Service, Azure SQL Database
- **AI Integration**: Azure AI Services for anomaly detection in audit logs
- **Storage**: Azure Blob Storage for exports and backups
- **Messaging**: MediatR for CQRS pattern implementation
- **Security**: Azure Key Vault for encryption keys

---

## 2. Domain Model

### 2.1 Aggregate: AuditLog
The AuditLog aggregate tracks all system entity operations.

#### 2.1.1 Entity Properties
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| AuditLogId | Guid | Yes | Unique identifier |
| EntityType | string | Yes | Type of entity (e.g., "Event", "Customer") |
| EntityId | Guid | Yes | ID of the affected entity |
| OperationType | AuditOperationType | Yes | Type of operation performed |
| UserId | Guid | Yes | User who performed the action |
| Timestamp | DateTime | Yes | When the operation occurred |
| Changes | string | No | JSON serialized changes (before/after) |
| IpAddress | string | No | Source IP address |
| UserAgent | string | No | Browser/client user agent |
| CorrelationId | Guid | Yes | For tracking related operations |
| Metadata | string | No | Additional context as JSON |

#### 2.1.2 AuditOperationType Enumeration
```csharp
public enum AuditOperationType
{
    Created = 0,
    Updated = 1,
    Deleted = 2,
    BulkOperation = 3,
    Accessed = 4,
    Exported = 5
}
```

### 2.2 Aggregate: DataExport
Manages data export requests and their lifecycle.

#### 2.2.1 Entity Properties
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| DataExportId | Guid | Yes | Unique identifier |
| RequestedBy | Guid | Yes | User who requested the export |
| RequestedAt | DateTime | Yes | Request timestamp |
| ExportType | ExportType | Yes | Type of export |
| EntityType | string | No | Specific entity type to export |
| FilterCriteria | string | No | JSON filter criteria |
| Status | ExportStatus | Yes | Current export status |
| FileUrl | string | No | Azure Blob Storage URL |
| FileSize | long | No | Size in bytes |
| RecordCount | int | No | Number of records exported |
| CompletedAt | DateTime | No | Completion timestamp |
| ExpiresAt | DateTime | No | When the file will be deleted |
| ErrorMessage | string | No | Error details if failed |

#### 2.2.2 ExportType Enumeration
```csharp
public enum ExportType
{
    AuditLogs = 0,
    CustomerData = 1,
    EventData = 2,
    ComplianceReport = 3,
    FullSystemExport = 4
}
```

#### 2.2.3 ExportStatus Enumeration
```csharp
public enum ExportStatus
{
    Pending = 0,
    InProgress = 1,
    Completed = 2,
    Failed = 3,
    Expired = 4
}
```

### 2.3 Aggregate: DataImport
Manages data import operations.

#### 2.3.1 Entity Properties
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| DataImportId | Guid | Yes | Unique identifier |
| UploadedBy | Guid | Yes | User who uploaded the data |
| UploadedAt | DateTime | Yes | Upload timestamp |
| ImportType | ImportType | Yes | Type of import |
| FileUrl | string | Yes | Source file URL |
| FileSize | long | Yes | File size in bytes |
| Status | ImportStatus | Yes | Current import status |
| RecordCount | int | No | Number of records processed |
| SuccessCount | int | No | Successfully imported records |
| FailureCount | int | No | Failed records |
| CompletedAt | DateTime | No | Completion timestamp |
| ErrorLog | string | No | Detailed error information |
| ValidationErrors | string | No | JSON array of validation errors |

#### 2.3.2 ImportType Enumeration
```csharp
public enum ImportType
{
    CustomerData = 0,
    EventData = 1,
    ConfigurationData = 2
}
```

#### 2.3.3 ImportStatus Enumeration
```csharp
public enum ImportStatus
{
    Uploaded = 0,
    Validating = 1,
    Validated = 2,
    Importing = 3,
    Completed = 4,
    Failed = 5,
    PartiallyCompleted = 6
}
```

### 2.4 Aggregate: SystemBackup
Manages system backup operations.

#### 2.4.1 Entity Properties
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| BackupId | Guid | Yes | Unique identifier |
| BackupType | BackupType | Yes | Type of backup |
| InitiatedBy | Guid | Yes | User or system that initiated |
| StartedAt | DateTime | Yes | Backup start time |
| CompletedAt | DateTime | No | Backup completion time |
| Status | BackupStatus | Yes | Current backup status |
| BackupUrl | string | No | Azure Blob Storage URL |
| BackupSize | long | No | Size in bytes |
| DatabaseSnapshot | string | No | Database snapshot name |
| RetentionUntil | DateTime | Yes | When backup will be deleted |
| ErrorMessage | string | No | Error details if failed |
| IsAutomated | bool | Yes | Whether scheduled or manual |

#### 2.4.2 BackupType Enumeration
```csharp
public enum BackupType
{
    Full = 0,
    Differential = 1,
    TransactionLog = 2
}
```

#### 2.4.3 BackupStatus Enumeration
```csharp
public enum BackupStatus
{
    Initiated = 0,
    InProgress = 1,
    Completed = 2,
    Failed = 3,
    Verified = 4
}
```

### 2.5 Aggregate: DataRestore
Manages data restoration operations.

#### 2.5.1 Entity Properties
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| RestoreId | Guid | Yes | Unique identifier |
| BackupId | Guid | Yes | Source backup reference |
| InitiatedBy | Guid | Yes | User who initiated restore |
| RequestedAt | DateTime | Yes | Request timestamp |
| RestoreType | RestoreType | Yes | Type of restore operation |
| TargetPointInTime | DateTime | No | Point-in-time for restore |
| Status | RestoreStatus | Yes | Current restore status |
| StartedAt | DateTime | No | Restore start time |
| CompletedAt | DateTime | No | Restore completion time |
| ErrorMessage | string | No | Error details if failed |
| ApprovalRequired | bool | Yes | Whether approval is needed |
| ApprovedBy | Guid | No | User who approved |
| ApprovedAt | DateTime | No | Approval timestamp |

#### 2.5.2 RestoreType Enumeration
```csharp
public enum RestoreType
{
    FullRestore = 0,
    PartialRestore = 1,
    PointInTimeRestore = 2
}
```

#### 2.5.3 RestoreStatus Enumeration
```csharp
public enum RestoreStatus
{
    PendingApproval = 0,
    Approved = 1,
    InProgress = 2,
    Completed = 3,
    Failed = 4,
    Rejected = 5
}
```

### 2.6 Aggregate: CustomerDataAccessLog
Tracks all customer data access for GDPR compliance.

#### 2.6.1 Entity Properties
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| AccessLogId | Guid | Yes | Unique identifier |
| CustomerId | Guid | Yes | Customer whose data was accessed |
| AccessedBy | Guid | Yes | User who accessed the data |
| AccessedAt | DateTime | Yes | Access timestamp |
| AccessType | DataAccessType | Yes | Type of access |
| DataCategory | string | Yes | Category of data accessed |
| Purpose | string | Yes | Business purpose for access |
| IpAddress | string | No | Source IP address |
| LegalBasis | string | No | Legal basis for processing |
| RetentionPeriod | int | Yes | Days to retain this log |

#### 2.6.2 DataAccessType Enumeration
```csharp
public enum DataAccessType
{
    View = 0,
    Export = 1,
    Modify = 2,
    Delete = 3
}
```

### 2.7 Aggregate: ConsentRecord
Manages customer consents and privacy preferences.

#### 2.7.1 Entity Properties
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| ConsentId | Guid | Yes | Unique identifier |
| CustomerId | Guid | Yes | Customer reference |
| ConsentType | ConsentType | Yes | Type of consent |
| Status | ConsentStatus | Yes | Current consent status |
| Version | string | Yes | Version of T&C/Privacy Policy |
| GrantedAt | DateTime | Yes | When consent was given |
| RevokedAt | DateTime | No | When consent was revoked |
| ExpiresAt | DateTime | No | Expiration date if applicable |
| IpAddress | string | No | IP address when granted |
| UserAgent | string | No | User agent when granted |
| ConsentText | string | Yes | Text that was consented to |
| Metadata | string | No | Additional context as JSON |

#### 2.7.2 ConsentType Enumeration
```csharp
public enum ConsentType
{
    PrivacyPolicy = 0,
    TermsOfService = 1,
    MarketingCommunications = 2,
    DataProcessing = 3,
    ThirdPartySharing = 4,
    Analytics = 5
}
```

#### 2.7.3 ConsentStatus Enumeration
```csharp
public enum ConsentStatus
{
    Active = 0,
    Revoked = 1,
    Expired = 2,
    Superseded = 3
}
```

### 2.8 Entity: CustomerDataDeletion
Tracks customer data deletion requests (GDPR Right to be Forgotten).

#### 2.8.1 Entity Properties
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| DeletionRequestId | Guid | Yes | Unique identifier |
| CustomerId | Guid | Yes | Customer requesting deletion |
| RequestedAt | DateTime | Yes | Request timestamp |
| RequestedBy | Guid | Yes | User who made request |
| Status | DeletionStatus | Yes | Current deletion status |
| ScheduledFor | DateTime | Yes | Planned deletion date |
| CompletedAt | DateTime | No | Actual deletion timestamp |
| DeletedBy | Guid | No | User who executed deletion |
| DeletionScope | string | Yes | JSON array of data categories |
| RetentionOverrides | string | No | Legal retention requirements |
| ApprovalRequired | bool | Yes | Whether approval is needed |
| ApprovedBy | Guid | No | User who approved |
| ApprovedAt | DateTime | No | Approval timestamp |
| VerificationHash | string | No | Hash for deletion verification |

#### 2.8.2 DeletionStatus Enumeration
```csharp
public enum DeletionStatus
{
    Requested = 0,
    PendingApproval = 1,
    Approved = 2,
    Scheduled = 3,
    InProgress = 4,
    Completed = 5,
    Failed = 6,
    Rejected = 7
}
```

---

## 3. Domain Events

### 3.1 Entity Audit Events
| Event | Trigger | Payload |
|-------|---------|---------|
| EntityCreated | Any entity created | EntityType, EntityId, CreatedBy, Timestamp, Data |
| EntityUpdated | Any entity modified | EntityType, EntityId, ModifiedBy, Timestamp, Changes |
| EntityDeleted | Any entity deleted | EntityType, EntityId, DeletedBy, Timestamp, IsHardDelete |
| BulkOperationPerformed | Bulk operation executed | OperationType, EntityType, AffectedCount, PerformedBy |

### 3.2 Data Management Events
| Event | Trigger | Payload |
|-------|---------|---------|
| DataExported | Export completed | DataExportId, ExportType, RecordCount, ExportedBy |
| DataImported | Import completed | DataImportId, ImportType, SuccessCount, FailureCount |
| SystemBackupCompleted | Backup succeeded | BackupId, BackupType, BackupSize, Duration |
| SystemBackupFailed | Backup failed | BackupId, BackupType, ErrorMessage, FailedAt |
| DataRestorePerformed | Restore completed | RestoreId, BackupId, RestoredBy, RestoreType |

### 3.3 GDPR/Privacy Events
| Event | Trigger | Payload |
|-------|---------|---------|
| CustomerDataAccessLogged | Customer data accessed | CustomerId, AccessedBy, AccessType, DataCategory |
| CustomerDataExported | Customer data exported | CustomerId, ExportId, ExportedBy, DataScope |
| CustomerDataDeleted | Customer data deleted | CustomerId, DeletionRequestId, DeletedBy, Scope |
| PrivacyPolicyAccepted | Privacy policy accepted | CustomerId, Version, AcceptedAt, IpAddress |
| TermsOfServiceAccepted | Terms accepted | CustomerId, Version, AcceptedAt, IpAddress |
| ConsentRecorded | New consent recorded | ConsentId, CustomerId, ConsentType, GrantedAt |
| ConsentRevoked | Consent revoked | ConsentId, CustomerId, ConsentType, RevokedAt |

---

## 4. API Endpoints

### 4.1 Audit Log Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/audit-logs | List audit logs with filtering |
| GET | /api/audit-logs/{auditLogId} | Get audit log by ID |
| GET | /api/audit-logs/entity/{entityType}/{entityId} | Get logs for specific entity |
| GET | /api/audit-logs/user/{userId} | Get user's action history |
| POST | /api/audit-logs/search | Advanced search with filters |

### 4.2 Data Export Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/exports | List export requests |
| GET | /api/exports/{exportId} | Get export details |
| POST | /api/exports | Create new export request |
| GET | /api/exports/{exportId}/download | Download export file |
| DELETE | /api/exports/{exportId} | Cancel/delete export |

### 4.3 Data Import Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/imports | List import operations |
| GET | /api/imports/{importId} | Get import details |
| POST | /api/imports | Upload file for import |
| POST | /api/imports/{importId}/validate | Validate import data |
| POST | /api/imports/{importId}/execute | Execute import |
| GET | /api/imports/{importId}/errors | Get import error details |

### 4.4 Backup & Restore Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/backups | List system backups |
| GET | /api/backups/{backupId} | Get backup details |
| POST | /api/backups | Initiate manual backup |
| POST | /api/backups/{backupId}/verify | Verify backup integrity |
| POST | /api/restores | Request data restore |
| POST | /api/restores/{restoreId}/approve | Approve restore request |
| GET | /api/restores/{restoreId} | Get restore status |

### 4.5 Customer Data Access Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/customer-data-access/{customerId} | Get customer's access log |
| POST | /api/customer-data-access/log | Manually log data access |
| GET | /api/customer-data-access/report | Generate access report |

### 4.6 Consent Management Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/consents/{customerId} | Get customer's consents |
| POST | /api/consents | Record new consent |
| POST | /api/consents/{consentId}/revoke | Revoke consent |
| GET | /api/consents/required | Get required consents list |
| GET | /api/privacy-policy/current | Get current privacy policy |
| GET | /api/terms-of-service/current | Get current terms of service |

### 4.7 Data Deletion Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/data-deletions | List deletion requests |
| GET | /api/data-deletions/{deletionRequestId} | Get deletion request details |
| POST | /api/data-deletions | Request data deletion |
| POST | /api/data-deletions/{deletionRequestId}/approve | Approve deletion |
| POST | /api/data-deletions/{deletionRequestId}/execute | Execute deletion |
| GET | /api/data-deletions/{customerId}/verify | Verify deletion completion |

---

## 5. Commands and Queries (CQRS)

### 5.1 Commands
```
EventManagementPlatform.Api/Features/AuditCompliance/
├── AuditLogs/
│   ├── CreateAuditLog/
│   │   ├── CreateAuditLogCommand.cs
│   │   ├── CreateAuditLogCommandHandler.cs
│   │   └── AuditLogDto.cs
├── Exports/
│   ├── CreateExport/
│   │   ├── CreateExportCommand.cs
│   │   ├── CreateExportCommandHandler.cs
│   │   └── CreateExportDto.cs
│   ├── ProcessExport/
│   │   ├── ProcessExportCommand.cs
│   │   └── ProcessExportCommandHandler.cs
├── Imports/
│   ├── CreateImport/
│   │   ├── CreateImportCommand.cs
│   │   └── CreateImportCommandHandler.cs
│   ├── ValidateImport/
│   │   ├── ValidateImportCommand.cs
│   │   └── ValidateImportCommandHandler.cs
│   ├── ExecuteImport/
│   │   ├── ExecuteImportCommand.cs
│   │   └── ExecuteImportCommandHandler.cs
├── Backups/
│   ├── CreateBackup/
│   │   ├── CreateBackupCommand.cs
│   │   └── CreateBackupCommandHandler.cs
│   ├── VerifyBackup/
│   │   ├── VerifyBackupCommand.cs
│   │   └── VerifyBackupCommandHandler.cs
├── Restores/
│   ├── RequestRestore/
│   │   ├── RequestRestoreCommand.cs
│   │   └── RequestRestoreCommandHandler.cs
│   ├── ApproveRestore/
│   │   ├── ApproveRestoreCommand.cs
│   │   └── ApproveRestoreCommandHandler.cs
│   ├── ExecuteRestore/
│   │   ├── ExecuteRestoreCommand.cs
│   │   └── ExecuteRestoreCommandHandler.cs
├── Consents/
│   ├── RecordConsent/
│   │   ├── RecordConsentCommand.cs
│   │   ├── RecordConsentCommandHandler.cs
│   │   └── ConsentDto.cs
│   ├── RevokeConsent/
│   │   ├── RevokeConsentCommand.cs
│   │   └── RevokeConsentCommandHandler.cs
└── DataDeletion/
    ├── RequestDataDeletion/
    │   ├── RequestDataDeletionCommand.cs
    │   └── RequestDataDeletionCommandHandler.cs
    ├── ApproveDataDeletion/
    │   ├── ApproveDataDeletionCommand.cs
    │   └── ApproveDataDeletionCommandHandler.cs
    └── ExecuteDataDeletion/
        ├── ExecuteDataDeletionCommand.cs
        └── ExecuteDataDeletionCommandHandler.cs
```

### 5.2 Queries
```
EventManagementPlatform.Api/Features/AuditCompliance/
├── AuditLogs/
│   ├── GetAuditLogs/
│   │   ├── GetAuditLogsQuery.cs
│   │   ├── GetAuditLogsQueryHandler.cs
│   │   └── AuditLogListDto.cs
│   ├── GetEntityAuditTrail/
│   │   ├── GetEntityAuditTrailQuery.cs
│   │   └── GetEntityAuditTrailQueryHandler.cs
│   ├── SearchAuditLogs/
│   │   ├── SearchAuditLogsQuery.cs
│   │   └── SearchAuditLogsQueryHandler.cs
├── Exports/
│   ├── GetExports/
│   │   ├── GetExportsQuery.cs
│   │   └── GetExportsQueryHandler.cs
│   ├── GetExportById/
│   │   ├── GetExportByIdQuery.cs
│   │   └── GetExportByIdQueryHandler.cs
├── Imports/
│   ├── GetImports/
│   │   ├── GetImportsQuery.cs
│   │   └── GetImportsQueryHandler.cs
│   ├── GetImportErrors/
│   │   ├── GetImportErrorsQuery.cs
│   │   └── GetImportErrorsQueryHandler.cs
├── Backups/
│   ├── GetBackups/
│   │   ├── GetBackupsQuery.cs
│   │   └── GetBackupsQueryHandler.cs
│   ├── GetBackupById/
│   │   ├── GetBackupByIdQuery.cs
│   │   └── GetBackupByIdQueryHandler.cs
├── Consents/
│   ├── GetCustomerConsents/
│   │   ├── GetCustomerConsentsQuery.cs
│   │   └── GetCustomerConsentsQueryHandler.cs
│   ├── GetRequiredConsents/
│   │   ├── GetRequiredConsentsQuery.cs
│   │   └── GetRequiredConsentsQueryHandler.cs
└── DataDeletion/
    ├── GetDeletionRequests/
    │   ├── GetDeletionRequestsQuery.cs
    │   └── GetDeletionRequestsQueryHandler.cs
    └── VerifyDeletion/
        ├── VerifyDeletionQuery.cs
        └── VerifyDeletionQueryHandler.cs
```

---

## 6. Business Rules

### 6.1 Audit Logging Rules
| Rule ID | Description |
|---------|-------------|
| AUD-001 | All entity create, update, delete operations must generate audit logs |
| AUD-002 | Audit logs are immutable once created |
| AUD-003 | Audit logs must be retained for minimum 7 years |
| AUD-004 | Sensitive data in changes must be masked |
| AUD-005 | Bulk operations affecting >100 records must log summary only |

### 6.2 Data Export Rules
| Rule ID | Description |
|---------|-------------|
| AUD-010 | Users can only export data they have permission to access |
| AUD-011 | Customer data exports require customer consent |
| AUD-012 | Export files expire after 7 days |
| AUD-013 | Large exports (>10MB) must be processed asynchronously |
| AUD-014 | All exports must be encrypted at rest |

### 6.3 Data Import Rules
| Rule ID | Description |
|---------|-------------|
| AUD-020 | All imports must be validated before execution |
| AUD-021 | Failed import records must be logged for review |
| AUD-022 | Imports must support rollback on critical failures |
| AUD-023 | Import files must be scanned for malware |
| AUD-024 | Duplicate records must be detected and flagged |

### 6.4 Backup & Restore Rules
| Rule ID | Description |
|---------|-------------|
| AUD-030 | Full backups must run daily at minimum |
| AUD-031 | Backups must be verified within 24 hours |
| AUD-032 | Backups retained: Daily (7 days), Weekly (4 weeks), Monthly (12 months) |
| AUD-033 | Restore operations require multi-person approval |
| AUD-034 | Point-in-time restore limited to last 30 days |

### 6.5 GDPR Compliance Rules
| Rule ID | Description |
|---------|-------------|
| AUD-040 | All customer data access must be logged |
| AUD-041 | Customer data export requests must complete within 30 days |
| AUD-042 | Data deletion requests must complete within 30 days |
| AUD-043 | Deleted data must be unrecoverable (cryptographic erasure) |
| AUD-044 | Customers must consent to data processing |
| AUD-045 | Consent can be withdrawn at any time |

### 6.6 Consent Management Rules
| Rule ID | Description |
|---------|-------------|
| AUD-050 | Privacy policy acceptance required before data collection |
| AUD-051 | Terms of service acceptance required before account activation |
| AUD-052 | Consent records must track version of policy accepted |
| AUD-053 | Updated policies require re-consent |
| AUD-054 | Consent must be freely given, specific, informed, and unambiguous |

---

## 7. Azure Integration

### 7.1 Azure AI Services
| Service | Purpose |
|---------|---------|
| Azure Cognitive Services | Anomaly detection in audit logs |
| Azure OpenAI | Smart categorization of audit events |
| Azure Document Intelligence | OCR for import document processing |

### 7.2 Azure Infrastructure
| Service | Purpose |
|---------|---------|
| Azure App Service | Host API application |
| Azure SQL Database | Primary data storage with point-in-time restore |
| Azure Blob Storage | Store exports, backups, import files |
| Azure Key Vault | Manage encryption keys for sensitive data |
| Azure Service Bus | Queue export/import/backup jobs |
| Azure Application Insights | Monitoring and compliance reporting |
| Azure Monitor | Alert on backup failures or anomalies |

### 7.3 Azure Security
| Feature | Implementation |
|---------|----------------|
| Encryption at Rest | Azure Storage Service Encryption (SSE) |
| Encryption in Transit | TLS 1.3 |
| Key Management | Azure Key Vault with key rotation |
| Access Control | Azure RBAC with least privilege |
| Data Residency | Configurable Azure region selection |

---

## 8. Data Transfer Objects (DTOs)

### 8.1 AuditLogDto
```csharp
public record AuditLogDto(
    Guid AuditLogId,
    string EntityType,
    Guid EntityId,
    string OperationType,
    Guid UserId,
    string UserName,
    DateTime Timestamp,
    string? Changes,
    string? IpAddress,
    Guid CorrelationId
);
```

### 8.2 CreateExportDto
```csharp
public record CreateExportDto(
    ExportType ExportType,
    string? EntityType,
    string? FilterCriteria,
    DateTime? StartDate,
    DateTime? EndDate
);
```

### 8.3 ExportDetailDto
```csharp
public record ExportDetailDto(
    Guid DataExportId,
    string ExportType,
    string Status,
    DateTime RequestedAt,
    DateTime? CompletedAt,
    DateTime? ExpiresAt,
    string? FileUrl,
    long? FileSize,
    int? RecordCount,
    string RequestedByName
);
```

### 8.4 ConsentDto
```csharp
public record ConsentDto(
    Guid ConsentId,
    Guid CustomerId,
    string ConsentType,
    string Status,
    string Version,
    DateTime GrantedAt,
    DateTime? RevokedAt,
    DateTime? ExpiresAt
);
```

### 8.5 DataDeletionRequestDto
```csharp
public record DataDeletionRequestDto(
    Guid DeletionRequestId,
    Guid CustomerId,
    string Status,
    DateTime RequestedAt,
    DateTime? ScheduledFor,
    DateTime? CompletedAt,
    string[] DeletionScope,
    bool ApprovalRequired
);
```

---

## 9. Validation Requirements

### 9.1 Export Validation
```csharp
public class CreateExportCommandValidator : AbstractValidator<CreateExportCommand>
{
    public CreateExportCommandValidator()
    {
        RuleFor(x => x.ExportType)
            .IsInEnum()
            .WithMessage("Invalid export type");

        When(x => x.StartDate.HasValue && x.EndDate.HasValue, () =>
        {
            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after start date");
        });

        When(x => x.ExportType == ExportType.CustomerData, () =>
        {
            RuleFor(x => x.FilterCriteria)
                .NotEmpty()
                .WithMessage("Customer ID required for customer data export");
        });
    }
}
```

### 9.2 Consent Validation
```csharp
public class RecordConsentCommandValidator : AbstractValidator<RecordConsentCommand>
{
    public RecordConsentCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required");

        RuleFor(x => x.ConsentType)
            .IsInEnum()
            .WithMessage("Invalid consent type");

        RuleFor(x => x.Version)
            .NotEmpty()
            .WithMessage("Policy version is required");

        RuleFor(x => x.ConsentText)
            .NotEmpty()
            .MinimumLength(10)
            .WithMessage("Consent text must be provided");
    }
}
```

---

## 10. Security Requirements

### 10.1 Authentication
- All endpoints require JWT Bearer token authentication
- Service accounts for automated backups with certificate authentication
- Multi-factor authentication required for restore operations

### 10.2 Authorization
| Role | Permissions |
|------|-------------|
| User | View own audit logs, Request data export, Manage own consents |
| Staff | View audit logs, Create exports |
| Auditor | View all audit logs, Create compliance reports |
| DataProtectionOfficer | All data access logs, Manage deletion requests |
| SystemAdministrator | All operations, Initiate backups, Approve restores |

### 10.3 Data Protection
| Requirement | Implementation |
|-------------|----------------|
| PII Encryption | AES-256 encryption for sensitive fields |
| Access Logging | All customer data access must be logged |
| Data Minimization | Only collect necessary data |
| Pseudonymization | Use where full identification not needed |
| Right to Access | API for customers to request their data |
| Right to Erasure | Automated deletion workflow |

---

## 11. Performance Requirements

| Metric | Requirement |
|--------|-------------|
| Audit Log Write | < 50ms for 95th percentile |
| Audit Log Query | < 300ms for standard queries |
| Export Generation | < 5 minutes for 100K records |
| Import Processing | 1000 records/second minimum |
| Backup Completion | Full backup within 4 hours |
| Restore Time Objective (RTO) | < 4 hours |
| Recovery Point Objective (RPO) | < 24 hours |

---

## 12. Error Handling

### 12.1 Error Response Format
```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "Bad Request",
    "status": 400,
    "detail": "Export type is required",
    "instance": "/api/exports",
    "errors": {
        "ExportType": ["Export type must be specified"]
    }
}
```

### 12.2 Exception Types
| Exception | HTTP Status | Description |
|-----------|-------------|-------------|
| ValidationException | 400 | Input validation failed |
| NotFoundException | 404 | Resource not found |
| ConflictException | 409 | Operation conflict (e.g., consent already exists) |
| UnauthorizedException | 401 | Authentication required |
| ForbiddenException | 403 | Insufficient permissions |
| DataRetentionException | 409 | Cannot delete due to retention policy |
| BackupFailedException | 500 | Backup operation failed |

---

## 13. Testing Requirements

### 13.1 Unit Tests
- Test all audit log generation
- Test export/import validation logic
- Test consent workflow state transitions
- Test data deletion masking algorithms

### 13.2 Integration Tests
- Test end-to-end export workflow
- Test backup and restore operations
- Test Azure Blob Storage integration
- Test GDPR workflow compliance

### 13.3 Compliance Tests
- Verify GDPR right to access
- Verify GDPR right to erasure
- Verify audit trail completeness
- Verify data retention policies

### 13.4 Test Coverage
- Minimum 85% code coverage
- 100% coverage for GDPR workflows
- 100% coverage for audit logging

---

## 14. Background Jobs

### 14.1 Scheduled Jobs
| Job | Schedule | Purpose |
|-----|----------|---------|
| DailyBackupJob | 2:00 AM daily | Create full system backup |
| ExportCleanupJob | Hourly | Delete expired export files |
| ConsentExpiryJob | Daily | Mark expired consents |
| AuditLogArchivalJob | Monthly | Archive old audit logs |
| DataDeletionExecutionJob | Daily | Process approved deletion requests |

### 14.2 Event-Driven Jobs
| Job | Trigger | Purpose |
|-----|---------|---------|
| ExportProcessingJob | Export created | Generate export file |
| ImportProcessingJob | Import uploaded | Process import file |
| BackupVerificationJob | Backup completed | Verify backup integrity |
| AnomalyDetectionJob | Audit log created | Detect suspicious patterns |

---

## 15. Compliance Requirements

### 15.1 GDPR Requirements
| Requirement | Implementation |
|-------------|----------------|
| Lawfulness, fairness, transparency | Explicit consent tracking |
| Purpose limitation | Purpose recorded with each access |
| Data minimization | Only necessary fields collected |
| Accuracy | Update/correction workflows |
| Storage limitation | Automated retention policies |
| Integrity and confidentiality | Encryption, access controls |
| Accountability | Comprehensive audit trails |

### 15.2 Data Retention Policies
| Data Type | Retention Period | Legal Basis |
|-----------|------------------|-------------|
| Audit Logs | 7 years | Regulatory compliance |
| Customer Data | Account lifetime + 30 days | Contract performance |
| Financial Records | 7 years | Tax regulations |
| Consent Records | 3 years after revocation | Legal obligation |
| Backup Files | 12 months | Business continuity |

---

## 16. Appendices

### 16.1 Related Documents
- [Frontend Specification](./frontend-spec.md)
- [Use Case Diagram](./diagrams/use-case.drawio)
- [C4 Diagrams](./diagrams/c4-context.drawio)
- [Class Diagram](./plantuml/class-diagram.plantuml)
- [Sequence Diagrams](./plantuml/sequence-diagram.plantuml)

### 16.2 Revision History
| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-22 | System | Initial specification |
