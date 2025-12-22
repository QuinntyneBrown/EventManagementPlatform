# Staff Management - Frontend Specification

## Document Information
| Field | Value |
|-------|-------|
| Feature | Staff Management |
| Version | 1.0 |
| Date | 2025-12-22 |
| Status | Final |

---

## 1. Overview

### 1.1 Purpose
This specification defines the frontend requirements for the Staff Management feature, providing users with a comprehensive interface for managing staff members, their availability, assignments, and performance.

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

## 2. Requirements

### 2.1 Staff CRUD Requirements

### REQ-FE-STF-001: Display Staff List with Search and Filters

**Requirement:** The UI shall display a paginated list of staff members with search and filtering capabilities.

**Acceptance Criteria:**
- [ ] Page displays staff in responsive grid or list view
- [ ] Real-time search by name and email implemented
- [ ] Filter panel supports status, role, skills, and minimum rating filters
- [ ] Sort controls for name, rating, and hire date
- [ ] View toggle between grid and list layouts
- [ ] Pagination controls with page size selector (10, 25, 50, 100)
- [ ] Loading state shown during data fetch
- [ ] Empty state displayed when no staff found

### REQ-FE-STF-002: Implement Staff Registration Form

**Requirement:** The UI shall provide a form to register new staff members.

**Acceptance Criteria:**
- [ ] Form includes fields for first name, last name, email, phone, role, hire date, hourly rate, skills
- [ ] Real-time validation feedback on all fields
- [ ] Email uniqueness check on blur event
- [ ] Phone number formatting applied automatically
- [ ] Skills field with autocomplete from predefined list
- [ ] Date picker restricts hire date to past/today only
- [ ] Photo upload with preview before submission
- [ ] Submit button disabled until form valid
- [ ] Success message and navigation to staff detail after registration
- [ ] Error messages displayed for failed submissions

### REQ-FE-STF-003: Display Staff Detail Page

**Requirement:** The UI shall show comprehensive staff member details in an organized layout.

**Acceptance Criteria:**
- [ ] Header section displays photo, name, role, status badge, and action buttons
- [ ] Profile card shows contact info, hire date, hourly rate, and skills
- [ ] Tabbed interface for Availability, Assignments, Performance, Timesheet, and Activity sections
- [ ] Action buttons visible based on user permissions
- [ ] Loading skeleton shown while data loads
- [ ] 404 page displayed if staff not found
- [ ] Breadcrumb navigation included

### REQ-FE-STF-004: Implement Staff Profile Edit Form

**Requirement:** The UI shall allow editing of staff profile information.

**Acceptance Criteria:**
- [ ] Form pre-populated with current staff data
- [ ] Email field disabled (not editable)
- [ ] All validation rules applied as in registration
- [ ] Unsaved changes guard warns before navigation
- [ ] Cancel button discards changes with confirmation
- [ ] Save button shows loading state during submission
- [ ] Success notification on successful update
- [ ] Form returns to view mode after save

### REQ-FE-STF-005: Implement Staff Photo Upload

**Requirement:** The UI shall support uploading and managing staff profile photos.

**Acceptance Criteria:**
- [ ] Drag and drop area for photo upload
- [ ] File selector button as alternative to drag/drop
- [ ] Image preview shown before upload
- [ ] File type validation (JPG, PNG only)
- [ ] File size validation (max 5MB)
- [ ] Automatic image resizing to 500x500px
- [ ] Upload progress indicator
- [ ] Remove photo button with confirmation dialog
- [ ] Error handling for upload failures
- [ ] Success notification on upload completion

### REQ-FE-STF-006: Implement Staff Activation/Deactivation

**Requirement:** The UI shall allow managers to activate or deactivate staff profiles.

**Acceptance Criteria:**
- [ ] Activate button shown when status is Inactive
- [ ] Deactivate button shown when status is Active
- [ ] Deactivation dialog prompts for reason
- [ ] Confirmation dialog before activation
- [ ] Action buttons disabled during operation
- [ ] Status badge updates immediately after successful operation
- [ ] Success notification displayed
- [ ] Error handling for failed operations

### REQ-FE-STF-007: Display Staff Cards in List View

**Requirement:** The UI shall render individual staff information as cards in list/grid views.

**Acceptance Criteria:**
- [ ] Card displays photo, name, role, status badge, and rating
- [ ] Quick action buttons for common operations
- [ ] Click on card navigates to detail page
- [ ] Status badge color-coded by status type
- [ ] Star rating display for average rating
- [ ] Responsive card sizing for different breakpoints
- [ ] Hover effects for interactivity
- [ ] Loading skeleton for cards being fetched

