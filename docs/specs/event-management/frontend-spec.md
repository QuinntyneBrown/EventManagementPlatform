# Event Management - Frontend Specification

## Document Information
| Field | Value |
|-------|-------|
| Feature | Event Management |
| Version | 1.0 |
| Date | 2025-12-22 |
| Status | Final |

---

## 1. Overview

### 1.1 Purpose
This specification defines the frontend requirements for the Event Management feature, providing users with a comprehensive interface for managing events throughout their lifecycle.

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
│   ├── events/
│   │   ├── event-list/
│   │   │   ├── event-list.ts
│   │   │   ├── event-list.html
│   │   │   ├── event-list.scss
│   │   │   └── index.ts
│   │   ├── event-detail/
│   │   │   ├── event-detail.ts
│   │   │   ├── event-detail.html
│   │   │   ├── event-detail.scss
│   │   │   └── index.ts
│   │   ├── event-create/
│   │   │   ├── event-create.ts
│   │   │   ├── event-create.html
│   │   │   ├── event-create.scss
│   │   │   └── index.ts
│   │   ├── event-edit/
│   │   │   ├── event-edit.ts
│   │   │   ├── event-edit.html
│   │   │   ├── event-edit.scss
│   │   │   └── index.ts
│   │   └── index.ts
```

### 2.2 Components
```
├── components/
│   ├── events/
│   │   ├── event-card/
│   │   │   ├── event-card.ts
│   │   │   ├── event-card.html
│   │   │   ├── event-card.scss
│   │   │   └── index.ts
│   │   ├── event-status-badge/
│   │   │   ├── event-status-badge.ts
│   │   │   ├── event-status-badge.html
│   │   │   ├── event-status-badge.scss
│   │   │   └── index.ts
│   │   ├── event-form/
│   │   │   ├── event-form.ts
│   │   │   ├── event-form.html
│   │   │   ├── event-form.scss
│   │   │   └── index.ts
│   │   ├── event-notes-list/
│   │   │   ├── event-notes-list.ts
│   │   │   ├── event-notes-list.html
│   │   │   ├── event-notes-list.scss
│   │   │   └── index.ts
│   │   ├── event-note-dialog/
│   │   │   ├── event-note-dialog.ts
│   │   │   ├── event-note-dialog.html
│   │   │   ├── event-note-dialog.scss
│   │   │   └── index.ts
│   │   ├── event-actions-menu/
│   │   │   ├── event-actions-menu.ts
│   │   │   ├── event-actions-menu.html
│   │   │   ├── event-actions-menu.scss
│   │   │   └── index.ts
│   │   ├── event-filter-panel/
│   │   │   ├── event-filter-panel.ts
│   │   │   ├── event-filter-panel.html
│   │   │   ├── event-filter-panel.scss
│   │   │   └── index.ts
│   │   └── index.ts
```

---

## 3. User Interface Requirements

### 3.1 Event List Page

#### 3.1.1 Layout
- **Header**: Page title, search bar, create button
- **Filter Panel**: Status, date range, event type filters
- **Content**: Responsive grid/list of event cards
- **Pagination**: Bottom pagination with page size selector

#### 3.1.2 Features
| Feature | Description |
|---------|-------------|
| Search | Real-time search by event title |
| Filter | Filter by status, date, type, venue |
| Sort | Sort by date, title, status |
| View Toggle | Grid view / List view toggle |
| Bulk Actions | Select multiple events for bulk operations |

#### 3.1.3 Responsive Behavior
| Breakpoint | Layout |
|------------|--------|
| < 600px | Single column, stacked cards |
| 600-960px | 2-column grid |
| > 960px | 3-4 column grid or table view |

### 3.2 Event Detail Page

#### 3.2.1 Sections
| Section | Content |
|---------|---------|
| Header | Title, status badge, action buttons |
| Details Card | Date, time, venue, type, customer |
| Notes Tab | List of notes with add/edit capability |
| Activity Tab | Event history/audit log |
| Attachments Tab | Related documents |

#### 3.2.2 Actions
| Action | Condition | UI Element |
|--------|-----------|------------|
| Edit | Status != Archived | Primary button |
| Cancel | Status in [Confirmed, Approved] | Warning button |
| Reinstate | Status == Cancelled | Secondary button |
| Complete | Status == Confirmed | Success button |
| Archive | Status == Completed | Secondary button |
| Approve | Status == PendingApproval, User is Manager | Primary button |
| Reject | Status == PendingApproval, User is Manager | Warning button |

### 3.3 Event Create/Edit Page

#### 3.3.1 Form Fields
| Field | Type | Validation |
|-------|------|------------|
| Title | mat-input | Required, max 200 chars |
| Description | mat-textarea | Optional, max 2000 chars |
| Event Date | mat-datepicker | Required, future date |
| Event Time | mat-timepicker | Required |
| Venue | mat-select | Required, searchable |
| Event Type | mat-select | Required |
| Customer | mat-autocomplete | Required (pre-filled for customer users) |

#### 3.3.2 Form Behavior
- Auto-save draft every 30 seconds
- Unsaved changes warning on navigation
- Real-time validation feedback
- Smart venue suggestions based on date

---

## 4. Services

### 4.1 EventService
```typescript
@Injectable({ providedIn: 'root' })
export class EventService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getEvents(params: EventQueryParams): Observable<PagedResult<EventListDto>> { }

    getEventById(eventId: string): Observable<EventDetailDto> { }

    createEvent(event: CreateEventDto): Observable<EventDetailDto> { }

    updateEvent(eventId: string, event: UpdateEventDto): Observable<EventDetailDto> { }

    cancelEvent(eventId: string, reason: string): Observable<void> { }

    reinstateEvent(eventId: string): Observable<void> { }

    completeEvent(eventId: string): Observable<void> { }

    archiveEvent(eventId: string): Observable<void> { }

    submitForApproval(eventId: string): Observable<void> { }

    approveEvent(eventId: string): Observable<void> { }

    rejectEvent(eventId: string, reason: string): Observable<void> { }
}
```

### 4.2 EventNoteService
```typescript
@Injectable({ providedIn: 'root' })
export class EventNoteService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getNotes(eventId: string): Observable<EventNoteDto[]> { }

    addNote(eventId: string, note: CreateNoteDto): Observable<EventNoteDto> { }

    updateNote(eventId: string, noteId: string, note: UpdateNoteDto): Observable<EventNoteDto> { }

    deleteNote(eventId: string, noteId: string): Observable<void> { }
}
```

### 4.3 EventStateService
```typescript
@Injectable({ providedIn: 'root' })
export class EventStateService {
    private readonly eventsSubject = new BehaviorSubject<EventListDto[]>([]);
    private readonly selectedEventSubject = new BehaviorSubject<EventDetailDto | null>(null);
    private readonly loadingSubject = new BehaviorSubject<boolean>(false);

