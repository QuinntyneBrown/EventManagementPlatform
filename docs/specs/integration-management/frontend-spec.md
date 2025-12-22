# Integration & External System Management - Frontend Specification

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
This document specifies the frontend requirements for the Integration & External System Management module, providing administrators and developers with intuitive interfaces to manage external integrations, API keys, webhooks, and data synchronization.

### 1.2 Scope
This specification covers:
- Integration configuration and management UI
- API key generation and management interfaces
- Webhook subscription and monitoring dashboards
- Data synchronization monitoring and control
- Real-time status updates and notifications
- Integration health monitoring dashboards

### 1.3 Technology Stack
- **Framework**: Angular 18+
- **UI Library**: Angular Material 18+
- **State Management**: NgRx (Store, Effects, Entity)
- **Real-time**: SignalR for live updates
- **Charts**: Chart.js / ng2-charts
- **Forms**: Reactive Forms with custom validators
- **HTTP**: HttpClient with interceptors
- **Routing**: Angular Router with guards
- **Icons**: Material Icons, Font Awesome
- **Notifications**: Angular Material Snackbar

---

## 2. Application Structure

### 2.1 Module Organization
```
src/app/features/integration-management/
├── integration-management.module.ts
├── integration-management-routing.module.ts
├── components/
│   ├── integration-list/
│   │   ├── integration-list.component.ts
│   │   ├── integration-list.component.html
│   │   ├── integration-list.component.scss
│   │   └── integration-list.component.spec.ts
│   ├── integration-detail/
│   │   ├── integration-detail.component.ts
│   │   ├── integration-detail.component.html
│   │   └── integration-detail.component.scss
│   ├── integration-form/
│   │   ├── integration-form.component.ts
│   │   ├── integration-form.component.html
│   │   └── integration-form.component.scss
│   ├── apikey-list/
│   ├── apikey-form/
│   ├── apikey-detail/
│   ├── webhook-list/
│   ├── webhook-form/
│   ├── webhook-detail/
│   ├── webhook-delivery-log/
│   ├── datasync-list/
│   ├── datasync-detail/
│   ├── datasync-monitor/
│   ├── integration-health-dashboard/
│   └── integration-wizard/
├── services/
│   ├── integration.service.ts
│   ├── apikey.service.ts
│   ├── webhook.service.ts
│   ├── datasync.service.ts
│   └── integration-signalr.service.ts
├── models/
│   ├── integration.model.ts
│   ├── apikey.model.ts
│   ├── webhook.model.ts
│   ├── datasync.model.ts
│   └── enums.ts
├── store/
│   ├── actions/
│   ├── reducers/
│   ├── effects/
│   └── selectors/
└── guards/
    └── integration-admin.guard.ts
```

### 2.2 Routing Structure
```typescript
const routes: Routes = [
  {
    path: 'integrations',
    component: IntegrationManagementComponent,
    canActivate: [IntegrationAdminGuard],
    children: [
      { path: '', component: IntegrationListComponent },
      { path: 'new', component: IntegrationWizardComponent },
      { path: ':id', component: IntegrationDetailComponent },
      { path: ':id/edit', component: IntegrationFormComponent },
      { path: ':id/health', component: IntegrationHealthDashboardComponent }
    ]
  },
  {
    path: 'api-keys',
    component: APIKeyManagementComponent,
    canActivate: [IntegrationAdminGuard],
    children: [
      { path: '', component: APIKeyListComponent },
      { path: 'new', component: APIKeyFormComponent },
      { path: ':id', component: APIKeyDetailComponent }
    ]
  },
  {
    path: 'webhooks',
    component: WebhookManagementComponent,
    canActivate: [IntegrationAdminGuard],
    children: [
      { path: '', component: WebhookListComponent },
      { path: 'new', component: WebhookFormComponent },
      { path: ':id', component: WebhookDetailComponent },
      { path: ':id/deliveries', component: WebhookDeliveryLogComponent }
    ]
  },
  {
    path: 'data-sync',
    component: DataSyncManagementComponent,
    canActivate: [IntegrationAdminGuard],
    children: [
      { path: '', component: DataSyncListComponent },
      { path: ':id', component: DataSyncDetailComponent },
      { path: 'monitor', component: DataSyncMonitorComponent }
    ]
  }
];
```

---

## 3. Data Models

### 3.1 Integration Models
```typescript
export interface Integration {
  integrationId: string;
  name: string;
  integrationType: IntegrationType;
  provider: string;
  status: IntegrationStatus;
  configuration: Record<string, any>;
  isEnabled: boolean;
  healthStatus: HealthStatus;
  lastHealthCheck?: Date;
  lastSyncTime?: Date;
  createdAt: Date;
  modifiedAt?: Date;
  createdBy: string;
  modifiedBy?: string;
}

export enum IntegrationType {
  PaymentGateway = 0,
  EmailService = 1,
  SMSService = 2,
  CalendarSync = 3,
  CustomAPI = 4,
  CRM = 5,
  Analytics = 6,
  Storage = 7
}

export enum IntegrationStatus {
  Draft = 0,
  PendingValidation = 1,
  Active = 2,
  Suspended = 3,
  Disconnected = 4,
  Error = 5
}

export enum HealthStatus {
  Healthy = 0,
  Degraded = 1,
  Unhealthy = 2,
  Unknown = 3
}

export interface IntegrationHealth {
  integrationId: string;
  healthStatus: HealthStatus;
  lastCheckTime: Date;
  responseTime: number;
  errorMessage?: string;
  metrics: {
    uptime: number;
    successRate: number;
    averageResponseTime: number;
  };
}
```

### 3.2 API Key Models
```typescript
export interface APIKey {
  apiKeyId: string;
  keyName: string;
  keyHash: string;
  keyPrefix: string;
  scopes: string[];
  expiresAt?: Date;
  isActive: boolean;
  lastUsedAt?: Date;
  usageCount: number;
  rateLimitPerHour: number;
  createdAt: Date;
  createdBy: string;
  revokedAt?: Date;
  revokedBy?: string;
}

export interface APIKeyCreate {
  keyName: string;
  scopes: string[];
  expiresAt?: Date;
  rateLimitPerHour: number;
}

export interface APIKeyResponse {
  apiKey: APIKey;
  plainTextKey: string; // Only returned once
}
```

### 3.3 Webhook Models
```typescript
export interface Webhook {
  webhookId: string;
  integrationId?: string;
  url: string;
  eventTypes: string[];
  secret: string;
  isActive: boolean;
  retryPolicy: RetryPolicy;
  maxRetries: number;
  timeoutSeconds: number;
  lastTriggeredAt?: Date;
  successCount: number;
  failureCount: number;
  createdAt: Date;
  createdBy: string;
  modifiedAt?: Date;
}

export interface RetryPolicy {
  maxRetries: number;
  backoffMultiplier: number;
  initialDelaySeconds: number;
}

export interface WebhookDelivery {
  webhookDeliveryId: string;
  webhookId: string;
  eventType: string;
  payload: any;
  httpStatusCode?: number;
  responseBody?: string;
  attemptCount: number;
  deliveryStatus: DeliveryStatus;
  errorMessage?: string;
  scheduledAt: Date;
  deliveredAt?: Date;
  nextRetryAt?: Date;
}

export enum DeliveryStatus {
  Pending = 0,
  Sending = 1,
  Delivered = 2,
  Failed = 3,
  Retrying = 4,
  Expired = 5
}
```

