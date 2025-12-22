# Invitation Management - Frontend Specification

## Document Information
| Field | Value |
|-------|-------|
| Feature | Invitation Management |
| Version | 1.0 |
| Date | 2025-12-22 |
| Status | Final |

---

## 1. Overview

### 1.1 Purpose
This specification defines the frontend requirements for the Invitation Management feature, providing users with a comprehensive interface for ordering, designing, approving, and tracking event invitations.

### 1.2 Technology Stack
- **Framework**: Angular 18+ (latest stable)
- **UI Library**: Angular Material (Material 3)
- **State Management**: RxJS (no NgRx)
- **Styling**: SCSS with BEM methodology
- **Testing**: Jest (unit), Playwright (e2e)

### 1.3 Design Principles
- Mobile-first responsive design
- Material 3 design guidelines
- Accessibility (WCAG 2.1 AA compliance)
- Default Angular Material theme colors only

---

## 2. Page Structure

### 2.1 Pages
```
src/EventManagementPlatform.WebApp/projects/EventManagementPlatform/src/app/
├── pages/
│   ├── invitations/
│   │   ├── invitation-order-list/
│   │   │   ├── invitation-order-list.ts
│   │   │   ├── invitation-order-list.html
│   │   │   ├── invitation-order-list.scss
│   │   │   └── index.ts
│   │   ├── invitation-order-detail/
│   │   │   ├── invitation-order-detail.ts
│   │   │   ├── invitation-order-detail.html
│   │   │   ├── invitation-order-detail.scss
│   │   │   └── index.ts
│   │   ├── invitation-order-create/
│   │   │   ├── invitation-order-create.ts
│   │   │   ├── invitation-order-create.html
│   │   │   ├── invitation-order-create.scss
│   │   │   └── index.ts
│   │   ├── template-gallery/
│   │   │   ├── template-gallery.ts
│   │   │   ├── template-gallery.html
│   │   │   ├── template-gallery.scss
│   │   │   └── index.ts
│   │   ├── design-preview/
│   │   │   ├── design-preview.ts
│   │   │   ├── design-preview.html
│   │   │   ├── design-preview.scss
│   │   │   └── index.ts
│   │   ├── print-job-dashboard/
│   │   │   ├── print-job-dashboard.ts
│   │   │   ├── print-job-dashboard.html
│   │   │   ├── print-job-dashboard.scss
│   │   │   └── index.ts
│   │   └── index.ts
```

### 2.2 Components
```
├── components/
│   ├── invitations/
│   │   ├── invitation-order-card/
│   │   │   ├── invitation-order-card.ts
│   │   │   ├── invitation-order-card.html
│   │   │   ├── invitation-order-card.scss
│   │   │   └── index.ts
│   │   ├── invitation-status-badge/
│   │   │   ├── invitation-status-badge.ts
│   │   │   ├── invitation-status-badge.html
│   │   │   ├── invitation-status-badge.scss
│   │   │   └── index.ts
│   │   ├── template-card/
│   │   │   ├── template-card.ts
│   │   │   ├── template-card.html
│   │   │   ├── template-card.scss
│   │   │   └── index.ts
│   │   ├── invitation-text-editor/
│   │   │   ├── invitation-text-editor.ts
│   │   │   ├── invitation-text-editor.html
│   │   │   ├── invitation-text-editor.scss
│   │   │   └── index.ts
│   │   ├── design-approval-dialog/
│   │   │   ├── design-approval-dialog.ts
│   │   │   ├── design-approval-dialog.html
│   │   │   ├── design-approval-dialog.scss
│   │   │   └── index.ts
│   │   ├── custom-design-uploader/
│   │   │   ├── custom-design-uploader.ts
│   │   │   ├── custom-design-uploader.html
│   │   │   ├── custom-design-uploader.scss
│   │   │   └── index.ts
│   │   ├── quantity-selector/
│   │   │   ├── quantity-selector.ts
│   │   │   ├── quantity-selector.html
│   │   │   ├── quantity-selector.scss
│   │   │   └── index.ts
│   │   ├── delivery-tracker/
│   │   │   ├── delivery-tracker.ts
│   │   │   ├── delivery-tracker.html
│   │   │   ├── delivery-tracker.scss
│   │   │   └── index.ts
│   │   ├── print-job-card/
│   │   │   ├── print-job-card.ts
│   │   │   ├── print-job-card.html
│   │   │   ├── print-job-card.scss
│   │   │   └── index.ts
│   │   ├── quality-check-form/
│   │   │   ├── quality-check-form.ts
│   │   │   ├── quality-check-form.html
│   │   │   ├── quality-check-form.scss
│   │   │   └── index.ts
│   │   └── index.ts
```

