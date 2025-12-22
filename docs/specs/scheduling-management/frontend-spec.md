# Scheduling & Calendar Management - Frontend Specification

## Document Information
| Field | Value |
|-------|-------|
| Feature | Scheduling & Calendar Management |
| Version | 1.0 |
| Date | 2025-12-22 |
| Status | Final |

---

## 1. Overview

### 1.1 Purpose
This specification defines the frontend requirements for the Scheduling & Calendar Management feature, providing users with comprehensive interfaces for scheduling events, viewing calendars, detecting conflicts, and managing resource availability.

### 1.2 Technology Stack
- **Framework**: Angular 18+ (latest stable)
- **UI Library**: Angular Material (Material 3)
- **Calendar Library**: FullCalendar Angular integration
- **State Management**: RxJS (no NgRx)
- **Date/Time**: date-fns library
- **Styling**: SCSS with BEM methodology
- **Testing**: Jest (unit), Playwright (e2e)

### 1.3 Design Principles
- Mobile-first responsive design
- Material 3 design guidelines
- Accessibility (WCAG 2.1 AA compliance)
- Default Angular Material theme colors only
- Intuitive drag-and-drop scheduling
- Real-time conflict feedback

---

## 2. Page Structure

### 2.1 Pages
```
src/EventManagementPlatform.WebApp/projects/EventManagementPlatform/src/app/
├── pages/
│   ├── scheduling/
│   │   ├── calendar-view/
│   │   │   ├── calendar-view.ts
│   │   │   ├── calendar-view.html
│   │   │   ├── calendar-view.scss
│   │   │   └── index.ts
│   │   ├── schedule-event/
│   │   │   ├── schedule-event.ts
│   │   │   ├── schedule-event.html
│   │   │   ├── schedule-event.scss
│   │   │   └── index.ts
│   │   ├── conflict-dashboard/
│   │   │   ├── conflict-dashboard.ts
│   │   │   ├── conflict-dashboard.html
│   │   │   ├── conflict-dashboard.scss
│   │   │   └── index.ts
│   │   ├── resource-availability/
│   │   │   ├── resource-availability.ts
│   │   │   ├── resource-availability.html
│   │   │   ├── resource-availability.scss
│   │   │   └── index.ts
│   │   └── index.ts
```

### 2.2 Components
```
├── components/
│   ├── scheduling/
│   │   ├── calendar-grid/
│   │   │   ├── calendar-grid.ts
│   │   │   ├── calendar-grid.html
│   │   │   ├── calendar-grid.scss
│   │   │   └── index.ts
│   │   ├── event-time-slot/
│   │   │   ├── event-time-slot.ts
│   │   │   ├── event-time-slot.html
│   │   │   ├── event-time-slot.scss
│   │   │   └── index.ts
│   │   ├── schedule-form/
│   │   │   ├── schedule-form.ts
│   │   │   ├── schedule-form.html
│   │   │   ├── schedule-form.scss
│   │   │   └── index.ts
│   │   ├── conflict-card/
│   │   │   ├── conflict-card.ts
│   │   │   ├── conflict-card.html
│   │   │   ├── conflict-card.scss
│   │   │   └── index.ts
│   │   ├── conflict-resolution-dialog/
│   │   │   ├── conflict-resolution-dialog.ts
│   │   │   ├── conflict-resolution-dialog.html
│   │   │   ├── conflict-resolution-dialog.scss
│   │   │   └── index.ts
│   │   ├── resource-availability-panel/
│   │   │   ├── resource-availability-panel.ts
│   │   │   ├── resource-availability-panel.html
│   │   │   ├── resource-availability-panel.scss
│   │   │   └── index.ts
│   │   ├── timeline-view/
│   │   │   ├── timeline-view.ts
│   │   │   ├── timeline-view.html
│   │   │   ├── timeline-view.scss
│   │   │   └── index.ts
│   │   ├── calendar-selector/
│   │   │   ├── calendar-selector.ts
│   │   │   ├── calendar-selector.html
│   │   │   ├── calendar-selector.scss
│   │   │   └── index.ts
│   │   ├── time-slot-picker/
│   │   │   ├── time-slot-picker.ts
│   │   │   ├── time-slot-picker.html
│   │   │   ├── time-slot-picker.scss
│   │   │   └── index.ts
│   │   ├── recurrence-pattern-editor/
│   │   │   ├── recurrence-pattern-editor.ts
│   │   │   ├── recurrence-pattern-editor.html
│   │   │   ├── recurrence-pattern-editor.scss
│   │   │   └── index.ts
│   │   ├── calendar-export-dialog/
│   │   │   ├── calendar-export-dialog.ts
│   │   │   ├── calendar-export-dialog.html
│   │   │   ├── calendar-export-dialog.scss
│   │   │   └── index.ts
│   │   └── index.ts
```