### 3.4 Data Sync Models
```typescript
export interface DataSyncJob {
  dataSyncJobId: string;
  integrationId: string;
  syncType: SyncType;
  direction: SyncDirection;
  status: SyncStatus;
  recordsTotal: number;
  recordsProcessed: number;
  recordsFailed: number;
  errorLog?: any[];
  startedAt?: Date;
  completedAt?: Date;
  scheduledAt: Date;
  createdAt: Date;
  createdBy: string;
  progress: number; // Calculated: (recordsProcessed / recordsTotal) * 100
}

export enum SyncType {
  Full = 0,
  Incremental = 1,
  Differential = 2
}

export enum SyncDirection {
  Import = 0,
  Export = 1,
  Bidirectional = 2
}

export enum SyncStatus {
  Scheduled = 0,
  Running = 1,
  Completed = 2,
  Failed = 3,
  Cancelled = 4,
  PartialSuccess = 5
}
```

---

## 4. User Interface Components

### 4.1 Integration List Component

#### 4.1.1 Layout
- **Header**: "Integrations" title, "+ New Integration" button
- **Filter Bar**: Type, Status, Provider filters, Search box
- **Grid/List View Toggle**
- **Integration Cards/Table**:
  - Card view: Provider logo, name, type badge, status indicator, health icon
  - Table view: Columns for Name, Type, Provider, Status, Health, Last Sync, Actions

#### 4.1.2 Features
- Real-time status updates via SignalR
- Health status indicators (green/yellow/red dots)
- Quick actions: Test, Configure, Disable/Enable, Delete
- Bulk operations: Enable/Disable multiple
- Sort by: Name, Type, Status, Last Sync
- Pagination: 10, 25, 50, 100 per page

#### 4.1.3 Visual Design
```html
<mat-card>
  <mat-card-header>
    <mat-card-title>Integrations</mat-card-title>
    <button mat-raised-button color="primary" (click)="createIntegration()">
      <mat-icon>add</mat-icon> New Integration
    </button>
  </mat-card-header>

  <mat-card-content>
    <!-- Filter Bar -->
    <div class="filter-bar">
      <mat-form-field>
        <mat-label>Type</mat-label>
        <mat-select [(value)]="filterType">
          <mat-option value="">All Types</mat-option>
          <mat-option value="PaymentGateway">Payment Gateway</mat-option>
          <mat-option value="EmailService">Email Service</mat-option>
          <mat-option value="SMSService">SMS Service</mat-option>
          <mat-option value="CalendarSync">Calendar Sync</mat-option>
        </mat-select>
      </mat-form-field>

      <mat-form-field>
        <mat-label>Status</mat-label>
        <mat-select [(value)]="filterStatus">
          <mat-option value="">All Statuses</mat-option>
          <mat-option value="Active">Active</mat-option>
          <mat-option value="Suspended">Suspended</mat-option>
          <mat-option value="Error">Error</mat-option>
        </mat-select>
      </mat-form-field>

      <mat-form-field class="search-box">
        <mat-label>Search</mat-label>
        <input matInput [(ngModel)]="searchTerm" placeholder="Search integrations...">
        <mat-icon matSuffix>search</mat-icon>
      </mat-form-field>

      <button mat-icon-button [matMenuTriggerFor]="viewMenu">
        <mat-icon>view_module</mat-icon>
      </button>
    </div>

    <!-- Integration Grid (Card View) -->
    <div class="integration-grid" *ngIf="viewMode === 'grid'">
      <mat-card *ngFor="let integration of integrations$ | async" class="integration-card">
        <mat-card-header>
          <div mat-card-avatar [style.background-image]="'url(' + integration.providerLogo + ')'"></div>
          <mat-card-title>{{ integration.name }}</mat-card-title>
          <mat-card-subtitle>{{ integration.provider }}</mat-card-subtitle>

          <div class="status-indicators">
            <mat-chip [class]="'status-' + integration.status">
              {{ integration.status }}
            </mat-chip>
            <mat-icon [class]="'health-' + integration.healthStatus" matTooltip="Health: {{ integration.healthStatus }}">
              {{ getHealthIcon(integration.healthStatus) }}
            </mat-icon>
          </div>
        </mat-card-header>

        <mat-card-content>
          <div class="integration-info">
            <div class="info-row">
              <span class="label">Type:</span>
              <span class="value">{{ integration.integrationType }}</span>
            </div>
            <div class="info-row">
              <span class="label">Last Sync:</span>
              <span class="value">{{ integration.lastSyncTime | date:'short' }}</span>
            </div>
          </div>
        </mat-card-content>

        <mat-card-actions>
          <button mat-button (click)="testConnection(integration)">
            <mat-icon>check_circle</mat-icon> Test
          </button>
          <button mat-button (click)="viewIntegration(integration)">
            <mat-icon>visibility</mat-icon> View
          </button>
          <button mat-icon-button [matMenuTriggerFor]="actionMenu">
            <mat-icon>more_vert</mat-icon>
          </button>

          <mat-menu #actionMenu="matMenu">
            <button mat-menu-item (click)="editIntegration(integration)">
              <mat-icon>edit</mat-icon> Edit
            </button>
            <button mat-menu-item (click)="toggleEnabled(integration)">
              <mat-icon>{{ integration.isEnabled ? 'pause' : 'play_arrow' }}</mat-icon>
              {{ integration.isEnabled ? 'Disable' : 'Enable' }}
            </button>
            <button mat-menu-item (click)="deleteIntegration(integration)">
              <mat-icon>delete</mat-icon> Delete
            </button>
          </mat-menu>
        </mat-card-actions>
      </mat-card>
    </div>

    <!-- Pagination -->
    <mat-paginator [length]="totalCount" [pageSize]="pageSize" [pageSizeOptions]="[10, 25, 50, 100]">
    </mat-paginator>
  </mat-card-content>
</mat-card>
```

### 4.2 Integration Wizard Component

#### 4.2.1 Multi-Step Form
- **Step 1: Select Integration Type**
  - Card-based selection with provider logos
  - Categories: Payment, Communication, Calendar, Custom
- **Step 2: Provider Selection**
  - Popular providers for selected type
  - Search functionality
- **Step 3: Configuration**
  - Dynamic form based on provider requirements
  - Credential validation
  - Test connection before saving
- **Step 4: Review & Activate**
  - Summary of configuration
  - Final validation
  - Activate integration

