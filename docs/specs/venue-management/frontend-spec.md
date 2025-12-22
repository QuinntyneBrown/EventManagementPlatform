# Venue Management - Frontend Specification

## 1. Introduction

### 1.1 Purpose
This Software Requirements Specification (SRS) document describes the frontend implementation for the Venue Management feature of the Event Management Platform using Angular 18+ and Angular Material.

### 1.2 Scope
The Venue Management frontend provides:
- Responsive web interface for venue management
- Interactive venue directory with search and filtering
- Detailed venue views with photo galleries
- Contact management interface
- Venue rating and feedback forms
- Issue reporting system
- Real-time updates and notifications
- Mobile-responsive design

### 1.3 Technology Stack
- **Framework**: Angular 18+ (Standalone Components)
- **UI Library**: Angular Material 18+
- **State Management**: NgRx (Store, Effects, Entity)
- **Forms**: Reactive Forms with validation
- **HTTP Client**: Angular HttpClient with interceptors
- **Routing**: Angular Router with lazy loading
- **Maps**: Google Maps / Leaflet for location display
- **Image Upload**: ng2-file-upload / ngx-dropzone
- **Date/Time**: date-fns / Luxon
- **Charts**: Chart.js / ngx-charts
- **Authentication**: MSAL (Microsoft Authentication Library)
- **Real-time**: SignalR for notifications
- **Testing**: Jasmine, Karma, Cypress

## 2. Application Architecture

### 2.1 Project Structure
```
venue-management/
├── components/
│   ├── venue-list/
│   ├── venue-detail/
│   ├── venue-form/
│   ├── venue-card/
│   ├── venue-search/
│   ├── venue-filters/
│   ├── venue-map/
│   ├── venue-photos/
│   ├── venue-contacts/
│   ├── venue-history/
│   ├── venue-rating/
│   ├── venue-feedback/
│   └── venue-issues/
├── services/
│   ├── venue.service.ts
│   ├── venue-api.service.ts
│   ├── venue-cache.service.ts
│   └── venue-photo.service.ts
├── store/
│   ├── venue.actions.ts
│   ├── venue.effects.ts
│   ├── venue.reducer.ts
│   ├── venue.selectors.ts
│   └── venue.state.ts
├── models/
│   ├── venue.model.ts
│   ├── venue-contact.model.ts
│   ├── venue-filter.model.ts
│   └── venue-history.model.ts
├── guards/
│   ├── venue-permission.guard.ts
│   └── unsaved-changes.guard.ts
├── validators/
│   ├── venue.validators.ts
│   └── contact.validators.ts
├── pipes/
│   ├── venue-status.pipe.ts
│   └── venue-rating.pipe.ts
└── venue-management-routing.module.ts
```

### 2.2 Module Organization
```typescript
// Feature Module (Lazy Loaded)
@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    VenueManagementRoutingModule,
    StoreModule.forFeature('venues', venueReducer),
    EffectsModule.forFeature([VenueEffects])
  ]
})
export class VenueManagementModule { }
```

## 3. Data Models

### 3.1 TypeScript Interfaces

```typescript
export interface Venue {
  venueId: string;
  name: string;
  description: string;
  status: VenueStatus;
  venueType: VenueType;
  address: VenueAddress;
  capacity: VenueCapacity;
  amenities: string[];
  contacts: VenueContact[];
  accessInstructions?: string;
  parkingInfo?: ParkingInfo;
  photos: VenuePhoto[];
  rating: VenueRating;
  isActive: boolean;
  createdAt: Date;
  createdBy: string;
  updatedAt?: Date;
  updatedBy?: string;
}

export interface VenueAddress {
  street1: string;
  street2?: string;
  city: string;
  state: string;
  country: string;
  postalCode: string;
  latitude?: number;
  longitude?: number;
  timeZone?: string;
}

export interface VenueCapacity {
  maxCapacity: number;
  seatedCapacity?: number;
  standingCapacity?: number;
  configurableLayouts?: LayoutCapacity[];
}

export interface LayoutCapacity {
  layoutType: string;
  capacity: number;
}

export interface VenueContact {
  contactId: string;
  contactType: ContactType;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  position?: string;
  notes?: string;
}

export interface ParkingInfo {
  hasParking: boolean;
  parkingCapacity?: number;
  parkingType?: ParkingType;
  parkingInstructions?: string;
}

export interface VenuePhoto {
  photoId: string;
  url: string;
  thumbnailUrl: string;
  caption?: string;
  isPrimary: boolean;
  uploadedAt: Date;
  uploadedBy: string;
}

export interface VenueRating {
  averageRating: number;
  totalRatings: number;
  ratingBreakdown: { [key: number]: number };
}

export interface VenueHistory {
  historyId: string;
  venueId: string;
  eventId: string;
  eventName: string;
  eventDate: Date;
  rating?: number;
  feedback?: string;
  issues?: string;
  createdAt: Date;
}

export interface VenueIssue {
  issueId: string;
  venueId: string;
  issueType: IssueType;
  description: string;
  severity: Severity;
  status: IssueStatus;
  reportedBy: string;
  reportedAt: Date;
  resolvedAt?: Date;
  resolution?: string;
}

export enum VenueStatus {
  Active = 'Active',
  Inactive = 'Inactive',
  Blacklisted = 'Blacklisted',
  PendingApproval = 'PendingApproval'
}

export enum VenueType {
  ConferenceCenter = 'ConferenceCenter',
  Hotel = 'Hotel',
  ConventionCenter = 'ConventionCenter',
  Outdoor = 'Outdoor',
  Restaurant = 'Restaurant',
  Theater = 'Theater',
  Stadium = 'Stadium',
  Other = 'Other'
}

export enum ContactType {
  Primary = 'Primary',
  Booking = 'Booking',
  Technical = 'Technical',
  Catering = 'Catering',
  Emergency = 'Emergency'
}

export enum ParkingType {
  Free = 'Free',
  Paid = 'Paid',
  Valet = 'Valet',
  Street = 'Street',
  None = 'None'
}

export enum IssueType {
  Facility = 'Facility',
  Equipment = 'Equipment',
  Service = 'Service',
  Safety = 'Safety',
  Cleanliness = 'Cleanliness',
  Other = 'Other'
}

export enum Severity {
  Low = 'Low',
  Medium = 'Medium',
  High = 'High',
  Critical = 'Critical'
}

export enum IssueStatus {
  Open = 'Open',
  InProgress = 'InProgress',
  Resolved = 'Resolved',
  Closed = 'Closed'
}

export interface VenueFilter {
  status?: VenueStatus;
  venueType?: VenueType;
  city?: string;
  country?: string;
  minCapacity?: number;
  maxCapacity?: number;
  amenities?: string[];
  searchTerm?: string;
  sortBy?: string;
  sortOrder?: 'asc' | 'desc';
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}
```

