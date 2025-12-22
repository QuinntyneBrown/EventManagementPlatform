# Customer Management Software Requirements Specification - Frontend

## Document Information

- **Project:** {project} - Angular Frontend
- **Version:** 1.0.0
- **Date:** 2025-12-22
- **Status:** Draft

---

## Table of Contents

1. [Customer List Requirements](#customer-list-requirements)
2. [Customer Detail Requirements](#customer-detail-requirements)
3. [Customer Form Requirements](#customer-form-requirements)
4. [Contact Management Requirements](#contact-management-requirements)
5. [Communication History Requirements](#communication-history-requirements)
6. [Complaint Management Requirements](#complaint-management-requirements)
7. [Customer Insights Requirements](#customer-insights-requirements)
8. [State Management Requirements](#state-management-requirements)
9. [Routing Requirements](#routing-requirements)
10. [Validation Requirements](#validation-requirements)
11. [UI/UX Requirements](#uiux-requirements)
12. [Performance Requirements](#performance-requirements)
13. [Accessibility Requirements](#accessibility-requirements)
14. [Testing Requirements](#testing-requirements)

---

## 1. Customer List Requirements

### REQ-FE-CUS-001: Customer List Display

**Requirement:** The system shall provide a responsive customer list component with search, filtering, sorting, and pagination capabilities.

**Acceptance Criteria:**
- [ ] Component uses Angular Reactive Forms for filters
- [ ] Displays customers in Material table with sortable columns
- [ ] Table columns: customerNumber, companyName, type, segment, status, lifetimeValue, actions
- [ ] OnPush change detection strategy for performance
- [ ] Pagination with configurable page size (10, 20, 50, 100)
- [ ] Loading indicator displayed during data fetch
- [ ] Empty state message when no customers found
- [ ] Row click navigates to customer detail view
- [ ] Actions menu with View, Edit, and Deactivate options

**Component Features:**

| Feature | Implementation |
|---------|----------------|
| Change Detection | OnPush |
| Data Source | Observable from Facade |
| Pagination | MatPaginator |
| Sorting | MatSort |
| Filtering | Reactive Forms |

---

### REQ-FE-CUS-002: Customer Search

**Requirement:** The system shall provide real-time search functionality with debouncing for performance.

**Acceptance Criteria:**
- [ ] Search input field with Material design
- [ ] Search icon displayed as prefix
- [ ] Search query debounced by 300ms
- [ ] Searches across company name, email, and customer number
- [ ] Search is case-insensitive
- [ ] Clear button to reset search
- [ ] Search results update table data reactively
- [ ] Loading state during search

**Search Implementation:**

```typescript
searchControl = new FormControl('');

ngOnInit(): void {
  this.searchControl.valueChanges.pipe(
    debounceTime(300),
    distinctUntilChanged()
  ).subscribe(query => {
    this.customerFacade.searchCustomers(query);
  });
}
```

---

### REQ-FE-CUS-003: Customer Filtering

**Requirement:** The system shall provide multi-select filtering for customer type, segment, and status.

**Acceptance Criteria:**
- [ ] Filter form uses Material select components
- [ ] Multiple selections allowed for each filter
- [ ] Filter options: Type (Individual, SmallBusiness, Enterprise, NonProfit, Government)
- [ ] Filter options: Segment (Standard, Premium, VIP, Corporate)
- [ ] Filter options: Status (Active, Inactive, Suspended)
- [ ] Clear filters button resets all selections
- [ ] Filter changes update table data reactively
- [ ] Selected filters persist during pagination

**Filter Form:**

```typescript
filterForm = this.fb.group({
  type: [[]],
  segment: [[]],
  status: [[]]
});
```

---

### REQ-FE-CUS-004: Customer Export

**Requirement:** The system shall provide customer list export functionality with format selection.

**Acceptance Criteria:**
- [ ] Export button displays download icon
- [ ] Export supports CSV, Excel, and JSON formats
- [ ] Export includes all filtered results (not just current page)
- [ ] Export triggers file download
- [ ] Success message displayed after export
- [ ] Error handling for failed exports
- [ ] Loading indicator during export process

---

### REQ-FE-CUS-005: Customer Status Indicators

**Requirement:** The system shall display visual indicators for customer status and segment using color-coded chips.

**Acceptance Criteria:**
- [ ] Material chips used for status display
- [ ] Active status: green color
- [ ] Inactive status: grey color
- [ ] Suspended status: orange color
- [ ] VIP segment: gold color
- [ ] Premium segment: blue color
- [ ] Standard segment: default color
- [ ] Corporate segment: purple color

**Status Colors:**

| Status/Segment | Color | Class |
|----------------|-------|-------|
| Active | Green | accent |
| Inactive | Grey | - |
| Suspended | Orange | warn |
| VIP | Gold | vip |
| Premium | Blue | primary |

---

## 2. Customer Detail Requirements

### REQ-FE-CUS-006: Customer Detail View

**Requirement:** The system shall provide a comprehensive customer detail view with tabbed navigation for different data categories.

**Acceptance Criteria:**
- [ ] Component uses OnPush change detection
- [ ] Header displays customer company name
- [ ] Back button navigates to customer list
- [ ] Edit button navigates to edit form
- [ ] Quick action buttons for Send Email and Schedule Follow-up
- [ ] Summary card displays key customer metrics
- [ ] Tabbed interface for Profile, Contacts, Communications, Complaints, Testimonials, AI Insights
- [ ] Data loaded from facade service
- [ ] Loading state for async data
- [ ] Error handling for failed data loads

**Summary Card Metrics:**

| Metric | Display Format |
|--------|----------------|
| Customer Number | Text |
| Type | Chip |
| Segment | Chip |
| Status | Color-coded chip |
| Lifetime Value | Currency pipe |
| Total Events | Number |

---

### REQ-FE-CUS-007: Customer Profile Tab

**Requirement:** The system shall display complete customer profile information in an organized layout.

**Acceptance Criteria:**
- [ ] Profile information displayed in grid layout
- [ ] Company name, industry, website displayed
- [ ] Primary and secondary email displayed
- [ ] Primary and secondary phone displayed
- [ ] Billing address displayed with all fields
- [ ] Shipping address displayed if different from billing
- [ ] Social media links displayed as clickable icons
- [ ] All fields are read-only (edit via Edit button)
- [ ] Responsive layout for mobile devices

---

### REQ-FE-CUS-008: Customer Tabs Navigation

**Requirement:** The system shall provide seamless tab navigation with lazy loading of tab content.

**Acceptance Criteria:**
- [ ] Material tab group with smooth animations
- [ ] Animation duration: 300ms
- [ ] Tab content lazy loaded on first access
- [ ] Selected tab index persisted in component state
- [ ] Tab labels display counts (e.g., "Contacts (5)")
- [ ] Tab content scrollable independently
- [ ] Tab navigation accessible via keyboard
- [ ] Active tab visually distinguished

---

### REQ-FE-CUS-009: Quick Action Dialogs

**Requirement:** The system shall provide modal dialogs for quick actions like sending emails and scheduling meetings.

**Acceptance Criteria:**
- [ ] Send Email dialog: width 800px, includes customer context
- [ ] Schedule Meeting dialog: width 700px, calendar integration
- [ ] Schedule Follow-up dialog: width 600px, task assignment
- [ ] Dialogs use Material Dialog component
- [ ] Dialogs validate input before submission
- [ ] Success notification after action completion
- [ ] Error handling with user-friendly messages
- [ ] Dialogs close on successful submission
- [ ] Dialogs can be cancelled without data loss

---

## 3. Customer Form Requirements

### REQ-FE-CUS-010: Customer Create/Edit Form

**Requirement:** The system shall provide a comprehensive form for creating and editing customer information with validation.

**Acceptance Criteria:**
- [ ] Form uses Angular Reactive Forms
- [ ] Form determines mode based on route (create vs edit)
- [ ] Edit mode pre-populates form with existing data
- [ ] Company name field: required, min 2 characters
- [ ] Type field: required, dropdown selection
- [ ] Industry field: optional, dropdown with predefined options
- [ ] Email fields: required (primary), email format validation
- [ ] Phone fields: required (primary), international format validation
- [ ] Website field: optional, URL format validation
- [ ] Form displays validation errors in real-time
- [ ] Submit button disabled when form invalid
- [ ] Cancel button navigates back without saving

**Form Fields:**

| Field Group | Fields | Validation |
|-------------|--------|------------|
| Profile | companyName, type, industry | required, minLength(2) |
| Contact | primaryEmail, secondaryEmail, primaryPhone, secondaryPhone | required, email, phone |
| Web | website | url pattern |
| Billing Address | street, city, state, zipCode, country | required |
| Shipping Address | street, city, state, zipCode, country | optional |
| Preferences | channels, language, timezone, notifications | defaults |

---

### REQ-FE-CUS-011: Address Form Section

**Requirement:** The system shall provide address input sections with validation and copy functionality.

**Acceptance Criteria:**
- [ ] Billing address form group with all required fields
- [ ] Shipping address form group (optional)
- [ ] "Copy billing to shipping" button
- [ ] Clicking copy button populates shipping address
- [ ] Street, city, state, zip code, country all required for billing
- [ ] All shipping address fields required if any is provided
- [ ] Country dropdown with searchable list
- [ ] State/province dropdown based on country selection
- [ ] Zip code format validation based on country

---

### REQ-FE-CUS-012: Customer Preferences Form Section

**Requirement:** The system shall provide customer preferences configuration with intuitive controls.

**Acceptance Criteria:**
- [ ] Communication channels: checkbox group (Email, SMS, Phone)
- [ ] Preferred language: dropdown (default 'en')
- [ ] Time zone: searchable dropdown (default 'UTC')
- [ ] Email notifications: toggle switch (default true)
- [ ] SMS notifications: toggle switch (default false)
- [ ] Push notifications: toggle switch (default true)
- [ ] Marketing consent: checkbox (default false, requires explicit opt-in)
- [ ] Data processing consent: checkbox (default true, required)
- [ ] GDPR notice displayed for consent fields

---

### REQ-FE-CUS-013: Form Submission and Error Handling

**Requirement:** The system shall handle form submission with loading states and comprehensive error handling.

**Acceptance Criteria:**
- [ ] Submit button shows loading spinner during submission
- [ ] Form disabled during submission
- [ ] Success: snackbar message for 3 seconds
- [ ] Success: navigate to customer list
- [ ] Error: snackbar message with error details for 5 seconds
- [ ] Error: form remains editable for corrections
- [ ] Validation errors: form.markAllAsTouched() to show all errors
- [ ] Network errors: retry option provided
- [ ] 409 Conflict: specific message for duplicate email

---

## 4. Contact Management Requirements

### REQ-FE-CUS-014: Contact List Display

**Requirement:** The system shall display customer contacts in a table with management actions.

**Acceptance Criteria:**
- [ ] Material table with columns: name, email, phone, position, isPrimary, tags, actions
- [ ] Primary contact indicated with star icon and badge
- [ ] Tags displayed as small chips
- [ ] Add Contact button at top of list
- [ ] Import Contacts button for bulk operations
- [ ] Export Contacts button for data export
- [ ] Edit action opens contact form dialog
- [ ] Delete action shows confirmation dialog
- [ ] Empty state when no contacts exist
- [ ] Maximum 50 contacts enforced with warning

**Contact Display:**

```typescript
displayedColumns = [
  'name',
  'email',
  'phone',
  'position',
  'isPrimary',
  'tags',
  'actions'
];
```

---

### REQ-FE-CUS-015: Contact Form Dialog

**Requirement:** The system shall provide a modal dialog for adding and editing contacts.

**Acceptance Criteria:**
- [ ] Dialog width: 600px
- [ ] First name field: required
- [ ] Last name field: required
- [ ] Email field: required, email validation, async uniqueness check
- [ ] Phone field: optional, phone format validation
- [ ] Position field: optional
- [ ] Primary contact checkbox
- [ ] Tags input: chip list with autocomplete
- [ ] Save button disabled when form invalid
- [ ] Cancel button closes without saving
- [ ] Edit mode pre-populates all fields
- [ ] Create mode sets isPrimary true if first contact

---

### REQ-FE-CUS-016: Contact Tags Management

**Requirement:** The system shall provide tag management with autocomplete and visual feedback.

**Acceptance Criteria:**
- [ ] Material chip list for tag display
- [ ] Autocomplete input for adding tags
- [ ] Tags are alphanumeric with hyphens/underscores
- [ ] Maximum 50 characters per tag
- [ ] Remove tag by clicking X on chip
- [ ] Tag suggestions based on existing tags
- [ ] Case-insensitive tag matching
- [ ] Duplicate tags prevented
- [ ] Visual feedback for tag operations

---

### REQ-FE-CUS-017: Contact Import Dialog

**Requirement:** The system shall provide contact import functionality with file upload and field mapping.

**Acceptance Criteria:**
- [ ] Dialog width: 800px
- [ ] File upload dropzone (drag-and-drop)
- [ ] Accepts CSV, Excel (.xlsx), JSON formats
- [ ] Maximum file size: 10MB
- [ ] File preview showing first 5 rows
- [ ] Field mapping interface
- [ ] Required field validation (firstName, lastName, email)
- [ ] Import progress indicator
- [ ] Error report for failed records
- [ ] Success summary with counts
- [ ] Download error report button if failures

**Import Steps:**

1. File selection
2. File validation
3. Field mapping
4. Preview and confirm
5. Import execution
6. Results summary

---

### REQ-FE-CUS-018: Contact Export Functionality

**Requirement:** The system shall provide contact export with filtering and format selection.

**Acceptance Criteria:**
- [ ] Export dialog with format selection (CSV, Excel, JSON)
- [ ] Field selection checkboxes (select which fields to export)
- [ ] Tag filter (export only contacts with specific tags)
- [ ] Include headers option for CSV/Excel
- [ ] Export button triggers download
- [ ] File naming: {companyName}_contacts_{date}.{format}
- [ ] Progress indicator for large exports
- [ ] Success notification with file size
- [ ] Maximum 50,000 contacts per export

---

## 5. Communication History Requirements

### REQ-FE-CUS-019: Communication History Timeline

**Requirement:** The system shall display communication history in a chronological timeline with filtering.

**Acceptance Criteria:**
- [ ] Timeline view with Material components
- [ ] Communications sorted by date (most recent first)
- [ ] Each entry shows type icon (email, phone, SMS, meeting)
- [ ] Entry displays: type, subject/summary, date/time, status
- [ ] Color coding by type (email: blue, phone: green, SMS: orange, meeting: purple)
- [ ] Click entry to view full details
- [ ] Filter by communication type (multi-select)
- [ ] Filter by date range (start and end date pickers)
- [ ] Empty state when no communications found
- [ ] Pagination for large histories

---

### REQ-FE-CUS-020: Log Phone Call Dialog

**Requirement:** The system shall provide a dialog for logging phone call details.

**Acceptance Criteria:**
- [ ] Dialog width: 600px
- [ ] Phone number field: required, validated
- [ ] Call type: radio buttons (Inbound/Outbound)
- [ ] Duration: time input in minutes and seconds
- [ ] Call time: datetime picker (defaults to now)
- [ ] Summary field: required, minimum 10 characters, textarea
- [ ] Notes field: optional, textarea
- [ ] Save button creates communication record
- [ ] Validation prevents submission of incomplete data
- [ ] Success notification after save

---

### REQ-FE-CUS-021: Schedule Meeting Dialog

**Requirement:** The system shall provide meeting scheduling with calendar integration.

**Acceptance Criteria:**
- [ ] Dialog width: 700px
- [ ] Title field: required
- [ ] Description field: optional, rich text editor
- [ ] Start date/time: required, datetime picker
- [ ] End date/time: required, datetime picker, must be after start
- [ ] Location field: required for in-person meetings
- [ ] Virtual meeting toggle
- [ ] Meeting link field: required when virtual
- [ ] Attendees: chip list with email validation
- [ ] Calendar integration checkbox (send invites)
- [ ] Save creates meeting and sends invites
- [ ] Validation enforces all requirements

---

### REQ-FE-CUS-022: Send Email Dialog

**Requirement:** The system shall provide email composition with template support and attachments.

**Acceptance Criteria:**
- [ ] Dialog width: 800px
- [ ] To field: pre-filled with customer primary email
- [ ] Subject field: required
- [ ] Body field: rich text editor (Quill)
- [ ] Template selector: dropdown with predefined templates
- [ ] Selecting template populates subject and body
- [ ] Template variables replaced ({{companyName}}, {{contactName}})
- [ ] Attachment upload: max 5 files, 10MB total
- [ ] Attachment list with remove option
- [ ] Send button: validates and sends email
- [ ] Send status indicator
- [ ] Success/error notification

**Rich Text Editor Features:**

- Bold, italic, underline
- Bullet and numbered lists
- Links
- Text alignment
- Font size and color

---

### REQ-FE-CUS-023: Send SMS Dialog

**Requirement:** The system shall provide SMS composition with character counting.

**Acceptance Criteria:**
- [ ] Dialog width: 500px
- [ ] To field: phone number, pre-filled with customer primary phone
- [ ] Message field: textarea, max 1600 characters
- [ ] Character counter displayed (updates in real-time)
- [ ] SMS part indicator (1 SMS = 160 chars)
- [ ] Template selector for predefined messages
- [ ] Template variables replaced
- [ ] Send button validates and sends
- [ ] Warning if message > 160 characters (multi-part)
- [ ] Success/error notification

**Character Counter Display:**

```
Characters: 145 / 1600 (1 SMS)
Characters: 320 / 1600 (2 SMS)
```

---

## 6. Complaint Management Requirements

### REQ-FE-CUS-024: Complaint List Display

**Requirement:** The system shall display customer complaints in a table with status tracking.

**Acceptance Criteria:**
- [ ] Material table with columns: complaintNumber, subject, category, priority, status, createdAt, actions
- [ ] Priority indicated with color-coded badges (Critical: red, High: orange, Medium: yellow, Low: green)
- [ ] Status shown as chips with appropriate colors
- [ ] Click row to view complaint details
- [ ] Filter by status (multi-select)
- [ ] Filter by priority (multi-select)
- [ ] Sort by date, priority, status
- [ ] Create Complaint button at top
- [ ] Empty state when no complaints exist
- [ ] Complaint count badge on tab

**Status Colors:**

| Status | Color |
|--------|-------|
| New | Blue |
| InProgress | Orange |
| Resolved | Green |
| Closed | Grey |
| Escalated | Red |

---

### REQ-FE-CUS-025: Complaint Detail Dialog

**Requirement:** The system shall provide detailed complaint view with status management.

**Acceptance Criteria:**
- [ ] Dialog width: 800px
- [ ] Displays: complaintNumber, subject, description, category, priority
- [ ] Shows: customer info, createdAt, assignedTo, status
- [ ] Displays attachments with download links
- [ ] Status history timeline
- [ ] Update Status button (only for assigned user/managers)
- [ ] Resolve button (opens resolution dialog)
- [ ] Escalate button (requires confirmation)
- [ ] Add Comment section for notes
- [ ] All timestamps display in user's timezone
- [ ] Print button for complaint report

---

### REQ-FE-CUS-026: Resolve Complaint Dialog

**Requirement:** The system shall provide complaint resolution workflow with satisfaction tracking.

**Acceptance Criteria:**
- [ ] Dialog width: 600px
- [ ] Resolution description: required, textarea, min 20 characters
- [ ] Compensation offered: optional, text input
- [ ] Customer satisfied: required, yes/no radio buttons
- [ ] Resolution automatically calculated and displayed
- [ ] Resolve button submits resolution
- [ ] Success creates follow-up if customer not satisfied
- [ ] Status automatically changes to Resolved
- [ ] Email notification sent to customer
- [ ] Validation prevents incomplete submissions

---

### REQ-FE-CUS-027: Submit Complaint Dialog

**Requirement:** The system shall provide complaint submission form with categorization.

**Acceptance Criteria:**
- [ ] Dialog width: 700px
- [ ] Subject field: required, max 500 characters
- [ ] Description field: required, textarea, max 5000 characters
- [ ] Category dropdown: optional, predefined categories
- [ ] Priority: required, radio buttons (Low, Medium, High, Critical)
- [ ] Related event: optional, searchable dropdown
- [ ] Attachment upload: max 5 files, 20MB total
- [ ] Character counters for subject and description
- [ ] Submit button validates and creates complaint
- [ ] Complaint number generated and displayed
- [ ] Success notification with complaint number

**Complaint Categories:**

- Event Quality
- Customer Service
- Billing Issue
- Technical Problem
- Accessibility
- Other

---

## 7. Customer Insights Requirements

### REQ-FE-CUS-028: Customer Insights Dashboard

**Requirement:** The system shall display AI-generated customer insights with visualizations.

**Acceptance Criteria:**
- [ ] Insights refreshed every 24 hours
- [ ] Manual refresh button available
- [ ] Sentiment score displayed as gauge chart (0-100)
- [ ] Engagement level: visual indicator (Low/Medium/High)
- [ ] Churn risk: percentage with color coding (<30% green, 30-60% orange, >60% red)
- [ ] Recommended actions: bullet list
- [ ] Key interests: tag cloud
- [ ] Preferred communication times: timeline chart
- [ ] Last interaction date displayed
- [ ] Total interactions count
- [ ] Loading state during AI processing
- [ ] Fallback message if AI unavailable

**Visualizations:**

| Insight | Chart Type | Library |
|---------|-----------|---------|
| Sentiment Score | Gauge | ng2-charts |
| Engagement Level | Progress Bar | Material |
| Churn Risk | Radial Progress | ng2-charts |
| Interests | Tag Cloud | Custom |
| Communication Times | Timeline | ng2-charts |

---

### REQ-FE-CUS-029: Sentiment Analysis Display

**Requirement:** The system shall display sentiment analysis with visual indicators.

**Acceptance Criteria:**
- [ ] Overall sentiment score (0-100)
- [ ] Sentiment breakdown: positive, neutral, negative percentages
- [ ] Pie chart visualization
- [ ] Color coding: green (positive), grey (neutral), red (negative)
- [ ] Trend indicator (up/down arrow with percentage change)
- [ ] Based on analysis of recent communications
- [ ] Tooltip explains calculation method
- [ ] Click for detailed sentiment history

---

### REQ-FE-CUS-030: Recommended Actions List

**Requirement:** The system shall display AI-recommended actions with priority and actionability.

**Acceptance Criteria:**
- [ ] Actions displayed as expandable cards
- [ ] Each action shows: title, description, priority, suggested date
- [ ] Priority color coding
- [ ] "Take Action" button for each recommendation
- [ ] Clicking button opens relevant dialog (email, meeting, etc.)
- [ ] Mark as done option
- [ ] Dismiss option with feedback
- [ ] Actions sorted by priority
- [ ] Maximum 5 recommendations displayed
- [ ] Refresh recommendations button

**Recommended Action Types:**

- Send follow-up email
- Schedule check-in call
- Offer upgrade/premium service
- Address potential churn risk
- Request testimonial
- Resolve outstanding complaint

---

## 8. State Management Requirements

### REQ-FE-CUS-031: NgRx Store Configuration

**Requirement:** The system shall implement NgRx for state management with entities, effects, and selectors.

**Acceptance Criteria:**
- [ ] Feature state registered in app module
- [ ] Entity adapters for customers, contacts, communications, complaints
- [ ] State interface includes: entities, selectedId, loading, error, filters, pagination
- [ ] Actions for all CRUD operations
- [ ] Success and failure actions for each operation
- [ ] Effects handle API calls and side effects
- [ ] Selectors for all state slices
- [ ] Memoized selectors for derived state
- [ ] Dev tools integration for debugging

**State Structure:**

```typescript
interface CustomerManagementState {
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

---

### REQ-FE-CUS-032: Customer Actions

**Requirement:** The system shall define comprehensive actions for customer operations.

**Acceptance Criteria:**
- [ ] Load Customers action with query parameters
- [ ] Load Customers Success with customers array and total count
- [ ] Load Customers Failure with error message
- [ ] Create Customer with customer DTO
- [ ] Update Customer with id and partial changes
- [ ] Deactivate Customer with id and reason
- [ ] Search Customers with query string
- [ ] Filter Customers with filter criteria
- [ ] All actions use createAction from NgRx
- [ ] Actions use props for type safety

---

### REQ-FE-CUS-033: Customer Effects

**Requirement:** The system shall implement effects for handling side effects and API calls.

**Acceptance Criteria:**
- [ ] Load customers effect calls service and dispatches success/failure
- [ ] Create customer effect calls service, navigates on success
- [ ] Update customer effect calls service, shows notification
- [ ] Delete customer effect calls service, updates list
- [ ] All effects use switchMap for cancellable operations
- [ ] Error handling with catchError
- [ ] Success notifications via snackbar service
- [ ] Router navigation for create/update success
- [ ] Effects properly typed with Actions type

**Effect Pattern:**

```typescript
loadCustomers$ = createEffect(() =>
  this.actions$.pipe(
    ofType(loadCustomers),
    switchMap(({ params }) =>
      this.customerService.getCustomers(params).pipe(
        map(response => loadCustomersSuccess({
          customers: response.items,
          totalCount: response.totalCount
        })),
        catchError(error => of(loadCustomersFailure({
          error: error.message
        })))
      )
    )
  )
);
```

---

### REQ-FE-CUS-034: Customer Selectors

**Requirement:** The system shall provide selectors for accessing state slices efficiently.

**Acceptance Criteria:**
- [ ] Feature selector for customer management state
- [ ] Entity selectors using adapter selectors (selectAll, selectEntities, selectIds)
- [ ] Selected customer selector
- [ ] Loading state selector
- [ ] Error state selector
- [ ] Filtered customers selector
- [ ] Total count selector
- [ ] All selectors memoized for performance
- [ ] Selectors properly typed

**Selector Examples:**

```typescript
export const selectAllCustomers = createSelector(
  selectCustomerState,
  adapter.getSelectors().selectAll
);

export const selectCustomerById = (id: string) => createSelector(
  selectCustomerState,
  state => state.entities[id]
);

export const selectLoading = createSelector(
  selectCustomerState,
  state => state.loading
);
```

---

### REQ-FE-CUS-035: Facade Service

**Requirement:** The system shall provide a facade service to simplify component interaction with the store.

**Acceptance Criteria:**
- [ ] Facade exposes observables for all state slices
- [ ] Facade provides methods for dispatching actions
- [ ] Methods return observables for async operations
- [ ] Type-safe method signatures
- [ ] Facade injected into components
- [ ] Centralized place for store interactions
- [ ] Reduces boilerplate in components
- [ ] Methods document expected behavior

**Facade Interface:**

```typescript
@Injectable()
export class CustomerFacade {
  customers$ = this.store.select(selectAllCustomers);
  loading$ = this.store.select(selectLoading);
  selectedCustomer$ = this.store.select(selectSelectedCustomer);

  loadCustomers(params: CustomerQueryParams): void;
  createCustomer(customer: CreateCustomerDto): Observable<void>;
  updateCustomer(id: string, changes: Partial<Customer>): Observable<void>;
  deactivateCustomer(id: string, reason: string): Observable<void>;
}
```

---

## 9. Routing Requirements

### REQ-FE-CUS-036: Route Configuration

**Requirement:** The system shall define routes for customer management with guards and resolvers.

**Acceptance Criteria:**
- [ ] Customer list route: /customers
- [ ] Customer detail route: /customers/:id
- [ ] Customer create route: /customers/new
- [ ] Customer edit route: /customers/:id/edit
- [ ] Customer dashboard route: /customers/dashboard
- [ ] All routes protected by AuthGuard
- [ ] Customer management routes protected by CustomerManagementGuard
- [ ] Detail and edit routes use CustomerResolver
- [ ] Routes use data property for metadata (title, breadcrumbs)
- [ ] Lazy loaded module for performance

**Route Configuration:**

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
      }
    ]
  }
];
```

---

### REQ-FE-CUS-037: Route Guards

**Requirement:** The system shall implement guards to protect routes based on permissions.

**Acceptance Criteria:**
- [ ] CustomerManagementGuard checks customer-management permission
- [ ] Guard implements CanActivate interface
- [ ] Guard uses AuthService to check permissions
- [ ] Unauthorized users redirected to /unauthorized
- [ ] Guard returns Observable<boolean>
- [ ] Navigation cancelled if unauthorized
- [ ] Guard integrated with Angular router

**Guard Implementation:**

```typescript
@Injectable()
export class CustomerManagementGuard implements CanActivate {
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

### REQ-FE-CUS-038: Route Resolver

**Requirement:** The system shall implement resolver to pre-load customer data before route activation.

**Acceptance Criteria:**
- [ ] CustomerResolver implements Resolve interface
- [ ] Resolver loads customer data by ID from route params
- [ ] Resolved data available in component via ActivatedRoute.data
- [ ] Resolver handles errors gracefully
- [ ] Failed resolution redirects to customer list
- [ ] Loading indicator shown during resolution
- [ ] Resolver prevents route activation until data loaded

---

## 10. Validation Requirements

### REQ-FE-CUS-039: Email Validation

**Requirement:** The system shall validate email addresses with format and uniqueness checks.

**Acceptance Criteria:**
- [ ] Email format validated using Validators.email
- [ ] Async validation checks email uniqueness
- [ ] API call debounced by 300ms
- [ ] Validation errors: 'required', 'email', 'emailTaken'
- [ ] Error messages displayed below field
- [ ] Validation occurs on blur and value change
- [ ] Loading indicator during async validation

**Async Validator:**

```typescript
emailFormControl = new FormControl(
  '',
  [Validators.required, Validators.email],
  [CustomerValidators.uniqueEmail(this.customerService)]
);
```

---

### REQ-FE-CUS-040: Phone Number Validation

**Requirement:** The system shall validate phone numbers using international format.

**Acceptance Criteria:**
- [ ] Phone format validated using custom validator
- [ ] Accepts E.164 format: +[country][number]
- [ ] Special characters allowed: + - ( ) space
- [ ] Validation errors: 'required', 'invalidPhone'
- [ ] Error message: "Please enter a valid phone number"
- [ ] Example shown: +1-555-123-4567
- [ ] Validation on blur and value change

**Phone Validator:**

```typescript
static phoneNumber(control: AbstractControl): ValidationErrors | null {
  const phoneRegex = /^\+?[1-9]\d{1,14}$/;
  return phoneRegex.test(control.value) ? null : { invalidPhone: true };
}
```

---

### REQ-FE-CUS-041: Form Error Messages

**Requirement:** The system shall display context-specific error messages for validation failures.

**Acceptance Criteria:**
- [ ] Error messages displayed using mat-error
- [ ] Messages shown when field touched and invalid
- [ ] Custom messages for each validation type
- [ ] Dynamic messages with validation parameters
- [ ] Error messages match backend validation
- [ ] Consistent error styling across forms
- [ ] ARIA labels for accessibility

**Error Messages Map:**

```typescript
FORM_ERROR_MESSAGES = {
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

### REQ-FE-CUS-042: Custom Validators

**Requirement:** The system shall provide reusable custom validators for complex validation scenarios.

**Acceptance Criteria:**
- [ ] Validators follow Angular validator pattern
- [ ] Validators return ValidationErrors or null
- [ ] Async validators return Observable
- [ ] Validators are reusable across forms
- [ ] Validators properly typed
- [ ] Validators documented with examples

---

## 11. UI/UX Requirements

### REQ-FE-CUS-043: Material Design Theme

**Requirement:** The system shall implement consistent Material Design theme across all components.

**Acceptance Criteria:**
- [ ] Primary color: Indigo (#3f51b5)
- [ ] Accent color: Pink (#ff4081)
- [ ] Warn color: Red (#f44336)
- [ ] Custom palette defined for brand colors
- [ ] Typography config uses Roboto font
- [ ] Component density set to 0 (default)
- [ ] Light theme by default
- [ ] Dark theme available as option
- [ ] Theme applied to all Material components

**Theme Configuration:**

```scss
$custom-primary: mat.define-palette(mat.$indigo-palette);
$custom-accent: mat.define-palette(mat.$pink-palette);
$custom-warn: mat.define-palette(mat.$red-palette);

$custom-theme: mat.define-light-theme((
  color: (
    primary: $custom-primary,
    accent: $custom-accent,
    warn: $custom-warn,
  )
));
```

---

### REQ-FE-CUS-044: Responsive Layout

**Requirement:** The system shall provide responsive layouts that adapt to different screen sizes.

**Acceptance Criteria:**
- [ ] Mobile-first design approach
- [ ] Breakpoints: xs (0), sm (600px), md (960px), lg (1280px), xl (1920px)
- [ ] Tables switch to cards on mobile
- [ ] Side navigation becomes overlay on mobile
- [ ] Form layouts stack on mobile
- [ ] Touch-friendly button sizes on mobile (min 44px)
- [ ] Readable font sizes on all devices
- [ ] Proper spacing and padding for touch targets

**Responsive Grid:**

```scss
.customer-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 16px;

  @media (max-width: 600px) {
    grid-template-columns: 1fr;
  }
}
```

---

### REQ-FE-CUS-045: Loading States

**Requirement:** The system shall provide clear loading indicators for all asynchronous operations.

**Acceptance Criteria:**
- [ ] Material spinner for full-page loads
- [ ] Progress bar for partial loads
- [ ] Skeleton screens for list loading
- [ ] Disabled buttons with spinner during submission
- [ ] Loading overlay for dialogs
- [ ] Minimum display time: 300ms (prevents flicker)
- [ ] Loading state from store observable
- [ ] Accessible loading announcements

**Loading Patterns:**

| Operation | Indicator |
|-----------|-----------|
| Page load | Full page spinner |
| List load | Skeleton rows |
| Form submit | Button spinner |
| Dialog load | Overlay spinner |
| Inline action | Mini spinner |

---

### REQ-FE-CUS-046: Empty States

**Requirement:** The system shall display helpful empty states when no data is available.

**Acceptance Criteria:**
- [ ] Empty state message displayed when list has no items
- [ ] Icon or illustration for visual appeal
- [ ] Helpful message explaining why list is empty
- [ ] Call-to-action button when appropriate (e.g., "Add First Customer")
- [ ] Different messages for filtered vs truly empty
- [ ] Consistent styling across all empty states

**Empty State Messages:**

| Context | Message | Action |
|---------|---------|--------|
| No customers | "No customers yet. Create your first customer to get started." | Add Customer |
| No search results | "No customers match your search. Try different keywords." | Clear Search |
| No contacts | "No contacts added yet. Add a contact to get started." | Add Contact |
| No communications | "No communication history. Send an email or log a call." | - |

---

### REQ-FE-CUS-047: Notification System

**Requirement:** The system shall provide consistent notifications for user feedback.

**Acceptance Criteria:**
- [ ] Material snackbar for notifications
- [ ] Success notifications: green background, 3 second duration
- [ ] Error notifications: red background, 5 second duration
- [ ] Info notifications: blue background, 3 second duration
- [ ] Action button option (e.g., "Undo")
- [ ] Notifications stack vertically
- [ ] Maximum 3 simultaneous notifications
- [ ] Dismiss button on each notification
- [ ] Accessible announcements for screen readers

**Notification Types:**

```typescript
showSuccess(message: string): void {
  this.snackBar.open(message, 'Close', {
    duration: 3000,
    panelClass: ['success-snackbar']
  });
}

showError(message: string): void {
  this.snackBar.open(message, 'Close', {
    duration: 5000,
    panelClass: ['error-snackbar']
  });
}
```

---

## 12. Performance Requirements

### REQ-FE-CUS-048: Change Detection Strategy

**Requirement:** The system shall use OnPush change detection strategy for optimal performance.

**Acceptance Criteria:**
- [ ] All components use ChangeDetectionStrategy.OnPush
- [ ] Component inputs are immutable
- [ ] State updates trigger change detection via observables
- [ ] Manual change detection when needed
- [ ] Trackby functions for ngFor loops
- [ ] Reduces unnecessary change detection cycles
- [ ] Performance improvement validated with profiler

**OnPush Implementation:**

```typescript
@Component({
  selector: 'app-customer-list',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomerListComponent {
  trackByCustomerId(index: number, customer: Customer): string {
    return customer.id;
  }
}
```

---

### REQ-FE-CUS-049: Lazy Loading

**Requirement:** The system shall implement lazy loading for feature modules.

**Acceptance Criteria:**
- [ ] Customer management module lazy loaded
- [ ] Route configuration uses loadChildren
- [ ] Module only loaded when route accessed
- [ ] Separate bundle created for customer module
- [ ] Initial bundle size reduced
- [ ] Faster initial page load
- [ ] Code splitting verified in build output

**Lazy Loading Configuration:**

```typescript
{
  path: 'customers',
  loadChildren: () => import('./customer-management/customer-management.module')
    .then(m => m.CustomerManagementModule)
}
```

---

### REQ-FE-CUS-050: Virtual Scrolling

**Requirement:** The system shall use virtual scrolling for large lists to improve performance.

**Acceptance Criteria:**
- [ ] CDK Virtual Scroll for lists > 100 items
- [ ] Item size configured appropriately
- [ ] Buffer size optimized for smooth scrolling
- [ ] Scroll position maintained during updates
- [ ] Works with filtering and sorting
- [ ] Reduces DOM nodes for better performance
- [ ] Smooth scrolling experience

**Virtual Scroll Configuration:**

```html
<cdk-virtual-scroll-viewport itemSize="48" class="customer-list-viewport">
  <div *cdkVirtualFor="let customer of customers$ | async; trackBy: trackByCustomerId">
    <!-- Customer row -->
  </div>
</cdk-virtual-scroll-viewport>
```

---

### REQ-FE-CUS-051: Bundle Size Optimization

**Requirement:** The system shall optimize bundle size for faster load times.

**Acceptance Criteria:**
- [ ] Initial bundle < 500KB
- [ ] Lazy loaded chunks < 200KB each
- [ ] Production build uses AOT compilation
- [ ] Build optimization enabled
- [ ] Unused code eliminated via tree shaking
- [ ] Source maps generated for debugging
- [ ] Bundle analysis performed regularly

**Build Configuration:**

```json
{
  "optimization": true,
  "outputHashing": "all",
  "sourceMap": false,
  "extractCss": true,
  "namedChunks": false,
  "aot": true,
  "buildOptimizer": true
}
```

---

## 13. Accessibility Requirements

### REQ-FE-CUS-052: ARIA Attributes

**Requirement:** The system shall implement ARIA attributes for screen reader accessibility.

**Acceptance Criteria:**
- [ ] All interactive elements have aria-label
- [ ] Form fields have aria-describedby for errors
- [ ] Loading states announced with aria-live
- [ ] Dialog roles properly defined
- [ ] Landmark roles for page sections
- [ ] Table headers associated with cells
- [ ] Button purposes clearly labeled
- [ ] WCAG 2.1 Level AA compliance

**ARIA Examples:**

```html
<button aria-label="Add new customer" (click)="addCustomer()">
  <mat-icon>add</mat-icon>
</button>

<div aria-live="polite" aria-atomic="true" *ngIf="loading">
  Loading customers...
</div>

<mat-error role="alert" aria-live="assertive">
  Email is required
</mat-error>
```

---

### REQ-FE-CUS-053: Keyboard Navigation

**Requirement:** The system shall support full keyboard navigation.

**Acceptance Criteria:**
- [ ] All features accessible via keyboard
- [ ] Tab order follows logical flow
- [ ] Enter key submits forms
- [ ] Escape key closes dialogs
- [ ] Arrow keys navigate lists
- [ ] Space key toggles checkboxes
- [ ] Focus indicators visible
- [ ] No keyboard traps
- [ ] Skip links for main content

**Keyboard Shortcuts:**

| Key | Action |
|-----|--------|
| Tab | Move to next element |
| Shift+Tab | Move to previous element |
| Enter | Submit form / Activate button |
| Escape | Close dialog / Cancel |
| Space | Toggle checkbox / Select |
| Arrow keys | Navigate list |

---

### REQ-FE-CUS-054: Color Contrast

**Requirement:** The system shall maintain sufficient color contrast for readability.

**Acceptance Criteria:**
- [ ] Normal text: 4.5:1 contrast ratio minimum
- [ ] Large text: 3:1 contrast ratio minimum
- [ ] UI components: 3:1 contrast ratio minimum
- [ ] Focus indicators: 3:1 contrast ratio minimum
- [ ] Status indicators use icons in addition to color
- [ ] Error states indicated by more than just color
- [ ] Contrast ratios verified with accessibility tools

**Contrast Ratios:**

| Element | Foreground | Background | Ratio |
|---------|------------|------------|-------|
| Body text | #000000 | #FFFFFF | 21:1 |
| Primary button | #FFFFFF | #3f51b5 | 4.6:1 |
| Error text | #d32f2f | #FFFFFF | 5.5:1 |

---

### REQ-FE-CUS-055: Focus Management

**Requirement:** The system shall manage focus appropriately for modal dialogs and dynamic content.

**Acceptance Criteria:**
- [ ] Dialog opening moves focus to first focusable element
- [ ] Dialog closing returns focus to trigger element
- [ ] Focus trapped within modal dialogs
- [ ] Dynamic content announcements for screen readers
- [ ] Focus moved to error fields on validation failure
- [ ] Focus visible indicator always shown
- [ ] Focus order logical and predictable

---

## 14. Testing Requirements

### REQ-FE-CUS-056: Unit Testing

**Requirement:** The system shall maintain comprehensive unit test coverage for components and services.

**Acceptance Criteria:**
- [ ] Minimum 80% code coverage
- [ ] All components have unit tests
- [ ] All services have unit tests
- [ ] All pipes have unit tests
- [ ] All validators have unit tests
- [ ] Tests use Jasmine framework
- [ ] Tests run via Karma
- [ ] Mocked dependencies using spies
- [ ] Tests cover success and error scenarios
- [ ] Tests automated in CI/CD pipeline

**Test Example:**

```typescript
describe('CustomerListComponent', () => {
  let component: CustomerListComponent;
  let fixture: ComponentFixture<CustomerListComponent>;
  let mockFacade: jasmine.SpyObj<CustomerFacade>;

  beforeEach(() => {
    mockFacade = jasmine.createSpyObj('CustomerFacade',
      ['loadCustomers', 'searchCustomers']
    );
    mockFacade.customers$ = of([]);

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

---

### REQ-FE-CUS-057: E2E Testing

**Requirement:** The system shall include end-to-end tests for critical user workflows.

**Acceptance Criteria:**
- [ ] E2E tests use Cypress framework
- [ ] Tests cover customer creation workflow
- [ ] Tests cover customer editing workflow
- [ ] Tests cover contact management
- [ ] Tests cover complaint submission
- [ ] Tests verify navigation flows
- [ ] Tests check form validation
- [ ] Tests run in CI/CD pipeline
- [ ] Tests use data-cy attributes for selectors

**E2E Test Example:**

```typescript
describe('Customer Management', () => {
  beforeEach(() => {
    cy.login('admin@example.com', 'password');
    cy.visit('/customers');
  });

  it('should create a new customer', () => {
    cy.get('[data-cy=new-customer-btn]').click();
    cy.get('[data-cy=company-name]').type('Test Company');
    cy.get('[data-cy=type]').click();
    cy.get('mat-option').contains('SmallBusiness').click();
    cy.get('[data-cy=primary-email]').type('test@company.com');
    cy.get('[data-cy=primary-phone]').type('+1234567890');
    cy.get('[data-cy=street]').type('123 Main St');
    cy.get('[data-cy=city]').type('New York');
    cy.get('[data-cy=state]').type('NY');
    cy.get('[data-cy=zip]').type('10001');
    cy.get('[data-cy=country]').type('USA');
    cy.get('[data-cy=submit-btn]').click();

    cy.contains('Customer created successfully');
    cy.url().should('include', '/customers');
  });
});
```

---

### REQ-FE-CUS-058: Component Testing

**Requirement:** The system shall test components in isolation with proper mocking.

**Acceptance Criteria:**
- [ ] Each component has dedicated test file
- [ ] Services mocked using jasmine.createSpyObj
- [ ] Observables mocked with 'of' operator
- [ ] Component inputs tested
- [ ] Component outputs tested
- [ ] DOM manipulation tested
- [ ] Event handlers tested
- [ ] Async operations tested with fakeAsync/tick
- [ ] Material components properly mocked

---

### REQ-FE-CUS-059: Service Testing

**Requirement:** The system shall test services with mocked HTTP client.

**Acceptance Criteria:**
- [ ] All service methods tested
- [ ] HTTP requests verified with HttpTestingController
- [ ] Request URLs validated
- [ ] Request bodies validated
- [ ] Response handling tested
- [ ] Error handling tested
- [ ] Query parameters validated
- [ ] Headers validated
- [ ] Observable completion tested

**Service Test Example:**

```typescript
describe('CustomerService', () => {
  let service: CustomerService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CustomerService]
    });

    service = TestBed.inject(CustomerService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should get customers', () => {
    const mockResponse = {
      items: [{ id: '1', companyName: 'Test' }],
      totalCount: 1
    };

    service.getCustomers({ page: 0, pageSize: 20 }).subscribe(response => {
      expect(response.items.length).toBe(1);
      expect(response.totalCount).toBe(1);
    });

    const req = httpMock.expectOne(req =>
      req.url.includes('/api/v1/customers')
    );
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  afterEach(() => {
    httpMock.verify();
  });
});
```

---

### REQ-FE-CUS-060: Visual Regression Testing

**Requirement:** The system should implement visual regression testing for UI consistency.

**Acceptance Criteria:**
- [ ] Screenshot tests for major components
- [ ] Visual diffs detected automatically
- [ ] Tests run on different viewport sizes
- [ ] Theme variations tested (light/dark)
- [ ] Baseline images maintained
- [ ] Visual changes require approval
- [ ] Integration with CI/CD pipeline

---

## Appendix A: Component Architecture

### Component Tree

```
CustomerManagementModule
├── CustomerListComponent (Smart)
│   ├── CustomerTableComponent (Dumb)
│   ├── CustomerFilterComponent (Dumb)
│   └── CustomerSearchComponent (Dumb)
├── CustomerDetailComponent (Smart)
│   ├── CustomerProfileComponent (Dumb)
│   ├── ContactListComponent (Smart)
│   │   └── ContactFormComponent (Dumb)
│   ├── CommunicationHistoryComponent (Smart)
│   │   ├── EmailDialogComponent (Dumb)
│   │   ├── SmsDialogComponent (Dumb)
│   │   ├── PhoneCallDialogComponent (Dumb)
│   │   └── MeetingDialogComponent (Dumb)
│   ├── ComplaintListComponent (Smart)
│   │   ├── ComplaintDetailComponent (Dumb)
│   │   └── ResolveComplaintDialogComponent (Dumb)
│   ├── TestimonialListComponent (Smart)
│   └── CustomerInsightsComponent (Smart)
└── CustomerFormComponent (Smart)
```

---

## Appendix B: Service Dependencies

### Services

- **CustomerService**: API client for customer operations
- **ContactService**: API client for contact operations
- **CommunicationService**: API client for communication operations
- **ComplaintService**: API client for complaint operations
- **TestimonialService**: API client for testimonial operations
- **CustomerFacade**: State management facade
- **LocalStorageService**: Browser storage management
- **AuthService**: Authentication and authorization
- **NotificationService**: Snackbar notifications

---

## Appendix C: Data Models

### TypeScript Interfaces

```typescript
interface Customer {
  id: string;
  customerNumber: string;
  profile: CustomerProfile;
  contactInfo: CustomerContactInfo;
  preferences: CustomerPreferences;
  status: CustomerStatus;
  createdAt: Date;
  lastModifiedAt?: Date;
}

interface CustomerProfile {
  companyName: string;
  industry: string;
  type: CustomerType;
  segment: CustomerSegment;
  lifetimeValue: number;
  totalEvents: number;
  rating: string;
}

interface CustomerContactInfo {
  primaryEmail: string;
  secondaryEmail?: string;
  primaryPhone: string;
  secondaryPhone?: string;
  billingAddress: Address;
  shippingAddress?: Address;
  website?: string;
  socialMedia?: SocialMediaLinks;
}

interface Contact {
  id: string;
  customerId: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  position?: string;
  isPrimary: boolean;
  tags: string[];
  status: string;
}

interface Communication {
  id: string;
  customerId: string;
  type: CommunicationType;
  subject?: string;
  content?: string;
  recipientEmail?: string;
  recipientPhone?: string;
  status: string;
  createdAt: Date;
  createdBy: string;
}

interface Complaint {
  id: string;
  complaintNumber: string;
  customerId: string;
  subject: string;
  description: string;
  category: string;
  priority: ComplaintPriority;
  status: ComplaintStatus;
  createdAt: Date;
  resolvedAt?: Date;
}

interface CustomerInsights {
  customerId: string;
  sentimentScore: number;
  engagementLevel: string;
  churnRisk: number;
  recommendedActions: string[];
  keyInterests: string[];
  preferredCommunicationTimes: string[];
  generatedAt: Date;
}
```

---

## Document History

| Version | Date | Author | Description |
|---------|------|--------|-------------|
| 1.0.0 | 2025-12-22 | Frontend Architect | Initial structured requirements version |