#### 4.2.2 Visual Design
```html
<mat-card class="wizard-container">
  <mat-horizontal-stepper [linear]="true" #stepper>
    <!-- Step 1: Type Selection -->
    <mat-step [stepControl]="typeFormGroup">
      <ng-template matStepLabel>Select Type</ng-template>
      <div class="step-content">
        <h3>What type of integration would you like to add?</h3>
        <div class="type-grid">
          <mat-card class="type-card" *ngFor="let type of integrationTypes"
                    [class.selected]="selectedType === type"
                    (click)="selectType(type)">
            <mat-icon class="type-icon">{{ type.icon }}</mat-icon>
            <h4>{{ type.name }}</h4>
            <p>{{ type.description }}</p>
          </mat-card>
        </div>
        <div class="wizard-actions">
          <button mat-raised-button color="primary" matStepperNext [disabled]="!selectedType">
            Next
          </button>
        </div>
      </div>
    </mat-step>

    <!-- Step 2: Provider Selection -->
    <mat-step [stepControl]="providerFormGroup">
      <ng-template matStepLabel>Select Provider</ng-template>
      <div class="step-content">
        <h3>Choose your {{ selectedType.name }} provider</h3>

        <mat-form-field class="search-provider">
          <mat-label>Search providers</mat-label>
          <input matInput [(ngModel)]="providerSearch" placeholder="Search...">
          <mat-icon matSuffix>search</mat-icon>
        </mat-form-field>

        <div class="provider-grid">
          <mat-card class="provider-card" *ngFor="let provider of filteredProviders"
                    [class.selected]="selectedProvider === provider"
                    (click)="selectProvider(provider)">
            <img [src]="provider.logo" [alt]="provider.name">
            <h4>{{ provider.name }}</h4>
            <mat-chip-list>
              <mat-chip *ngIf="provider.recommended">Recommended</mat-chip>
              <mat-chip *ngIf="provider.popular">Popular</mat-chip>
            </mat-chip-list>
          </mat-card>
        </div>

        <div class="wizard-actions">
          <button mat-button matStepperPrevious>Back</button>
          <button mat-raised-button color="primary" matStepperNext [disabled]="!selectedProvider">
            Next
          </button>
        </div>
      </div>
    </mat-step>

    <!-- Step 3: Configuration -->
    <mat-step [stepControl]="configFormGroup">
      <ng-template matStepLabel>Configure</ng-template>
      <div class="step-content">
        <h3>Configure {{ selectedProvider.name }} Integration</h3>

        <form [formGroup]="configFormGroup">
          <mat-form-field>
            <mat-label>Integration Name</mat-label>
            <input matInput formControlName="name" placeholder="My {{ selectedProvider.name }} Integration">
            <mat-error *ngIf="configFormGroup.get('name')?.hasError('required')">
              Name is required
            </mat-error>
          </mat-form-field>

          <!-- Dynamic fields based on provider -->
          <ng-container *ngFor="let field of selectedProvider.configFields">
            <mat-form-field *ngIf="field.type === 'text'">
              <mat-label>{{ field.label }}</mat-label>
              <input matInput [formControlName]="field.name" [type]="field.secure ? 'password' : 'text'">
              <mat-hint>{{ field.hint }}</mat-hint>
              <mat-error *ngIf="configFormGroup.get(field.name)?.hasError('required')">
                {{ field.label }} is required
              </mat-error>
            </mat-form-field>

            <mat-form-field *ngIf="field.type === 'select'">
              <mat-label>{{ field.label }}</mat-label>
              <mat-select [formControlName]="field.name">
                <mat-option *ngFor="let option of field.options" [value]="option.value">
                  {{ option.label }}
                </mat-option>
              </mat-select>
            </mat-form-field>
          </ng-container>

          <div class="test-connection" *ngIf="configFormGroup.valid">
            <button mat-raised-button (click)="testConnection()" [disabled]="testingConnection">
              <mat-icon>{{ testingConnection ? 'hourglass_empty' : 'check_circle' }}</mat-icon>
              {{ testingConnection ? 'Testing...' : 'Test Connection' }}
            </button>

            <div class="test-result" *ngIf="testResult">
              <mat-icon [class]="testResult.success ? 'success' : 'error'">
                {{ testResult.success ? 'check_circle' : 'error' }}
              </mat-icon>
              <span>{{ testResult.message }}</span>
            </div>
          </div>
        </form>

        <div class="wizard-actions">
          <button mat-button matStepperPrevious>Back</button>
          <button mat-raised-button color="primary" matStepperNext
                  [disabled]="!configFormGroup.valid || !testResult?.success">
            Next
          </button>
        </div>
      </div>
    </mat-step>

    <!-- Step 4: Review -->
    <mat-step>
      <ng-template matStepLabel>Review & Activate</ng-template>
      <div class="step-content">
        <h3>Review Integration Configuration</h3>

        <mat-card class="review-card">
          <mat-card-header>
            <mat-card-title>{{ configFormGroup.get('name')?.value }}</mat-card-title>
            <mat-card-subtitle>{{ selectedProvider.name }} - {{ selectedType.name }}</mat-card-subtitle>
          </mat-card-header>

          <mat-card-content>
            <div class="review-section">
              <h4>Configuration Details</h4>
              <mat-list>
                <mat-list-item *ngFor="let field of selectedProvider.configFields">
                  <span matListItemTitle>{{ field.label }}</span>
                  <span matListItemLine>
                    {{ field.secure ? '••••••••' : configFormGroup.get(field.name)?.value }}
                  </span>
                </mat-list-item>
              </mat-list>
            </div>

            <div class="review-section">
              <h4>Capabilities</h4>
              <mat-chip-list>
                <mat-chip *ngFor="let capability of selectedProvider.capabilities">
                  {{ capability }}
                </mat-chip>
              </mat-chip-list>
            </div>
          </mat-card-content>
        </mat-card>

        <div class="wizard-actions">
          <button mat-button matStepperPrevious>Back</button>
          <button mat-raised-button color="primary" (click)="createIntegration()" [disabled]="creating">
            <mat-icon>{{ creating ? 'hourglass_empty' : 'check' }}</mat-icon>
            {{ creating ? 'Creating...' : 'Create Integration' }}
          </button>
        </div>
      </div>
    </mat-step>
  </mat-horizontal-stepper>
</mat-card>
```

### 4.3 Integration Health Dashboard Component

#### 4.3.1 Features
- Real-time health monitoring
- Response time charts
- Uptime percentage
- Success/failure rate graphs
- Recent errors list
- Health history timeline