---

## 3. User Interface Requirements

### 3.1 Calendar View Page

#### 3.1.1 Layout
- **Header**: Calendar selector, view switcher (Day/Week/Month), date navigator, export button
- **Toolbar**: Filter panel, search, create event button, refresh button
- **Main Area**: Full calendar with event slots
- **Sidebar**: Upcoming conflicts, resource availability summary, legend

#### 3.1.2 Features
| Feature | Description |
|---------|-------------|
| View Modes | Day, Week, Month, Agenda views |
| Drag & Drop | Drag events to reschedule |
| Click to Create | Click empty slot to create event |
| Event Details | Click event to view/edit details |
| Multi-calendar | View multiple calendars overlayed |
| Color Coding | Events colored by status/type |
| Conflict Indicators | Visual warning for conflicting events |
| Time Zone Display | Show events in user's or calendar's timezone |

#### 3.1.3 Responsive Behavior
| Breakpoint | Layout |
|------------|--------|
| < 600px | Day view only, stacked layout, bottom nav |
| 600-960px | Day/Week view, collapsible sidebar |
| > 960px | All views, fixed sidebar, full features |

#### 3.1.4 Calendar Views

##### Day View
- **Time Range**: 6am - 11pm (configurable)
- **Time Slots**: 30-minute increments
- **Display**: Vertical timeline with events as blocks
- **Features**: Multi-resource columns, hour markers

##### Week View
- **Days**: Monday - Sunday (configurable start day)
- **Time Range**: 6am - 11pm
- **Display**: Grid layout with 7 columns
- **Features**: All-day event row, overflow indicators

##### Month View
- **Layout**: Traditional calendar grid
- **Display**: Event counts per day, up to 3 events shown
- **Features**: Click day to see all events, today highlight

##### Agenda View
- **Layout**: List view of upcoming events
- **Display**: Grouped by date, chronological order
- **Features**: Infinite scroll, date separators

### 3.2 Schedule Event Page

#### 3.2.1 Form Sections
| Section | Content |
|---------|---------|
| Event Selection | Select existing event to schedule |
| Calendar Selection | Choose target calendar(s) |
| Time & Date | Date picker, start/end time, timezone selector |
| Recurrence | Optional recurrence pattern configuration |
| Availability Check | Real-time availability verification |
| Conflict Preview | Show detected conflicts before saving |

#### 3.2.2 Form Fields
| Field | Type | Validation |
|-------|------|------------|
| Event | mat-autocomplete | Required, must exist |
| Calendar | mat-select | Required, must be active |
| Start Date | mat-datepicker | Required, must be future |
| Start Time | mat-timepicker | Required |
| End Date | mat-datepicker | Required, >= start date |
| End Time | mat-timepicker | Required, > start time |
| Timezone | mat-select | Required, IANA timezone |
| Recurrence | custom component | Optional |

#### 3.2.3 Smart Features
- **Auto-conflict Detection**: Check conflicts as user types
- **Smart Suggestions**: Suggest alternative time slots
- **Availability Visualization**: Show resource availability timeline
- **Quick Actions**: Buttons for common time durations (1hr, 2hr, 4hr)

### 3.3 Conflict Dashboard Page

