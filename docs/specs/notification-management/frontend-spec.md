# Notification & Alert Management - Frontend Specification

## 1. Introduction

### 1.1 Purpose
This document specifies the frontend requirements for the Notification & Alert Management feature using Angular 18+ and Angular Material. The interface provides users with real-time notifications, alert management, and preference configuration capabilities.

### 1.2 Scope
The frontend application provides:
- Real-time notification center with live updates
- Interactive notification list with filtering and sorting
- Alert dashboard with priority indicators
- User preference management interface
- Multi-channel notification support
- Emergency broadcast display
- Notification analytics and insights
- Responsive design for all devices

### 1.3 Technology Stack
- **Framework**: Angular 18+
- **UI Library**: Angular Material 18+
- **State Management**: NgRx (Store, Effects, Entity)
- **Real-time**: SignalR Client (@microsoft/signalr)
- **Forms**: Reactive Forms
- **HTTP**: HttpClient with interceptors
- **Routing**: Angular Router
- **Animation**: Angular Animations
- **Icons**: Material Icons
- **Notifications**: Angular Material Snackbar
- **Charts**: ng2-charts / Chart.js
- **Date/Time**: date-fns

## 2. Architecture

### 2.1 Module Structure

```
src/app/features/notifications/
├── components/
│   ├── notification-center/
│   │   ├── notification-center.component.ts
│   │   ├── notification-center.component.html
│   │   ├── notification-center.component.scss
│   │   └── notification-center.component.spec.ts
│   ├── notification-list/
│   │   ├── notification-list.component.ts
│   │   ├── notification-list.component.html
│   │   └── notification-list.component.scss
│   ├── notification-item/
│   │   ├── notification-item.component.ts
│   │   ├── notification-item.component.html
│   │   └── notification-item.component.scss
│   ├── alert-dashboard/
│   │   ├── alert-dashboard.component.ts
│   │   ├── alert-dashboard.component.html
│   │   └── alert-dashboard.component.scss
│   ├── alert-card/
│   │   ├── alert-card.component.ts
│   │   ├── alert-card.component.html
│   │   └── alert-card.component.scss
│   ├── notification-preferences/
│   │   ├── notification-preferences.component.ts
│   │   ├── notification-preferences.component.html
│   │   └── notification-preferences.component.scss
│   ├── emergency-broadcast-banner/
│   │   ├── emergency-broadcast-banner.component.ts
│   │   ├── emergency-broadcast-banner.component.html
│   │   └── emergency-broadcast-banner.component.scss
│   └── notification-badge/
│       ├── notification-badge.component.ts
│       ├── notification-badge.component.html
│       └── notification-badge.component.scss
├── services/
│   ├── notification.service.ts
│   ├── alert.service.ts
│   ├── notification-preferences.service.ts
│   ├── realtime-notification.service.ts
│   └── emergency-broadcast.service.ts
├── store/
│   ├── actions/
│   │   ├── notification.actions.ts
│   │   ├── alert.actions.ts
│   │   └── preference.actions.ts
│   ├── reducers/
│   │   ├── notification.reducer.ts
│   │   ├── alert.reducer.ts
│   │   ├── preference.reducer.ts
│   │   └── index.ts
│   ├── effects/
│   │   ├── notification.effects.ts
│   │   ├── alert.effects.ts
│   │   └── preference.effects.ts
│   ├── selectors/
│   │   ├── notification.selectors.ts
│   │   ├── alert.selectors.ts
│   │   └── preference.selectors.ts
│   └── models/
│       ├── notification.model.ts
│       ├── alert.model.ts
│       └── preference.model.ts
├── guards/
│   └── notification.guard.ts
├── pipes/
│   ├── notification-priority.pipe.ts
│   ├── time-ago.pipe.ts
│   └── notification-icon.pipe.ts
└── notifications.module.ts
```

## 3. Component Specifications

### 3.1 Notification Center Component

#### 3.1.1 Component Overview
The main notification center displays all user notifications with filtering, sorting, and action capabilities.

#### 3.1.2 TypeScript Implementation
```typescript
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NotificationActions } from '../../store/actions/notification.actions';
import { selectNotifications, selectNotificationSummary, selectNotificationLoading } from '../../store/selectors/notification.selectors';
import { Notification, NotificationFilter, NotificationSummary } from '../../store/models/notification.model';

@Component({
  selector: 'app-notification-center',
  templateUrl: './notification-center.component.html',
  styleUrls: ['./notification-center.component.scss']
})
export class NotificationCenterComponent implements OnInit, OnDestroy {
  notifications$: Observable<Notification[]>;
  summary$: Observable<NotificationSummary>;
  loading$: Observable<boolean>;

  private destroy$ = new Subject<void>();

  filterOptions = {
    priority: ['All', 'Low', 'Normal', 'High', 'Urgent', 'Critical'],
    category: ['All', 'System', 'Event', 'Payment', 'Alert', 'Reminder'],
    status: ['All', 'Unread', 'Read']
  };

  currentFilter: NotificationFilter = {
    status: 'Unread',
    pageNumber: 1,
    pageSize: 20
  };

  constructor(private store: Store) {
    this.notifications$ = this.store.select(selectNotifications);
    this.summary$ = this.store.select(selectNotificationSummary);
    this.loading$ = this.store.select(selectNotificationLoading);
  }

  ngOnInit(): void {
    this.loadNotifications();
    this.loadSummary();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadNotifications(): void {
    this.store.dispatch(NotificationActions.loadNotifications({ filter: this.currentFilter }));
  }

  loadSummary(): void {
    this.store.dispatch(NotificationActions.loadNotificationSummary());
  }

  onFilterChange(filter: NotificationFilter): void {
    this.currentFilter = { ...this.currentFilter, ...filter };
    this.loadNotifications();
  }

  onNotificationClick(notification: Notification): void {
    if (!notification.isRead) {
      this.store.dispatch(NotificationActions.markAsViewed({ notificationId: notification.id }));
    }
    this.handleNotificationAction(notification);
  }

  onDismiss(notificationId: string): void {
    this.store.dispatch(NotificationActions.dismissNotification({ notificationId }));
  }

  onMarkAllRead(): void {
    this.store.dispatch(NotificationActions.markAllAsRead({ category: this.currentFilter.category }));
  }

  onLoadMore(): void {
    this.currentFilter.pageNumber++;
    this.loadNotifications();
  }

  private handleNotificationAction(notification: Notification): void {
    // Navigate to relevant entity or perform action
    switch (notification.sourceEntityType) {
      case 'Event':
        // Navigate to event details
        break;
      case 'Payment':
        // Navigate to payment details
        break;
      // ... other cases
    }
  }
}
```

