# Audit & Compliance - Frontend Specification

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
This specification defines the frontend requirements for the Audit & Compliance feature, providing administrators, data protection officers, and users with comprehensive interfaces for audit trail management, data lifecycle operations, and GDPR compliance.

### 1.2 Technology Stack
- **Framework**: Angular 18+ (latest stable)
- **UI Library**: Angular Material (Material 3)
- **State Management**: RxJS (no NgRx)
- **Styling**: SCSS with BEM methodology
- **Testing**: Jest (unit), Playwright (e2e)
- **Charts**: Chart.js with ng2-charts
- **File Handling**: ngx-file-drop for imports

### 1.3 Design Principles
- Mobile-first responsive design
- Material 3 design guidelines
- Accessibility (WCAG 2.1 AA compliance)
- Default Angular Material theme colors only
- Data visualization for audit insights
- Progressive disclosure for complex operations

---

## 2. Page Structure

### 2.1 Pages
```
src/EventManagementPlatform.WebApp/projects/EventManagementPlatform/src/app/
├── pages/
│   ├── audit-compliance/
│   │   ├── audit-logs/
│   │   │   ├── audit-log-list/
│   │   │   │   ├── audit-log-list.ts
│   │   │   │   ├── audit-log-list.html
│   │   │   │   ├── audit-log-list.scss
│   │   │   │   └── index.ts
│   │   │   ├── audit-log-detail/
│   │   │   │   ├── audit-log-detail.ts
│   │   │   │   ├── audit-log-detail.html
│   │   │   │   ├── audit-log-detail.scss
│   │   │   │   └── index.ts
│   │   │   ├── entity-audit-trail/
│   │   │   │   ├── entity-audit-trail.ts
│   │   │   │   ├── entity-audit-trail.html
│   │   │   │   ├── entity-audit-trail.scss
│   │   │   │   └── index.ts
│   │   │   └── index.ts
│   │   ├── data-exports/
│   │   │   ├── export-list/
│   │   │   │   ├── export-list.ts
│   │   │   │   ├── export-list.html
│   │   │   │   ├── export-list.scss
│   │   │   │   └── index.ts
│   │   │   ├── export-create/
│   │   │   │   ├── export-create.ts
│   │   │   │   ├── export-create.html
│   │   │   │   ├── export-create.scss
│   │   │   │   └── index.ts
│   │   │   └── index.ts
│   │   ├── data-imports/
│   │   │   ├── import-list/
│   │   │   │   ├── import-list.ts
│   │   │   │   ├── import-list.html
│   │   │   │   ├── import-list.scss
│   │   │   │   └── index.ts
│   │   │   ├── import-upload/
│   │   │   │   ├── import-upload.ts
│   │   │   │   ├── import-upload.html
│   │   │   │   ├── import-upload.scss
│   │   │   │   └── index.ts
│   │   │   ├── import-detail/
│   │   │   │   ├── import-detail.ts
│   │   │   │   ├── import-detail.html
│   │   │   │   ├── import-detail.scss
│   │   │   │   └── index.ts
│   │   │   └── index.ts
│   │   ├── backups/
│   │   │   ├── backup-list/
│   │   │   │   ├── backup-list.ts
│   │   │   │   ├── backup-list.html
│   │   │   │   ├── backup-list.scss
│   │   │   │   └── index.ts
│   │   │   ├── backup-restore/
│   │   │   │   ├── backup-restore.ts
│   │   │   │   ├── backup-restore.html
│   │   │   │   ├── backup-restore.scss
│   │   │   │   └── index.ts
│   │   │   └── index.ts
│   │   ├── consents/
│   │   │   ├── consent-dashboard/
│   │   │   │   ├── consent-dashboard.ts
│   │   │   │   ├── consent-dashboard.html
│   │   │   │   ├── consent-dashboard.scss
│   │   │   │   └── index.ts
│   │   │   ├── consent-management/
│   │   │   │   ├── consent-management.ts
│   │   │   │   ├── consent-management.html
│   │   │   │   ├── consent-management.scss
│   │   │   │   └── index.ts
│   │   │   ├── privacy-center/
│   │   │   │   ├── privacy-center.ts
│   │   │   │   ├── privacy-center.html
│   │   │   │   ├── privacy-center.scss
│   │   │   │   └── index.ts
│   │   │   └── index.ts
│   │   ├── data-deletion/
│   │   │   ├── deletion-requests/
│   │   │   │   ├── deletion-requests.ts
│   │   │   │   ├── deletion-requests.html
│   │   │   │   ├── deletion-requests.scss
│   │   │   │   └── index.ts
│   │   │   ├── request-deletion/
│   │   │   │   ├── request-deletion.ts
│   │   │   │   ├── request-deletion.html
│   │   │   │   ├── request-deletion.scss
│   │   │   │   └── index.ts
│   │   │   └── index.ts
│   │   ├── compliance-dashboard/
│   │   │   ├── compliance-dashboard.ts
│   │   │   ├── compliance-dashboard.html
│   │   │   ├── compliance-dashboard.scss
│   │   │   └── index.ts
│   │   └── index.ts
```

