# Event Management Platform - Domain Events

## Document Overview
This document catalogs all domain events in the event management system. Domain events represent significant business occurrences that have already happened and are named in past tense.

---

## 1. Event Management

### Event Lifecycle Events
- `EventCreated` - A new event was registered in the system
- `EventDetailsUpdated` - Event information (date, time, venue, type) was modified
- `EventDateChanged` - Event date/time was rescheduled
- `EventVenueChanged` - Event location was updated
- `EventTypeChanged` - Event type was modified
- `EventCancelled` - Event was cancelled by customer or system
- `EventReinstated` - Cancelled event was restored
- `EventCompleted` - Event successfully took place and was marked complete
- `EventArchived` - Completed event was moved to historical records

### Event Status Events
- `EventDraftCreated` - Initial event entry created in draft status
- `EventConfirmed` - Event booking was confirmed by customer
- `EventPendingApproval` - Event submitted and awaiting internal approval
- `EventApproved` - Event was approved for scheduling
- `EventRejected` - Event booking was rejected with reason

### Event Notes & Communication
- `EventNoteAdded` - Internal note or comment added to event
- `EventNoteUpdated` - Existing event note was modified
- `EventNoteDeleted` - Event note was removed
- `CustomerCommentRecorded` - Customer feedback or special request recorded

---

## 2. Invitation Management

### Invitation Ordering Events
- `InvitationOrderCreated` - Customer ordered invitations for their event
- `InvitationOrderUpdated` - Invitation order details were modified
- `InvitationOrderCancelled` - Invitation order was cancelled
- `InvitationTemplateSelected` - Customer chose a pre-designed template
- `CustomInvitationDesignRequested` - Customer requested custom invitation design
- `InvitationDesignApproved` - Customer approved the invitation design
- `InvitationDesignRejected` - Customer rejected design and requested changes
- `InvitationDesignRevisionRequested` - Customer requested changes to design

### Invitation Production Events
- `InvitationPrintJobCreated` - Invitations sent to production/printing
- `InvitationPrintJobCompleted` - Invitations finished printing
- `InvitationQualityCheckPassed` - Printed invitations passed quality inspection
- `InvitationQualityCheckFailed` - Printed invitations failed quality check
- `InvitationPackageShipped` - Invitations shipped to customer
- `InvitationPackageDelivered` - Invitations delivered to customer address

### Invitation Personalization Events
- `InvitationTextCustomized` - Custom text/wording added to invitation
- `InvitationQuantityChanged` - Number of invitations ordered was modified
- `InvitationFreePromoApplied` - Free invitations included with event package
- `InvitationDigitalVersionGenerated` - Digital/electronic version created

---

## 3. Staff Management

### Staff Profile Events
- `StaffMemberRegistered` - New staff member added to the system
- `StaffProfileUpdated` - Staff member information was modified
- `StaffProfileActivated` - Staff profile set to active/available status
- `StaffProfileDeactivated` - Staff profile set to inactive
- `StaffPhotoUploaded` - Staff photo added or updated
- `StaffPhotoRemoved` - Staff photo deleted
- `StaffMemberRemoved` - Staff member permanently removed from system

### Staff Availability Events
- `StaffAvailabilityDeclared` - Staff member indicated available dates/times
- `StaffAvailabilityUpdated` - Staff member modified their availability
- `StaffUnavailableDateAdded` - Staff member marked specific date as unavailable
- `StaffUnavailableDateRemoved` - Previously unavailable date made available
- `StaffRecurringAvailabilitySet` - Regular weekly availability pattern established

### Staff Booking Events
- `StaffBookingRequested` - Event requested specific staff member
- `StaffAssignedToEvent` - Staff member scheduled for an event
- `StaffReassignedToEvent` - Different staff member assigned (replacement)
- `StaffUnassignedFromEvent` - Staff member removed from event schedule
- `StaffBookingConfirmed` - Staff member confirmed their assignment
- `StaffBookingDeclined` - Staff member declined the booking
- `StaffDoubleBookingDetected` - System detected conflicting bookings
- `StaffDoubleBookingPrevented` - System blocked conflicting booking