### 2.2 Staff Availability Requirements

### REQ-FE-STF-008: Display Staff Availability Calendar

**Requirement:** The UI shall show staff availability in an interactive calendar view.

**Acceptance Criteria:**
- [ ] Weekly calendar view displays availability blocks
- [ ] Each day shows multiple time slots
- [ ] Color-coded availability types
- [ ] Unavailable dates marked on calendar
- [ ] Toggle between week view and list view
- [ ] Navigate between weeks
- [ ] Current week highlighted
- [ ] Loading state during data fetch

### REQ-FE-STF-009: Implement Add Availability Dialog

**Requirement:** The UI shall provide a dialog to declare new availability.

**Acceptance Criteria:**
- [ ] Dialog triggered by "Add Availability" button
- [ ] Day of week selector (dropdown or buttons)
- [ ] Start time and end time pickers
- [ ] Recurring checkbox option
- [ ] Effective from date picker (required)
- [ ] Effective to date picker (optional)
- [ ] Validation that end time is after start time
- [ ] Save button disabled until valid
- [ ] Cancel button closes dialog
- [ ] Success notification after save

### REQ-FE-STF-010: Implement Edit Availability Inline

**Requirement:** The UI shall allow inline editing of existing availability entries.

**Acceptance Criteria:**
- [ ] Click on availability block enables edit mode
- [ ] Time fields become editable
- [ ] Save and cancel icons appear
- [ ] Validation applied on save
- [ ] Conflicts highlighted before save
- [ ] Confirmation for changes affecting assignments
- [ ] Update reflected immediately in calendar
- [ ] Error handling for failed updates

### REQ-FE-STF-011: Implement Recurring Availability Setup

**Requirement:** The UI shall provide interface to set recurring weekly availability patterns.

**Acceptance Criteria:**
- [ ] Dialog shows all 7 days of week
- [ ] Each day allows multiple time slot entries
- [ ] Add/remove time slot buttons for each day
- [ ] Copy day schedule to other days functionality
- [ ] Effective date range selector
- [ ] Preview of generated schedule
- [ ] Validation for all time slots
- [ ] Bulk save with loading indicator
- [ ] Success message with count of slots created

### REQ-FE-STF-012: Implement Mark Unavailable Date

**Requirement:** The UI shall allow marking specific dates as unavailable.

**Acceptance Criteria:**
- [ ] "Mark Unavailable" button opens dialog
- [ ] Date picker for selecting date(s)
- [ ] All-day checkbox
- [ ] Time range pickers (disabled if all-day checked)
- [ ] Reason text field (optional)
- [ ] Date cannot be in the past
- [ ] Warning if conflicts with assignments
- [ ] Save and cancel buttons
- [ ] Date visually marked in calendar after save

### REQ-FE-STF-013: Display Unavailable Dates List

**Requirement:** The UI shall show a list of all unavailable dates for a staff member.

**Acceptance Criteria:**
- [ ] List displayed in chronological order
- [ ] Each entry shows date, time (if not all-day), and reason
- [ ] Delete button for each entry
- [ ] Confirmation dialog before deletion
- [ ] Past dates shown but not editable
- [ ] Empty state when no unavailable dates
- [ ] Pagination if many entries

### 2.3 Staff Assignment Requirements

### REQ-FE-STF-014: Display Staff Assignments List

**Requirement:** The UI shall show all assignments for a staff member.

**Acceptance Criteria:**
- [ ] Assignments displayed in chronological order
- [ ] Each assignment shows event name, date, role, and status
- [ ] Status badge color-coded
- [ ] Filter by assignment status
- [ ] Separate sections for upcoming and past assignments
- [ ] Click on assignment shows details
- [ ] Action buttons based on status (confirm, decline)
- [ ] Empty state for no assignments

### REQ-FE-STF-015: Implement Staff Assignment Dialog

**Requirement:** The UI shall provide a dialog to assign staff to events.

**Acceptance Criteria:**
- [ ] Event selector with search/autocomplete
- [ ] Staff selector with available staff filtered by event time
- [ ] Available staff ranked by rating and skills
- [ ] Assigned role dropdown
- [ ] Optional notes text area
- [ ] Conflict warning if double booking detected
- [ ] Availability indicator for each staff option
- [ ] Save and cancel buttons
- [ ] Loading state during save
- [ ] Success notification with assignment details

### REQ-FE-STF-016: Implement Assignment Confirmation

**Requirement:** The UI shall allow staff to confirm requested assignments.