## 4. State Management (NgRx)

### 4.1 State Interface
```typescript
export interface VenueState {
  venues: EntityState<Venue>;
  selectedVenue: Venue | null;
  loading: boolean;
  error: string | null;
  filters: VenueFilter;
  pagination: {
    page: number;
    pageSize: number;
    totalCount: number;
  };
  searchResults: Venue[];
  topRated: Venue[];
}
```

### 4.2 Actions
```typescript
// Load Venues
export const loadVenues = createAction(
  '[Venue List] Load Venues',
  props<{ filter?: VenueFilter; page?: number; pageSize?: number }>()
);

export const loadVenuesSuccess = createAction(
  '[Venue API] Load Venues Success',
  props<{ result: PagedResult<Venue> }>()
);

export const loadVenuesFailure = createAction(
  '[Venue API] Load Venues Failure',
  props<{ error: string }>()
);

// Load Single Venue
export const loadVenue = createAction(
  '[Venue Detail] Load Venue',
  props<{ venueId: string }>()
);

export const loadVenueSuccess = createAction(
  '[Venue API] Load Venue Success',
  props<{ venue: Venue }>()
);

// Create Venue
export const createVenue = createAction(
  '[Venue Form] Create Venue',
  props<{ venue: Partial<Venue> }>()
);

export const createVenueSuccess = createAction(
  '[Venue API] Create Venue Success',
  props<{ venue: Venue }>()
);

// Update Venue
export const updateVenue = createAction(
  '[Venue Form] Update Venue',
  props<{ venueId: string; changes: Partial<Venue> }>()
);

export const updateVenueSuccess = createAction(
  '[Venue API] Update Venue Success',
  props<{ venue: Venue }>()
);

// Delete Venue
export const deleteVenue = createAction(
  '[Venue List] Delete Venue',
  props<{ venueId: string; reason?: string }>()
);

export const deleteVenueSuccess = createAction(
  '[Venue API] Delete Venue Success',
  props<{ venueId: string }>()
);

// Activate/Deactivate
export const activateVenue = createAction(
  '[Venue Detail] Activate Venue',
  props<{ venueId: string }>()
);

export const deactivateVenue = createAction(
  '[Venue Detail] Deactivate Venue',
  props<{ venueId: string; reason: string }>()
);

// Blacklist/Whitelist
export const blacklistVenue = createAction(
  '[Venue Detail] Blacklist Venue',
  props<{ venueId: string; reason: string }>()
);

export const whitelistVenue = createAction(
  '[Venue Detail] Whitelist Venue',
  props<{ venueId: string }>()
);

// Contact Management
export const addVenueContact = createAction(
  '[Venue Detail] Add Contact',
  props<{ venueId: string; contact: VenueContact }>()
);

export const updateVenueContact = createAction(
  '[Venue Detail] Update Contact',
  props<{ venueId: string; contactId: string; changes: Partial<VenueContact> }>()
);

export const removeVenueContact = createAction(
  '[Venue Detail] Remove Contact',
  props<{ venueId: string; contactId: string }>()
);

// Photo Management
export const uploadVenuePhoto = createAction(
  '[Venue Photos] Upload Photo',
  props<{ venueId: string; file: File; caption?: string; isPrimary?: boolean }>()
);

export const uploadVenuePhotoSuccess = createAction(
  '[Venue API] Upload Photo Success',
  props<{ venueId: string; photo: VenuePhoto }>()
);

export const deleteVenuePhoto = createAction(
  '[Venue Photos] Delete Photo',
  props<{ venueId: string; photoId: string }>()
);

// Search
export const searchVenues = createAction(
  '[Venue Search] Search Venues',
  props<{ query: string; filters?: VenueFilter }>()
);

export const searchVenuesSuccess = createAction(
  '[Venue API] Search Venues Success',
  props<{ venues: Venue[] }>()
);

// Filters
export const setVenueFilters = createAction(
  '[Venue Filters] Set Filters',
  props<{ filters: VenueFilter }>()
);

export const clearVenueFilters = createAction(
  '[Venue Filters] Clear Filters'
);
```

