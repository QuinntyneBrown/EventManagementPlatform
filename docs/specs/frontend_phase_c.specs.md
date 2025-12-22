# Frontend Phase C - Full Specification

## Document Information
| Field | Value |
|-------|-------|
| Phase | C (Full) |
| Version | 1.0 |
| Date | 2025-12-22 |
| Status | Draft |

---

## Overview

This document defines Phase C (Full) frontend requirements for the Event Management Platform. Phase C includes ALL requirements from all feature specifications. This is the complete feature set with no requirements out of scope.

### Complete Technology Stack
- **Framework**: Angular 18+ (latest stable)
- **UI Library**: Angular Material (Material 3)
- **State Management**: RxJS (no NgRx)
- **Styling**: SCSS with BEM methodology
- **Testing**: Jest (unit), Playwright (e2e)
- **Charts**: ngx-charts
- **Calendar**: FullCalendar
- **Maps**: Google Maps / Leaflet
- **Real-time**: SignalR Client
- **File Upload**: Angular CDK drag-drop
- **PDF**: PDF.js for viewing
- **Print**: ngx-print

---

## Complete Module Inventory

Phase C includes ALL requirements from the following frontend modules:

| Module | Status |
|--------|--------|
| Identity & Authentication UI | FULL |
| Event Management UI | FULL |
| Customer Management UI | FULL |
| Venue Management UI | FULL |
| Staff Management UI | FULL |
| Equipment Management UI | FULL |
| Invoice Management UI | FULL |
| Notification Center | FULL |
| Scheduling UI | FULL |
| Shipper Management UI | FULL |
| Prize Management UI | FULL |
| Invitation Management UI | FULL |
| Reporting Dashboard | FULL |
| Audit Log Viewer | FULL |
| Integration Management UI | FULL |

---

## 1. Identity Module (FULL)

### All Requirements In Scope

| Requirement ID | Description |
|----------------|-------------|
| REQ-FE-AUTH-001 | Login Page (POST /api/identity/authenticate) |
| REQ-FE-AUTH-002 | Login Form with Validation (username, password required) |
| REQ-FE-AUTH-003 | Token Storage (localStorage: accessToken, refreshToken, currentUser) |
| REQ-FE-AUTH-004 | Auth Guard for Protected Routes |
| REQ-FE-AUTH-005 | Logout Functionality |
| REQ-FE-AUTH-006 | JWT Token Refresh Flow (POST /api/identity/refresh-token) |
| REQ-FE-AUTH-007 | HTTP Headers Interceptor (Bearer token injection) |
| REQ-FE-AUTH-008 | JWT Error Interceptor (401 handling, auto-refresh) |
| REQ-FE-AUTH-009 | Remember Me Checkbox |
| REQ-FE-AUTH-010 | Registration Page (POST /api/identity/register) |
| REQ-FE-AUTH-011 | Registration Form with Validation |
| REQ-FE-AUTH-012 | Forgot Password Flow |
| REQ-FE-AUTH-013 | Password Reset Page |
| REQ-FE-AUTH-015 | Multi-Factor Authentication UI |
| REQ-FE-AUTH-016 | OAuth/Social Login Buttons |
| REQ-FE-AUTHZ-001 | User Model with Roles |
| REQ-FE-AUTHZ-002 | Role-Based UI Rendering |
| REQ-FE-USER-001 | User Profile Page (GET /api/identity/profile) |
| REQ-FE-USER-002 | Edit Profile Form (PUT /api/identity/profile) |
| REQ-FE-USER-003 | Avatar Upload |
| REQ-FE-USER-004 | Change Password Form |
| REQ-FE-USER-005 | User Preferences |
| REQ-FE-SESSION-001 | Active Sessions List |
| REQ-FE-SESSION-002 | Revoke Session |
| REQ-FE-INVITE-001 | User Invitation Form |
| REQ-FE-INVITE-002 | Invitation Acceptance Page |
| REQ-FE-AUDIT-001 | Login History View |
| REQ-FE-LAYOUT-001 | Main App Layout |
| REQ-FE-LAYOUT-002 | Responsive Header |
| REQ-FE-LAYOUT-003 | Side Navigation Menu |
| REQ-FE-LAYOUT-004 | Breadcrumbs |
| REQ-FE-LAYOUT-005 | Footer |

---

## 2. Event Management Module (FULL)

