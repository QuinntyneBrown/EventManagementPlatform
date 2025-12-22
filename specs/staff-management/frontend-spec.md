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

## 2. Page Structure

### 2.1 Pages
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

### 2.2 Components
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

## 3. User Interface Requirements

### 3.1 Staff List Page

#### 3.1.1 Layout
- **Header**: Page title, search bar, register button
- **Filter Panel**: Status, role, skills, rating filters
- **Content**: Responsive grid/list of staff cards
- **Pagination**: Bottom pagination with page size selector

#### 3.1.2 Features
| Feature | Description |
|---------|-------------|
| Search | Real-time search by name, email |
| Filter | Filter by status, role, skills, minimum rating |
| Sort | Sort by name, rating, hire date |
| View Toggle | Grid view / List view toggle |
| Bulk Actions | Select multiple staff for bulk operations |
| Quick Actions | Activate/Deactivate, View schedule |

#### 3.1.3 Responsive Behavior
| Breakpoint | Layout |
|------------|--------|
| < 600px | Single column, stacked cards |
| 600-960px | 2-column grid |
| > 960px | 3-4 column grid or table view |

### 3.2 Staff Detail Page

#### 3.2.1 Sections
| Section | Content |
|---------|---------|
| Header | Photo, name, role, status badge, action buttons |
| Profile Card | Contact info, hire date, hourly rate, skills |
| Availability Tab | Weekly availability schedule, unavailable dates |
| Assignments Tab | List of current and past assignments |
| Performance Tab | Ratings, reviews, feedback history |
| Timesheet Tab | Check-in/out history with hours worked |
| Activity Tab | Audit log of profile changes |

#### 3.2.2 Actions
| Action | Condition | UI Element |
|--------|-----------|------------|
| Edit Profile | User is Manager/Admin | Primary button |
| Upload Photo | User is Manager/Admin or self | Icon button |
| Activate | Status == Inactive | Success button |
| Deactivate | Status == Active | Warning button |
| Manage Availability | User is Staff/Manager | Secondary button |
| Assign to Event | Status == Active, Manager only | Primary button |
| Check In | Has active assignment | Success button |
| Check Out | Checked in | Success button |
| Add Review | Manager only, completed assignment | Secondary button |

### 3.3 Staff Register/Edit Page

#### 3.3.1 Form Fields
| Field | Type | Validation |
|-------|------|------------|
| First Name | mat-input | Required, max 100 chars |
| Last Name | mat-input | Required, max 100 chars |
| Email | mat-input | Required, valid email, unique |
| Phone Number | mat-input | Required, phone format |
| Role | mat-select | Required |
| Hire Date | mat-datepicker | Required, not future |
| Hourly Rate | mat-input | Optional, number >= 0 |
| Skills | mat-chip-list | Optional, multi-select |
| Photo | File upload | Optional, max 5MB, image only |

#### 3.3.2 Form Behavior
- Real-time validation feedback
- Photo preview before upload
- Email uniqueness check on blur
- Phone number formatting
- Skills autocomplete from predefined list

### 3.4 Staff Availability Page

#### 3.4.1 Layout
- **Calendar View**: Weekly calendar with availability blocks
- **List View**: List of availability entries
- **Unavailable Dates**: Calendar with marked unavailable dates

#### 3.4.2 Features
| Feature | Description |
|---------|-------------|
| Add Availability | Dialog to add time slots for specific days |
| Edit Availability | Inline editing of existing slots |
| Recurring Schedule | Set repeating weekly availability |
| Mark Unavailable | Add specific unavailable dates with reason |
| Copy Schedule | Copy availability to another week |

### 3.5 Staff Schedule Page

#### 3.5.1 Layout
- **Timeline View**: Visual timeline of assignments
- **Calendar View**: Monthly calendar with assignments
- **List View**: Chronological list of assignments

#### 3.5.2 Features
| Feature | Description |
|---------|-------------|
| View Assignments | See all upcoming and past assignments |
| Confirm Assignment | Accept requested assignment |
| Decline Assignment | Decline with reason |
| View Event Details | Navigate to event details |
| Check Conflicts | Highlight scheduling conflicts |

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

### 4.6 StaffStateService
```typescript
@Injectable({ providedIn: 'root' })
export class StaffStateService {
    private readonly staffSubject = new BehaviorSubject<StaffListDto[]>([]);
    private readonly selectedStaffSubject = new BehaviorSubject<StaffDetailDto | null>(null);
    private readonly loadingSubject = new BehaviorSubject<boolean>(false);

    readonly staff$ = this.staffSubject.asObservable();
    readonly selectedStaff$ = this.selectedStaffSubject.asObservable();
    readonly loading$ = this.loadingSubject.asObservable();

    loadStaff(params: StaffQueryParams): void { }

    selectStaff(staffId: string): void { }

    clearSelection(): void { }

    refreshStaff(): void { }
}
```