### 4.3 Selectors
```typescript
export const selectVenueState = createFeatureSelector<VenueState>('venues');

export const selectAllVenues = createSelector(
  selectVenueState,
  (state) => Object.values(state.venues.entities)
);

export const selectSelectedVenue = createSelector(
  selectVenueState,
  (state) => state.selectedVenue
);

export const selectVenueLoading = createSelector(
  selectVenueState,
  (state) => state.loading
);

export const selectVenueError = createSelector(
  selectVenueState,
  (state) => state.error
);

export const selectVenueFilters = createSelector(
  selectVenueState,
  (state) => state.filters
);

export const selectActiveVenues = createSelector(
  selectAllVenues,
  (venues) => venues.filter(v => v.status === VenueStatus.Active)
);

export const selectTopRatedVenues = createSelector(
  selectVenueState,
  (state) => state.topRated
);
```

## 5. Components

### 5.1 Venue List Component

#### Template
```html
<div class="venue-list-container">
  <!-- Header -->
  <mat-toolbar color="primary">
    <h1>Venue Management</h1>
    <span class="spacer"></span>
    <button mat-raised-button color="accent" (click)="createVenue()" *ngIf="canCreate">
      <mat-icon>add</mat-icon>
      Add Venue
    </button>
  </mat-toolbar>

  <!-- Search and Filters -->
  <div class="filters-section">
    <mat-form-field appearance="outline" class="search-field">
      <mat-label>Search venues</mat-label>
      <input matInput [formControl]="searchControl" placeholder="Search by name, city, or amenities">
      <mat-icon matPrefix>search</mat-icon>
    </mat-form-field>

    <button mat-button (click)="toggleFilters()">
      <mat-icon>filter_list</mat-icon>
      Filters
      <mat-icon>{{ showFilters ? 'expand_less' : 'expand_more' }}</mat-icon>
    </button>
  </div>

  <!-- Advanced Filters Panel -->
  <mat-expansion-panel [(expanded)]="showFilters">
    <app-venue-filters
      [filters]="filters$ | async"
      (filtersChange)="onFiltersChange($event)"
      (clearFilters)="onClearFilters()">
    </app-venue-filters>
  </mat-expansion-panel>

  <!-- Loading Indicator -->
  <mat-progress-bar mode="indeterminate" *ngIf="loading$ | async"></mat-progress-bar>

  <!-- Venues Grid -->
  <div class="venues-grid" *ngIf="venues$ | async as venues">
    <app-venue-card
      *ngFor="let venue of venues"
      [venue]="venue"
      (viewDetails)="viewVenueDetails($event)"
      (edit)="editVenue($event)"
      (delete)="deleteVenue($event)">
    </app-venue-card>

    <!-- Empty State -->
    <div class="empty-state" *ngIf="venues.length === 0">
      <mat-icon>business</mat-icon>
      <h2>No venues found</h2>
      <p>Try adjusting your filters or create a new venue</p>
    </div>
  </div>

  <!-- Pagination -->
  <mat-paginator
    [length]="totalCount$ | async"
    [pageSize]="20"
    [pageSizeOptions]="[10, 20, 50, 100]"
    (page)="onPageChange($event)">
  </mat-paginator>
</div>
```

#### Component Class
```typescript
@Component({
  selector: 'app-venue-list',
  standalone: true,
  imports: [CommonModule, MaterialModule, VenueCardComponent, VenueFiltersComponent],
  templateUrl: './venue-list.component.html',
  styleUrls: ['./venue-list.component.scss']
})
export class VenueListComponent implements OnInit {
  venues$ = this.store.select(selectAllVenues);
  loading$ = this.store.select(selectVenueLoading);
  totalCount$ = this.store.select(selectVenueState).pipe(
    map(state => state.pagination.totalCount)
  );
  filters$ = this.store.select(selectVenueFilters);

  searchControl = new FormControl('');
  showFilters = false;
  canCreate = false;

  constructor(
    private store: Store,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.loadVenues();
    this.setupSearch();
    this.checkPermissions();
  }

  loadVenues() {
    this.store.dispatch(loadVenues({ page: 1, pageSize: 20 }));
  }

  setupSearch() {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(searchTerm => {
        if (searchTerm) {
          this.store.dispatch(searchVenues({ query: searchTerm }));
        } else {
          this.loadVenues();
        }
      });
  }

  onFiltersChange(filters: VenueFilter) {
    this.store.dispatch(setVenueFilters({ filters }));
    this.store.dispatch(loadVenues({ filter: filters }));
  }

  onClearFilters() {
    this.store.dispatch(clearVenueFilters());
    this.loadVenues();
  }

  onPageChange(event: PageEvent) {
    this.store.dispatch(loadVenues({
      page: event.pageIndex + 1,
      pageSize: event.pageSize
    }));
  }

  viewVenueDetails(venueId: string) {
    this.router.navigate(['/venues', venueId]);
  }

  createVenue() {
    this.router.navigate(['/venues', 'new']);
  }

  editVenue(venueId: string) {
    this.router.navigate(['/venues', venueId, 'edit']);
  }

  deleteVenue(venueId: string) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete Venue',
        message: 'Are you sure you want to delete this venue?',
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.store.dispatch(deleteVenue({ venueId }));
      }
    });
  }

  toggleFilters() {
    this.showFilters = !this.showFilters;
  }

  private checkPermissions() {
    this.canCreate = this.authService.hasPermission('venues.create');
  }
}
```