#### 3.3.1 Layout
- **Header**: Page title, filter controls, bulk actions
- **Metrics Cards**: Total conflicts, critical count, average resolution time
- **Conflict List**: Filterable, sortable list of conflicts
- **Detail Panel**: Selected conflict details and resolution options

#### 3.3.2 Conflict List Columns
| Column | Content |
|--------|---------|
| Priority | Visual indicator (color/icon) |
| Conflict Type | Badge with type |
| Events | Both event names |
| Date/Time | When conflict occurs |
| Status | Current status badge |
| Detected | When detected |
| Actions | Resolve, escalate, dismiss buttons |

#### 3.3.3 Conflict Resolution Dialog
- **Conflict Details**: Summary of conflicting events
- **AI Suggestions**: Smart resolution recommendations
- **Resolution Options**:
  - Reschedule primary event
  - Reschedule conflicting event
  - Change resource allocation
  - Cancel one event
  - Apply override (if authorized)
  - Escalate to manager
- **Preview**: Show result of selected resolution
- **Notes**: Add resolution notes

### 3.4 Resource Availability Page

#### 3.4.1 Layout
- **Header**: Resource selector, date range selector
- **Timeline**: Visual availability timeline
- **Availability Grid**: Table view of availability
- **Actions Panel**: Mark available/unavailable controls

#### 3.4.2 Features
| Feature | Description |
|---------|-------------|
| Multi-resource View | Compare availability across resources |
| Timeline View | Visual representation of available slots |
| Quick Mark | Click to mark time ranges |
| Bulk Operations | Mark multiple resources/times |
| Availability Patterns | Set recurring availability rules |
| Export | Download availability report |

#### 3.4.3 Mark Unavailable Dialog
- **Resource Selection**: Choose resource(s)
- **Date Range**: Start and end date/time
- **Reason**: Required text field
- **Recurrence**: Optional recurring pattern
- **Notification**: Option to notify affected users

---

## 4. Services

### 4.1 SchedulingService
```typescript
@Injectable({ providedIn: 'root' })
export class SchedulingService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getScheduledEvents(params: ScheduleQueryParams): Observable<PagedResult<ScheduledEventListDto>> { }

    getScheduledEventById(scheduleId: string): Observable<ScheduledEventDetailDto> { }

    scheduleEvent(request: ScheduleEventDto): Observable<ScheduledEventDetailDto> { }

    rescheduleEvent(scheduleId: string, request: RescheduleEventDto): Observable<ScheduledEventDetailDto> { }

    removeEventFromCalendar(scheduleId: string): Observable<void> { }

    confirmSchedule(scheduleId: string): Observable<void> { }

    checkConflicts(request: ConflictCheckDto): Observable<ConflictCheckResultDto> { }
}
```

### 4.2 CalendarService
```typescript
@Injectable({ providedIn: 'root' })
export class CalendarService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getCalendars(): Observable<CalendarDto[]> { }

    getCalendarById(calendarId: string): Observable<CalendarDto> { }

    getCalendarView(calendarId: string, params: CalendarViewParams): Observable<CalendarViewDto> { }

    getDailySchedule(calendarId: string, date: Date): Observable<DailyScheduleDto> { }

    getWeeklySchedule(calendarId: string, weekStart: Date): Observable<WeeklyScheduleDto> { }

    getMonthlySchedule(calendarId: string, month: number, year: number): Observable<MonthlyScheduleDto> { }

    exportDailySchedule(calendarId: string, date: Date, format: ExportFormat): Observable<Blob> { }

    exportWeeklySchedule(calendarId: string, weekStart: Date, format: ExportFormat): Observable<Blob> { }

    exportMonthlySchedule(calendarId: string, month: number, year: number, format: ExportFormat): Observable<Blob> { }
}
```

