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

## 2. Equipment List Page Requirements

### REQ-FE-EQP-001: Display Equipment List

**Requirement:** The system shall display a paginated, filterable list of equipment items.

**Acceptance Criteria:**
- [ ] Equipment is displayed in responsive grid layout
- [ ] Each equipment card shows name, category, status, condition, and primary photo
- [ ] Grid switches to single column on mobile (< 600px)
- [ ] Grid shows 2 columns on tablet (600-960px)
- [ ] Grid shows 3-4 columns on desktop (> 960px)
- [ ] Pagination controls appear at bottom with page size selector
- [ ] Loading spinner displays while fetching data

### REQ-FE-EQP-002: Search Equipment

**Requirement:** The system shall provide real-time search functionality for equipment.

**Acceptance Criteria:**
- [ ] Search bar is prominently displayed in page header
- [ ] Search filters by equipment name, manufacturer, and model
- [ ] Search results update in real-time as user types
- [ ] Search is debounced to avoid excessive API calls
- [ ] Clear button appears when search has text
- [ ] Search persists when navigating back to list

### REQ-FE-EQP-003: Filter Equipment

**Requirement:** The system shall allow users to filter equipment by multiple criteria.

**Acceptance Criteria:**
- [ ] Filter panel includes status, condition, category, and availability filters
- [ ] Filters can be expanded/collapsed using expansion panel
- [ ] Multiple filters can be applied simultaneously
- [ ] Active filter count is displayed on filter button
- [ ] Clear all filters button is available when filters are active
- [ ] Filtered results update immediately

### REQ-FE-EQP-004: Sort Equipment List

**Requirement:** The system shall allow users to sort equipment by various fields.

**Acceptance Criteria:**
- [ ] Sort dropdown includes options for name, purchase date, condition, and value
- [ ] Sort direction (ascending/descending) can be toggled
- [ ] Current sort indicator is visible
- [ ] Sort selection persists across pagination
- [ ] Default sort is by name (ascending)

### REQ-FE-EQP-005: Category Tab Navigation

**Requirement:** The system shall provide quick category filtering via tabs.

**Acceptance Criteria:**
- [ ] Category tabs display all equipment categories (Table, Game, AudioVisual, etc.)
- [ ] All category tab shows unfiltered equipment
- [ ] Clicking tab filters equipment by selected category
- [ ] Active tab is visually highlighted
- [ ] Tab labels include equipment count per category
- [ ] Tabs are scrollable on mobile devices

### REQ-FE-EQP-006: Toggle Equipment View

**Requirement:** The system shall support multiple view modes for equipment list.

**Acceptance Criteria:**
- [ ] View toggle buttons support Grid, Table, and List views
- [ ] Grid view shows equipment cards with photos
- [ ] Table view shows tabular data with sortable columns
- [ ] List view shows condensed single-column layout
- [ ] View preference is saved in local storage
- [ ] View mode is restored on page reload

### REQ-FE-EQP-007: Display Availability Indicator

**Requirement:** The system shall display visual availability indicators on equipment cards.

**Acceptance Criteria:**
- [ ] Green indicator for Available status
- [ ] Yellow indicator for Reserved status
- [ ] Red indicator for InMaintenance or OutOfService
- [ ] Gray indicator for Retired status
- [ ] Tooltip shows next reservation date for reserved equipment
- [ ] Indicator updates in real-time when availability changes

### REQ-FE-EQP-008: Navigate to Equipment Detail

**Requirement:** The system shall allow users to navigate to equipment details.

**Acceptance Criteria:**
- [ ] Clicking equipment card navigates to detail page
- [ ] Detail link opens in new tab when middle-clicked or Ctrl+clicked
- [ ] Navigation preserves current list filters and page number
- [ ] Back button returns to list with preserved state

---

## 3. Equipment Detail Page Requirements

### REQ-FE-EQP-010: Display Equipment Details

**Requirement:** The system shall display comprehensive equipment information on detail page.

**Acceptance Criteria:**
- [ ] Header shows equipment name, status badge, and condition badge
- [ ] Photo gallery displays primary photo with thumbnail carousel
- [ ] Details card shows all equipment properties including manufacturer, model, serial number
- [ ] Specifications are displayed in key-value format
- [ ] Purchase information includes date, price, and current value
- [ ] Warehouse location is displayed if available
- [ ] Created and modified timestamps are shown

### REQ-FE-EQP-011: Equipment Detail Tabs

**Requirement:** The system shall organize equipment information into tabbed sections.

**Acceptance Criteria:**
- [ ] Tab navigation includes Reservations, Logistics, Maintenance, and Damage Reports
- [ ] Active tab content is displayed below tab bar
- [ ] Tab selection persists in URL query parameter
- [ ] Direct link to specific tab is supported
- [ ] Badge count shows number of items in each tab (e.g., "Reservations (3)")
- [ ] Empty state message displays when tab has no data

### REQ-FE-EQP-012: Display Reservations Calendar

**Requirement:** The system shall display current and upcoming reservations in calendar view.