#### 3.1.3 Template
```html
<div class="notification-center">
  <!-- Header -->
  <div class="notification-header">
    <h2>Notifications</h2>
    <div class="header-actions">
      <button mat-icon-button [matMenuTriggerFor]="filterMenu">
        <mat-icon>filter_list</mat-icon>
      </button>
      <button mat-icon-button (click)="onMarkAllRead()">
        <mat-icon>done_all</mat-icon>
      </button>
      <button mat-icon-button (click)="loadNotifications()">
        <mat-icon>refresh</mat-icon>
      </button>
    </div>
  </div>

  <!-- Summary Cards -->
  <div class="notification-summary" *ngIf="summary$ | async as summary">
    <mat-card class="summary-card">
      <mat-card-content>
        <div class="summary-item">
          <span class="summary-label">Unread</span>
          <span class="summary-value">{{ summary.totalUnread }}</span>
        </div>
      </mat-card-content>
    </mat-card>

    <mat-card class="summary-card alert">
      <mat-card-content>
        <div class="summary-item">
          <span class="summary-label">Alerts</span>
          <span class="summary-value">{{ summary.recentAlerts }}</span>
        </div>
      </mat-card-content>
    </mat-card>

    <mat-card class="summary-card action">
      <mat-card-content>
        <div class="summary-item">
          <span class="summary-label">Action Required</span>
          <span class="summary-value">{{ summary.pendingActions }}</span>
        </div>
      </mat-card-content>
    </mat-card>
  </div>

  <!-- Filters -->
  <div class="notification-filters">
    <mat-chip-listbox [(ngModel)]="currentFilter.status" (change)="onFilterChange($event)">
      <mat-chip-option value="All">All</mat-chip-option>
      <mat-chip-option value="Unread">Unread</mat-chip-option>
      <mat-chip-option value="Read">Read</mat-chip-option>
    </mat-chip-listbox>
  </div>

  <!-- Notification List -->
  <div class="notification-content">
    <app-notification-list
      [notifications]="notifications$ | async"
      [loading]="loading$ | async"
      (notificationClick)="onNotificationClick($event)"
      (dismiss)="onDismiss($event)"
      (loadMore)="onLoadMore()">
    </app-notification-list>
  </div>

  <!-- Filter Menu -->
  <mat-menu #filterMenu="matMenu">
    <div class="filter-menu-content" (click)="$event.stopPropagation()">
      <h3>Filter Notifications</h3>

      <mat-form-field>
        <mat-label>Priority</mat-label>
        <mat-select [(ngModel)]="currentFilter.priority" (selectionChange)="onFilterChange(currentFilter)">
          <mat-option *ngFor="let priority of filterOptions.priority" [value]="priority">
            {{ priority }}
          </mat-option>
        </mat-select>
      </mat-form-field>

      <mat-form-field>
        <mat-label>Category</mat-label>
        <mat-select [(ngModel)]="currentFilter.category" (selectionChange)="onFilterChange(currentFilter)">
          <mat-option *ngFor="let category of filterOptions.category" [value]="category">
            {{ category }}
          </mat-option>
        </mat-select>
      </mat-form-field>
    </div>
  </mat-menu>
</div>
```

#### 3.1.4 Styles
```scss
.notification-center {
  height: 100%;
  display: flex;
  flex-direction: column;

  .notification-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 16px 24px;
    border-bottom: 1px solid #e0e0e0;

    h2 {
      margin: 0;
      font-size: 24px;
      font-weight: 500;
    }

    .header-actions {
      display: flex;
      gap: 8px;
    }
  }

  .notification-summary {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
    gap: 16px;
    padding: 16px 24px;

    .summary-card {
      &.alert {
        border-left: 4px solid #ff9800;
      }

      &.action {
        border-left: 4px solid #f44336;
      }

      .summary-item {
        display: flex;
        justify-content: space-between;
        align-items: center;

        .summary-label {
          font-size: 14px;
          color: #666;
        }

        .summary-value {
          font-size: 28px;
          font-weight: 600;
          color: #333;
        }
      }
    }
  }

  .notification-filters {
    padding: 16px 24px;
    border-bottom: 1px solid #e0e0e0;
  }

  .notification-content {
    flex: 1;
    overflow-y: auto;
  }
}

.filter-menu-content {
  padding: 16px;
  min-width: 250px;

  h3 {
    margin-top: 0;
    margin-bottom: 16px;
  }

  mat-form-field {
    width: 100%;
    margin-bottom: 8px;
  }
}
```