**Acceptance Criteria:**
- [ ] Confirm button visible for Requested status assignments
- [ ] Confirmation dialog with assignment details
- [ ] Event information displayed for review
- [ ] Confirm and cancel buttons
- [ ] Button disabled during operation
- [ ] Status updates to Confirmed immediately
- [ ] Success notification
- [ ] Calendar updated to reflect confirmed assignment

### REQ-FE-STF-017: Implement Assignment Decline

**Requirement:** The UI shall allow staff to decline requested assignments.

**Acceptance Criteria:**
- [ ] Decline button visible for Requested status assignments
- [ ] Dialog prompts for decline reason
- [ ] Reason text area (required)
- [ ] Decline and cancel buttons
- [ ] Confirmation before submitting
- [ ] Status updates to Declined
- [ ] Notification sent to assignment creator
- [ ] Success message displayed

### REQ-FE-STF-018: Display Available Staff Finder

**Requirement:** The UI shall provide interface to find available staff for events.

**Acceptance Criteria:**
- [ ] Event date/time selector
- [ ] Role filter (optional)
- [ ] Search initiates staff availability check
- [ ] Results show staff with availability indicators
- [ ] Staff ranked by rating and relevant skills
- [ ] Quick assign button for each staff member
- [ ] Conflict warnings displayed
- [ ] Loading state during search
- [ ] Empty state if no available staff

### REQ-FE-STF-019: Implement Staff Schedule View

**Requirement:** The UI shall display staff member's schedule across events.

**Acceptance Criteria:**
- [ ] Calendar view showing all assignments
- [ ] Timeline view option
- [ ] List view option
- [ ] Toggle between view types
- [ ] Color-coded by assignment status
- [ ] Conflict highlighting
- [ ] Click on assignment for details
- [ ] Navigate between months
- [ ] Export schedule functionality

### 2.4 Staff Check-in/Check-out Requirements

### REQ-FE-STF-020: Implement Staff Check-In Interface

**Requirement:** The UI shall provide functionality to check in staff at events.

**Acceptance Criteria:**
- [ ] Check-in button visible for confirmed assignments
- [ ] Button only enabled within allowed time window (1 hour before event)
- [ ] Check-in dialog shows staff and event details
- [ ] Timestamp display of check-in time
- [ ] Confirm and cancel buttons
- [ ] Loading state during check-in
- [ ] Success notification
- [ ] Status updates to show checked-in state
- [ ] Error message if check-in window not valid

### REQ-FE-STF-021: Implement Staff Check-Out Interface

**Requirement:** The UI shall provide functionality to check out staff after events.

**Acceptance Criteria:**
- [ ] Check-out button visible only if checked in
- [ ] Check-out dialog shows check-in time and calculates duration
- [ ] Current time shown as check-out time
- [ ] Total hours worked calculated and displayed
- [ ] Confirm and cancel buttons
- [ ] Loading state during check-out
- [ ] Success notification with hours worked
- [ ] Status updates to show checked-out state

### REQ-FE-STF-022: Implement No-Show Marking

**Requirement:** The UI shall allow managers to mark staff as no-show.

**Acceptance Criteria:**
- [ ] Mark No-Show button visible to managers only
- [ ] Button enabled only after event start time + 30 minutes
- [ ] Confirmation dialog with warning message
- [ ] Reason text field (optional)
- [ ] Confirm and cancel buttons
- [ ] Status updates to NoShow
- [ ] Notification sent to staff
- [ ] Reliability metrics updated

### REQ-FE-STF-023: Display Staff Timesheet

**Requirement:** The UI shall show staff work hours in a timesheet format.

**Acceptance Criteria:**
- [ ] Date range selector for timesheet period
- [ ] Table showing check-in/out times per assignment
- [ ] Event details for each entry
- [ ] Hours worked calculated for each entry
- [ ] Total hours summary at bottom
- [ ] Export to CSV/PDF functionality
- [ ] Pagination for large timesheets
- [ ] Empty state for no time entries

### 2.5 Staff Performance Review Requirements

### REQ-FE-STF-024: Display Staff Ratings Summary

**Requirement:** The UI shall show aggregate performance metrics for staff.

**Acceptance Criteria:**
- [ ] Average rating prominently displayed with star visualization
- [ ] Total number of ratings shown
- [ ] Rating distribution chart (1-5 star breakdown)
- [ ] Recent feedback list
- [ ] Compliments and complaints counts
- [ ] Performance trend graph over time
- [ ] Filter by date range
- [ ] Refresh button to reload metrics