### Staff Performance Events
- `StaffCheckedIn` - Staff member arrived at event location
- `StaffCheckedOut` - Staff member completed their shift
- `StaffNoShow` - Staff member failed to appear for scheduled event
- `StaffRatingRecorded` - Customer or internal rating captured
- `StaffFeedbackReceived` - Customer feedback about staff member recorded
- `StaffComplaintReceived` - Complaint about staff behavior filed
- `StaffComplimentReceived` - Positive feedback/compliment recorded

---

## 4. Prize Management

### Prize Catalog Events
- `PrizeItemAdded` - New prize item added to catalog
- `PrizeItemUpdated` - Prize item details modified
- `PrizeItemRemoved` - Prize item removed from catalog
- `PrizeItemActivated` - Prize made available for ordering
- `PrizeItemDeactivated` - Prize removed from active offerings
- `PrizeItemPhotoUploaded` - Prize item image added
- `PrizePackCreated` - Pre-configured prize package created
- `PrizePackUpdated` - Prize package contents modified

### Prize Ordering Events
- `EventPrizesOrdered` - Prizes ordered for specific event
- `EventPrizeOrderUpdated` - Prize order quantities or items modified
- `EventPrizeOrderCancelled` - Prize order cancelled
- `PrizeQuantityIncreased` - More prizes added to event order
- `PrizeQuantityDecreased` - Prize quantities reduced
- `PrizeItemAddedToOrder` - Additional prize type added to order
- `PrizeItemRemovedFromOrder` - Prize type removed from order

### Prize Inventory Events
- `PrizeInventoryReceived` - New prize stock received
- `PrizeInventoryAdjusted` - Manual inventory correction made
- `PrizeInventoryAllocated` - Prizes reserved for specific event
- `PrizeInventoryDeallocated` - Prize reservation released
- `PrizeStockDepleted` - Prize item out of stock
- `PrizeLowStockWarningTriggered` - Prize inventory below threshold
- `PrizeInventoryReplenished` - Prize stock refilled

### Prize Distribution Events
- `PrizesPackedForEvent` - Prizes prepared for event delivery
- `PrizesShippedToEvent` - Prizes sent to event venue
- `PrizesDeliveredToEvent` - Prizes arrived at venue
- `PrizesDistributedAtEvent` - Prizes given out during event
- `UnusedPrizesReturned` - Leftover prizes returned to inventory

---

## 5. Equipment Management

### Equipment Catalog Events
- `EquipmentItemAdded` - New equipment (table, game) added to inventory
- `EquipmentItemUpdated` - Equipment details modified
- `EquipmentItemRemoved` - Equipment removed from inventory
- `EquipmentItemActivated` - Equipment made available for rental
- `EquipmentItemDeactivated` - Equipment taken out of service
- `EquipmentPhotoUploaded` - Equipment image added
- `EquipmentSpecificationsUpdated` - Technical details/specs modified

### Equipment Booking Events
- `EquipmentBookingRequested` - Equipment requested for event
- `EquipmentReservedForEvent` - Equipment allocated to specific event
- `EquipmentReservationUpdated` - Equipment booking modified
- `EquipmentReservationCancelled` - Equipment booking cancelled
- `EquipmentDoubleBookingDetected` - Conflict detected in equipment schedule
- `EquipmentDoubleBookingPrevented` - System blocked conflicting booking
- `EquipmentDoubleBookingOverridden` - Manual override of booking conflict
- `EquipmentAvailabilityChecked` - Equipment availability queried for date
- `EquipmentAlternativeSuggested` - System suggested alternate equipment

### Equipment Logistics Events
- `EquipmentPackedForShipment` - Equipment prepared for transport
- `EquipmentLoadedOnTruck` - Equipment loaded for delivery
- `EquipmentDispatchedToEvent` - Equipment left warehouse/storage
- `EquipmentDeliveredToVenue` - Equipment arrived at event location
- `EquipmentSetupCompleted` - Equipment installed and ready at venue
- `EquipmentPickedUpFromVenue` - Equipment collected after event
- `EquipmentReturnedToWarehouse` - Equipment back in storage