#### 4.3.2 Visual Design
```html
<div class="health-dashboard">
  <mat-card class="health-summary">
    <mat-card-header>
      <mat-card-title>Integration Health</mat-card-title>
      <div class="health-status-badge" [class]="'status-' + currentHealth">
        <mat-icon>{{ getHealthIcon(currentHealth) }}</mat-icon>
        <span>{{ currentHealth }}</span>
      </div>
    </mat-card-header>

    <mat-card-content>
      <div class="metrics-grid">
        <div class="metric-card">
          <div class="metric-value">{{ uptime }}%</div>
          <div class="metric-label">Uptime (30 days)</div>
          <mat-progress-bar mode="determinate" [value]="uptime"></mat-progress-bar>
        </div>

        <div class="metric-card">
          <div class="metric-value">{{ successRate }}%</div>
          <div class="metric-label">Success Rate</div>
          <mat-progress-bar mode="determinate" [value]="successRate" color="accent"></mat-progress-bar>
        </div>

        <div class="metric-card">
          <div class="metric-value">{{ avgResponseTime }}ms</div>
          <div class="metric-label">Avg Response Time</div>
        </div>

        <div class="metric-card">
          <div class="metric-value">{{ lastCheckTime | date:'short' }}</div>
          <div class="metric-label">Last Health Check</div>
        </div>
      </div>

      <mat-divider></mat-divider>

      <div class="charts-section">
        <div class="chart-container">
          <h4>Response Time (24 hours)</h4>
          <canvas baseChart [data]="responseTimeData" [options]="chartOptions" [type]="'line'">
          </canvas>
        </div>

        <div class="chart-container">
          <h4>Request Volume</h4>
          <canvas baseChart [data]="requestVolumeData" [options]="chartOptions" [type]="'bar'">
          </canvas>
        </div>
      </div>

      <mat-divider></mat-divider>

      <div class="recent-errors">
        <h4>Recent Errors</h4>
        <mat-list *ngIf="recentErrors.length > 0; else noErrors">
          <mat-list-item *ngFor="let error of recentErrors">
            <mat-icon matListItemIcon color="warn">error</mat-icon>
            <div matListItemTitle>{{ error.message }}</div>
            <div matListItemLine>{{ error.timestamp | date:'medium' }}</div>
          </mat-list-item>
        </mat-list>
        <ng-template #noErrors>
          <div class="no-errors">
            <mat-icon color="primary">check_circle</mat-icon>
            <p>No recent errors</p>
          </div>
        </ng-template>
      </div>
    </mat-card-content>

    <mat-card-actions>
      <button mat-button (click)="runHealthCheck()">
        <mat-icon>refresh</mat-icon> Run Health Check
      </button>
      <button mat-button (click)="viewFullHistory()">
        <mat-icon>history</mat-icon> View Full History
      </button>
    </mat-card-actions>
  </mat-card>
</div>
```

### 4.4 API Key List Component

#### 4.4.1 Features
- List all API keys with usage statistics
- Show key prefix for identification
- Usage count and last used timestamp
- Rate limit progress indicators
- Expiration warnings
- Quick revoke action

#### 4.4.2 Visual Design
```html
<mat-card>
  <mat-card-header>
    <mat-card-title>API Keys</mat-card-title>
    <button mat-raised-button color="primary" (click)="generateKey()">
      <mat-icon>add</mat-icon> Generate New Key
    </button>
  </mat-card-header>

  <mat-card-content>
    <table mat-table [dataSource]="apiKeys$" class="api-keys-table">
      <!-- Name Column -->
      <ng-container matColumnDef="name">
        <th mat-header-cell *matHeaderCellDef>Name</th>
        <td mat-cell *matCellDef="let key">
          <div class="key-name">
            <strong>{{ key.keyName }}</strong>
            <code class="key-prefix">{{ key.keyPrefix }}...</code>
          </div>
        </td>
      </ng-container>

      <!-- Scopes Column -->
      <ng-container matColumnDef="scopes">
        <th mat-header-cell *matHeaderCellDef>Scopes</th>
        <td mat-cell *matCellDef="let key">
          <mat-chip-list>
            <mat-chip *ngFor="let scope of key.scopes | slice:0:3">{{ scope }}</mat-chip>
            <mat-chip *ngIf="key.scopes.length > 3" matTooltip="{{ key.scopes.slice(3).join(', ') }}">
              +{{ key.scopes.length - 3 }} more
            </mat-chip>
          </mat-chip-list>
        </td>
      </ng-container>

      <!-- Usage Column -->
      <ng-container matColumnDef="usage">
        <th mat-header-cell *matHeaderCellDef>Usage</th>
        <td mat-cell *matCellDef="let key">
          <div class="usage-info">
            <div class="usage-count">{{ key.usageCount | number }} requests</div>
            <div class="last-used" *ngIf="key.lastUsedAt">
              Last used: {{ key.lastUsedAt | date:'short' }}
            </div>
          </div>
        </td>
      </ng-container>

      <!-- Rate Limit Column -->
      <ng-container matColumnDef="rateLimit">
        <th mat-header-cell *matHeaderCellDef>Rate Limit</th>
        <td mat-cell *matCellDef="let key">
          <div class="rate-limit-info">
            <span>{{ key.rateLimitPerHour }} req/hr</span>
            <mat-progress-bar mode="determinate"
                              [value]="getCurrentUsagePercent(key)"
                              [color]="getCurrentUsagePercent(key) > 80 ? 'warn' : 'primary'">
            </mat-progress-bar>
          </div>
        </td>
      </ng-container>

      <!-- Expiration Column -->
      <ng-container matColumnDef="expiration">
        <th mat-header-cell *matHeaderCellDef>Expires</th>
        <td mat-cell *matCellDef="let key">
          <span *ngIf="key.expiresAt" [class.expiring-soon]="isExpiringSoon(key.expiresAt)">
            {{ key.expiresAt | date:'mediumDate' }}
          </span>
          <span *ngIf="!key.expiresAt">Never</span>
        </td>
      </ng-container>

      <!-- Status Column -->
      <ng-container matColumnDef="status">
        <th mat-header-cell *matHeaderCellDef>Status</th>
        <td mat-cell *matCellDef="let key">
          <mat-chip [class]="key.isActive ? 'active' : 'revoked'">
            {{ key.isActive ? 'Active' : 'Revoked' }}
          </mat-chip>
        </td>
      </ng-container>

      <!-- Actions Column -->
      <ng-container matColumnDef="actions">
        <th mat-header-cell *matHeaderCellDef>Actions</th>
        <td mat-cell *matCellDef="let key">
          <button mat-icon-button [matMenuTriggerFor]="menu">
            <mat-icon>more_vert</mat-icon>
          </button>
          <mat-menu #menu="matMenu">
            <button mat-menu-item (click)="viewDetails(key)">
              <mat-icon>visibility</mat-icon> View Details
            </button>
            <button mat-menu-item (click)="rotateKey(key)" [disabled]="!key.isActive">
              <mat-icon>sync</mat-icon> Rotate Key
            </button>
            <button mat-menu-item (click)="revokeKey(key)" [disabled]="!key.isActive">
              <mat-icon>block</mat-icon> Revoke
            </button>
          </mat-menu>
        </td>
      </ng-container>

      <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
      <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
    </table>

    <mat-paginator [length]="totalCount" [pageSize]="pageSize" [pageSizeOptions]="[10, 25, 50]">
    </mat-paginator>
  </mat-card-content>
</mat-card>
```

### 4.5 API Key Generation Dialog