### 3.2 Notification Item Component

#### 3.2.1 Component Implementation
```typescript
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Notification, NotificationPriority } from '../../store/models/notification.model';

@Component({
  selector: 'app-notification-item',
  templateUrl: './notification-item.component.html',
  styleUrls: ['./notification-item.component.scss']
})
export class NotificationItemComponent {
  @Input() notification!: Notification;
  @Output() click = new EventEmitter<Notification>();
  @Output() dismiss = new EventEmitter<string>();
  @Output() actionClick = new EventEmitter<{ notificationId: string; action: string }>();

  get priorityColor(): string {
    const colors = {
      Low: '#4caf50',
      Normal: '#2196f3',
      High: '#ff9800',
      Urgent: '#ff5722',
      Critical: '#f44336'
    };
    return colors[this.notification.priority] || '#2196f3';
  }

  get priorityIcon(): string {
    const icons = {
      Low: 'info',
      Normal: 'notifications',
      High: 'warning',
      Urgent: 'priority_high',
      Critical: 'error'
    };
    return icons[this.notification.priority] || 'notifications';
  }

  onClick(): void {
    this.click.emit(this.notification);
  }

  onDismiss(event: Event): void {
    event.stopPropagation();
    this.dismiss.emit(this.notification.id);
  }

  onActionClick(action: string, event: Event): void {
    event.stopPropagation();
    this.actionClick.emit({ notificationId: this.notification.id, action });
  }
}
```

#### 3.2.2 Template
```html
<mat-card class="notification-item"
          [class.unread]="!notification.isRead"
          [class.dismissed]="notification.isDismissed"
          (click)="onClick()">
  <mat-card-content>
    <div class="notification-content">
      <!-- Priority Indicator -->
      <div class="priority-indicator" [style.background-color]="priorityColor">
        <mat-icon>{{ priorityIcon }}</mat-icon>
      </div>

      <!-- Main Content -->
      <div class="notification-body">
        <div class="notification-header-row">
          <h4 class="notification-title">{{ notification.title }}</h4>
          <span class="notification-time">{{ notification.createdAt | timeAgo }}</span>
        </div>

        <p class="notification-message">{{ notification.message }}</p>

        <!-- Category Badge -->
        <div class="notification-meta">
          <mat-chip class="category-chip">{{ notification.category }}</mat-chip>
          <mat-chip *ngIf="notification.priority === 'Urgent' || notification.priority === 'Critical'"
                    class="priority-chip"
                    [style.background-color]="priorityColor">
            {{ notification.priority }}
          </mat-chip>
        </div>

        <!-- Actions -->
        <div class="notification-actions" *ngIf="notification.actions?.length > 0">
          <button mat-stroked-button
                  *ngFor="let action of notification.actions"
                  (click)="onActionClick(action.type, $event)"
                  color="primary">
            {{ action.label }}
          </button>
        </div>
      </div>

      <!-- Dismiss Button -->
      <button mat-icon-button class="dismiss-button" (click)="onDismiss($event)">
        <mat-icon>close</mat-icon>
      </button>
    </div>
  </mat-card-content>
</mat-card>
```

#### 3.2.3 Styles
```scss
.notification-item {
  margin-bottom: 12px;
  cursor: pointer;
  transition: all 0.2s ease;
  position: relative;

  &:hover {
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.15);
    transform: translateY(-2px);
  }

  &.unread {
    background-color: #f5f5f5;
    border-left: 4px solid #2196f3;
  }

  &.dismissed {
    opacity: 0.6;
  }

  .notification-content {
    display: flex;
    gap: 16px;
    align-items: flex-start;

    .priority-indicator {
      width: 40px;
      height: 40px;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      flex-shrink: 0;

      mat-icon {
        color: white;
        font-size: 20px;
      }
    }

    .notification-body {
      flex: 1;
      min-width: 0;

      .notification-header-row {
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
        margin-bottom: 8px;

        .notification-title {
          margin: 0;
          font-size: 16px;
          font-weight: 500;
          color: #333;
        }

        .notification-time {
          font-size: 12px;
          color: #999;
          white-space: nowrap;
        }
      }

      .notification-message {
        margin: 0 0 12px 0;
        font-size: 14px;
        color: #666;
        line-height: 1.5;
      }

      .notification-meta {
        display: flex;
        gap: 8px;
        margin-bottom: 12px;

        .category-chip {
          height: 24px;
          font-size: 12px;
        }

        .priority-chip {
          height: 24px;
          font-size: 12px;
          color: white;
        }
      }

      .notification-actions {
        display: flex;
        gap: 8px;
        flex-wrap: wrap;

        button {
          height: 32px;
          line-height: 32px;
        }
      }
    }

    .dismiss-button {
      flex-shrink: 0;
    }
  }
}
```

### 3.3 Alert Dashboard Component