### 4.3 ConflictService
```typescript
@Injectable({ providedIn: 'root' })
export class ConflictService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getConflicts(params: ConflictQueryParams): Observable<PagedResult<ConflictListDto>> { }

    getConflictById(conflictId: string): Observable<ConflictDetailDto> { }

    detectConflicts(): Observable<ConflictDetectionResultDto> { }

    resolveConflict(conflictId: string, resolution: ResolveConflictDto): Observable<void> { }

    escalateConflict(conflictId: string, escalation: EscalateConflictDto): Observable<void> { }

    dismissConflict(conflictId: string, reason: string): Observable<void> { }
}
```

### 4.4 ResourceAvailabilityService
```typescript
@Injectable({ providedIn: 'root' })
export class ResourceAvailabilityService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getResourceAvailability(resourceId: string, params: AvailabilityParams): Observable<ResourceAvailabilityDto> { }

    checkAvailability(request: CheckAvailabilityDto): Observable<AvailabilityResultDto> { }

    checkMultipleResources(request: CheckMultipleResourcesDto): Observable<AvailabilityResultDto[]> { }

    markResourceUnavailable(resourceId: string, request: MarkUnavailableDto): Observable<void> { }

    markResourceAvailable(resourceId: string, request: MarkAvailableDto): Observable<void> { }

    getAvailableSlots(resourceId: string, params: SlotQueryParams): Observable<TimeSlotDto[]> { }
}
```

### 4.5 SchedulingStateService
```typescript
@Injectable({ providedIn: 'root' })
export class SchedulingStateService {
    private readonly selectedCalendarSubject = new BehaviorSubject<CalendarDto | null>(null);
    private readonly currentViewSubject = new BehaviorSubject<CalendarViewType>('month');
    private readonly currentDateSubject = new BehaviorSubject<Date>(new Date());
    private readonly scheduledEventsSubject = new BehaviorSubject<ScheduledEventListDto[]>([]);
    private readonly conflictsSubject = new BehaviorSubject<ConflictListDto[]>([]);
    private readonly loadingSubject = new BehaviorSubject<boolean>(false);

    readonly selectedCalendar$ = this.selectedCalendarSubject.asObservable();
    readonly currentView$ = this.currentViewSubject.asObservable();
    readonly currentDate$ = this.currentDateSubject.asObservable();
    readonly scheduledEvents$ = this.scheduledEventsSubject.asObservable();
    readonly conflicts$ = this.conflictsSubject.asObservable();
    readonly loading$ = this.loadingSubject.asObservable();

    selectCalendar(calendar: CalendarDto): void { }

    setView(view: CalendarViewType): void { }

    setDate(date: Date): void { }

    navigateDate(direction: 'prev' | 'next'): void { }

    loadScheduledEvents(params: ScheduleQueryParams): void { }

    loadConflicts(): void { }

    refreshCalendar(): void { }
}
```

---

## 5. Models/Interfaces

### 5.1 Core Models
```typescript
export interface ScheduledEventListDto {
    scheduledEventId: string;
    eventId: string;
    eventTitle: string;
    calendarId: string;
    calendarName: string;
    startDateTime: Date;
    endDateTime: Date;
    timeZone: string;
    status: ScheduleStatus;
    conflictLevel: ConflictLevel;
    hasConflicts: boolean;
}

export interface ScheduledEventDetailDto extends ScheduledEventListDto {
    isBlocked: boolean;
    blockReason?: string;
    recurrencePattern?: RecurrencePattern;
    conflicts: ConflictSummaryDto[];
}

export interface CalendarDto {
    calendarId: string;
    name: string;
    description?: string;
    calendarType: CalendarType;
    ownerId: string;
    ownerName: string;
    ownerType: OwnerType;
    isActive: boolean;
    allowOverbooking: boolean;
    maxConcurrentEvents: number;
    timeZone: string;
}

export interface ConflictDetailDto {
    scheduleConflictId: string;
    scheduledEventId: string;
    scheduledEventTitle: string;
    conflictingEventId: string;
    conflictingEventTitle: string;
    conflictType: ConflictType;
    conflictLevel: ConflictLevel;
    status: ConflictStatus;
    detectedAt: Date;
    resolvedAt?: Date;
    resolutionMethod?: ResolutionMethod;
    suggestedResolutions: string[];
}

export interface ResourceAvailabilityDto {
    resourceId: string;
    resourceName: string;
    resourceType: ResourceType;
    checkStartTime: Date;
    checkEndTime: Date;
    isAvailable: boolean;
    unavailableReason?: string;
    availableSlots: TimeSlotDto[];
}

export interface TimeSlotDto {
    startDateTime: Date;
    endDateTime: Date;
    duration: string;
    isAvailable: boolean;
}

export interface RecurrencePattern {
    type: RecurrenceType;
    interval: number;
    daysOfWeek?: DayOfWeek[];
    dayOfMonth?: number;
    endDate?: Date;
    occurrenceCount?: number;
}
```