#### 4.5.1 Visual Design
```html
<h2 mat-dialog-title>Generate New API Key</h2>

<mat-dialog-content>
  <form [formGroup]="keyForm">
    <mat-form-field>
      <mat-label>Key Name</mat-label>
      <input matInput formControlName="keyName" placeholder="e.g., Mobile App Production">
      <mat-hint>A descriptive name to identify this key</mat-hint>
      <mat-error *ngIf="keyForm.get('keyName')?.hasError('required')">
        Key name is required
      </mat-error>
    </mat-form-field>

    <mat-form-field>
      <mat-label>Scopes</mat-label>
      <mat-select formControlName="scopes" multiple>
        <mat-option *ngFor="let scope of availableScopes" [value]="scope.value">
          {{ scope.label }}
        </mat-option>
      </mat-select>
      <mat-hint>Select the permissions for this API key</mat-hint>
      <mat-error *ngIf="keyForm.get('scopes')?.hasError('required')">
        At least one scope is required
      </mat-error>
    </mat-form-field>

    <mat-form-field>
      <mat-label>Rate Limit (requests per hour)</mat-label>
      <input matInput type="number" formControlName="rateLimitPerHour" min="1" max="10000">
      <mat-hint>Maximum requests allowed per hour</mat-hint>
    </mat-form-field>

    <mat-form-field>
      <mat-label>Expiration Date (Optional)</mat-label>
      <input matInput [matDatepicker]="picker" formControlName="expiresAt">
      <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
      <mat-datepicker #picker></mat-datepicker>
      <mat-hint>Leave blank for no expiration</mat-hint>
    </mat-form-field>
  </form>

  <!-- Display generated key (after generation) -->
  <div class="generated-key" *ngIf="generatedKey">
    <mat-card class="warning-card">
      <mat-card-header>
        <mat-icon color="warn">warning</mat-icon>
        <mat-card-title>Save Your API Key</mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <p>This is the only time you'll see this key. Store it securely!</p>
        <div class="key-display">
          <code>{{ generatedKey }}</code>
          <button mat-icon-button (click)="copyToClipboard(generatedKey)" matTooltip="Copy to clipboard">
            <mat-icon>content_copy</mat-icon>
          </button>
        </div>
      </mat-card-content>
    </mat-card>
  </div>
</mat-dialog-content>

<mat-dialog-actions align="end">
  <button mat-button mat-dialog-close *ngIf="!generatedKey">Cancel</button>
  <button mat-raised-button color="primary" (click)="generateKey()"
          [disabled]="!keyForm.valid || generating" *ngIf="!generatedKey">
    {{ generating ? 'Generating...' : 'Generate Key' }}
  </button>
  <button mat-raised-button color="primary" mat-dialog-close *ngIf="generatedKey">
    Done
  </button>
</mat-dialog-actions>
```

### 4.6 Webhook List Component

#### 4.6.1 Features
- List webhooks with delivery statistics
- Success/failure rate visualization
- Quick enable/disable toggle
- Test webhook functionality
- View delivery log

#### 4.6.2 Visual Design (similar structure to API Keys with webhook-specific columns)

### 4.7 Webhook Delivery Log Component

#### 4.7.1 Features
- Chronological delivery history
- Filter by status, date range, event type
- Retry attempts visualization
- Request/response inspection
- Manual retry capability

#### 4.7.2 Visual Design
```html
<mat-card>
  <mat-card-header>
    <mat-card-title>Webhook Delivery Log</mat-card-title>
    <div class="stats-summary">
      <span class="stat">
        <mat-icon class="success">check_circle</mat-icon>
        {{ successCount }} delivered
      </span>
      <span class="stat">
        <mat-icon class="error">error</mat-icon>
        {{ failureCount }} failed
      </span>
      <span class="stat">
        <mat-icon class="pending">schedule</mat-icon>
        {{ pendingCount }} pending
      </span>
    </div>
  </mat-card-header>

  <mat-card-content>
    <!-- Filters -->
    <mat-expansion-panel class="filter-panel">
      <mat-expansion-panel-header>
        <mat-panel-title>
          <mat-icon>filter_list</mat-icon>
          Filters
        </mat-panel-title>
      </mat-expansion-panel-header>

      <div class="filter-content">
        <mat-form-field>
          <mat-label>Status</mat-label>
          <mat-select [(value)]="filterStatus">
            <mat-option value="">All</mat-option>
            <mat-option value="Delivered">Delivered</mat-option>
            <mat-option value="Failed">Failed</mat-option>
            <mat-option value="Retrying">Retrying</mat-option>
            <mat-option value="Pending">Pending</mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field>
          <mat-label>Event Type</mat-label>
          <mat-select [(value)]="filterEventType">
            <mat-option value="">All</mat-option>
            <mat-option *ngFor="let eventType of eventTypes" [value]="eventType">
              {{ eventType }}
            </mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field>
          <mat-label>Date Range</mat-label>
          <mat-date-range-input [rangePicker]="rangePicker">
            <input matStartDate placeholder="Start date" [(ngModel)]="startDate">
            <input matEndDate placeholder="End date" [(ngModel)]="endDate">
          </mat-date-range-input>
          <mat-datepicker-toggle matSuffix [for]="rangePicker"></mat-datepicker-toggle>
          <mat-date-range-picker #rangePicker></mat-date-range-picker>
        </mat-form-field>

        <button mat-raised-button (click)="applyFilters()">Apply</button>
        <button mat-button (click)="clearFilters()">Clear</button>
      </div>
    </mat-expansion-panel>

    <!-- Timeline View -->
    <mat-list class="delivery-timeline">
      <mat-list-item *ngFor="let delivery of deliveries$ | async" class="delivery-item">
        <mat-icon matListItemIcon [class]="'status-' + delivery.deliveryStatus">
          {{ getStatusIcon(delivery.deliveryStatus) }}
        </mat-icon>

        <div matListItemTitle class="delivery-header">
          <span class="event-type">{{ delivery.eventType }}</span>
          <mat-chip [class]="'status-' + delivery.deliveryStatus">
            {{ delivery.deliveryStatus }}
          </mat-chip>
          <span class="timestamp">{{ delivery.scheduledAt | date:'medium' }}</span>
        </div>

        <div matListItemLine class="delivery-details">
          <span>Attempts: {{ delivery.attemptCount }}</span>
          <span *ngIf="delivery.httpStatusCode">HTTP {{ delivery.httpStatusCode }}</span>
          <span *ngIf="delivery.deliveredAt">
            Delivered: {{ delivery.deliveredAt | date:'short' }}
          </span>
        </div>

        <div matListItemLine *ngIf="delivery.errorMessage" class="error-message">
          <mat-icon>error_outline</mat-icon>
          {{ delivery.errorMessage }}
        </div>

        <button mat-icon-button [matMenuTriggerFor]="deliveryMenu">
          <mat-icon>more_vert</mat-icon>
        </button>

        <mat-menu #deliveryMenu="matMenu">
          <button mat-menu-item (click)="viewPayload(delivery)">
            <mat-icon>code</mat-icon> View Payload
          </button>
          <button mat-menu-item (click)="viewResponse(delivery)" *ngIf="delivery.responseBody">
            <mat-icon>description</mat-icon> View Response
          </button>
          <button mat-menu-item (click)="retryDelivery(delivery)"
                  *ngIf="delivery.deliveryStatus === 'Failed'">
            <mat-icon>refresh</mat-icon> Retry Now
          </button>
        </mat-menu>
      </mat-list-item>
    </mat-list>

    <mat-paginator [length]="totalCount" [pageSize]="pageSize" [pageSizeOptions]="[10, 25, 50, 100]">
    </mat-paginator>
  </mat-card-content>
</mat-card>
```