### All Requirements In Scope

| Requirement ID | Description |
|----------------|-------------|
| REQ-FE-EVT-001 | Event List Page |
| REQ-FE-EVT-002 | Event List Table with Pagination |
| REQ-FE-EVT-003 | Event List Sorting |
| REQ-FE-EVT-004 | Event List Filtering |
| REQ-FE-EVT-005 | Event Search |
| REQ-FE-EVT-006 | Event Calendar View |
| REQ-FE-EVT-007 | Event Card Grid View |
| REQ-FE-EVT-010 | Event Detail Page |
| REQ-FE-EVT-011 | Event Information Display |
| REQ-FE-EVT-012 | Event Notes Tab |
| REQ-FE-EVT-013 | Event Staff Tab |
| REQ-FE-EVT-014 | Event Equipment Tab |
| REQ-FE-EVT-015 | Event Timeline Tab |
| REQ-FE-EVT-016 | Event Prizes Tab |
| REQ-FE-EVT-017 | Event Invitations Tab |
| REQ-FE-EVT-018 | Event Invoice Tab |
| REQ-FE-EVT-020 | Create Event Page |
| REQ-FE-EVT-021 | Event Form with Validation |
| REQ-FE-EVT-022 | Venue Selection |
| REQ-FE-EVT-023 | Customer Selection |
| REQ-FE-EVT-024 | Date/Time Pickers |
| REQ-FE-EVT-025 | Event Status Workflow |
| REQ-FE-EVT-026 | Venue Availability Check |
| REQ-FE-EVT-027 | Double Booking Warning |
| REQ-FE-EVT-028 | Double Booking Override Dialog |
| REQ-FE-EVT-030 | Edit Event Page |
| REQ-FE-EVT-035 | Edit Event Status |
| REQ-FE-EVT-036 | Event Cancellation Dialog |
| REQ-FE-EVT-040 | Delete Event Confirmation |
| REQ-FE-EVT-050 | Event Status Badge |
| REQ-FE-EVT-060 | Event Service |
| REQ-FE-EVT-070 | Event Calendar Component |
| REQ-FE-EVT-071 | Event Filter Panel |
| REQ-FE-EVT-072 | Event Status Stepper |
| REQ-FE-EVT-080 | AI Event Recommendations |
| REQ-FE-EVT-081 | AI Optimization Suggestions |
| REQ-FE-EVT-090 | Event Analytics Dashboard |
| REQ-FE-EVT-091 | Event Metrics Charts |

---

## 3. Customer Management Module (FULL)

### All Requirements In Scope

| Requirement ID | Description |
|----------------|-------------|
| REQ-FE-CUS-001 | Customer List Page |
| REQ-FE-CUS-002 | Customer List Table |
| REQ-FE-CUS-003 | Customer List Sorting |
| REQ-FE-CUS-004 | Customer List Filtering |
| REQ-FE-CUS-005 | Customer Search |
| REQ-FE-CUS-006 | Customer Card View |
| REQ-FE-CUS-010 | Customer Detail Page |
| REQ-FE-CUS-011 | Customer Information Display |
| REQ-FE-CUS-012 | Customer Contact Display |
| REQ-FE-CUS-013 | Customer Address Display |
| REQ-FE-CUS-014 | Customer Events History Tab |
| REQ-FE-CUS-015 | Customer Communication Tab |
| REQ-FE-CUS-016 | Customer Photo Upload |
| REQ-FE-CUS-017 | Customer Tags Management |
| REQ-FE-CUS-020 | Create Customer Page |
| REQ-FE-CUS-021 | Customer Form |
| REQ-FE-CUS-025 | Multiple Addresses Editor |
| REQ-FE-CUS-030 | Edit Customer Page |
| REQ-FE-CUS-035 | Customer Complaint Management |
| REQ-FE-CUS-036 | Customer Testimonials |
| REQ-FE-CUS-040 | Delete Customer Confirmation |
| REQ-FE-CUS-050 | Customer Service |
| REQ-FE-CUS-060 | Customer Filter Panel |
| REQ-FE-CUS-070 | Customer Sentiment Display |
| REQ-FE-CUS-080 | Customer Analytics Dashboard |
| REQ-FE-CUS-081 | Customer Lifetime Value Chart |

---

## 4. Venue Management Module (FULL)

### All Requirements In Scope