---

## 3. User Interface Requirements

### 3.1 Invitation Order List Page

#### 3.1.1 Layout
- **Header**: Page title, search bar, create order button
- **Filter Panel**: Status, date range, event filters
- **Content**: Responsive grid/list of order cards
- **Pagination**: Bottom pagination with page size selector

#### 3.1.2 Features
| Feature | Description |
|---------|-------------|
| Search | Real-time search by order number, event title |
| Filter | Filter by status, event, date range |
| Sort | Sort by order date, status, quantity |
| View Toggle | Grid view / List view toggle |
| Quick Actions | Approve/reject design from list view |

#### 3.1.3 Responsive Behavior
| Breakpoint | Layout |
|------------|--------|
| < 600px | Single column, stacked cards |
| 600-960px | 2-column grid |
| > 960px | 3-4 column grid or table view |

### 3.2 Invitation Order Detail Page

#### 3.2.1 Sections
| Section | Content |
|---------|---------|
| Header | Order number, status badge, action buttons |
| Details Card | Event, customer, quantity, order date |
| Design Tab | Template/custom design preview, text content |
| Printing Tab | Print job status, quality check results |
| Delivery Tab | Tracking info, delivery status timeline |
| Activity Tab | Order history/audit log |

#### 3.2.2 Actions
| Action | Condition | UI Element |
|--------|-----------|------------|
| Edit Text | Status in [Draft, PendingDesignApproval] | Primary button |
| Approve Design | Status == PendingDesignApproval, User is Customer | Success button |
| Reject Design | Status == PendingDesignApproval, User is Customer | Warning button |
| Request Revision | Status == PendingDesignApproval, User is Customer | Secondary button |
| Cancel Order | Status not in [Printing, Shipped, Delivered] | Warning button |
| Update Quantity | Status in [Draft, PendingDesignApproval] | Input field |
| Download Digital | Status >= DesignApproved | Link button |

### 3.3 Template Gallery Page

#### 3.3.1 Layout
- **Header**: Search bar, category filters
- **Categories**: Tabs or chips for categories (Wedding, Corporate, Birthday, etc.)
- **Grid**: Responsive masonry grid of template cards
- **Preview**: Click card to view full preview in dialog

#### 3.3.2 Template Card
| Element | Description |
|---------|-------------|
| Preview Image | Large preview thumbnail |
| Template Name | Title overlay or below image |
| Category Badge | Visual indicator of category |
| Select Button | Primary action to select template |

### 3.4 Create Invitation Order Page

#### 3.4.1 Stepper Workflow
| Step | Content |
|------|---------|
| 1. Select Event | Dropdown/autocomplete to select event |
| 2. Choose Design | Toggle between Template or Custom |
| 2a. Template | Browse template gallery |
| 2b. Custom | Upload custom design file |
| 3. Customize Text | Text editor with preview |
| 4. Set Quantity | Quantity selector with pricing |
| 5. Review | Summary of order with cost breakdown |
| 6. Confirm | Submit order button |

#### 3.4.2 Form Fields
| Field | Type | Validation |
|-------|------|------------|
| Event | mat-autocomplete | Required, must be confirmed event |
| Design Type | mat-button-toggle-group | Required |
| Template | Template gallery | Required if design type = template |
| Custom Design | File upload | Required if design type = custom, PDF/AI only |
| Invitation Text | mat-textarea | Required, max 1000 chars |
| Quantity | mat-slider + input | Required, 10-10,000 |

### 3.5 Design Preview Page

#### 3.5.1 Layout
- **Preview Panel**: Large design preview with zoom controls
- **Text Content**: Display invitation text on preview
- **Actions Panel**: Approve, Reject, Request Revision buttons
- **Comments**: Text area for revision notes

#### 3.5.2 Features
| Feature | Description |
|---------|-------------|
| Zoom Controls | Zoom in/out, fit to screen |
| Text Overlay | Show text as it will appear |
| Print Simulation | Show how it looks printed |
| Revision History | View previous versions |

### 3.6 Print Job Dashboard Page

#### 3.6.1 Layout
- **Summary Cards**: Count of jobs by status
- **Job Queue**: List of pending print jobs
- **Active Jobs**: Currently printing jobs with progress
- **Completed Jobs**: Recent completed jobs