### Equipment Maintenance Events
- `EquipmentMaintenanceScheduled` - Maintenance appointment created
- `EquipmentMaintenanceStarted` - Maintenance work began
- `EquipmentMaintenanceCompleted` - Maintenance finished
- `EquipmentInspectionPassed` - Equipment passed quality check
- `EquipmentInspectionFailed` - Equipment failed quality check
- `EquipmentDamageReported` - Damage or issue discovered
- `EquipmentRepairRequested` - Repair work needed
- `EquipmentRepairCompleted` - Repair work finished
- `EquipmentReplacementRequired` - Equipment beyond repair
- `EquipmentConditionDowngraded` - Equipment condition rating lowered
- `EquipmentConditionUpgraded` - Equipment condition improved
- `EquipmentRetired` - Equipment permanently removed from service

---

## 6. Invoice & Financial Management

### Invoice Generation Events
- `InvoiceDraftCreated` - Initial invoice generated for event
- `InvoiceItemAdded` - Line item added to invoice
- `InvoiceItemRemoved` - Line item removed from invoice
- `InvoiceItemUpdated` - Line item details or pricing changed
- `InvoiceRecalculated` - Invoice totals recalculated
- `InvoiceFinalized` - Invoice locked and ready for customer
- `InvoiceVoided` - Invoice cancelled and nullified
- `InvoiceCorrected` - Correction invoice created for errors

### Invoice Line Items Events
- `StaffFeesAddedToInvoice` - Staff charges added
- `InvitationFeesAddedToInvoice` - Invitation charges added
- `PrizeFeesAddedToInvoice` - Prize charges added
- `EquipmentRentalFeesAddedToInvoice` - Equipment rental charges added
- `AdditionalFeesAddedToInvoice` - Miscellaneous charges added
- `DiscountAppliedToInvoice` - Discount or promotion applied
- `DiscountRemovedFromInvoice` - Discount removed
- `TaxCalculated` - Sales tax computed for invoice

### Invoice Delivery Events
- `InvoiceSentToCustomer` - Invoice delivered via email or mail
- `InvoiceViewed` - Customer opened/viewed invoice
- `InvoiceDownloaded` - Customer downloaded invoice PDF
- `InvoiceReminderSent` - Payment reminder sent to customer
- `InvoicePrintRequested` - Physical invoice print requested

### Payment Events
- `PaymentReceived` - Customer payment recorded
- `PartialPaymentReceived` - Partial payment recorded
- `PaymentRefunded` - Payment returned to customer
- `PartialRefundIssued` - Partial refund processed
- `PaymentFailed` - Payment processing failed
- `PaymentRetried` - Failed payment attempted again
- `InvoiceFullyPaid` - Invoice paid in full
- `InvoiceOverpaid` - Payment exceeded invoice amount
- `OverpaymentRefunded` - Excess payment returned
- `InvoicePastDue` - Invoice payment deadline passed
- `InvoiceWrittenOff` - Uncollectable invoice written off

---

## 7. Shipper & Logistics Management

### Shipper List Events
- `ShipperListGenerated` - Checklist created for event delivery
- `ShipperListUpdated` - Checklist items modified
- `ShipperListFinalized` - Checklist locked and ready for driver
- `ShipperListPrinted` - Physical checklist printed
- `ShipperListExported` - Checklist exported to PDF

### Shipping Items Events
- `ItemAddedToShipperList` - Equipment/prize added to checklist
- `ItemRemovedFromShipperList` - Item removed from checklist
- `ItemQuantityUpdatedOnShipperList` - Item quantity changed
- `ItemMarkedAsPacked` - Item confirmed in shipment
- `ItemMarkedAsLoaded` - Item confirmed on vehicle

### Delivery Tracking Events
- `ShipmentCreatedForEvent` - Shipment record created
- `ShipmentAssignedToDriver` - Driver assigned to delivery
- `ShipmentDepartedWarehouse` - Delivery vehicle left facility
- `ShipmentInTransit` - Delivery en route to venue
- `ShipmentArrivedAtVenue` - Delivery reached destination
- `ItemDeliveredToVenue` - Individual item confirmed delivered
- `AllItemsDeliveredToVenue` - Complete delivery confirmed
- `DeliverySignatureReceived` - Customer signed for delivery
- `DeliveryExceptionReported` - Issue during delivery reported
- `DeliveryRescheduled` - Delivery date/time changed