### 5.2 Enums
```typescript
export enum ScheduleStatus {
    Tentative = 'Tentative',
    Scheduled = 'Scheduled',
    Confirmed = 'Confirmed',
    Rescheduling = 'Rescheduling',
    Cancelled = 'Cancelled'
}

export enum ConflictLevel {
    None = 'None',
    Minor = 'Minor',
    Moderate = 'Moderate',
    Severe = 'Severe',
    Critical = 'Critical'
}

export enum ConflictType {
    TimeOverlap = 'TimeOverlap',
    ResourceConflict = 'ResourceConflict',
    VenueConflict = 'VenueConflict',
    StaffConflict = 'StaffConflict',
    EquipmentConflict = 'EquipmentConflict'
}

export enum ConflictStatus {
    Detected = 'Detected',
    UnderReview = 'UnderReview',
    Resolved = 'Resolved',
    Escalated = 'Escalated',
    Dismissed = 'Dismissed'
}

export enum ResolutionMethod {
    Rescheduled = 'Rescheduled',
    ResourceChanged = 'ResourceChanged',
    EventCancelled = 'EventCancelled',
    OverrideApplied = 'OverrideApplied',
    Split = 'Split'
}

export enum CalendarViewType {
    Day = 'day',
    Week = 'week',
    Month = 'month',
    Agenda = 'agenda'
}

export enum ResourceType {
    Venue = 'Venue',
    Staff = 'Staff',
    Equipment = 'Equipment',
    Other = 'Other'
}

export enum RecurrenceType {
    None = 'None',
    Daily = 'Daily',
    Weekly = 'Weekly',
    Monthly = 'Monthly',
    Yearly = 'Yearly',
    Custom = 'Custom'
}

export enum ExportFormat {
    PDF = 'PDF',
    Excel = 'Excel',
    iCalendar = 'iCalendar'
}
```

---

## 6. Component Specifications

### 6.1 CalendarGridComponent

#### 6.1.1 Purpose
Main calendar display component using FullCalendar library.

#### 6.1.2 Inputs
```typescript
@Input() calendarId: string;
@Input() viewType: CalendarViewType = 'month';
@Input() events: ScheduledEventListDto[] = [];
@Input() editable: boolean = true;
@Input() selectable: boolean = true;
```

#### 6.1.3 Outputs
```typescript
@Output() eventClicked = new EventEmitter<ScheduledEventListDto>();
@Output() dateClicked = new EventEmitter<Date>();
@Output() eventDropped = new EventEmitter<EventDropInfo>();
@Output() eventResized = new EventEmitter<EventResizeInfo>();
@Output() dateRangeChanged = new EventEmitter<DateRange>();
```

#### 6.1.4 Key Features
- FullCalendar integration
- Drag & drop support
- Event resizing
- Color-coded events by conflict level
- Responsive layout
- Touch support for mobile

### 6.2 ConflictCardComponent

#### 6.2.1 Purpose
Display individual conflict with resolution options.

#### 6.2.2 Inputs
```typescript
@Input() conflict: ConflictDetailDto;
@Input() showActions: boolean = true;
```

#### 6.2.3 Outputs
```typescript
@Output() resolve = new EventEmitter<ConflictDetailDto>();
@Output() escalate = new EventEmitter<ConflictDetailDto>();
@Output() dismiss = new EventEmitter<ConflictDetailDto>();
```