**Acceptance Criteria:**
- [ ] Calendar shows reservations for selected equipment
- [ ] Reservations are color-coded by status (Confirmed, Requested, Cancelled)
- [ ] Clicking reservation shows details in popup
- [ ] Calendar supports month and week views
- [ ] Current date is highlighted
- [ ] Navigation controls allow moving between months/weeks

### REQ-FE-EQP-013: Display Logistics Status

**Requirement:** The system shall display current logistics status for equipment in transit.

**Acceptance Criteria:**
- [ ] Logistics timeline shows all stages: Packing, Loading, Dispatch, Delivery, Setup, Pickup, Return
- [ ] Completed stages are marked with checkmark and timestamp
- [ ] Current stage is highlighted
- [ ] Pending stages are grayed out
- [ ] Driver and truck information is displayed when available
- [ ] Logistics notes are shown for each stage

### REQ-FE-EQP-014: Display Maintenance History

**Requirement:** The system shall display complete maintenance history for equipment.

**Acceptance Criteria:**
- [ ] Maintenance records are listed in reverse chronological order
- [ ] Each record shows type, date, status, cost, and technician
- [ ] Scheduled maintenance is highlighted with upcoming badge
- [ ] Overdue maintenance is highlighted with warning badge
- [ ] Clicking maintenance record expands to show full details
- [ ] Filter controls allow filtering by maintenance type and status

### REQ-FE-EQP-015: Display Damage Reports

**Requirement:** The system shall display damage reports with photos.

**Acceptance Criteria:**
- [ ] Damage reports are listed in reverse chronological order
- [ ] Each report shows severity, description, date, and reported by
- [ ] Damage photos are displayed in gallery format
- [ ] Repair status and estimated cost are shown
- [ ] Clicking photo opens full-size lightbox view
- [ ] TotalLoss severity is prominently highlighted

### REQ-FE-EQP-016: Equipment Detail Action Buttons

**Requirement:** The system shall provide context-aware action buttons based on user role and equipment status.

**Acceptance Criteria:**
- [ ] Edit button is visible only to Warehouse Managers and Admins
- [ ] Upload Photo button is visible only to Warehouse Managers and Admins
- [ ] Reserve button is visible only when status is Available and user is Staff or higher
- [ ] Activate/Deactivate buttons are visible only to Managers and Admins
- [ ] Schedule Maintenance button is visible to Maintenance Techs and Managers
- [ ] Report Damage button is visible to all Staff roles
- [ ] Retire button is visible only to Managers and Admins
- [ ] Buttons are disabled with tooltip when action is not allowed

---

## 4. Equipment Form Requirements

### REQ-FE-EQP-020: Create Equipment Form

**Requirement:** The system shall provide a form for creating new equipment items.

**Acceptance Criteria:**
- [ ] Form includes all required fields: name, category, purchase date, purchase price
- [ ] Form includes optional fields: description, manufacturer, model, serial number, warehouse location
- [ ] Form provides condition selector with all condition options
- [ ] Form validates all inputs before submission
- [ ] Real-time validation feedback is displayed inline
- [ ] Submit button is disabled until form is valid
- [ ] Cancel button returns to equipment list with confirmation if form is dirty

### REQ-FE-EQP-021: Edit Equipment Form

**Requirement:** The system shall provide a form for editing existing equipment.

**Acceptance Criteria:**
- [ ] Form is pre-populated with current equipment data
- [ ] All editable fields can be updated
- [ ] Equipment ID and timestamps are read-only
- [ ] Form tracks dirty state to detect unsaved changes
- [ ] Save button is disabled if form is invalid or unchanged
- [ ] Unsaved changes warning appears on navigation attempt
- [ ] Success message displays after successful save

### REQ-FE-EQP-022: Validate Equipment Name

**Requirement:** The system shall validate equipment name uniqueness within category.

**Acceptance Criteria:**
- [ ] Name is required with max 200 characters
- [ ] Name uniqueness is validated against category on blur
- [ ] Error message displays if duplicate name exists in category
- [ ] Validation calls backend API to check uniqueness
- [ ] Validation is debounced to avoid excessive API calls
- [ ] Loading indicator shows during validation

### REQ-FE-EQP-023: Validate Purchase Information

**Requirement:** The system shall validate purchase date and price.

**Acceptance Criteria:**
- [ ] Purchase date is required and cannot be future date
- [ ] Date picker prevents selection of future dates
- [ ] Purchase price is required and must be greater than 0
- [ ] Price input accepts decimal values with 2 decimal places
- [ ] Current value cannot exceed purchase price by more than 20%
- [ ] Validation errors display immediately below fields

### REQ-FE-EQP-024: Inline Specifications Editor

**Requirement:** The system shall provide inline editor for equipment specifications.

**Acceptance Criteria:**
- [ ] Specifications are displayed in editable table format
- [ ] Add button creates new specification row
- [ ] Each row includes key, value, and unit fields
- [ ] Delete button removes specification row
- [ ] Empty specifications are not saved
- [ ] Specifications are saved with equipment form submission
- [ ] Common specifications (Dimensions, Weight, etc.) are suggested in dropdown

### REQ-FE-EQP-025: Photo Upload with Drag and Drop

**Requirement:** The system shall support drag-and-drop photo upload.

