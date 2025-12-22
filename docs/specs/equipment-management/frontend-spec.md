# Equipment Management - Frontend Specification

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
This specification defines the frontend requirements for the Equipment Management feature, providing users with a comprehensive interface for managing equipment items, reservations, logistics tracking, and maintenance.

### 1.2 Technology Stack
- **Framework**: Angular 18+ (latest stable)
- **UI Library**: Angular Material (Material 3)
- **State Management**: RxJS (no NgRx)
- **Styling**: SCSS with BEM methodology
- **Testing**: Jest (unit), Playwright (e2e)
- **File Upload**: Angular Material with drag-and-drop

### 1.3 Design Principles
- Mobile-first responsive design
- Material 3 design guidelines
- Accessibility (WCAG 2.1 AA compliance)
- Default Angular Material theme colors only
- Real-time availability updates

---

## 2. Page Structure

### 2.1 Pages
```
src/EventManagementPlatform.WebApp/projects/EventManagementPlatform/src/app/
├── pages/
│   ├── equipment/
│   │   ├── equipment-list/
│   │   │   ├── equipment-list.ts
│   │   │   ├── equipment-list.html
│   │   │   ├── equipment-list.scss
│   │   │   └── index.ts
│   │   ├── equipment-detail/
│   │   │   ├── equipment-detail.ts
│   │   │   ├── equipment-detail.html
│   │   │   ├── equipment-detail.scss
│   │   │   └── index.ts
│   │   ├── equipment-create/
│   │   │   ├── equipment-create.ts
│   │   │   ├── equipment-create.html
│   │   │   ├── equipment-create.scss
│   │   │   └── index.ts
│   │   ├── equipment-edit/
│   │   │   ├── equipment-edit.ts
│   │   │   ├── equipment-edit.html
│   │   │   ├── equipment-edit.scss
│   │   │   └── index.ts
│   │   ├── reservations-list/
│   │   │   ├── reservations-list.ts
│   │   │   ├── reservations-list.html
│   │   │   ├── reservations-list.scss
│   │   │   └── index.ts
│   │   ├── maintenance-schedule/
│   │   │   ├── maintenance-schedule.ts
│   │   │   ├── maintenance-schedule.html
│   │   │   ├── maintenance-schedule.scss
│   │   │   └── index.ts
│   │   ├── logistics-tracker/
│   │   │   ├── logistics-tracker.ts
│   │   │   ├── logistics-tracker.html
│   │   │   ├── logistics-tracker.scss
│   │   │   └── index.ts
│   │   └── index.ts
```

### 2.2 Components
```
├── components/
│   ├── equipment/
│   │   ├── equipment-card/
│   │   │   ├── equipment-card.ts
│   │   │   ├── equipment-card.html
│   │   │   ├── equipment-card.scss
│   │   │   └── index.ts
│   │   ├── equipment-status-badge/
│   │   │   ├── equipment-status-badge.ts
│   │   │   ├── equipment-status-badge.html
│   │   │   ├── equipment-status-badge.scss
│   │   │   └── index.ts
│   │   ├── equipment-condition-badge/
│   │   │   ├── equipment-condition-badge.ts
│   │   │   ├── equipment-condition-badge.html
│   │   │   ├── equipment-condition-badge.scss
│   │   │   └── index.ts
│   │   ├── equipment-form/
│   │   │   ├── equipment-form.ts
│   │   │   ├── equipment-form.html
│   │   │   ├── equipment-form.scss
│   │   │   └── index.ts
│   │   ├── equipment-photo-gallery/
│   │   │   ├── equipment-photo-gallery.ts
│   │   │   ├── equipment-photo-gallery.html
│   │   │   ├── equipment-photo-gallery.scss
│   │   │   └── index.ts
│   │   ├── photo-upload-dialog/
│   │   │   ├── photo-upload-dialog.ts
│   │   │   ├── photo-upload-dialog.html
│   │   │   ├── photo-upload-dialog.scss
│   │   │   └── index.ts
│   │   ├── specifications-editor/
│   │   │   ├── specifications-editor.ts
│   │   │   ├── specifications-editor.html
│   │   │   ├── specifications-editor.scss
│   │   │   └── index.ts
│   │   ├── reservation-form/
│   │   │   ├── reservation-form.ts
│   │   │   ├── reservation-form.html
│   │   │   ├── reservation-form.scss
│   │   │   └── index.ts
│   │   ├── availability-checker/
│   │   │   ├── availability-checker.ts
│   │   │   ├── availability-checker.html
│   │   │   ├── availability-checker.scss
│   │   │   └── index.ts
│   │   ├── reservation-calendar/
│   │   │   ├── reservation-calendar.ts
│   │   │   ├── reservation-calendar.html
│   │   │   ├── reservation-calendar.scss
│   │   │   └── index.ts
│   │   ├── logistics-timeline/
│   │   │   ├── logistics-timeline.ts
│   │   │   ├── logistics-timeline.html
│   │   │   ├── logistics-timeline.scss
│   │   │   └── index.ts
│   │   ├── maintenance-form/
│   │   │   ├── maintenance-form.ts
│   │   │   ├── maintenance-form.html
│   │   │   ├── maintenance-form.scss
│   │   │   └── index.ts
│   │   ├── damage-report-dialog/
│   │   │   ├── damage-report-dialog.ts
│   │   │   ├── damage-report-dialog.html
│   │   │   ├── damage-report-dialog.scss
│   │   │   └── index.ts
│   │   ├── equipment-filter-panel/
│   │   │   ├── equipment-filter-panel.ts
│   │   │   ├── equipment-filter-panel.html
│   │   │   ├── equipment-filter-panel.scss
│   │   │   └── index.ts
│   │   └── index.ts
```