#### 3.3.1 Component Implementation
```typescript
import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { AlertActions } from '../../store/actions/alert.actions';
import { selectAlerts, selectAlertsByStatus, selectAlertLoading } from '../../store/selectors/alert.selectors';
import { Alert, AlertFilter, AlertStatus } from '../../store/models/alert.model';

@Component({
  selector: 'app-alert-dashboard',
  templateUrl: './alert-dashboard.component.html',
  styleUrls: ['./alert-dashboard.component.scss']
})
export class AlertDashboardComponent implements OnInit {
  alerts$: Observable<Alert[]>;
  activeAlerts$: Observable<Alert[]>;
  loading$: Observable<boolean>;

  displayedColumns: string[] = ['severity', 'type', 'title', 'status', 'createdAt', 'actions'];

  currentFilter: AlertFilter = {
    status: AlertStatus.Active,
    pageNumber: 1,
    pageSize: 20
  };

  severityColors = {
    Low: '#4caf50',
    Medium: '#ff9800',
    High: '#ff5722',
    Critical: '#f44336',
    Emergency: '#9c27b0'
  };

  constructor(private store: Store) {
    this.alerts$ = this.store.select(selectAlerts);
    this.activeAlerts$ = this.store.select(selectAlertsByStatus(AlertStatus.Active));
    this.loading$ = this.store.select(selectAlertLoading);
  }

  ngOnInit(): void {
    this.loadAlerts();
  }

  loadAlerts(): void {
    this.store.dispatch(AlertActions.loadAlerts({ filter: this.currentFilter }));
  }

  onAcknowledge(alertId: string): void {
    this.store.dispatch(AlertActions.acknowledgeAlert({ alertId }));
  }

  onResolve(alert: Alert): void {
    // Open resolve dialog
    this.store.dispatch(AlertActions.resolveAlert({
      alertId: alert.id,
      resolutionNotes: '',
      resolutionData: {}
    }));
  }

  onEscalate(alert: Alert): void {
    // Open escalation dialog
    this.store.dispatch(AlertActions.escalateAlert({
      alertId: alert.id,
      reason: '',
      escalateTo: []
    }));
  }

  onFilterChange(filter: AlertFilter): void {
    this.currentFilter = { ...this.currentFilter, ...filter };
    this.loadAlerts();
  }
}
```

#### 3.3.2 Template
```html
<div class="alert-dashboard">
  <!-- Header -->
  <div class="dashboard-header">
    <h2>Alert Dashboard</h2>
    <button mat-raised-button color="primary" (click)="loadAlerts()">
      <mat-icon>refresh</mat-icon>
      Refresh
    </button>
  </div>

  <!-- Alert Statistics -->
  <div class="alert-stats">
    <mat-card *ngFor="let severity of ['Low', 'Medium', 'High', 'Critical', 'Emergency']"
              class="stat-card"
              [style.border-left-color]="severityColors[severity]">
      <mat-card-content>
        <div class="stat-label">{{ severity }}</div>
        <div class="stat-value">0</div>
      </mat-card-content>
    </mat-card>
  </div>

  <!-- Alert Table -->
  <mat-card class="alert-table-card">
    <mat-card-content>
      <table mat-table [dataSource]="alerts$ | async" class="alert-table">
        <!-- Severity Column -->
        <ng-container matColumnDef="severity">
          <th mat-header-cell *matHeaderCellDef>Severity</th>
          <td mat-cell *matCellDef="let alert">
            <mat-chip [style.background-color]="severityColors[alert.severity]" class="severity-chip">
              {{ alert.severity }}
            </mat-chip>
          </td>
        </ng-container>

        <!-- Type Column -->
        <ng-container matColumnDef="type">
          <th mat-header-cell *matHeaderCellDef>Type</th>
          <td mat-cell *matCellDef="let alert">{{ alert.type }}</td>
        </ng-container>

        <!-- Title Column -->
        <ng-container matColumnDef="title">
          <th mat-header-cell *matHeaderCellDef>Title</th>
          <td mat-cell *matCellDef="let alert">
            <div class="alert-title">{{ alert.title }}</div>
            <div class="alert-description">{{ alert.description }}</div>
          </td>
        </ng-container>

        <!-- Status Column -->
        <ng-container matColumnDef="status">
          <th mat-header-cell *matHeaderCellDef>Status</th>
          <td mat-cell *matCellDef="let alert">
            <mat-chip>{{ alert.status }}</mat-chip>
          </td>
        </ng-container>

        <!-- Created At Column -->
        <ng-container matColumnDef="createdAt">
          <th mat-header-cell *matHeaderCellDef>Created</th>
          <td mat-cell *matCellDef="let alert">{{ alert.createdAt | timeAgo }}</td>
        </ng-container>

        <!-- Actions Column -->
        <ng-container matColumnDef="actions">
          <th mat-header-cell *matHeaderCellDef>Actions</th>
          <td mat-cell *matCellDef="let alert">
            <button mat-icon-button [matMenuTriggerFor]="alertMenu">
              <mat-icon>more_vert</mat-icon>
            </button>
            <mat-menu #alertMenu="matMenu">
              <button mat-menu-item (click)="onAcknowledge(alert.id)" *ngIf="alert.status === 'Active'">
                <mat-icon>check</mat-icon>
                Acknowledge
              </button>
              <button mat-menu-item (click)="onResolve(alert)">
                <mat-icon>done_all</mat-icon>
                Resolve
              </button>
              <button mat-menu-item (click)="onEscalate(alert)">
                <mat-icon>arrow_upward</mat-icon>
                Escalate
              </button>
            </mat-menu>
          </td>
        </ng-container>

        <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
        <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
      </table>

      <mat-paginator [pageSize]="20" [pageSizeOptions]="[10, 20, 50, 100]"></mat-paginator>
    </mat-card-content>
  </mat-card>
</div>
```