### Return Tracking Events
- `ItemPickupScheduled` - Return pickup arranged
- `ItemPickedUpFromVenue` - Item collected from venue
- `ItemReturnedToWarehouse` - Item back in inventory
- `ItemDamagedDuringReturn` - Damage noted during return
- `ItemLostDuringReturn` - Item not returned/missing
- `AllItemsReturnedFromEvent` - Complete return confirmed

---

## 8. Scheduling & Calendar Management

### Calendar Events
- `EventScheduledOnCalendar` - Event added to schedule
- `EventRescheduled` - Event moved to different date/time
- `EventRemovedFromCalendar` - Event removed from schedule
- `CalendarViewGenerated` - Calendar report created for date range
- `DailyScheduleExported` - Day's events exported
- `WeeklyScheduleExported` - Week's events exported
- `MonthlyScheduleExported` - Month's events exported

### Conflict Detection Events
- `ScheduleConflictDetected` - Overlapping booking identified
- `ScheduleConflictResolved` - Conflict manually resolved
- `ScheduleConflictEscalated` - Conflict requires management review
- `OverbookingWarningTriggered` - Multiple bookings for same resource
- `OverbookingPreventedBySystem` - System blocked conflicting booking
- `OverbookingAllowedByOverride` - Manual override of conflict

### Availability Events
- `ResourceAvailabilityChecked` - System queried for available resources
- `MultipleResourcesChecked` - Bulk availability query performed
- `ResourceMarkedUnavailable` - Resource blocked for maintenance/other
- `ResourceMarkedAvailable` - Resource returned to available pool

---

## 9. Venue Management

### Venue Directory Events
- `VenueAdded` - New venue added to system
- `VenueDetailsUpdated` - Venue information modified
- `VenueRemoved` - Venue removed from directory
- `VenueActivated` - Venue made available for events
- `VenueDeactivated` - Venue removed from active list
- `VenueContactAdded` - Venue contact person added
- `VenueContactUpdated` - Venue contact information changed
- `VenueContactRemoved` - Venue contact deleted

### Venue Characteristics Events
- `VenueAddressUpdated` - Venue location changed
- `VenueCapacityUpdated` - Venue size/capacity modified
- `VenueAmenitiesUpdated` - Venue features/amenities changed
- `VenueAccessInstructionsAdded` - Delivery/access notes added
- `VenueParkingInfoUpdated` - Parking details modified
- `VenuePhotoUploaded` - Venue image added

### Venue History Events
- `VenueUsedForEvent` - Venue recorded as event location
- `VenueRatingRecorded` - Venue quality rating captured
- `VenueFeedbackReceived` - Notes about venue recorded
- `VenueIssueReported` - Problem with venue documented
- `VenueBlacklisted` - Venue marked as problematic
- `VenueWhitelisted` - Venue marked as preferred

---

## 10. Contact & Customer Management

### Customer Events
- `CustomerRegistered` - New customer account created
- `CustomerProfileUpdated` - Customer information modified
- `CustomerContactInfoUpdated` - Customer contact details changed
- `CustomerPreferencesUpdated` - Customer preferences recorded
- `CustomerMerged` - Duplicate customer records combined
- `CustomerDeactivated` - Customer account disabled
- `CustomerReactivated` - Customer account restored

### Contact Lists Events
- `ContactAdded` - New contact added to directory
- `ContactUpdated` - Contact information modified
- `ContactRemoved` - Contact deleted from system
- `ContactTagged` - Label/category applied to contact
- `ContactUntagged` - Label removed from contact
- `ContactListImported` - Bulk contacts imported
- `ContactListExported` - Contact list exported

### Customer Communication Events
- `CustomerEmailSent` - Email sent to customer
- `CustomerSMSSent` - Text message sent to customer
- `CustomerPhoneCallLogged` - Phone conversation recorded
- `CustomerMeetingScheduled` - In-person meeting arranged
- `CustomerFollowUpCreated` - Follow-up task created
- `CustomerComplaintReceived` - Customer complaint logged
- `CustomerComplaintResolved` - Complaint handled
- `CustomerTestimonialReceived` - Positive feedback recorded