### REQ-FE-STF-025: Implement Add Performance Review Dialog

**Requirement:** The UI shall provide dialog to submit performance reviews.

**Acceptance Criteria:**
- [ ] Dialog opened from "Add Review" button
- [ ] Event selector if review is event-based (optional)
- [ ] Star rating selector (1-5)
- [ ] Review type selector (Feedback, Complaint, Compliment, Performance Review)
- [ ] Feedback text area
- [ ] Rating required, feedback optional
- [ ] Submit and cancel buttons
- [ ] Loading state during submission
- [ ] Success notification
- [ ] Review appears in list immediately

### REQ-FE-STF-026: Display Performance Reviews List

**Requirement:** The UI shall show all performance reviews for a staff member.

**Acceptance Criteria:**
- [ ] Reviews displayed in reverse chronological order
- [ ] Each review shows rating, type, feedback, reviewer, and date
- [ ] Filter by review type
- [ ] Filter by date range
- [ ] Pagination for many reviews
- [ ] Visual distinction for different review types
- [ ] Expandable review cards for long feedback
- [ ] Empty state when no reviews

### REQ-FE-STF-027: Implement Feedback Submission

**Requirement:** The UI shall allow users to submit general feedback for staff.

**Acceptance Criteria:**
- [ ] Quick feedback button in staff detail header
- [ ] Dialog with feedback text area
- [ ] Optional rating field
- [ ] Submit and cancel buttons
- [ ] Character limit indicator
- [ ] Anonymous feedback option (if allowed)
- [ ] Success notification
- [ ] Feedback appears in review list

### REQ-FE-STF-028: Implement Complaint Filing

**Requirement:** The UI shall provide interface to file complaints against staff.

**Acceptance Criteria:**
- [ ] "File Complaint" button (visible to all authenticated users)
- [ ] Dialog with complaint details text area (required)
- [ ] Incident date selector
- [ ] Related event selector (optional)
- [ ] Severity indicator
- [ ] Warning about complaint consequences
- [ ] Submit and cancel buttons
- [ ] Confirmation dialog before submission
- [ ] Success notification
- [ ] Complaint ID provided for tracking

### REQ-FE-STF-029: Display Rating Visualization

**Requirement:** The UI shall provide visual representation of staff ratings.

**Acceptance Criteria:**
- [ ] Star rating component for average rating
- [ ] Half-star support for decimal ratings
- [ ] Rating distribution bar chart
- [ ] Color coding (green for high, yellow for medium, red for low)
- [ ] Tooltip showing exact rating value
- [ ] Click on rating to see detailed breakdown
- [ ] Responsive sizing

### 2.6 Double Booking Prevention Requirements

### REQ-FE-STF-030: Display Assignment Conflict Warnings

**Requirement:** The UI shall show warnings when assignment conflicts are detected.

**Acceptance Criteria:**
- [ ] Warning icon/badge on conflicting time slots in calendar
- [ ] Tooltip shows conflict details on hover
- [ ] Conflict dialog when attempting conflicting assignment
- [ ] List of conflicting events displayed
- [ ] Option to override with manager permission
- [ ] Visual highlighting of conflict period
- [ ] Disable assignment button if conflict detected
- [ ] Clear error message explaining conflict

### REQ-FE-STF-031: Implement Availability Conflict Detection UI

**Requirement:** The UI shall detect and display availability conflicts with assignments.

**Acceptance Criteria:**
- [ ] Warning shown when removing availability affects assignments
- [ ] List of affected assignments displayed
- [ ] Require manager confirmation to proceed
- [ ] Conflict highlighted in calendar view
- [ ] Validation prevents save without resolution
- [ ] Alternative time suggestions if available
- [ ] Cancel option preserves current state

### REQ-FE-STF-032: Show Real-Time Conflict Check

**Requirement:** The UI shall perform real-time validation for assignment conflicts.

**Acceptance Criteria:**
- [ ] Conflict check triggered on staff/event selection
- [ ] Loading indicator during check
- [ ] Instant feedback on conflict detection
- [ ] Details of conflict shown inline
- [ ] Assignment button disabled if conflict exists
- [ ] Conflict resolves automatically when selection changes
- [ ] Error message auto-dismisses when resolved

### 2.7 Validation Requirements

### REQ-FE-STF-033: Implement Form Validation

**Requirement:** The UI shall validate all form inputs according to business rules.

