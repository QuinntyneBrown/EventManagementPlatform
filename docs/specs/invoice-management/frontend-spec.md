# Invoice & Financial Management - Frontend Specification

## 1. Executive Summary

### 1.1 Purpose
This Software Requirements Specification (SRS) document describes the frontend architecture and implementation requirements for the Invoice & Financial Management feature of the Event Management Platform. The system provides a comprehensive user interface for managing invoices, payments, refunds, and financial reporting using Angular 18+ and Angular Material.

### 1.2 Scope
The Invoice & Financial Management frontend provides:
- Intuitive invoice creation and management interface
- Real-time invoice preview and PDF generation
- Payment processing with multiple payment methods
- Refund management workflow
- Financial dashboards and reporting
- Responsive design for desktop, tablet, and mobile
- Accessibility compliance (WCAG 2.1 AA)

### 1.3 Target Audience
- Frontend Developers
- UI/UX Designers
- QA Engineers
- Product Managers

## 2. Technology Stack

### 2.1 Core Framework
- **Angular 18.x**: Primary frontend framework
- **TypeScript 5.3+**: Programming language
- **RxJS 7.8+**: Reactive programming
- **Angular Material 18.x**: UI component library
- **Angular CDK**: Component development utilities

### 2.2 Additional Libraries
- **Angular Router**: Navigation and routing
- **Angular Forms**: Reactive forms
- **NgRx**: State management
- **NgRx Effects**: Side effects management
- **NgRx Router Store**: Router state integration
- **Chart.js / ng2-charts**: Data visualization
- **ngx-stripe**: Stripe payment integration
- **jsPDF**: Client-side PDF generation
- **date-fns**: Date manipulation
- **numeral.js**: Number formatting
- **ngx-currency**: Currency input formatting
- **Angular CDK Virtual Scroll**: Large list rendering
- **Angular CDK Drag and Drop**: Item reordering

### 2.3 Development Tools
- **Angular CLI 18.x**: Project scaffolding and build
- **ESLint**: Linting
- **Prettier**: Code formatting
- **Jasmine/Karma**: Unit testing
- **Cypress**: E2E testing
- **Compodoc**: Documentation generation
- **Nx**: Monorepo management (optional)

## 3. Application Architecture

### 3.1 Module Structure

```
app/
├── core/
│   ├── auth/
│   ├── interceptors/
│   ├── guards/
│   ├── services/
│   └── models/
├── shared/
│   ├── components/
│   ├── directives/
│   ├── pipes/
│   └── utils/
├── features/
│   └── invoice-management/
│       ├── components/
│       │   ├── invoice-list/
│       │   ├── invoice-detail/
│       │   ├── invoice-form/
│       │   ├── invoice-items/
│       │   ├── payment-form/
│       │   ├── payment-history/
│       │   ├── refund-form/
│       │   ├── financial-dashboard/
│       │   └── aging-report/
│       ├── services/
│       │   ├── invoice.service.ts
│       │   ├── payment.service.ts
│       │   ├── refund.service.ts
│       │   └── financial.service.ts
│       ├── store/
│       │   ├── actions/
│       │   ├── reducers/
│       │   ├── effects/
│       │   ├── selectors/
│       │   └── models/
│       └── invoice-management-routing.module.ts
└── app-routing.module.ts
```

### 3.2 State Management with NgRx

#### State Structure
```typescript
export interface InvoiceState {
  invoices: {
    entities: { [id: string]: Invoice };
    ids: string[];
    selectedInvoiceId: string | null;
    loading: boolean;
    error: string | null;
    filters: InvoiceFilters;
    pagination: Pagination;
  };
  payments: {
    entities: { [id: string]: Payment };
    ids: string[];
    loading: boolean;
    error: string | null;
  };
  refunds: {
    entities: { [id: string]: Refund };
    ids: string[];
    loading: boolean;
    error: string | null;
  };
  financialData: {
    dashboard: FinancialDashboard | null;
    agingReport: AgingReport | null;
    loading: boolean;
    error: string | null;
  };
}
```

#### Actions
```typescript
// Invoice Actions
export const loadInvoices = createAction(
  '[Invoice List] Load Invoices',
  props<{ filters?: InvoiceFilters; page?: number }>()
);

export const loadInvoicesSuccess = createAction(
  '[Invoice API] Load Invoices Success',
  props<{ invoices: Invoice[]; totalCount: number }>()
);

export const loadInvoicesFailure = createAction(
  '[Invoice API] Load Invoices Failure',
  props<{ error: string }>()
);

export const createInvoice = createAction(
  '[Invoice Form] Create Invoice',
  props<{ invoice: CreateInvoiceRequest }>()
);

export const createInvoiceSuccess = createAction(
  '[Invoice API] Create Invoice Success',
  props<{ invoice: Invoice }>()
);

export const addInvoiceItem = createAction(
  '[Invoice Items] Add Item',
  props<{ invoiceId: string; item: InvoiceItemRequest }>()
);

export const finalizeInvoice = createAction(
  '[Invoice Detail] Finalize Invoice',
  props<{ invoiceId: string }>()
);

export const sendInvoice = createAction(
  '[Invoice Detail] Send Invoice',
  props<{ invoiceId: string; request: SendInvoiceRequest }>()
);

// Payment Actions
export const processPayment = createAction(
  '[Payment Form] Process Payment',
  props<{ payment: PaymentRequest }>()
);

export const processPaymentSuccess = createAction(
  '[Payment API] Process Payment Success',
  props<{ payment: Payment }>()
);

export const refundPayment = createAction(
  '[Refund Form] Refund Payment',
  props<{ paymentId: string; refund: RefundRequest }>()
);
```

#### Selectors
```typescript
export const selectInvoiceState = createFeatureSelector<InvoiceState>('invoice');

export const selectAllInvoices = createSelector(
  selectInvoiceState,
  (state) => state.invoices.ids.map(id => state.invoices.entities[id])
);

export const selectSelectedInvoice = createSelector(
  selectInvoiceState,
  (state) => state.invoices.selectedInvoiceId
    ? state.invoices.entities[state.invoices.selectedInvoiceId]
    : null
);

export const selectInvoicesByStatus = (status: string) => createSelector(
  selectAllInvoices,
  (invoices) => invoices.filter(inv => inv.status === status)
);

export const selectFinancialDashboard = createSelector(
  selectInvoiceState,
  (state) => state.financialData.dashboard
);

export const selectIsLoading = createSelector(
  selectInvoiceState,
  (state) => state.invoices.loading ||
             state.payments.loading ||
             state.financialData.loading
);
```