### 5.2 Venue Detail Component

#### Template
```html
<div class="venue-detail-container" *ngIf="venue$ | async as venue">
  <!-- Header -->
  <div class="detail-header">
    <button mat-icon-button (click)="goBack()">
      <mat-icon>arrow_back</mat-icon>
    </button>
    <h1>{{ venue.name }}</h1>
    <span class="spacer"></span>

    <!-- Status Badge -->
    <mat-chip [color]="getStatusColor(venue.status)">
      {{ venue.status | venueStatus }}
    </mat-chip>

    <!-- Actions Menu -->
    <button mat-icon-button [matMenuTriggerFor]="actionsMenu">
      <mat-icon>more_vert</mat-icon>
    </button>
    <mat-menu #actionsMenu="matMenu">
      <button mat-menu-item (click)="editVenue()" *ngIf="canEdit">
        <mat-icon>edit</mat-icon>
        Edit
      </button>
      <button mat-menu-item (click)="activateVenue()" *ngIf="venue.status === 'Inactive' && canActivate">
        <mat-icon>check_circle</mat-icon>
        Activate
      </button>
      <button mat-menu-item (click)="deactivateVenue()" *ngIf="venue.status === 'Active' && canDeactivate">
        <mat-icon>cancel</mat-icon>
        Deactivate
      </button>
      <button mat-menu-item (click)="blacklistVenue()" *ngIf="venue.status !== 'Blacklisted' && canBlacklist">
        <mat-icon>block</mat-icon>
        Blacklist
      </button>
      <button mat-menu-item (click)="whitelistVenue()" *ngIf="venue.status === 'Blacklisted' && canWhitelist">
        <mat-icon>check</mat-icon>
        Whitelist
      </button>
      <mat-divider></mat-divider>
      <button mat-menu-item (click)="deleteVenue()" *ngIf="canDelete" class="delete-action">
        <mat-icon>delete</mat-icon>
        Delete
      </button>
    </mat-menu>
  </div>

  <!-- Tabs -->
  <mat-tab-group>
    <!-- Overview Tab -->
    <mat-tab label="Overview">
      <div class="tab-content">
        <!-- Photo Gallery -->
        <app-venue-photos
          [venueId]="venue.venueId"
          [photos]="venue.photos"
          [canUpload]="canUploadPhotos">
        </app-venue-photos>

        <!-- Basic Info -->
        <mat-card>
          <mat-card-header>
            <mat-card-title>Basic Information</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <div class="info-grid">
              <div class="info-item">
                <span class="label">Venue Type:</span>
                <span class="value">{{ venue.venueType }}</span>
              </div>
              <div class="info-item">
                <span class="label">Max Capacity:</span>
                <span class="value">{{ venue.capacity.maxCapacity }}</span>
              </div>
              <div class="info-item">
                <span class="label">Rating:</span>
                <span class="value">
                  <app-venue-rating [rating]="venue.rating"></app-venue-rating>
                </span>
              </div>
            </div>
            <p class="description">{{ venue.description }}</p>
          </mat-card-content>
        </mat-card>

        <!-- Location -->
        <mat-card>
          <mat-card-header>
            <mat-card-title>Location</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <div class="address">
              <p>{{ venue.address.street1 }}</p>
              <p *ngIf="venue.address.street2">{{ venue.address.street2 }}</p>
              <p>{{ venue.address.city }}, {{ venue.address.state }} {{ venue.address.postalCode }}</p>
              <p>{{ venue.address.country }}</p>
            </div>
            <app-venue-map
              *ngIf="venue.address.latitude && venue.address.longitude"
              [latitude]="venue.address.latitude"
              [longitude]="venue.address.longitude"
              [venueName]="venue.name">
            </app-venue-map>
          </mat-card-content>
        </mat-card>

        <!-- Amenities -->
        <mat-card>
          <mat-card-header>
            <mat-card-title>Amenities</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <mat-chip-listbox>
              <mat-chip *ngFor="let amenity of venue.amenities">{{ amenity }}</mat-chip>
            </mat-chip-listbox>
          </mat-card-content>
        </mat-card>

        <!-- Parking Info -->
        <mat-card *ngIf="venue.parkingInfo?.hasParking">
          <mat-card-header>
            <mat-card-title>Parking Information</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <div class="info-grid">
              <div class="info-item">
                <span class="label">Type:</span>
                <span class="value">{{ venue.parkingInfo.parkingType }}</span>
              </div>
              <div class="info-item" *ngIf="venue.parkingInfo.parkingCapacity">
                <span class="label">Capacity:</span>
                <span class="value">{{ venue.parkingInfo.parkingCapacity }} spaces</span>
              </div>
            </div>
            <p *ngIf="venue.parkingInfo.parkingInstructions">
              {{ venue.parkingInfo.parkingInstructions }}
            </p>
          </mat-card-content>
        </mat-card>

        <!-- Access Instructions -->
        <mat-card *ngIf="venue.accessInstructions">
          <mat-card-header>
            <mat-card-title>Access Instructions</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <p>{{ venue.accessInstructions }}</p>
          </mat-card-content>
        </mat-card>
      </div>
    </mat-tab>

    <!-- Contacts Tab -->
    <mat-tab label="Contacts">
      <div class="tab-content">
        <app-venue-contacts
          [venueId]="venue.venueId"
          [contacts]="venue.contacts"
          [canManage]="canManageContacts">
        </app-venue-contacts>
      </div>
    </mat-tab>

    <!-- History Tab -->
    <mat-tab label="History">
      <div class="tab-content">
        <app-venue-history [venueId]="venue.venueId"></app-venue-history>
      </div>
    </mat-tab>

    <!-- Issues Tab -->
    <mat-tab label="Issues">
      <div class="tab-content">
        <app-venue-issues
          [venueId]="venue.venueId"
          [canReport]="canReportIssues">
        </app-venue-issues>
      </div>
    </mat-tab>

    <!-- Analytics Tab -->
    <mat-tab label="Analytics" *ngIf="canViewAnalytics">
      <div class="tab-content">
        <app-venue-analytics [venueId]="venue.venueId"></app-venue-analytics>
      </div>
    </mat-tab>
  </mat-tab-group>
</div>
```