#### 3.6.2 Print Job Card
| Element | Description |
|---------|-------------|
| Order Number | Link to order detail |
| Event Name | Associated event |
| Quantity | Number to print |
| Status | Current status badge |
| Printer | Assigned printer name |
| Start/Complete | Action buttons |
| QC Button | Open quality check form |

---

## 4. Services

### 4.1 InvitationOrderService
```typescript
@Injectable({ providedIn: 'root' })
export class InvitationOrderService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getOrders(params: InvitationOrderQueryParams): Observable<PagedResult<InvitationOrderListDto>> { }

    getOrderById(orderId: string): Observable<InvitationOrderDetailDto> { }

    createOrder(order: CreateInvitationOrderDto): Observable<InvitationOrderDetailDto> { }

    updateOrder(orderId: string, order: UpdateInvitationOrderDto): Observable<InvitationOrderDetailDto> { }

    cancelOrder(orderId: string, reason: string): Observable<void> { }

    approveDesign(orderId: string): Observable<void> { }

    rejectDesign(orderId: string, reason: string): Observable<void> { }

    requestRevision(orderId: string, notes: string): Observable<void> { }

    updateText(orderId: string, text: string): Observable<void> { }

    updateQuantity(orderId: string, quantity: number): Observable<void> { }

    downloadDigitalVersion(orderId: string): Observable<Blob> { }
}
```

### 4.2 InvitationTemplateService
```typescript
@Injectable({ providedIn: 'root' })
export class InvitationTemplateService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getTemplates(category?: string): Observable<InvitationTemplateDto[]> { }

    getTemplateById(templateId: string): Observable<InvitationTemplateDto> { }

    searchTemplates(searchTerm: string): Observable<InvitationTemplateDto[]> { }
}
```

### 4.3 PrintJobService
```typescript
@Injectable({ providedIn: 'root' })
export class PrintJobService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getPrintJobs(params: PrintJobQueryParams): Observable<PagedResult<PrintJobDto>> { }

    getPrintJobById(jobId: string): Observable<PrintJobDto> { }

    createPrintJob(job: CreatePrintJobDto): Observable<PrintJobDto> { }

    startPrintJob(jobId: string): Observable<void> { }

    completePrintJob(jobId: string): Observable<void> { }

    recordQualityCheck(jobId: string, qc: QualityCheckDto): Observable<void> { }
}
```

### 4.4 DeliveryService
```typescript
@Injectable({ providedIn: 'root' })
export class DeliveryService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getDeliveryTracking(deliveryId: string): Observable<DeliveryTrackingDto> { }

    createDelivery(delivery: CreateDeliveryDto): Observable<DeliveryTrackingDto> { }

    markAsShipped(deliveryId: string, trackingNumber: string): Observable<void> { }

    markAsDelivered(deliveryId: string): Observable<void> { }
}
```

### 4.5 InvitationStateService
```typescript
@Injectable({ providedIn: 'root' })
export class InvitationStateService {
    private readonly ordersSubject = new BehaviorSubject<InvitationOrderListDto[]>([]);
    private readonly selectedOrderSubject = new BehaviorSubject<InvitationOrderDetailDto | null>(null);
    private readonly loadingSubject = new BehaviorSubject<boolean>(false);
    private readonly templatesSubject = new BehaviorSubject<InvitationTemplateDto[]>([]);

    readonly orders$ = this.ordersSubject.asObservable();
    readonly selectedOrder$ = this.selectedOrderSubject.asObservable();
    readonly loading$ = this.loadingSubject.asObservable();
    readonly templates$ = this.templatesSubject.asObservable();

    loadOrders(params: InvitationOrderQueryParams): void { }

    loadTemplates(category?: string): void { }

    selectOrder(orderId: string): void { }

    clearSelection(): void { }

    refreshOrders(): void { }
}
```

---

## 5. Models/Interfaces

