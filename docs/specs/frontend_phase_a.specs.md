# Frontend Phase A - MVP Specification

## Document Information
| Field | Value |
|-------|-------|
| Phase | A (MVP) |
| Version | 1.0 |
| Date | 2025-12-22 |
| Status | Draft |

---

## Overview

This document defines the Minimum Viable Product (MVP) frontend requirements for the Event Management Platform. Phase A focuses on core UI functionality needed to manage basic event operations. Many advanced features are marked as **OUT OF SCOPE** and will be implemented in subsequent phases.

### Technology Stack (MVP)
- **Framework**: Angular 18+ (latest stable)
- **UI Library**: Angular Material (Material 3)
- **State Management**: RxJS (no NgRx)
- **Styling**: SCSS with BEM methodology
- **Testing**: Jest (unit), Playwright (e2e)

### Deferred for Later Phases
- Photo upload components
- Advanced calendar views
- Real-time updates (WebSocket/SignalR)
- AI-powered features
- Advanced analytics dashboards
- Complex wizards and workflows

---

## Phase A Requirements by Feature

---

## 1. Identity Module (Frontend)

### In Scope

| Requirement ID | Description | Status |
|----------------|-------------|--------|
| REQ-FE-AUTH-001 | Login Page (POST /api/identity/authenticate) | IN SCOPE |
| REQ-FE-AUTH-002 | Login Form with Validation (username, password required) | IN SCOPE |
| REQ-FE-AUTH-003 | Token Storage (localStorage: accessToken, refreshToken, currentUser) | IN SCOPE |
| REQ-FE-AUTH-004 | Auth Guard for Protected Routes | IN SCOPE |
| REQ-FE-AUTH-005 | Logout Functionality | IN SCOPE |
| REQ-FE-AUTH-006 | JWT Token Refresh Flow (POST /api/identity/refresh-token) | IN SCOPE |
| REQ-FE-AUTH-007 | HTTP Headers Interceptor (Bearer token injection) | IN SCOPE |
| REQ-FE-AUTH-008 | JWT Error Interceptor (401 handling, auto-refresh) | IN SCOPE |
| REQ-FE-AUTH-010 | Registration Page (POST /api/identity/register) | IN SCOPE |
| REQ-FE-AUTH-011 | Registration Form with Validation (username 3-100 chars, password min 6, confirmPassword match) | IN SCOPE |
| REQ-FE-AUTHZ-001 | User Model with Roles | IN SCOPE |
| REQ-FE-AUTHZ-002 | Role-Based UI Rendering | IN SCOPE |
| REQ-FE-LAYOUT-001 | Main App Layout with Navigation | IN SCOPE |
| REQ-FE-LAYOUT-002 | Responsive Header | IN SCOPE |
| REQ-FE-LAYOUT-003 | Side Navigation Menu | IN SCOPE |

### Out of Scope (Deferred)

| Requirement ID | Description | Target Phase |
|----------------|-------------|--------------|
| REQ-FE-AUTH-009 | Remember Me Checkbox | Phase B |
| REQ-FE-AUTH-012 | Forgot Password Flow | Phase B |
| REQ-FE-AUTH-013 | Password Reset Page | Phase B |
| REQ-FE-AUTH-015 | Multi-Factor Authentication UI | Phase C |
| REQ-FE-USER-001 | User Profile Page | Phase B |
| REQ-FE-USER-002 | Edit Profile Form | Phase B |
| REQ-FE-USER-003 | Avatar Upload | Phase B |
| REQ-FE-USER-004 | Change Password Form | Phase B |
| REQ-FE-USER-005 | User Preferences | Phase B |

---

## 2. Event Management Module (Frontend)

### In Scope

| Requirement ID | Description | Status |
|----------------|-------------|--------|
| REQ-FE-EVT-001 | Event List Page | IN SCOPE |
| REQ-FE-EVT-002 | Event List Table with Pagination | IN SCOPE |
| REQ-FE-EVT-003 | Event List Sorting | IN SCOPE |
| REQ-FE-EVT-010 | Event Detail Page | IN SCOPE |
| REQ-FE-EVT-011 | Event Information Display | IN SCOPE |
| REQ-FE-EVT-020 | Create Event Page | IN SCOPE |
| REQ-FE-EVT-021 | Event Form with Validation | IN SCOPE |
| REQ-FE-EVT-022 | Venue Selection Dropdown | IN SCOPE |
| REQ-FE-EVT-023 | Customer Selection Dropdown | IN SCOPE |
| REQ-FE-EVT-024 | Date/Time Pickers | IN SCOPE |
| REQ-FE-EVT-030 | Edit Event Page | IN SCOPE |
| REQ-FE-EVT-040 | Delete Event Confirmation Dialog | IN SCOPE |
| REQ-FE-EVT-050 | Event Status Badge Component | IN SCOPE |
| REQ-FE-EVT-060 | Event Service (API calls) | IN SCOPE |