**Acceptance Criteria:**
- [ ] Drag-and-drop zone is visually distinct
- [ ] Multiple files can be selected or dragged
- [ ] Accepted formats are JPG, PNG, WEBP
- [ ] Maximum file size is 10MB per file
- [ ] File validation occurs before upload
- [ ] Upload progress bar displays for each file
- [ ] Preview thumbnails appear after upload
- [ ] Primary photo can be selected from thumbnails
- [ ] Delete button removes uploaded photo

---

## 5. Equipment Booking Requirements

### REQ-FE-EQP-030: Create Reservation Form

**Requirement:** The system shall provide a wizard-based reservation creation flow.

**Acceptance Criteria:**
- [ ] Step 1: Select Equipment via search and filter
- [ ] Step 2: Check Availability on calendar
- [ ] Step 3: Set date range with start and end date/time pickers
- [ ] Step 4: Link event via search autocomplete
- [ ] Step 5: Add optional notes and quantity
- [ ] Step 6: Review and confirm reservation
- [ ] Navigation between steps is supported
- [ ] Form data persists when navigating between steps
- [ ] Submit button creates reservation and navigates to confirmation

### REQ-FE-EQP-031: Display Availability Calendar

**Requirement:** The system shall display visual availability calendar during reservation.

**Acceptance Criteria:**
- [ ] Calendar shows existing reservations for selected equipment
- [ ] Available dates are highlighted in green
- [ ] Partially available dates are highlighted in yellow
- [ ] Fully booked dates are highlighted in red
- [ ] Maintenance periods are highlighted in gray
- [ ] Hovering over date shows reservation details in tooltip
- [ ] Clicking date selects it as start or end date
- [ ] Date range selection is visually indicated

### REQ-FE-EQP-032: Handle Booking Conflicts

**Requirement:** The system shall detect and handle booking conflicts gracefully.

**Acceptance Criteria:**
- [ ] Conflict detection occurs when date range is selected
- [ ] Conflict dialog displays if equipment is unavailable
- [ ] Dialog shows details of conflicting reservations
- [ ] Alternative equipment suggestions are displayed if available
- [ ] Alternative equipment can be selected directly from dialog
- [ ] Manager override option is shown only to Managers
- [ ] Override requires justification in text field
- [ ] Reservation is created after successful override

### REQ-FE-EQP-033: Update Reservation

**Requirement:** The system shall allow users to update existing reservations.

**Acceptance Criteria:**
- [ ] Reservation form is pre-populated with existing data
- [ ] Date range, quantity, and notes can be updated
- [ ] Availability is re-checked when dates are changed
- [ ] Conflict handling applies to updates
- [ ] Status can be changed by authorized users
- [ ] Update saves changes and refreshes detail view
- [ ] Validation prevents invalid date ranges

### REQ-FE-EQP-034: Cancel Reservation

**Requirement:** The system shall allow users to cancel reservations with confirmation.

**Acceptance Criteria:**
- [ ] Cancel button is available on reservation detail
- [ ] Confirmation dialog displays before cancellation
- [ ] Late cancellation (< 48 hours) shows warning message
- [ ] Late cancellation requires manager approval
- [ ] Cancellation reason can be provided optionally
- [ ] Cancelled reservations are marked with Cancelled status
- [ ] Success message confirms cancellation

### REQ-FE-EQP-035: Display Alternative Equipment

**Requirement:** The system shall display alternative equipment suggestions when primary choice is unavailable.

**Acceptance Criteria:**
- [ ] Alternatives are displayed in card format
- [ ] Each alternative shows name, category, photo, and availability
- [ ] Similarity score or reason for suggestion is displayed
- [ ] Clicking alternative opens its detail page in new tab
- [ ] Select button replaces primary equipment with alternative
- [ ] No alternatives message displays if none are available

---

## 6. Equipment Logistics Requirements

### REQ-FE-EQP-040: Display Logistics Tracker

**Requirement:** The system shall display logistics tracking for equipment reservations.

**Acceptance Criteria:**
- [ ] Logistics tracker page lists all active logistics
- [ ] Each card shows equipment name, event, and current stage
- [ ] Timeline visualization shows all logistics stages
- [ ] Filters allow filtering by stage, date range, and event
- [ ] In-transit equipment is highlighted
- [ ] Clicking card expands to show full logistics details

### REQ-FE-EQP-041: Logistics Timeline Component

**Requirement:** The system shall display horizontal timeline showing logistics progression.

**Acceptance Criteria:**
- [ ] Timeline shows stages: Packing → Loading → Dispatch → Delivery → Setup → Pickup → Return
- [ ] Each stage shows icon, label, and timestamp
- [ ] Completed stages have checkmark icon and green color
- [ ] Current stage is highlighted with pulsing animation
- [ ] Pending stages are grayed out
- [ ] Clicking stage shows detailed notes in popup
- [ ] User who completed each stage is displayed

### REQ-FE-EQP-042: Update Logistics Status

**Requirement:** The system shall allow authorized users to update logistics status.