---

## 3. User Interface Requirements

### 3.1 Equipment List Page

#### 3.1.1 Layout
- **Header**: Page title, search bar, create button, category tabs
- **Filter Panel**: Status, condition, category, availability filters
- **Content**: Responsive grid of equipment cards with photos
- **Pagination**: Bottom pagination with page size selector

#### 3.1.2 Features
| Feature | Description |
|---------|-------------|
| Search | Real-time search by name, manufacturer, model |
| Filter | Filter by status, condition, category |
| Category Tabs | Quick filter by equipment category |
| Sort | Sort by name, purchase date, condition, value |
| View Toggle | Grid view / Table view / List view |
| Availability Indicator | Visual indicator of current availability |

#### 3.1.3 Responsive Behavior
| Breakpoint | Layout |
|------------|--------|
| < 600px | Single column, stacked cards |
| 600-960px | 2-column grid |
| > 960px | 3-4 column grid or table view |

### 3.2 Equipment Detail Page

#### 3.2.1 Sections
| Section | Content |
|---------|---------|
| Header | Name, status badge, condition badge, action buttons |
| Photo Gallery | Primary photo with thumbnail carousel |
| Details Card | All equipment properties, specifications |
| Reservations Tab | Current and upcoming reservations calendar |
| Logistics Tab | Current logistics status if in transit |
| Maintenance Tab | Maintenance history and scheduled maintenance |
| Damage Reports Tab | Any reported damage with photos |

#### 3.2.2 Actions
| Action | Condition | UI Element |
|--------|-----------|------------|
| Edit | User is Warehouse Manager | Primary button |
| Upload Photo | User is Warehouse Manager | Icon button |
| Reserve | Status == Available, User is Staff | Primary button |
| Activate | Status == Inactive, User is Manager | Secondary button |
| Deactivate | Status == Active, User is Manager | Warning button |
| Schedule Maintenance | User is Maintenance Tech or Manager | Secondary button |
| Report Damage | User is Staff | Warning button |
| Retire | User is Manager | Warning button |

### 3.3 Equipment Create/Edit Page

#### 3.3.1 Form Fields
| Field | Type | Validation |
|-------|------|------------|
| Name | mat-input | Required, max 200 chars, unique per category |
| Description | mat-textarea | Optional, max 2000 chars |
| Category | mat-select | Required |
| Purchase Date | mat-datepicker | Required, not future |
| Purchase Price | mat-input (number) | Required, > 0 |
| Current Value | mat-input (number) | Optional, reasonable vs purchase |
| Manufacturer | mat-input | Optional, max 100 chars |
| Model | mat-input | Optional, max 100 chars |
| Serial Number | mat-input | Optional, max 50 chars |
| Warehouse Location | mat-input | Optional, max 50 chars |
| Condition | mat-select | Required |

#### 3.3.2 Form Behavior
- Real-time validation feedback
- Unsaved changes warning on navigation
- Inline specifications editor
- Photo upload with drag-and-drop

### 3.4 Reservations List Page