**Acceptance Criteria:**
- [ ] Required field validation with visual indicators
- [ ] Email format validation
- [ ] Phone number format validation
- [ ] Date range validations
- [ ] Field length validations (max characters)
- [ ] Custom validation messages for each rule
- [ ] Validation triggered on blur and change events
- [ ] Form submit button disabled when invalid
- [ ] Error messages displayed below fields
- [ ] Error summary at top of form

### REQ-FE-STF-034: Implement Client-Side Business Rule Validation

**Requirement:** The UI shall enforce business rules before submission.

**Acceptance Criteria:**
- [ ] Hire date cannot be future date validation
- [ ] Availability end time after start time validation
- [ ] Rating must be 1-5 validation
- [ ] Hourly rate must be non-negative validation
- [ ] Email uniqueness check via API
- [ ] Validation feedback shown in real-time
- [ ] Prevent submission with client-side errors

### REQ-FE-STF-035: Display Server-Side Validation Errors

**Requirement:** The UI shall display validation errors returned from the API.

**Acceptance Criteria:**
- [ ] Parse 400 Bad Request responses for validation errors
- [ ] Map errors to corresponding form fields
- [ ] Display field-level error messages
- [ ] Show general errors in notification/alert
- [ ] Keep form in error state until corrected
- [ ] Allow user to correct and resubmit
- [ ] Highlight fields with server-side errors

### 2.8 Authorization Requirements

### REQ-FE-STF-036: Implement Role-Based UI Controls

**Requirement:** The UI shall show/hide controls based on user roles.

**Acceptance Criteria:**
- [ ] Register staff button visible to Manager/Admin only
- [ ] Edit profile button visible to Manager/Admin or self
- [ ] Activate/Deactivate buttons visible to Manager/Admin only
- [ ] Assign to event button visible to Manager only
- [ ] Check-in/out buttons visible based on permissions
- [ ] Mark no-show visible to Manager only
- [ ] File complaint visible to all authenticated users
- [ ] Resolve complaint visible to Manager only

### REQ-FE-STF-037: Implement Resource-Based Access Control

**Requirement:** The UI shall enforce ownership-based access restrictions.

**Acceptance Criteria:**
- [ ] Staff can view/edit own profile only
- [ ] Staff can manage own availability only
- [ ] Staff can confirm/decline own assignments only
- [ ] Managers can access all staff resources
- [ ] Unauthorized actions hidden in UI
- [ ] 403 errors handled with appropriate message
- [ ] Redirect to appropriate page on authorization failure

### REQ-FE-STF-038: Handle Authentication State

**Requirement:** The UI shall handle authentication requirements for all staff operations.

**Acceptance Criteria:**
- [ ] Redirect to login if not authenticated
- [ ] Store return URL for post-login redirect
- [ ] Show loading state during auth check
- [ ] Display authentication error messages
- [ ] Handle token expiration gracefully
- [ ] Refresh token automatically when possible
- [ ] Logout functionality available
- [ ] Protected routes require authentication

### 2.9 Azure Integration Requirements

### REQ-FE-STF-039: Implement Photo Upload to Azure Blob Storage

**Requirement:** The UI shall upload photos directly to Azure Blob Storage via API.

**Acceptance Criteria:**
- [ ] Use multipart/form-data for upload
- [ ] Display upload progress percentage
- [ ] Handle Azure Blob Storage URLs for display
- [ ] Implement retry logic for failed uploads
- [ ] Show error messages for upload failures
- [ ] Support cancellation of ongoing upload
- [ ] Validate file before upload

### REQ-FE-STF-040: Display AI-Powered Staff Recommendations

**Requirement:** The UI shall show intelligent staff recommendations from Azure OpenAI.

**Acceptance Criteria:**
- [ ] Recommendations displayed in assignment dialog
- [ ] Confidence score shown for each recommendation
- [ ] Explanation of why staff is recommended
- [ ] Fallback to manual selection if AI unavailable
- [ ] Loading state during recommendation fetch
- [ ] Recommendations ranked by relevance
- [ ] Quick assign from recommendations

### REQ-FE-STF-041: Show Feedback Sentiment Analysis

**Requirement:** The UI shall display sentiment analysis results for performance feedback.

**Acceptance Criteria:**
- [ ] Sentiment indicator (positive/neutral/negative) on reviews
- [ ] Color-coded sentiment badges
- [ ] Sentiment trend over time visualization
- [ ] Filter reviews by sentiment
- [ ] Sentiment score tooltip with details
- [ ] Graceful handling if sentiment not available

### 2.10 Performance Requirements

### REQ-FE-STF-042: Optimize Initial Page Load

**Requirement:** The UI shall meet performance targets for initial page rendering.