---

## 11. Reporting & Analytics

### Report Generation Events
- `DailySummaryGenerated` - Daily event summary created
- `WeeklySummaryGenerated` - Weekly report created
- `MonthlySummaryGenerated` - Monthly report created
- `YearlySummaryGenerated` - Annual report created
- `CustomReportGenerated` - Ad-hoc report created
- `ReportScheduled` - Recurring report scheduled
- `ReportExportedToPDF` - Report exported as PDF
- `ReportExportedToExcel` - Report exported as spreadsheet
- `ReportEmailedToRecipient` - Report sent via email

### Statistics Events
- `EventStatisticsCalculated` - Event metrics computed
- `RevenueStatisticsCalculated` - Financial metrics computed
- `StaffStatisticsCalculated` - Staff performance metrics computed
- `EquipmentUtilizationCalculated` - Equipment usage metrics computed
- `CustomerStatisticsCalculated` - Customer engagement metrics computed
- `VenueStatisticsCalculated` - Venue usage metrics computed

### Dashboard Events
- `DashboardViewed` - User accessed dashboard
- `DashboardRefreshed` - Dashboard data updated
- `DashboardWidgetAdded` - New widget added to dashboard
- `DashboardWidgetRemoved` - Widget removed from dashboard
- `DashboardLayoutSaved` - Dashboard configuration saved

---

## 12. Notification & Alert Management

### System Notifications
- `OverbookingAlertSent` - Equipment double-booking notification sent
- `LowInventoryAlertSent` - Low stock notification sent
- `MaintenanceDueAlertSent` - Equipment maintenance reminder sent
- `PaymentDueAlertSent` - Invoice payment reminder sent
- `StaffNoShowAlertSent` - Staff absence notification sent
- `EventReminderSent` - Upcoming event reminder sent
- `DeliveryDelayAlertSent` - Shipment delay notification sent

### User Notifications
- `UserNotificationCreated` - Notification generated for user
- `UserNotificationViewed` - User opened notification
- `UserNotificationDismissed` - User closed notification
- `UserNotificationActionTaken` - User acted on notification
- `NotificationPreferencesUpdated` - User notification settings changed

### Escalation Events
- `IssueEscalated` - Problem escalated to management
- `UrgentAlertSent` - Critical issue notification sent
- `EmergencyNotificationBroadcast` - Emergency message sent to all

---

## 13. User & Access Management

### User Account Events
- `UserAccountCreated` - New system user account created
- `UserAccountUpdated` - User profile modified
- `UserAccountDeactivated` - User account disabled
- `UserAccountReactivated` - User account restored
- `UserPasswordChanged` - User updated password
- `UserPasswordReset` - Password reset performed
- `UserLoginSuccessful` - User logged in successfully
- `UserLoginFailed` - Failed login attempt
- `UserLoggedOut` - User ended session
- `UserSessionExpired` - User session timed out

### Permission Events
- `UserRoleAssigned` - Role granted to user
- `UserRoleRevoked` - Role removed from user
- `UserPermissionGranted` - Specific permission granted
- `UserPermissionRevoked` - Specific permission removed
- `UnauthorizedAccessAttempted` - User tried to access restricted resource

---

## 14. Audit & Compliance

### Audit Trail Events
- `EntityCreated` - Any entity created in system
- `EntityUpdated` - Any entity modified
- `EntityDeleted` - Any entity removed
- `BulkOperationPerformed` - Mass update executed
- `DataExported` - Data exported from system
- `DataImported` - Data imported into system
- `SystemBackupCompleted` - System backup performed
- `SystemBackupFailed` - Backup operation failed
- `DataRestorePerformed` - Data restored from backup

### Compliance Events
- `CustomerDataAccessLogged` - Customer data viewed
- `CustomerDataExported` - Customer data exported
- `CustomerDataDeleted` - Customer data removed (GDPR/privacy)
- `PrivacyPolicyAccepted` - User accepted privacy policy
- `TermsOfServiceAccepted` - User accepted terms
- `ConsentRecorded` - Customer consent documented
- `ConsentRevoked` - Customer withdrew consent