#### 3.4.1 Layout
- **Header**: Page title, create reservation button
- **Filter Panel**: Date range, status, equipment, event filters
- **Calendar View**: Visual calendar showing all reservations
- **List View**: Table of reservations with details
- **View Toggle**: Calendar / Table view

#### 3.4.2 Features
| Feature | Description |
|---------|-------------|
| Date Range Filter | Filter by reservation period |
| Status Filter | Filter by reservation status |
| Conflict Detection | Visual indicator for conflicts |
| Drag to Create | Drag on calendar to create reservation |
| Quick View | Hover/click for reservation details |

### 3.5 Create Reservation Flow

#### 3.5.1 Steps
1. **Select Equipment**: Search and select equipment
2. **Check Availability**: Visual calendar with availability
3. **Set Dates**: Select start and end dates/times
4. **Link Event**: Select or search for event
5. **Add Notes**: Optional special instructions
6. **Confirm**: Review and confirm reservation

#### 3.5.2 Availability Checker Component
- Visual calendar showing existing reservations
- Color coding for availability
- Conflict warnings
- Alternative suggestions if unavailable

### 3.6 Logistics Tracker Page

#### 3.6.1 Layout
- **Header**: Page title, filters
- **Timeline Cards**: Each reservation with logistics timeline
- **Status Indicators**: Visual progress indicators
- **Action Buttons**: Context-aware action buttons

#### 3.6.2 Logistics Timeline Component
```
Packing → Loading → Dispatch → Delivery → Setup → Pickup → Return
```

Each stage shows:
- Completion status (pending/completed)
- Timestamp when completed
- User who completed
- Quick action button for next stage

### 3.7 Maintenance Schedule Page

#### 3.7.1 Layout
- **Header**: Page title, schedule new button
- **Calendar View**: Maintenance calendar
- **List View**: Table of scheduled maintenance
- **Overdue Alerts**: Highlighted overdue maintenance

#### 3.7.2 Features
| Feature | Description |
|---------|-------------|
| Calendar View | Visual calendar of scheduled maintenance |
| Filtering | Filter by equipment, type, status |
| Overdue Alerts | Red badges for overdue maintenance |
| Quick Complete | Quick button to mark as complete |

---

## 4. Services

### 4.1 EquipmentService
```typescript
@Injectable({ providedIn: 'root' })
export class EquipmentService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getEquipment(params: EquipmentQueryParams): Observable<PagedResult<EquipmentListDto>> { }

    getEquipmentById(equipmentId: string): Observable<EquipmentDetailDto> { }

    createEquipment(equipment: CreateEquipmentDto): Observable<EquipmentDetailDto> { }

    updateEquipment(equipmentId: string, equipment: UpdateEquipmentDto): Observable<EquipmentDetailDto> { }

    deleteEquipment(equipmentId: string): Observable<void> { }

    activateEquipment(equipmentId: string): Observable<void> { }

    deactivateEquipment(equipmentId: string): Observable<void> { }

    uploadPhoto(equipmentId: string, file: File): Observable<EquipmentPhotoDto> { }

    deletePhoto(equipmentId: string, photoId: string): Observable<void> { }

    updateSpecifications(equipmentId: string, specs: SpecificationDto[]): Observable<void> { }

    retireEquipment(equipmentId: string, reason: string): Observable<void> { }
}
```

### 4.2 ReservationService
```typescript
@Injectable({ providedIn: 'root' })
export class ReservationService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getReservations(params: ReservationQueryParams): Observable<PagedResult<ReservationListDto>> { }

    getReservationById(reservationId: string): Observable<ReservationDetailDto> { }

    createReservation(reservation: CreateReservationDto): Observable<ReservationDetailDto> { }

    updateReservation(reservationId: string, reservation: UpdateReservationDto): Observable<ReservationDetailDto> { }

    cancelReservation(reservationId: string, reason: string): Observable<void> { }

    checkAvailability(equipmentId: string, startDate: Date, endDate: Date): Observable<AvailabilityResultDto> { }

    getAlternatives(equipmentId: string, startDate: Date, endDate: Date): Observable<AlternativeEquipmentDto[]> { }

    overrideConflict(reservationId: string, reason: string): Observable<void> { }
}
```