## 4. User Interface Components

### 4.1 Invoice List Component

#### Component Structure
```typescript
@Component({
  selector: 'app-invoice-list',
  templateUrl: './invoice-list.component.html',
  styleUrls: ['./invoice-list.component.scss']
})
export class InvoiceListComponent implements OnInit {
  invoices$ = this.store.select(selectAllInvoices);
  loading$ = this.store.select(selectIsLoading);
  pagination$ = this.store.select(selectPagination);

  displayedColumns = [
    'invoiceNumber',
    'customerName',
    'eventName',
    'issueDate',
    'dueDate',
    'totalAmount',
    'amountDue',
    'status',
    'actions'
  ];

  filterForm = this.fb.group({
    status: [''],
    eventId: [''],
    customerId: [''],
    dateRange: this.fb.group({
      start: [''],
      end: ['']
    }),
    searchTerm: ['']
  });

  constructor(
    private store: Store,
    private fb: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadInvoices();
    this.setupFilterListener();
  }

  loadInvoices(page: number = 1): void {
    const filters = this.filterForm.value;
    this.store.dispatch(loadInvoices({ filters, page }));
  }

  setupFilterListener(): void {
    this.filterForm.valueChanges
      .pipe(debounceTime(300))
      .subscribe(() => this.loadInvoices());
  }

  viewInvoice(invoice: Invoice): void {
    this.router.navigate(['/invoices', invoice.invoiceId]);
  }

  createInvoice(): void {
    this.router.navigate(['/invoices/new']);
  }

  sendInvoice(invoice: Invoice): void {
    // Open send invoice dialog
  }

  downloadPdf(invoice: Invoice): void {
    window.open(invoice.pdfUrl, '_blank');
  }

  getStatusColor(status: string): string {
    const colors = {
      'Draft': 'accent',
      'Finalized': 'primary',
      'Paid': 'success',
      'PartiallyPaid': 'warn',
      'PastDue': 'error',
      'Voided': 'disabled',
      'WrittenOff': 'disabled'
    };
    return colors[status] || 'primary';
  }
}
```

#### Template
```html
<div class="invoice-list-container">
  <mat-toolbar color="primary">
    <h1>Invoices</h1>
    <span class="spacer"></span>
    <button mat-raised-button color="accent" (click)="createInvoice()">
      <mat-icon>add</mat-icon>
      Create Invoice
    </button>
  </mat-toolbar>

  <!-- Filters -->
  <mat-card class="filter-card">
    <mat-card-content>
      <form [formGroup]="filterForm" class="filter-form">
        <mat-form-field appearance="outline">
          <mat-label>Search</mat-label>
          <input matInput formControlName="searchTerm"
                 placeholder="Invoice number, customer name...">
          <mat-icon matPrefix>search</mat-icon>
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Status</mat-label>
          <mat-select formControlName="status">
            <mat-option value="">All</mat-option>
            <mat-option value="Draft">Draft</mat-option>
            <mat-option value="Finalized">Finalized</mat-option>
            <mat-option value="Paid">Paid</mat-option>
            <mat-option value="PartiallyPaid">Partially Paid</mat-option>
            <mat-option value="PastDue">Past Due</mat-option>
            <mat-option value="Voided">Voided</mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Date Range</mat-label>
          <mat-date-range-input [rangePicker]="picker" formGroupName="dateRange">
            <input matStartDate formControlName="start" placeholder="Start date">
            <input matEndDate formControlName="end" placeholder="End date">
          </mat-date-range-input>
          <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
          <mat-date-range-picker #picker></mat-date-range-picker>
        </mat-form-field>
      </form>
    </mat-card-content>
  </mat-card>

  <!-- Invoice Table -->
  <mat-card>
    <mat-card-content>
      <div class="table-container">
        <table mat-table [dataSource]="invoices$ | async" class="invoice-table">
          <!-- Invoice Number Column -->
          <ng-container matColumnDef="invoiceNumber">
            <th mat-header-cell *matHeaderCellDef>Invoice #</th>
            <td mat-cell *matCellDef="let invoice">
              <a [routerLink]="['/invoices', invoice.invoiceId]">
                {{ invoice.invoiceNumber || 'DRAFT' }}
              </a>
            </td>
          </ng-container>

          <!-- Customer Column -->
          <ng-container matColumnDef="customerName">
            <th mat-header-cell *matHeaderCellDef>Customer</th>
            <td mat-cell *matCellDef="let invoice">{{ invoice.customerName }}</td>
          </ng-container>

          <!-- Event Column -->
          <ng-container matColumnDef="eventName">
            <th mat-header-cell *matHeaderCellDef>Event</th>
            <td mat-cell *matCellDef="let invoice">{{ invoice.eventName }}</td>
          </ng-container>

          <!-- Issue Date Column -->
          <ng-container matColumnDef="issueDate">
            <th mat-header-cell *matHeaderCellDef>Issue Date</th>
            <td mat-cell *matCellDef="let invoice">
              {{ invoice.issueDate | date:'shortDate' }}
            </td>
          </ng-container>

          <!-- Due Date Column -->
          <ng-container matColumnDef="dueDate">
            <th mat-header-cell *matHeaderCellDef>Due Date</th>
            <td mat-cell *matCellDef="let invoice">
              {{ invoice.dueDate | date:'shortDate' }}
            </td>
          </ng-container>

          <!-- Total Amount Column -->
          <ng-container matColumnDef="totalAmount">
            <th mat-header-cell *matHeaderCellDef>Total</th>
            <td mat-cell *matCellDef="let invoice">
              {{ invoice.totalAmount.amount | currency:invoice.totalAmount.currency }}
            </td>
          </ng-container>

          <!-- Amount Due Column -->
          <ng-container matColumnDef="amountDue">
            <th mat-header-cell *matHeaderCellDef>Amount Due</th>
            <td mat-cell *matCellDef="let invoice">
              <span [class.amount-overdue]="invoice.status === 'PastDue'">
                {{ invoice.amountDue.amount | currency:invoice.amountDue.currency }}
              </span>
            </td>
          </ng-container>

          <!-- Status Column -->
          <ng-container matColumnDef="status">
            <th mat-header-cell *matHeaderCellDef>Status</th>
            <td mat-cell *matCellDef="let invoice">
              <mat-chip [color]="getStatusColor(invoice.status)" selected>
                {{ invoice.status }}
              </mat-chip>
            </td>
          </ng-container>

          <!-- Actions Column -->
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let invoice">
              <button mat-icon-button [matMenuTriggerFor]="menu">
                <mat-icon>more_vert</mat-icon>
              </button>
              <mat-menu #menu="matMenu">
                <button mat-menu-item (click)="viewInvoice(invoice)">
                  <mat-icon>visibility</mat-icon>
                  <span>View</span>
                </button>
                <button mat-menu-item (click)="sendInvoice(invoice)"
                        [disabled]="invoice.status === 'Draft'">
                  <mat-icon>send</mat-icon>
                  <span>Send</span>
                </button>
                <button mat-menu-item (click)="downloadPdf(invoice)"
                        [disabled]="!invoice.pdfUrl">
                  <mat-icon>download</mat-icon>
                  <span>Download PDF</span>
                </button>
              </mat-menu>
            </td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
        </table>
      </div>

      <!-- Pagination -->
      <mat-paginator
        [length]="(pagination$ | async)?.totalCount"
        [pageSize]="(pagination$ | async)?.pageSize"
        [pageSizeOptions]="[10, 20, 50, 100]"
        (page)="loadInvoices($event.pageIndex + 1)">
      </mat-paginator>
    </mat-card-content>
  </mat-card>

  <!-- Loading Spinner -->
  <div class="loading-overlay" *ngIf="loading$ | async">
    <mat-spinner></mat-spinner>
  </div>
</div>
```