---

## 5. Models/Interfaces

### 5.1 DTOs
```typescript
export interface StaffListDto {
    staffMemberId: string;
    fullName: string;
    email: string;
    status: StaffStatus;
    role: StaffRole;
    averageRating?: number;
    photoUrl?: string;
}

export interface StaffDetailDto {
    staffMemberId: string;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    photoUrl?: string;
    status: StaffStatus;
    role: StaffRole;
    hireDate: Date;
    terminationDate?: Date;
    hourlyRate?: number;
    skills: string[];
    averageRating: number;
    totalAssignments: number;
    completedAssignments: number;
    createdAt: Date;
}

export interface RegisterStaffDto {
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    role: StaffRole;
    hireDate: Date;
    hourlyRate?: number;
    skills?: string[];
}

export interface UpdateStaffDto {
    firstName: string;
    lastName: string;
    phoneNumber: string;
    role: StaffRole;
    hourlyRate?: number;
    skills?: string[];
}

export interface StaffAvailabilityDto {
    staffAvailabilityId: string;
    dayOfWeek: DayOfWeek;
    startTime: string;
    endTime: string;
    isRecurring: boolean;
    effectiveFrom: Date;
    effectiveTo?: Date;
}

export interface DeclareAvailabilityDto {
    dayOfWeek: DayOfWeek;
    startTime: string;
    endTime: string;
    isRecurring: boolean;
    effectiveFrom: Date;
    effectiveTo?: Date;
}

export interface StaffAssignmentDto {
    staffAssignmentId: string;
    staffMemberId: string;
    staffName: string;
    eventId: string;
    eventTitle: string;
    eventDate: Date;
    assignedRole: StaffRole;
    status: AssignmentStatus;
    requestedAt: Date;
    confirmedAt?: Date;
}

export interface AssignStaffDto {
    staffMemberId: string;
    eventId: string;
    assignedRole: StaffRole;
    notes?: string;
}

export interface CheckInOutDto {
    staffCheckInOutId: string;
    staffAssignmentId: string;
    checkInTime: Date;
    checkOutTime?: Date;
    totalHours?: number;
}

export interface StaffReviewDto {
    staffPerformanceReviewId: string;
    staffMemberId: string;
    eventId?: string;
    rating: number;
    feedback?: string;
    reviewType: ReviewType;
    reviewedBy: string;
    reviewedAt: Date;
}

export interface AddReviewDto {
    eventId?: string;
    rating: number;
    feedback?: string;
    reviewType: ReviewType;
}

export interface TimesheetDto {
    staffMemberId: string;
    startDate: Date;
    endDate: Date;
    entries: CheckInOutDto[];
    totalHours: number;
}

export interface RatingsSummaryDto {
    staffMemberId: string;
    averageRating: number;
    totalRatings: number;
    ratingDistribution: { [rating: number]: number };
    recentFeedback: StaffReviewDto[];
}
```

### 5.2 Enums
```typescript
export enum StaffStatus {
    Active = 'Active',
    Inactive = 'Inactive',
    OnLeave = 'OnLeave',
    Terminated = 'Terminated'
}

export enum StaffRole {
    EventCoordinator = 'EventCoordinator',
    SetupCrew = 'SetupCrew',
    Server = 'Server',
    Bartender = 'Bartender',
    Chef = 'Chef',
    Security = 'Security',
    Photographer = 'Photographer',
    DJ = 'DJ',
    Manager = 'Manager',
    Other = 'Other'
}

export enum AssignmentStatus {
    Requested = 'Requested',
    Confirmed = 'Confirmed',
    Declined = 'Declined',
    Completed = 'Completed',
    NoShow = 'NoShow',
    Cancelled = 'Cancelled'
}

export enum ReviewType {
    Feedback = 'Feedback',
    Complaint = 'Complaint',
    Compliment = 'Compliment',
    PerformanceReview = 'PerformanceReview'
}

export enum DayOfWeek {
    Sunday = 0,
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday = 5,
    Saturday = 6
}
```

---

## 6. Routing

### 6.1 Route Configuration
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

## 7. Material Components Used

### 7.1 Component List
| Component | Usage |
|-----------|-------|
| mat-card | Staff cards, detail sections |
| mat-table | Staff list table view |
| mat-paginator | Pagination |
| mat-sort | Table sorting |
| mat-form-field | Form inputs |
| mat-input | Text inputs |
| mat-select | Dropdowns for role, status |
| mat-autocomplete | Skills selection |
| mat-datepicker | Date selection for hire date |
| mat-button | Action buttons |
| mat-icon | Icons throughout |
| mat-menu | Actions dropdown |
| mat-dialog | Photo upload, assignments, reviews |
| mat-snackbar | Notifications |
| mat-progress-spinner | Loading states |
| mat-chip | Status badges, skills display |
| mat-tabs | Detail page sections |
| mat-expansion-panel | Filter panel |
| mat-slider | Rating display |
| mat-badge | Notification counts |
| mat-calendar | Availability calendar |
| mat-button-toggle | View toggle (grid/list) |