| Requirement ID | Description |
|----------------|-------------|
| REQ-FE-VEN-001 | Venue List Page |
| REQ-FE-VEN-002 | Venue List Table |
| REQ-FE-VEN-003 | Venue List Sorting |
| REQ-FE-VEN-004 | Venue List Filtering |
| REQ-FE-VEN-005 | Venue Search |
| REQ-FE-VEN-006 | Venue Card View |
| REQ-FE-VEN-007 | Venue Map View |
| REQ-FE-VEN-010 | Venue Detail Page |
| REQ-FE-VEN-011 | Venue Information Display |
| REQ-FE-VEN-012 | Venue Contact Display |
| REQ-FE-VEN-013 | Venue Address Display |
| REQ-FE-VEN-014 | Venue Capacity Display |
| REQ-FE-VEN-015 | Venue Amenities Editor |
| REQ-FE-VEN-016 | Venue Photo Gallery |
| REQ-FE-VEN-017 | Venue Events Calendar |
| REQ-FE-VEN-018 | Venue Availability Calendar |
| REQ-FE-VEN-020 | Create Venue Page |
| REQ-FE-VEN-021 | Venue Form |
| REQ-FE-VEN-025 | Venue Ratings Display |
| REQ-FE-VEN-026 | Venue Issue Management |
| REQ-FE-VEN-030 | Edit Venue Page |
| REQ-FE-VEN-040 | Delete Venue Confirmation |
| REQ-FE-VEN-050 | Venue Service |
| REQ-FE-VEN-060 | Venue Filter Panel |
| REQ-FE-VEN-061 | Photo Upload Dialog |
| REQ-FE-VEN-062 | Amenity Selector |
| REQ-FE-VEN-070 | Map Integration |
| REQ-FE-VEN-080 | Venue Analytics Dashboard |

---

## 5. Staff Management Module (FULL)

### All Requirements In Scope

| Requirement ID | Description |
|----------------|-------------|
| REQ-FE-STF-001 | Staff List Page |
| REQ-FE-STF-002 | Staff List Table |
| REQ-FE-STF-003 | Staff List Sorting |
| REQ-FE-STF-004 | Staff List Filtering |
| REQ-FE-STF-005 | Staff Search |
| REQ-FE-STF-006 | Staff Card View |
| REQ-FE-STF-010 | Staff Detail Page |
| REQ-FE-STF-011 | Staff Information Display |
| REQ-FE-STF-012 | Staff Contact Display |
| REQ-FE-STF-013 | Staff Role Display |
| REQ-FE-STF-014 | Staff Photo Upload |
| REQ-FE-STF-015 | Staff Availability Calendar |
| REQ-FE-STF-016 | Staff Event Assignments Tab |
| REQ-FE-STF-017 | Staff Shift Scheduler |
| REQ-FE-STF-018 | Staff Check-in Interface |
| REQ-FE-STF-020 | Create Staff Page |
| REQ-FE-STF-021 | Staff Form |
| REQ-FE-STF-025 | Staff Performance Dashboard |
| REQ-FE-STF-030 | Edit Staff Page |
| REQ-FE-STF-040 | Delete Staff Confirmation |
| REQ-FE-STF-050 | Staff Service |
| REQ-FE-STF-060 | Staff Filter Panel |
| REQ-FE-STF-061 | Availability Calendar Component |
| REQ-FE-STF-062 | Shift Assignment Dialog |
| REQ-FE-STF-080 | AI Scheduling Suggestions |
| REQ-FE-STF-085 | Staff Analytics Dashboard |
| REQ-FE-STF-086 | Staff Utilization Charts |

---

## 6. Equipment Management Module (FULL)

### All Requirements In Scope

Includes ALL requirements from REQ-FE-EQP-001 through REQ-FE-EQP-144:

| Category | Requirements |
|----------|--------------|
| List Page | REQ-FE-EQP-001 to REQ-FE-EQP-008 |
| Detail Page | REQ-FE-EQP-010 to REQ-FE-EQP-016 |
| Forms | REQ-FE-EQP-020 to REQ-FE-EQP-025 |
| Booking | REQ-FE-EQP-030 to REQ-FE-EQP-036 |
| Logistics | REQ-FE-EQP-040 to REQ-FE-EQP-042 |
| Maintenance | REQ-FE-EQP-050 to REQ-FE-EQP-055 |
| Damage | REQ-FE-EQP-060 to REQ-FE-EQP-062 |
| Components | REQ-FE-EQP-070 to REQ-FE-EQP-076 |
| Services | REQ-FE-EQP-080 to REQ-FE-EQP-084 |
| Validation | REQ-FE-EQP-090 to REQ-FE-EQP-092 |
| Authorization | REQ-FE-EQP-100 to REQ-FE-EQP-102 |
| Accessibility | REQ-FE-EQP-110 to REQ-FE-EQP-113 |
| Performance | REQ-FE-EQP-120 to REQ-FE-EQP-124 |
| Testing | REQ-FE-EQP-130 to REQ-FE-EQP-134 |
| Error Handling | REQ-FE-EQP-140 to REQ-FE-EQP-144 |
| Analytics | REQ-FE-EQP-090 |

---

## 7. Invoice Management Module (FULL)

### All Requirements In Scope

| Requirement ID | Description |
|----------------|-------------|
| REQ-FE-INV-001 | Invoice List Page |
| REQ-FE-INV-002 | Invoice List Table |
| REQ-FE-INV-003 | Invoice List Filtering |
| REQ-FE-INV-004 | Invoice Search |
| REQ-FE-INV-010 | Invoice Detail Page |
| REQ-FE-INV-011 | Invoice Header Display |
| REQ-FE-INV-012 | Invoice Line Items Table |
| REQ-FE-INV-013 | Invoice Totals Display |
| REQ-FE-INV-014 | Payment History Display |
| REQ-FE-INV-020 | Create Invoice Page |
| REQ-FE-INV-021 | Invoice Form |
| REQ-FE-INV-022 | Line Item Editor |
| REQ-FE-INV-023 | Customer Selection |
| REQ-FE-INV-024 | Event Selection |
| REQ-FE-INV-030 | Edit Invoice Page |
| REQ-FE-INV-035 | Finalize Invoice Confirmation |
| REQ-FE-INV-036 | Void Invoice Confirmation |
| REQ-FE-INV-040 | Record Payment Dialog |
| REQ-FE-INV-041 | Partial Payment Support |
| REQ-FE-INV-050 | Download PDF Button |
| REQ-FE-INV-055 | Invoice Status Badge |
| REQ-FE-INV-060 | Invoice Service |
| REQ-FE-INV-070 | Refund Management UI |
| REQ-FE-INV-071 | Refund Request Dialog |
| REQ-FE-INV-072 | Refund Approval Workflow |
| REQ-FE-INV-075 | Credit Note UI |
| REQ-FE-INV-080 | Financial Dashboard |
| REQ-FE-INV-081 | Revenue Charts |
| REQ-FE-INV-082 | Outstanding Balance Widget |
| REQ-FE-INV-085 | AI Anomaly Alerts |
| REQ-FE-INV-086 | Payment Prediction Display |

---

## 8. Notification Center Module (FULL)

### All Requirements In Scope

| Requirement ID | Description |
|----------------|-------------|
| REQ-FE-NTF-001 | Notification Bell Icon |
| REQ-FE-NTF-002 | Unread Count Badge |
| REQ-FE-NTF-003 | Notification Dropdown Panel |
| REQ-FE-NTF-004 | Notification List |
| REQ-FE-NTF-005 | Notification Item Component |
| REQ-FE-NTF-006 | Mark as Read |
| REQ-FE-NTF-007 | Mark All as Read |
| REQ-FE-NTF-008 | Delete Notification |
| REQ-FE-NTF-010 | Notification Page (Full List) |
| REQ-FE-NTF-011 | Notification Filtering |
| REQ-FE-NTF-015 | Real-time Updates |
| REQ-FE-NTF-016 | Toast Notifications |
| REQ-FE-NTF-020 | Notification Preferences Page |
| REQ-FE-NTF-021 | Email Preference Toggle |
| REQ-FE-NTF-022 | Category Preferences |
| REQ-FE-NTF-023 | SMS Preference Toggle |
| REQ-FE-NTF-024 | Push Preference Toggle |
| REQ-FE-NTF-030 | Notification Service |
| REQ-FE-NTF-031 | SignalR Connection |
| REQ-FE-NTF-040 | Notification Analytics |
| REQ-FE-NTF-041 | Delivery Stats Dashboard |

---

## 9. Scheduling Module (FULL)