**Acceptance Criteria:**
- [ ] Action button appears for next logistics stage
- [ ] Button is enabled only for Staff and Warehouse Managers
- [ ] Clicking button opens confirmation dialog
- [ ] Dialog allows entering notes for the stage
- [ ] Additional fields appear based on stage (e.g., truck ID for loading)
- [ ] Submitting dialog updates logistics and refreshes timeline
- [ ] Success message confirms status update
- [ ] Invalid sequence transitions are prevented with error message

---

## 7. Equipment Maintenance Requirements

### REQ-FE-EQP-050: Display Maintenance Schedule

**Requirement:** The system shall display maintenance schedule in calendar and list views.

**Acceptance Criteria:**
- [ ] Calendar view shows scheduled maintenance by date
- [ ] List view shows maintenance records in table format
- [ ] View toggle switches between calendar and list
- [ ] Overdue maintenance is highlighted with red badge
- [ ] Upcoming maintenance (within 7 days) has yellow badge
- [ ] Completed maintenance is shown in muted color
- [ ] Clicking maintenance opens detail view

### REQ-FE-EQP-051: Schedule Maintenance Form

**Requirement:** The system shall provide form for scheduling equipment maintenance.

**Acceptance Criteria:**
- [ ] Form includes equipment selector (autocomplete)
- [ ] Maintenance type dropdown includes all types
- [ ] Scheduled date picker is required
- [ ] Description field is required with max 2000 characters
- [ ] Technician can be assigned via dropdown
- [ ] Estimated cost field accepts decimal values
- [ ] Notes field is optional
- [ ] Form validates before submission
- [ ] Success message displays after scheduling

### REQ-FE-EQP-052: Start Maintenance

**Requirement:** The system shall allow technicians to start scheduled maintenance.

**Acceptance Criteria:**
- [ ] Start button is visible only to Maintenance Techs and Managers
- [ ] Button is enabled only for Scheduled maintenance
- [ ] Clicking opens confirmation dialog
- [ ] Dialog shows maintenance details for review
- [ ] Confirming sets status to InProgress and records start time
- [ ] Equipment status updates to InMaintenance
- [ ] Success notification displays

### REQ-FE-EQP-053: Complete Maintenance

**Requirement:** The system shall allow technicians to complete maintenance work.

**Acceptance Criteria:**
- [ ] Complete button is visible only for InProgress maintenance
- [ ] Completion dialog requires actual cost input
- [ ] Completion notes field allows detailed description
- [ ] Equipment condition can be updated in dialog
- [ ] Submitting sets status to Completed and records completion time
- [ ] Equipment status returns to Available (if condition allows)
- [ ] Success message displays with cost summary

### REQ-FE-EQP-054: Display Overdue Maintenance Alerts

**Requirement:** The system shall visually indicate overdue maintenance.

**Acceptance Criteria:**
- [ ] Overdue badge appears on equipment cards with overdue maintenance
- [ ] Dashboard widget shows list of overdue maintenance
- [ ] Overdue items are sorted by days overdue (highest first)
- [ ] Clicking overdue item navigates to maintenance detail
- [ ] Count of overdue items displays in navigation badge
- [ ] Email notification trigger is available for managers

---

## 8. Equipment Damage Reporting Requirements

### REQ-FE-EQP-060: Report Damage Dialog

**Requirement:** The system shall provide dialog for reporting equipment damage.

**Acceptance Criteria:**
- [ ] Dialog opens from Report Damage button
- [ ] Equipment ID is pre-filled if opened from equipment detail
- [ ] Severity selector includes Minor, Moderate, Severe, TotalLoss
- [ ] Description field is required with max 2000 characters
- [ ] Event can be linked via autocomplete
- [ ] Repair required checkbox is available
- [ ] Estimated repair cost field appears when repair is required
- [ ] Photo upload supports multiple images
- [ ] Submit creates damage report and closes dialog

### REQ-FE-EQP-061: Display Damage Photo Gallery

**Requirement:** The system shall display damage photos in gallery format.

**Acceptance Criteria:**
- [ ] Photos are displayed as thumbnails in grid
- [ ] Clicking thumbnail opens full-size lightbox
- [ ] Lightbox supports navigation between photos
- [ ] Zoom controls are available in lightbox
- [ ] Download button allows saving photo
- [ ] Caption shows photo metadata (date, uploaded by)

---

## 9. Component Requirements

### REQ-FE-EQP-070: Equipment Card Component

**Requirement:** The system shall provide reusable equipment card component.

**Acceptance Criteria:**
- [ ] Card displays equipment name as header
- [ ] Primary photo is displayed with fallback image
- [ ] Category, status, and condition badges are shown
- [ ] Manufacturer and model are displayed if available
- [ ] Availability indicator is included
- [ ] Card supports click event for navigation
- [ ] Card is responsive and accessible
- [ ] Hover effect provides visual feedback

### REQ-FE-EQP-071: Status Badge Component

**Requirement:** The system shall provide status badge component with color coding.

**Acceptance Criteria:**
- [ ] Badge accepts status as input
- [ ] Available status displays with green background
- [ ] Reserved status displays with blue background
- [ ] InTransit status displays with orange background
- [ ] AtVenue status displays with purple background
- [ ] InMaintenance status displays with yellow background
- [ ] Retired status displays with gray background
- [ ] Badge includes appropriate ARIA label