### 2.2 Components
```
├── components/
│   ├── audit-compliance/
│   │   ├── audit-log-table/
│   │   │   ├── audit-log-table.ts
│   │   │   ├── audit-log-table.html
│   │   │   ├── audit-log-table.scss
│   │   │   └── index.ts
│   │   ├── audit-log-filter/
│   │   │   ├── audit-log-filter.ts
│   │   │   ├── audit-log-filter.html
│   │   │   ├── audit-log-filter.scss
│   │   │   └── index.ts
│   │   ├── audit-timeline/
│   │   │   ├── audit-timeline.ts
│   │   │   ├── audit-timeline.html
│   │   │   ├── audit-timeline.scss
│   │   │   └── index.ts
│   │   ├── export-status-card/
│   │   │   ├── export-status-card.ts
│   │   │   ├── export-status-card.html
│   │   │   ├── export-status-card.scss
│   │   │   └── index.ts
│   │   ├── export-form/
│   │   │   ├── export-form.ts
│   │   │   ├── export-form.html
│   │   │   ├── export-form.scss
│   │   │   └── index.ts
│   │   ├── import-uploader/
│   │   │   ├── import-uploader.ts
│   │   │   ├── import-uploader.html
│   │   │   ├── import-uploader.scss
│   │   │   └── index.ts
│   │   ├── import-validation-results/
│   │   │   ├── import-validation-results.ts
│   │   │   ├── import-validation-results.html
│   │   │   ├── import-validation-results.scss
│   │   │   └── index.ts
│   │   ├── backup-status-badge/
│   │   │   ├── backup-status-badge.ts
│   │   │   ├── backup-status-badge.html
│   │   │   ├── backup-status-badge.scss
│   │   │   └── index.ts
│   │   ├── restore-wizard/
│   │   │   ├── restore-wizard.ts
│   │   │   ├── restore-wizard.html
│   │   │   ├── restore-wizard.scss
│   │   │   └── index.ts
│   │   ├── consent-card/
│   │   │   ├── consent-card.ts
│   │   │   ├── consent-card.html
│   │   │   ├── consent-card.scss
│   │   │   └── index.ts
│   │   ├── consent-dialog/
│   │   │   ├── consent-dialog.ts
│   │   │   ├── consent-dialog.html
│   │   │   ├── consent-dialog.scss
│   │   │   └── index.ts
│   │   ├── data-deletion-stepper/
│   │   │   ├── data-deletion-stepper.ts
│   │   │   ├── data-deletion-stepper.html
│   │   │   ├── data-deletion-stepper.scss
│   │   │   └── index.ts
│   │   ├── compliance-metrics-card/
│   │   │   ├── compliance-metrics-card.ts
│   │   │   ├── compliance-metrics-card.html
│   │   │   ├── compliance-metrics-card.scss
│   │   │   └── index.ts
│   │   ├── activity-chart/
│   │   │   ├── activity-chart.ts
│   │   │   ├── activity-chart.html
│   │   │   ├── activity-chart.scss
│   │   │   └── index.ts
│   │   └── index.ts
```

---

## 3. User Interface Requirements

### 3.1 Audit Log List Page

#### 3.1.1 Layout
- **Header**: Page title "Audit Logs", date range picker, export button
- **Filter Panel**: Entity type, operation type, user, date range filters
- **Content**: Data table with sorting and pagination
- **Details Panel**: Expandable row details showing full change data

#### 3.1.2 Features
| Feature | Description |
|---------|-------------|
| Search | Full-text search across entity types and changes |
| Filter | Multi-select filters for entity type, operation, user |
| Date Range | Custom date range picker with presets (Today, Last 7 days, etc.) |
| Export | Export filtered results to CSV/Excel |
| Timeline View | Alternative visualization showing chronological timeline |
| Real-time Updates | Auto-refresh for new audit entries |

