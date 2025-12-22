# Frontend Phase B - Enhanced Features Specification

## Document Information
| Field | Value |
|-------|-------|
| Phase | B (Enhanced) |
| Version | 1.0 |
| Date | 2025-12-22 |
| Status | Draft |

---

## Overview

This document defines Phase B frontend requirements for the Event Management Platform. Phase B builds upon the MVP foundation and adds significant functionality including search/filtering, calendar views, photo uploads, and additional modules. Some advanced AI-powered features and analytics dashboards remain **OUT OF SCOPE** for Phase C.

### Technology Stack Additions (Phase B)
- **@angular/cdk/drag-drop**: Drag and drop for file uploads
- **ngx-lightbox**: Photo gallery lightbox
- **FullCalendar**: Calendar views
- **SignalR Client**: Real-time notifications
- **ngx-charts**: Basic charts

### Still Deferred for Phase C
- AI-powered features UI
- Advanced analytics dashboards
- Complex reporting interfaces
- Audit log viewers

---

## Phase B Requirements by Feature

---

## 1. Identity Module (Frontend)

### In Scope (Phase B)

| Requirement ID | Description | Status |
|----------------|-------------|--------|
| REQ-FE-AUTH-001 to REQ-FE-AUTH-011 | (From Phase A: Login, Register, Tokens, Guards, Interceptors) | IN SCOPE |
| REQ-FE-AUTHZ-001 to REQ-FE-AUTHZ-002 | (From Phase A: User Model with Roles, Role-Based UI) | IN SCOPE |
| REQ-FE-AUTH-009 | Remember Me Checkbox | IN SCOPE |
| REQ-FE-AUTH-012 | Forgot Password Flow | IN SCOPE |
| REQ-FE-AUTH-013 | Password Reset Page | IN SCOPE |
| REQ-FE-USER-001 | User Profile Page (GET /api/identity/profile) | IN SCOPE |
| REQ-FE-USER-002 | Edit Profile Form (PUT /api/identity/profile) | IN SCOPE |
| REQ-FE-USER-003 | Avatar Upload | IN SCOPE |
| REQ-FE-USER-004 | Change Password Form | IN SCOPE |
| REQ-FE-USER-005 | User Preferences | IN SCOPE |
| REQ-FE-SESSION-001 | Active Sessions List | IN SCOPE |
| REQ-FE-SESSION-002 | Revoke Session | IN SCOPE |
| REQ-FE-INVITE-001 | User Invitation Form | IN SCOPE |
| REQ-FE-INVITE-002 | Invitation Acceptance Page | IN SCOPE |

### Out of Scope (Deferred to Phase C)

| Requirement ID | Description | Target Phase |
|----------------|-------------|--------------|
| REQ-FE-AUTH-015 | Multi-Factor Authentication UI | Phase C |
| REQ-FE-AUTH-016 | OAuth/Social Login Buttons | Phase C |
| REQ-FE-AUDIT-001 | Login History View | Phase C |

---

## 2. Event Management Module (Frontend)

### In Scope (Phase B)

| Requirement ID | Description | Status |
|----------------|-------------|--------|
| REQ-FE-EVT-001 to REQ-FE-EVT-060 | (From Phase A) | IN SCOPE |
| REQ-FE-EVT-004 | Event List Filtering | IN SCOPE |
| REQ-FE-EVT-005 | Event Search | IN SCOPE |
| REQ-FE-EVT-006 | Event Calendar View | IN SCOPE |
| REQ-FE-EVT-007 | Event Card Grid View | IN SCOPE |
| REQ-FE-EVT-012 | Event Notes Tab | IN SCOPE |
| REQ-FE-EVT-013 | Event Staff Tab | IN SCOPE |
| REQ-FE-EVT-014 | Event Equipment Tab | IN SCOPE |
| REQ-FE-EVT-025 | Event Status Workflow | IN SCOPE |
| REQ-FE-EVT-026 | Venue Availability Check | IN SCOPE |
| REQ-FE-EVT-027 | Double Booking Warning | IN SCOPE |
| REQ-FE-EVT-028 | Double Booking Override Dialog | IN SCOPE |
| REQ-FE-EVT-035 | Edit Event Status | IN SCOPE |
| REQ-FE-EVT-036 | Event Cancellation Dialog | IN SCOPE |
| REQ-FE-EVT-070 | Event Calendar Component | IN SCOPE |
| REQ-FE-EVT-071 | Event Filter Panel | IN SCOPE |
| REQ-FE-EVT-072 | Event Status Workflow Stepper | IN SCOPE |