### Out of Scope (Deferred)

| Requirement ID | Description | Target Phase |
|----------------|-------------|--------------|
| REQ-FE-EVT-004 | Event List Filtering | Phase B |
| REQ-FE-EVT-005 | Event Search | Phase B |
| REQ-FE-EVT-006 | Event Calendar View | Phase B |
| REQ-FE-EVT-007 | Event Card Grid View | Phase B |
| REQ-FE-EVT-012 | Event Notes Tab | Phase B |
| REQ-FE-EVT-013 | Event Staff Tab | Phase B |
| REQ-FE-EVT-014 | Event Equipment Tab | Phase B |
| REQ-FE-EVT-015 | Event Timeline Tab | Phase C |
| REQ-FE-EVT-025 | Event Status Workflow | Phase B |
| REQ-FE-EVT-026 | Venue Availability Check | Phase B |
| REQ-FE-EVT-027 | Double Booking Warning | Phase B |
| REQ-FE-EVT-035 | Edit Event Status | Phase B |
| REQ-FE-EVT-070-091 | Advanced Components | Phase B/C |

---

## 3. Customer Management Module (Frontend)

### In Scope

| Requirement ID | Description | Status |
|----------------|-------------|--------|
| REQ-FE-CUS-001 | Customer List Page | IN SCOPE |
| REQ-FE-CUS-002 | Customer List Table with Pagination | IN SCOPE |
| REQ-FE-CUS-003 | Customer List Sorting | IN SCOPE |
| REQ-FE-CUS-010 | Customer Detail Page | IN SCOPE |
| REQ-FE-CUS-011 | Customer Information Display | IN SCOPE |
| REQ-FE-CUS-012 | Customer Contact Display | IN SCOPE |
| REQ-FE-CUS-013 | Customer Address Display | IN SCOPE |
| REQ-FE-CUS-020 | Create Customer Page | IN SCOPE |
| REQ-FE-CUS-021 | Customer Form with Validation | IN SCOPE |
| REQ-FE-CUS-030 | Edit Customer Page | IN SCOPE |
| REQ-FE-CUS-040 | Delete Customer Confirmation Dialog | IN SCOPE |
| REQ-FE-CUS-050 | Customer Service (API calls) | IN SCOPE |

### Out of Scope (Deferred)

| Requirement ID | Description | Target Phase |
|----------------|-------------|--------------|
| REQ-FE-CUS-004 | Customer List Filtering | Phase B |
| REQ-FE-CUS-005 | Customer Search | Phase B |
| REQ-FE-CUS-006 | Customer Card View | Phase B |
| REQ-FE-CUS-014 | Customer Events History Tab | Phase B |
| REQ-FE-CUS-015 | Customer Communication Tab | Phase B |
| REQ-FE-CUS-016 | Customer Photo Upload | Phase B |
| REQ-FE-CUS-017 | Customer Tags Management | Phase B |
| REQ-FE-CUS-025 | Multiple Addresses Editor | Phase B |
| REQ-FE-CUS-035 | Customer Complaint Management | Phase C |
| REQ-FE-CUS-036 | Customer Testimonials | Phase C |

---

## 4. Venue Management Module (Frontend)

### In Scope

| Requirement ID | Description | Status |
|----------------|-------------|--------|
| REQ-FE-VEN-001 | Venue List Page | IN SCOPE |
| REQ-FE-VEN-002 | Venue List Table with Pagination | IN SCOPE |
| REQ-FE-VEN-003 | Venue List Sorting | IN SCOPE |
| REQ-FE-VEN-010 | Venue Detail Page | IN SCOPE |
| REQ-FE-VEN-011 | Venue Information Display | IN SCOPE |
| REQ-FE-VEN-012 | Venue Contact Display | IN SCOPE |
| REQ-FE-VEN-013 | Venue Address Display | IN SCOPE |
| REQ-FE-VEN-014 | Venue Capacity Display | IN SCOPE |
| REQ-FE-VEN-020 | Create Venue Page | IN SCOPE |
| REQ-FE-VEN-021 | Venue Form with Validation | IN SCOPE |
| REQ-FE-VEN-030 | Edit Venue Page | IN SCOPE |
| REQ-FE-VEN-040 | Delete Venue Confirmation Dialog | IN SCOPE |
| REQ-FE-VEN-050 | Venue Service (API calls) | IN SCOPE |

### Out of Scope (Deferred)