### 4.3 LogisticsService
```typescript
@Injectable({ providedIn: 'root' })
export class LogisticsService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getLogistics(reservationId: string): Observable<LogisticsDto> { }

    markAsPacked(reservationId: string): Observable<void> { }

    markAsLoaded(reservationId: string, truckId: string): Observable<void> { }

    markAsDispatched(reservationId: string, driverId: string): Observable<void> { }

    markAsDelivered(reservationId: string): Observable<void> { }

    markSetupComplete(reservationId: string): Observable<void> { }

    markAsPickedUp(reservationId: string): Observable<void> { }

    markAsReturned(reservationId: string): Observable<void> { }

    getInTransit(): Observable<LogisticsDto[]> { }
}
```

### 4.4 MaintenanceService
```typescript
@Injectable({ providedIn: 'root' })
export class MaintenanceService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getMaintenanceHistory(equipmentId: string): Observable<MaintenanceRecordDto[]> { }

    scheduleMaintenance(maintenance: ScheduleMaintenanceDto): Observable<MaintenanceRecordDto> { }

    updateMaintenance(maintenanceId: string, maintenance: UpdateMaintenanceDto): Observable<MaintenanceRecordDto> { }

    startMaintenance(maintenanceId: string, technicianId: string): Observable<void> { }

    completeMaintenance(maintenanceId: string, notes: string, cost: number): Observable<void> { }

    getScheduledMaintenance(params: MaintenanceQueryParams): Observable<PagedResult<MaintenanceRecordDto>> { }

    reportDamage(damage: ReportDamageDto): Observable<DamageReportDto> { }
}
```

### 4.5 EquipmentStateService
```typescript
@Injectable({ providedIn: 'root' })
export class EquipmentStateService {
    private readonly equipmentSubject = new BehaviorSubject<EquipmentListDto[]>([]);
    private readonly selectedEquipmentSubject = new BehaviorSubject<EquipmentDetailDto | null>(null);
    private readonly loadingSubject = new BehaviorSubject<boolean>(false);
    private readonly filterSubject = new BehaviorSubject<EquipmentFilter>({});

    readonly equipment$ = this.equipmentSubject.asObservable();
    readonly selectedEquipment$ = this.selectedEquipmentSubject.asObservable();
    readonly loading$ = this.loadingSubject.asObservable();
    readonly filter$ = this.filterSubject.asObservable();

    loadEquipment(params: EquipmentQueryParams): void { }

    selectEquipment(equipmentId: string): void { }

    clearSelection(): void { }

    refreshEquipment(): void { }

    setFilter(filter: EquipmentFilter): void { }
}
```

---

## 5. Models/Interfaces

### 5.1 DTOs
```typescript
export interface EquipmentListDto {
    equipmentItemId: string;
    name: string;
    category: EquipmentCategory;
    condition: EquipmentCondition;
    status: EquipmentStatus;
    manufacturer?: string;
    model?: string;
    primaryPhotoUrl?: string;
    isAvailable: boolean;
    nextReservation?: Date;
}

export interface EquipmentDetailDto {
    equipmentItemId: string;
    name: string;
    description?: string;
    category: EquipmentCategory;
    condition: EquipmentCondition;
    status: EquipmentStatus;
    purchaseDate: Date;
    purchasePrice: number;
    currentValue?: number;
    manufacturer?: string;
    model?: string;
    serialNumber?: string;
    warehouseLocation?: string;
    isActive: boolean;
    photos: EquipmentPhotoDto[];
    specifications: EquipmentSpecificationDto[];
    createdAt: Date;
    modifiedAt?: Date;
}

export interface CreateEquipmentDto {
    name: string;
    description?: string;
    category: EquipmentCategory;
    purchaseDate: Date;
    purchasePrice: number;
    manufacturer?: string;
    model?: string;
    serialNumber?: string;
    warehouseLocation?: string;
}

export interface ReservationListDto {
    reservationId: string;
    equipmentItemId: string;
    equipmentName: string;
    eventId: string;
    eventTitle: string;
    startDate: Date;
    endDate: Date;
    status: ReservationStatus;
    hasConflict: boolean;
}

export interface ReservationDetailDto {
    reservationId: string;
    equipmentItemId: string;
    equipmentName: string;
    eventId: string;
    eventTitle: string;
    quantity: number;
    startDate: Date;
    endDate: Date;
    status: ReservationStatus;
    notes?: string;
    createdAt: Date;
    createdByName: string;
    logistics?: LogisticsDto;
}

export interface AvailabilityResultDto {
    equipmentItemId: string;
    isAvailable: boolean;
    conflicts: ConflictingReservationDto[];
    alternatives: AlternativeEquipmentDto[];
}

export interface LogisticsDto {
    logisticsId: string;
    reservationId: string;
    packedAt?: Date;
    loadedAt?: Date;
    dispatchedAt?: Date;
    deliveredAt?: Date;
    setupCompletedAt?: Date;
    pickedUpAt?: Date;
    returnedAt?: Date;
    currentStage: LogisticsStage;
    notes?: string;
}

export interface MaintenanceRecordDto {
    maintenanceId: string;
    equipmentItemId: string;
    equipmentName: string;
    maintenanceType: MaintenanceType;
    scheduledDate: Date;
    startedAt?: Date;
    completedAt?: Date;
    status: MaintenanceStatus;
    description: string;
    cost?: number;
    technicianName?: string;
    notes?: string;
}

export interface DamageReportDto {
    damageReportId: string;
    equipmentItemId: string;
    equipmentName: string;
    eventId?: string;
    reportedAt: Date;
    reportedByName: string;
    severity: DamageSeverity;
    description: string;
    photoUrls: string[];
    repairRequired: boolean;
    estimatedRepairCost?: number;
    status: DamageStatus;
}
```

