# Shipper & Logistics Management - Frontend Specification

## Document Information
- **Version**: 1.0
- **Last Updated**: 2025-12-22
- **Technology Stack**: Angular 18+, Angular Material, RxJS, NgRx
- **Design System**: Material Design 3

---

## Table of Contents
1. [Introduction](#1-introduction)
2. [Architecture](#2-architecture)
3. [User Interface Design](#3-user-interface-design)
4. [Component Specifications](#4-component-specifications)
5. [State Management](#5-state-management)
6. [Routing](#6-routing)
7. [Forms & Validation](#7-forms--validation)
8. [Real-Time Features](#8-real-time-features)
9. [Accessibility](#9-accessibility)
10. [Performance](#10-performance)
11. [Testing](#11-testing)
12. [Deployment](#12-deployment)

---

## 1. Introduction

### 1.1 Purpose
This document specifies the frontend requirements for the Shipper & Logistics Management module using Angular 18+ and Angular Material. The application provides an intuitive interface for managing shipper lists, tracking shipments, managing deliveries, and processing returns.

### 1.2 Scope
The frontend application includes:
- Shipper list creation and management interface
- Real-time shipment tracking dashboard
- Delivery management with signature capture
- Return item processing interface
- Driver assignment and route visualization
- Mobile-responsive design for field operations
- Progressive Web App (PWA) capabilities

### 1.3 User Roles
- **Event Manager**: Creates and manages shipper lists
- **Logistics Coordinator**: Assigns drivers, monitors shipments
- **Warehouse Personnel**: Packs and loads items
- **Driver**: Updates shipment status, manages deliveries
- **Venue Staff**: Receives deliveries, signs for items

---

## 2. Architecture

### 2.1 Project Structure

```
src/
├── app/
│   ├── core/
│   │   ├── services/
│   │   │   ├── api/
│   │   │   │   ├── shipper-list-api.service.ts
│   │   │   │   ├── shipment-api.service.ts
│   │   │   │   ├── delivery-api.service.ts
│   │   │   │   └── return-api.service.ts
│   │   │   ├── auth/
│   │   │   │   ├── auth.service.ts
│   │   │   │   └── auth.guard.ts
│   │   │   ├── signalr/
│   │   │   │   └── shipment-tracking-hub.service.ts
│   │   │   └── notification/
│   │   │       └── notification.service.ts
│   │   ├── interceptors/
│   │   │   ├── auth.interceptor.ts
│   │   │   ├── error.interceptor.ts
│   │   │   └── loading.interceptor.ts
│   │   ├── guards/
│   │   │   ├── role.guard.ts
│   │   │   └── unsaved-changes.guard.ts
│   │   └── models/
│   │       ├── shipper-list.model.ts
│   │       ├── shipment.model.ts
│   │       ├── delivery.model.ts
│   │       └── return-item.model.ts
│   ├── features/
│   │   ├── shipper-lists/
│   │   │   ├── components/
│   │   │   │   ├── shipper-list-overview/
│   │   │   │   ├── shipper-list-detail/
│   │   │   │   ├── shipper-list-form/
│   │   │   │   ├── shipper-list-items/
│   │   │   │   └── shipper-list-export/
│   │   │   ├── store/
│   │   │   │   ├── shipper-list.actions.ts
│   │   │   │   ├── shipper-list.reducer.ts
│   │   │   │   ├── shipper-list.effects.ts
│   │   │   │   └── shipper-list.selectors.ts
│   │   │   └── shipper-lists-routing.module.ts
│   │   ├── shipments/
│   │   │   ├── components/
│   │   │   │   ├── shipment-dashboard/
│   │   │   │   ├── shipment-tracking/
│   │   │   │   ├── shipment-map/
│   │   │   │   ├── shipment-timeline/
│   │   │   │   ├── driver-assignment/
│   │   │   │   └── shipment-details/
│   │   │   ├── store/
│   │   │   └── shipments-routing.module.ts
│   │   ├── deliveries/
│   │   │   ├── components/
│   │   │   │   ├── delivery-list/
│   │   │   │   ├── delivery-detail/
│   │   │   │   ├── signature-capture/
│   │   │   │   ├── delivery-exception/
│   │   │   │   └── delivery-reschedule/
│   │   │   ├── store/
│   │   │   └── deliveries-routing.module.ts
│   │   └── returns/
│   │       ├── components/
│   │       │   ├── return-overview/
│   │       │   ├── return-schedule/
│   │       │   ├── return-inspection/
│   │       │   └── damage-report/
│   │       ├── store/
│   │       └── returns-routing.module.ts
│   ├── shared/
│   │   ├── components/
│   │   │   ├── data-table/
│   │   │   ├── confirmation-dialog/
│   │   │   ├── status-badge/
│   │   │   ├── file-upload/
│   │   │   ├── signature-pad/
│   │   │   └── loading-spinner/
│   │   ├── directives/
│   │   │   ├── permission.directive.ts
│   │   │   └── auto-focus.directive.ts
│   │   ├── pipes/
│   │   │   ├── status-color.pipe.ts
│   │   │   ├── tracking-number.pipe.ts
│   │   │   └── relative-time.pipe.ts
│   │   └── validators/
│   │       ├── quantity.validator.ts
│   │       └── date-range.validator.ts
│   └── app.component.ts
├── assets/
│   ├── icons/
│   ├── images/
│   └── i18n/
├── environments/
│   ├── environment.ts
│   └── environment.prod.ts
└── styles/
    ├── _variables.scss
    ├── _theme.scss
    └── styles.scss
```

### 2.2 Technology Stack

#### 2.2.1 Core Framework
- **Angular 18+**: Latest stable version with signals
- **TypeScript 5.3+**: Type-safe development
- **RxJS 7+**: Reactive programming
- **Zone.js**: Change detection

#### 2.2.2 UI Framework
- **Angular Material 18+**: Component library
- **Material Design Icons**: Icon set
- **Angular Flex Layout**: Responsive layouts
- **Angular CDK**: Component dev kit

#### 2.2.3 State Management
- **NgRx Store**: State management
- **NgRx Effects**: Side effects
- **NgRx Entity**: Entity management
- **NgRx Router Store**: Router integration

#### 2.2.4 Real-Time Communication
- **@microsoft/signalr**: SignalR client
- **RxJS WebSocket**: Fallback communication

#### 2.2.5 Maps & Location
- **@azure/maps-control**: Azure Maps SDK
- **Leaflet**: Fallback mapping library

#### 2.2.6 Forms & Validation
- **Angular Reactive Forms**: Form handling
- **Custom Validators**: Business rule validation

#### 2.2.7 Testing
- **Jasmine**: Test framework
- **Karma**: Test runner
- **Cypress**: E2E testing
- **ng-mocks**: Component mocking

#### 2.2.8 Build & Development
- **Angular CLI**: Development tooling
- **Webpack**: Module bundling
- **ESLint**: Code linting
- **Prettier**: Code formatting

---

## 3. User Interface Design

### 3.1 Design Principles
- **Material Design 3**: Modern, clean aesthetics
- **Mobile-First**: Responsive across all devices
- **Accessibility**: WCAG 2.1 AA compliance
- **Progressive Enhancement**: Core functionality works everywhere
- **Performance**: Fast loading and interactions

### 3.2 Color Scheme

```scss
$primary: #1976d2;        // Blue - primary actions
$accent: #ff9800;          // Orange - secondary actions
$warn: #f44336;            // Red - warnings and errors
$success: #4caf50;         // Green - success states
$info: #2196f3;            // Light blue - informational

// Status Colors
$status-draft: #9e9e9e;
$status-in-progress: #ff9800;
$status-completed: #4caf50;
$status-cancelled: #f44336;
$status-exception: #ff5722;
```

### 3.3 Typography

```scss
$font-family: 'Roboto', 'Helvetica Neue', sans-serif;

// Headings
h1 { font-size: 2.5rem; font-weight: 300; }
h2 { font-size: 2rem; font-weight: 400; }
h3 { font-size: 1.75rem; font-weight: 400; }
h4 { font-size: 1.5rem; font-weight: 500; }
h5 { font-size: 1.25rem; font-weight: 500; }
h6 { font-size: 1rem; font-weight: 500; }

// Body
body { font-size: 1rem; line-height: 1.5; }
```

### 3.4 Layout Structure

#### 3.4.1 Main Layout
```
┌─────────────────────────────────────────────┐
│ Header (Toolbar)                            │
│ - Logo, Navigation, User Menu               │
├──────────┬──────────────────────────────────┤
│          │                                  │
│ Sidenav  │ Main Content Area                │
│          │                                  │
│ - Menu   │ - Page Title                     │
│ - Links  │ - Breadcrumbs                    │
│          │ - Content                        │
│          │                                  │
│          │                                  │
│          │                                  │
└──────────┴──────────────────────────────────┘
```

---

## 4. Component Specifications

### 4.1 Shipper List Components

#### 4.1.1 Shipper List Overview Component

**Purpose**: Display all shipper lists with filtering and search

**Template Structure**:
```html
<div class="shipper-list-overview">
  <mat-toolbar color="primary">
    <h2>Shipper Lists</h2>
    <span class="spacer"></span>
    <button mat-raised-button color="accent" (click)="createNewList()">
      <mat-icon>add</mat-icon>
      New Shipper List
    </button>
  </mat-toolbar>

  <mat-card>
    <mat-card-content>
      <!-- Filters -->
      <div class="filters">
        <mat-form-field>
          <mat-label>Search</mat-label>
          <input matInput [(ngModel)]="searchTerm" (input)="applyFilter()">
          <mat-icon matPrefix>search</mat-icon>
        </mat-form-field>

        <mat-form-field>
          <mat-label>Status</mat-label>
          <mat-select [(ngModel)]="statusFilter" (selectionChange)="applyFilter()">
            <mat-option value="">All</mat-option>
            <mat-option value="Draft">Draft</mat-option>
            <mat-option value="Finalized">Finalized</mat-option>
            <mat-option value="Shipped">Shipped</mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field>
          <mat-label>Event</mat-label>
          <mat-select [(ngModel)]="eventFilter" (selectionChange)="applyFilter()">
            <mat-option value="">All Events</mat-option>
            <mat-option *ngFor="let event of events$ | async" [value]="event.id">
              {{event.name}}
            </mat-option>
          </mat-select>
        </mat-form-field>
      </div>

      <!-- Data Table -->
      <table mat-table [dataSource]="dataSource" matSort>
        <ng-container matColumnDef="listNumber">
          <th mat-header-cell *matHeaderCellDef mat-sort-header>List Number</th>
          <td mat-cell *matCellDef="let list">{{list.listNumber}}</td>
        </ng-container>

        <ng-container matColumnDef="eventName">
          <th mat-header-cell *matHeaderCellDef mat-sort-header>Event</th>
          <td mat-cell *matCellDef="let list">{{list.eventName}}</td>
        </ng-container>

        <ng-container matColumnDef="status">
          <th mat-header-cell *matHeaderCellDef mat-sort-header>Status</th>
          <td mat-cell *matCellDef="let list">
            <app-status-badge [status]="list.status"></app-status-badge>
          </td>
        </ng-container>

        <ng-container matColumnDef="itemCount">
          <th mat-header-cell *matHeaderCellDef>Items</th>
          <td mat-cell *matCellDef="let list">{{list.itemCount}}</td>
        </ng-container>

        <ng-container matColumnDef="generatedDate">
          <th mat-header-cell *matHeaderCellDef mat-sort-header>Generated</th>
          <td mat-cell *matCellDef="let list">
            {{list.generatedDate | date:'short'}}
          </td>
        </ng-container>

        <ng-container matColumnDef="actions">
          <th mat-header-cell *matHeaderCellDef>Actions</th>
          <td mat-cell *matCellDef="let list">
            <button mat-icon-button [matMenuTriggerFor]="menu">
              <mat-icon>more_vert</mat-icon>
            </button>
            <mat-menu #menu="matMenu">
              <button mat-menu-item (click)="viewDetails(list.id)">
                <mat-icon>visibility</mat-icon>
                View Details
              </button>
              <button mat-menu-item (click)="editList(list.id)"
                      *ngIf="list.status === 'Draft'">
                <mat-icon>edit</mat-icon>
                Edit
              </button>
              <button mat-menu-item (click)="finalizeList(list.id)"
                      *ngIf="list.status === 'Draft'">
                <mat-icon>check_circle</mat-icon>
                Finalize
              </button>
              <button mat-menu-item (click)="exportList(list.id)">
                <mat-icon>download</mat-icon>
                Export
              </button>
              <button mat-menu-item (click)="printList(list.id)">
                <mat-icon>print</mat-icon>
                Print
              </button>
            </mat-menu>
          </td>
        </ng-container>

        <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
        <tr mat-row *matRowDef="let row; columns: displayedColumns;"
            (click)="viewDetails(row.id)" class="clickable-row"></tr>
      </table>

      <mat-paginator [pageSizeOptions]="[10, 25, 50, 100]"
                     showFirstLastButtons>
      </mat-paginator>
    </mat-card-content>
  </mat-card>
</div>
```

**Component TypeScript**:
```typescript
@Component({
  selector: 'app-shipper-list-overview',
  templateUrl: './shipper-list-overview.component.html',
  styleUrls: ['./shipper-list-overview.component.scss']
})
export class ShipperListOverviewComponent implements OnInit, OnDestroy {
  displayedColumns = ['listNumber', 'eventName', 'status', 'itemCount',
                      'generatedDate', 'actions'];
  dataSource: MatTableDataSource<ShipperList>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  searchTerm = '';
  statusFilter = '';
  eventFilter = '';

  shipperLists$ = this.store.select(selectAllShipperLists);
  events$ = this.store.select(selectAllEvents);
  loading$ = this.store.select(selectShipperListsLoading);

  private destroy$ = new Subject<void>();

  constructor(
    private store: Store,
    private router: Router,
    private dialog: MatDialog
  ) {
    this.dataSource = new MatTableDataSource<ShipperList>([]);
  }

  ngOnInit(): void {
    this.store.dispatch(ShipperListActions.loadShipperLists());

    this.shipperLists$
      .pipe(takeUntil(this.destroy$))
      .subscribe(lists => {
        this.dataSource.data = lists;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  applyFilter(): void {
    const filterValue = this.searchTerm.toLowerCase();
    this.dataSource.filter = filterValue;

    // Additional filtering logic for status and event
    this.dataSource.filterPredicate = (data: ShipperList, filter: string) => {
      const matchesSearch = data.listNumber.toLowerCase().includes(filter) ||
                           data.eventName.toLowerCase().includes(filter);
      const matchesStatus = !this.statusFilter || data.status === this.statusFilter;
      const matchesEvent = !this.eventFilter || data.eventId === this.eventFilter;

      return matchesSearch && matchesStatus && matchesEvent;
    };
  }

  createNewList(): void {
    this.router.navigate(['/shipper-lists/new']);
  }

  viewDetails(id: string): void {
    this.router.navigate(['/shipper-lists', id]);
  }

  editList(id: string): void {
    this.router.navigate(['/shipper-lists', id, 'edit']);
  }

  finalizeList(id: string): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Finalize Shipper List',
        message: 'Are you sure you want to finalize this shipper list? This action cannot be undone.',
        confirmText: 'Finalize',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.store.dispatch(ShipperListActions.finalizeShipperList({ id }));
      }
    });
  }

  exportList(id: string): void {
    this.store.dispatch(ShipperListActions.exportShipperList({ id, format: 'pdf' }));
  }

  printList(id: string): void {
    this.store.dispatch(ShipperListActions.printShipperList({ id }));
  }
}
```

#### 4.1.2 Shipper List Detail Component

**Purpose**: Display and manage individual shipper list details

**Key Features**:
- View all items in the shipper list
- Add/remove items
- Update item quantities
- Mark items as packed/loaded
- View packing progress
- Finalize list

**Component Structure**:
```typescript
@Component({
  selector: 'app-shipper-list-detail',
  templateUrl: './shipper-list-detail.component.html'
})
export class ShipperListDetailComponent implements OnInit {
  shipperList$ = this.store.select(selectCurrentShipperList);
  items$ = this.store.select(selectShipperListItems);
  packingProgress$ = this.store.select(selectPackingProgress);

  displayedColumns = ['itemName', 'itemCode', 'quantity', 'packed',
                      'loaded', 'actions'];

  constructor(
    private store: Store,
    private route: ActivatedRoute,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.store.dispatch(ShipperListActions.loadShipperListDetails({ id }));
  }

  addItem(): void {
    const dialogRef = this.dialog.open(AddItemDialogComponent, {
      width: '600px',
      data: { eventId: this.getCurrentEventId() }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.store.dispatch(ShipperListActions.addItem({
          shipperListId: this.getCurrentShipperListId(),
          item: result
        }));
      }
    });
  }

  markAsPacked(itemId: string): void {
    this.store.dispatch(ShipperListActions.markItemAsPacked({
      shipperListId: this.getCurrentShipperListId(),
      itemId
    }));
  }

  markAsLoaded(itemId: string): void {
    this.store.dispatch(ShipperListActions.markItemAsLoaded({
      shipperListId: this.getCurrentShipperListId(),
      itemId
    }));
  }
}
```

### 4.2 Shipment Components

#### 4.2.1 Shipment Dashboard Component

**Purpose**: Real-time overview of all active shipments

**Template Structure**:
```html
<div class="shipment-dashboard">
  <div class="dashboard-header">
    <h2>Shipment Dashboard</h2>
    <div class="stats-cards">
      <mat-card class="stat-card">
        <mat-card-content>
          <div class="stat-value">{{(statistics$ | async)?.total}}</div>
          <div class="stat-label">Total Shipments</div>
        </mat-card-content>
      </mat-card>

      <mat-card class="stat-card in-transit">
        <mat-card-content>
          <div class="stat-value">{{(statistics$ | async)?.inTransit}}</div>
          <div class="stat-label">In Transit</div>
        </mat-card-content>
      </mat-card>

      <mat-card class="stat-card delivered">
        <mat-card-content>
          <div class="stat-value">{{(statistics$ | async)?.delivered}}</div>
          <div class="stat-label">Delivered Today</div>
        </mat-card-content>
      </mat-card>

      <mat-card class="stat-card exceptions">
        <mat-card-content>
          <div class="stat-value">{{(statistics$ | async)?.exceptions}}</div>
          <div class="stat-label">Exceptions</div>
        </mat-card-content>
      </mat-card>
    </div>
  </div>

  <mat-tab-group>
    <mat-tab label="Active Shipments">
      <app-shipment-list
        [shipments]="activeShipments$ | async"
        [showMap]="true">
      </app-shipment-list>
    </mat-tab>

    <mat-tab label="Map View">
      <app-shipment-map
        [shipments]="activeShipments$ | async">
      </app-shipment-map>
    </mat-tab>

    <mat-tab label="Timeline">
      <app-shipment-timeline
        [events]="shipmentEvents$ | async">
      </app-shipment-timeline>
    </mat-tab>
  </mat-tab-group>
</div>
```

#### 4.2.2 Shipment Tracking Component

**Purpose**: Real-time tracking of individual shipment

**Key Features**:
- Live location updates via SignalR
- Interactive map with route
- Status timeline
- ETA calculations
- Driver information
- Item details

**Component TypeScript**:
```typescript
@Component({
  selector: 'app-shipment-tracking',
  templateUrl: './shipment-tracking.component.html'
})
export class ShipmentTrackingComponent implements OnInit, OnDestroy {
  shipment$ = this.store.select(selectCurrentShipment);
  trackingEvents$ = this.store.select(selectShipmentTrackingEvents);

  private hubConnection?: signalR.HubConnection;
  private destroy$ = new Subject<void>();

  constructor(
    private store: Store,
    private route: ActivatedRoute,
    private hubService: ShipmentTrackingHubService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.store.dispatch(ShipmentActions.loadShipmentTracking({ id }));

    // Subscribe to real-time updates
    this.hubService.startConnection();
    this.hubService.subscribeToShipment(id);

    this.hubService.locationUpdates$
      .pipe(takeUntil(this.destroy$))
      .subscribe(update => {
        this.store.dispatch(ShipmentActions.updateLocation({
          shipmentId: id,
          location: update
        }));
      });

    this.hubService.statusUpdates$
      .pipe(takeUntil(this.destroy$))
      .subscribe(update => {
        this.store.dispatch(ShipmentActions.updateStatus({
          shipmentId: id,
          status: update
        }));
      });
  }

  ngOnDestroy(): void {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.hubService.unsubscribeFromShipment(id);
    this.destroy$.next();
    this.destroy$.complete();
  }
}
```

#### 4.2.3 Shipment Map Component

**Purpose**: Display shipment locations on interactive map

**Component TypeScript**:
```typescript
@Component({
  selector: 'app-shipment-map',
  templateUrl: './shipment-map.component.html'
})
export class ShipmentMapComponent implements OnInit, AfterViewInit {
  @Input() shipments: Shipment[] = [];
  @ViewChild('mapContainer', { static: false }) mapContainer!: ElementRef;

  private map?: atlas.Map;
  private dataSource?: atlas.source.DataSource;

  ngAfterViewInit(): void {
    this.initializeMap();
  }

  private initializeMap(): void {
    this.map = new atlas.Map(this.mapContainer.nativeElement, {
      center: [-122.33, 47.6],
      zoom: 10,
      language: 'en-US',
      authOptions: {
        authType: atlas.AuthenticationType.subscriptionKey,
        subscriptionKey: environment.azureMapsKey
      }
    });

    this.map.events.add('ready', () => {
      this.dataSource = new atlas.source.DataSource();
      this.map!.sources.add(this.dataSource);

      // Add symbol layer for shipment locations
      this.map!.layers.add(new atlas.layer.SymbolLayer(this.dataSource, null, {
        iconOptions: {
          image: 'pin-blue',
          anchor: 'center',
          allowOverlap: true
        }
      }));

      this.updateMapMarkers();
    });
  }

  private updateMapMarkers(): void {
    if (!this.dataSource) return;

    const features = this.shipments
      .filter(s => s.currentLocation)
      .map(s => new atlas.data.Feature(
        new atlas.data.Point([
          s.currentLocation!.longitude,
          s.currentLocation!.latitude
        ]),
        {
          shipmentId: s.id,
          trackingNumber: s.trackingNumber,
          status: s.status
        }
      ));

    this.dataSource.clear();
    this.dataSource.add(features);
  }

  @HostListener('window:shipmentsUpdated')
  onShipmentsUpdated(): void {
    this.updateMapMarkers();
  }
}
```

### 4.3 Delivery Components

#### 4.3.1 Signature Capture Component

**Purpose**: Capture delivery signature on mobile/tablet

**Template Structure**:
```html
<div class="signature-capture">
  <mat-card>
    <mat-card-header>
      <mat-card-title>Delivery Signature</mat-card-title>
    </mat-card-header>

    <mat-card-content>
      <form [formGroup]="signatureForm">
        <mat-form-field>
          <mat-label>Recipient Name</mat-label>
          <input matInput formControlName="recipientName" required>
          <mat-error *ngIf="signatureForm.get('recipientName')?.hasError('required')">
            Recipient name is required
          </mat-error>
        </mat-form-field>

        <mat-form-field>
          <mat-label>Recipient Title</mat-label>
          <input matInput formControlName="recipientTitle" required>
        </mat-form-field>

        <div class="signature-pad-container">
          <canvas #signaturePad
                  width="600"
                  height="300"
                  (touchstart)="startDrawing($event)"
                  (touchmove)="draw($event)"
                  (touchend)="stopDrawing()"
                  (mousedown)="startDrawing($event)"
                  (mousemove)="draw($event)"
                  (mouseup)="stopDrawing()">
          </canvas>
        </div>

        <div class="actions">
          <button mat-button type="button" (click)="clearSignature()">
            <mat-icon>clear</mat-icon>
            Clear
          </button>
          <button mat-raised-button
                  color="primary"
                  (click)="saveSignature()"
                  [disabled]="!signatureForm.valid || !hasSignature">
            <mat-icon>save</mat-icon>
            Save Signature
          </button>
        </div>
      </form>
    </mat-card-content>
  </mat-card>
</div>
```

**Component TypeScript**:
```typescript
@Component({
  selector: 'app-signature-capture',
  templateUrl: './signature-capture.component.html',
  styleUrls: ['./signature-capture.component.scss']
})
export class SignatureCaptureComponent implements OnInit, AfterViewInit {
  @ViewChild('signaturePad', { static: false }) signaturePad!: ElementRef<HTMLCanvasElement>;
  @Input() deliveryId!: string;
  @Output() signatureSaved = new EventEmitter<string>();

  signatureForm: FormGroup;
  hasSignature = false;

  private ctx?: CanvasRenderingContext2D;
  private isDrawing = false;
  private lastX = 0;
  private lastY = 0;

  constructor(
    private fb: FormBuilder,
    private store: Store
  ) {
    this.signatureForm = this.fb.group({
      recipientName: ['', Validators.required],
      recipientTitle: ['', Validators.required]
    });
  }

  ngAfterViewInit(): void {
    this.ctx = this.signaturePad.nativeElement.getContext('2d')!;
    this.ctx.strokeStyle = '#000000';
    this.ctx.lineWidth = 2;
    this.ctx.lineCap = 'round';
  }

  startDrawing(event: MouseEvent | TouchEvent): void {
    this.isDrawing = true;
    const coords = this.getCoordinates(event);
    this.lastX = coords.x;
    this.lastY = coords.y;
  }

  draw(event: MouseEvent | TouchEvent): void {
    if (!this.isDrawing || !this.ctx) return;

    event.preventDefault();
    const coords = this.getCoordinates(event);

    this.ctx.beginPath();
    this.ctx.moveTo(this.lastX, this.lastY);
    this.ctx.lineTo(coords.x, coords.y);
    this.ctx.stroke();

    this.lastX = coords.x;
    this.lastY = coords.y;
    this.hasSignature = true;
  }

  stopDrawing(): void {
    this.isDrawing = false;
  }

  private getCoordinates(event: MouseEvent | TouchEvent): { x: number; y: number } {
    const canvas = this.signaturePad.nativeElement;
    const rect = canvas.getBoundingClientRect();

    if (event instanceof TouchEvent) {
      return {
        x: event.touches[0].clientX - rect.left,
        y: event.touches[0].clientY - rect.top
      };
    } else {
      return {
        x: event.clientX - rect.left,
        y: event.clientY - rect.top
      };
    }
  }

  clearSignature(): void {
    if (!this.ctx) return;
    const canvas = this.signaturePad.nativeElement;
    this.ctx.clearRect(0, 0, canvas.width, canvas.height);
    this.hasSignature = false;
  }

  saveSignature(): void {
    if (!this.signatureForm.valid || !this.hasSignature) return;

    const canvas = this.signaturePad.nativeElement;
    const signatureData = canvas.toDataURL('image/png');
    const base64Data = signatureData.split(',')[1];

    this.store.dispatch(DeliveryActions.captureSignature({
      deliveryId: this.deliveryId,
      signature: {
        base64Data,
        recipientName: this.signatureForm.value.recipientName,
        recipientTitle: this.signatureForm.value.recipientTitle,
        capturedAt: new Date().toISOString()
      }
    }));

    this.signatureSaved.emit(signatureData);
  }
}
```

### 4.4 Return Components

#### 4.4.1 Damage Report Component

**Purpose**: Document and photograph damaged items

**Template Structure**:
```html
<div class="damage-report">
  <mat-card>
    <mat-card-header>
      <mat-card-title>Damage Report</mat-card-title>
    </mat-card-header>

    <mat-card-content>
      <form [formGroup]="damageForm">
        <mat-form-field>
          <mat-label>Item</mat-label>
          <mat-select formControlName="itemId" required>
            <mat-option *ngFor="let item of returnItems$ | async" [value]="item.id">
              {{item.itemName}} - {{item.itemCode}}
            </mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field>
          <mat-label>Damage Description</mat-label>
          <textarea matInput
                    formControlName="description"
                    rows="4"
                    required>
          </textarea>
          <mat-hint>Describe the damage in detail</mat-hint>
        </mat-form-field>

        <div class="photo-upload">
          <h3>Damage Photos</h3>
          <app-file-upload
            [multiple]="true"
            [accept]="'image/*'"
            [maxFiles]="5"
            (filesSelected)="onPhotosSelected($event)">
          </app-file-upload>

          <div class="photo-preview" *ngIf="selectedPhotos.length > 0">
            <div *ngFor="let photo of selectedPhotos; let i = index" class="photo-item">
              <img [src]="photo.preview" alt="Damage photo {{i+1}}">
              <button mat-icon-button (click)="removePhoto(i)">
                <mat-icon>close</mat-icon>
              </button>
            </div>
          </div>
        </div>

        <div class="actions">
          <button mat-button (click)="cancel()">Cancel</button>
          <button mat-raised-button
                  color="warn"
                  (click)="submitReport()"
                  [disabled]="!damageForm.valid">
            Submit Damage Report
          </button>
        </div>
      </form>
    </mat-card-content>
  </mat-card>
</div>
```

**Component TypeScript**:
```typescript
@Component({
  selector: 'app-damage-report',
  templateUrl: './damage-report.component.html'
})
export class DamageReportComponent implements OnInit {
  damageForm: FormGroup;
  returnItems$ = this.store.select(selectReturnItems);
  selectedPhotos: { file: File; preview: string }[] = [];

  constructor(
    private fb: FormBuilder,
    private store: Store,
    private dialogRef: MatDialogRef<DamageReportComponent>
  ) {
    this.damageForm = this.fb.group({
      itemId: ['', Validators.required],
      description: ['', [Validators.required, Validators.minLength(10)]]
    });
  }

  onPhotosSelected(files: File[]): void {
    files.forEach(file => {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.selectedPhotos.push({
          file,
          preview: e.target.result
        });
      };
      reader.readAsDataURL(file);
    });
  }

  removePhoto(index: number): void {
    this.selectedPhotos.splice(index, 1);
  }

  submitReport(): void {
    if (!this.damageForm.valid) return;

    const formData = new FormData();
    formData.append('itemId', this.damageForm.value.itemId);
    formData.append('description', this.damageForm.value.description);

    this.selectedPhotos.forEach((photo, index) => {
      formData.append(`photos[${index}]`, photo.file);
    });

    this.store.dispatch(ReturnActions.submitDamageReport({ formData }));
    this.dialogRef.close(true);
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}
```

---

## 5. State Management

### 5.1 NgRx Store Structure

#### 5.1.1 Shipper List State
```typescript
export interface ShipperListState {
  entities: { [id: string]: ShipperList };
  ids: string[];
  selectedId: string | null;
  loading: boolean;
  error: string | null;
  filters: {
    search: string;
    status: string;
    eventId: string;
  };
}

export const initialState: ShipperListState = {
  entities: {},
  ids: [],
  selectedId: null,
  loading: false,
  error: null,
  filters: {
    search: '',
    status: '',
    eventId: ''
  }
};
```

#### 5.1.2 Actions
```typescript
export const ShipperListActions = createActionGroup({
  source: 'Shipper List',
  events: {
    'Load Shipper Lists': emptyProps(),
    'Load Shipper Lists Success': props<{ shipperLists: ShipperList[] }>(),
    'Load Shipper Lists Failure': props<{ error: string }>(),

    'Load Shipper List Details': props<{ id: string }>(),
    'Load Shipper List Details Success': props<{ shipperList: ShipperList }>(),

    'Create Shipper List': props<{ shipperList: CreateShipperListDto }>(),
    'Create Shipper List Success': props<{ shipperList: ShipperList }>(),
    'Create Shipper List Failure': props<{ error: string }>(),

    'Add Item': props<{ shipperListId: string; item: ShipperListItemDto }>(),
    'Add Item Success': props<{ item: ShipperListItem }>(),

    'Mark Item As Packed': props<{ shipperListId: string; itemId: string }>(),
    'Mark Item As Packed Success': props<{ itemId: string }>(),

    'Finalize Shipper List': props<{ id: string }>(),
    'Finalize Shipper List Success': props<{ id: string }>(),

    'Export Shipper List': props<{ id: string; format: string }>(),
    'Print Shipper List': props<{ id: string }>()
  }
});
```

#### 5.1.3 Reducer
```typescript
export const shipperListReducer = createReducer(
  initialState,

  on(ShipperListActions.loadShipperLists, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

  on(ShipperListActions.loadShipperListsSuccess, (state, { shipperLists }) =>
    adapter.setAll(shipperLists, {
      ...state,
      loading: false
    })
  ),

  on(ShipperListActions.loadShipperListsFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  on(ShipperListActions.createShipperListSuccess, (state, { shipperList }) =>
    adapter.addOne(shipperList, state)
  ),

  on(ShipperListActions.finalizeShipperListSuccess, (state, { id }) =>
    adapter.updateOne(
      { id, changes: { status: 'Finalized', finalizedDate: new Date() } },
      state
    )
  )
);
```

#### 5.1.4 Effects
```typescript
@Injectable()
export class ShipperListEffects {
  loadShipperLists$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ShipperListActions.loadShipperLists),
      switchMap(() =>
        this.shipperListApi.getAll().pipe(
          map(shipperLists =>
            ShipperListActions.loadShipperListsSuccess({ shipperLists })
          ),
          catchError(error =>
            of(ShipperListActions.loadShipperListsFailure({
              error: error.message
            }))
          )
        )
      )
    )
  );

  createShipperList$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ShipperListActions.createShipperList),
      switchMap(({ shipperList }) =>
        this.shipperListApi.create(shipperList).pipe(
          map(created =>
            ShipperListActions.createShipperListSuccess({
              shipperList: created
            })
          ),
          tap(() => {
            this.notification.success('Shipper list created successfully');
            this.router.navigate(['/shipper-lists']);
          }),
          catchError(error =>
            of(ShipperListActions.createShipperListFailure({
              error: error.message
            }))
          )
        )
      )
    )
  );

  finalizeShipperList$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ShipperListActions.finalizeShipperList),
      switchMap(({ id }) =>
        this.shipperListApi.finalize(id).pipe(
          map(() =>
            ShipperListActions.finalizeShipperListSuccess({ id })
          ),
          tap(() => {
            this.notification.success('Shipper list finalized');
          }),
          catchError(error => {
            this.notification.error('Failed to finalize shipper list');
            return EMPTY;
          })
        )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private shipperListApi: ShipperListApiService,
    private notification: NotificationService,
    private router: Router
  ) {}
}
```

#### 5.1.5 Selectors
```typescript
export const selectShipperListState =
  createFeatureSelector<ShipperListState>('shipperLists');

export const selectAllShipperLists = createSelector(
  selectShipperListState,
  (state) => Object.values(state.entities)
);

export const selectShipperListsLoading = createSelector(
  selectShipperListState,
  (state) => state.loading
);

export const selectCurrentShipperList = createSelector(
  selectShipperListState,
  (state) => state.selectedId ? state.entities[state.selectedId] : null
);

export const selectShipperListItems = createSelector(
  selectCurrentShipperList,
  (shipperList) => shipperList?.items || []
);

export const selectPackingProgress = createSelector(
  selectShipperListItems,
  (items) => {
    const total = items.length;
    const packed = items.filter(i => i.isPacked).length;
    const loaded = items.filter(i => i.isLoaded).length;

    return {
      total,
      packed,
      loaded,
      packedPercentage: total > 0 ? (packed / total) * 100 : 0,
      loadedPercentage: total > 0 ? (loaded / total) * 100 : 0
    };
  }
);
```

---

## 6. Routing

### 6.1 Route Configuration

```typescript
const routes: Routes = [
  {
    path: 'shipper-management',
    canActivate: [AuthGuard],
    children: [
      {
        path: 'shipper-lists',
        loadChildren: () => import('./features/shipper-lists/shipper-lists.module')
          .then(m => m.ShipperListsModule),
        data: { roles: ['EventManager', 'LogisticsCoordinator'] }
      },
      {
        path: 'shipments',
        loadChildren: () => import('./features/shipments/shipments.module')
          .then(m => m.ShipmentsModule),
        data: { roles: ['LogisticsCoordinator', 'Driver'] }
      },
      {
        path: 'deliveries',
        loadChildren: () => import('./features/deliveries/deliveries.module')
          .then(m => m.DeliveriesModule),
        data: { roles: ['Driver', 'DeliveryPersonnel'] }
      },
      {
        path: 'returns',
        loadChildren: () => import('./features/returns/returns.module')
          .then(m => m.ReturnsModule),
        data: { roles: ['WarehouseManager', 'LogisticsCoordinator'] }
      },
      {
        path: '',
        redirectTo: 'shipper-lists',
        pathMatch: 'full'
      }
    ]
  }
];

// Shipper Lists Routes
const shipperListRoutes: Routes = [
  {
    path: '',
    component: ShipperListOverviewComponent
  },
  {
    path: 'new',
    component: ShipperListFormComponent,
    canDeactivate: [UnsavedChangesGuard]
  },
  {
    path: ':id',
    component: ShipperListDetailComponent
  },
  {
    path: ':id/edit',
    component: ShipperListFormComponent,
    canDeactivate: [UnsavedChangesGuard]
  }
];
```

---

## 7. Forms & Validation

### 7.1 Reactive Forms

```typescript
// Create Shipper List Form
export class ShipperListFormComponent implements OnInit {
  shipperListForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.shipperListForm = this.fb.group({
      eventId: ['', Validators.required],
      notes: ['', Validators.maxLength(500)],
      items: this.fb.array([])
    });
  }

  get items(): FormArray {
    return this.shipperListForm.get('items') as FormArray;
  }

  addItem(): void {
    const itemGroup = this.fb.group({
      inventoryItemId: ['', Validators.required],
      quantity: ['', [Validators.required, Validators.min(1)]],
      notes: ['']
    });

    this.items.push(itemGroup);
  }

  removeItem(index: number): void {
    this.items.removeAt(index);
  }

  onSubmit(): void {
    if (this.shipperListForm.valid) {
      this.store.dispatch(ShipperListActions.createShipperList({
        shipperList: this.shipperListForm.value
      }));
    }
  }
}
```

### 7.2 Custom Validators

```typescript
export class CustomValidators {
  static futureDate(control: AbstractControl): ValidationErrors | null {
    if (!control.value) return null;

    const inputDate = new Date(control.value);
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    return inputDate >= today ? null : { pastDate: true };
  }

  static deliveryWindow(group: AbstractControl): ValidationErrors | null {
    const start = group.get('startTime')?.value;
    const end = group.get('endTime')?.value;

    if (!start || !end) return null;

    return new Date(start) < new Date(end) ? null : { invalidWindow: true };
  }

  static trackingNumber(control: AbstractControl): ValidationErrors | null {
    const pattern = /^SHP-\d{8}-[A-Z0-9]{8}$/;
    return pattern.test(control.value) ? null : { invalidTrackingNumber: true };
  }
}
```

---

## 8. Real-Time Features

### 8.1 SignalR Hub Service

```typescript
@Injectable({
  providedIn: 'root'
})
export class ShipmentTrackingHubService {
  private hubConnection?: signalR.HubConnection;
  private locationUpdatesSubject = new Subject<LocationUpdate>();
  private statusUpdatesSubject = new Subject<StatusUpdate>();

  locationUpdates$ = this.locationUpdatesSubject.asObservable();
  statusUpdates$ = this.statusUpdatesSubject.asObservable();

  constructor() {}

  async startConnection(): Promise<void> {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/hubs/shipment-tracking`, {
        accessTokenFactory: () => this.getAccessToken()
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on('ReceiveLocationUpdate', (update: LocationUpdate) => {
      this.locationUpdatesSubject.next(update);
    });

    this.hubConnection.on('ReceiveStatusUpdate', (update: StatusUpdate) => {
      this.statusUpdatesSubject.next(update);
    });

    try {
      await this.hubConnection.start();
      console.log('SignalR connection established');
    } catch (error) {
      console.error('Error starting SignalR connection:', error);
      setTimeout(() => this.startConnection(), 5000);
    }
  }

  async subscribeToShipment(shipmentId: string): Promise<void> {
    if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
      await this.hubConnection.invoke('SubscribeToShipment', shipmentId);
    }
  }

  async unsubscribeFromShipment(shipmentId: string): Promise<void> {
    if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
      await this.hubConnection.invoke('UnsubscribeFromShipment', shipmentId);
    }
  }

  private getAccessToken(): string {
    return localStorage.getItem('access_token') || '';
  }
}
```

---

## 9. Accessibility

### 9.1 ARIA Attributes

```html
<!-- Accessible Data Table -->
<table mat-table
       [dataSource]="dataSource"
       role="grid"
       aria-label="Shipper Lists">
  <ng-container matColumnDef="listNumber">
    <th mat-header-cell
        *matHeaderCellDef
        role="columnheader"
        scope="col">
      List Number
    </th>
    <td mat-cell
        *matCellDef="let list"
        role="gridcell">
      {{list.listNumber}}
    </td>
  </ng-container>
</table>

<!-- Accessible Form -->
<mat-form-field>
  <mat-label id="event-label">Event</mat-label>
  <mat-select formControlName="eventId"
              aria-labelledby="event-label"
              aria-required="true">
    <mat-option *ngFor="let event of events" [value]="event.id">
      {{event.name}}
    </mat-option>
  </mat-select>
  <mat-error role="alert">Event selection is required</mat-error>
</mat-form-field>
```

### 9.2 Keyboard Navigation

```typescript
@HostListener('keydown', ['$event'])
handleKeyboardEvent(event: KeyboardEvent): void {
  switch (event.key) {
    case 'ArrowUp':
      this.navigatePrevious();
      event.preventDefault();
      break;
    case 'ArrowDown':
      this.navigateNext();
      event.preventDefault();
      break;
    case 'Enter':
    case ' ':
      this.selectCurrent();
      event.preventDefault();
      break;
  }
}
```

---

## 10. Performance

### 10.1 Lazy Loading

```typescript
// Feature modules are lazy loaded
const routes: Routes = [
  {
    path: 'shipper-lists',
    loadChildren: () => import('./features/shipper-lists/shipper-lists.module')
      .then(m => m.ShipperListsModule)
  }
];
```

### 10.2 Virtual Scrolling

```html
<cdk-virtual-scroll-viewport itemSize="50" class="item-list">
  <div *cdkVirtualFor="let item of items" class="item">
    {{item.name}}
  </div>
</cdk-virtual-scroll-viewport>
```

### 10.3 Change Detection Optimization

```typescript
@Component({
  selector: 'app-shipment-list-item',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ShipmentListItemComponent {
  @Input() shipment!: Shipment;
}
```

### 10.4 TrackBy Functions

```typescript
trackByShipmentId(index: number, shipment: Shipment): string {
  return shipment.id;
}
```

```html
<div *ngFor="let shipment of shipments; trackBy: trackByShipmentId">
  {{shipment.trackingNumber}}
</div>
```

---

## 11. Testing

### 11.1 Unit Testing

```typescript
describe('ShipperListOverviewComponent', () => {
  let component: ShipperListOverviewComponent;
  let fixture: ComponentFixture<ShipperListOverviewComponent>;
  let store: MockStore;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ShipperListOverviewComponent],
      imports: [MaterialModule, NoopAnimationsModule],
      providers: [
        provideMockStore({
          initialState: {
            shipperLists: {
              entities: {},
              ids: [],
              loading: false
            }
          }
        })
      ]
    }).compileComponents();

    store = TestBed.inject(MockStore);
    fixture = TestBed.createComponent(ShipperListOverviewComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should dispatch loadShipperLists on init', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    component.ngOnInit();
    expect(dispatchSpy).toHaveBeenCalledWith(
      ShipperListActions.loadShipperLists()
    );
  });
});
```

### 11.2 Integration Testing

```typescript
describe('Shipment Tracking Integration', () => {
  it('should update shipment location in real-time', fakeAsync(() => {
    const hubService = TestBed.inject(ShipmentTrackingHubService);
    const store = TestBed.inject(MockStore);

    const locationUpdate = {
      shipmentId: 'test-id',
      latitude: 47.6062,
      longitude: -122.3321
    };

    let receivedUpdate: LocationUpdate | undefined;
    hubService.locationUpdates$.subscribe(update => {
      receivedUpdate = update;
    });

    hubService['locationUpdatesSubject'].next(locationUpdate);
    tick();

    expect(receivedUpdate).toEqual(locationUpdate);
  }));
});
```

### 11.3 E2E Testing

```typescript
describe('Shipper List Creation', () => {
  it('should create a new shipper list', () => {
    cy.visit('/shipper-lists');
    cy.get('[data-testid="new-shipper-list-btn"]').click();

    cy.get('[formControlName="eventId"]').click();
    cy.get('mat-option').first().click();

    cy.get('[data-testid="add-item-btn"]').click();
    cy.get('[formControlName="inventoryItemId"]').first().click();
    cy.get('mat-option').first().click();
    cy.get('[formControlName="quantity"]').first().type('50');

    cy.get('[data-testid="save-btn"]').click();

    cy.url().should('include', '/shipper-lists');
    cy.contains('Shipper list created successfully').should('be.visible');
  });
});
```

---

## 12. Deployment

### 12.1 Build Configuration

**angular.json**:
```json
{
  "projects": {
    "event-management": {
      "architect": {
        "build": {
          "configurations": {
            "production": {
              "optimization": true,
              "outputHashing": "all",
              "sourceMap": false,
              "namedChunks": false,
              "aot": true,
              "extractLicenses": true,
              "vendorChunk": false,
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
        }
      }
    }
  }
}
```

### 12.2 Environment Configuration

**environment.prod.ts**:
```typescript
export const environment = {
  production: true,
  apiUrl: 'https://api.eventmanagement.com',
  signalRHubUrl: 'https://api.eventmanagement.com/hubs',
  azureMapsKey: 'YOUR_AZURE_MAPS_KEY',
  appInsightsKey: 'YOUR_APP_INSIGHTS_KEY'
};
```

### 12.3 PWA Configuration

**manifest.webmanifest**:
```json
{
  "name": "Event Management - Shipper & Logistics",
  "short_name": "Shipper Mgmt",
  "theme_color": "#1976d2",
  "background_color": "#fafafa",
  "display": "standalone",
  "scope": "/",
  "start_url": "/shipper-management",
  "icons": [
    {
      "src": "assets/icons/icon-72x72.png",
      "sizes": "72x72",
      "type": "image/png"
    },
    {
      "src": "assets/icons/icon-192x192.png",
      "sizes": "192x192",
      "type": "image/png"
    },
    {
      "src": "assets/icons/icon-512x512.png",
      "sizes": "512x512",
      "type": "image/png"
    }
  ]
}
```

---

**End of Frontend Specification Document**