#### 3.1.3 Table Columns
| Column | Sortable | Width | Description |
|--------|----------|-------|-------------|
| Timestamp | Yes | 180px | Date and time of operation |
| Entity Type | Yes | 150px | Type of entity affected |
| Entity ID | No | 120px | ID with copy button |
| Operation | Yes | 120px | Created/Updated/Deleted badge |
| User | Yes | 200px | User who performed action |
| Changes | No | Flex | Summary of changes |
| Actions | No | 80px | View details button |

#### 3.1.4 Responsive Behavior
| Breakpoint | Layout |
|------------|--------|
| < 600px | Card view with stacked fields |
| 600-960px | Table with essential columns only |
| > 960px | Full table with all columns |

### 3.2 Audit Log Detail Page

#### 3.2.1 Sections
| Section | Content |
|---------|---------|
| Header | Entity type, operation badge, timestamp |
| User Info | User details, IP address, user agent |
| Changes | Before/after comparison with diff highlighting |
| Context | Correlation ID, related operations |
| JSON View | Raw JSON data viewer |

#### 3.2.2 Features
- JSON diff viewer with syntax highlighting
- Copy to clipboard for changes
- Link to entity details if accessible
- Timeline of related operations

### 3.3 Entity Audit Trail Page

#### 3.3.1 Layout
- **Header**: Entity type and ID, back button
- **Timeline**: Vertical timeline of all operations
- **Filters**: Operation type filter
- **Summary**: Total operations count

#### 3.3.2 Timeline Features
- Color-coded by operation type (create=green, update=blue, delete=red)
- Expandable timeline items showing full details
- Chronological ordering (newest first)
- Visual diff for updates

### 3.4 Data Export Page

#### 3.4.1 Export Creation Form
| Field | Type | Validation |
|-------|------|------------|
| Export Type | mat-select | Required |
| Entity Type | mat-select | Required if applicable |
| Date Range | mat-date-range-picker | Optional |
| Format | mat-radio-group (CSV/Excel/JSON) | Required |
| Filter Criteria | mat-textarea (JSON) | Valid JSON if provided |

#### 3.4.2 Export List
| Column | Description |
|--------|-------------|
| Export Type | Badge with type |
| Status | Progress bar for in-progress, badge for completed/failed |
| Requested At | Timestamp |
| Records | Count of records |
| File Size | Human-readable size |
| Actions | Download/Delete buttons |

#### 3.4.3 Export Status States
```typescript
export enum ExportStatusDisplay {
  Pending = 'Processing request...',
  InProgress = 'Generating export... {progress}%',
  Completed = 'Ready for download',
  Failed = 'Export failed',
  Expired = 'Download expired'
}
```

### 3.5 Data Import Page

#### 3.5.1 Import Upload
- **Drag-and-drop zone**: Upload CSV/Excel files
- **File validation**: Size limit (50MB), format check
- **Preview**: Show first 10 rows after upload
- **Import type selection**: Dropdown for import type

#### 3.5.2 Validation Step
- **Progress indicator**: Show validation progress
- **Validation results table**: Errors and warnings
- **Error details**: Expandable rows with error messages
- **Actions**: Fix and re-upload or continue with warnings

#### 3.5.3 Import Execution
- **Progress bar**: Real-time import progress
- **Statistics**: Total/Success/Failed counts
- **Live log**: Scrolling log of import operations
- **Completion summary**: Final results with download error report

### 3.6 Backup List Page

#### 3.6.1 Layout
- **Header**: Page title, "Create Backup" button
- **Filter**: Backup type, status filters
- **Grid**: Card grid of backups

#### 3.6.2 Backup Card
| Element | Description |
|---------|-------------|
| Type Badge | Full/Differential/Log |
| Status Indicator | Color-coded status |
| Date | Backup date and time |
| Size | File size |
| Retention | Expiry date |
| Actions | Verify/Restore/Download buttons |

#### 3.6.3 Backup Creation Dialog
| Field | Type | Description |
|-------|------|-------------|
| Backup Type | mat-select | Full/Differential/Log |
| Include Blobs | mat-checkbox | Include blob storage |
| Retention Days | mat-input (number) | Days to retain |
| Description | mat-textarea | Optional notes |

### 3.7 Restore Wizard