| Requirement ID | Description | Target Phase |
|----------------|-------------|--------------|
| REQ-FE-VEN-004 | Venue List Filtering | Phase B |
| REQ-FE-VEN-005 | Venue Search | Phase B |
| REQ-FE-VEN-006 | Venue Card View | Phase B |
| REQ-FE-VEN-007 | Venue Map View | Phase C |
| REQ-FE-VEN-015 | Venue Amenities Editor | Phase B |
| REQ-FE-VEN-016 | Venue Photo Gallery | Phase B |
| REQ-FE-VEN-017 | Venue Events Calendar | Phase B |
| REQ-FE-VEN-018 | Venue Availability Calendar | Phase B |
| REQ-FE-VEN-025 | Venue Ratings Display | Phase C |
| REQ-FE-VEN-026 | Venue Issue Management | Phase C |

---

## 5. Staff Management Module (Frontend)

### In Scope

| Requirement ID | Description | Status |
|----------------|-------------|--------|
| REQ-FE-STF-001 | Staff List Page | IN SCOPE |
| REQ-FE-STF-002 | Staff List Table with Pagination | IN SCOPE |
| REQ-FE-STF-003 | Staff List Sorting | IN SCOPE |
| REQ-FE-STF-010 | Staff Detail Page | IN SCOPE |
| REQ-FE-STF-011 | Staff Information Display | IN SCOPE |
| REQ-FE-STF-012 | Staff Contact Display | IN SCOPE |
| REQ-FE-STF-013 | Staff Role Display | IN SCOPE |
| REQ-FE-STF-020 | Create Staff Page | IN SCOPE |
| REQ-FE-STF-021 | Staff Form with Validation | IN SCOPE |
| REQ-FE-STF-030 | Edit Staff Page | IN SCOPE |
| REQ-FE-STF-040 | Delete Staff Confirmation Dialog | IN SCOPE |
| REQ-FE-STF-050 | Staff Service (API calls) | IN SCOPE |

### Out of Scope (Deferred)

| Requirement ID | Description | Target Phase |
|----------------|-------------|--------------|
| REQ-FE-STF-004 | Staff List Filtering | Phase B |
| REQ-FE-STF-005 | Staff Search | Phase B |
| REQ-FE-STF-006 | Staff Card View | Phase B |
| REQ-FE-STF-014 | Staff Photo Upload | Phase B |
| REQ-FE-STF-015 | Staff Availability Calendar | Phase B |
| REQ-FE-STF-016 | Staff Event Assignments Tab | Phase B |
| REQ-FE-STF-017 | Staff Shift Scheduler | Phase B |
| REQ-FE-STF-018 | Staff Check-in Interface | Phase B |
| REQ-FE-STF-025 | Staff Performance Dashboard | Phase C |

---

## 6. Equipment Management Module (Frontend)

### In Scope

| Requirement ID | Description | Status |
|----------------|-------------|--------|
| REQ-FE-EQP-001 | Display Equipment List | IN SCOPE |
| REQ-FE-EQP-004 | Sort Equipment List | IN SCOPE |
| REQ-FE-EQP-005 | Category Tab Navigation | IN SCOPE |
| REQ-FE-EQP-008 | Navigate to Equipment Detail | IN SCOPE |
| REQ-FE-EQP-010 | Display Equipment Details | IN SCOPE |
| REQ-FE-EQP-020 | Create Equipment Form | IN SCOPE |
| REQ-FE-EQP-021 | Edit Equipment Form | IN SCOPE |
| REQ-FE-EQP-022 | Validate Equipment Name | IN SCOPE |
| REQ-FE-EQP-023 | Validate Purchase Information | IN SCOPE |
| REQ-FE-EQP-070 | Equipment Card Component | IN SCOPE |
| REQ-FE-EQP-071 | Status Badge Component | IN SCOPE |
| REQ-FE-EQP-072 | Condition Badge Component | IN SCOPE |
| REQ-FE-EQP-080 | Equipment Service | IN SCOPE |

### Out of Scope (Deferred)

| Requirement ID | Description | Target Phase |
|----------------|-------------|--------------|
| REQ-FE-EQP-002 | Search Equipment | Phase B |
| REQ-FE-EQP-003 | Filter Equipment | Phase B |
| REQ-FE-EQP-006 | Toggle Equipment View | Phase B |
| REQ-FE-EQP-007 | Display Availability Indicator | Phase B |
| REQ-FE-EQP-011 | Equipment Detail Tabs | Phase B |
| REQ-FE-EQP-012 | Display Reservations Calendar | Phase B |
| REQ-FE-EQP-013 | Display Logistics Status | Phase B |
| REQ-FE-EQP-014 | Display Maintenance History | Phase B |
| REQ-FE-EQP-015 | Display Damage Reports | Phase B |
| REQ-FE-EQP-016 | Equipment Detail Action Buttons | Phase B |
| REQ-FE-EQP-024 | Inline Specifications Editor | Phase B |
| REQ-FE-EQP-025 | Photo Upload with Drag and Drop | Phase B |
| REQ-FE-EQP-030-035 | Equipment Booking | Phase B |
| REQ-FE-EQP-040-042 | Equipment Logistics | Phase B |
| REQ-FE-EQP-050-054 | Equipment Maintenance | Phase B |
| REQ-FE-EQP-060-061 | Equipment Damage Reporting | Phase B |
| REQ-FE-EQP-073-076 | Advanced Components | Phase B |