### 4.2 Invoice Form Component

#### Component Structure
```typescript
@Component({
  selector: 'app-invoice-form',
  templateUrl: './invoice-form.component.html',
  styleUrls: ['./invoice-form.component.scss']
})
export class InvoiceFormComponent implements OnInit, OnDestroy {
  invoiceForm = this.fb.group({
    eventId: ['', Validators.required],
    customerId: ['', Validators.required],
    dueDate: ['', Validators.required],
    billingAddress: this.fb.group({
      street: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      postalCode: ['', Validators.required],
      country: ['USA', Validators.required]
    }),
    notes: [''],
    terms: ['Net 30']
  });

  events$ = this.eventService.getEvents();
  customers$ = this.customerService.getCustomers();

  private destroy$ = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private store: Store,
    private eventService: EventService,
    private customerService: CustomerService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.setupCustomerSelection();
  }

  setupCustomerSelection(): void {
    this.invoiceForm.get('customerId')?.valueChanges
      .pipe(
        takeUntil(this.destroy$),
        switchMap(customerId =>
          this.customerService.getCustomer(customerId)
        )
      )
      .subscribe(customer => {
        if (customer?.billingAddress) {
          this.invoiceForm.patchValue({
            billingAddress: customer.billingAddress
          });
        }
      });
  }

  onSubmit(): void {
    if (this.invoiceForm.valid) {
      const invoice = this.invoiceForm.value;
      this.store.dispatch(createInvoice({ invoice }));

      // Navigate to invoice detail after creation
      this.store.select(selectSelectedInvoice)
        .pipe(
          filter(inv => inv !== null),
          take(1)
        )
        .subscribe(invoice => {
          this.router.navigate(['/invoices', invoice.invoiceId]);
        });
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
```

### 4.3 Invoice Detail Component

#### Component Structure
```typescript
@Component({
  selector: 'app-invoice-detail',
  templateUrl: './invoice-detail.component.html',
  styleUrls: ['./invoice-detail.component.scss']
})
export class InvoiceDetailComponent implements OnInit {
  invoice$ = this.store.select(selectSelectedInvoice);
  payments$ = this.store.select(selectPaymentsByInvoice);
  canEdit$ = this.invoice$.pipe(
    map(invoice => invoice?.status === 'Draft')
  );

  constructor(
    private route: ActivatedRoute,
    private store: Store,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    const invoiceId = this.route.snapshot.params['id'];
    this.store.dispatch(loadInvoiceDetail({ invoiceId }));
  }

  addItem(): void {
    this.invoice$.pipe(take(1)).subscribe(invoice => {
      const dialogRef = this.dialog.open(AddInvoiceItemDialogComponent, {
        width: '600px',
        data: { invoiceId: invoice.invoiceId }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.store.dispatch(addInvoiceItem({
            invoiceId: invoice.invoiceId,
            item: result
          }));
        }
      });
    });
  }

  removeItem(itemId: string): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Remove Item',
        message: 'Are you sure you want to remove this item?'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.invoice$.pipe(take(1)).subscribe(invoice => {
          this.store.dispatch(removeInvoiceItem({
            invoiceId: invoice.invoiceId,
            itemId
          }));
        });
      }
    });
  }

  applyDiscount(): void {
    this.invoice$.pipe(take(1)).subscribe(invoice => {
      const dialogRef = this.dialog.open(ApplyDiscountDialogComponent, {
        width: '500px',
        data: { invoice }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.store.dispatch(applyDiscount({
            invoiceId: invoice.invoiceId,
            discount: result
          }));
        }
      });
    });
  }

  calculateTax(): void {
    this.invoice$.pipe(take(1)).subscribe(invoice => {
      const dialogRef = this.dialog.open(CalculateTaxDialogComponent, {
        width: '500px',
        data: { invoice }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.store.dispatch(calculateTax({
            invoiceId: invoice.invoiceId,
            taxRate: result.taxRate,
            jurisdiction: result.jurisdiction
          }));
        }
      });
    });
  }

  finalizeInvoice(): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Finalize Invoice',
        message: 'Once finalized, the invoice cannot be edited. Continue?'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.invoice$.pipe(take(1)).subscribe(invoice => {
          this.store.dispatch(finalizeInvoice({
            invoiceId: invoice.invoiceId
          }));
        });
      }
    });
  }

  sendInvoice(): void {
    this.invoice$.pipe(take(1)).subscribe(invoice => {
      const dialogRef = this.dialog.open(SendInvoiceDialogComponent, {
        width: '600px',
        data: { invoice }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.store.dispatch(sendInvoice({
            invoiceId: invoice.invoiceId,
            request: result
          }));
        }
      });
    });
  }

  processPayment(): void {
    this.invoice$.pipe(take(1)).subscribe(invoice => {
      const dialogRef = this.dialog.open(PaymentFormComponent, {
        width: '700px',
        data: { invoice }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.store.dispatch(processPayment({ payment: result }));
        }
      });
    });
  }

  voidInvoice(): void {
    const dialogRef = this.dialog.open(VoidInvoiceDialogComponent, {
      width: '500px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.invoice$.pipe(take(1)).subscribe(invoice => {
          this.store.dispatch(voidInvoice({
            invoiceId: invoice.invoiceId,
            reason: result.reason
          }));
        });
      }
    });
  }

  downloadPdf(): void {
    this.invoice$.pipe(take(1)).subscribe(invoice => {
      if (invoice.pdfUrl) {
        window.open(invoice.pdfUrl, '_blank');
      }
    });
  }

  printInvoice(): void {
    this.invoice$.pipe(take(1)).subscribe(invoice => {
      this.store.dispatch(printInvoice({
        invoiceId: invoice.invoiceId
      }));
      window.print();
    });
  }
}
```