#### 3.7.1 Wizard Steps
1. **Select Backup**: Choose backup to restore from
2. **Restore Options**: Full/Partial, Point-in-time
3. **Confirmation**: Review and confirm (requires password)
4. **Approval**: Request approval if required
5. **Progress**: Monitor restore progress

#### 3.7.2 Restore Confirmation
```html
<mat-dialog-content>
  <mat-icon color="warn">warning</mat-icon>
  <h2>Restore Confirmation</h2>
  <p>This operation will restore data from backup:</p>
  <ul>
    <li>Backup ID: {{backupId}}</li>
    <li>Created: {{backupDate}}</li>
    <li>Type: {{restoreType}}</li>
  </ul>
  <mat-form-field>
    <mat-label>Enter your password to confirm</mat-label>
    <input matInput type="password" required>
  </mat-form-field>
</mat-dialog-content>
```

### 3.8 Privacy Center (Customer-Facing)

#### 3.8.1 Sections
| Section | Content |
|---------|---------|
| My Consents | List of active consents with revoke option |
| Privacy Policy | Current privacy policy with accept button |
| Terms of Service | Current terms with accept button |
| Data Access | Request data export |
| Data Deletion | Request account/data deletion |
| Activity Log | Customer's own access log |

#### 3.8.2 Consent Card
```html
<mat-card>
  <mat-card-header>
    <mat-card-title>{{consentType}}</mat-card-title>
    <mat-chip [color]="status === 'Active' ? 'primary' : 'warn'">
      {{status}}
    </mat-chip>
  </mat-card-header>
  <mat-card-content>
    <p><strong>Version:</strong> {{version}}</p>
    <p><strong>Granted:</strong> {{grantedDate | date}}</p>
    <p><strong>Expires:</strong> {{expiresDate | date}}</p>
  </mat-card-content>
  <mat-card-actions>
    <button mat-button color="warn" (click)="revokeConsent()">
      Revoke Consent
    </button>
  </mat-card-actions>
</mat-card>
```

### 3.9 Consent Management Page (Admin)

#### 3.9.1 Layout
- **Header**: Page title, analytics overview
- **Filters**: Consent type, status, customer
- **Table**: List of all consent records
- **Actions**: Export compliance report

#### 3.9.2 Analytics Cards
| Metric | Description |
|--------|-------------|
| Total Consents | Count of active consents |
| Revocation Rate | Percentage of revoked consents |
| Pending Re-consent | Customers needing to re-consent |
| Compliance Score | Overall compliance percentage |

### 3.10 Data Deletion Requests Page

#### 3.10.1 Request List
| Column | Description |
|--------|-------------|
| Customer | Customer name and ID |
| Requested | Date of request |
| Status | Pending/Approved/Scheduled/Completed |
| Scheduled For | Planned deletion date |
| Scope | Data categories to delete |
| Actions | Approve/Reject/Execute buttons |

#### 3.10.2 Deletion Request Stepper (Customer)
1. **Verify Identity**: Email/SMS verification
2. **Select Data**: Choose data categories to delete
3. **Understand Impact**: Warning about data loss
4. **Confirm**: Final confirmation
5. **Confirmation**: Request submitted message

### 3.11 Compliance Dashboard

#### 3.11.1 Metrics Overview
```html
<div class="metrics-grid">
  <app-compliance-metrics-card
    title="Audit Logs Today"
    [value]="todayLogsCount"
    icon="history"
    color="primary">
  </app-compliance-metrics-card>

  <app-compliance-metrics-card
    title="Active Exports"
    [value]="activeExportsCount"
    icon="file_download"
    color="accent">
  </app-compliance-metrics-card>

  <app-compliance-metrics-card
    title="Pending Deletions"
    [value]="pendingDeletionsCount"
    icon="delete_forever"
    color="warn">
  </app-compliance-metrics-card>

  <app-compliance-metrics-card
    title="Last Backup"
    [value]="lastBackupDate"
    [status]="backupStatus"
    icon="backup">
  </app-compliance-metrics-card>
</div>
```

#### 3.11.2 Activity Chart
- **Chart Type**: Line chart showing audit activity over time
- **Time Range**: Last 7 days, 30 days, 90 days
- **Metrics**: Operations per day by type
- **Interactivity**: Click to drill down to specific day

#### 3.11.3 Recent Activity
- Table of last 20 audit log entries
- Auto-refresh every 30 seconds
- Click to view details

---

## 4. Services