#### 6.2.4 Visual Design
- Priority indicator (left border color)
- Conflict type icon
- Event titles with links
- Time overlap visualization
- Status badge
- Action buttons (contextual based on permissions)

### 6.3 TimeSlotPickerComponent

#### 6.3.1 Purpose
Interactive time slot selection with availability visualization.

#### 6.3.2 Inputs
```typescript
@Input() date: Date;
@Input() resourceId: string;
@Input() resourceType: ResourceType;
@Input() minDuration: number = 15; // minutes
@Input() slotInterval: number = 30; // minutes
```

#### 6.3.3 Outputs
```typescript
@Output() slotSelected = new EventEmitter<TimeSlotDto>();
@Output() availabilityChecked = new EventEmitter<Date>();
```

#### 6.3.4 Features
- Visual timeline with available/unavailable slots
- Click to select start/end times
- Drag to select range
- Color coding (green=available, red=unavailable, yellow=partial)
- Duration display
- Conflict warnings

### 6.4 RecurrencePatternEditorComponent

#### 6.4.1 Purpose
Configure event recurrence patterns.

#### 6.4.2 Form Structure
```typescript
recurrenceForm = new FormGroup({
    type: new FormControl<RecurrenceType>('None'),
    interval: new FormControl<number>(1),
    daysOfWeek: new FormControl<DayOfWeek[]>([]),
    dayOfMonth: new FormControl<number | null>(null),
    endType: new FormControl<'date' | 'count' | 'never'>('never'),
    endDate: new FormControl<Date | null>(null),
    occurrenceCount: new FormControl<number | null>(null)
});
```

#### 6.4.3 UI Elements
- Recurrence type selector (None, Daily, Weekly, Monthly, Yearly)
- Conditional fields based on type
- Preview of next 5 occurrences
- End condition options (never, on date, after N times)
- Validation messages

---

## 7. Routing

### 7.1 Route Configuration
```typescript
const routes: Routes = [
    {
        path: 'scheduling',
        children: [
            {
                path: 'calendar',
                component: CalendarViewPage,
                data: { title: 'Calendar View' }
            },
            {
                path: 'schedule-event',
                component: ScheduleEventPage,
                data: { title: 'Schedule Event' }
            },
            {
                path: 'conflicts',
                component: ConflictDashboardPage,
                data: { title: 'Conflict Dashboard' }
            },
            {
                path: 'availability',
                component: ResourceAvailabilityPage,
                data: { title: 'Resource Availability' }
            }
        ]
    }
];
```

---

## 8. State Management Patterns

### 8.1 Calendar State Flow
```typescript
// Load calendar view
this.schedulingStateService.selectCalendar(calendar);
this.schedulingStateService.setDate(new Date());
this.schedulingStateService.setView('month');

// Subscribe to updates
combineLatest([
    this.schedulingStateService.selectedCalendar$,
    this.schedulingStateService.currentDate$,
    this.schedulingStateService.currentView$
]).pipe(
    switchMap(([calendar, date, view]) => {
        if (!calendar) return of(null);
        return this.calendarService.getCalendarView(calendar.calendarId, { date, view });
    })
).subscribe(calendarView => {
    // Update UI
});
```

### 8.2 Conflict Detection Flow
```typescript
// Real-time conflict checking during scheduling
this.scheduleForm.valueChanges.pipe(
    debounceTime(500),
    filter(form => form.startDateTime && form.endDateTime && form.calendarId),
    switchMap(form =>
        this.schedulingService.checkConflicts({
            calendarId: form.calendarId,
            startDateTime: form.startDateTime,
            endDateTime: form.endDateTime,
            excludeEventId: form.eventId
        })
    )
).subscribe(result => {
    this.conflicts = result.conflicts;
    this.updateConflictWarnings();
});
```

---

## 9. Validation & Error Handling