#### Template (Partial)
```html
<div class="invoice-detail-container" *ngIf="invoice$ | async as invoice">
  <!-- Header -->
  <mat-toolbar color="primary">
    <button mat-icon-button routerLink="/invoices">
      <mat-icon>arrow_back</mat-icon>
    </button>
    <h1>Invoice {{ invoice.invoiceNumber || 'DRAFT' }}</h1>
    <span class="spacer"></span>
    <mat-chip [color]="getStatusColor(invoice.status)" selected>
      {{ invoice.status }}
    </mat-chip>
  </mat-toolbar>

  <!-- Action Bar -->
  <mat-card class="action-bar">
    <mat-card-content>
      <div class="action-buttons">
        <button mat-raised-button color="primary"
                (click)="finalizeInvoice()"
                *ngIf="invoice.status === 'Draft'">
          <mat-icon>check_circle</mat-icon>
          Finalize Invoice
        </button>

        <button mat-raised-button color="accent"
                (click)="sendInvoice()"
                [disabled]="invoice.status === 'Draft'">
          <mat-icon>send</mat-icon>
          Send Invoice
        </button>

        <button mat-raised-button
                (click)="processPayment()"
                [disabled]="invoice.status === 'Draft' || invoice.amountDue.amount <= 0">
          <mat-icon>payment</mat-icon>
          Record Payment
        </button>

        <button mat-button (click)="downloadPdf()" [disabled]="!invoice.pdfUrl">
          <mat-icon>download</mat-icon>
          Download PDF
        </button>

        <button mat-button (click)="printInvoice()">
          <mat-icon>print</mat-icon>
          Print
        </button>

        <button mat-button color="warn"
                (click)="voidInvoice()"
                [disabled]="invoice.status === 'Voided' || invoice.status === 'Paid'">
          <mat-icon>cancel</mat-icon>
          Void
        </button>
      </div>
    </mat-card-content>
  </mat-card>

  <div class="invoice-content">
    <!-- Invoice Information -->
    <mat-card class="invoice-info-card">
      <mat-card-header>
        <mat-card-title>Invoice Information</mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <div class="info-grid">
          <div class="info-item">
            <label>Invoice Number:</label>
            <span>{{ invoice.invoiceNumber || 'Not assigned' }}</span>
          </div>
          <div class="info-item">
            <label>Issue Date:</label>
            <span>{{ invoice.issueDate | date:'mediumDate' }}</span>
          </div>
          <div class="info-item">
            <label>Due Date:</label>
            <span>{{ invoice.dueDate | date:'mediumDate' }}</span>
          </div>
          <div class="info-item">
            <label>Event:</label>
            <span>{{ invoice.eventName }}</span>
          </div>
          <div class="info-item">
            <label>Customer:</label>
            <span>{{ invoice.customerName }}</span>
          </div>
        </div>

        <mat-divider></mat-divider>

        <div class="billing-address">
          <h3>Billing Address</h3>
          <p>{{ invoice.billingAddress.street }}</p>
          <p>{{ invoice.billingAddress.city }}, {{ invoice.billingAddress.state }} {{ invoice.billingAddress.postalCode }}</p>
          <p>{{ invoice.billingAddress.country }}</p>
        </div>
      </mat-card-content>
    </mat-card>

    <!-- Invoice Items -->
    <mat-card class="invoice-items-card">
      <mat-card-header>
        <mat-card-title>Items</mat-card-title>
        <button mat-icon-button (click)="addItem()"
                *ngIf="canEdit$ | async">
          <mat-icon>add</mat-icon>
        </button>
      </mat-card-header>
      <mat-card-content>
        <table mat-table [dataSource]="invoice.items" class="items-table">
          <!-- Description Column -->
          <ng-container matColumnDef="description">
            <th mat-header-cell *matHeaderCellDef>Description</th>
            <td mat-cell *matCellDef="let item">
              <div class="item-description">
                <strong>{{ item.description }}</strong>
                <span class="fee-type-badge">{{ item.feeType }}</span>
              </div>
            </td>
          </ng-container>

          <!-- Quantity Column -->
          <ng-container matColumnDef="quantity">
            <th mat-header-cell *matHeaderCellDef>Quantity</th>
            <td mat-cell *matCellDef="let item">{{ item.quantity }}</td>
          </ng-container>

          <!-- Unit Price Column -->
          <ng-container matColumnDef="unitPrice">
            <th mat-header-cell *matHeaderCellDef>Unit Price</th>
            <td mat-cell *matCellDef="let item">
              {{ item.unitPrice.amount | currency:item.unitPrice.currency }}
            </td>
          </ng-container>

          <!-- Line Total Column -->
          <ng-container matColumnDef="lineTotal">
            <th mat-header-cell *matHeaderCellDef>Total</th>
            <td mat-cell *matCellDef="let item">
              {{ item.lineTotal.amount | currency:item.lineTotal.currency }}
            </td>
          </ng-container>

          <!-- Actions Column -->
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef></th>
            <td mat-cell *matCellDef="let item">
              <button mat-icon-button (click)="removeItem(item.itemId)"
                      *ngIf="canEdit$ | async">
                <mat-icon>delete</mat-icon>
              </button>
            </td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="['description', 'quantity', 'unitPrice', 'lineTotal', 'actions']"></tr>
          <tr mat-row *matRowDef="let row; columns: ['description', 'quantity', 'unitPrice', 'lineTotal', 'actions'];"></tr>
        </table>

        <!-- Totals Section -->
        <div class="totals-section">
          <div class="total-row">
            <span>Subtotal:</span>
            <span>{{ invoice.subtotal.amount | currency:invoice.subtotal.currency }}</span>
          </div>

          <div class="total-row" *ngIf="invoice.discountAmount.amount > 0">
            <span>
              Discount
              <button mat-icon-button (click)="removeDiscount()" *ngIf="canEdit$ | async">
                <mat-icon>close</mat-icon>
              </button>
            </span>
            <span class="discount-amount">
              -{{ invoice.discountAmount.amount | currency:invoice.discountAmount.currency }}
            </span>
          </div>

          <div class="total-row" *ngIf="invoice.taxAmount.amount > 0">
            <span>Tax ({{ invoice.taxRate }}%):</span>
            <span>{{ invoice.taxAmount.amount | currency:invoice.taxAmount.currency }}</span>
          </div>

          <mat-divider></mat-divider>

          <div class="total-row total-amount">
            <span><strong>Total:</strong></span>
            <span><strong>{{ invoice.totalAmount.amount | currency:invoice.totalAmount.currency }}</strong></span>
          </div>

          <div class="total-row" *ngIf="invoice.amountPaid.amount > 0">
            <span>Amount Paid:</span>
            <span class="paid-amount">
              {{ invoice.amountPaid.amount | currency:invoice.amountPaid.currency }}
            </span>
          </div>

          <div class="total-row amount-due">
            <span><strong>Amount Due:</strong></span>
            <span><strong>{{ invoice.amountDue.amount | currency:invoice.amountDue.currency }}</strong></span>
          </div>
        </div>

        <!-- Action buttons for totals -->
        <div class="total-actions" *ngIf="canEdit$ | async">
          <button mat-button (click)="applyDiscount()">
            <mat-icon>local_offer</mat-icon>
            Apply Discount
          </button>
          <button mat-button (click)="calculateTax()">
            <mat-icon>calculate</mat-icon>
            Calculate Tax
          </button>
        </div>
      </mat-card-content>
    </mat-card>

    <!-- Payment History -->
    <mat-card class="payment-history-card" *ngIf="(payments$ | async)?.length > 0">
      <mat-card-header>
        <mat-card-title>Payment History</mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <app-payment-history [payments]="payments$ | async"></app-payment-history>
      </mat-card-content>
    </mat-card>
  </div>
</div>
```