### 4.8 Data Sync Monitor Component

#### 4.8.1 Features
- Real-time sync progress
- Multiple concurrent sync jobs display
- Progress bars with percentage
- Records processed/failed counters
- ETA calculation
- Error log access

#### 4.8.2 Visual Design
```html
<mat-card class="sync-monitor">
  <mat-card-header>
    <mat-card-title>Data Synchronization Monitor</mat-card-title>
    <button mat-raised-button color="primary" (click)="createSyncJob()">
      <mat-icon>sync</mat-icon> New Sync Job
    </button>
  </mat-card-header>

  <mat-card-content>
    <!-- Active Syncs -->
    <div class="active-syncs">
      <h3>Active Syncs</h3>
      <mat-card *ngFor="let sync of activeSyncs$ | async" class="sync-card">
        <mat-card-header>
          <mat-card-title>{{ sync.integrationName }}</mat-card-title>
          <mat-card-subtitle>
            <mat-chip [class]="'type-' + sync.syncType">{{ sync.syncType }}</mat-chip>
            <mat-chip [class]="'direction-' + sync.direction">{{ sync.direction }}</mat-chip>
          </mat-card-subtitle>
        </mat-card-header>

        <mat-card-content>
          <div class="sync-progress">
            <div class="progress-header">
              <span class="records-info">
                {{ sync.recordsProcessed | number }} / {{ sync.recordsTotal | number }} records
              </span>
              <span class="progress-percent">{{ sync.progress }}%</span>
            </div>

            <mat-progress-bar mode="determinate" [value]="sync.progress" color="primary">
            </mat-progress-bar>

            <div class="progress-details">
              <span *ngIf="sync.recordsFailed > 0" class="failures">
                <mat-icon>error</mat-icon>
                {{ sync.recordsFailed }} failed
              </span>
              <span class="eta" *ngIf="sync.estimatedCompletion">
                ETA: {{ sync.estimatedCompletion | date:'shortTime' }}
              </span>
              <span class="duration">
                Duration: {{ getElapsedTime(sync.startedAt) }}
              </span>
            </div>
          </div>

          <div class="sync-status" [class]="'status-' + sync.status">
            <mat-icon>{{ getStatusIcon(sync.status) }}</mat-icon>
            <span>{{ sync.status }}</span>
          </div>
        </mat-card-content>

        <mat-card-actions>
          <button mat-button (click)="viewDetails(sync)">
            <mat-icon>visibility</mat-icon> Details
          </button>
          <button mat-button (click)="viewErrors(sync)" *ngIf="sync.recordsFailed > 0">
            <mat-icon>error_outline</mat-icon> View Errors
          </button>
          <button mat-button (click)="cancelSync(sync)" *ngIf="sync.status === 'Running'">
            <mat-icon>cancel</mat-icon> Cancel
          </button>
        </mat-card-actions>
      </mat-card>

      <div class="no-active-syncs" *ngIf="(activeSyncs$ | async)?.length === 0">
        <mat-icon>sync_disabled</mat-icon>
        <p>No active synchronization jobs</p>
      </div>
    </div>

    <mat-divider></mat-divider>

    <!-- Recent Syncs -->
    <div class="recent-syncs">
      <h3>Recent Syncs</h3>
      <mat-list>
        <mat-list-item *ngFor="let sync of recentSyncs$ | async">
          <mat-icon matListItemIcon [class]="'status-' + sync.status">
            {{ getStatusIcon(sync.status) }}
          </mat-icon>

          <div matListItemTitle>
            {{ sync.integrationName }} - {{ sync.syncType }}
          </div>

          <div matListItemLine>
            {{ sync.completedAt | date:'medium' }} •
            {{ sync.recordsProcessed | number }} records •
            {{ sync.recordsFailed > 0 ? sync.recordsFailed + ' failures' : 'No failures' }}
          </div>

          <button mat-icon-button [matMenuTriggerFor]="syncMenu">
            <mat-icon>more_vert</mat-icon>
          </button>

          <mat-menu #syncMenu="matMenu">
            <button mat-menu-item (click)="viewDetails(sync)">
              <mat-icon>visibility</mat-icon> View Details
            </button>
            <button mat-menu-item (click)="retrySync(sync)">
              <mat-icon>refresh</mat-icon> Retry Sync
            </button>
          </mat-menu>
        </mat-list-item>
      </mat-list>
    </div>
  </mat-card-content>
</mat-card>
```

---

## 5. State Management (NgRx)

### 5.1 Integration State
```typescript
export interface IntegrationState {
  integrations: Integration[];
  selectedIntegration: Integration | null;
  loading: boolean;
  error: string | null;
  filter: IntegrationFilter;
  pagination: Pagination;
}

export interface IntegrationFilter {
  type?: IntegrationType;
  status?: IntegrationStatus;
  provider?: string;
  searchTerm?: string;
}
```

### 5.2 Actions
```typescript
// Integration Actions
export const loadIntegrations = createAction('[Integration] Load Integrations', props<{ filter?: IntegrationFilter }>());
export const loadIntegrationsSuccess = createAction('[Integration] Load Success', props<{ integrations: Integration[] }>());
export const loadIntegrationsFailure = createAction('[Integration] Load Failure', props<{ error: string }>());

export const createIntegration = createAction('[Integration] Create', props<{ integration: Partial<Integration> }>());
export const createIntegrationSuccess = createAction('[Integration] Create Success', props<{ integration: Integration }>());

export const updateIntegration = createAction('[Integration] Update', props<{ id: string, changes: Partial<Integration> }>());
export const deleteIntegration = createAction('[Integration] Delete', props<{ id: string }>());

export const testIntegration = createAction('[Integration] Test Connection', props<{ id: string }>());
export const toggleIntegrationEnabled = createAction('[Integration] Toggle Enabled', props<{ id: string }>());

// API Key Actions
export const generateAPIKey = createAction('[API Key] Generate', props<{ request: APIKeyCreate }>());
export const generateAPIKeySuccess = createAction('[API Key] Generate Success', props<{ response: APIKeyResponse }>());

export const revokeAPIKey = createAction('[API Key] Revoke', props<{ id: string, reason: string }>());
export const rotateAPIKey = createAction('[API Key] Rotate', props<{ id: string }>());

// Webhook Actions
export const createWebhook = createAction('[Webhook] Create', props<{ webhook: Partial<Webhook> }>());
export const testWebhook = createAction('[Webhook] Test', props<{ id: string }>());
export const deleteWebhook = createAction('[Webhook] Delete', props<{ id: string }>());

export const loadWebhookDeliveries = createAction('[Webhook] Load Deliveries', props<{ webhookId: string }>());
export const retryWebhookDelivery = createAction('[Webhook] Retry Delivery', props<{ deliveryId: string }>());

// Data Sync Actions
export const createDataSync = createAction('[Data Sync] Create', props<{ syncJob: Partial<DataSyncJob> }>());
export const cancelDataSync = createAction('[Data Sync] Cancel', props<{ id: string }>());
export const retryDataSync = createAction('[Data Sync] Retry', props<{ id: string }>());

// Real-time updates
export const integrationHealthUpdated = createAction('[Integration] Health Updated', props<{ id: string, health: HealthStatus }>());
export const dataSyncProgressUpdated = createAction('[Data Sync] Progress Updated', props<{ id: string, progress: number }>());
export const webhookDeliveryStatusUpdated = createAction('[Webhook] Delivery Status Updated', props<{ deliveryId: string, status: DeliveryStatus }>());
```