### 5.3 Venue Form Component

#### Template
```html
<div class="venue-form-container">
  <h2>{{ isEditMode ? 'Edit Venue' : 'Create New Venue' }}</h2>

  <form [formGroup]="venueForm" (ngSubmit)="onSubmit()">
    <!-- Basic Information -->
    <mat-card>
      <mat-card-header>
        <mat-card-title>Basic Information</mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <div class="form-row">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Venue Name</mat-label>
            <input matInput formControlName="name" required>
            <mat-error *ngIf="venueForm.get('name')?.hasError('required')">
              Venue name is required
            </mat-error>
            <mat-error *ngIf="venueForm.get('name')?.hasError('maxlength')">
              Name cannot exceed 200 characters
            </mat-error>
          </mat-form-field>
        </div>

        <div class="form-row">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Description</mat-label>
            <textarea matInput formControlName="description" rows="4"></textarea>
            <mat-hint align="end">
              {{ venueForm.get('description')?.value?.length || 0 }} / 2000
            </mat-hint>
          </mat-form-field>
        </div>

        <div class="form-row">
          <mat-form-field appearance="outline">
            <mat-label>Venue Type</mat-label>
            <mat-select formControlName="venueType" required>
              <mat-option *ngFor="let type of venueTypes" [value]="type">
                {{ type }}
              </mat-option>
            </mat-select>
          </mat-form-field>
        </div>
      </mat-card-content>
    </mat-card>

    <!-- Address -->
    <mat-card formGroupName="address">
      <mat-card-header>
        <mat-card-title>Address</mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <div class="form-row">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Street Address</mat-label>
            <input matInput formControlName="street1" required>
          </mat-form-field>
        </div>

        <div class="form-row">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Street Address Line 2</mat-label>
            <input matInput formControlName="street2">
          </mat-form-field>
        </div>

        <div class="form-row">
          <mat-form-field appearance="outline">
            <mat-label>City</mat-label>
            <input matInput formControlName="city" required>
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>State/Province</mat-label>
            <input matInput formControlName="state" required>
          </mat-form-field>
        </div>

        <div class="form-row">
          <mat-form-field appearance="outline">
            <mat-label>Postal Code</mat-label>
            <input matInput formControlName="postalCode" required>
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Country</mat-label>
            <mat-select formControlName="country" required>
              <mat-option *ngFor="let country of countries" [value]="country">
                {{ country }}
              </mat-option>
            </mat-select>
          </mat-form-field>
        </div>
      </mat-card-content>
    </mat-card>

    <!-- Capacity -->
    <mat-card formGroupName="capacity">
      <mat-card-header>
        <mat-card-title>Capacity</mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <div class="form-row">
          <mat-form-field appearance="outline">
            <mat-label>Maximum Capacity</mat-label>
            <input matInput type="number" formControlName="maxCapacity" required>
            <mat-error *ngIf="venueForm.get('capacity.maxCapacity')?.hasError('min')">
              Capacity must be greater than 0
            </mat-error>
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Seated Capacity</mat-label>
            <input matInput type="number" formControlName="seatedCapacity">
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Standing Capacity</mat-label>
            <input matInput type="number" formControlName="standingCapacity">
          </mat-form-field>
        </div>
      </mat-card-content>
    </mat-card>

    <!-- Amenities -->
    <mat-card>
      <mat-card-header>
        <mat-card-title>Amenities</mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Select Amenities</mat-label>
          <mat-select formControlName="amenities" multiple>
            <mat-option *ngFor="let amenity of availableAmenities" [value]="amenity">
              {{ amenity }}
            </mat-option>
          </mat-select>
        </mat-form-field>
      </mat-card-content>
    </mat-card>

    <!-- Parking Information -->
    <mat-card formGroupName="parkingInfo">
      <mat-card-header>
        <mat-card-title>Parking Information</mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <div class="form-row">
          <mat-checkbox formControlName="hasParking">Parking Available</mat-checkbox>
        </div>

        <div *ngIf="venueForm.get('parkingInfo.hasParking')?.value">
          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Parking Type</mat-label>
              <mat-select formControlName="parkingType">
                <mat-option *ngFor="let type of parkingTypes" [value]="type">
                  {{ type }}
                </mat-option>
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Parking Capacity</mat-label>
              <input matInput type="number" formControlName="parkingCapacity">
            </mat-form-field>
          </div>

          <div class="form-row">
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Parking Instructions</mat-label>
              <textarea matInput formControlName="parkingInstructions" rows="2"></textarea>
            </mat-form-field>
          </div>
        </div>
      </mat-card-content>
    </mat-card>

    <!-- Access Instructions -->
    <mat-card>
      <mat-card-header>
        <mat-card-title>Access Instructions</mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Access Instructions</mat-label>
          <textarea matInput formControlName="accessInstructions" rows="3"></textarea>
          <mat-hint>Provide information about how to access the venue</mat-hint>
        </mat-form-field>
      </mat-card-content>
    </mat-card>

    <!-- Form Actions -->
    <div class="form-actions">
      <button mat-button type="button" (click)="cancel()">Cancel</button>
      <button mat-raised-button color="primary" type="submit" [disabled]="!venueForm.valid || submitting">
        <mat-spinner *ngIf="submitting" diameter="20"></mat-spinner>
        {{ isEditMode ? 'Update' : 'Create' }} Venue
      </button>
    </div>
  </form>
</div>
```