### 4.4 Payment Form Component

#### Component Structure
```typescript
@Component({
  selector: 'app-payment-form',
  templateUrl: './payment-form.component.html',
  styleUrls: ['./payment-form.component.scss']
})
export class PaymentFormComponent implements OnInit {
  paymentForm = this.fb.group({
    amount: [0, [Validators.required, Validators.min(0.01)]],
    paymentMethod: ['CreditCard', Validators.required],
    notes: ['']
  });

  creditCardForm = this.fb.group({
    cardNumber: ['', Validators.required],
    expiryDate: ['', Validators.required],
    cvv: ['', Validators.required],
    cardholderName: ['', Validators.required]
  });

  paymentMethods = [
    { value: 'CreditCard', label: 'Credit Card' },
    { value: 'BankTransfer', label: 'Bank Transfer' },
    { value: 'Check', label: 'Check' },
    { value: 'Cash', label: 'Cash' },
    { value: 'PayPal', label: 'PayPal' }
  ];

  processing = false;

  @Input() invoice: Invoice;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<PaymentFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private stripeService: StripeService
  ) {
    this.invoice = data.invoice;
  }

  ngOnInit(): void {
    this.paymentForm.patchValue({
      amount: this.invoice.amountDue.amount
    });
  }

  async processPayment(): Promise<void> {
    if (this.paymentForm.invalid) return;

    this.processing = true;

    try {
      const paymentMethod = this.paymentForm.value.paymentMethod;

      if (paymentMethod === 'CreditCard') {
        await this.processCreditCard();
      } else {
        await this.processOtherMethods();
      }
    } catch (error) {
      console.error('Payment processing error:', error);
      // Show error message
    } finally {
      this.processing = false;
    }
  }

  private async processCreditCard(): Promise<void> {
    if (this.creditCardForm.invalid) return;

    // Create Stripe token
    const cardData = this.creditCardForm.value;
    const token = await this.stripeService.createToken({
      number: cardData.cardNumber,
      exp_month: cardData.expiryDate.split('/')[0],
      exp_year: cardData.expiryDate.split('/')[1],
      cvc: cardData.cvv,
      name: cardData.cardholderName
    });

    const paymentRequest = {
      invoiceId: this.invoice.invoiceId,
      amount: {
        amount: this.paymentForm.value.amount,
        currency: this.invoice.totalAmount.currency
      },
      paymentMethod: 'CreditCard',
      paymentGateway: 'Stripe',
      paymentDetails: {
        stripeToken: token.id
      },
      notes: this.paymentForm.value.notes
    };

    this.dialogRef.close(paymentRequest);
  }

  private async processOtherMethods(): Promise<void> {
    const paymentRequest = {
      invoiceId: this.invoice.invoiceId,
      amount: {
        amount: this.paymentForm.value.amount,
        currency: this.invoice.totalAmount.currency
      },
      paymentMethod: this.paymentForm.value.paymentMethod,
      notes: this.paymentForm.value.notes
    };

    this.dialogRef.close(paymentRequest);
  }

  cancel(): void {
    this.dialogRef.close();
  }
}
```

### 4.5 Financial Dashboard Component