    readonly events$ = this.eventsSubject.asObservable();
    readonly selectedEvent$ = this.selectedEventSubject.asObservable();
    readonly loading$ = this.loadingSubject.asObservable();

    loadEvents(params: EventQueryParams): void { }

    selectEvent(eventId: string): void { }

    clearSelection(): void { }

    refreshEvents(): void { }
}
```

---

## 5. Models/Interfaces

### 5.1 DTOs
```typescript
export interface EventListDto {
    eventId: string;
    title: string;
    eventDate: Date;
    venueName: string;
    eventTypeName: string;
    status: EventStatus;
    customerName: string;
}

export interface EventDetailDto {
    eventId: string;
    title: string;
    description?: string;
    eventDate: Date;
    venueId: string;
    venueName: string;
    eventTypeId: string;
    eventTypeName: string;
    status: EventStatus;
    customerId: string;
    customerName: string;
    createdAt: Date;
    modifiedAt?: Date;
    notes: EventNoteDto[];
}

export interface CreateEventDto {
    title: string;
    description?: string;
    eventDate: Date;
    venueId: string;
    eventTypeId: string;
    customerId: string;
}

export interface UpdateEventDto {
    title: string;
    description?: string;
    eventDate: Date;
    venueId: string;
    eventTypeId: string;
}