### 4.1 AuditLogService
```typescript
@Injectable({ providedIn: 'root' })
export class AuditLogService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getAuditLogs(params: AuditLogQueryParams): Observable<PagedResult<AuditLogDto>> {
        const queryParams = new HttpParams({ fromObject: params as any });
        return this.http.get<PagedResult<AuditLogDto>>(
            `${this.baseUrl}/audit-logs`,
            { params: queryParams }
        );
    }

    getAuditLogById(auditLogId: string): Observable<AuditLogDto> {
        return this.http.get<AuditLogDto>(
            `${this.baseUrl}/audit-logs/${auditLogId}`
        );
    }

    getEntityAuditTrail(entityType: string, entityId: string): Observable<AuditLogDto[]> {
        return this.http.get<AuditLogDto[]>(
            `${this.baseUrl}/audit-logs/entity/${entityType}/${entityId}`
        );
    }

    searchAuditLogs(criteria: AuditLogSearchCriteria): Observable<PagedResult<AuditLogDto>> {
        return this.http.post<PagedResult<AuditLogDto>>(
            `${this.baseUrl}/audit-logs/search`,
            criteria
        );
    }
}
```

### 4.2 DataExportService
```typescript
@Injectable({ providedIn: 'root' })
export class DataExportService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getExports(): Observable<PagedResult<ExportDetailDto>> {
        return this.http.get<PagedResult<ExportDetailDto>>(
            `${this.baseUrl}/exports`
        );
    }

    getExportById(exportId: string): Observable<ExportDetailDto> {
        return this.http.get<ExportDetailDto>(
            `${this.baseUrl}/exports/${exportId}`
        );
    }

    createExport(request: CreateExportDto): Observable<ExportDetailDto> {
        return this.http.post<ExportDetailDto>(
            `${this.baseUrl}/exports`,
            request
        );
    }

    downloadExport(exportId: string): Observable<Blob> {
        return this.http.get(
            `${this.baseUrl}/exports/${exportId}/download`,
            { responseType: 'blob' }
        );
    }

    deleteExport(exportId: string): Observable<void> {
        return this.http.delete<void>(
            `${this.baseUrl}/exports/${exportId}`
        );
    }

    pollExportStatus(exportId: string): Observable<ExportDetailDto> {
        return interval(2000).pipe(
            switchMap(() => this.getExportById(exportId)),
            takeWhile(export =>
                export.status === 'Pending' || export.status === 'InProgress',
                true
            )
        );
    }
}
```

### 4.3 DataImportService
```typescript
@Injectable({ providedIn: 'root' })
export class DataImportService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    uploadImport(file: File, importType: string): Observable<ImportDetailDto> {
        const formData = new FormData();
        formData.append('file', file);
        formData.append('importType', importType);

        return this.http.post<ImportDetailDto>(
            `${this.baseUrl}/imports`,
            formData
        );
    }

    validateImport(importId: string): Observable<ValidationResultDto> {
        return this.http.post<ValidationResultDto>(
            `${this.baseUrl}/imports/${importId}/validate`,
            {}
        );
    }

    executeImport(importId: string): Observable<ImportDetailDto> {
        return this.http.post<ImportDetailDto>(
            `${this.baseUrl}/imports/${importId}/execute`,
            {}
        );
    }

    getImportErrors(importId: string): Observable<ImportErrorDto[]> {
        return this.http.get<ImportErrorDto[]>(
            `${this.baseUrl}/imports/${importId}/errors`
        );
    }
}
```

### 4.4 BackupService
```typescript
@Injectable({ providedIn: 'root' })
export class BackupService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getBackups(): Observable<BackupDto[]> {
        return this.http.get<BackupDto[]>(`${this.baseUrl}/backups`);
    }

    createBackup(request: CreateBackupDto): Observable<BackupDto> {
        return this.http.post<BackupDto>(
            `${this.baseUrl}/backups`,
            request
        );
    }

    verifyBackup(backupId: string): Observable<BackupVerificationDto> {
        return this.http.post<BackupVerificationDto>(
            `${this.baseUrl}/backups/${backupId}/verify`,
            {}
        );
    }

    requestRestore(request: RestoreRequestDto): Observable<RestoreDto> {
        return this.http.post<RestoreDto>(
            `${this.baseUrl}/restores`,
            request
        );
    }

    approveRestore(restoreId: string): Observable<void> {
        return this.http.post<void>(
            `${this.baseUrl}/restores/${restoreId}/approve`,
            {}
        );
    }
}
```