#### Component Structure
```typescript
@Component({
  selector: 'app-financial-dashboard',
  templateUrl: './financial-dashboard.component.html',
  styleUrls: ['./financial-dashboard.component.scss']
})
export class FinancialDashboardComponent implements OnInit {
  dashboard$ = this.store.select(selectFinancialDashboard);

  dateRangeForm = this.fb.group({
    startDate: [new Date(new Date().getFullYear(), 0, 1)],
    endDate: [new Date()]
  });

  // Chart configurations
  revenueChartData: ChartData<'line'>;
  revenueChartOptions: ChartOptions<'line'>;

  feeTypeChartData: ChartData<'doughnut'>;
  feeTypeChartOptions: ChartOptions<'doughnut'>;

  constructor(
    private store: Store,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.loadDashboard();
    this.setupDateRangeListener();
  }

  loadDashboard(): void {
    const { startDate, endDate } = this.dateRangeForm.value;
    this.store.dispatch(loadFinancialDashboard({ startDate, endDate }));

    this.dashboard$.pipe(
      filter(dashboard => dashboard !== null),
      take(1)
    ).subscribe(dashboard => {
      this.setupCharts(dashboard);
    });
  }

  setupDateRangeListener(): void {
    this.dateRangeForm.valueChanges
      .pipe(debounceTime(500))
      .subscribe(() => this.loadDashboard());
  }

  setupCharts(dashboard: FinancialDashboard): void {
    // Revenue by month chart
    this.revenueChartData = {
      labels: dashboard.revenueByMonth.map(r => r.month),
      datasets: [
        {
          label: 'Revenue',
          data: dashboard.revenueByMonth.map(r => r.amount.amount),
          borderColor: '#3f51b5',
          backgroundColor: 'rgba(63, 81, 181, 0.1)',
          fill: true
        }
      ]
    };

    this.revenueChartOptions = {
      responsive: true,
      plugins: {
        legend: { display: true },
        tooltip: {
          callbacks: {
            label: (context) => {
              return `Revenue: $${context.parsed.y.toLocaleString()}`;
            }
          }
        }
      }
    };

    // Fee type distribution chart
    this.feeTypeChartData = {
      labels: Object.keys(dashboard.revenueByFeeType),
      datasets: [
        {
          data: Object.values(dashboard.revenueByFeeType)
            .map((m: Money) => m.amount),
          backgroundColor: [
            '#3f51b5',
            '#f44336',
            '#4caf50',
            '#ff9800',
            '#9c27b0'
          ]
        }
      ]
    };

    this.feeTypeChartOptions = {
      responsive: true,
      plugins: {
        legend: { position: 'right' }
      }
    };
  }

  exportData(format: string): void {
    const { startDate, endDate } = this.dateRangeForm.value;
    this.store.dispatch(exportFinancialData({
      format,
      startDate,
      endDate
    }));
  }
}
```

#### Template
```html
<div class="financial-dashboard">
  <mat-toolbar color="primary">
    <h1>Financial Dashboard</h1>
  </mat-toolbar>

  <!-- Date Range Selector -->
  <mat-card class="date-range-card">
    <mat-card-content>
      <form [formGroup]="dateRangeForm" class="date-range-form">
        <mat-form-field appearance="outline">
          <mat-label>Start Date</mat-label>
          <input matInput [matDatepicker]="startPicker" formControlName="startDate">
          <mat-datepicker-toggle matSuffix [for]="startPicker"></mat-datepicker-toggle>
          <mat-datepicker #startPicker></mat-datepicker>
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>End Date</mat-label>
          <input matInput [matDatepicker]="endPicker" formControlName="endDate">
          <mat-datepicker-toggle matSuffix [for]="endPicker"></mat-datepicker-toggle>
          <mat-datepicker #endPicker></mat-datepicker>
        </mat-form-field>

        <button mat-raised-button color="accent" (click)="exportData('csv')">
          <mat-icon>download</mat-icon>
          Export CSV
        </button>
      </form>
    </mat-card-content>
  </mat-card>

  <div class="dashboard-content" *ngIf="dashboard$ | async as dashboard">
    <!-- KPI Cards -->
    <div class="kpi-grid">
      <mat-card class="kpi-card">
        <mat-card-content>
          <div class="kpi-icon revenue-icon">
            <mat-icon>attach_money</mat-icon>
          </div>
          <div class="kpi-details">
            <h3>Total Revenue</h3>
            <p class="kpi-value">
              {{ dashboard.totalRevenue.amount | currency:dashboard.totalRevenue.currency }}
            </p>
          </div>
        </mat-card-content>
      </mat-card>

      <mat-card class="kpi-card">
        <mat-card-content>
          <div class="kpi-icon paid-icon">
            <mat-icon>check_circle</mat-icon>
          </div>
          <div class="kpi-details">
            <h3>Total Paid</h3>
            <p class="kpi-value">
              {{ dashboard.totalPaid.amount | currency:dashboard.totalPaid.currency }}
            </p>
          </div>
        </mat-card-content>
      </mat-card>

      <mat-card class="kpi-card">
        <mat-card-content>
          <div class="kpi-icon outstanding-icon">
            <mat-icon>schedule</mat-icon>
          </div>
          <div class="kpi-details">
            <h3>Outstanding</h3>
            <p class="kpi-value">
              {{ dashboard.totalOutstanding.amount | currency:dashboard.totalOutstanding.currency }}
            </p>
          </div>
        </mat-card-content>
      </mat-card>

      <mat-card class="kpi-card">
        <mat-card-content>
          <div class="kpi-icon refund-icon">
            <mat-icon>replay</mat-icon>
          </div>
          <div class="kpi-details">
            <h3>Refunded</h3>
            <p class="kpi-value">
              {{ dashboard.totalRefunded.amount | currency:dashboard.totalRefunded.currency }}
            </p>
          </div>
        </mat-card-content>
      </mat-card>
    </div>

    <!-- Charts -->
    <div class="charts-grid">
      <mat-card class="chart-card">
        <mat-card-header>
          <mat-card-title>Revenue Trend</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <canvas baseChart
            [data]="revenueChartData"
            [options]="revenueChartOptions"
            type="line">
          </canvas>
        </mat-card-content>
      </mat-card>

      <mat-card class="chart-card">
        <mat-card-header>
          <mat-card-title>Revenue by Fee Type</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <canvas baseChart
            [data]="feeTypeChartData"
            [options]="feeTypeChartOptions"
            type="doughnut">
          </canvas>
        </mat-card-content>
      </mat-card>
    </div>

    <!-- Statistics -->
    <mat-card class="stats-card">
      <mat-card-header>
        <mat-card-title>Invoice Statistics</mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <div class="stats-grid">
          <div class="stat-item">
            <label>Total Invoices:</label>
            <span>{{ dashboard.invoiceCount }}</span>
          </div>
          <div class="stat-item">
            <label>Paid Invoices:</label>
            <span>{{ dashboard.paidInvoiceCount }}</span>
          </div>
          <div class="stat-item">
            <label>Past Due:</label>
            <span class="warn-text">{{ dashboard.pastDueInvoiceCount }}</span>
          </div>
          <div class="stat-item">
            <label>Avg Invoice Value:</label>
            <span>{{ dashboard.averageInvoiceValue.amount | currency:dashboard.averageInvoiceValue.currency }}</span>
          </div>
          <div class="stat-item">
            <label>Avg Days to Payment:</label>
            <span>{{ dashboard.averageDaysToPayment }} days</span>
          </div>
        </div>
      </mat-card-content>
    </mat-card>
  </div>
</div>
```