### 5.3 Effects
```typescript
@Injectable()
export class IntegrationEffects {
  loadIntegrations$ = createEffect(() =>
    this.actions$.pipe(
      ofType(IntegrationActions.loadIntegrations),
      switchMap(({ filter }) =>
        this.integrationService.getIntegrations(filter).pipe(
          map(integrations => IntegrationActions.loadIntegrationsSuccess({ integrations })),
          catchError(error => of(IntegrationActions.loadIntegrationsFailure({ error: error.message })))
        )
      )
    )
  );

  createIntegration$ = createEffect(() =>
    this.actions$.pipe(
      ofType(IntegrationActions.createIntegration),
      switchMap(({ integration }) =>
        this.integrationService.createIntegration(integration).pipe(
          map(created => {
            this.snackBar.open('Integration created successfully', 'Close', { duration: 3000 });
            this.router.navigate(['/integrations', created.integrationId]);
            return IntegrationActions.createIntegrationSuccess({ integration: created });
          }),
          catchError(error => {
            this.snackBar.open('Failed to create integration: ' + error.message, 'Close', { duration: 5000 });
            return of(IntegrationActions.loadIntegrationsFailure({ error: error.message }));
          })
        )
      )
    )
  );

  testIntegration$ = createEffect(() =>
    this.actions$.pipe(
      ofType(IntegrationActions.testIntegration),
      switchMap(({ id }) =>
        this.integrationService.testConnection(id).pipe(
          map(result => {
            this.snackBar.open(
              result.success ? 'Connection successful' : 'Connection failed: ' + result.message,
              'Close',
              { duration: 3000 }
            );
            return IntegrationActions.loadIntegrations({});
          }),
          catchError(error => {
            this.snackBar.open('Test failed: ' + error.message, 'Close', { duration: 5000 });
            return of(IntegrationActions.loadIntegrationsFailure({ error: error.message }));
          })
        )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private integrationService: IntegrationService,
    private snackBar: MatSnackBar,
    private router: Router
  ) {}
}
```

---

## 6. Services

### 6.1 Integration Service
```typescript
@Injectable({ providedIn: 'root' })
export class IntegrationService {
  private baseUrl = '/api/integrations';

  constructor(private http: HttpClient) {}

  getIntegrations(filter?: IntegrationFilter): Observable<Integration[]> {
    const params = this.buildFilterParams(filter);
    return this.http.get<Integration[]>(this.baseUrl, { params });
  }

  getIntegration(id: string): Observable<Integration> {
    return this.http.get<Integration>(`${this.baseUrl}/${id}`);
  }

  createIntegration(integration: Partial<Integration>): Observable<Integration> {
    return this.http.post<Integration>(this.baseUrl, integration);
  }

  updateIntegration(id: string, changes: Partial<Integration>): Observable<Integration> {
    return this.http.put<Integration>(`${this.baseUrl}/${id}`, changes);
  }

  deleteIntegration(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  testConnection(id: string): Observable<{ success: boolean; message: string }> {
    return this.http.post<{ success: boolean; message: string }>(`${this.baseUrl}/${id}/test`, {});
  }

  getHealth(id: string): Observable<IntegrationHealth> {
    return this.http.get<IntegrationHealth>(`${this.baseUrl}/${id}/health`);
  }

  private buildFilterParams(filter?: IntegrationFilter): HttpParams {
    let params = new HttpParams();
    if (filter) {
      if (filter.type !== undefined) params = params.set('type', filter.type.toString());
      if (filter.status !== undefined) params = params.set('status', filter.status.toString());
      if (filter.provider) params = params.set('provider', filter.provider);
      if (filter.searchTerm) params = params.set('search', filter.searchTerm);
    }
    return params;
  }
}
```

### 6.2 SignalR Service for Real-time Updates
```typescript
@Injectable({ providedIn: 'root' })
export class IntegrationSignalRService {
  private hubConnection!: signalR.HubConnection;

  public integrationHealthUpdated$ = new Subject<{ id: string; health: HealthStatus }>();
  public dataSyncProgressUpdated$ = new Subject<{ id: string; progress: number }>();
  public webhookDeliveryUpdated$ = new Subject<{ deliveryId: string; status: DeliveryStatus }>();

  constructor(private store: Store) {}

  public startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/hubs/integration')
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.error('Error starting SignalR connection: ', err));

    this.registerHandlers();
  }

  private registerHandlers(): void {
    this.hubConnection.on('IntegrationHealthUpdated', (id: string, health: HealthStatus) => {
      this.integrationHealthUpdated$.next({ id, health });
      this.store.dispatch(integrationHealthUpdated({ id, health }));
    });

    this.hubConnection.on('DataSyncProgressUpdated', (id: string, progress: number) => {
      this.dataSyncProgressUpdated$.next({ id, progress });
      this.store.dispatch(dataSyncProgressUpdated({ id, progress }));
    });

    this.hubConnection.on('WebhookDeliveryStatusUpdated', (deliveryId: string, status: DeliveryStatus) => {
      this.webhookDeliveryUpdated$.next({ deliveryId, status });
      this.store.dispatch(webhookDeliveryStatusUpdated({ deliveryId, status }));
    });
  }

  public stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }
}
```

---

## 7. Styling Guidelines

### 7.1 Color Scheme
```scss
// Health Status Colors
$health-healthy: #4caf50;
$health-degraded: #ff9800;
$health-unhealthy: #f44336;
$health-unknown: #9e9e9e;

// Integration Status Colors
$status-active: #2196f3;
$status-suspended: #ff9800;
$status-error: #f44336;
$status-draft: #9e9e9e;

// Webhook Delivery Colors
$delivery-delivered: #4caf50;
$delivery-failed: #f44336;
$delivery-pending: #ff9800;
$delivery-retrying: #2196f3;
```

### 7.2 Component Styling Example
```scss
.integration-card {
  margin: 16px;
  transition: transform 0.2s, box-shadow 0.2s;

  &:hover {
    transform: translateY(-4px);
    box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
  }

  .status-indicators {
    display: flex;
    align-items: center;
    gap: 8px;

    mat-chip {
      &.status-Active { background-color: $status-active; color: white; }
      &.status-Suspended { background-color: $status-suspended; color: white; }
      &.status-Error { background-color: $status-error; color: white; }
    }

    mat-icon {
      &.health-Healthy { color: $health-healthy; }
      &.health-Degraded { color: $health-degraded; }
      &.health-Unhealthy { color: $health-unhealthy; }
      &.health-Unknown { color: $health-unknown; }
    }
  }
}

.health-dashboard {
  .metrics-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
    gap: 16px;
    margin: 24px 0;

    .metric-card {
      padding: 16px;
      border: 1px solid #e0e0e0;
      border-radius: 8px;

      .metric-value {
        font-size: 32px;
        font-weight: 600;
        color: #2196f3;
      }

      .metric-label {
        font-size: 14px;
        color: #757575;
        margin-top: 8px;
      }
    }
  }
}
```