### REQ-FE-EQP-072: Condition Badge Component

**Requirement:** The system shall provide condition badge component with color coding.

**Acceptance Criteria:**
- [ ] Badge accepts condition as input
- [ ] Excellent condition displays with green background
- [ ] Good condition displays with light green background
- [ ] Fair condition displays with yellow background
- [ ] Poor condition displays with orange background
- [ ] NeedsRepair condition displays with red background
- [ ] OutOfService condition displays with dark red background
- [ ] Badge includes appropriate ARIA label

### REQ-FE-EQP-073: Photo Gallery Component

**Requirement:** The system shall provide photo gallery component with thumbnail navigation.

**Acceptance Criteria:**
- [ ] Primary photo is displayed prominently
- [ ] Thumbnail carousel displays all photos
- [ ] Clicking thumbnail changes primary photo
- [ ] Navigation arrows scroll through thumbnails
- [ ] Photo count indicator is displayed (e.g., "1 of 5")
- [ ] Fullscreen button opens lightbox view
- [ ] Component handles empty state gracefully
- [ ] Images are lazy loaded for performance

### REQ-FE-EQP-074: Availability Checker Component

**Requirement:** The system shall provide availability checker component for date range selection.

**Acceptance Criteria:**
- [ ] Component accepts equipment ID as input
- [ ] Calendar displays with current month
- [ ] Available dates are clickable
- [ ] Unavailable dates are disabled
- [ ] Selected date range is visually highlighted
- [ ] Component emits date range change event
- [ ] Conflict information is displayed if applicable
- [ ] Loading indicator shows while checking availability

### REQ-FE-EQP-075: Reservation Calendar Component

**Requirement:** The system shall provide calendar component for displaying reservations.

**Acceptance Criteria:**
- [ ] Calendar displays month or week view
- [ ] Reservations appear as colored blocks on dates
- [ ] Color coding matches reservation status
- [ ] Clicking reservation shows details in popup
- [ ] Navigation buttons switch months/weeks
- [ ] Today button returns to current date
- [ ] Legend explains color coding
- [ ] Component is responsive on mobile

### REQ-FE-EQP-076: Filter Panel Component

**Requirement:** The system shall provide collapsible filter panel component.

**Acceptance Criteria:**
- [ ] Panel uses mat-expansion-panel
- [ ] Filters are grouped logically (Status, Condition, Category, etc.)
- [ ] Each filter emits change event
- [ ] Active filter count is displayed in panel header
- [ ] Clear all button resets all filters
- [ ] Panel state (expanded/collapsed) persists in session
- [ ] Component is accessible via keyboard

---

## 10. Service Requirements

### REQ-FE-EQP-080: Equipment Service

**Requirement:** The system shall provide Equipment Service for API interactions.

**Acceptance Criteria:**
- [ ] Service provides methods for all equipment CRUD operations
- [ ] Service provides method for activating/deactivating equipment
- [ ] Service provides method for uploading photos
- [ ] Service provides method for deleting photos
- [ ] Service provides method for updating specifications
- [ ] Service provides method for retiring equipment
- [ ] All methods return Observables
- [ ] Error handling is implemented for all methods
- [ ] Service uses HttpClient with proper headers

### REQ-FE-EQP-081: Reservation Service

**Requirement:** The system shall provide Reservation Service for booking operations.

**Acceptance Criteria:**
- [ ] Service provides methods for creating, updating, and canceling reservations
- [ ] Service provides method for checking availability
- [ ] Service provides method for getting alternative suggestions
- [ ] Service provides method for overriding conflicts
- [ ] Service provides method for listing reservations with filters
- [ ] All methods return Observables
- [ ] Service properly handles pagination parameters

### REQ-FE-EQP-082: Logistics Service

**Requirement:** The system shall provide Logistics Service for tracking operations.

**Acceptance Criteria:**
- [ ] Service provides methods for all logistics stage updates
- [ ] Service provides method for getting logistics by reservation ID
- [ ] Service provides method for getting in-transit equipment
- [ ] All methods return Observables
- [ ] Service includes proper error handling

### REQ-FE-EQP-083: Maintenance Service

**Requirement:** The system shall provide Maintenance Service for maintenance operations.

**Acceptance Criteria:**
- [ ] Service provides methods for scheduling, starting, and completing maintenance
- [ ] Service provides method for getting maintenance history
- [ ] Service provides method for getting scheduled maintenance with filters
- [ ] Service provides method for reporting damage
- [ ] All methods return Observables
- [ ] Service supports pagination and filtering

### REQ-FE-EQP-084: Equipment State Service

**Requirement:** The system shall provide state management service using RxJS.

**Acceptance Criteria:**
- [ ] Service maintains equipment list state using BehaviorSubject
- [ ] Service maintains selected equipment state
- [ ] Service maintains loading state
- [ ] Service maintains filter state
- [ ] Service exposes state as Observables
- [ ] Service provides methods for loading and refreshing equipment
- [ ] Service provides method for setting filters
- [ ] State updates trigger component re-renders

---

## 11. Validation Requirements

### REQ-FE-EQP-090: Client-Side Validation