### 5.2 Enums
```typescript
export enum EquipmentCategory {
    Table = 'Table',
    Game = 'Game',
    AudioVisual = 'AudioVisual',
    Decoration = 'Decoration',
    Seating = 'Seating',
    Lighting = 'Lighting',
    Other = 'Other'
}

export enum EquipmentCondition {
    Excellent = 'Excellent',
    Good = 'Good',
    Fair = 'Fair',
    Poor = 'Poor',
    NeedsRepair = 'NeedsRepair',
    OutOfService = 'OutOfService'
}

export enum EquipmentStatus {
    Available = 'Available',
    Reserved = 'Reserved',
    InTransit = 'InTransit',
    AtVenue = 'AtVenue',
    InMaintenance = 'InMaintenance',
    Retired = 'Retired'
}

export enum ReservationStatus {
    Requested = 'Requested',
    Confirmed = 'Confirmed',
    Cancelled = 'Cancelled',
    Fulfilled = 'Fulfilled'
}

export enum LogisticsStage {
    NotStarted = 'NotStarted',
    Packed = 'Packed',
    Loaded = 'Loaded',
    Dispatched = 'Dispatched',
    Delivered = 'Delivered',
    SetupCompleted = 'SetupCompleted',
    PickedUp = 'PickedUp',
    Returned = 'Returned'
}

export enum MaintenanceType {
    Inspection = 'Inspection',
    PreventiveMaintenance = 'PreventiveMaintenance',
    Repair = 'Repair',
    Cleaning = 'Cleaning',
    Replacement = 'Replacement'
}

export enum MaintenanceStatus {
    Scheduled = 'Scheduled',
    InProgress = 'InProgress',
    Completed = 'Completed',
    Cancelled = 'Cancelled'
}

export enum DamageSeverity {
    Minor = 'Minor',
    Moderate = 'Moderate',
    Severe = 'Severe',
    TotalLoss = 'TotalLoss'
}

export enum DamageStatus {
    Reported = 'Reported',
    UnderReview = 'UnderReview',
    Approved = 'Approved',
    RepairScheduled = 'RepairScheduled',
    Repaired = 'Repaired',
    WriteOff = 'WriteOff'
}
```

---

## 6. Routing