### 3.4 Notification Preferences Component

#### 3.4.1 Component Implementation
```typescript
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { PreferenceActions } from '../../store/actions/preference.actions';
import { selectNotificationPreferences } from '../../store/selectors/preference.selectors';
import { NotificationPreferences } from '../../store/models/preference.model';

@Component({
  selector: 'app-notification-preferences',
  templateUrl: './notification-preferences.component.html',
  styleUrls: ['./notification-preferences.component.scss']
})
export class NotificationPreferencesComponent implements OnInit {
  preferences$: Observable<NotificationPreferences>;
  preferencesForm: FormGroup;

  categories = [
    { value: 'system', label: 'System Notifications' },
    { value: 'events', label: 'Event Updates' },
    { value: 'payments', label: 'Payment Notifications' },
    { value: 'alerts', label: 'Alerts' },
    { value: 'reminders', label: 'Reminders' },
    { value: 'marketing', label: 'Marketing' }
  ];

  channels = [
    { value: 'email', label: 'Email', icon: 'email' },
    { value: 'sms', label: 'SMS', icon: 'sms' },
    { value: 'push', label: 'Push Notifications', icon: 'notifications' },
    { value: 'inApp', label: 'In-App', icon: 'app_settings_alt' }
  ];

  constructor(
    private fb: FormBuilder,
    private store: Store
  ) {
    this.preferences$ = this.store.select(selectNotificationPreferences);
    this.preferencesForm = this.createForm();
  }

  ngOnInit(): void {
    this.loadPreferences();
  }

  loadPreferences(): void {
    this.preferences$.pipe(take(1)).subscribe(preferences => {
      if (preferences) {
        this.patchFormValues(preferences);
      }
    });
  }

  createForm(): FormGroup {
    return this.fb.group({
      enableEmailNotifications: [true],
      enableSmsNotifications: [false],
      enablePushNotifications: [true],
      enableInAppNotifications: [true],
      categoryPreferences: this.fb.array(
        this.categories.map(cat => this.createCategoryPreference(cat.value))
      ),
      quietHours: this.fb.group({
        enabled: [false],
        startTime: ['22:00'],
        endTime: ['08:00'],
        allowUrgent: [true]
      }),
      digestSettings: this.fb.group({
        enabled: [false],
        frequency: ['Daily'],
        preferredTime: ['08:00']
      })
    });
  }

  createCategoryPreference(category: string): FormGroup {
    return this.fb.group({
      category: [category],
      email: [true],
      sms: [false],
      push: [true],
      inApp: [true],
      minimumPriority: ['Normal']
    });
  }

  get categoryPreferences(): FormArray {
    return this.preferencesForm.get('categoryPreferences') as FormArray;
  }

  patchFormValues(preferences: NotificationPreferences): void {
    this.preferencesForm.patchValue({
      enableEmailNotifications: preferences.enableEmailNotifications,
      enableSmsNotifications: preferences.enableSmsNotifications,
      enablePushNotifications: preferences.enablePushNotifications,
      enableInAppNotifications: preferences.enableInAppNotifications,
      quietHours: preferences.quietHours,
      digestSettings: preferences.digestSettings
    });
  }

  onSave(): void {
    if (this.preferencesForm.valid) {
      const preferences = this.preferencesForm.value;
      this.store.dispatch(PreferenceActions.updatePreferences({ preferences }));
    }
  }

  onReset(): void {
    this.loadPreferences();
  }
}
```