### Out of Scope (Deferred to Phase C)

| Requirement ID | Description | Target Phase |
|----------------|-------------|--------------|
| REQ-FE-EVT-015 | Event Timeline Tab | Phase C |
| REQ-FE-EVT-080 | AI Event Recommendations | Phase C |
| REQ-FE-EVT-081 | AI Optimization Suggestions | Phase C |
| REQ-FE-EVT-090 | Event Analytics Dashboard | Phase C |

---

## 3. Customer Management Module (Frontend)

### In Scope (Phase B)

| Requirement ID | Description | Status |
|----------------|-------------|--------|
| REQ-FE-CUS-001 to REQ-FE-CUS-050 | (From Phase A) | IN SCOPE |
| REQ-FE-CUS-004 | Customer List Filtering | IN SCOPE |
| REQ-FE-CUS-005 | Customer Search | IN SCOPE |
| REQ-FE-CUS-006 | Customer Card View | IN SCOPE |
| REQ-FE-CUS-014 | Customer Events History Tab | IN SCOPE |
| REQ-FE-CUS-015 | Customer Communication Tab | IN SCOPE |
| REQ-FE-CUS-016 | Customer Photo Upload | IN SCOPE |
| REQ-FE-CUS-017 | Customer Tags Management | IN SCOPE |
| REQ-FE-CUS-025 | Multiple Addresses Editor | IN SCOPE |
| REQ-FE-CUS-060 | Customer Filter Panel | IN SCOPE |

### Out of Scope (Deferred to Phase C)

| Requirement ID | Description | Target Phase |
|----------------|-------------|--------------|
| REQ-FE-CUS-035 | Customer Complaint Management | Phase C |
| REQ-FE-CUS-036 | Customer Testimonials | Phase C |
| REQ-FE-CUS-070 | Customer Sentiment Display | Phase C |
| REQ-FE-CUS-080 | Customer Analytics Dashboard | Phase C |

---

## 4. Venue Management Module (Frontend)

### In Scope (Phase B)

| Requirement ID | Description | Status |
|----------------|-------------|--------|
| REQ-FE-VEN-001 to REQ-FE-VEN-050 | (From Phase A) | IN SCOPE |
| REQ-FE-VEN-004 | Venue List Filtering | IN SCOPE |
| REQ-FE-VEN-005 | Venue Search | IN SCOPE |
| REQ-FE-VEN-006 | Venue Card View | IN SCOPE |
| REQ-FE-VEN-015 | Venue Amenities Editor | IN SCOPE |
| REQ-FE-VEN-016 | Venue Photo Gallery | IN SCOPE |
| REQ-FE-VEN-017 | Venue Events Calendar | IN SCOPE |
| REQ-FE-VEN-018 | Venue Availability Calendar | IN SCOPE |
| REQ-FE-VEN-060 | Venue Filter Panel | IN SCOPE |
| REQ-FE-VEN-061 | Photo Upload Dialog | IN SCOPE |
| REQ-FE-VEN-062 | Amenity Selector Component | IN SCOPE |

### Out of Scope (Deferred to Phase C)

| Requirement ID | Description | Target Phase |
|----------------|-------------|--------------|
| REQ-FE-VEN-007 | Venue Map View | Phase C |
| REQ-FE-VEN-025 | Venue Ratings Display | Phase C |
| REQ-FE-VEN-026 | Venue Issue Management | Phase C |
| REQ-FE-VEN-080 | Venue Analytics Dashboard | Phase C |

---

## 5. Staff Management Module (Frontend)

### In Scope (Phase B)