### 6.1 Route Configuration
```typescript
export const equipmentRoutes: Routes = [
    {
        path: 'equipment',
        children: [
            {
                path: '',
                loadComponent: () => import('./pages/equipment/equipment-list').then(m => m.EquipmentList),
                title: 'Equipment'
            },
            {
                path: 'create',
                loadComponent: () => import('./pages/equipment/equipment-create').then(m => m.EquipmentCreate),
                title: 'Add Equipment',
                canActivate: [authGuard, roleGuard(['WarehouseManager', 'Admin'])]
            },
            {
                path: ':equipmentId',
                loadComponent: () => import('./pages/equipment/equipment-detail').then(m => m.EquipmentDetail),
                title: 'Equipment Details'
            },
            {
                path: ':equipmentId/edit',
                loadComponent: () => import('./pages/equipment/equipment-edit').then(m => m.EquipmentEdit),
                title: 'Edit Equipment',
                canActivate: [authGuard, roleGuard(['WarehouseManager', 'Admin'])],
                canDeactivate: [unsavedChangesGuard]
            },
            {
                path: 'reservations',
                loadComponent: () => import('./pages/equipment/reservations-list').then(m => m.ReservationsList),
                title: 'Equipment Reservations'
            },
            {
                path: 'logistics',
                loadComponent: () => import('./pages/equipment/logistics-tracker').then(m => m.LogisticsTracker),
                title: 'Logistics Tracker',
                canActivate: [authGuard, roleGuard(['Staff', 'WarehouseManager', 'Manager', 'Admin'])]
            },
            {
                path: 'maintenance',
                loadComponent: () => import('./pages/equipment/maintenance-schedule').then(m => m.MaintenanceSchedule),
                title: 'Maintenance Schedule',
                canActivate: [authGuard, roleGuard(['MaintenanceTech', 'Manager', 'Admin'])]
            }
        ]
    }
];
```

---

## 7. Material Components Used

### 7.1 Component List
| Component | Usage |
|-----------|-------|
| mat-card | Equipment cards, detail sections |
| mat-table | Equipment/reservation list table view |
| mat-paginator | Pagination |
| mat-sort | Table sorting |
| mat-form-field | Form inputs |
| mat-input | Text inputs |
| mat-select | Dropdowns (category, status, etc.) |
| mat-autocomplete | Equipment/Event search |
| mat-datepicker | Date selection |
| mat-button | Action buttons |
| mat-icon | Icons throughout |
| mat-menu | Actions dropdown |
| mat-dialog | Photo upload, damage report, confirmations |
| mat-snackbar | Notifications |
| mat-progress-spinner | Loading states |
| mat-chip | Status/condition badges |
| mat-tabs | Detail page sections |
| mat-expansion-panel | Filter panel |
| mat-stepper | Reservation creation wizard |
| mat-badge | Notification badges |
| mat-slide-toggle | Active/inactive toggle |
| mat-calendar | Reservation calendar |
| mat-grid-list | Photo gallery |

---

## 8. Styling Guidelines

### 8.1 BEM Naming Convention
```scss
// Equipment Card
.equipment-card { }
.equipment-card__header { }
.equipment-card__photo { }
.equipment-card__title { }
.equipment-card__info { }
.equipment-card__badges { }
.equipment-card__footer { }
.equipment-card__actions { }
.equipment-card--available { }
.equipment-card--reserved { }
.equipment-card--maintenance { }

// Logistics Timeline
.logistics-timeline { }
.logistics-timeline__stage { }
.logistics-timeline__stage-icon { }
.logistics-timeline__stage-label { }
.logistics-timeline__stage-timestamp { }
.logistics-timeline__connector { }
.logistics-timeline__stage--completed { }
.logistics-timeline__stage--current { }
.logistics-timeline__stage--pending { }
```

### 8.2 Design Tokens
```scss
// Spacing tokens
$spacing-xs: 4px;
$spacing-sm: 8px;
$spacing-md: 16px;
$spacing-lg: 24px;
$spacing-xl: 32px;
$spacing-xxl: 48px;

// Status colors (from Material theme)
// Use mat-palette colors only
```

### 8.3 Responsive Mixins
```scss
@mixin mobile {
    @media (max-width: 599px) { @content; }
}

@mixin tablet {
    @media (min-width: 600px) and (max-width: 959px) { @content; }
}

@mixin desktop {
    @media (min-width: 960px) { @content; }
}
```

---

## 9. Error Handling

### 9.1 Error Display
| Error Type | Display Method |
|------------|----------------|
| Validation | Inline form errors |
| API 400 | Snackbar with details |
| API 404 | Redirect to not found page |
| API 409 (Conflict) | Dialog with conflict details and alternatives |
| API 401 | Redirect to login |
| API 403 | Display forbidden message |
| API 500 | Error dialog with retry option |
| Network | Snackbar with retry action |
| File Upload | Progress bar with error handling |

### 9.2 Conflict Resolution Dialog
When double booking is detected:
1. Show dialog with conflict details
2. Display conflicting reservations
3. Offer alternatives if available
4. Allow override with reason (Manager only)

---

## 10. Accessibility Requirements