---

## 8. Validation Rules

### 8.1 Client-Side Validators
```typescript
export class IntegrationValidators {
  static integrationName(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (!value) return null;

    if (value.length < 1 || value.length > 200) {
      return { invalidLength: true };
    }
    return null;
  }

  static webhookUrl(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (!value) return null;

    try {
      const url = new URL(value);
      if (url.protocol !== 'https:' && url.hostname !== 'localhost') {
        return { httpsRequired: true };
      }
    } catch {
      return { invalidUrl: true };
    }
    return null;
  }

  static apiKeyScopes(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (!value || !Array.isArray(value) || value.length === 0) {
      return { scopesRequired: true };
    }
    return null;
  }

  static rateLimit(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (value < 1 || value > 10000) {
      return { invalidRateLimit: true };
    }
    return null;
  }
}
```

---

## 9. Error Handling

### 9.1 HTTP Interceptor
```typescript
@Injectable()
export class IntegrationHttpInterceptor implements HttpInterceptor {
  constructor(private snackBar: MatSnackBar, private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = 'An unknown error occurred';

        if (error.error instanceof ErrorEvent) {
          // Client-side error
          errorMessage = error.error.message;
        } else {
          // Server-side error
          switch (error.status) {
            case 400:
              errorMessage = 'Invalid request: ' + (error.error?.message || 'Please check your input');
              break;
            case 401:
              errorMessage = 'Unauthorized: Please log in again';
              this.router.navigate(['/login']);
              break;
            case 403:
              errorMessage = 'Forbidden: You do not have permission to perform this action';
              break;
            case 404:
              errorMessage = 'Not found: The requested resource does not exist';
              break;
            case 429:
              errorMessage = 'Rate limit exceeded: Please try again later';
              break;
            case 500:
              errorMessage = 'Server error: Please try again later';
              break;
            default:
              errorMessage = `Error ${error.status}: ${error.error?.message || error.message}`;
          }
        }

        this.snackBar.open(errorMessage, 'Close', {
          duration: 5000,
          panelClass: ['error-snackbar']
        });

        return throwError(() => error);
      })
    );
  }
}
```

---

## 10. Accessibility Requirements

### 10.1 WCAG 2.1 Level AA Compliance
- All interactive elements keyboard accessible
- ARIA labels on all icon buttons
- Sufficient color contrast (4.5:1 minimum)
- Focus indicators visible
- Screen reader announcements for dynamic content
- Form validation errors announced

### 10.2 Implementation Example
```html
<button mat-icon-button
        (click)="deleteIntegration(integration)"
        aria-label="Delete integration"
        [attr.aria-describedby]="'delete-desc-' + integration.integrationId">
  <mat-icon>delete</mat-icon>
</button>
<span [id]="'delete-desc-' + integration.integrationId" class="sr-only">
  Delete {{ integration.name }} integration permanently
</span>
```

---

## 11. Performance Optimization

### 11.1 Strategies
- Virtual scrolling for large lists (>100 items)
- Lazy loading of modules and components
- OnPush change detection strategy
- Debouncing search inputs (300ms)
- Caching integration providers list
- Pagination for all list views
- Image optimization for provider logos

### 11.2 Implementation
```typescript
@Component({
  selector: 'app-integration-list',
  templateUrl: './integration-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class IntegrationListComponent implements OnInit {
  searchControl = new FormControl('');

  ngOnInit(): void {
    // Debounce search
    this.searchControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(term => {
        this.store.dispatch(loadIntegrations({ filter: { searchTerm: term } }));
      });
  }
}
```

---

## 12. Testing Requirements

### 12.1 Unit Tests
- All components with >80% coverage
- All services and effects tested
- Validators tested with edge cases
- State reducers tested

### 12.2 Integration Tests
- End-to-end user flows
- Form submission and validation
- Real-time updates via SignalR
- Error handling scenarios

### 12.3 Example Test
```typescript
describe('IntegrationListComponent', () => {
  let component: IntegrationListComponent;
  let fixture: ComponentFixture<IntegrationListComponent>;
  let store: MockStore;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [IntegrationListComponent],
      imports: [MaterialModule, NoopAnimationsModule],
      providers: [
        provideMockStore({ initialState: mockIntegrationState })
      ]
    }).compileComponents();

    store = TestBed.inject(MockStore);
    fixture = TestBed.createComponent(IntegrationListComponent);
    component = fixture.componentInstance;
  });

  it('should load integrations on init', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    component.ngOnInit();
    expect(dispatchSpy).toHaveBeenCalledWith(loadIntegrations({}));
  });

  it('should filter integrations by type', () => {
    component.filterType = IntegrationType.PaymentGateway;
    component.applyFilters();
    expect(store.dispatch).toHaveBeenCalledWith(
      loadIntegrations({ filter: { type: IntegrationType.PaymentGateway } })
    );
  });
});
```

---

## 13. Security Considerations

### 13.1 Frontend Security
- API keys never stored in localStorage
- Sensitive data not logged to console in production
- HTTPS-only webhook URLs enforced (except localhost)
- XSS prevention via Angular sanitization
- CSRF tokens included in requests

### 13.2 Input Sanitization
```typescript
import { DomSanitizer } from '@angular/platform-browser';

constructor(private sanitizer: DomSanitizer) {}

sanitizeUrl(url: string): SafeUrl {
  return this.sanitizer.sanitize(SecurityContext.URL, url) || '';
}
```

---

## 14. Browser Support

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+
- Mobile browsers (iOS Safari 14+, Chrome Mobile 90+)

---

## 15. Deployment Considerations

### 15.1 Build Configuration
```json
{
  "production": {
    "optimization": true,
    "outputHashing": "all",
    "sourceMap": false,
    "extractCss": true,
    "namedChunks": false,
    "aot": true,
    "buildOptimizer": true,
    "budgets": [
      {
        "type": "initial",
        "maximumWarning": "2mb",
        "maximumError": "5mb"
      }
    ]
  }
}
```

### 15.2 Environment Configuration
```typescript
export const environment = {
  production: true,
  apiUrl: 'https://api.eventmanagement.com',
  signalRUrl: 'https://api.eventmanagement.com/hubs',
  azureAIEndpoint: 'https://eventmgmt-ai.cognitiveservices.azure.com'
};
```

---

## 16. Future Enhancements

- Drag-and-drop webhook event subscription
- Visual workflow builder for data transformations
- AI-powered integration health predictions
- Mobile app for integration monitoring
- Integration templates marketplace
- Advanced analytics dashboard
- Multi-language support
- Dark mode theme