| Requirement ID | Description | Status |
|----------------|-------------|--------|
| REQ-FE-STF-001 to REQ-FE-STF-050 | (From Phase A) | IN SCOPE |
| REQ-FE-STF-004 | Staff List Filtering | IN SCOPE |
| REQ-FE-STF-005 | Staff Search | IN SCOPE |
| REQ-FE-STF-006 | Staff Card View | IN SCOPE |
| REQ-FE-STF-014 | Staff Photo Upload | IN SCOPE |
| REQ-FE-STF-015 | Staff Availability Calendar | IN SCOPE |
| REQ-FE-STF-016 | Staff Event Assignments Tab | IN SCOPE |
| REQ-FE-STF-017 | Staff Shift Scheduler | IN SCOPE |
| REQ-FE-STF-018 | Staff Check-in Interface | IN SCOPE |
| REQ-FE-STF-060 | Staff Filter Panel | IN SCOPE |
| REQ-FE-STF-061 | Availability Calendar Component | IN SCOPE |
| REQ-FE-STF-062 | Shift Assignment Dialog | IN SCOPE |

### Out of Scope (Deferred to Phase C)

| Requirement ID | Description | Target Phase |
|----------------|-------------|--------------|
| REQ-FE-STF-025 | Staff Performance Dashboard | Phase C |
| REQ-FE-STF-080 | AI Scheduling Suggestions | Phase C |
| REQ-FE-STF-085 | Staff Analytics Dashboard | Phase C |

---

## 6. Equipment Management Module (Frontend)

### In Scope (Phase B)

| Requirement ID | Description | Status |
|----------------|-------------|--------|
| REQ-FE-EQP-001 to REQ-FE-EQP-080 | (From Phase A) | IN SCOPE |
| REQ-FE-EQP-002 | Search Equipment | IN SCOPE |
| REQ-FE-EQP-003 | Filter Equipment | IN SCOPE |
| REQ-FE-EQP-006 | Toggle Equipment View | IN SCOPE |
| REQ-FE-EQP-007 | Display Availability Indicator | IN SCOPE |
| REQ-FE-EQP-011 | Equipment Detail Tabs | IN SCOPE |
| REQ-FE-EQP-012 | Display Reservations Calendar | IN SCOPE |
| REQ-FE-EQP-013 | Display Logistics Status | IN SCOPE |
| REQ-FE-EQP-014 | Display Maintenance History | IN SCOPE |
| REQ-FE-EQP-015 | Display Damage Reports | IN SCOPE |
| REQ-FE-EQP-016 | Equipment Detail Action Buttons | IN SCOPE |
| REQ-FE-EQP-024 | Inline Specifications Editor | IN SCOPE |
| REQ-FE-EQP-025 | Photo Upload with Drag and Drop | IN SCOPE |
| REQ-FE-EQP-030 | Create Reservation Form | IN SCOPE |
| REQ-FE-EQP-031 | Display Availability Calendar | IN SCOPE |
| REQ-FE-EQP-032 | Handle Booking Conflicts | IN SCOPE |
| REQ-FE-EQP-033 | Update Reservation | IN SCOPE |
| REQ-FE-EQP-034 | Cancel Reservation | IN SCOPE |
| REQ-FE-EQP-035 | Display Alternative Equipment | IN SCOPE |
| REQ-FE-EQP-040 | Display Logistics Tracker | IN SCOPE |
| REQ-FE-EQP-041 | Logistics Timeline Component | IN SCOPE |
| REQ-FE-EQP-042 | Update Logistics Status | IN SCOPE |
| REQ-FE-EQP-050 | Display Maintenance Schedule | IN SCOPE |
| REQ-FE-EQP-051 | Schedule Maintenance Form | IN SCOPE |
| REQ-FE-EQP-052 | Start Maintenance | IN SCOPE |
| REQ-FE-EQP-053 | Complete Maintenance | IN SCOPE |
| REQ-FE-EQP-054 | Display Overdue Maintenance Alerts | IN SCOPE |
| REQ-FE-EQP-060 | Report Damage Dialog | IN SCOPE |
| REQ-FE-EQP-061 | Display Damage Photo Gallery | IN SCOPE |
| REQ-FE-EQP-073 | Photo Gallery Component | IN SCOPE |
| REQ-FE-EQP-074 | Availability Checker Component | IN SCOPE |
| REQ-FE-EQP-075 | Reservation Calendar Component | IN SCOPE |
| REQ-FE-EQP-076 | Filter Panel Component | IN SCOPE |
| REQ-FE-EQP-081 | Reservation Service | IN SCOPE |
| REQ-FE-EQP-082 | Logistics Service | IN SCOPE |
| REQ-FE-EQP-083 | Maintenance Service | IN SCOPE |
| REQ-FE-EQP-084 | Equipment State Service | IN SCOPE |
| REQ-FE-EQP-090-092 | Validation Requirements | IN SCOPE |
| REQ-FE-EQP-100-102 | Authorization Requirements | IN SCOPE |
| REQ-FE-EQP-110-113 | Accessibility Requirements | IN SCOPE |
| REQ-FE-EQP-120-124 | Performance Requirements | IN SCOPE |
| REQ-FE-EQP-130-134 | Testing Requirements | IN SCOPE |
| REQ-FE-EQP-140-144 | Error Handling Requirements | IN SCOPE |