export interface EventNoteDto {
    eventNoteId: string;
    content: string;
    noteType: NoteType;
    createdAt: Date;
    createdByName: string;
}
```

### 5.2 Enums
```typescript
export enum EventStatus {
    Draft = 'Draft',
    PendingApproval = 'PendingApproval',
    Approved = 'Approved',
    Confirmed = 'Confirmed',
    InProgress = 'InProgress',
    Completed = 'Completed',
    Cancelled = 'Cancelled',
    Archived = 'Archived'
}

export enum NoteType {
    Internal = 'Internal',
    CustomerComment = 'CustomerComment',
    SystemGenerated = 'SystemGenerated'
}
```

---

## 6. Routing

### 6.1 Route Configuration
```typescript
export const eventRoutes: Routes = [
    {
        path: 'events',
        children: [
            {
                path: '',
                loadComponent: () => import('./pages/events/event-list').then(m => m.EventList),
                title: 'Events'
            },
            {
                path: 'create',
                loadComponent: () => import('./pages/events/event-create').then(m => m.EventCreate),
                title: 'Create Event',
                canActivate: [authGuard]
            },
            {
                path: ':eventId',
                loadComponent: () => import('./pages/events/event-detail').then(m => m.EventDetail),
                title: 'Event Details'
            },
            {
                path: ':eventId/edit',
                loadComponent: () => import('./pages/events/event-edit').then(m => m.EventEdit),
                title: 'Edit Event',
                canActivate: [authGuard],
                canDeactivate: [unsavedChangesGuard]
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
| mat-card | Event cards, detail sections |
| mat-table | Event list table view |
| mat-paginator | Pagination |
| mat-sort | Table sorting |
| mat-form-field | Form inputs |
| mat-input | Text inputs |
| mat-select | Dropdowns |
| mat-autocomplete | Customer/Venue search |
| mat-datepicker | Date selection |
| mat-button | Action buttons |
| mat-icon | Icons throughout |
| mat-menu | Actions dropdown |
| mat-dialog | Note editor, confirmations |
| mat-snackbar | Notifications |
| mat-progress-spinner | Loading states |
| mat-chip | Status badges |
| mat-tabs | Detail page sections |
| mat-expansion-panel | Filter panel |

---

## 8. Styling Guidelines

### 8.1 BEM Naming Convention
```scss
// Block
.event-card { }

// Element
.event-card__header { }
.event-card__title { }
.event-card__content { }
.event-card__footer { }
.event-card__actions { }

// Modifier
.event-card--cancelled { }
.event-card--highlighted { }
.event-card--compact { }
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

### 10.2 ARIA Implementation
```html
<mat-card role="article" [attr.aria-label]="event.title">
    <mat-card-header>
        <mat-card-title id="event-{{event.eventId}}-title">
            {{event.title}}
        </mat-card-title>
    </mat-card-header>
    <mat-card-content aria-describedby="event-{{event.eventId}}-title">
        <!-- content -->
    </mat-card-content>
</mat-card>
```

---

## 11. Testing Requirements

### 11.1 Unit Tests (Jest)
| Component/Service | Test Coverage |
|-------------------|---------------|
| EventService | 100% |
| EventStateService | 100% |
| EventList | 90% |
| EventDetail | 90% |
| EventForm | 100% (validation) |
| All components | Minimum 80% |

### 11.2 E2E Tests (Playwright)
| Scenario | Priority |
|----------|----------|
| Create new event | High |
| Edit existing event | High |
| Cancel event | High |
| Filter events | Medium |
| Search events | Medium |
| Pagination | Medium |
| Add/edit notes | Medium |
| Status transitions | High |

### 11.3 Test File Naming
```
event-list.spec.ts      // Unit test
event-list.e2e.ts       // E2E test
```

---

## 12. Performance Requirements

| Metric | Requirement |
|--------|-------------|
| First Contentful Paint | < 1.5s |
| Time to Interactive | < 3s |
| Bundle Size | < 200KB (initial) |
| Event List Render | < 100ms for 50 items |
| Navigation | < 200ms between pages |

### 12.1 Optimization Strategies
- Lazy loading of routes
- Virtual scrolling for large lists
- OnPush change detection
- Pagination (no infinite scroll)
- Image lazy loading

---

## 13. Internationalization

### 13.1 i18n Support
- Use Angular i18n for translations
- Date formatting via DatePipe with locale
- Number formatting via DecimalPipe

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