#### Component Class
```typescript
@Component({
  selector: 'app-venue-form',
  standalone: true,
  imports: [CommonModule, MaterialModule, ReactiveFormsModule],
  templateUrl: './venue-form.component.html',
  styleUrls: ['./venue-form.component.scss']
})
export class VenueFormComponent implements OnInit {
  venueForm: FormGroup;
  isEditMode = false;
  submitting = false;
  venueId?: string;

  venueTypes = Object.values(VenueType);
  parkingTypes = Object.values(ParkingType);
  countries = ['USA', 'Canada', 'UK', 'Australia']; // Load from service
  availableAmenities = ['WiFi', 'Parking', 'AC', 'Projector', 'Catering', 'Audio System', 'Stage'];

  constructor(
    private fb: FormBuilder,
    private store: Store,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.venueForm = this.createForm();
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params['id'] && params['id'] !== 'new') {
        this.isEditMode = true;
        this.venueId = params['id'];
        this.loadVenue(this.venueId);
      }
    });
  }

  createForm(): FormGroup {
    return this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', Validators.maxLength(2000)],
      venueType: ['', Validators.required],
      address: this.fb.group({
        street1: ['', Validators.required],
        street2: [''],
        city: ['', Validators.required],
        state: ['', Validators.required],
        country: ['', Validators.required],
        postalCode: ['', Validators.required]
      }),
      capacity: this.fb.group({
        maxCapacity: [0, [Validators.required, Validators.min(1)]],
        seatedCapacity: [0],
        standingCapacity: [0]
      }),
      amenities: [[]],
      parkingInfo: this.fb.group({
        hasParking: [false],
        parkingType: [''],
        parkingCapacity: [0],
        parkingInstructions: ['']
      }),
      accessInstructions: ['']
    });
  }

  loadVenue(venueId: string) {
    this.store.dispatch(loadVenue({ venueId }));
    this.store.select(selectSelectedVenue)
      .pipe(filter(venue => !!venue))
      .subscribe(venue => {
        this.venueForm.patchValue(venue);
      });
  }

  onSubmit() {
    if (this.venueForm.valid) {
      this.submitting = true;
      const venue = this.venueForm.value;

      if (this.isEditMode && this.venueId) {
        this.store.dispatch(updateVenue({
          venueId: this.venueId,
          changes: venue
        }));
      } else {
        this.store.dispatch(createVenue({ venue }));
      }

      // Navigate back on success
      this.store.select(selectVenueError)
        .pipe(filter(error => error === null))
        .subscribe(() => {
          this.submitting = false;
          this.snackBar.open('Venue saved successfully', 'Close', { duration: 3000 });
          this.router.navigate(['/venues']);
        });
    }
  }

  cancel() {
    this.router.navigate(['/venues']);
  }
}
```

## 6. Services