**Acceptance Criteria:**
- [ ] First Contentful Paint < 1.5s
- [ ] Time to Interactive < 3s
- [ ] Initial bundle size < 250KB
- [ ] Lazy loading of route modules
- [ ] Code splitting implemented
- [ ] Performance monitoring integrated
- [ ] Lighthouse score > 90

### REQ-FE-STF-043: Implement Efficient List Rendering

**Requirement:** The UI shall render large staff lists efficiently.

**Acceptance Criteria:**
- [ ] Virtual scrolling for lists > 50 items
- [ ] Pagination instead of infinite scroll
- [ ] OnPush change detection strategy
- [ ] Render time < 100ms for 50 items
- [ ] Smooth scrolling performance
- [ ] Debounced search input
- [ ] Optimized filtering and sorting

### REQ-FE-STF-044: Optimize Image Loading

**Requirement:** The UI shall handle staff photos efficiently.

**Acceptance Criteria:**
- [ ] Lazy loading of images
- [ ] Placeholder images while loading
- [ ] Image compression applied
- [ ] Responsive image sizing
- [ ] Cached images reused
- [ ] Error fallback images
- [ ] Loading state for slow connections

### REQ-FE-STF-045: Implement Response Caching

**Requirement:** The UI shall cache API responses to reduce network requests.

**Acceptance Criteria:**
- [ ] HTTP interceptor implements caching
- [ ] Cache duration appropriate per endpoint
- [ ] Cache invalidation on mutations
- [ ] Manual refresh option available
- [ ] Cache stored in memory, not localStorage
- [ ] Cache size limits enforced

### 2.11 Testing Requirements

### REQ-FE-STF-046: Implement Component Unit Tests

**Requirement:** The UI shall have comprehensive unit tests for all components.

**Acceptance Criteria:**
- [ ] All components have unit tests
- [ ] All services have unit tests
- [ ] Test coverage minimum 80% for components
- [ ] Test coverage 100% for services
- [ ] Mock external dependencies
- [ ] Test user interactions
- [ ] Test validation logic
- [ ] Test error handling

### REQ-FE-STF-047: Implement End-to-End Tests

**Requirement:** The UI shall have E2E tests for critical user journeys.

**Acceptance Criteria:**
- [ ] E2E test for staff registration flow
- [ ] E2E test for staff profile edit flow
- [ ] E2E test for photo upload flow
- [ ] E2E test for availability management
- [ ] E2E test for assignment workflow
- [ ] E2E test for check-in/out process
- [ ] E2E test for performance review submission
- [ ] E2E tests run in CI/CD pipeline
- [ ] Tests use Page Object Model pattern

### REQ-FE-STF-048: Implement Accessibility Testing

**Requirement:** The UI shall be tested for accessibility compliance.

**Acceptance Criteria:**
- [ ] Automated accessibility tests with axe
- [ ] Keyboard navigation tested
- [ ] Screen reader compatibility tested
- [ ] Color contrast validated
- [ ] ARIA labels verified
- [ ] Focus management tested
- [ ] WCAG 2.1 AA compliance verified

### REQ-FE-STF-049: Implement Visual Regression Testing

**Requirement:** The UI shall have visual regression tests to detect UI changes.

**Acceptance Criteria:**
- [ ] Screenshot tests for key pages
- [ ] Baseline images stored in repository
- [ ] Automated comparison on PR
- [ ] Different viewport sizes tested
- [ ] Visual diff reports generated
- [ ] Failures block PR merge

### REQ-FE-STF-050: Achieve Test Coverage Targets

**Requirement:** The UI shall meet minimum test coverage requirements.

**Acceptance Criteria:**
- [ ] Overall test coverage minimum 80%
- [ ] Service layer coverage 100%
- [ ] Form validation coverage 100%
- [ ] Critical paths covered by E2E tests
- [ ] Coverage reports generated automatically
- [ ] Coverage tracked over time
- [ ] PRs blocked if coverage decreases

---

## 3. Page Structure