### 4.5 ConsentService
```typescript
@Injectable({ providedIn: 'root' })
export class ConsentService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getCustomerConsents(customerId: string): Observable<ConsentDto[]> {
        return this.http.get<ConsentDto[]>(
            `${this.baseUrl}/consents/${customerId}`
        );
    }

    recordConsent(consent: RecordConsentDto): Observable<ConsentDto> {
        return this.http.post<ConsentDto>(
            `${this.baseUrl}/consents`,
            consent
        );
    }

    revokeConsent(consentId: string): Observable<void> {
        return this.http.post<void>(
            `${this.baseUrl}/consents/${consentId}/revoke`,
            {}
        );
    }

    getRequiredConsents(): Observable<RequiredConsentDto[]> {
        return this.http.get<RequiredConsentDto[]>(
            `${this.baseUrl}/consents/required`
        );
    }

    getCurrentPrivacyPolicy(): Observable<PolicyDto> {
        return this.http.get<PolicyDto>(
            `${this.baseUrl}/privacy-policy/current`
        );
    }

    getCurrentTermsOfService(): Observable<PolicyDto> {
        return this.http.get<PolicyDto>(
            `${this.baseUrl}/terms-of-service/current`
        );
    }
}
```

### 4.6 DataDeletionService
```typescript
@Injectable({ providedIn: 'root' })
export class DataDeletionService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getDeletionRequests(): Observable<DataDeletionRequestDto[]> {
        return this.http.get<DataDeletionRequestDto[]>(
            `${this.baseUrl}/data-deletions`
        );
    }

    requestDeletion(request: CreateDeletionRequestDto): Observable<DataDeletionRequestDto> {
        return this.http.post<DataDeletionRequestDto>(
            `${this.baseUrl}/data-deletions`,
            request
        );
    }

    approveDeletion(deletionRequestId: string): Observable<void> {
        return this.http.post<void>(
            `${this.baseUrl}/data-deletions/${deletionRequestId}/approve`,
            {}
        );
    }

    executeDeletion(deletionRequestId: string): Observable<void> {
        return this.http.post<void>(
            `${this.baseUrl}/data-deletions/${deletionRequestId}/execute`,
            {}
        );
    }

    verifyDeletion(customerId: string): Observable<DeletionVerificationDto> {
        return this.http.get<DeletionVerificationDto>(
            `${this.baseUrl}/data-deletions/${customerId}/verify`
        );
    }
}
```

---

## 5. State Management

### 5.1 Audit Log State
```typescript
@Injectable({ providedIn: 'root' })
export class AuditLogStateService {
    private readonly auditLogService = inject(AuditLogService);

    private readonly logsSubject = new BehaviorSubject<AuditLogDto[]>([]);
    private readonly loadingSubject = new BehaviorSubject<boolean>(false);
    private readonly filtersSubject = new BehaviorSubject<AuditLogFilters>({});

    readonly logs$ = this.logsSubject.asObservable();
    readonly loading$ = this.loadingSubject.asObservable();
    readonly filters$ = this.filtersSubject.asObservable();

    loadAuditLogs(filters: AuditLogFilters): void {
        this.loadingSubject.next(true);
        this.filtersSubject.next(filters);

        this.auditLogService.getAuditLogs(filters).pipe(
            finalize(() => this.loadingSubject.next(false))
        ).subscribe(result => {
            this.logsSubject.next(result.items);
        });
    }

    refreshLogs(): void {
        this.loadAuditLogs(this.filtersSubject.value);
    }
}
```

### 5.2 Export State
```typescript
@Injectable({ providedIn: 'root' })
export class ExportStateService {
    private readonly exportService = inject(DataExportService);

    private readonly exportsSubject = new BehaviorSubject<ExportDetailDto[]>([]);
    readonly exports$ = this.exportsSubject.asObservable();

    loadExports(): void {
        this.exportService.getExports().subscribe(result => {
            this.exportsSubject.next(result.items);
        });
    }

    addExport(exportDto: ExportDetailDto): void {
        const current = this.exportsSubject.value;
        this.exportsSubject.next([exportDto, ...current]);
    }

    updateExportStatus(exportId: string, status: ExportDetailDto): void {
        const current = this.exportsSubject.value;
        const updated = current.map(e =>
            e.dataExportId === exportId ? status : e
        );
        this.exportsSubject.next(updated);
    }
}
```