### Out of Scope (Deferred to Phase C)

| Requirement ID | Description | Target Phase |
|----------------|-------------|--------------|
| REQ-FE-EQP-036 | AI Equipment Suggestions | Phase C |
| REQ-FE-EQP-055 | Predictive Maintenance Alerts | Phase C |
| REQ-FE-EQP-062 | AI Damage Detection Display | Phase C |
| REQ-FE-EQP-090 | Equipment Analytics Dashboard | Phase C |

---

## 7. Invoice Management Module (Frontend - NEW)

### In Scope (Phase B)

| Requirement ID | Description | Status |
|----------------|-------------|--------|
| REQ-FE-INV-001 | Invoice List Page | IN SCOPE |
| REQ-FE-INV-002 | Invoice List Table with Pagination | IN SCOPE |
| REQ-FE-INV-003 | Invoice List Filtering | IN SCOPE |
| REQ-FE-INV-004 | Invoice Search | IN SCOPE |
| REQ-FE-INV-010 | Invoice Detail Page | IN SCOPE |
| REQ-FE-INV-011 | Invoice Header Display | IN SCOPE |
| REQ-FE-INV-012 | Invoice Line Items Table | IN SCOPE |
| REQ-FE-INV-013 | Invoice Totals Display | IN SCOPE |
| REQ-FE-INV-014 | Payment History Display | IN SCOPE |
| REQ-FE-INV-020 | Create Invoice Page | IN SCOPE |
| REQ-FE-INV-021 | Invoice Form | IN SCOPE |
| REQ-FE-INV-022 | Line Item Editor | IN SCOPE |
| REQ-FE-INV-023 | Customer Selection | IN SCOPE |
| REQ-FE-INV-024 | Event Selection | IN SCOPE |
| REQ-FE-INV-030 | Edit Invoice Page | IN SCOPE |
| REQ-FE-INV-035 | Finalize Invoice Confirmation | IN SCOPE |
| REQ-FE-INV-036 | Void Invoice Confirmation | IN SCOPE |
| REQ-FE-INV-040 | Record Payment Dialog | IN SCOPE |
| REQ-FE-INV-041 | Partial Payment Support | IN SCOPE |
| REQ-FE-INV-050 | Download PDF Button | IN SCOPE |
| REQ-FE-INV-055 | Invoice Status Badge | IN SCOPE |
| REQ-FE-INV-060 | Invoice Service | IN SCOPE |

### Out of Scope (Deferred to Phase C)

| Requirement ID | Description | Target Phase |
|----------------|-------------|--------------|
| REQ-FE-INV-070 | Refund Management UI | Phase C |
| REQ-FE-INV-075 | Credit Note UI | Phase C |
| REQ-FE-INV-080 | Financial Dashboard | Phase C |
| REQ-FE-INV-085 | AI Anomaly Alerts | Phase C |

---

## 8. Notification Center Module (Frontend - NEW)

### In Scope (Phase B)

