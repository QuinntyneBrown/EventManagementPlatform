# Prize Management - Frontend Specification

## Document Information
| Field | Value |
|-------|-------|
| Feature | Prize Management |
| Version | 1.0 |
| Date | 2025-12-22 |
| Status | Final |

---

## 1. Overview

### 1.1 Purpose
This specification defines the frontend requirements for the Prize Management feature, providing users with a comprehensive interface for managing prize items, inventory, orders, packs, and distribution.

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
│   ├── prizes/
│   │   ├── prize-list/
│   │   │   ├── prize-list.ts
│   │   │   ├── prize-list.html
│   │   │   ├── prize-list.scss
│   │   │   └── index.ts
│   │   ├── prize-detail/
│   │   │   ├── prize-detail.ts
│   │   │   ├── prize-detail.html
│   │   │   ├── prize-detail.scss
│   │   │   └── index.ts
│   │   ├── prize-create/
│   │   │   ├── prize-create.ts
│   │   │   ├── prize-create.html
│   │   │   ├── prize-create.scss
│   │   │   └── index.ts
│   │   ├── prize-edit/
│   │   │   ├── prize-edit.ts
│   │   │   ├── prize-edit.html
│   │   │   ├── prize-edit.scss
│   │   │   └── index.ts
│   │   ├── prize-pack-list/
│   │   │   ├── prize-pack-list.ts
│   │   │   ├── prize-pack-list.html
│   │   │   ├── prize-pack-list.scss
│   │   │   └── index.ts
│   │   ├── prize-pack-detail/
│   │   │   ├── prize-pack-detail.ts
│   │   │   ├── prize-pack-detail.html
│   │   │   ├── prize-pack-detail.scss
│   │   │   └── index.ts
│   │   ├── event-prize-order-list/
│   │   │   ├── event-prize-order-list.ts
│   │   │   ├── event-prize-order-list.html
│   │   │   ├── event-prize-order-list.scss
│   │   │   └── index.ts
│   │   ├── event-prize-order-detail/
│   │   │   ├── event-prize-order-detail.ts
│   │   │   ├── event-prize-order-detail.html
│   │   │   ├── event-prize-order-detail.scss
│   │   │   └── index.ts
│   │   ├── inventory-dashboard/
│   │   │   ├── inventory-dashboard.ts
│   │   │   ├── inventory-dashboard.html
│   │   │   ├── inventory-dashboard.scss
│   │   │   └── index.ts
│   │   └── index.ts
```

### 2.2 Components
```
├── components/
│   ├── prizes/
│   │   ├── prize-card/
│   │   │   ├── prize-card.ts
│   │   │   ├── prize-card.html
│   │   │   ├── prize-card.scss
│   │   │   └── index.ts
│   │   ├── prize-stock-badge/
│   │   │   ├── prize-stock-badge.ts
│   │   │   ├── prize-stock-badge.html
│   │   │   ├── prize-stock-badge.scss
│   │   │   └── index.ts
│   │   ├── prize-form/
│   │   │   ├── prize-form.ts
│   │   │   ├── prize-form.html
│   │   │   ├── prize-form.scss
│   │   │   └── index.ts
│   │   ├── prize-photo-upload/
│   │   │   ├── prize-photo-upload.ts
│   │   │   ├── prize-photo-upload.html
│   │   │   ├── prize-photo-upload.scss
│   │   │   └── index.ts
│   │   ├── prize-pack-form/
│   │   │   ├── prize-pack-form.ts
│   │   │   ├── prize-pack-form.html
│   │   │   ├── prize-pack-form.scss
│   │   │   └── index.ts
│   │   ├── prize-pack-item-selector/
│   │   │   ├── prize-pack-item-selector.ts
│   │   │   ├── prize-pack-item-selector.html
│   │   │   ├── prize-pack-item-selector.scss
│   │   │   └── index.ts
│   │   ├── order-status-stepper/
│   │   │   ├── order-status-stepper.ts
│   │   │   ├── order-status-stepper.html
│   │   │   ├── order-status-stepper.scss
│   │   │   └── index.ts
│   │   ├── order-item-list/
│   │   │   ├── order-item-list.ts
│   │   │   ├── order-item-list.html
│   │   │   ├── order-item-list.scss
│   │   │   └── index.ts
│   │   ├── inventory-transaction-list/
│   │   │   ├── inventory-transaction-list.ts
│   │   │   ├── inventory-transaction-list.html
│   │   │   ├── inventory-transaction-list.scss
│   │   │   └── index.ts
│   │   ├── low-stock-alert/
│   │   │   ├── low-stock-alert.ts
│   │   │   ├── low-stock-alert.html
│   │   │   ├── low-stock-alert.scss
│   │   │   └── index.ts
│   │   ├── receive-inventory-dialog/
│   │   │   ├── receive-inventory-dialog.ts
│   │   │   ├── receive-inventory-dialog.html
│   │   │   ├── receive-inventory-dialog.scss
│   │   │   └── index.ts
│   │   ├── adjust-inventory-dialog/
│   │   │   ├── adjust-inventory-dialog.ts
│   │   │   ├── adjust-inventory-dialog.html
│   │   │   ├── adjust-inventory-dialog.scss
│   │   │   └── index.ts
│   │   └── index.ts
```

---

## 3. User Interface Requirements

### 3.1 Prize List Page

#### 3.1.1 Layout
- **Header**: Page title, search bar, create button, import button
- **Filter Panel**: Category, stock status, active/inactive filters
- **Content**: Responsive grid of prize cards with photos
- **Pagination**: Bottom pagination with page size selector

#### 3.1.2 Features
| Feature | Description |
|---------|-------------|
| Search | Real-time search by name, SKU, category |
| Filter | Filter by category, stock status, active status |
| Sort | Sort by name, SKU, stock level, cost |
| View Toggle | Grid view / Table view toggle |
| Stock Alerts | Visual indicators for low stock items |
| Bulk Actions | Select multiple items for bulk operations |

#### 3.1.3 Responsive Behavior
| Breakpoint | Layout |
|------------|--------|
| < 600px | Single column, stacked cards |
| 600-960px | 2-column grid |
| > 960px | 3-4 column grid or table view |

### 3.2 Prize Detail Page

#### 3.2.1 Sections
| Section | Content |
|---------|---------|
| Header | Name, SKU, stock badge, action buttons |
| Photo Card | Prize image with upload capability |
| Details Card | Category, cost, stock levels, description |
| Inventory Tab | Transaction history with filters |
| Orders Tab | Active orders containing this prize |
| Analytics Tab | Usage statistics and charts |

#### 3.2.2 Actions
| Action | Condition | UI Element |
|--------|-----------|------------|
| Edit | User is Manager/Admin | Primary button |
| Activate | Status == Inactive | Success button |
| Deactivate | Status == Active, no active orders | Warning button |
| Receive Inventory | Always | Primary button |
| Adjust Inventory | User is Manager/Admin | Secondary button |
| Upload Photo | Always | Icon button on photo |

### 3.3 Prize Create/Edit Page

#### 3.3.1 Form Fields
| Field | Type | Validation |
|-------|------|------------|
| Name | mat-input | Required, max 200 chars |
| SKU | mat-input | Required, unique, alphanumeric |
| Category | mat-select | Required, searchable |
| Description | mat-textarea | Optional, max 2000 chars |
| Unit Cost | mat-input | Required, number, > 0 |
| Current Stock | mat-input | Required, integer, >= 0 |
| Minimum Stock | mat-input | Required, integer, >= 0 |
| Photo | file-upload | Optional, image only, max 5MB |

#### 3.3.2 Form Behavior
- Auto-save draft every 30 seconds
- Unsaved changes warning on navigation
- Real-time validation feedback
- Photo preview on upload
- SKU uniqueness validation on blur

### 3.4 Prize Pack List Page

#### 3.4.1 Layout
- **Header**: Page title, search bar, create button
- **Content**: List/grid of prize packs
- **Each Pack Card**: Name, item count, total cost, actions

#### 3.4.2 Features
| Feature | Description |
|---------|-------------|
| Search | Search by pack name |
| Quick Add | Add pack to new order |
| Clone Pack | Duplicate existing pack |
| View Items | Expand to see pack contents |

### 3.5 Prize Pack Detail Page

#### 3.5.1 Sections
| Section | Content |
|---------|---------|
| Header | Pack name, total items, total cost |
| Items List | Table of items with quantities |
| Usage History | Events that used this pack |

#### 3.5.2 Actions
| Action | Description |
|--------|-------------|
| Edit Pack | Modify pack name/description |
| Add Items | Add prizes to pack |
| Remove Items | Remove prizes from pack |
| Update Quantities | Change item quantities |

### 3.6 Event Prize Order List Page

#### 3.6.1 Layout
- **Header**: Page title, filters, create order button
- **Status Tabs**: Draft, Submitted, Packed, Shipped, Delivered
- **Content**: Table of orders with status indicators
- **Pagination**: Bottom pagination

#### 3.6.2 Features
| Feature | Description |
|---------|-------------|
| Filter by Event | Filter orders for specific event |
| Filter by Status | Tab-based status filtering |
| Filter by Date | Date range picker for required date |
| Sort | Sort by order date, required date, status |
| Quick Actions | Pack, ship, deliver buttons inline |

### 3.7 Event Prize Order Detail Page

#### 3.7.1 Layout
- **Status Stepper**: Visual progress indicator
- **Order Info Card**: Event, dates, status
- **Items Table**: Ordered items with quantities
- **Actions Panel**: Context-aware action buttons
- **Timeline**: Order history with timestamps

#### 3.7.2 Status Stepper Steps
1. Draft
2. Submitted
3. Allocated
4. Packed
5. Shipped
6. Delivered
7. Distributed

#### 3.7.3 Actions by Status
| Status | Available Actions |
|--------|------------------|
| Draft | Edit, Submit, Cancel |
| Submitted | Edit Items, Cancel |
| Allocated | Pack, Cancel |
| Packed | Ship, Unpack |
| Shipped | Mark Delivered |
| Delivered | Distribute |
| Distributed | Return Unused |

### 3.8 Inventory Dashboard Page

#### 3.8.1 Layout
- **Summary Cards**: Total items, low stock count, total value
- **Low Stock Alert Section**: Items below minimum
- **Recent Transactions**: Latest inventory movements
- **Stock Chart**: Visual stock levels by category
- **Quick Actions**: Receive inventory, adjust inventory

#### 3.8.2 Features
| Feature | Description |
|---------|-------------|
| Real-time Updates | Live stock level updates |
| Low Stock Alerts | Prominent warnings for low stock |
| Transaction Filter | Filter by type, date, item |
| Export | Export inventory report |
| Analytics | Stock usage trends and forecasting |

---

## 4. Services

### 4.1 PrizeService
```typescript
@Injectable({ providedIn: 'root' })
export class PrizeService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getPrizeItems(params: PrizeQueryParams): Observable<PagedResult<PrizeItemListDto>> { }

    getPrizeItemById(prizeItemId: string): Observable<PrizeItemDetailDto> { }

    createPrizeItem(prize: CreatePrizeItemDto): Observable<PrizeItemDetailDto> { }

    updatePrizeItem(prizeItemId: string, prize: UpdatePrizeItemDto): Observable<PrizeItemDetailDto> { }

    deletePrizeItem(prizeItemId: string): Observable<void> { }

    activatePrizeItem(prizeItemId: string): Observable<void> { }

    deactivatePrizeItem(prizeItemId: string): Observable<void> { }

    uploadPrizePhoto(prizeItemId: string, photo: File): Observable<string> { }

    getLowStockItems(): Observable<LowStockItemDto[]> { }
}
```

### 4.2 PrizePackService
```typescript
@Injectable({ providedIn: 'root' })
export class PrizePackService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getPrizePacks(params: PrizePackQueryParams): Observable<PagedResult<PrizePackListDto>> { }

    getPrizePackById(prizePackId: string): Observable<PrizePackDetailDto> { }

    createPrizePack(pack: CreatePrizePackDto): Observable<PrizePackDetailDto> { }

    updatePrizePack(prizePackId: string, pack: UpdatePrizePackDto): Observable<PrizePackDetailDto> { }

    deletePrizePack(prizePackId: string): Observable<void> { }

    addItemToPack(prizePackId: string, item: PrizePackItemDto): Observable<void> { }

    removeItemFromPack(prizePackId: string, itemId: string): Observable<void> { }
}
```

### 4.3 EventPrizeOrderService
```typescript
@Injectable({ providedIn: 'root' })
export class EventPrizeOrderService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getEventPrizeOrders(params: OrderQueryParams): Observable<PagedResult<EventPrizeOrderListDto>> { }

    getEventPrizeOrderById(orderId: string): Observable<EventPrizeOrderDetailDto> { }

    createEventPrizeOrder(order: CreateEventPrizeOrderDto): Observable<EventPrizeOrderDetailDto> { }

    updateEventPrizeOrder(orderId: string, order: UpdateEventPrizeOrderDto): Observable<EventPrizeOrderDetailDto> { }

    cancelEventPrizeOrder(orderId: string): Observable<void> { }

    addItemToOrder(orderId: string, item: OrderItemDto): Observable<void> { }

    removeItemFromOrder(orderId: string, itemId: string): Observable<void> { }

    packPrizes(orderId: string): Observable<void> { }

    shipPrizes(orderId: string, trackingNumber?: string): Observable<void> { }

    deliverPrizes(orderId: string): Observable<void> { }

    distributePrizes(orderId: string, distributedQuantities: DistributedQuantityDto[]): Observable<void> { }

    returnPrizes(orderId: string, returnedQuantities: ReturnedQuantityDto[]): Observable<void> { }
}
```

### 4.4 InventoryService
```typescript
@Injectable({ providedIn: 'root' })
export class InventoryService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getInventoryTransactions(params: InventoryQueryParams): Observable<PagedResult<InventoryTransactionDto>> { }

    receiveInventory(prizeItemId: string, quantity: number, notes?: string): Observable<void> { }

    adjustInventory(prizeItemId: string, quantity: number, notes: string): Observable<void> { }

    getLowStockReport(): Observable<LowStockReportDto> { }
}
```

### 4.5 PrizeStateService
```typescript
@Injectable({ providedIn: 'root' })
export class PrizeStateService {
    private readonly prizesSubject = new BehaviorSubject<PrizeItemListDto[]>([]);
    private readonly selectedPrizeSubject = new BehaviorSubject<PrizeItemDetailDto | null>(null);
    private readonly lowStockCountSubject = new BehaviorSubject<number>(0);
    private readonly loadingSubject = new BehaviorSubject<boolean>(false);