**Requirement:** The system shall perform client-side validation before API calls.

**Acceptance Criteria:**
- [ ] All required fields are validated
- [ ] Field length limits are enforced
- [ ] Date validations prevent future dates where applicable
- [ ] Numeric validations ensure positive values
- [ ] Email format is validated for contact fields
- [ ] Custom validators check business rules (e.g., end date after start date)
- [ ] Validation errors display inline below fields
- [ ] Form submit is disabled until validation passes

### REQ-FE-EQP-091: Real-Time Validation Feedback

**Requirement:** The system shall provide immediate validation feedback.

**Acceptance Criteria:**
- [ ] Validation runs on blur or value change
- [ ] Error messages appear immediately below fields
- [ ] Valid fields show success indicator (checkmark)
- [ ] Invalid fields show error indicator (exclamation)
- [ ] Error messages are clear and actionable
- [ ] Multiple errors per field are supported
- [ ] Validation state is visually distinct (red border for errors)

### REQ-FE-EQP-092: Async Validation

**Requirement:** The system shall support asynchronous validation for uniqueness checks.

**Acceptance Criteria:**
- [ ] Equipment name uniqueness validated against backend
- [ ] Validation is debounced (500ms delay) to reduce API calls
- [ ] Loading indicator shows during async validation
- [ ] Validation result updates field validity state
- [ ] Error message explains uniqueness conflict
- [ ] Validation only runs after field is touched and has value

---

## 12. Authorization Requirements

### REQ-FE-EQP-100: Route Guards

**Requirement:** The system shall implement route guards for authorized access.

**Acceptance Criteria:**
- [ ] authGuard protects all equipment routes
- [ ] roleGuard restricts create/edit routes to Warehouse Managers and Admins
- [ ] roleGuard restricts logistics routes to Staff and higher
- [ ] roleGuard restricts maintenance routes to Maintenance Techs and higher
- [ ] Unauthorized users are redirected to access denied page
- [ ] Route guards check user roles from authentication service

### REQ-FE-EQP-101: Conditional UI Elements

**Requirement:** The system shall conditionally display UI elements based on user role.

**Acceptance Criteria:**
- [ ] Create Equipment button visible only to Warehouse Managers and Admins
- [ ] Edit button visible only to Warehouse Managers and Admins
- [ ] Delete button visible only to Admins
- [ ] Override Conflict button visible only to Managers
- [ ] Retire Equipment button visible only to Managers
- [ ] Action buttons include role check via structural directive
- [ ] Tooltips explain why actions are disabled

### REQ-FE-EQP-102: Unsaved Changes Guard

**Requirement:** The system shall prevent navigation with unsaved changes.

**Acceptance Criteria:**
- [ ] canDeactivate guard is applied to create/edit routes
- [ ] Guard checks form dirty state before allowing navigation
- [ ] Confirmation dialog displays if form has unsaved changes
- [ ] User can choose to discard changes or stay on page
- [ ] Guard works with browser back button
- [ ] Guard works with programmatic navigation

---

## 13. Accessibility Requirements

### REQ-FE-EQP-110: Keyboard Navigation

**Requirement:** The system shall support complete keyboard navigation.

**Acceptance Criteria:**
- [ ] All interactive elements are reachable via Tab key
- [ ] Tab order follows logical visual flow
- [ ] Focus indicators are clearly visible
- [ ] Enter key activates buttons and links
- [ ] Escape key closes dialogs and dropdowns
- [ ] Arrow keys navigate within menus and lists
- [ ] Skip to content link is available

### REQ-FE-EQP-111: Screen Reader Support

**Requirement:** The system shall provide comprehensive screen reader support.

**Acceptance Criteria:**
- [ ] All images have alt text
- [ ] All form inputs have associated labels
- [ ] Status badges have aria-label describing status
- [ ] Interactive elements have descriptive aria-labels
- [ ] Dynamic content changes announced via aria-live regions
- [ ] Error messages associated with fields via aria-describedby
- [ ] Modal dialogs have aria-modal and proper focus management

### REQ-FE-EQP-112: Color Contrast

**Requirement:** The system shall meet WCAG 2.1 AA color contrast requirements.

**Acceptance Criteria:**
- [ ] Text has minimum 4.5:1 contrast ratio
- [ ] Large text (18pt+) has minimum 3:1 contrast ratio
- [ ] Interactive elements have 3:1 contrast with background
- [ ] Status colors meet contrast requirements
- [ ] Color is not sole indicator of state (icons/text also used)
- [ ] Angular Material theme provides compliant colors by default

### REQ-FE-EQP-113: Form Accessibility

**Requirement:** The system shall make forms fully accessible.

**Acceptance Criteria:**
- [ ] All inputs have associated labels
- [ ] Required fields indicated with aria-required
- [ ] Error messages have role="alert" for immediate announcement
- [ ] Field groups use fieldset and legend
- [ ] Help text associated via aria-describedby
- [ ] Validation errors linked to fields
- [ ] Form can be completed using keyboard only

---

## 14. Performance Requirements

### REQ-FE-EQP-120: Initial Load Performance

**Requirement:** The system shall meet initial page load performance targets.