### All Requirements In Scope

| Requirement ID | Description |
|----------------|-------------|
| REQ-FE-SCH-001 | Schedule Calendar Page |
| REQ-FE-SCH-002 | Month View |
| REQ-FE-SCH-003 | Week View |
| REQ-FE-SCH-004 | Day View |
| REQ-FE-SCH-005 | Resource View |
| REQ-FE-SCH-010 | Create Schedule Entry Dialog |
| REQ-FE-SCH-011 | Edit Schedule Entry |
| REQ-FE-SCH-012 | Delete Schedule Entry |
| REQ-FE-SCH-015 | Drag and Drop Scheduling |
| REQ-FE-SCH-016 | Resize Schedule Entry |
| REQ-FE-SCH-020 | Conflict Indicator |
| REQ-FE-SCH-021 | Conflict Resolution Dialog |
| REQ-FE-SCH-025 | Filter by Resource Type |
| REQ-FE-SCH-026 | Filter by Date Range |
| REQ-FE-SCH-030 | Schedule Service |
| REQ-FE-SCH-035 | FullCalendar Integration |
| REQ-FE-SCH-040 | AI Scheduling Suggestions |
| REQ-FE-SCH-045 | Schedule Analytics |
| REQ-FE-SCH-046 | Utilization Charts |
| REQ-FE-SCH-050 | Print Schedule |
| REQ-FE-SCH-051 | Export Schedule |

---

## 10. Shipper Management Module (FULL)

### All Requirements In Scope

| Requirement ID | Description |
|----------------|-------------|
| REQ-FE-SHP-001 | Shipper List Page |
| REQ-FE-SHP-002 | Shipper List Table |
| REQ-FE-SHP-003 | Shipper Search |
| REQ-FE-SHP-010 | Shipper Detail Page |
| REQ-FE-SHP-011 | Shipper Information Display |
| REQ-FE-SHP-012 | Vehicle List Display |
| REQ-FE-SHP-013 | Driver Assignment Display |
| REQ-FE-SHP-020 | Create Shipper Page |
| REQ-FE-SHP-021 | Shipper Form |
| REQ-FE-SHP-030 | Edit Shipper Page |
| REQ-FE-SHP-040 | Shipment Tracking Page |
| REQ-FE-SHP-041 | Shipment List |
| REQ-FE-SHP-042 | Shipment Status Timeline |
| REQ-FE-SHP-050 | Route Map Display |
| REQ-FE-SHP-060 | Shipper Service |
| REQ-FE-SHP-070 | Shipper Analytics |

---

## 11. Prize Management Module (FULL)

### All Requirements In Scope

| Requirement ID | Description |
|----------------|-------------|
| REQ-FE-PRZ-001 | Prize List Page |
| REQ-FE-PRZ-002 | Prize List Table |
| REQ-FE-PRZ-003 | Prize Search |
| REQ-FE-PRZ-010 | Prize Detail Page |
| REQ-FE-PRZ-011 | Prize Information Display |
| REQ-FE-PRZ-012 | Prize Photo Gallery |
| REQ-FE-PRZ-020 | Create Prize Page |
| REQ-FE-PRZ-021 | Prize Form |
| REQ-FE-PRZ-030 | Edit Prize Page |
| REQ-FE-PRZ-040 | Assign to Event Dialog |
| REQ-FE-PRZ-041 | Winner Selection Dialog |
| REQ-FE-PRZ-042 | Prize Claim Processing |
| REQ-FE-PRZ-050 | Prize Service |
| REQ-FE-PRZ-060 | Prize Analytics |

---

## 12. Invitation Management Module (FULL)

### All Requirements In Scope

| Requirement ID | Description |
|----------------|-------------|
| REQ-FE-INVT-001 | Invitation List Page |
| REQ-FE-INVT-002 | Invitation List Table |
| REQ-FE-INVT-003 | Invitation Search |
| REQ-FE-INVT-004 | RSVP Status Filter |
| REQ-FE-INVT-010 | Invitation Detail Page |
| REQ-FE-INVT-011 | Guest Information Display |
| REQ-FE-INVT-012 | RSVP Status Display |
| REQ-FE-INVT-020 | Create Invitation Page |
| REQ-FE-INVT-021 | Invitation Form |
| REQ-FE-INVT-022 | Bulk Import Guests |
| REQ-FE-INVT-025 | Send Invitation Dialog |
| REQ-FE-INVT-026 | Email Template Selector |
| REQ-FE-INVT-027 | Bulk Send Dialog |
| REQ-FE-INVT-030 | Edit Invitation Page |
| REQ-FE-INVT-040 | QR Code Display |
| REQ-FE-INVT-041 | Check-in Interface |
| REQ-FE-INVT-042 | Check-in Scanner |
| REQ-FE-INVT-050 | Invitation Service |
| REQ-FE-INVT-060 | Invitation Analytics |
| REQ-FE-INVT-061 | RSVP Statistics Dashboard |