### 9.1 Form Validation
```typescript
export class ScheduleEventValidators {
    static futureDate(): ValidatorFn {
        return (control: AbstractControl): ValidationErrors | null => {
            if (!control.value) return null;
            const date = new Date(control.value);
            return date > new Date() ? null : { futureDate: true };
        };
    }

    static endAfterStart(startControlName: string, endControlName: string): ValidatorFn {
        return (group: AbstractControl): ValidationErrors | null => {
            const start = group.get(startControlName)?.value;
            const end = group.get(endControlName)?.value;
            if (!start || !end) return null;
            return new Date(end) > new Date(start) ? null : { endAfterStart: true };
        };
    }

    static minDuration(minutes: number): ValidatorFn {
        return (group: AbstractControl): ValidationErrors | null => {
            const start = group.get('startDateTime')?.value;
            const end = group.get('endDateTime')?.value;
            if (!start || !end) return null;
            const duration = (new Date(end).getTime() - new Date(start).getTime()) / 60000;
            return duration >= minutes ? null : { minDuration: { required: minutes, actual: duration } };
        };
    }
}
```

### 9.2 Error Messages
```typescript
export const SCHEDULING_ERROR_MESSAGES = {
    futureDate: 'Start date must be in the future',
    endAfterStart: 'End time must be after start time',
    minDuration: 'Event duration must be at least 15 minutes',
    maxDuration: 'Event duration cannot exceed 30 days',
    conflictDetected: 'This time slot conflicts with existing events',
    resourceUnavailable: 'Selected resource is not available at this time',
    overbooking: 'Calendar has reached maximum capacity for this time',
    invalidTimezone: 'Invalid timezone selected'
};
```

---

## 10. Accessibility Features

### 10.1 ARIA Labels
- All interactive elements have aria-labels
- Calendar events have aria-describedby for details
- Conflict indicators have role="alert"
- Loading states announced with aria-live regions

### 10.2 Keyboard Navigation
| Key | Action |
|-----|--------|
| Tab | Navigate between calendar elements |
| Arrow Keys | Navigate days in calendar |
| Enter | Select/open event |
| Space | Toggle selection |
| Escape | Close dialogs/cancel actions |
| Home/End | Jump to start/end of week/month |

### 10.3 Screen Reader Support
- Calendar structure announced properly
- Event details read in logical order
- Conflict severity announced
- Status changes announced
- Form validation errors announced

---

## 11. Performance Optimization

### 11.1 Lazy Loading
- Calendar component lazy loaded
- Large month views use virtual scrolling
- Event details loaded on demand

### 11.2 Caching Strategy
```typescript
@Injectable({ providedIn: 'root' })
export class CalendarCacheService {
    private cache = new Map<string, CacheEntry>();
    private readonly CACHE_DURATION = 5 * 60 * 1000; // 5 minutes

    get(key: string): CalendarViewDto | null {
        const entry = this.cache.get(key);
        if (!entry) return null;
        if (Date.now() - entry.timestamp > this.CACHE_DURATION) {
            this.cache.delete(key);
            return null;
        }
        return entry.data;
    }

    set(key: string, data: CalendarViewDto): void {
        this.cache.set(key, {
            data,
            timestamp: Date.now()
        });
    }
}
```

### 11.3 Change Detection
- OnPush strategy for all components
- Immutable data patterns
- Observables with async pipe
- TrackBy functions for lists

---

## 12. Testing Requirements

### 12.1 Unit Tests
```typescript
describe('CalendarViewPage', () => {
    it('should load calendar view on init', () => { });
    it('should handle view type changes', () => { });
    it('should detect date navigation', () => { });
    it('should emit event when event clicked', () => { });
});

describe('ConflictCardComponent', () => {
    it('should display conflict details', () => { });
    it('should show appropriate actions based on status', () => { });
    it('should emit resolve event', () => { });
});

describe('SchedulingService', () => {
    it('should schedule event successfully', () => { });
    it('should handle conflict detection', () => { });
    it('should reschedule event', () => { });
});
```