### 3.1 Pages
```
src/EventManagementPlatform.WebApp/projects/EventManagementPlatform/src/app/
├── pages/
│   ├── staff/
│   │   ├── staff-list/
│   │   │   ├── staff-list.ts
│   │   │   ├── staff-list.html
│   │   │   ├── staff-list.scss
│   │   │   └── index.ts
│   │   ├── staff-detail/
│   │   │   ├── staff-detail.ts
│   │   │   ├── staff-detail.html
│   │   │   ├── staff-detail.scss
│   │   │   └── index.ts
│   │   ├── staff-register/
│   │   │   ├── staff-register.ts
│   │   │   ├── staff-register.html
│   │   │   ├── staff-register.scss
│   │   │   └── index.ts
│   │   ├── staff-edit/
│   │   │   ├── staff-edit.ts
│   │   │   ├── staff-edit.html
│   │   │   ├── staff-edit.scss
│   │   │   └── index.ts
│   │   ├── staff-availability/
│   │   │   ├── staff-availability.ts
│   │   │   ├── staff-availability.html
│   │   │   ├── staff-availability.scss
│   │   │   └── index.ts
│   │   ├── staff-schedule/
│   │   │   ├── staff-schedule.ts
│   │   │   ├── staff-schedule.html
│   │   │   ├── staff-schedule.scss
│   │   │   └── index.ts
│   │   └── index.ts
```

### 3.2 Components
```
├── components/
│   ├── staff/
│   │   ├── staff-card/
│   │   │   ├── staff-card.ts
│   │   │   ├── staff-card.html
│   │   │   ├── staff-card.scss
│   │   │   └── index.ts
│   │   ├── staff-status-badge/
│   │   │   ├── staff-status-badge.ts
│   │   │   ├── staff-status-badge.html
│   │   │   ├── staff-status-badge.scss
│   │   │   └── index.ts
│   │   ├── staff-profile-form/
│   │   │   ├── staff-profile-form.ts
│   │   │   ├── staff-profile-form.html
│   │   │   ├── staff-profile-form.scss
│   │   │   └── index.ts
│   │   ├── staff-photo-upload/
│   │   │   ├── staff-photo-upload.ts
│   │   │   ├── staff-photo-upload.html
│   │   │   ├── staff-photo-upload.scss
│   │   │   └── index.ts
│   │   ├── staff-availability-calendar/
│   │   │   ├── staff-availability-calendar.ts
│   │   │   ├── staff-availability-calendar.html
│   │   │   ├── staff-availability-calendar.scss
│   │   │   └── index.ts
│   │   ├── staff-assignment-list/
│   │   │   ├── staff-assignment-list.ts
│   │   │   ├── staff-assignment-list.html
│   │   │   ├── staff-assignment-list.scss
│   │   │   └── index.ts
│   │   ├── staff-rating-display/
│   │   │   ├── staff-rating-display.ts
│   │   │   ├── staff-rating-display.html
│   │   │   ├── staff-rating-display.scss
│   │   │   └── index.ts
│   │   ├── staff-review-dialog/
│   │   │   ├── staff-review-dialog.ts
│   │   │   ├── staff-review-dialog.html
│   │   │   ├── staff-review-dialog.scss
│   │   │   └── index.ts
│   │   ├── staff-assignment-dialog/
│   │   │   ├── staff-assignment-dialog.ts
│   │   │   ├── staff-assignment-dialog.html
│   │   │   ├── staff-assignment-dialog.scss
│   │   │   └── index.ts
│   │   ├── staff-check-in-dialog/
│   │   │   ├── staff-check-in-dialog.ts
│   │   │   ├── staff-check-in-dialog.html
│   │   │   ├── staff-check-in-dialog.scss
│   │   │   └── index.ts
│   │   ├── staff-filter-panel/
│   │   │   ├── staff-filter-panel.ts
│   │   │   ├── staff-filter-panel.html
│   │   │   ├── staff-filter-panel.scss
│   │   │   └── index.ts
│   │   └── index.ts
```

---

## 4. Services

### 4.1 StaffService
```typescript
@Injectable({ providedIn: 'root' })
export class StaffService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getStaff(params: StaffQueryParams): Observable<PagedResult<StaffListDto>> { }

    getStaffById(staffId: string): Observable<StaffDetailDto> { }

    registerStaff(staff: RegisterStaffDto): Observable<StaffDetailDto> { }

    updateStaff(staffId: string, staff: UpdateStaffDto): Observable<StaffDetailDto> { }

    activateStaff(staffId: string): Observable<void> { }

    deactivateStaff(staffId: string, reason: string): Observable<void> { }

    uploadPhoto(staffId: string, photo: File): Observable<string> { }

    deletePhoto(staffId: string): Observable<void> { }
}
```