---

## 6. Routing Configuration

```typescript
export const auditComplianceRoutes: Routes = [
    {
        path: 'audit-compliance',
        children: [
            {
                path: 'audit-logs',
                component: AuditLogListComponent,
                data: { title: 'Audit Logs' }
            },
            {
                path: 'audit-logs/:id',
                component: AuditLogDetailComponent,
                data: { title: 'Audit Log Detail' }
            },
            {
                path: 'audit-trail/:entityType/:entityId',
                component: EntityAuditTrailComponent,
                data: { title: 'Audit Trail' }
            },
            {
                path: 'exports',
                component: ExportListComponent,
                data: { title: 'Data Exports' }
            },
            {
                path: 'exports/create',
                component: ExportCreateComponent,
                data: { title: 'Create Export' }
            },
            {
                path: 'imports',
                component: ImportListComponent,
                data: { title: 'Data Imports' }
            },
            {
                path: 'imports/upload',
                component: ImportUploadComponent,
                data: { title: 'Upload Import' }
            },
            {
                path: 'imports/:id',
                component: ImportDetailComponent,
                data: { title: 'Import Detail' }
            },
            {
                path: 'backups',
                component: BackupListComponent,
                data: { title: 'System Backups' }
            },
            {
                path: 'backups/restore',
                component: BackupRestoreComponent,
                data: { title: 'Restore Data' }
            },
            {
                path: 'consents',
                component: ConsentManagementComponent,
                data: { title: 'Consent Management' }
            },
            {
                path: 'privacy-center',
                component: PrivacyCenterComponent,
                data: { title: 'Privacy Center' }
            },
            {
                path: 'data-deletions',
                component: DeletionRequestsComponent,
                data: { title: 'Data Deletion Requests' }
            },
            {
                path: 'data-deletions/request',
                component: RequestDeletionComponent,
                data: { title: 'Request Data Deletion' }
            },
            {
                path: 'dashboard',
                component: ComplianceDashboardComponent,
                data: { title: 'Compliance Dashboard' }
            },
            {
                path: '',
                redirectTo: 'dashboard',
                pathMatch: 'full'
            }
        ]
    }
];
```

---

## 7. UI/UX Specifications

### 7.1 Color Coding
| Element | Color | Usage |
|---------|-------|-------|
| Created Operation | Green | Success/Creation indicators |
| Updated Operation | Blue | Update/Modification indicators |
| Deleted Operation | Red | Deletion/Warning indicators |
| Accessed Operation | Purple | Read access indicators |
| Exported Operation | Orange | Export/Download indicators |

### 7.2 Status Badges
```scss
.status-badge {
  padding: 4px 12px;
  border-radius: 12px;
  font-size: 12px;
  font-weight: 500;

  &--pending { background: #FFF3CD; color: #856404; }
  &--in-progress { background: #D1ECF1; color: #0C5460; }
  &--completed { background: #D4EDDA; color: #155724; }
  &--failed { background: #F8D7DA; color: #721C24; }
  &--expired { background: #E2E3E5; color: #383D41; }
}
```

### 7.3 Loading States
- **Skeleton loaders** for tables during initial load
- **Progress bars** for export/import operations
- **Spinners** for quick operations (< 3 seconds)
- **Progress indicators** with percentage for long operations

### 7.4 Empty States
```html
<div class="empty-state" *ngIf="!hasData">
  <mat-icon>folder_open</mat-icon>
  <h3>No audit logs found</h3>
  <p>Audit logs will appear here as users perform actions</p>
</div>
```

### 7.5 Error States
```html
<mat-error *ngIf="errorMessage">
  <mat-icon>error_outline</mat-icon>
  <span>{{errorMessage}}</span>
  <button mat-button (click)="retry()">Retry</button>
</mat-error>
```

---

## 8. Accessibility Requirements

### 8.1 WCAG 2.1 AA Compliance
| Requirement | Implementation |
|-------------|----------------|
| Keyboard Navigation | All interactive elements focusable and operable |
| Screen Readers | ARIA labels on all components |
| Color Contrast | Minimum 4.5:1 for normal text, 3:1 for large text |
| Focus Indicators | Visible focus outline on all interactive elements |
| Form Labels | All form fields have associated labels |

### 8.2 ARIA Attributes
```html
<button
  mat-icon-button
  [attr.aria-label]="'Download export ' + export.name"
  [attr.aria-disabled]="export.status !== 'Completed'">
  <mat-icon>download</mat-icon>
</button>
```