    readonly prizes$ = this.prizesSubject.asObservable();
    readonly selectedPrize$ = this.selectedPrizeSubject.asObservable();
    readonly lowStockCount$ = this.lowStockCountSubject.asObservable();
    readonly loading$ = this.loadingSubject.asObservable();

    loadPrizes(params: PrizeQueryParams): void { }

    selectPrize(prizeItemId: string): void { }

    clearSelection(): void { }

    refreshPrizes(): void { }

    updateLowStockCount(): void { }
}
```

---

## 5. Models/Interfaces

### 5.1 DTOs
```typescript
export interface PrizeItemListDto {
    prizeItemId: string;
    name: string;
    sku: string;
    category: string;
    unitCost: number;
    currentStock: number;
    minimumStock: number;
    isActive: boolean;
    photoUrl?: string;
    isLowStock: boolean;
}

export interface PrizeItemDetailDto {
    prizeItemId: string;
    name: string;
    description?: string;
    sku: string;
    category: string;
    unitCost: number;
    currentStock: number;
    minimumStock: number;
    isActive: boolean;
    photoUrl?: string;
    createdAt: Date;
    modifiedAt?: Date;
}

export interface CreatePrizeItemDto {
    name: string;
    description?: string;
    sku: string;
    category: string;
    unitCost: number;
    currentStock: number;
    minimumStock: number;
}