## 5. Models & Interfaces

### 5.1 Core Models
```typescript
export interface Invoice {
  invoiceId: string;
  invoiceNumber: string | null;
  eventId: string;
  eventName: string;
  customerId: string;
  customerName: string;
  status: InvoiceStatus;
  issueDate: Date;
  dueDate: Date;
  finalizedDate: Date | null;
  paidDate: Date | null;
  items: InvoiceItem[];
  subtotal: Money;
  taxAmount: Money;
  taxRate: number | null;
  taxJurisdiction: string | null;
  discountAmount: Money;
  discountType: string | null;
  discountReason: string | null;
  totalAmount: Money;
  amountPaid: Money;
  amountDue: Money;
  billingAddress: Address;
  notes: string;
  terms: string;
  pdfUrl: string | null;
  version: number;
}

export type InvoiceStatus =
  | 'Draft'
  | 'Finalized'
  | 'Paid'
  | 'PartiallyPaid'
  | 'Voided'
  | 'WrittenOff'
  | 'PastDue';

export interface InvoiceItem {
  itemId: string;
  invoiceId: string;
  feeType: FeeType;
  description: string;
  referenceId: string | null;
  quantity: number;
  unitPrice: Money;
  lineTotal: Money;
  taxRate: number | null;
  taxAmount: Money;
  discountAmount: Money;
  sortOrder: number;
}

export type FeeType =
  | 'Staff'
  | 'Invitation'
  | 'Prize'
  | 'Equipment'
  | 'Additional';

export interface Payment {
  paymentId: string;
  invoiceId: string;
  amount: Money;
  paymentMethod: PaymentMethod;
  status: PaymentStatus;
  transactionId: string | null;
  paymentGateway: string | null;
  processedDate: Date | null;
  failureReason: string | null;
  retryCount: number;
  paymentDetails: PaymentDetails;
  notes: string;
}

export type PaymentMethod =
  | 'CreditCard'
  | 'BankTransfer'
  | 'Check'
  | 'Cash'
  | 'PayPal'
  | 'Stripe';

export type PaymentStatus =
  | 'Pending'
  | 'Successful'
  | 'Failed'
  | 'Refunded'
  | 'PartiallyRefunded';

export interface Refund {
  refundId: string;
  paymentId: string;
  invoiceId: string;
  amount: Money;
  refundMethod: RefundMethod;
  status: RefundStatus;
  reason: string;
  processedDate: Date | null;
  transactionId: string | null;
  notes: string;
}

export type RefundMethod =
  | 'ToOriginalPaymentMethod'
  | 'StoreCredit'
  | 'Check';

export type RefundStatus =
  | 'Pending'
  | 'Processed'
  | 'Failed';

export interface Money {
  amount: number;
  currency: string;
}

export interface Address {
  street: string;
  city: string;
  state: string;
  postalCode: string;
  country: string;
}

export interface PaymentDetails {
  last4Digits?: string;
  cardType?: string;
  accountNumber?: string;
  payPalEmail?: string;
}

export interface FinancialDashboard {
  period: { startDate: Date; endDate: Date };
  totalRevenue: Money;
  totalPaid: Money;
  totalOutstanding: Money;
  totalRefunded: Money;
  invoiceCount: number;
  paidInvoiceCount: number;
  pastDueInvoiceCount: number;
  averageInvoiceValue: Money;
  averageDaysToPayment: number;
  revenueByMonth: MonthlyRevenue[];
  revenueByFeeType: { [key: string]: Money };
}

export interface MonthlyRevenue {
  month: string;
  amount: Money;
}

export interface AgingReport {
  asOfDate: Date;
  current: AgingBucket;
  days1to30: AgingBucket;
  days31to60: AgingBucket;
  days61to90: AgingBucket;
  over90Days: AgingBucket;
  total: AgingBucket;
}

export interface AgingBucket {
  amount: Money;
  count: number;
}
```

## 6. Services