**Acceptance Criteria:**
- [ ] First Contentful Paint occurs in under 1.5 seconds
- [ ] Time to Interactive is under 3 seconds
- [ ] Initial bundle size is under 250KB (gzipped)
- [ ] Critical CSS is inlined
- [ ] Performance is measured via Lighthouse
- [ ] Metrics are tracked in analytics

### REQ-FE-EQP-121: List Rendering Performance

**Requirement:** The system shall efficiently render large equipment lists.

**Acceptance Criteria:**
- [ ] Equipment list renders 100 items in under 100ms
- [ ] Virtual scrolling is used for lists over 100 items
- [ ] Images are lazy loaded as they enter viewport
- [ ] Pagination limits items to manageable size (25-50 per page)
- [ ] Change detection uses OnPush strategy
- [ ] Rendering performance measured and logged

### REQ-FE-EQP-122: API Call Optimization

**Requirement:** The system shall optimize API calls to reduce network overhead.

**Acceptance Criteria:**
- [ ] Search input is debounced (300ms)
- [ ] Availability checks are debounced (500ms)
- [ ] Duplicate concurrent requests are prevented
- [ ] Responses are cached when appropriate
- [ ] Pagination reduces data transfer
- [ ] Loading states prevent duplicate submissions

### REQ-FE-EQP-123: Photo Upload Performance

**Requirement:** The system shall handle photo uploads efficiently.

**Acceptance Criteria:**
- [ ] File size validation prevents uploads over 10MB
- [ ] Image preview is generated client-side
- [ ] Upload progress is displayed
- [ ] Multiple uploads processed in parallel
- [ ] Failed uploads can be retried individually
- [ ] Upload completes in under 5 seconds for 10MB file

### REQ-FE-EQP-124: Calendar Rendering Performance

**Requirement:** The system shall render reservation calendar efficiently.

**Acceptance Criteria:**
- [ ] Calendar renders 100 reservations in under 150ms
- [ ] Month view loads in under 200ms
- [ ] Navigation between months is smooth (no lag)
- [ ] Reservation tooltips appear within 100ms
- [ ] Performance is consistent on mobile devices

---

## 15. Testing Requirements

### REQ-FE-EQP-130: Unit Test Coverage

**Requirement:** The system shall have comprehensive unit test coverage.

**Acceptance Criteria:**
- [ ] All services have 100% test coverage
- [ ] All components have minimum 80% test coverage
- [ ] All validators have 100% test coverage
- [ ] All pipes have 100% test coverage
- [ ] State management logic has 100% coverage
- [ ] Tests use Jest framework
- [ ] Tests run in under 30 seconds

### REQ-FE-EQP-131: Component Testing

**Requirement:** The system shall test component behavior and rendering.

**Acceptance Criteria:**
- [ ] Component inputs and outputs are tested
- [ ] User interactions are simulated and tested
- [ ] Conditional rendering based on state is tested
- [ ] Form validation logic is tested
- [ ] Error handling is tested
- [ ] Component lifecycle hooks are tested
- [ ] Tests use Angular testing utilities

### REQ-FE-EQP-132: Service Testing

**Requirement:** The system shall test service methods and HTTP interactions.

**Acceptance Criteria:**
- [ ] All service methods are unit tested
- [ ] HTTP calls are mocked using HttpClientTestingModule
- [ ] Success and error scenarios are tested
- [ ] Observable streams are tested
- [ ] Error handling and retries are tested
- [ ] State service updates are tested

### REQ-FE-EQP-133: E2E Test Coverage

**Requirement:** The system shall have end-to-end tests for critical user flows.

**Acceptance Criteria:**
- [ ] Create equipment flow is tested end-to-end
- [ ] Upload equipment photo flow is tested
- [ ] Create reservation flow is tested
- [ ] Check availability flow is tested
- [ ] Handle double booking conflict is tested
- [ ] Update logistics status flow is tested
- [ ] Schedule maintenance flow is tested
- [ ] Report damage flow is tested
- [ ] Filter and search equipment is tested
- [ ] Tests use Playwright framework

### REQ-FE-EQP-134: Test Accessibility

**Requirement:** The system shall include automated accessibility tests.

**Acceptance Criteria:**
- [ ] Accessibility tests run on all pages
- [ ] Tests check for WCAG 2.1 AA compliance
- [ ] Keyboard navigation is tested
- [ ] ARIA attributes are validated
- [ ] Color contrast is verified
- [ ] Tests fail build if violations found

---

## 16. Error Handling Requirements

### REQ-FE-EQP-140: Display Validation Errors

**Requirement:** The system shall display validation errors inline with form fields.

**Acceptance Criteria:**
- [ ] Error messages appear below invalid fields
- [ ] Errors are displayed in red color
- [ ] Error icon appears next to field
- [ ] Multiple errors are shown if applicable
- [ ] Errors clear when field becomes valid
- [ ] Required field errors show on submit attempt

### REQ-FE-EQP-141: Display API Errors

**Requirement:** The system shall display API errors using snackbar notifications.