export interface PrizePackDetailDto {
    prizePackId: string;
    name: string;
    description?: string;
    isActive: boolean;
    items: PrizePackItemDto[];
    totalCost: number;
}

export interface PrizePackItemDto {
    prizePackItemId: string;
    prizeItemId: string;
    prizeItemName: string;
    quantity: number;
    unitCost: number;
}

export interface EventPrizeOrderDetailDto {
    eventPrizeOrderId: string;
    eventId: string;
    eventTitle: string;
    orderStatus: OrderStatus;
    orderDate: Date;
    requiredByDate: Date;
    packedDate?: Date;
    shippedDate?: Date;
    deliveredDate?: Date;
    distributedDate?: Date;
    notes?: string;
    items: EventPrizeOrderItemDto[];
    totalCost: number;
}

export interface EventPrizeOrderItemDto {
    eventPrizeOrderItemId: string;
    prizeItemId: string;
    prizeItemName: string;
    quantity: number;
    allocatedQuantity: number;
    distributedQuantity: number;
    returnedQuantity: number;
    unitCost: number;
}

export interface InventoryTransactionDto {
    inventoryTransactionId: string;
    prizeItemId: string;
    prizeItemName: string;
    transactionType: TransactionType;
    quantity: number;
    transactionDate: Date;
    notes?: string;
    createdByName: string;
}
```

### 5.2 Enums
```typescript
export enum OrderStatus {
    Draft = 'Draft',
    Submitted = 'Submitted',
    Confirmed = 'Confirmed',
    Allocated = 'Allocated',
    Packed = 'Packed',
    Shipped = 'Shipped',
    Delivered = 'Delivered',
    Distributed = 'Distributed',
    Returned = 'Returned',
    Cancelled = 'Cancelled'
}