### 4.2 StaffAvailabilityService
```typescript
@Injectable({ providedIn: 'root' })
export class StaffAvailabilityService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getAvailability(staffId: string): Observable<StaffAvailabilityDto[]> { }

    declareAvailability(staffId: string, availability: DeclareAvailabilityDto): Observable<StaffAvailabilityDto> { }

    updateAvailability(staffId: string, availabilityId: string, availability: UpdateAvailabilityDto): Observable<StaffAvailabilityDto> { }

    deleteAvailability(staffId: string, availabilityId: string): Observable<void> { }

    setRecurringAvailability(staffId: string, pattern: RecurringAvailabilityDto): Observable<void> { }

    getUnavailableDates(staffId: string): Observable<UnavailableDateDto[]> { }

    addUnavailableDate(staffId: string, date: AddUnavailableDateDto): Observable<UnavailableDateDto> { }

    removeUnavailableDate(staffId: string, dateId: string): Observable<void> { }
}
```

### 4.3 StaffAssignmentService
```typescript
@Injectable({ providedIn: 'root' })
export class StaffAssignmentService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getAssignments(params: AssignmentQueryParams): Observable<PagedResult<StaffAssignmentDto>> { }

    getStaffAssignments(staffId: string): Observable<StaffAssignmentDto[]> { }

    assignStaffToEvent(assignment: AssignStaffDto): Observable<StaffAssignmentDto> { }

    updateAssignment(assignmentId: string, assignment: UpdateAssignmentDto): Observable<StaffAssignmentDto> { }

    removeAssignment(assignmentId: string): Observable<void> { }

    confirmAssignment(assignmentId: string): Observable<void> { }

    declineAssignment(assignmentId: string, reason: string): Observable<void> { }

    findAvailableStaff(eventId: string, role: StaffRole): Observable<AvailableStaffDto[]> { }
}
```

### 4.4 StaffCheckInService
```typescript
@Injectable({ providedIn: 'root' })
export class StaffCheckInService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    checkIn(assignmentId: string): Observable<CheckInOutDto> { }

    checkOut(assignmentId: string): Observable<CheckInOutDto> { }

    markNoShow(assignmentId: string): Observable<void> { }

    getTimesheet(staffId: string, startDate: Date, endDate: Date): Observable<TimesheetDto> { }
}
```

### 4.5 StaffPerformanceService
```typescript
@Injectable({ providedIn: 'root' })
export class StaffPerformanceService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getReviews(staffId: string): Observable<StaffReviewDto[]> { }

    addReview(staffId: string, review: AddReviewDto): Observable<StaffReviewDto> { }

    submitFeedback(staffId: string, feedback: FeedbackDto): Observable<void> { }

    fileComplaint(staffId: string, complaint: ComplaintDto): Observable<void> { }

    giveCompliment(staffId: string, compliment: ComplimentDto): Observable<void> { }

    getRatingsSummary(staffId: string): Observable<RatingsSummaryDto> { }
}
```

---

## 5. Routing

### 5.1 Route Configuration
```typescript
export const staffRoutes: Routes = [
    {
        path: 'staff',
        children: [
            {
                path: '',
                loadComponent: () => import('./pages/staff/staff-list').then(m => m.StaffList),
                title: 'Staff Members'
            },
            {
                path: 'register',
                loadComponent: () => import('./pages/staff/staff-register').then(m => m.StaffRegister),
                title: 'Register Staff',
                canActivate: [authGuard, roleGuard(['Manager', 'Admin'])]
            },
            {
                path: ':staffId',
                loadComponent: () => import('./pages/staff/staff-detail').then(m => m.StaffDetail),
                title: 'Staff Details'
            },
            {
                path: ':staffId/edit',
                loadComponent: () => import('./pages/staff/staff-edit').then(m => m.StaffEdit),
                title: 'Edit Staff',
                canActivate: [authGuard, roleGuard(['Manager', 'Admin'])],
                canDeactivate: [unsavedChangesGuard]
            },
            {
                path: ':staffId/availability',
                loadComponent: () => import('./pages/staff/staff-availability').then(m => m.StaffAvailability),
                title: 'Manage Availability',
                canActivate: [authGuard]
            },
            {
                path: ':staffId/schedule',
                loadComponent: () => import('./pages/staff/staff-schedule').then(m => m.StaffSchedule),
                title: 'Staff Schedule',
                canActivate: [authGuard]
            }
        ]
    }
];
```

---

## 6. Appendices

### 6.1 Related Documents
- [Backend Specification](./backend-spec.md)
- [Use Case Diagram](./diagrams/use-case.drawio)
- [C4 Diagrams](./diagrams/c4-context.drawio)
- [Class Diagram](./plantuml/class-diagram.plantuml)
- [Sequence Diagrams](./plantuml/sequence-diagram.plantuml)

### 6.2 Wireframes
Wireframes are available in the design system documentation.

### 6.3 Revision History
| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-22 | System | Initial specification with requirements format |