#### 3.4.2 Template
```html
<div class="notification-preferences">
  <mat-card>
    <mat-card-header>
      <mat-card-title>Notification Preferences</mat-card-title>
      <mat-card-subtitle>Manage how you receive notifications</mat-card-subtitle>
    </mat-card-header>

    <mat-card-content>
      <form [formGroup]="preferencesForm">
        <!-- Global Channel Settings -->
        <section class="preferences-section">
          <h3>Notification Channels</h3>
          <div class="channel-toggles">
            <mat-slide-toggle formControlName="enableEmailNotifications">
              <mat-icon>email</mat-icon>
              Email Notifications
            </mat-slide-toggle>
            <mat-slide-toggle formControlName="enableSmsNotifications">
              <mat-icon>sms</mat-icon>
              SMS Notifications
            </mat-slide-toggle>
            <mat-slide-toggle formControlName="enablePushNotifications">
              <mat-icon>notifications</mat-icon>
              Push Notifications
            </mat-slide-toggle>
            <mat-slide-toggle formControlName="enableInAppNotifications">
              <mat-icon>app_settings_alt</mat-icon>
              In-App Notifications
            </mat-slide-toggle>
          </div>
        </section>

        <mat-divider></mat-divider>

        <!-- Category Preferences -->
        <section class="preferences-section">
          <h3>Category Preferences</h3>
          <div formArrayName="categoryPreferences">
            <mat-expansion-panel *ngFor="let pref of categoryPreferences.controls; let i = index">
              <mat-expansion-panel-header>
                <mat-panel-title>
                  {{ categories[i].label }}
                </mat-panel-title>
              </mat-expansion-panel-header>

              <div [formGroupName]="i" class="category-preferences">
                <div class="channel-checkboxes">
                  <mat-checkbox formControlName="email">Email</mat-checkbox>
                  <mat-checkbox formControlName="sms">SMS</mat-checkbox>
                  <mat-checkbox formControlName="push">Push</mat-checkbox>
                  <mat-checkbox formControlName="inApp">In-App</mat-checkbox>
                </div>

                <mat-form-field>
                  <mat-label>Minimum Priority</mat-label>
                  <mat-select formControlName="minimumPriority">
                    <mat-option value="Low">Low</mat-option>
                    <mat-option value="Normal">Normal</mat-option>
                    <mat-option value="High">High</mat-option>
                    <mat-option value="Urgent">Urgent</mat-option>
                  </mat-select>
                </mat-form-field>
              </div>
            </mat-expansion-panel>
          </div>
        </section>

        <mat-divider></mat-divider>

        <!-- Quiet Hours -->
        <section class="preferences-section" formGroupName="quietHours">
          <h3>Quiet Hours</h3>
          <mat-slide-toggle formControlName="enabled">Enable Quiet Hours</mat-slide-toggle>

          <div class="quiet-hours-settings" *ngIf="preferencesForm.get('quietHours.enabled')?.value">
            <mat-form-field>
              <mat-label>Start Time</mat-label>
              <input matInput type="time" formControlName="startTime">
            </mat-form-field>

            <mat-form-field>
              <mat-label>End Time</mat-label>
              <input matInput type="time" formControlName="endTime">
            </mat-form-field>

            <mat-checkbox formControlName="allowUrgent">
              Allow urgent notifications during quiet hours
            </mat-checkbox>
          </div>
        </section>

        <mat-divider></mat-divider>

        <!-- Digest Settings -->
        <section class="preferences-section" formGroupName="digestSettings">
          <h3>Digest Notifications</h3>
          <mat-slide-toggle formControlName="enabled">Enable Digest</mat-slide-toggle>

          <div class="digest-settings" *ngIf="preferencesForm.get('digestSettings.enabled')?.value">
            <mat-form-field>
              <mat-label>Frequency</mat-label>
              <mat-select formControlName="frequency">
                <mat-option value="Daily">Daily</mat-option>
                <mat-option value="Weekly">Weekly</mat-option>
                <mat-option value="BiWeekly">Bi-Weekly</mat-option>
              </mat-select>
            </mat-form-field>

            <mat-form-field>
              <mat-label>Preferred Time</mat-label>
              <input matInput type="time" formControlName="preferredTime">
            </mat-form-field>
          </div>
        </section>
      </form>
    </mat-card-content>

    <mat-card-actions>
      <button mat-raised-button color="primary" (click)="onSave()">Save Preferences</button>
      <button mat-button (click)="onReset()">Reset</button>
    </mat-card-actions>
  </mat-card>
</div>
```

## 4. State Management (NgRx)

### 4.1 Actions

#### 4.1.1 Notification Actions
```typescript
import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { Notification, NotificationFilter, NotificationSummary } from '../models/notification.model';

export const NotificationActions = createActionGroup({
  source: 'Notification',
  events: {
    'Load Notifications': props<{ filter: NotificationFilter }>(),
    'Load Notifications Success': props<{ notifications: Notification[]; totalCount: number }>(),
    'Load Notifications Failure': props<{ error: string }>(),

    'Load Notification Summary': emptyProps(),
    'Load Notification Summary Success': props<{ summary: NotificationSummary }>(),
    'Load Notification Summary Failure': props<{ error: string }>(),

    'Mark As Viewed': props<{ notificationId: string }>(),
    'Mark As Viewed Success': props<{ notificationId: string }>(),
    'Mark As Viewed Failure': props<{ error: string }>(),

    'Dismiss Notification': props<{ notificationId: string }>(),
    'Dismiss Notification Success': props<{ notificationId: string }>(),
    'Dismiss Notification Failure': props<{ error: string }>(),

    'Mark All As Read': props<{ category?: string }>(),
    'Mark All As Read Success': props<{ updatedCount: number }>(),
    'Mark All As Read Failure': props<{ error: string }>(),

    'Realtime Notification Received': props<{ notification: Notification }>(),
    'Notification Updated': props<{ notification: Notification }>()
  }
});
```

### 4.2 Reducers

#### 4.2.1 Notification Reducer
```typescript
import { createReducer, on } from '@ngrx/store';
import { EntityState, EntityAdapter, createEntityAdapter } from '@ngrx/entity';
import { Notification } from '../models/notification.model';
import { NotificationActions } from '../actions/notification.actions';

export interface NotificationState extends EntityState<Notification> {
  loading: boolean;
  error: string | null;
  totalCount: number;
  summary: any;
}

export const adapter: EntityAdapter<Notification> = createEntityAdapter<Notification>();

export const initialState: NotificationState = adapter.getInitialState({
  loading: false,
  error: null,
  totalCount: 0,
  summary: null
});

export const notificationReducer = createReducer(
  initialState,
  on(NotificationActions.loadNotifications, (state) => ({
    ...state,
    loading: true,
    error: null
  })),
  on(NotificationActions.loadNotificationsSuccess, (state, { notifications, totalCount }) =>
    adapter.setAll(notifications, {
      ...state,
      loading: false,
      totalCount
    })
  ),
  on(NotificationActions.loadNotificationsFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),
  on(NotificationActions.markAsViewedSuccess, (state, { notificationId }) =>
    adapter.updateOne(
      { id: notificationId, changes: { isRead: true, viewedAt: new Date() } },
      state
    )
  ),
  on(NotificationActions.dismissNotificationSuccess, (state, { notificationId }) =>
    adapter.updateOne(
      { id: notificationId, changes: { isDismissed: true, dismissedAt: new Date() } },
      state
    )
  ),
  on(NotificationActions.realtimeNotificationReceived, (state, { notification }) =>
    adapter.addOne(notification, state)
  )
);

export const {
  selectIds,
  selectEntities,
  selectAll,
  selectTotal
} = adapter.getSelectors();
```