### 10.1 WCAG 2.1 AA Compliance
| Requirement | Implementation |
|-------------|----------------|
| Keyboard Navigation | All interactive elements focusable |
| Screen Reader | ARIA labels on all components |
| Color Contrast | Material theme ensures compliance |
| Focus Indicators | Visible focus rings |
| Form Labels | Associated labels for all inputs |
| Error Announcements | aria-live regions for errors |
| Status Badges | aria-label describing status |

### 10.2 ARIA Implementation
```html
<mat-card role="article" [attr.aria-label]="'Equipment: ' + equipment.name">
    <mat-card-header>
        <mat-card-title id="equipment-{{equipment.equipmentItemId}}-title">
            {{equipment.name}}
        </mat-card-title>
    </mat-card-header>
    <mat-card-content aria-describedby="equipment-{{equipment.equipmentItemId}}-title">
        <equipment-status-badge
            [status]="equipment.status"
            [attr.aria-label]="'Status: ' + equipment.status">
        </equipment-status-badge>
    </mat-card-content>
</mat-card>
```

---

## 11. Testing Requirements

### 11.1 Unit Tests (Jest)
| Component/Service | Test Coverage |
|-------------------|---------------|
| EquipmentService | 100% |
| ReservationService | 100% |
| LogisticsService | 100% |
| MaintenanceService | 100% |
| EquipmentStateService | 100% |
| EquipmentList | 90% |
| EquipmentDetail | 90% |
| ReservationForm | 100% (validation) |
| AvailabilityChecker | 100% |
| All components | Minimum 80% |

### 11.2 E2E Tests (Playwright)
| Scenario | Priority |
|----------|----------|
| Create equipment item | High |
| Upload equipment photo | High |
| Create reservation | High |
| Check availability | High |
| Handle double booking | High |
| Update logistics status | High |
| Schedule maintenance | Medium |
| Report damage | Medium |
| Filter equipment | Medium |
| Search equipment | Medium |

### 11.3 Test File Naming
```
equipment-list.spec.ts      // Unit test
equipment-list.e2e.ts       // E2E test
```

---

## 12. Performance Requirements

| Metric | Requirement |
|--------|-------------|
| First Contentful Paint | < 1.5s |
| Time to Interactive | < 3s |
| Bundle Size | < 250KB (initial) |
| Equipment List Render | < 100ms for 100 items |
| Photo Upload | < 5s for 10MB file |
| Availability Check | < 200ms |
| Calendar Render | < 150ms for 100 reservations |

### 12.1 Optimization Strategies
- Lazy loading of routes
- Virtual scrolling for large lists
- OnPush change detection
- Pagination (no infinite scroll)
- Image lazy loading and thumbnails
- Debounce search and availability checks

---

## 13. Internationalization

### 13.1 i18n Support
- Use Angular i18n for translations
- Date formatting via DatePipe with locale
- Currency formatting via CurrencyPipe
- Number formatting via DecimalPipe

### 13.2 Date/Time Handling
- Store all dates in UTC
- Display in user's local timezone
- Use Angular Material datepicker with locale
- Show timezone in logistics timeline

---

## 14. Special Features

### 14.1 Photo Upload with Drag and Drop
- Accept multiple files
- Show upload progress
- Preview before upload
- Set primary photo
- Maximum 10MB per file
- Supported formats: JPG, PNG, WEBP

### 14.2 Availability Calendar
- Visual month/week view
- Color coding:
  - Green: Available
  - Yellow: Partially available
  - Red: Fully booked
  - Gray: In maintenance
- Hover to see reservation details
- Click to create reservation

### 14.3 Logistics Timeline
- Horizontal stepper visualization
- Real-time status updates
- Context-aware action buttons
- Timestamp display for each stage
- Notes display at each stage

### 14.4 Maintenance Alerts
- Badge on equipment card for overdue maintenance
- Dashboard widget for upcoming maintenance
- Email notifications (backend trigger)
- Visual indicators for equipment needing inspection

---

## 15. Appendices

### 15.1 Related Documents
- [Backend Specification](./backend-spec.md)
- [Use Case Diagram](./diagrams/use-case.drawio)
- [C4 Diagrams](./diagrams/c4-context.drawio)
- [Class Diagram](./plantuml/class-diagram.plantuml)
- [Sequence Diagrams](./plantuml/sequence-diagram.plantuml)

### 15.2 Wireframes
Wireframes are available in the design system documentation.

### 15.3 Revision History
| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-22 | System | Initial specification |