### 12.2 E2E Tests
```typescript
test('schedule event workflow', async ({ page }) => {
    await page.goto('/scheduling/calendar');
    await page.click('[data-testid="schedule-event-button"]');
    await page.fill('[data-testid="event-input"]', 'Test Event');
    await page.click('[data-testid="date-picker"]');
    await page.click('[data-testid="save-button"]');
    await expect(page.locator('[data-testid="success-message"]')).toBeVisible();
});

test('conflict resolution workflow', async ({ page }) => {
    await page.goto('/scheduling/conflicts');
    await page.click('[data-testid="conflict-card"]:first-child');
    await page.click('[data-testid="resolve-button"]');
    await page.selectOption('[data-testid="resolution-method"]', 'Rescheduled');
    await page.click('[data-testid="confirm-button"]');
    await expect(page.locator('[data-testid="resolved-badge"]')).toBeVisible();
});
```

### 12.3 Test Coverage Requirements
- Minimum 85% code coverage
- 100% coverage for critical scheduling logic
- All user workflows covered by E2E tests

---

## 13. User Experience Guidelines

### 13.1 Loading States
- Show skeleton screens during calendar load
- Progress indicators for long operations
- Optimistic updates for quick feedback

### 13.2 Error States
- Inline validation errors
- Toast notifications for async errors
- Conflict warnings with clear messaging
- Recovery suggestions

### 13.3 Empty States
- Helpful message when no events scheduled
- Quick action to schedule first event
- Tips for using calendar features

### 13.4 Feedback Messages
| Action | Success Message | Error Message |
|--------|----------------|---------------|
| Schedule Event | "Event scheduled successfully" | "Failed to schedule event: [reason]" |
| Reschedule | "Event rescheduled to [new time]" | "Cannot reschedule: [reason]" |
| Resolve Conflict | "Conflict resolved" | "Failed to resolve conflict: [reason]" |
| Mark Unavailable | "Resource marked unavailable" | "Failed to update availability: [reason]" |

---

## 14. Browser Support

| Browser | Minimum Version | Notes |
|---------|----------------|-------|
| Chrome | 90+ | Full support |
| Firefox | 88+ | Full support |
| Safari | 14+ | Full support |
| Edge | 90+ | Full support |
| Mobile Safari | iOS 14+ | Touch optimized |
| Chrome Mobile | Android 90+ | Touch optimized |

---

## 15. Third-Party Libraries

### 15.1 FullCalendar
```typescript
import { FullCalendarModule } from '@fullcalendar/angular';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';

calendarOptions: CalendarOptions = {
    plugins: [dayGridPlugin, timeGridPlugin, interactionPlugin],
    initialView: 'dayGridMonth',
    headerToolbar: {
        left: 'prev,next today',
        center: 'title',
        right: 'dayGridMonth,timeGridWeek,timeGridDay'
    },
    editable: true,
    selectable: true,
    selectMirror: true,
    dayMaxEvents: true,
    eventClick: this.handleEventClick.bind(this),
    select: this.handleDateSelect.bind(this),
    eventDrop: this.handleEventDrop.bind(this)
};
```

### 15.2 date-fns
```typescript
import {
    format,
    addDays,
    addWeeks,
    addMonths,
    startOfWeek,
    endOfWeek,
    isSameDay,
    parseISO
} from 'date-fns';
import { zonedTimeToUtc, utcToZonedTime } from 'date-fns-tz';
```

---

## 16. Appendices

### 16.1 Related Documents
- [Backend Specification](./backend-spec.md)
- [Use Case Diagram](./diagrams/use-case.drawio)
- [C4 Diagrams](./diagrams/c4-context.drawio)
- [Class Diagram](./plantuml/class-diagram.plantuml)
- [Sequence Diagrams](./plantuml/sequence-diagram.plantuml)

### 16.2 Design Mockups
- Calendar View (Day/Week/Month)
- Schedule Event Form
- Conflict Dashboard
- Resource Availability Timeline

### 16.3 Revision History
| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-22 | System | Initial specification |