### 4.3 Effects

#### 4.3.1 Notification Effects
```typescript
import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { map, mergeMap, catchError, tap } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NotificationActions } from '../actions/notification.actions';
import { NotificationService } from '../../services/notification.service';

@Injectable()
export class NotificationEffects {
  loadNotifications$ = createEffect(() =>
    this.actions$.pipe(
      ofType(NotificationActions.loadNotifications),
      mergeMap(({ filter }) =>
        this.notificationService.getNotifications(filter).pipe(
          map(response => NotificationActions.loadNotificationsSuccess({
            notifications: response.items,
            totalCount: response.totalCount
          })),
          catchError(error => of(NotificationActions.loadNotificationsFailure({ error: error.message })))
        )
      )
    )
  );

  loadSummary$ = createEffect(() =>
    this.actions$.pipe(
      ofType(NotificationActions.loadNotificationSummary),
      mergeMap(() =>
        this.notificationService.getSummary().pipe(
          map(summary => NotificationActions.loadNotificationSummarySuccess({ summary })),
          catchError(error => of(NotificationActions.loadNotificationSummaryFailure({ error: error.message })))
        )
      )
    )
  );

  markAsViewed$ = createEffect(() =>
    this.actions$.pipe(
      ofType(NotificationActions.markAsViewed),
      mergeMap(({ notificationId }) =>
        this.notificationService.markAsViewed(notificationId).pipe(
          map(() => NotificationActions.markAsViewedSuccess({ notificationId })),
          catchError(error => of(NotificationActions.markAsViewedFailure({ error: error.message })))
        )
      )
    )
  );

  dismissNotification$ = createEffect(() =>
    this.actions$.pipe(
      ofType(NotificationActions.dismissNotification),
      mergeMap(({ notificationId }) =>
        this.notificationService.dismissNotification(notificationId).pipe(
          map(() => NotificationActions.dismissNotificationSuccess({ notificationId })),
          tap(() => this.snackBar.open('Notification dismissed', 'Close', { duration: 2000 })),
          catchError(error => of(NotificationActions.dismissNotificationFailure({ error: error.message })))
        )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private notificationService: NotificationService,
    private snackBar: MatSnackBar
  ) {}
}
```

### 4.4 Selectors

#### 4.4.1 Notification Selectors
```typescript
import { createFeatureSelector, createSelector } from '@ngrx/store';
import { NotificationState, selectAll } from '../reducers/notification.reducer';

export const selectNotificationState = createFeatureSelector<NotificationState>('notifications');

export const selectNotifications = createSelector(
  selectNotificationState,
  selectAll
);

export const selectNotificationLoading = createSelector(
  selectNotificationState,
  (state) => state.loading
);

export const selectNotificationError = createSelector(
  selectNotificationState,
  (state) => state.error
);

export const selectNotificationSummary = createSelector(
  selectNotificationState,
  (state) => state.summary
);

export const selectUnreadNotifications = createSelector(
  selectNotifications,
  (notifications) => notifications.filter(n => !n.isRead)
);

export const selectUnreadCount = createSelector(
  selectUnreadNotifications,
  (notifications) => notifications.length
);

export const selectNotificationsByCategory = (category: string) =>
  createSelector(
    selectNotifications,
    (notifications) => notifications.filter(n => n.category === category)
  );

export const selectNotificationsByPriority = (priority: string) =>
  createSelector(
    selectNotifications,
    (notifications) => notifications.filter(n => n.priority === priority)
  );
```

## 5. Services