| Requirement ID | Description | Status |
|----------------|-------------|--------|
| REQ-FE-NTF-001 | Notification Bell Icon | IN SCOPE |
| REQ-FE-NTF-002 | Unread Count Badge | IN SCOPE |
| REQ-FE-NTF-003 | Notification Dropdown Panel | IN SCOPE |
| REQ-FE-NTF-004 | Notification List | IN SCOPE |
| REQ-FE-NTF-005 | Notification Item Component | IN SCOPE |
| REQ-FE-NTF-006 | Mark as Read | IN SCOPE |
| REQ-FE-NTF-007 | Mark All as Read | IN SCOPE |
| REQ-FE-NTF-008 | Delete Notification | IN SCOPE |
| REQ-FE-NTF-010 | Notification Page (Full List) | IN SCOPE |
| REQ-FE-NTF-011 | Notification Filtering | IN SCOPE |
| REQ-FE-NTF-015 | Real-time Notification Updates | IN SCOPE |
| REQ-FE-NTF-016 | Toast Notifications | IN SCOPE |
| REQ-FE-NTF-020 | Notification Preferences Page | IN SCOPE |
| REQ-FE-NTF-021 | Email Preference Toggle | IN SCOPE |
| REQ-FE-NTF-022 | Category Preferences | IN SCOPE |
| REQ-FE-NTF-030 | Notification Service | IN SCOPE |
| REQ-FE-NTF-031 | SignalR Connection | IN SCOPE |

### Out of Scope (Deferred to Phase C)

| Requirement ID | Description | Target Phase |
|----------------|-------------|--------------|
| REQ-FE-NTF-023 | SMS Preference Toggle | Phase C |
| REQ-FE-NTF-024 | Push Preference Toggle | Phase C |
| REQ-FE-NTF-040 | Notification Analytics | Phase C |

---

## 9. Scheduling Module (Frontend - NEW)

### In Scope (Phase B)

| Requirement ID | Description | Status |
|----------------|-------------|--------|
| REQ-FE-SCH-001 | Schedule Calendar Page | IN SCOPE |
| REQ-FE-SCH-002 | Month View | IN SCOPE |
| REQ-FE-SCH-003 | Week View | IN SCOPE |
| REQ-FE-SCH-004 | Day View | IN SCOPE |
| REQ-FE-SCH-005 | Resource View (Staff/Equipment) | IN SCOPE |
| REQ-FE-SCH-010 | Create Schedule Entry Dialog | IN SCOPE |
| REQ-FE-SCH-011 | Edit Schedule Entry | IN SCOPE |
| REQ-FE-SCH-012 | Delete Schedule Entry | IN SCOPE |
| REQ-FE-SCH-015 | Drag and Drop Scheduling | IN SCOPE |
| REQ-FE-SCH-016 | Resize Schedule Entry | IN SCOPE |
| REQ-FE-SCH-020 | Conflict Indicator | IN SCOPE |
| REQ-FE-SCH-021 | Conflict Resolution Dialog | IN SCOPE |
| REQ-FE-SCH-025 | Filter by Resource Type | IN SCOPE |
| REQ-FE-SCH-026 | Filter by Date Range | IN SCOPE |
| REQ-FE-SCH-030 | Schedule Service | IN SCOPE |
| REQ-FE-SCH-035 | FullCalendar Integration | IN SCOPE |

### Out of Scope (Deferred to Phase C)

| Requirement ID | Description | Target Phase |
|----------------|-------------|--------------|
| REQ-FE-SCH-040 | AI Scheduling Suggestions | Phase C |
| REQ-FE-SCH-045 | Schedule Analytics | Phase C |
| REQ-FE-SCH-050 | Print Schedule | Phase C |

---

## Page Structure (Phase B Additions)

### New Pages
```
src/app/pages/
├── invoices/
│   ├── invoice-list/         # Invoice list page
│   ├── invoice-detail/       # Invoice detail page
│   ├── invoice-create/       # Create invoice page
│   └── invoice-edit/         # Edit invoice page
├── notifications/
│   ├── notification-list/    # Full notification list
│   └── notification-prefs/   # Notification preferences
├── schedule/
│   └── schedule-calendar/    # Full calendar view
├── auth/
│   ├── forgot-password/      # Forgot password page
│   └── reset-password/       # Reset password page
└── invitations/
    └── accept-invitation/    # Accept invitation page
```