---

## 8. Styling Guidelines

### 8.1 BEM Naming Convention
```scss
// Block
.staff-card { }

// Element
.staff-card__header { }
.staff-card__photo { }
.staff-card__name { }
.staff-card__role { }
.staff-card__rating { }
.staff-card__actions { }

// Modifier
.staff-card--inactive { }
.staff-card--featured { }
.staff-card--compact { }
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
| API 400 | Snackbar with details |
| API 404 | Redirect to not found page |
| API 401 | Redirect to login |
| API 403 | Display forbidden message |
| API 409 | Dialog with conflict details |
| API 500 | Error dialog with retry option |
| Network | Snackbar with retry action |
| Double Booking | Warning dialog with conflict info |

### 9.2 Error Interceptor
```typescript
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
    return next(req).pipe(
        catchError((error: HttpErrorResponse) => {
            // Handle double booking errors specially
            if (error.status === 409 && error.error?.errorCode === 'DOUBLE_BOOKING') {
                // Show conflict dialog
            }
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
| Image Alt Text | Alt text for all staff photos |

### 10.2 ARIA Implementation
```html
<mat-card role="article" [attr.aria-label]="staff.fullName + ' profile'">
    <mat-card-header>
        <img mat-card-avatar [src]="staff.photoUrl" [alt]="staff.fullName + ' photo'">
        <mat-card-title id="staff-{{staff.staffMemberId}}-name">
            {{staff.fullName}}
        </mat-card-title>
    </mat-card-header>
    <mat-card-content aria-describedby="staff-{{staff.staffMemberId}}-name">
        <!-- content -->
    </mat-card-content>
</mat-card>
```

---

## 11. Testing Requirements

### 11.1 Unit Tests (Jest)
| Component/Service | Test Coverage |
|-------------------|---------------|
| StaffService | 100% |
| StaffAvailabilityService | 100% |
| StaffAssignmentService | 100% |
| StaffStateService | 100% |
| StaffList | 90% |
| StaffDetail | 90% |
| StaffProfileForm | 100% (validation) |
| StaffAvailabilityCalendar | 90% |
| All components | Minimum 80% |

### 11.2 E2E Tests (Playwright)
| Scenario | Priority |
|----------|----------|
| Register new staff | High |
| Edit staff profile | High |
| Upload staff photo | High |
| Declare availability | High |
| Assign staff to event | High |
| Confirm/Decline assignment | High |
| Check in/out staff | High |
| Add performance review | High |
| Search and filter staff | Medium |
| View staff schedule | Medium |
| Mark unavailable dates | Medium |
| Double booking prevention | High |

### 11.3 Test File Naming
```
staff-list.spec.ts      // Unit test
staff-list.e2e.ts       // E2E test
```

---

## 12. Performance Requirements

| Metric | Requirement |
|--------|-------------|
| First Contentful Paint | < 1.5s |
| Time to Interactive | < 3s |
| Bundle Size | < 250KB (initial) |
| Staff List Render | < 100ms for 50 items |
| Navigation | < 200ms between pages |
| Photo Upload | < 3s for 5MB file |
| Availability Calendar | < 150ms to render week view |

### 12.1 Optimization Strategies
- Lazy loading of routes
- Virtual scrolling for large staff lists
- OnPush change detection
- Pagination (no infinite scroll)
- Image lazy loading and optimization
- Calendar component virtualization

---

## 13. Internationalization

### 13.1 i18n Support
- Use Angular i18n for translations
- Date formatting via DatePipe with locale
- Time formatting for availability
- Phone number formatting by locale

### 13.2 Date/Time Handling
- Store all dates in UTC
- Display in user's local timezone
- Use Angular Material datepicker with locale
- Time zone awareness for availability and assignments

---

## 14. Special Features

### 14.1 Photo Upload
- Drag and drop support
- Image preview before upload
- Automatic resizing to 500x500px
- Support for JPG, PNG formats
- Max file size: 5MB
- Loading progress indicator

### 14.2 Availability Calendar
- Interactive weekly calendar view
- Drag to create availability blocks
- Color-coded availability types
- Conflict highlighting
- Quick copy to other weeks
- Export to external calendar

### 14.3 Assignment Management
- Real-time conflict detection
- Smart staff recommendations based on skills and ratings
- Bulk assignment capabilities
- Assignment confirmation workflow
- Automatic reminders via notifications

### 14.4 Performance Dashboard
- Visual rating charts
- Feedback timeline
- Attendance statistics
- Performance trends over time
- Exportable reports

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