### 5.1 Notification Service
```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Notification, NotificationFilter, NotificationSummary, PagedResult } from '../store/models/notification.model';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private apiUrl = `${environment.apiBaseUrl}/api/v1/notifications`;

  constructor(private http: HttpClient) {}

  getNotifications(filter: NotificationFilter): Observable<PagedResult<Notification>> {
    let params = new HttpParams();

    if (filter.status) params = params.append('status', filter.status);
    if (filter.priority) params = params.append('priority', filter.priority);
    if (filter.category) params = params.append('category', filter.category);
    if (filter.isRead !== undefined) params = params.append('isRead', filter.isRead.toString());
    params = params.append('pageNumber', filter.pageNumber.toString());
    params = params.append('pageSize', filter.pageSize.toString());

    return this.http.get<PagedResult<Notification>>(this.apiUrl, { params });
  }

  getNotificationById(id: string): Observable<Notification> {
    return this.http.get<Notification>(`${this.apiUrl}/${id}`);
  }

  getSummary(): Observable<NotificationSummary> {
    return this.http.get<NotificationSummary>(`${this.apiUrl}/summary`);
  }

  markAsViewed(notificationId: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${notificationId}/view`, {});
  }

  dismissNotification(notificationId: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${notificationId}/dismiss`, {});
  }

  markAllAsRead(category?: string): Observable<{ updatedCount: number }> {
    return this.http.post<{ updatedCount: number }>(`${this.apiUrl}/mark-all-read`, { category });
  }

  deleteNotification(notificationId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${notificationId}`);
  }
}
```

### 5.2 Realtime Notification Service
```typescript
import { Injectable, OnDestroy } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Store } from '@ngrx/store';
import { Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { NotificationActions } from '../store/actions/notification.actions';
import { Notification } from '../store/models/notification.model';

@Injectable({
  providedIn: 'root'
})
export class RealtimeNotificationService implements OnDestroy {
  private hubConnection: HubConnection;
  private destroy$ = new Subject<void>();

  constructor(private store: Store) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiBaseUrl}/hubs/notifications`)
      .withAutomaticReconnect()
      .build();

    this.registerHandlers();
  }

  async start(): Promise<void> {
    try {
      await this.hubConnection.start();
      console.log('SignalR connected');
      await this.subscribeToNotifications();
    } catch (error) {
      console.error('SignalR connection error:', error);
      setTimeout(() => this.start(), 5000);
    }
  }

  async stop(): Promise<void> {
    await this.hubConnection.stop();
  }

  private registerHandlers(): void {
    this.hubConnection.on('ReceiveNotification', (notification: Notification) => {
      this.store.dispatch(NotificationActions.realtimeNotificationReceived({ notification }));
      this.showDesktopNotification(notification);
    });

    this.hubConnection.on('NotificationUpdated', (notification: Notification) => {
      this.store.dispatch(NotificationActions.notificationUpdated({ notification }));
    });
  }

  private async subscribeToNotifications(): Promise<void> {
    await this.hubConnection.invoke('SubscribeToNotifications');
  }

  private showDesktopNotification(notification: Notification): void {
    if ('Notification' in window && Notification.permission === 'granted') {
      new Notification(notification.title, {
        body: notification.message,
        icon: '/assets/icons/notification-icon.png',
        tag: notification.id
      });
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.stop();
  }
}
```

## 6. Routing

### 6.1 Route Configuration
```typescript
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotificationCenterComponent } from './components/notification-center/notification-center.component';
import { AlertDashboardComponent } from './components/alert-dashboard/alert-dashboard.component';
import { NotificationPreferencesComponent } from './components/notification-preferences/notification-preferences.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: '', redirectTo: 'center', pathMatch: 'full' },
      { path: 'center', component: NotificationCenterComponent },
      { path: 'alerts', component: AlertDashboardComponent },
      { path: 'preferences', component: NotificationPreferencesComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NotificationsRoutingModule {}
```

## 7. Responsive Design

### 7.1 Breakpoints
```scss
// Mobile-first breakpoints
$breakpoint-xs: 0;
$breakpoint-sm: 600px;
$breakpoint-md: 960px;
$breakpoint-lg: 1280px;
$breakpoint-xl: 1920px;

// Responsive notification center
@media (max-width: $breakpoint-sm) {
  .notification-center {
    .notification-summary {
      grid-template-columns: 1fr;
    }

    .notification-item {
      .notification-content {
        flex-direction: column;
      }
    }
  }
}
```

## 8. Accessibility

### 8.1 ARIA Labels and Roles
```html
<div role="region" aria-label="Notification Center">
  <button aria-label="Mark all notifications as read" (click)="onMarkAllRead()">
    <mat-icon>done_all</mat-icon>
  </button>

  <div role="list" aria-label="Notifications">
    <div role="listitem" *ngFor="let notification of notifications" [attr.aria-label]="notification.title">
      <!-- Notification content -->
    </div>
  </div>
</div>
```

## 9. Performance Optimization

### 9.1 Virtual Scrolling
```typescript
import { ScrollingModule } from '@angular/cdk/scrolling';

// In template
<cdk-virtual-scroll-viewport itemSize="100" class="notification-viewport">
  <app-notification-item
    *cdkVirtualFor="let notification of notifications"
    [notification]="notification">
  </app-notification-item>
</cdk-virtual-scroll-viewport>
```

### 9.2 Change Detection Strategy
```typescript
@Component({
  selector: 'app-notification-item',
  templateUrl: './notification-item.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NotificationItemComponent {}
```

## 10. Testing

### 10.1 Component Tests
```typescript
describe('NotificationCenterComponent', () => {
  let component: NotificationCenterComponent;
  let fixture: ComponentFixture<NotificationCenterComponent>;
  let store: MockStore;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [NotificationCenterComponent],
      providers: [provideMockStore()],
      imports: [MaterialModule, NoopAnimationsModule]
    }).compileComponents();

    store = TestBed.inject(MockStore);
    fixture = TestBed.createComponent(NotificationCenterComponent);
    component = fixture.componentInstance;
  });

  it('should load notifications on init', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    component.ngOnInit();
    expect(dispatchSpy).toHaveBeenCalled();
  });
});
```

## 11. Error Handling

### 11.1 HTTP Interceptor
```typescript
@Injectable()
export class NotificationErrorInterceptor implements HttpInterceptor {
  constructor(private snackBar: MatSnackBar) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        this.snackBar.open(
          error.error?.message || 'An error occurred',
          'Close',
          { duration: 5000 }
        );
        return throwError(() => error);
      })
    );
  }
}
```

## 12. Build & Deployment

### 12.1 Production Build Configuration
```json
{
  "configurations": {
    "production": {
      "budgets": [
        {
          "type": "initial",
          "maximumWarning": "2mb",
          "maximumError": "5mb"
        }
      ],
      "optimization": true,
      "outputHashing": "all",
      "sourceMap": false,
      "namedChunks": false,
      "aot": true,
      "buildOptimizer": true
    }
  }
}
```