export enum TransactionType {
    Received = 'Received',
    Allocated = 'Allocated',
    Deallocated = 'Deallocated',
    Adjustment = 'Adjustment',
    Distributed = 'Distributed',
    Returned = 'Returned',
    Damaged = 'Damaged',
    Lost = 'Lost'
}
```

---

## 6. Routing

### 6.1 Route Configuration
```typescript
export const prizeRoutes: Routes = [
    {
        path: 'prizes',
        children: [
            {
                path: '',
                loadComponent: () => import('./pages/prizes/prize-list').then(m => m.PrizeList),
                title: 'Prize Items'
            },
            {
                path: 'create',
                loadComponent: () => import('./pages/prizes/prize-create').then(m => m.PrizeCreate),
                title: 'Create Prize Item',
                canActivate: [authGuard]
            },
            {
                path: ':prizeItemId',
                loadComponent: () => import('./pages/prizes/prize-detail').then(m => m.PrizeDetail),
                title: 'Prize Details'
            },
            {
                path: ':prizeItemId/edit',
                loadComponent: () => import('./pages/prizes/prize-edit').then(m => m.PrizeEdit),
                title: 'Edit Prize',
                canActivate: [authGuard],
                canDeactivate: [unsavedChangesGuard]
            },
            {
                path: 'packs',
                loadComponent: () => import('./pages/prizes/prize-pack-list').then(m => m.PrizePackList),
                title: 'Prize Packs'
            },
            {
                path: 'packs/:prizePackId',
                loadComponent: () => import('./pages/prizes/prize-pack-detail').then(m => m.PrizePackDetail),
                title: 'Prize Pack Details'
            },
            {
                path: 'orders',
                loadComponent: () => import('./pages/prizes/event-prize-order-list').then(m => m.EventPrizeOrderList),
                title: 'Prize Orders'
            },
            {
                path: 'orders/:orderId',
                loadComponent: () => import('./pages/prizes/event-prize-order-detail').then(m => m.EventPrizeOrderDetail),
                title: 'Prize Order Details'
            },
            {
                path: 'inventory',
                loadComponent: () => import('./pages/prizes/inventory-dashboard').then(m => m.InventoryDashboard),
                title: 'Inventory Dashboard',
                canActivate: [authGuard]
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
| mat-card | Prize cards, detail sections |
| mat-table | Prize/order list table view |
| mat-paginator | Pagination |
| mat-sort | Table sorting |
| mat-form-field | Form inputs |
| mat-input | Text inputs |
| mat-select | Dropdowns |
| mat-autocomplete | Item search |
| mat-datepicker | Required date selection |
| mat-button | Action buttons |
| mat-icon | Icons throughout |
| mat-menu | Actions dropdown |
| mat-dialog | Inventory operations, confirmations |
| mat-snackbar | Notifications |
| mat-progress-spinner | Loading states |
| mat-chip | Stock status badges |
| mat-tabs | Detail page sections |
| mat-stepper | Order status progression |
| mat-badge | Low stock count badges |
| mat-slider | Stock level adjustments |

---

## 8. Styling Guidelines

### 8.1 BEM Naming Convention
```scss
// Block
.prize-card { }

// Element
.prize-card__header { }
.prize-card__image { }
.prize-card__title { }
.prize-card__sku { }
.prize-card__stock { }
.prize-card__actions { }

// Modifier
.prize-card--low-stock { }
.prize-card--inactive { }
.prize-card--selected { }
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

### 8.3 Stock Status Colors
```scss
// Use Material theme colors via CSS variables
.stock-normal {
    color: var(--mat-sys-success);
}

.stock-low {
    color: var(--mat-sys-warning);
}

.stock-depleted {
    color: var(--mat-sys-error);
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
| API 401 | Redirect to login |
| API 403 | Display forbidden message |
| API 409 (Insufficient Stock) | Dialog with available quantity |
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
| Image Alt Text | Alt text for all prize photos |

### 10.2 ARIA Implementation
```html
<mat-card role="article" [attr.aria-label]="prize.name">
    <img [src]="prize.photoUrl" [alt]="prize.name + ' photo'" />
    <mat-card-header>
        <mat-card-title id="prize-{{prize.prizeItemId}}-title">
            {{prize.name}}
        </mat-card-title>
    </mat-card-header>
    <mat-card-content aria-describedby="prize-{{prize.prizeItemId}}-title">
        <span class="stock-badge"
              [attr.aria-label]="'Stock level: ' + prize.currentStock + ' units'">
            {{prize.currentStock}} in stock
        </span>
    </mat-card-content>
</mat-card>
```

---

## 11. Testing Requirements

### 11.1 Unit Tests (Jest)
| Component/Service | Test Coverage |
|-------------------|---------------|
| PrizeService | 100% |
| InventoryService | 100% |
| PrizeStateService | 100% |
| PrizeList | 90% |
| PrizeDetail | 90% |
| OrderDetail | 90% |
| PrizeForm | 100% (validation) |
| All components | Minimum 80% |

### 11.2 E2E Tests (Playwright)
| Scenario | Priority |
|----------|----------|
| Create new prize item | High |
| Upload prize photo | High |
| Receive inventory | High |
| Create prize order | High |
| Pack prizes | High |
| Ship prizes | High |
| Distribute prizes | High |
| Low stock alert | Medium |
| Adjust inventory | Medium |
| Filter prize list | Medium |

### 11.3 Test File Naming
```
prize-list.spec.ts      // Unit test
prize-list.e2e.ts       // E2E test
```

---

## 12. Performance Requirements

| Metric | Requirement |
|--------|-------------|
| First Contentful Paint | < 1.5s |
| Time to Interactive | < 3s |
| Bundle Size | < 200KB (initial) |
| Prize List Render | < 100ms for 50 items |
| Photo Load | Lazy load with placeholder |
| Navigation | < 200ms between pages |

### 12.1 Optimization Strategies
- Lazy loading of routes
- Virtual scrolling for large lists
- OnPush change detection
- Pagination (no infinite scroll)
- Image lazy loading with CDN
- Photo thumbnails for list views

---

## 13. Internationalization

### 13.1 i18n Support
- Use Angular i18n for translations
- Currency formatting via CurrencyPipe
- Date formatting via DatePipe with locale
- Number formatting via DecimalPipe

### 13.2 Currency Handling
- Display unit costs with currency symbol
- Support multiple currencies (future)
- Use Angular CurrencyPipe with locale

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