**Acceptance Criteria:**
- [ ] 400 Bad Request errors show validation details
- [ ] 404 Not Found errors show entity not found message
- [ ] 409 Conflict errors show conflict details
- [ ] 401 Unauthorized redirects to login
- [ ] 403 Forbidden shows permission denied message
- [ ] 500 Server errors show generic error with retry option
- [ ] Network errors show connectivity issue message

### REQ-FE-EQP-142: Handle Conflict Errors

**Requirement:** The system shall handle booking conflicts with detailed dialog.

**Acceptance Criteria:**
- [ ] Conflict dialog displays when 409 response received
- [ ] Dialog shows conflicting reservation details
- [ ] Dialog lists alternative equipment suggestions
- [ ] User can select alternative equipment
- [ ] Manager can override with justification
- [ ] Dialog provides clear action buttons (Cancel, Select Alternative, Override)

### REQ-FE-EQP-143: Display Loading States

**Requirement:** The system shall display loading indicators during async operations.

**Acceptance Criteria:**
- [ ] Spinner displays while loading equipment list
- [ ] Progress bar shows during photo upload
- [ ] Button shows loading state during form submission
- [ ] Skeleton screens display while loading detail page
- [ ] Loading state prevents duplicate submissions
- [ ] Timeout message displays if request takes over 30 seconds

### REQ-FE-EQP-144: Retry Failed Operations

**Requirement:** The system shall allow users to retry failed operations.

**Acceptance Criteria:**
- [ ] Error snackbar includes Retry action for 500 errors
- [ ] Failed photo uploads can be retried individually
- [ ] Network errors automatically retry up to 3 times
- [ ] Exponential backoff applied to retries
- [ ] User can manually trigger retry after auto-retries exhausted

---

## 17. Routing Configuration

### 17.1 Route Structure
```typescript
export const equipmentRoutes: Routes = [
    {
        path: 'equipment',
        children: [
            {
                path: '',
                loadComponent: () => import('./pages/equipment/equipment-list'),
                title: 'Equipment'
            },
            {
                path: 'create',
                loadComponent: () => import('./pages/equipment/equipment-create'),
                title: 'Add Equipment',
                canActivate: [authGuard, roleGuard(['WarehouseManager', 'Admin'])]
            },
            {
                path: ':equipmentId',
                loadComponent: () => import('./pages/equipment/equipment-detail'),
                title: 'Equipment Details'
            },
            {
                path: ':equipmentId/edit',
                loadComponent: () => import('./pages/equipment/equipment-edit'),
                title: 'Edit Equipment',
                canActivate: [authGuard, roleGuard(['WarehouseManager', 'Admin'])],
                canDeactivate: [unsavedChangesGuard]
            },
            {
                path: 'reservations',
                loadComponent: () => import('./pages/equipment/reservations-list'),
                title: 'Equipment Reservations'
            },
            {
                path: 'logistics',
                loadComponent: () => import('./pages/equipment/logistics-tracker'),
                title: 'Logistics Tracker',
                canActivate: [authGuard, roleGuard(['Staff', 'WarehouseManager', 'Manager', 'Admin'])]
            },
            {
                path: 'maintenance',
                loadComponent: () => import('./pages/equipment/maintenance-schedule'),
                title: 'Maintenance Schedule',
                canActivate: [authGuard, roleGuard(['MaintenanceTech', 'Manager', 'Admin'])]
            }
        ]
    }
];
```

---

## 18. Component Structure

### 18.1 Pages
```
src/app/pages/equipment/
├── equipment-list/       # Equipment list page
├── equipment-detail/     # Equipment detail page
├── equipment-create/     # Create equipment page
├── equipment-edit/       # Edit equipment page
├── reservations-list/    # Reservations list page
├── maintenance-schedule/ # Maintenance schedule page
└── logistics-tracker/    # Logistics tracker page
```

### 18.2 Components
```
src/app/components/equipment/
├── equipment-card/             # Reusable equipment card
├── equipment-status-badge/     # Status badge
├── equipment-condition-badge/  # Condition badge
├── equipment-form/             # Equipment form
├── equipment-photo-gallery/    # Photo gallery
├── photo-upload-dialog/        # Photo upload dialog
├── specifications-editor/      # Specifications editor
├── reservation-form/           # Reservation form
├── availability-checker/       # Availability checker
├── reservation-calendar/       # Reservation calendar
├── logistics-timeline/         # Logistics timeline
├── maintenance-form/           # Maintenance form
├── damage-report-dialog/       # Damage report dialog
└── equipment-filter-panel/     # Filter panel
```

---

## 19. Material Components Usage

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

## 20. Styling Guidelines

### 20.1 BEM Naming Convention
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

### 20.2 Responsive Breakpoints
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

## 21. Data Models

### 21.1 DTOs
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
```

### 21.2 Enums
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
```

---

## 22. Appendices

### 22.1 Related Documents
- [Backend Specification](./backend-spec.md)
- [Use Case Diagram](./diagrams/use-case.drawio)
- [C4 Diagrams](./diagrams/c4-context.drawio)
- [Class Diagram](./plantuml/class-diagram.plantuml)
- [Sequence Diagrams](./plantuml/sequence-diagram.plantuml)

### 22.2 Revision History
| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-22 | System | Initial specification |