### 8.3 Keyboard Shortcuts
| Shortcut | Action |
|----------|--------|
| Ctrl/Cmd + K | Global search |
| Ctrl/Cmd + E | Create export |
| Ctrl/Cmd + F | Focus filter panel |
| Esc | Close dialogs/panels |

---

## 9. Performance Optimization

### 9.1 Virtual Scrolling
```typescript
// For large audit log tables
<cdk-virtual-scroll-viewport itemSize="48" class="audit-log-viewport">
  <table mat-table [dataSource]="auditLogs">
    <!-- table content -->
  </table>
</cdk-virtual-scroll-viewport>
```

### 9.2 Lazy Loading
- Route-level code splitting for each feature
- Image lazy loading for charts and visualizations
- Pagination for all list views (default 25 items)

### 9.3 Caching
- Cache audit logs for 5 minutes
- Cache consent policies for 1 hour
- Cache backup list for 10 minutes
- Invalidate cache on mutations

### 9.4 Debouncing
- Search input debounced at 300ms
- Filter changes debounced at 500ms
- Auto-refresh configurable (default: 60 seconds)

---

## 10. Testing Requirements

### 10.1 Unit Tests
```typescript
describe('AuditLogListComponent', () => {
    it('should load audit logs on init', () => {
        // Test implementation
    });

    it('should apply filters when filter changes', () => {
        // Test implementation
    });

    it('should refresh logs every 60 seconds', fakeAsync(() => {
        // Test implementation with tick
    }));
});
```

### 10.2 Integration Tests
- Test export download flow
- Test import upload and validation flow
- Test consent recording and revocation
- Test data deletion request workflow

### 10.3 E2E Tests (Playwright)
```typescript
test('should export audit logs', async ({ page }) => {
    await page.goto('/audit-compliance/audit-logs');
    await page.click('button:has-text("Export")');
    await page.selectOption('select[name="exportType"]', 'AuditLogs');
    await page.click('button:has-text("Create Export")');
    await expect(page.locator('.export-status')).toContainText('Completed');
    await page.click('button:has-text("Download")');
    // Verify download
});
```

### 10.4 Accessibility Tests
- Automated testing with axe-core
- Manual keyboard navigation testing
- Screen reader testing (NVDA/JAWS)

---

## 11. Security Considerations

### 11.1 Data Masking
```typescript
export function maskSensitiveData(data: any, fields: string[]): any {
    const masked = { ...data };
    fields.forEach(field => {
        if (masked[field]) {
            masked[field] = '***MASKED***';
        }
    });
    return masked;
}
```

### 11.2 Secure File Handling
- Validate file types before upload
- Scan for malware (backend)
- Limit file sizes (50MB max)
- Use signed URLs for downloads

### 11.3 CSRF Protection
- CSRF tokens on all POST/PUT/DELETE operations
- SameSite cookies for session management

---

## 12. Internationalization (i18n)

### 12.1 Translation Keys
```json
{
  "audit.logs.title": "Audit Logs",
  "audit.logs.empty": "No audit logs found",
  "audit.operation.created": "Created",
  "audit.operation.updated": "Updated",
  "audit.operation.deleted": "Deleted",
  "export.status.pending": "Processing request...",
  "export.status.completed": "Ready for download",
  "consent.privacy_policy": "Privacy Policy",
  "consent.terms_of_service": "Terms of Service"
}
```

### 12.2 Date Formatting
- Use Angular DatePipe with locale
- Support multiple date formats based on region
- Display timezone information

---

## 13. Appendices

### 13.1 Related Documents
- [Backend Specification](./backend-spec.md)
- [Use Case Diagram](./diagrams/use-case.drawio)
- [C4 Diagrams](./diagrams/c4-context.drawio)
- [Class Diagram](./plantuml/class-diagram.plantuml)
- [Sequence Diagrams](./plantuml/sequence-diagram.plantuml)

### 13.2 Third-Party Libraries
| Library | Version | Purpose |
|---------|---------|---------|
| @angular/material | 18.x | UI components |
| @angular/cdk | 18.x | Component Dev Kit |
| chart.js | 4.x | Data visualization |
| ng2-charts | 6.x | Angular Chart.js wrapper |
| ngx-file-drop | 16.x | File upload component |
| date-fns | 3.x | Date manipulation |

### 13.3 Revision History
| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-22 | System | Initial specification |