---

## 13. Reporting Dashboard Module (FULL)

### All Requirements In Scope

| Requirement ID | Description |
|----------------|-------------|
| REQ-FE-RPT-001 | Main Dashboard Page |
| REQ-FE-RPT-002 | Dashboard Widgets |
| REQ-FE-RPT-003 | Widget Customization |
| REQ-FE-RPT-010 | Event Reports Page |
| REQ-FE-RPT-011 | Event Summary Charts |
| REQ-FE-RPT-012 | Event Trends Analysis |
| REQ-FE-RPT-020 | Financial Reports Page |
| REQ-FE-RPT-021 | Revenue Charts |
| REQ-FE-RPT-022 | Payment Trends |
| REQ-FE-RPT-023 | Outstanding Balance Summary |
| REQ-FE-RPT-030 | Resource Utilization Reports |
| REQ-FE-RPT-031 | Staff Utilization Charts |
| REQ-FE-RPT-032 | Equipment Utilization Charts |
| REQ-FE-RPT-033 | Venue Utilization Charts |
| REQ-FE-RPT-040 | Customer Reports Page |
| REQ-FE-RPT-041 | Customer Growth Charts |
| REQ-FE-RPT-050 | Report Export (PDF) |
| REQ-FE-RPT-051 | Report Export (Excel) |
| REQ-FE-RPT-052 | Report Email |
| REQ-FE-RPT-060 | Report Scheduling |
| REQ-FE-RPT-070 | AI Insights Panel |
| REQ-FE-RPT-071 | Anomaly Alerts |
| REQ-FE-RPT-072 | Predictions Display |
| REQ-FE-RPT-080 | Report Service |

---

## 14. Audit Log Viewer Module (FULL)

### All Requirements In Scope

| Requirement ID | Description |
|----------------|-------------|
| REQ-FE-AUD-001 | Audit Log List Page |
| REQ-FE-AUD-002 | Audit Log Table |
| REQ-FE-AUD-003 | Audit Log Filtering |
| REQ-FE-AUD-004 | Audit Log Search |
| REQ-FE-AUD-010 | Audit Log Detail View |
| REQ-FE-AUD-011 | Change Comparison View |
| REQ-FE-AUD-020 | User Activity View |
| REQ-FE-AUD-021 | User Activity Timeline |
| REQ-FE-AUD-030 | Export Audit Logs |
| REQ-FE-AUD-040 | Audit Service |

---

## 15. Integration Management Module (FULL)

### All Requirements In Scope

| Requirement ID | Description |
|----------------|-------------|
| REQ-FE-INT-001 | Integration List Page |
| REQ-FE-INT-002 | Integration Cards |
| REQ-FE-INT-003 | Integration Health Status |
| REQ-FE-INT-010 | Integration Detail Page |
| REQ-FE-INT-011 | Integration Configuration |
| REQ-FE-INT-012 | Connection Test |
| REQ-FE-INT-020 | Webhook Management Page |
| REQ-FE-INT-021 | Webhook List |
| REQ-FE-INT-022 | Create Webhook Dialog |
| REQ-FE-INT-023 | Webhook Logs |
| REQ-FE-INT-030 | API Keys Management |
| REQ-FE-INT-031 | API Key Generation |
| REQ-FE-INT-032 | API Usage Dashboard |
| REQ-FE-INT-040 | Calendar Sync Setup |
| REQ-FE-INT-050 | Integration Service |

---

## Complete Page Structure