### New Components
```
src/app/components/
├── notifications/
│   ├── notification-bell/       # Header notification bell
│   ├── notification-dropdown/   # Notification panel
│   └── notification-item/       # Single notification
├── scheduling/
│   ├── calendar-view/           # Calendar component
│   ├── schedule-entry-dialog/   # Create/edit dialog
│   └── conflict-dialog/         # Conflict resolution
├── invoices/
│   ├── invoice-form/            # Invoice form
│   ├── line-item-editor/        # Line items table
│   ├── payment-dialog/          # Record payment
│   └── invoice-status-badge/    # Status chip
├── shared/
│   ├── photo-uploader/          # Drag and drop photo upload
│   ├── photo-gallery/           # Photo gallery with lightbox
│   ├── filter-panel/            # Reusable filter panel
│   ├── search-input/            # Debounced search input
│   └── calendar-picker/         # Date range picker
└── user/
    ├── avatar-upload/           # Profile photo upload
    ├── sessions-list/           # Active sessions
    └── preferences-form/        # User preferences
```

---

## Shared Components (Phase B Additions)

| Component | Description |
|-----------|-------------|
| PhotoUploader | Drag and drop photo upload with preview |
| PhotoGallery | Photo grid with lightbox |
| FilterPanel | Collapsible filter panel |
| SearchInput | Debounced search input |
| CalendarPicker | Date range picker |
| CalendarView | FullCalendar wrapper |
| TagEditor | Multi-select tag editor |
| AvatarUpload | Circular avatar upload |
| ViewToggle | Grid/Table/List view toggle |
| ConflictDialog | Conflict resolution dialog |

---

## Services (Phase B Additions)

| Service | Description |
|---------|-------------|
| InvoiceService | Invoice CRUD operations |
| NotificationService | Notification operations |
| SignalRService | Real-time connection |
| ScheduleService | Schedule operations |
| SessionService | Session management |
| InvitationService | Invitation operations |
| FileUploadService | Photo upload to Azure Blob |

---

## Routing Configuration (Phase B Additions)

```typescript
// Additional routes for Phase B
{
    path: 'invoices',
    loadChildren: () => import('./pages/invoices/routes'),
    canActivate: [authGuard]
},
{
    path: 'notifications',
    loadChildren: () => import('./pages/notifications/routes'),
    canActivate: [authGuard]
},
{
    path: 'schedule',
    loadChildren: () => import('./pages/schedule/routes'),
    canActivate: [authGuard]
},
{
    path: 'forgot-password',
    loadComponent: () => import('./pages/auth/forgot-password')
},
{
    path: 'reset-password',
    loadComponent: () => import('./pages/auth/reset-password')
},
{
    path: 'accept-invitation/:token',
    loadComponent: () => import('./pages/invitations/accept-invitation')
}
```

---

## Modules NOT Included in Phase B

The following frontend modules remain out of scope for Phase B:

| Module | Target Phase |
|--------|--------------|
| Shipper Management UI | Phase C |
| Prize Management UI | Phase C |
| Guest Invitation UI | Phase C |
| Reporting Dashboard | Phase C |
| Analytics Charts | Phase C |
| Audit Logs Viewer | Phase C |
| AI Suggestions UI | Phase C |

---

## Non-Functional Requirements (Phase B)

### Performance
- First Contentful Paint < 1.8 seconds
- Time to Interactive < 3.5 seconds
- Initial bundle size < 350KB (gzipped)
- Calendar renders 100+ events smoothly

### Accessibility
- WCAG 2.1 Level AA compliance
- Full keyboard navigation
- Screen reader support for all components
- Focus management in dialogs

### Browser Support
- Chrome (latest 2 versions)
- Firefox (latest 2 versions)
- Safari (latest 2 versions)
- Edge (latest 2 versions)

### Real-time Features
- SignalR connection with auto-reconnect
- Notification updates < 500ms latency
- Graceful offline handling

### Testing
- Unit test coverage minimum 65%
- E2E tests for all critical flows
- Visual regression tests for key components

---

## Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-22 | System | Initial Phase B specification |