---

## 15. Integration & External System Events

### Third-Party Integration Events
- `PaymentGatewayConnected` - Payment system integrated
- `PaymentGatewayDisconnected` - Payment system disconnected
- `EmailServiceConnected` - Email provider integrated
- `SMSServiceConnected` - SMS provider integrated
- `CalendarSyncEnabled` - External calendar sync enabled
- `CalendarSyncDisabled` - External calendar sync disabled
- `APIKeyGenerated` - API access key created
- `APIKeyRevoked` - API access key invalidated

### Synchronization Events
- `DataSyncInitiated` - Data synchronization started
- `DataSyncCompleted` - Data synchronization finished
- `DataSyncFailed` - Data synchronization error
- `WebhookReceived` - Incoming webhook processed
- `WebhookSent` - Outgoing webhook transmitted
- `WebhookDeliveryFailed` - Webhook delivery error

---

## Event Categories Summary

| Category | Event Count | Primary Purpose |
|----------|-------------|-----------------|
| Event Management | 20 | Core event lifecycle and status tracking |
| Invitation Management | 18 | Invitation ordering and production |
| Staff Management | 28 | Staff scheduling and performance |
| Prize Management | 23 | Prize ordering and inventory |
| Equipment Management | 39 | Equipment booking and maintenance |
| Invoice & Financial | 28 | Billing and payment processing |
| Shipper & Logistics | 25 | Delivery and return tracking |
| Scheduling & Calendar | 14 | Resource scheduling and conflicts |
| Venue Management | 17 | Venue directory and history |
| Contact & Customer | 20 | Customer relationship management |
| Reporting & Analytics | 15 | Business intelligence and reporting |
| Notification & Alerts | 14 | System notifications and alerts |
| User & Access | 14 | Authentication and authorization |
| Audit & Compliance | 14 | Audit trails and compliance |
| Integration | 12 | External system integration |

**Total Domain Events: 301**

---

## Event Naming Conventions

All domain events follow these conventions:
1. **Past Tense**: Events represent something that already happened
2. **Business Language**: Use domain terminology, not technical jargon
3. **Specific**: Clearly indicate what changed and why
4. **Immutable**: Events cannot be changed once published
5. **Self-Describing**: Event name should explain what happened

---

## Event Metadata

Each domain event should carry:
- **Event ID**: Unique identifier
- **Event Type**: The event name (e.g., "EventCreated")
- **Aggregate ID**: ID of the entity that changed
- **Timestamp**: When the event occurred
- **User ID**: Who triggered the event
- **Correlation ID**: Link related events
- **Causation ID**: What caused this event
- **Version**: Event schema version
- **Payload**: Event-specific data

---

## Event Sourcing Considerations

This event catalog supports:
- **Event Sourcing**: Complete audit trail of all changes
- **CQRS**: Separate read/write models
- **Event-Driven Architecture**: Loosely coupled microservices
- **Temporal Queries**: Reconstruct state at any point in time
- **Analytics**: Historical analysis and reporting
- **Integration**: Publish events to external systems

---

## Implementation Notes

### High-Priority Events for MVP
1. Core event lifecycle (EventCreated, EventConfirmed, EventCompleted)
2. Equipment booking (EquipmentReservedForEvent, EquipmentDoubleBookingDetected)
3. Staff scheduling (StaffAssignedToEvent, StaffDoubleBookingPrevented)
4. Invoice generation (InvoiceFinalized, PaymentReceived)
5. Shipper lists (ShipperListGenerated, ItemDeliveredToVenue)

### Event Processing Patterns
- **Command**: User action that may trigger events
- **Event Handler**: React to published events
- **Saga/Process Manager**: Coordinate multi-step workflows
- **Projection**: Build read models from events
- **Policy**: Automated reaction to events

### Event Storage
- Events should be stored in append-only log
- Consider using event store (EventStoreDB, Marten, etc.)
- Implement event versioning strategy
- Plan for event schema evolution
- Consider retention policies for old events

---

## Document Version
- **Version**: 1.0
- **Created**: 2025
- **Last Updated**: 2025
- **Author**: Domain Expert Analysis
- **Status**: Draft for Review

