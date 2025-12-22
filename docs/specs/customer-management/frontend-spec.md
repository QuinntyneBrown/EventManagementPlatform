# Contact & Customer Management - Frontend Specification

## Document Information
- **Version**: 1.0.0
- **Last Updated**: 2025-12-22
- **Technology Stack**: Angular 18+, Angular Material, RxJS
- **Architecture Pattern**: Component-Based Architecture, Reactive Programming

## Table of Contents
1. [Overview](#overview)
2. [Architecture](#architecture)
3. [Module Structure](#module-structure)
4. [Component Specifications](#component-specifications)
5. [State Management](#state-management)
6. [Routing](#routing)
7. [UI/UX Design](#uiux-design)
8. [Forms and Validation](#forms-and-validation)
9. [Data Services](#data-services)
10. [Real-time Features](#real-time-features)
11. [Accessibility](#accessibility)
12. [Performance Optimization](#performance-optimization)
13. [Testing Strategy](#testing-strategy)
14. [Deployment](#deployment)

---

## 1. Overview

### 1.1 Purpose
The Customer Management frontend provides an intuitive, responsive interface for managing customer profiles, contacts, communications, complaints, and testimonials within the Event Management Platform.

### 1.2 Scope
This specification covers:
- Customer profile management interface
- Contact list management
- Communication history tracking and creation
- Complaint management dashboard
- Testimonial collection and display
- Customer insights and analytics dashboard
- Contact import/export functionality
- Real-time notifications
- Responsive design for desktop, tablet, and mobile

### 1.3 Key Features
- Modern Material Design interface
- Real-time updates via SignalR
- Advanced search and filtering
- Data visualization with charts
- Drag-and-drop functionality
- Offline capability (PWA)
- Accessibility compliance (WCAG 2.1 AA)
- Multi-language support

---

## 2. Architecture

### 2.1 High-Level Architecture
```
┌─────────────────────────────────────────────────────────────┐
│                    Angular Application                       │
│                                                              │
│  ┌──────────────────────────────────────────────────────┐  │
│  │              Customer Management Module               │  │
│  │  ┌────────────┐ ┌────────────┐ ┌────────────┐       │  │
│  │  │ Components │ │  Services  │ │   Guards   │       │  │
│  │  └────────────┘ └────────────┘ └────────────┘       │  │
│  │  ┌────────────┐ ┌────────────┐ ┌────────────┐       │  │
│  │  │   Models   │ │   Pipes    │ │ Validators │       │  │
│  │  └────────────┘ └────────────┘ └────────────┘       │  │
│  └──────────────────────────────────────────────────────┘  │
│                                                              │
│  ┌──────────────────────────────────────────────────────┐  │
│  │                   Shared Module                       │  │
│  │  (UI Components, Directives, Pipes, Services)        │  │
│  └──────────────────────────────────────────────────────┘  │
│                                                              │
│  ┌──────────────────────────────────────────────────────┐  │
│  │                    Core Module                        │  │
│  │  (HTTP, Auth, State Management, Error Handling)      │  │
│  └──────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────┐
│                     Backend API                              │
│              (REST API + SignalR Hubs)                       │
└─────────────────────────────────────────────────────────────┘
```

### 2.2 Technology Stack
- **Framework**: Angular 18+
- **UI Library**: Angular Material 18+
- **State Management**: NgRx (Store, Effects, Entity)
- **Forms**: Reactive Forms
- **HTTP Client**: Angular HttpClient
- **Real-time**: SignalR for Angular
- **Routing**: Angular Router
- **Internationalization**: ngx-translate
- **Charts**: Chart.js with ng2-charts
- **Date Handling**: date-fns
- **File Upload**: ngx-file-drop
- **Rich Text Editor**: ngx-quill
- **Testing**: Jasmine, Karma, Cypress

### 2.3 Design Patterns
- **Smart/Dumb Components** (Container/Presentational)
- **Reactive Programming** (RxJS)
- **Facade Pattern** (for state management)
- **Strategy Pattern** (for form validation)
- **Observer Pattern** (for event handling)
- **Lazy Loading** (for route modules)

---

## 3. Module Structure

### 3.1 Feature Module Structure
```
src/app/features/customer-management/
├── customer-management.module.ts
├── customer-management-routing.module.ts
├── components/
│   ├── customer-list/
│   │   ├── customer-list.component.ts
│   │   ├── customer-list.component.html
│   │   ├── customer-list.component.scss
│   │   └── customer-list.component.spec.ts
│   ├── customer-detail/
│   │   ├── customer-detail.component.ts
│   │   ├── customer-detail.component.html
│   │   ├── customer-detail.component.scss
│   │   └── customer-detail.component.spec.ts
│   ├── customer-form/
│   ├── contact-list/
│   ├── contact-form/
│   ├── communication-history/
│   ├── communication-form/
│   ├── complaint-list/
│   ├── complaint-detail/
│   ├── testimonial-list/
│   ├── customer-insights/
│   └── customer-dashboard/
├── services/
│   ├── customer.service.ts
│   ├── contact.service.ts
│   ├── communication.service.ts
│   ├── complaint.service.ts
│   └── testimonial.service.ts
├── store/
│   ├── actions/
│   ├── effects/
│   ├── reducers/
│   ├── selectors/
│   └── customer.facade.ts
├── models/
│   ├── customer.model.ts
│   ├── contact.model.ts
│   ├── communication.model.ts
│   └── complaint.model.ts
├── guards/
│   └── customer-management.guard.ts
└── validators/
    └── customer.validators.ts
```

---

## 4. Component Specifications

### 4.1 Customer List Component

#### 4.1.1 Overview
Displays a searchable, filterable, and sortable list of customers with pagination.

#### 4.1.2 Component Class
```typescript
@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomerListComponent implements OnInit, OnDestroy {
  customers$: Observable<Customer[]>;
  loading$: Observable<boolean>;
  totalCount$: Observable<number>;

  displayedColumns = [
    'customerNumber',
    'companyName',
    'type',
    'segment',
    'status',
    'lifetimeValue',
    'actions'
  ];

  searchControl = new FormControl('');
  filterForm: FormGroup;

  pageSize = 20;
  pageIndex = 0;

  constructor(
    private customerFacade: CustomerFacade,
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadCustomers();
    this.setupSearch();
    this.setupFilters();
  }

  loadCustomers(): void {
    this.customerFacade.loadCustomers({
      page: this.pageIndex,
      pageSize: this.pageSize
    });
  }

  onSearch(query: string): void {
    this.customerFacade.searchCustomers(query);
  }

  onFilterChange(filters: CustomerFilters): void {
    this.customerFacade.filterCustomers(filters);
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadCustomers();
  }

  viewCustomer(customer: Customer): void {
    this.router.navigate(['/customers', customer.id]);
  }

  editCustomer(customer: Customer): void {
    this.router.navigate(['/customers', customer.id, 'edit']);
  }

  deleteCustomer(customer: Customer): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Deactivate Customer',
        message: `Are you sure you want to deactivate ${customer.companyName}?`
      }
    });

    dialogRef.afterClosed().pipe(
      filter(result => result),
      switchMap(() => this.customerFacade.deactivateCustomer(customer.id))
    ).subscribe();
  }

  exportCustomers(): void {
    this.customerFacade.exportCustomers();
  }
}
```

#### 4.1.3 Template
```html
<div class="customer-list-container">
  <mat-toolbar color="primary">
    <h1>Customer Management</h1>
    <span class="spacer"></span>
    <button mat-raised-button color="accent" routerLink="/customers/new">
      <mat-icon>add</mat-icon>
      New Customer
    </button>
  </mat-toolbar>

  <mat-card class="filter-card">
    <mat-card-content>
      <!-- Search Bar -->
      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Search Customers</mat-label>
        <input matInput [formControl]="searchControl" placeholder="Search by name, email, or number">
        <mat-icon matPrefix>search</mat-icon>
      </mat-form-field>

      <!-- Filters -->
      <form [formGroup]="filterForm" class="filter-form">
        <mat-form-field appearance="outline">
          <mat-label>Type</mat-label>
          <mat-select formControlName="type" multiple>
            <mat-option value="Individual">Individual</mat-option>
            <mat-option value="SmallBusiness">Small Business</mat-option>
            <mat-option value="Enterprise">Enterprise</mat-option>
            <mat-option value="NonProfit">Non-Profit</mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Segment</mat-label>
          <mat-select formControlName="segment" multiple>
            <mat-option value="Standard">Standard</mat-option>
            <mat-option value="Premium">Premium</mat-option>
            <mat-option value="VIP">VIP</mat-option>
            <mat-option value="Corporate">Corporate</mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Status</mat-label>
          <mat-select formControlName="status" multiple>
            <mat-option value="Active">Active</mat-option>
            <mat-option value="Inactive">Inactive</mat-option>
            <mat-option value="Suspended">Suspended</mat-option>
          </mat-select>
        </mat-form-field>

        <button mat-raised-button type="button" (click)="clearFilters()">
          Clear Filters
        </button>
      </form>
    </mat-card-content>
  </mat-card>

  <!-- Customer Table -->
  <mat-card class="table-card">
    <mat-card-content>
      <div class="table-actions">
        <button mat-button (click)="exportCustomers()">
          <mat-icon>download</mat-icon>
          Export
        </button>
      </div>

      <div class="table-container">
        <table mat-table [dataSource]="customers$ | async" matSort>
          <!-- Customer Number Column -->
          <ng-container matColumnDef="customerNumber">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>
              Customer #
            </th>
            <td mat-cell *matCellDef="let customer">
              {{ customer.customerNumber }}
            </td>
          </ng-container>

          <!-- Company Name Column -->
          <ng-container matColumnDef="companyName">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>
              Company Name
            </th>
            <td mat-cell *matCellDef="let customer">
              {{ customer.profile.companyName }}
            </td>
          </ng-container>

          <!-- Type Column -->
          <ng-container matColumnDef="type">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>
              Type
            </th>
            <td mat-cell *matCellDef="let customer">
              <mat-chip>{{ customer.profile.type }}</mat-chip>
            </td>
          </ng-container>

          <!-- Segment Column -->
          <ng-container matColumnDef="segment">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>
              Segment
            </th>
            <td mat-cell *matCellDef="let customer">
              <mat-chip [color]="getSegmentColor(customer.profile.segment)">
                {{ customer.profile.segment }}
              </mat-chip>
            </td>
          </ng-container>

          <!-- Status Column -->
          <ng-container matColumnDef="status">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>
              Status
            </th>
            <td mat-cell *matCellDef="let customer">
              <mat-chip [color]="getStatusColor(customer.status)">
                {{ customer.status }}
              </mat-chip>
            </td>
          </ng-container>

          <!-- Lifetime Value Column -->
          <ng-container matColumnDef="lifetimeValue">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>
              Lifetime Value
            </th>
            <td mat-cell *matCellDef="let customer">
              {{ customer.profile.lifetimeValue | currency }}
            </td>
          </ng-container>

          <!-- Actions Column -->
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let customer">
              <button mat-icon-button [matMenuTriggerFor]="menu">
                <mat-icon>more_vert</mat-icon>
              </button>
              <mat-menu #menu="matMenu">
                <button mat-menu-item (click)="viewCustomer(customer)">
                  <mat-icon>visibility</mat-icon>
                  View
                </button>
                <button mat-menu-item (click)="editCustomer(customer)">
                  <mat-icon>edit</mat-icon>
                  Edit
                </button>
                <button mat-menu-item (click)="deleteCustomer(customer)">
                  <mat-icon>delete</mat-icon>
                  Deactivate
                </button>
              </mat-menu>
            </td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns;"
              class="customer-row"
              (click)="viewCustomer(row)">
          </tr>
        </table>
      </div>

      <mat-paginator
        [length]="totalCount$ | async"
        [pageSize]="pageSize"
        [pageSizeOptions]="[10, 20, 50, 100]"
        (page)="onPageChange($event)"
        showFirstLastButtons>
      </mat-paginator>
    </mat-card-content>
  </mat-card>
</div>
```

### 4.2 Customer Detail Component

#### 4.2.1 Overview
Displays comprehensive customer information with tabs for profile, contacts, communications, complaints, and insights.

#### 4.2.2 Component Class
```typescript
@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomerDetailComponent implements OnInit {
  customer$: Observable<Customer>;
  contacts$: Observable<Contact[]>;
  communications$: Observable<Communication[]>;
  complaints$: Observable<Complaint[]>;
  testimonials$: Observable<Testimonial[]>;
  insights$: Observable<CustomerInsights>;

  selectedTabIndex = 0;

  constructor(
    private route: ActivatedRoute,
    private customerFacade: CustomerFacade,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    const customerId = this.route.snapshot.paramMap.get('id');
    this.customerFacade.loadCustomerById(customerId);
    this.customer$ = this.customerFacade.selectedCustomer$;
    this.contacts$ = this.customerFacade.customerContacts$;
    this.communications$ = this.customerFacade.customerCommunications$;
    this.complaints$ = this.customerFacade.customerComplaints$;
    this.testimonials$ = this.customerFacade.customerTestimonials$;
    this.insights$ = this.customerFacade.customerInsights$;
  }

  sendEmail(customer: Customer): void {
    const dialogRef = this.dialog.open(EmailDialogComponent, {
      width: '800px',
      data: { customer }
    });

    dialogRef.afterClosed().pipe(
      filter(result => result),
      switchMap(emailData => this.customerFacade.sendEmail(emailData))
    ).subscribe();
  }

  scheduleFollowUp(customer: Customer): void {
    const dialogRef = this.dialog.open(FollowUpDialogComponent, {
      width: '600px',
      data: { customer }
    });

    dialogRef.afterClosed().pipe(
      filter(result => result),
      switchMap(followUpData => this.customerFacade.createFollowUp(followUpData))
    ).subscribe();
  }
}
```

#### 4.2.3 Template
```html
<div class="customer-detail-container" *ngIf="customer$ | async as customer">
  <!-- Header -->
  <mat-toolbar color="primary">
    <button mat-icon-button routerLink="/customers">
      <mat-icon>arrow_back</mat-icon>
    </button>
    <h1>{{ customer.profile.companyName }}</h1>
    <span class="spacer"></span>
    <button mat-button (click)="sendEmail(customer)">
      <mat-icon>email</mat-icon>
      Send Email
    </button>
    <button mat-button (click)="scheduleFollowUp(customer)">
      <mat-icon>event</mat-icon>
      Follow Up
    </button>
    <button mat-icon-button [routerLink]="['/customers', customer.id, 'edit']">
      <mat-icon>edit</mat-icon>
    </button>
  </mat-toolbar>

  <!-- Customer Summary Card -->
  <mat-card class="summary-card">
    <mat-card-content>
      <div class="summary-grid">
        <div class="summary-item">
          <span class="label">Customer #:</span>
          <span class="value">{{ customer.customerNumber }}</span>
        </div>
        <div class="summary-item">
          <span class="label">Type:</span>
          <mat-chip>{{ customer.profile.type }}</mat-chip>
        </div>
        <div class="summary-item">
          <span class="label">Segment:</span>
          <mat-chip>{{ customer.profile.segment }}</mat-chip>
        </div>
        <div class="summary-item">
          <span class="label">Status:</span>
          <mat-chip [color]="getStatusColor(customer.status)">
            {{ customer.status }}
          </mat-chip>
        </div>
        <div class="summary-item">
          <span class="label">Lifetime Value:</span>
          <span class="value">{{ customer.profile.lifetimeValue | currency }}</span>
        </div>
        <div class="summary-item">
          <span class="label">Total Events:</span>
          <span class="value">{{ customer.profile.totalEvents }}</span>
        </div>
      </div>
    </mat-card-content>
  </mat-card>

  <!-- Tabs -->
  <mat-tab-group [(selectedIndex)]="selectedTabIndex" animationDuration="300ms">
    <!-- Profile Tab -->
    <mat-tab label="Profile">
      <app-customer-profile [customer]="customer"></app-customer-profile>
    </mat-tab>

    <!-- Contacts Tab -->
    <mat-tab label="Contacts">
      <app-contact-list
        [customerId]="customer.id"
        [contacts]="contacts$ | async">
      </app-contact-list>
    </mat-tab>

    <!-- Communications Tab -->
    <mat-tab label="Communications">
      <app-communication-history
        [customerId]="customer.id"
        [communications]="communications$ | async">
      </app-communication-history>
    </mat-tab>

    <!-- Complaints Tab -->
    <mat-tab label="Complaints">
      <app-complaint-list
        [customerId]="customer.id"
        [complaints]="complaints$ | async">
      </app-complaint-list>
    </mat-tab>

    <!-- Testimonials Tab -->
    <mat-tab label="Testimonials">
      <app-testimonial-list
        [customerId]="customer.id"
        [testimonials]="testimonials$ | async">
      </app-testimonial-list>
    </mat-tab>

    <!-- Insights Tab -->
    <mat-tab label="AI Insights">
      <app-customer-insights
        [insights]="insights$ | async">
      </app-customer-insights>
    </mat-tab>
  </mat-tab-group>
</div>
```

### 4.3 Customer Form Component

#### 4.3.1 Component Class
```typescript
@Component({
  selector: 'app-customer-form',
  templateUrl: './customer-form.component.html',
  styleUrls: ['./customer-form.component.scss']
})
export class CustomerFormComponent implements OnInit {
  customerForm: FormGroup;
  isEditMode = false;
  customerId: string;

  customerTypes = ['Individual', 'SmallBusiness', 'Enterprise', 'NonProfit', 'Government'];
  industries = ['Technology', 'Healthcare', 'Finance', 'Education', 'Retail', 'Other'];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private customerFacade: CustomerFacade,
    private snackBar: MatSnackBar
  ) {
    this.createForm();
  }

  ngOnInit(): void {
    this.customerId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.customerId;

    if (this.isEditMode) {
      this.customerFacade.loadCustomerById(this.customerId);
      this.customerFacade.selectedCustomer$.pipe(
        filter(customer => !!customer),
        take(1)
      ).subscribe(customer => {
        this.populateForm(customer);
      });
    }
  }

  createForm(): void {
    this.customerForm = this.fb.group({
      companyName: ['', [Validators.required, Validators.minLength(2)]],
      type: ['', Validators.required],
      industry: [''],
      primaryEmail: ['', [Validators.required, Validators.email]],
      secondaryEmail: ['', Validators.email],
      primaryPhone: ['', [Validators.required, Validators.pattern(/^\+?[1-9]\d{1,14}$/)]],
      secondaryPhone: [''],
      website: ['', Validators.pattern(/^https?:\/\/.+/)],
      billingAddress: this.fb.group({
        street: ['', Validators.required],
        city: ['', Validators.required],
        state: ['', Validators.required],
        zipCode: ['', Validators.required],
        country: ['', Validators.required]
      }),
      shippingAddress: this.fb.group({
        street: [''],
        city: [''],
        state: [''],
        zipCode: [''],
        country: ['']
      }),
      preferences: this.fb.group({
        communicationChannels: [[]],
        preferredLanguage: ['en'],
        timeZone: ['UTC'],
        emailNotifications: [true],
        smsNotifications: [false]
      })
    });
  }

  populateForm(customer: Customer): void {
    this.customerForm.patchValue({
      companyName: customer.profile.companyName,
      type: customer.profile.type,
      industry: customer.profile.industry,
      primaryEmail: customer.contactInfo.primaryEmail,
      secondaryEmail: customer.contactInfo.secondaryEmail,
      primaryPhone: customer.contactInfo.primaryPhone,
      secondaryPhone: customer.contactInfo.secondaryPhone,
      website: customer.contactInfo.website,
      billingAddress: customer.contactInfo.billingAddress,
      shippingAddress: customer.contactInfo.shippingAddress,
      preferences: customer.preferences
    });
  }

  copyBillingToShipping(): void {
    const billingAddress = this.customerForm.get('billingAddress').value;
    this.customerForm.get('shippingAddress').patchValue(billingAddress);
  }

  onSubmit(): void {
    if (this.customerForm.invalid) {
      this.customerForm.markAllAsTouched();
      return;
    }

    const formData = this.customerForm.value;

    const action$ = this.isEditMode
      ? this.customerFacade.updateCustomer(this.customerId, formData)
      : this.customerFacade.createCustomer(formData);

    action$.subscribe({
      next: () => {
        this.snackBar.open(
          `Customer ${this.isEditMode ? 'updated' : 'created'} successfully`,
          'Close',
          { duration: 3000 }
        );
        this.router.navigate(['/customers']);
      },
      error: (error) => {
        this.snackBar.open(
          `Error: ${error.message}`,
          'Close',
          { duration: 5000 }
        );
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/customers']);
  }
}
```

### 4.4 Contact List Component

#### 4.4.1 Component Class
```typescript
@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.scss']
})
export class ContactListComponent {
  @Input() customerId: string;
  @Input() contacts: Contact[];

  displayedColumns = ['name', 'email', 'phone', 'position', 'isPrimary', 'tags', 'actions'];

  constructor(
    private dialog: MatDialog,
    private customerFacade: CustomerFacade
  ) {}

  addContact(): void {
    const dialogRef = this.dialog.open(ContactFormDialogComponent, {
      width: '600px',
      data: { customerId: this.customerId }
    });

    dialogRef.afterClosed().pipe(
      filter(result => result),
      switchMap(contactData =>
        this.customerFacade.addContact(this.customerId, contactData)
      )
    ).subscribe();
  }

  editContact(contact: Contact): void {
    const dialogRef = this.dialog.open(ContactFormDialogComponent, {
      width: '600px',
      data: { customerId: this.customerId, contact }
    });

    dialogRef.afterClosed().pipe(
      filter(result => result),
      switchMap(contactData =>
        this.customerFacade.updateContact(this.customerId, contact.id, contactData)
      )
    ).subscribe();
  }

  removeContact(contact: Contact): void {
    this.customerFacade.removeContact(this.customerId, contact.id).subscribe();
  }

  importContacts(): void {
    const dialogRef = this.dialog.open(ContactImportDialogComponent, {
      width: '800px',
      data: { customerId: this.customerId }
    });
  }

  exportContacts(): void {
    this.customerFacade.exportContacts(this.customerId).subscribe();
  }
}
```

### 4.5 Communication History Component

#### 4.5.1 Component Class
```typescript
@Component({
  selector: 'app-communication-history',
  templateUrl: './communication-history.component.html',
  styleUrls: ['./communication-history.component.scss']
})
export class CommunicationHistoryComponent implements OnInit {
  @Input() customerId: string;
  @Input() communications: Communication[];

  filterForm: FormGroup;
  filteredCommunications: Communication[];

  constructor(
    private fb: FormBuilder,
    private dialog: MatDialog,
    private customerFacade: CustomerFacade
  ) {
    this.filterForm = this.fb.group({
      type: [[]],
      startDate: [null],
      endDate: [null]
    });
  }

  ngOnInit(): void {
    this.filterForm.valueChanges.pipe(
      debounceTime(300)
    ).subscribe(() => {
      this.applyFilters();
    });

    this.filteredCommunications = this.communications;
  }

  applyFilters(): void {
    const filters = this.filterForm.value;
    // Apply filtering logic
  }

  logPhoneCall(): void {
    const dialogRef = this.dialog.open(PhoneCallDialogComponent, {
      width: '600px',
      data: { customerId: this.customerId }
    });

    dialogRef.afterClosed().pipe(
      filter(result => result),
      switchMap(callData =>
        this.customerFacade.logPhoneCall(this.customerId, callData)
      )
    ).subscribe();
  }

  scheduleMeeting(): void {
    const dialogRef = this.dialog.open(MeetingDialogComponent, {
      width: '700px',
      data: { customerId: this.customerId }
    });

    dialogRef.afterClosed().pipe(
      filter(result => result),
      switchMap(meetingData =>
        this.customerFacade.scheduleMeeting(this.customerId, meetingData)
      )
    ).subscribe();
  }
}
```

### 4.6 Complaint Management Component

#### 4.6.1 Component Class
```typescript
@Component({
  selector: 'app-complaint-list',
  templateUrl: './complaint-list.component.html',
  styleUrls: ['./complaint-list.component.scss']
})
export class ComplaintListComponent {
  @Input() customerId: string;
  @Input() complaints: Complaint[];

  displayedColumns = [
    'complaintNumber',
    'subject',
    'category',
    'priority',
    'status',
    'createdAt',
    'actions'
  ];

  constructor(
    private dialog: MatDialog,
    private customerFacade: CustomerFacade
  ) {}

  viewComplaint(complaint: Complaint): void {
    const dialogRef = this.dialog.open(ComplaintDetailDialogComponent, {
      width: '800px',
      data: { complaint }
    });
  }

  resolveComplaint(complaint: Complaint): void {
    const dialogRef = this.dialog.open(ResolveComplaintDialogComponent, {
      width: '600px',
      data: { complaint }
    });

    dialogRef.afterClosed().pipe(
      filter(result => result),
      switchMap(resolution =>
        this.customerFacade.resolveComplaint(
          this.customerId,
          complaint.id,
          resolution
        )
      )
    ).subscribe();
  }
}
```

### 4.7 Customer Insights Component

#### 4.7.1 Component Class
```typescript
@Component({
  selector: 'app-customer-insights',
  templateUrl: './customer-insights.component.html',
  styleUrls: ['./customer-insights.component.scss']
})
export class CustomerInsightsComponent implements OnInit {
  @Input() insights: CustomerInsights;

  sentimentChartData: ChartData;
  engagementChartData: ChartData;

  constructor() {}

  ngOnInit(): void {
    this.setupCharts();
  }

  setupCharts(): void {
    // Sentiment chart
    this.sentimentChartData = {
      labels: ['Positive', 'Neutral', 'Negative'],
      datasets: [{
        data: [
          this.insights?.sentimentScore || 0,
          50,
          100 - (this.insights?.sentimentScore || 0)
        ],
        backgroundColor: ['#4caf50', '#ff9800', '#f44336']
      }]
    };

    // Engagement chart
    this.engagementChartData = {
      // Chart configuration
    };
  }

  getSentimentColor(score: number): string {
    if (score >= 70) return 'green';
    if (score >= 40) return 'orange';
    return 'red';
  }

  getChurnRiskColor(risk: number): string {
    if (risk < 30) return 'green';
    if (risk < 60) return 'orange';
    return 'red';
  }
}
```

---

## 5. State Management

### 5.1 NgRx Store Structure

#### 5.1.1 State Interface
```typescript
export interface CustomerManagementState {
  customers: EntityState<Customer>;
  selectedCustomerId: string | null;
  contacts: EntityState<Contact>;
  communications: EntityState<Communication>;
  complaints: EntityState<Complaint>;
  testimonials: EntityState<Testimonial>;
  insights: CustomerInsights | null;
  loading: boolean;
  error: string | null;
  filters: CustomerFilters;
  pagination: PaginationState;
}
```

#### 5.1.2 Actions
```typescript
// Customer Actions
export const loadCustomers = createAction(
  '[Customer List] Load Customers',
  props<{ params: CustomerQueryParams }>()
);

export const loadCustomersSuccess = createAction(
  '[Customer API] Load Customers Success',
  props<{ customers: Customer[]; totalCount: number }>()
);

export const loadCustomersFailure = createAction(
  '[Customer API] Load Customers Failure',
  props<{ error: string }>()
);

export const createCustomer = createAction(
  '[Customer Form] Create Customer',
  props<{ customer: CreateCustomerDto }>()
);

export const updateCustomer = createAction(
  '[Customer Form] Update Customer',
  props<{ id: string; changes: Partial<Customer> }>()
);

export const deactivateCustomer = createAction(
  '[Customer Detail] Deactivate Customer',
  props<{ id: string; reason: string }>()
);

// Contact Actions
export const addContact = createAction(
  '[Contact Form] Add Contact',
  props<{ customerId: string; contact: CreateContactDto }>()
);

export const updateContact = createAction(
  '[Contact Form] Update Contact',
  props<{ customerId: string; contactId: string; changes: Partial<Contact> }>()
);

export const removeContact = createAction(
  '[Contact List] Remove Contact',
  props<{ customerId: string; contactId: string }>()
);

// Communication Actions
export const sendEmail = createAction(
  '[Communication] Send Email',
  props<{ customerId: string; email: EmailDto }>()
);

export const logPhoneCall = createAction(
  '[Communication] Log Phone Call',
  props<{ customerId: string; call: PhoneCallDto }>()
);

export const scheduleMeeting = createAction(
  '[Communication] Schedule Meeting',
  props<{ customerId: string; meeting: MeetingDto }>()
);
```

#### 5.1.3 Reducers
```typescript
export const customerReducer = createReducer(
  initialState,
  on(loadCustomers, (state) => ({
    ...state,
    loading: true,
    error: null
  })),
  on(loadCustomersSuccess, (state, { customers, totalCount }) =>
    adapter.setAll(customers, {
      ...state,
      loading: false,
      pagination: {
        ...state.pagination,
        totalCount
      }
    })
  ),
  on(loadCustomersFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),
  on(createCustomer, (state) => ({
    ...state,
    loading: true
  })),
  // ... more reducer cases
);
```

#### 5.1.4 Effects
```typescript
@Injectable()
export class CustomerEffects {
  loadCustomers$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadCustomers),
      switchMap(({ params }) =>
        this.customerService.getCustomers(params).pipe(
          map(response => loadCustomersSuccess({
            customers: response.items,
            totalCount: response.totalCount
          })),
          catchError(error => of(loadCustomersFailure({ error: error.message })))
        )
      )
    )
  );

  createCustomer$ = createEffect(() =>
    this.actions$.pipe(
      ofType(createCustomer),
      switchMap(({ customer }) =>
        this.customerService.createCustomer(customer).pipe(
          map(created => createCustomerSuccess({ customer: created })),
          tap(() => this.router.navigate(['/customers'])),
          catchError(error => of(createCustomerFailure({ error: error.message })))
        )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private customerService: CustomerService,
    private router: Router
  ) {}
}
```

#### 5.1.5 Selectors
```typescript
export const selectCustomerState = createFeatureSelector<CustomerManagementState>(
  'customerManagement'
);

export const selectAllCustomers = createSelector(
  selectCustomerState,
  adapter.getSelectors().selectAll
);

export const selectCustomerById = (id: string) => createSelector(
  selectCustomerState,
  state => adapter.getSelectors().selectEntities(state)[id]
);

export const selectLoading = createSelector(
  selectCustomerState,
  state => state.loading
);

export const selectError = createSelector(
  selectCustomerState,
  state => state.error
);

export const selectTotalCount = createSelector(
  selectCustomerState,
  state => state.pagination.totalCount
);
```

#### 5.1.6 Facade Service
```typescript
@Injectable()
export class CustomerFacade {
  customers$ = this.store.select(selectAllCustomers);
  loading$ = this.store.select(selectLoading);
  error$ = this.store.select(selectError);
  selectedCustomer$ = this.store.select(selectSelectedCustomer);

  constructor(private store: Store<CustomerManagementState>) {}

  loadCustomers(params: CustomerQueryParams): void {
    this.store.dispatch(loadCustomers({ params }));
  }

  loadCustomerById(id: string): void {
    this.store.dispatch(loadCustomerById({ id }));
  }

  createCustomer(customer: CreateCustomerDto): Observable<void> {
    this.store.dispatch(createCustomer({ customer }));
    return this.actions$.pipe(
      ofType(createCustomerSuccess, createCustomerFailure),
      take(1),
      map(action => {
        if (action.type === createCustomerFailure.type) {
          throw new Error(action.error);
        }
      })
    );
  }

  updateCustomer(id: string, changes: Partial<Customer>): Observable<void> {
    this.store.dispatch(updateCustomer({ id, changes }));
    return this.waitForAction(updateCustomerSuccess, updateCustomerFailure);
  }

  deactivateCustomer(id: string, reason: string): Observable<void> {
    this.store.dispatch(deactivateCustomer({ id, reason }));
    return this.waitForAction(deactivateCustomerSuccess, deactivateCustomerFailure);
  }

  private waitForAction(success: any, failure: any): Observable<void> {
    return this.actions$.pipe(
      ofType(success, failure),
      take(1),
      map(action => {
        if (action.type === failure.type) {
          throw new Error(action.error);
        }
      })
    );
  }
}
```

---

## 6. Routing

### 6.1 Route Configuration
```typescript
const routes: Routes = [
  {
    path: 'customers',
    component: CustomerManagementLayoutComponent,
    canActivate: [AuthGuard, CustomerManagementGuard],
    children: [
      {
        path: '',
        component: CustomerListComponent,
        data: { title: 'Customers' }
      },
      {
        path: 'new',
        component: CustomerFormComponent,
        data: { title: 'New Customer', mode: 'create' }
      },
      {
        path: ':id',
        component: CustomerDetailComponent,
        data: { title: 'Customer Detail' },
        resolve: { customer: CustomerResolver }
      },
      {
        path: ':id/edit',
        component: CustomerFormComponent,
        data: { title: 'Edit Customer', mode: 'edit' },
        resolve: { customer: CustomerResolver }
      },
      {
        path: 'dashboard',
        component: CustomerDashboardComponent,
        data: { title: 'Customer Dashboard' }
      }
    ]
  }
];
```

### 6.2 Route Guards
```typescript
@Injectable()
export class CustomerManagementGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return this.authService.hasPermission('customer-management').pipe(
      map(hasPermission => {
        if (!hasPermission) {
          this.router.navigate(['/unauthorized']);
          return false;
        }
        return true;
      })
    );
  }
}
```

---

## 7. UI/UX Design

### 7.1 Color Scheme
```scss
$primary: #3f51b5;
$accent: #ff4081;
$warn: #f44336;
$success: #4caf50;
$info: #2196f3;
$warning: #ff9800;
```

### 7.2 Typography
```scss
$font-family: 'Roboto', 'Helvetica Neue', sans-serif;
$font-size-base: 14px;
$font-size-large: 16px;
$font-size-small: 12px;
$line-height-base: 1.5;
```

### 7.3 Responsive Breakpoints
```scss
$breakpoints: (
  xs: 0,
  sm: 600px,
  md: 960px,
  lg: 1280px,
  xl: 1920px
);
```

### 7.4 Material Theme
```scss
@use '@angular/material' as mat;

$custom-primary: mat.define-palette(mat.$indigo-palette);
$custom-accent: mat.define-palette(mat.$pink-palette, A200, A100, A400);
$custom-warn: mat.define-palette(mat.$red-palette);

$custom-theme: mat.define-light-theme((
  color: (
    primary: $custom-primary,
    accent: $custom-accent,
    warn: $custom-warn,
  ),
  typography: mat.define-typography-config(),
  density: 0,
));

@include mat.all-component-themes($custom-theme);
```

---

## 8. Forms and Validation

### 8.1 Custom Validators
```typescript
export class CustomerValidators {
  static uniqueEmail(customerService: CustomerService): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      if (!control.value) {
        return of(null);
      }

      return customerService.checkEmailExists(control.value).pipe(
        map(exists => exists ? { emailTaken: true } : null),
        catchError(() => of(null))
      );
    };
  }

  static phoneNumber(control: AbstractControl): ValidationErrors | null {
    const phoneRegex = /^\+?[1-9]\d{1,14}$/;
    return phoneRegex.test(control.value) ? null : { invalidPhone: true };
  }
}
```

### 8.2 Form Error Messages
```typescript
export const FORM_ERROR_MESSAGES = {
  required: 'This field is required',
  email: 'Please enter a valid email address',
  minlength: 'Minimum length is {requiredLength} characters',
  maxlength: 'Maximum length is {requiredLength} characters',
  pattern: 'Invalid format',
  emailTaken: 'This email is already registered',
  invalidPhone: 'Please enter a valid phone number'
};
```

---

## 9. Data Services

### 9.1 Customer Service
```typescript
@Injectable()
export class CustomerService {
  private apiUrl = `${environment.apiUrl}/api/v1/customers`;

  constructor(private http: HttpClient) {}

  getCustomers(params: CustomerQueryParams): Observable<PagedResponse<Customer>> {
    const httpParams = new HttpParams({ fromObject: params as any });
    return this.http.get<PagedResponse<Customer>>(this.apiUrl, { params: httpParams });
  }

  getCustomerById(id: string): Observable<Customer> {
    return this.http.get<Customer>(`${this.apiUrl}/${id}`);
  }

  createCustomer(customer: CreateCustomerDto): Observable<Customer> {
    return this.http.post<Customer>(this.apiUrl, customer);
  }

  updateCustomer(id: string, changes: Partial<Customer>): Observable<Customer> {
    return this.http.put<Customer>(`${this.apiUrl}/${id}`, changes);
  }

  deactivateCustomer(id: string, reason: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/deactivate`, { reason });
  }

  mergeCustomers(sourceId: string, targetId: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/merge`, {
      sourceCustomerId: sourceId,
      targetCustomerId: targetId
    });
  }

  exportCustomers(filters: CustomerFilters): Observable<Blob> {
    return this.http.post(`${this.apiUrl}/export`, filters, {
      responseType: 'blob'
    });
  }

  getCustomerInsights(id: string): Observable<CustomerInsights> {
    return this.http.get<CustomerInsights>(`${this.apiUrl}/${id}/insights`);
  }
}
```

---

## 10. Real-time Features

### 10.1 SignalR Integration
```typescript
@Injectable()
export class CustomerRealtimeService {
  private hubConnection: signalR.HubConnection;

  constructor(
    private authService: AuthService,
    private store: Store
  ) {}

  startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/hubs/customer`, {
        accessTokenFactory: () => this.authService.getAccessToken()
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
      .then(() => console.log('SignalR Connected'))
      .catch(err => console.error('SignalR Error:', err));

    this.setupEventListeners();
  }

  private setupEventListeners(): void {
    this.hubConnection.on('CustomerUpdated', (customer: Customer) => {
      this.store.dispatch(customerUpdatedFromHub({ customer }));
    });

    this.hubConnection.on('CommunicationReceived', (communication: Communication) => {
      this.store.dispatch(communicationReceivedFromHub({ communication }));
    });

    this.hubConnection.on('ComplaintStatusChanged', (update: ComplaintUpdate) => {
      this.store.dispatch(complaintStatusChangedFromHub({ update }));
    });
  }

  stopConnection(): void {
    this.hubConnection?.stop();
  }
}
```

---

## 11. Accessibility

### 11.1 ARIA Attributes
- All interactive elements have proper ARIA labels
- Form fields have associated labels
- Error messages are announced to screen readers
- Focus management for modals and dialogs

### 11.2 Keyboard Navigation
- Tab order follows logical flow
- All actions accessible via keyboard
- Escape key closes modals
- Enter key submits forms

### 11.3 Color Contrast
- WCAG AA compliance (4.5:1 for normal text)
- Status indicators use icons in addition to color

---

## 12. Performance Optimization

### 12.1 Strategies
- Lazy loading of feature modules
- OnPush change detection strategy
- Virtual scrolling for large lists
- Image lazy loading
- Service worker for caching
- Memoization of selectors
- Pagination for data-heavy views

### 12.2 Bundle Size
- Target: < 500KB initial bundle
- Lazy loaded chunks: < 200KB each

---

## 13. Testing Strategy

### 13.1 Unit Tests
```typescript
describe('CustomerListComponent', () => {
  let component: CustomerListComponent;
  let fixture: ComponentFixture<CustomerListComponent>;
  let mockFacade: jasmine.SpyObj<CustomerFacade>;

  beforeEach(() => {
    mockFacade = jasmine.createSpyObj('CustomerFacade', ['loadCustomers']);

    TestBed.configureTestingModule({
      declarations: [CustomerListComponent],
      providers: [
        { provide: CustomerFacade, useValue: mockFacade }
      ]
    });

    fixture = TestBed.createComponent(CustomerListComponent);
    component = fixture.componentInstance;
  });

  it('should load customers on init', () => {
    component.ngOnInit();
    expect(mockFacade.loadCustomers).toHaveBeenCalled();
  });
});
```

### 13.2 E2E Tests (Cypress)
```typescript
describe('Customer Management', () => {
  beforeEach(() => {
    cy.login('admin@example.com', 'password');
    cy.visit('/customers');
  });

  it('should create a new customer', () => {
    cy.get('[data-cy=new-customer-btn]').click();
    cy.get('[data-cy=company-name]').type('Test Company');
    cy.get('[data-cy=email]').type('test@company.com');
    cy.get('[data-cy=phone]').type('+1234567890');
    cy.get('[data-cy=submit-btn]').click();
    cy.contains('Customer created successfully');
  });
});
```

---

## 14. Deployment

### 14.1 Build Configuration
```typescript
export const environment = {
  production: true,
  apiUrl: 'https://api.eventplatform.com',
  signalRUrl: 'https://api.eventplatform.com/hubs',
  azureAdConfig: {
    clientId: 'YOUR_CLIENT_ID',
    authority: 'YOUR_AUTHORITY',
    redirectUri: 'https://app.eventplatform.com'
  }
};
```

### 14.2 CI/CD Pipeline
1. Build: `ng build --configuration production`
2. Test: `ng test --watch=false --code-coverage`
3. Lint: `ng lint`
4. Deploy to Azure Static Web Apps

---

## Document History

| Version | Date | Author | Description |
|---------|------|--------|-------------|
| 1.0.0 | 2025-12-22 | Frontend Architect | Initial version |