### 6.1 Venue API Service
```typescript
@Injectable({ providedIn: 'root' })
export class VenueApiService {
  private readonly apiUrl = '/api/v1/venues';

  constructor(private http: HttpClient) {}

  getVenues(filter?: VenueFilter, page = 1, pageSize = 20): Observable<PagedResult<Venue>> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    if (filter) {
      params = this.addFilterParams(params, filter);
    }

    return this.http.get<PagedResult<Venue>>(this.apiUrl, { params });
  }

  getVenue(venueId: string): Observable<Venue> {
    return this.http.get<Venue>(`${this.apiUrl}/${venueId}`);
  }

  createVenue(venue: Partial<Venue>): Observable<Venue> {
    return this.http.post<Venue>(this.apiUrl, venue);
  }

  updateVenue(venueId: string, changes: Partial<Venue>): Observable<Venue> {
    return this.http.put<Venue>(`${this.apiUrl}/${venueId}`, changes);
  }

  deleteVenue(venueId: string, reason?: string): Observable<void> {
    const params = reason ? new HttpParams().set('reason', reason) : undefined;
    return this.http.delete<void>(`${this.apiUrl}/${venueId}`, { params });
  }

  activateVenue(venueId: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${venueId}/activate`, {});
  }

  deactivateVenue(venueId: string, reason: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${venueId}/deactivate`, { reason });
  }

  blacklistVenue(venueId: string, reason: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${venueId}/blacklist`, { reason });
  }

  whitelistVenue(venueId: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${venueId}/whitelist`, {});
  }

  // Contacts
  addContact(venueId: string, contact: VenueContact): Observable<{ contactId: string }> {
    return this.http.post<{ contactId: string }>(`${this.apiUrl}/${venueId}/contacts`, contact);
  }

  updateContact(venueId: string, contactId: string, changes: Partial<VenueContact>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${venueId}/contacts/${contactId}`, changes);
  }

  removeContact(venueId: string, contactId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${venueId}/contacts/${contactId}`);
  }

  // Photos
  uploadPhoto(venueId: string, file: File, caption?: string, isPrimary = false): Observable<VenuePhoto> {
    const formData = new FormData();
    formData.append('file', file);
    if (caption) formData.append('caption', caption);
    formData.append('isPrimary', isPrimary.toString());

    return this.http.post<VenuePhoto>(`${this.apiUrl}/${venueId}/photos`, formData);
  }

  deletePhoto(venueId: string, photoId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${venueId}/photos/${photoId}`);
  }

  // History
  getHistory(venueId: string, startDate?: Date, endDate?: Date): Observable<VenueHistory[]> {
    let params = new HttpParams();
    if (startDate) params = params.set('startDate', startDate.toISOString());
    if (endDate) params = params.set('endDate', endDate.toISOString());

    return this.http.get<VenueHistory[]>(`${this.apiUrl}/${venueId}/history`, { params });
  }

  submitFeedback(venueId: string, eventId: string, feedback: string): Observable<{ sentimentScore: number }> {
    return this.http.post<{ sentimentScore: number }>(`${this.apiUrl}/${venueId}/feedback`, {
      eventId,
      feedback
    });
  }

  recordRating(venueId: string, eventId: string, rating: number, feedback?: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${venueId}/ratings`, {
      eventId,
      rating,
      feedback
    });
  }

  // Issues
  getIssues(venueId: string, status?: IssueStatus, severity?: Severity): Observable<VenueIssue[]> {
    let params = new HttpParams();
    if (status) params = params.set('status', status);
    if (severity) params = params.set('severity', severity);

    return this.http.get<VenueIssue[]>(`${this.apiUrl}/${venueId}/issues`, { params });
  }

  reportIssue(venueId: string, issueType: IssueType, severity: Severity, description: string): Observable<{ issueId: string }> {
    return this.http.post<{ issueId: string }>(`${this.apiUrl}/${venueId}/issues`, {
      issueType,
      severity,
      description
    });
  }

  // Search
  searchVenues(query: string, filters?: VenueFilter): Observable<Venue[]> {
    let params = new HttpParams().set('q', query);
    if (filters) {
      params = this.addFilterParams(params, filters);
    }
    return this.http.get<Venue[]>(`${this.apiUrl}/search`, { params });
  }

  getTopRated(limit = 10, venueType?: VenueType, city?: string): Observable<Venue[]> {
    let params = new HttpParams().set('limit', limit.toString());
    if (venueType) params = params.set('venueType', venueType);
    if (city) params = params.set('city', city);

    return this.http.get<Venue[]>(`${this.apiUrl}/top-rated`, { params });
  }

  private addFilterParams(params: HttpParams, filter: VenueFilter): HttpParams {
    if (filter.status) params = params.set('status', filter.status);
    if (filter.venueType) params = params.set('venueType', filter.venueType);
    if (filter.city) params = params.set('city', filter.city);
    if (filter.country) params = params.set('country', filter.country);
    if (filter.minCapacity) params = params.set('minCapacity', filter.minCapacity.toString());
    if (filter.searchTerm) params = params.set('searchTerm', filter.searchTerm);
    if (filter.sortBy) params = params.set('sortBy', filter.sortBy);
    if (filter.sortOrder) params = params.set('sortOrder', filter.sortOrder);
    return params;
  }
}
```

## 7. Routing

```typescript
const routes: Routes = [
  {
    path: '',
    component: VenueListComponent,
    canActivate: [AuthGuard],
    data: { permission: 'venues.view' }
  },
  {
    path: 'new',
    component: VenueFormComponent,
    canActivate: [AuthGuard],
    canDeactivate: [UnsavedChangesGuard],
    data: { permission: 'venues.create' }
  },
  {
    path: ':id',
    component: VenueDetailComponent,
    canActivate: [AuthGuard],
    data: { permission: 'venues.view' }
  },
  {
    path: ':id/edit',
    component: VenueFormComponent,
    canActivate: [AuthGuard],
    canDeactivate: [UnsavedChangesGuard],
    data: { permission: 'venues.update' }
  }
];
```

## 8. Styling (SCSS)

### 8.1 Material Theme Customization
```scss
@use '@angular/material' as mat;

$primary-palette: mat.define-palette(mat.$indigo-palette);
$accent-palette: mat.define-palette(mat.$pink-palette);
$warn-palette: mat.define-palette(mat.$red-palette);

$theme: mat.define-light-theme((
  color: (
    primary: $primary-palette,
    accent: $accent-palette,
    warn: $warn-palette,
  ),
  typography: mat.define-typography-config(),
  density: 0,
));

@include mat.all-component-themes($theme);
```

### 8.2 Responsive Design
```scss
// Breakpoints
$mobile: 600px;
$tablet: 960px;
$desktop: 1280px;

.venues-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 24px;
  padding: 24px;

  @media (max-width: $mobile) {
    grid-template-columns: 1fr;
    padding: 16px;
  }
}
```

## 9. Testing

### 9.1 Unit Tests
```typescript
describe('VenueListComponent', () => {
  let component: VenueListComponent;
  let fixture: ComponentFixture<VenueListComponent>;
  let store: MockStore;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VenueListComponent],
      providers: [
        provideMockStore({ initialState })
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(VenueListComponent);
    component = fixture.componentInstance;
    store = TestBed.inject(MockStore);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load venues on init', () => {
    spyOn(store, 'dispatch');
    component.ngOnInit();
    expect(store.dispatch).toHaveBeenCalledWith(
      loadVenues({ page: 1, pageSize: 20 })
    );
  });
});
```

### 9.2 E2E Tests (Cypress)
```typescript
describe('Venue Management', () => {
  beforeEach(() => {
    cy.login();
    cy.visit('/venues');
  });

  it('should display venue list', () => {
    cy.get('app-venue-card').should('have.length.greaterThan', 0);
  });

  it('should create new venue', () => {
    cy.get('[data-test="add-venue-btn"]').click();
    cy.get('[formControlName="name"]').type('Test Venue');
    cy.get('[formControlName="venueType"]').click();
    cy.get('mat-option').contains('Conference Center').click();
    // Fill other fields...
    cy.get('[type="submit"]').click();
    cy.contains('Venue saved successfully');
  });

  it('should search venues', () => {
    cy.get('[data-test="search-input"]').type('Conference');
    cy.wait(500);
    cy.get('app-venue-card').should('contain', 'Conference');
  });
});
```

## 10. Performance Optimization

### 10.1 Lazy Loading
- Feature modules loaded on demand
- Route-based code splitting
- Defer loading of heavy components

### 10.2 Change Detection
```typescript
@Component({
  changeDetection: ChangeDetectionStrategy.OnPush
})
```

### 10.3 Virtual Scrolling
```html
<cdk-virtual-scroll-viewport itemSize="100" class="venue-list">
  <div *cdkVirtualFor="let venue of venues">
    <app-venue-card [venue]="venue"></app-venue-card>
  </div>
</cdk-virtual-scroll-viewport>
```

### 10.4 Image Optimization
- Lazy load images
- Use thumbnails for lists
- Implement progressive image loading
- CDN integration

## 11. Accessibility (A11y)

### 11.1 ARIA Labels
```html
<button mat-icon-button aria-label="Delete venue" (click)="deleteVenue()">
  <mat-icon>delete</mat-icon>
</button>
```

### 11.2 Keyboard Navigation
- Tab order management
- Keyboard shortcuts
- Focus management

### 11.3 Screen Reader Support
- Meaningful labels
- Live regions for dynamic content
- Descriptive error messages

## 12. Security

### 12.1 XSS Protection
- Sanitize user inputs
- Use Angular's built-in sanitization
- Content Security Policy

### 12.2 Authentication
```typescript
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.getToken();
    if (token) {
      req = req.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
      });
    }
    return next.handle(req);
  }
}
```

## 13. Error Handling

### 13.1 HTTP Error Interceptor
```typescript
@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = 'An error occurred';

        if (error.error instanceof ErrorEvent) {
          errorMessage = error.error.message;
        } else {
          errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
        }

        this.snackBar.open(errorMessage, 'Close', { duration: 5000 });
        return throwError(() => error);
      })
    );
  }
}
```

## 14. Build and Deployment

### 14.1 Build Configuration
```json
{
  "configurations": {
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
}
```

### 14.2 Environment Configuration
```typescript
export const environment = {
  production: true,
  apiUrl: 'https://api.eventmanagement.com',
  mapsApiKey: 'YOUR_MAPS_API_KEY',
  applicationInsights: {
    instrumentationKey: 'YOUR_KEY'
  }
};
```

## Version History
- v1.0.0 - Initial specification (2025-12-22)