### 6.1 Invoice Service
```typescript
@Injectable({
  providedIn: 'root'
})
export class InvoiceService {
  private apiUrl = environment.apiUrl + '/api/v1/invoices';

  constructor(private http: HttpClient) {}

  getInvoices(filters?: InvoiceFilters, page: number = 1, pageSize: number = 20): Observable<PagedResult<Invoice>> {
    const params = this.buildFilterParams(filters, page, pageSize);
    return this.http.get<PagedResult<Invoice>>(this.apiUrl, { params });
  }

  getInvoice(invoiceId: string): Observable<Invoice> {
    return this.http.get<Invoice>(`${this.apiUrl}/${invoiceId}`);
  }

  createInvoice(request: CreateInvoiceRequest): Observable<Invoice> {
    return this.http.post<Invoice>(this.apiUrl, request);
  }

  addItem(invoiceId: string, item: InvoiceItemRequest): Observable<InvoiceItem> {
    return this.http.post<InvoiceItem>(
      `${this.apiUrl}/${invoiceId}/items`,
      item
    );
  }

  removeItem(invoiceId: string, itemId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${invoiceId}/items/${itemId}`
    );
  }

  applyDiscount(invoiceId: string, discount: DiscountRequest): Observable<Invoice> {
    return this.http.post<Invoice>(
      `${this.apiUrl}/${invoiceId}/discount`,
      discount
    );
  }

  calculateTax(invoiceId: string, taxRate: number, jurisdiction: string): Observable<Invoice> {
    return this.http.post<Invoice>(
      `${this.apiUrl}/${invoiceId}/calculate-tax`,
      { taxRate, jurisdiction }
    );
  }

  finalizeInvoice(invoiceId: string): Observable<Invoice> {
    return this.http.post<Invoice>(
      `${this.apiUrl}/${invoiceId}/finalize`,
      {}
    );
  }

  sendInvoice(invoiceId: string, request: SendInvoiceRequest): Observable<void> {
    return this.http.post<void>(
      `${this.apiUrl}/${invoiceId}/send`,
      request
    );
  }

  voidInvoice(invoiceId: string, reason: string): Observable<Invoice> {
    return this.http.post<Invoice>(
      `${this.apiUrl}/${invoiceId}/void`,
      { reason }
    );
  }

  private buildFilterParams(filters: InvoiceFilters, page: number, pageSize: number): HttpParams {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    if (filters?.status) params = params.set('status', filters.status);
    if (filters?.eventId) params = params.set('eventId', filters.eventId);
    if (filters?.customerId) params = params.set('customerId', filters.customerId);
    if (filters?.searchTerm) params = params.set('searchTerm', filters.searchTerm);

    return params;
  }
}
```

## 7. Routing

### 7.1 Routes Configuration
```typescript
const routes: Routes = [
  {
    path: 'invoices',
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        component: InvoiceListComponent,
        data: { title: 'Invoices' }
      },
      {
        path: 'new',
        component: InvoiceFormComponent,
        data: { title: 'Create Invoice' },
        canDeactivate: [UnsavedChangesGuard]
      },
      {
        path: ':id',
        component: InvoiceDetailComponent,
        data: { title: 'Invoice Detail' }
      },
      {
        path: ':id/edit',
        component: InvoiceFormComponent,
        data: { title: 'Edit Invoice' },
        canDeactivate: [UnsavedChangesGuard]
      }
    ]
  },
  {
    path: 'financial',
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['Admin', 'Finance'] },
    children: [
      {
        path: 'dashboard',
        component: FinancialDashboardComponent,
        data: { title: 'Financial Dashboard' }
      },
      {
        path: 'aging-report',
        component: AgingReportComponent,
        data: { title: 'Aging Report' }
      }
    ]
  }
];
```

## 8. Responsive Design

### 8.1 Breakpoints
- Mobile: < 600px
- Tablet: 600px - 960px
- Desktop: > 960px

### 8.2 Mobile Optimizations
- Stack invoice items vertically on mobile
- Hide non-essential columns in tables
- Use bottom sheet for actions on mobile
- Touch-friendly button sizes (min 48x48px)
- Simplified forms with step-by-step wizard on mobile

## 9. Accessibility

### 9.1 WCAG 2.1 AA Compliance
- Semantic HTML5 elements
- ARIA labels for interactive elements
- Keyboard navigation support (Tab, Enter, Escape)
- Screen reader announcements for dynamic content
- Color contrast ratio >= 4.5:1
- Focus indicators on all interactive elements

## 10. Performance Optimization

### 10.1 Strategies
- Lazy loading for invoice management module
- Virtual scrolling for large invoice lists
- OnPush change detection strategy
- Debouncing search and filter inputs
- Caching frequently accessed data
- Image optimization and lazy loading
- Tree-shakable imports

### 10.2 Bundle Size Optimization
- Production build with AOT compilation
- Minification and uglification
- Code splitting
- Target bundle size: < 500KB initial load

## 11. Testing

### 11.1 Unit Testing
```typescript
describe('InvoiceDetailComponent', () => {
  let component: InvoiceDetailComponent;
  let fixture: ComponentFixture<InvoiceDetailComponent>;
  let store: MockStore;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [InvoiceDetailComponent],
      imports: [MaterialModule, RouterTestingModule],
      providers: [provideMockStore({ initialState })]
    }).compileComponents();

    store = TestBed.inject(MockStore);
    fixture = TestBed.createComponent(InvoiceDetailComponent);
    component = fixture.componentInstance;
  });

  it('should load invoice on init', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    component.ngOnInit();
    expect(dispatchSpy).toHaveBeenCalledWith(
      loadInvoiceDetail({ invoiceId: 'test-id' })
    );
  });

  it('should finalize invoice when confirmed', () => {
    // Test implementation
  });
});
```

### 11.2 E2E Testing
```typescript
describe('Invoice Creation Flow', () => {
  it('should create a new invoice', () => {
    cy.visit('/invoices/new');
    cy.get('[data-cy=event-select]').select('Test Event');
    cy.get('[data-cy=customer-select]').select('Test Customer');
    cy.get('[data-cy=due-date]').type('2025-12-31');
    cy.get('[data-cy=submit-button]').click();
    cy.url().should('include', '/invoices/');
    cy.contains('Invoice created successfully');
  });
});
```

## 12. Deployment

### 12.1 Build Configuration
```json
{
  "production": {
    "optimization": true,
    "outputHashing": "all",
    "sourceMap": false,
    "namedChunks": false,
    "aot": true,
    "extractLicenses": true,
    "buildOptimizer": true,
    "budgets": [
      {
        "type": "initial",
        "maximumWarning": "500kb",
        "maximumError": "1mb"
      }
    ]
  }
}
```

### 12.2 Environment Configuration
```typescript
export const environment = {
  production: true,
  apiUrl: 'https://api.eventplatform.com',
  stripePublishableKey: 'pk_live_...',
  enableAnalytics: true,
  logLevel: 'error'
};
```

## 13. Error Handling

### 13.1 Global Error Handler
```typescript
@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  constructor(
    private snackBar: MatSnackBar,
    private logger: LoggerService
  ) {}

  handleError(error: Error | HttpErrorResponse): void {
    if (error instanceof HttpErrorResponse) {
      // Server error
      this.logger.error('HTTP Error:', error);
      this.showErrorMessage(error.message);
    } else {
      // Client error
      this.logger.error('Client Error:', error);
      this.showErrorMessage('An unexpected error occurred');
    }
  }

  private showErrorMessage(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 5000,
      panelClass: ['error-snackbar']
    });
  }
}
```

## 14. Security

### 14.1 Authentication
- JWT token storage in HttpOnly cookies
- Token refresh mechanism
- Automatic logout on token expiration

### 14.2 Authorization
- Route guards for protected pages
- Component-level permission checks
- Hide/disable UI elements based on permissions

### 14.3 Input Sanitization
- Angular's built-in sanitization for templates
- Validate and sanitize user inputs
- Prevent XSS attacks

## 15. Appendix

### 15.1 Browser Support
- Chrome (latest 2 versions)
- Firefox (latest 2 versions)
- Safari (latest 2 versions)
- Edge (latest 2 versions)

### 15.2 Dependencies Version Matrix
- Angular: 18.x
- Angular Material: 18.x
- TypeScript: 5.3+
- RxJS: 7.8+
- NgRx: 18.x