```
src/app/pages/
├── auth/
│   ├── login/
│   ├── register/
│   ├── forgot-password/
│   ├── reset-password/
│   └── mfa-setup/
├── events/
│   ├── event-list/
│   ├── event-detail/
│   ├── event-create/
│   ├── event-edit/
│   └── event-analytics/
├── customers/
│   ├── customer-list/
│   ├── customer-detail/
│   ├── customer-create/
│   ├── customer-edit/
│   └── customer-analytics/
├── venues/
│   ├── venue-list/
│   ├── venue-detail/
│   ├── venue-create/
│   ├── venue-edit/
│   ├── venue-map/
│   └── venue-analytics/
├── staff/
│   ├── staff-list/
│   ├── staff-detail/
│   ├── staff-create/
│   ├── staff-edit/
│   ├── staff-schedule/
│   ├── staff-checkin/
│   └── staff-analytics/
├── equipment/
│   ├── equipment-list/
│   ├── equipment-detail/
│   ├── equipment-create/
│   ├── equipment-edit/
│   ├── reservations-list/
│   ├── logistics-tracker/
│   ├── maintenance-schedule/
│   └── equipment-analytics/
├── invoices/
│   ├── invoice-list/
│   ├── invoice-detail/
│   ├── invoice-create/
│   ├── invoice-edit/
│   ├── refunds/
│   └── financial-dashboard/
├── notifications/
│   ├── notification-list/
│   ├── notification-prefs/
│   └── notification-analytics/
├── schedule/
│   ├── schedule-calendar/
│   └── schedule-analytics/
├── shippers/
│   ├── shipper-list/
│   ├── shipper-detail/
│   ├── shipper-create/
│   ├── shipper-edit/
│   ├── shipment-tracking/
│   └── shipper-analytics/
├── prizes/
│   ├── prize-list/
│   ├── prize-detail/
│   ├── prize-create/
│   ├── prize-edit/
│   └── prize-analytics/
├── invitations/
│   ├── invitation-list/
│   ├── invitation-detail/
│   ├── invitation-create/
│   ├── checkin-scanner/
│   └── invitation-analytics/
├── reports/
│   ├── dashboard/
│   ├── event-reports/
│   ├── financial-reports/
│   ├── resource-reports/
│   └── customer-reports/
├── audit/
│   ├── audit-log-list/
│   └── user-activity/
├── integrations/
│   ├── integration-list/
│   ├── integration-detail/
│   ├── webhooks/
│   ├── api-keys/
│   └── calendar-sync/
├── profile/
│   └── user-profile/
└── invitations/
    └── accept-invitation/
```

---

## Complete Services

| Service | Description |
|---------|-------------|
| AuthService | Authentication, token management |
| EventService | Event CRUD operations |
| CustomerService | Customer CRUD operations |
| VenueService | Venue CRUD operations |
| StaffService | Staff CRUD operations |
| EquipmentService | Equipment CRUD operations |
| ReservationService | Equipment reservations |
| LogisticsService | Equipment logistics |
| MaintenanceService | Equipment maintenance |
| InvoiceService | Invoice operations |
| PaymentService | Payment operations |
| NotificationService | Notification operations |
| SignalRService | Real-time connection |
| ScheduleService | Schedule operations |
| ShipperService | Shipper operations |
| PrizeService | Prize operations |
| InvitationService | Invitation operations |
| ReportService | Report generation |
| AuditService | Audit log operations |
| IntegrationService | Integration management |
| FileUploadService | Photo upload |
| ExportService | PDF/Excel export |

---

## Non-Functional Requirements (Phase C)

### Performance
- First Contentful Paint < 1.5 seconds
- Time to Interactive < 3 seconds
- Initial bundle size < 400KB (gzipped)
- Lazy loading for all feature modules
- Image optimization and lazy loading
- Virtual scrolling for large lists

### Accessibility
- WCAG 2.1 Level AAA compliance
- Full keyboard navigation
- Screen reader support
- High contrast mode
- Reduced motion support
- Focus management

### Browser Support
- Chrome (latest 3 versions)
- Firefox (latest 3 versions)
- Safari (latest 3 versions)
- Edge (latest 3 versions)
- Mobile browsers (iOS Safari, Android Chrome)

### Internationalization
- i18n support ready
- RTL layout support
- Date/number formatting by locale

### Progressive Web App
- Service worker for offline support
- App manifest for installation
- Push notifications

### Testing
- Unit test coverage minimum 80%
- E2E tests for all critical flows
- Visual regression tests
- Performance tests
- Accessibility tests
- Cross-browser tests

---

## Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-22 | System | Initial full specification |