---

## Page Structure (Phase A)

### Layout Components
```
src/app/
├── layout/
│   ├── app-layout/           # Main layout wrapper
│   ├── header/               # Top navigation bar
│   └── sidenav/              # Side navigation menu
```

### Feature Pages
```
src/app/pages/
├── auth/
│   ├── login/                # Login page (POST /api/identity/authenticate)
│   └── register/             # Registration page (POST /api/identity/register)
├── events/
│   ├── event-list/           # Event list page
│   ├── event-detail/         # Event detail page
│   ├── event-create/         # Create event page
│   └── event-edit/           # Edit event page
├── customers/
│   ├── customer-list/        # Customer list page
│   ├── customer-detail/      # Customer detail page
│   ├── customer-create/      # Create customer page
│   └── customer-edit/        # Edit customer page
├── venues/
│   ├── venue-list/           # Venue list page
│   ├── venue-detail/         # Venue detail page
│   ├── venue-create/         # Create venue page
│   └── venue-edit/           # Edit venue page
├── staff/
│   ├── staff-list/           # Staff list page
│   ├── staff-detail/         # Staff detail page
│   ├── staff-create/         # Create staff page
│   └── staff-edit/           # Edit staff page
└── equipment/
    ├── equipment-list/       # Equipment list page
    ├── equipment-detail/     # Equipment detail page
    ├── equipment-create/     # Create equipment page
    └── equipment-edit/       # Edit equipment page
```

**Note:** User profile page is deferred to Phase B (depends on backend profile endpoints).

---

## Routing Configuration (Phase A)

```typescript
export const routes: Routes = [
    { path: '', redirectTo: '/events', pathMatch: 'full' },
    { path: 'login', loadComponent: () => import('./pages/auth/login') },
    { path: 'register', loadComponent: () => import('./pages/auth/register') },
    {
        path: '',
        canActivate: [authGuard],
        children: [
            { path: 'events', loadChildren: () => import('./pages/events/routes') },
            { path: 'customers', loadChildren: () => import('./pages/customers/routes') },
            { path: 'venues', loadChildren: () => import('./pages/venues/routes') },
            { path: 'staff', loadChildren: () => import('./pages/staff/routes') },
            { path: 'equipment', loadChildren: () => import('./pages/equipment/routes') }
            // Note: Profile route deferred to Phase B
        ]
    }
];
```

---

## Shared Components (Phase A)

| Component | Description |
|-----------|-------------|
| PageHeader | Reusable page header with title and action buttons |
| DataTable | Paginated, sortable table using mat-table |
| ConfirmDialog | Confirmation dialog for delete actions |
| LoadingSpinner | Full-page and inline loading indicators |
| FormField | Wrapped mat-form-field with error display |
| StatusBadge | Color-coded status chip |
| EmptyState | No data placeholder component |
| Breadcrumbs | Navigation breadcrumbs |

---

## Services (Phase A)

| Service | Description |
|---------|-------------|
| AuthService | Authentication, token management |
| EventService | Event CRUD operations |
| CustomerService | Customer CRUD operations |
| VenueService | Venue CRUD operations |
| StaffService | Staff CRUD operations |
| EquipmentService | Equipment CRUD operations |
| NotificationService | Snackbar notifications |
| ErrorHandlerService | Global error handling |

---

## Modules NOT Included in Phase A

The following frontend modules are entirely out of scope for Phase A MVP:

| Module | Target Phase |
|--------|--------------|
| Invoice Management UI | Phase B |
| Notification Center | Phase B |
| Scheduling UI | Phase B |
| Shipper Management UI | Phase B |
| Prize Management UI | Phase B |
| Invitation UI | Phase B |
| Reporting Dashboard | Phase C |
| Analytics Charts | Phase C |
| Audit Logs Viewer | Phase C |

---

## Non-Functional Requirements (Phase A)

### Performance
- First Contentful Paint < 2 seconds
- Time to Interactive < 4 seconds
- Initial bundle size < 300KB (gzipped)

### Accessibility
- WCAG 2.1 Level A compliance (minimum)
- Keyboard navigation for all interactive elements
- Form labels and ARIA attributes

### Browser Support
- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)

### Responsive Design
- Desktop (1200px+)
- Tablet (768px - 1199px)
- Mobile (320px - 767px)

### Testing
- Unit test coverage minimum 50%
- E2E tests for login and CRUD flows

---

## Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-22 | System | Initial MVP specification |