### 5.1 DTOs
```typescript
export interface InvitationOrderListDto {
    invitationOrderId: string;
    orderNumber: string;
    eventTitle: string;
    customerName: string;
    quantity: number;
    status: InvitationOrderStatus;
    orderDate: Date;
}

export interface InvitationOrderDetailDto {
    invitationOrderId: string;
    orderNumber: string;
    eventId: string;
    eventTitle: string;
    customerId: string;
    customerName: string;
    orderDate: Date;
    designType: DesignType;
    templateId?: string;
    templateName?: string;
    customDesignUrl?: string;
    invitationText: string;
    quantity: number;
    status: InvitationOrderStatus;
    designApprovedAt?: Date;
    printerName?: string;
    trackingNumber?: string;
    createdAt: Date;
    modifiedAt?: Date;
}

export interface CreateInvitationOrderDto {
    eventId: string;
    customerId: string;
    designType: DesignType;
    templateId?: string;
    customDesignUrl?: string;
    invitationText: string;
    quantity: number;
}

export interface UpdateInvitationOrderDto {
    invitationText: string;
    quantity: number;
}

export interface InvitationTemplateDto {
    invitationTemplateId: string;
    name: string;
    description?: string;
    previewImageUrl: string;
    category: string;
    isActive: boolean;
}

export interface PrintJobDto {
    invitationPrintJobId: string;
    invitationOrderId: string;
    orderNumber: string;
    printerId: string;
    printerName: string;
    startedAt?: Date;
    completedAt?: Date;
    status: PrintJobStatus;
    notes?: string;
}

export interface QualityCheckDto {
    passed: boolean;
    issues?: string;
    notes?: string;
}

export interface DeliveryTrackingDto {
    invitationDeliveryId: string;
    invitationOrderId: string;
    orderNumber: string;
    trackingNumber: string;
    shipperName: string;
    shippedAt?: Date;
    deliveredAt?: Date;
    deliveryAddress: string;
    status: DeliveryStatus;
}
```

### 5.2 Enums
```typescript
export enum InvitationOrderStatus {
    Draft = 'Draft',
    PendingDesignApproval = 'PendingDesignApproval',
    DesignApproved = 'DesignApproved',
    DesignRejected = 'DesignRejected',
    PrintQueued = 'PrintQueued',
    Printing = 'Printing',
    QualityCheck = 'QualityCheck',
    QualityCheckPassed = 'QualityCheckPassed',
    QualityCheckFailed = 'QualityCheckFailed',
    Packaging = 'Packaging',
    Shipped = 'Shipped',
    Delivered = 'Delivered',
    Cancelled = 'Cancelled'
}

export enum DesignType {
    Template = 'Template',
    Custom = 'Custom'
}

export enum PrintJobStatus {
    Queued = 'Queued',
    InProgress = 'InProgress',
    Completed = 'Completed',
    Failed = 'Failed'
}

export enum DeliveryStatus {
    Pending = 'Pending',
    Shipped = 'Shipped',
    InTransit = 'InTransit',
    OutForDelivery = 'OutForDelivery',
    Delivered = 'Delivered',
    Failed = 'Failed'
}
```

---

## 6. Routing

### 6.1 Route Configuration
```typescript
export const invitationRoutes: Routes = [
    {
        path: 'invitations',
        children: [
            {
                path: '',
                redirectTo: 'orders',
                pathMatch: 'full'
            },
            {
                path: 'orders',
                loadComponent: () => import('./pages/invitations/invitation-order-list').then(m => m.InvitationOrderList),
                title: 'Invitation Orders'
            },
            {
                path: 'orders/create',
                loadComponent: () => import('./pages/invitations/invitation-order-create').then(m => m.InvitationOrderCreate),
                title: 'Create Invitation Order',
                canActivate: [authGuard]
            },
            {
                path: 'orders/:orderId',
                loadComponent: () => import('./pages/invitations/invitation-order-detail').then(m => m.InvitationOrderDetail),
                title: 'Invitation Order Details'
            },
            {
                path: 'orders/:orderId/preview',
                loadComponent: () => import('./pages/invitations/design-preview').then(m => m.DesignPreview),
                title: 'Design Preview',
                canActivate: [authGuard]
            },
            {
                path: 'templates',
                loadComponent: () => import('./pages/invitations/template-gallery').then(m => m.TemplateGallery),
                title: 'Invitation Templates'
            },
            {
                path: 'print-jobs',
                loadComponent: () => import('./pages/invitations/print-job-dashboard').then(m => m.PrintJobDashboard),
                title: 'Print Jobs',
                canActivate: [authGuard, printerRoleGuard]
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
| mat-card | Order cards, template cards |
| mat-table | Order list table view |
| mat-paginator | Pagination |
| mat-sort | Table sorting |
| mat-stepper | Create order wizard |
| mat-form-field | Form inputs |
| mat-input | Text inputs |
| mat-select | Dropdowns |
| mat-autocomplete | Event selection |
| mat-button-toggle-group | Design type selector |
| mat-slider | Quantity selector |
| mat-button | Action buttons |
| mat-icon | Icons throughout |
| mat-menu | Actions dropdown |
| mat-dialog | Design approval, confirmations |
| mat-snackbar | Notifications |
| mat-progress-spinner | Loading states |
| mat-progress-bar | Upload/print progress |
| mat-chip | Status badges, category tags |
| mat-tabs | Order detail sections |
| mat-expansion-panel | Filter panel |
| mat-badge | Notification counts |
| mat-tooltip | Help text |

---

## 8. Styling Guidelines

### 8.1 BEM Naming Convention
```scss
// Block
.invitation-order-card { }

// Element
.invitation-order-card__header { }
.invitation-order-card__preview { }
.invitation-order-card__content { }
.invitation-order-card__footer { }
.invitation-order-card__actions { }

// Modifier
.invitation-order-card--pending { }
.invitation-order-card--approved { }
.invitation-order-card--shipped { }
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

// Use Angular Material theme colors only
// No custom colors allowed
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
| Upload Error | Snackbar with retry option |
| API 400 | Snackbar with details |
| API 404 | Redirect to not found page |
| API 401 | Redirect to login |
| API 402 | Payment required dialog |
| API 403 | Display forbidden message |
| API 500 | Error dialog with retry option |
| Network | Snackbar with retry action |

### 9.2 Error Interceptor
```typescript
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
    return next(req).pipe(
        catchError((error: HttpErrorResponse) => {
            // Handle errors consistently
            return throwError(() => error);
        })
    );
};
```

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
| Image Alt Text | Alt text for all preview images |

### 10.2 ARIA Implementation
```html
<mat-card role="article" [attr.aria-label]="'Order ' + order.orderNumber">
    <mat-card-header>
        <mat-card-title id="order-{{order.invitationOrderId}}-title">
            Order {{order.orderNumber}}
        </mat-card-title>
    </mat-card-header>
    <mat-card-content aria-describedby="order-{{order.invitationOrderId}}-title">
        <!-- content -->
    </mat-card-content>
</mat-card>
```

---

## 11. Testing Requirements

### 11.1 Unit Tests (Jest)
| Component/Service | Test Coverage |
|-------------------|---------------|
| InvitationOrderService | 100% |
| InvitationTemplateService | 100% |
| PrintJobService | 100% |
| InvitationStateService | 100% |
| InvitationOrderList | 90% |
| InvitationOrderDetail | 90% |
| TemplateGallery | 90% |
| DesignPreview | 90% |
| All components | Minimum 80% |

### 11.2 E2E Tests (Playwright)
| Scenario | Priority |
|----------|----------|
| Create new invitation order | High |
| Select template | High |
| Upload custom design | High |
| Approve design | High |
| Reject design | High |
| Update quantity | Medium |
| Cancel order | High |
| Track delivery | Medium |
| Print job workflow | High |
| Quality check | Medium |

### 11.3 Test File Naming
```
invitation-order-list.spec.ts      // Unit test
invitation-order-list.e2e.ts       // E2E test
```

---

## 12. Performance Requirements

| Metric | Requirement |
|--------|-------------|
| First Contentful Paint | < 1.5s |
| Time to Interactive | < 3s |
| Bundle Size | < 250KB (initial) |
| Order List Render | < 100ms for 50 items |
| Template Gallery Load | < 200ms with CDN |
| Design Preview Load | < 500ms |
| Navigation | < 200ms between pages |

### 12.1 Optimization Strategies
- Lazy loading of routes
- Virtual scrolling for large lists
- OnPush change detection
- Pagination (no infinite scroll)
- Image lazy loading with CDN
- Progressive image loading for previews

---

## 13. Internationalization

### 13.1 i18n Support
- Use Angular i18n for translations
- Date formatting via DatePipe with locale
- Number formatting via DecimalPipe
- Currency formatting for pricing

### 13.2 Date/Time Handling
- Store all dates in UTC
- Display in user's local timezone
- Use Angular Material datepicker with locale

---

## 14. Appendices

### 14.1 Related Documents
- [Backend Specification](./backend-spec.md)
- [Use Case Diagram](./diagrams/use-case.drawio)
- [C4 Diagrams](./diagrams/c4-context.drawio)
- [Class Diagram](./plantuml/class-diagram.plantuml)
- [Sequence Diagrams](./plantuml/sequence-diagram.plantuml)

### 14.2 Wireframes
Wireframes are available in the design system documentation.

### 14.3 Revision History
| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-22 | System | Initial specification |
