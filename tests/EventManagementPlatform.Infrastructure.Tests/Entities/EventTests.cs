// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.EventAggregate;
using EventManagementPlatform.Infrastructure.Tests.Fixtures;
using EventManagementPlatform.Infrastructure.Tests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Infrastructure.Tests.Entities;

[Collection("Database")]
public class EventTests : IntegrationTestBase
{
    public EventTests(SqlExpressDatabaseFixture fixture)
        : base(fixture)
    {
    }

    #region Create (Nominal)

    [Fact]
    public async Task CreateEvent_WithValidData_ShouldPersistToDatabase()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType(name: "Conference");
        var venue = TestDataFactory.CreateVenue(name: "Convention Center");
        var customer = TestDataFactory.CreateCustomer(companyName: "Test Corp");

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var evt = TestDataFactory.CreateEvent(
            venueId: venue.VenueId,
            eventTypeId: eventType.EventTypeId,
            customerId: customer.CustomerId,
            title: "Annual Tech Conference",
            eventDate: DateTime.UtcNow.AddMonths(3)
        );

        // Act
        DbContext.Events.Add(evt);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedEvent = await verifyContext.Events
            .FirstOrDefaultAsync(e => e.EventId == evt.EventId);

        savedEvent.Should().NotBeNull();
        savedEvent!.Title.Should().Be("Annual Tech Conference");
        savedEvent.VenueId.Should().Be(venue.VenueId);
        savedEvent.EventTypeId.Should().Be(eventType.EventTypeId);
        savedEvent.CustomerId.Should().Be(customer.CustomerId);
        savedEvent.Status.Should().Be(EventStatus.Planned);
    }

    [Fact]
    public async Task CreateEvent_WithAllFields_ShouldPersistAllValues()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var evt = new Event
        {
            EventId = Guid.NewGuid(),
            Title = "Full Event Details",
            Description = "A comprehensive event with all details filled",
            EventDate = new DateTime(2025, 6, 15, 14, 0, 0, DateTimeKind.Utc),
            VenueId = venue.VenueId,
            EventTypeId = eventType.EventTypeId,
            CustomerId = customer.CustomerId,
            Status = EventStatus.Confirmed,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid()
        };

        // Act
        DbContext.Events.Add(evt);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedEvent = await verifyContext.Events
            .FirstOrDefaultAsync(e => e.EventId == evt.EventId);

        savedEvent.Should().NotBeNull();
        savedEvent!.Description.Should().Be("A comprehensive event with all details filled");
        savedEvent.Status.Should().Be(EventStatus.Confirmed);
        savedEvent.EventDate.Should().Be(new DateTime(2025, 6, 15, 14, 0, 0, DateTimeKind.Utc));
    }

    [Fact]
    public async Task CreateMultipleEvents_ShouldPersistAll()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var events = new[]
        {
            TestDataFactory.CreateEvent(eventTypeId: eventType.EventTypeId, venueId: venue.VenueId, customerId: customer.CustomerId, title: "Event A"),
            TestDataFactory.CreateEvent(eventTypeId: eventType.EventTypeId, venueId: venue.VenueId, customerId: customer.CustomerId, title: "Event B"),
            TestDataFactory.CreateEvent(eventTypeId: eventType.EventTypeId, venueId: venue.VenueId, customerId: customer.CustomerId, title: "Event C")
        };

        // Act
        DbContext.Events.AddRange(events);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedEvents = await verifyContext.Events.ToListAsync();
        savedEvents.Should().HaveCount(3);
    }

    #endregion

    #region Read (Nominal)

    [Fact]
    public async Task GetEventById_WhenExists_ShouldReturnEvent()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var evt = TestDataFactory.CreateEvent(
            eventTypeId: eventType.EventTypeId,
            venueId: venue.VenueId,
            customerId: customer.CustomerId,
            title: "Query Test Event"
        );
        DbContext.Events.Add(evt);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var result = await queryContext.Events
            .FirstOrDefaultAsync(e => e.EventId == evt.EventId);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Query Test Event");
    }

    [Fact]
    public async Task GetEvent_WithEventTypeIncluded_ShouldReturnWithNavigation()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType(name: "Wedding");
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var evt = TestDataFactory.CreateEvent(
            eventTypeId: eventType.EventTypeId,
            venueId: venue.VenueId,
            customerId: customer.CustomerId
        );
        DbContext.Events.Add(evt);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var result = await queryContext.Events
            .Include(e => e.EventType)
            .FirstOrDefaultAsync(e => e.EventId == evt.EventId);

        // Assert
        result.Should().NotBeNull();
        result!.EventType.Should().NotBeNull();
        result.EventType!.Name.Should().Be("Wedding");
    }

    [Fact]
    public async Task GetEvents_WithStatusFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var plannedEvent = TestDataFactory.CreateEvent(eventTypeId: eventType.EventTypeId, venueId: venue.VenueId, customerId: customer.CustomerId, status: EventStatus.Planned);
        var confirmedEvent = TestDataFactory.CreateEvent(eventTypeId: eventType.EventTypeId, venueId: venue.VenueId, customerId: customer.CustomerId, status: EventStatus.Confirmed);
        var cancelledEvent = TestDataFactory.CreateEvent(eventTypeId: eventType.EventTypeId, venueId: venue.VenueId, customerId: customer.CustomerId, status: EventStatus.Cancelled);

        DbContext.Events.AddRange(plannedEvent, confirmedEvent, cancelledEvent);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var confirmedEvents = await queryContext.Events
            .Where(e => e.Status == EventStatus.Confirmed)
            .ToListAsync();

        // Assert
        confirmedEvents.Should().HaveCount(1);
        confirmedEvents[0].EventId.Should().Be(confirmedEvent.EventId);
    }

    [Fact]
    public async Task GetEvents_WithDateRangeFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var pastEvent = TestDataFactory.CreateEvent(
            eventTypeId: eventType.EventTypeId, venueId: venue.VenueId, customerId: customer.CustomerId,
            eventDate: DateTime.UtcNow.AddMonths(-1));
        var currentEvent = TestDataFactory.CreateEvent(
            eventTypeId: eventType.EventTypeId, venueId: venue.VenueId, customerId: customer.CustomerId,
            eventDate: DateTime.UtcNow.AddDays(7));
        var futureEvent = TestDataFactory.CreateEvent(
            eventTypeId: eventType.EventTypeId, venueId: venue.VenueId, customerId: customer.CustomerId,
            eventDate: DateTime.UtcNow.AddMonths(2));

        DbContext.Events.AddRange(pastEvent, currentEvent, futureEvent);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var upcomingEvents = await queryContext.Events
            .Where(e => e.EventDate > DateTime.UtcNow)
            .ToListAsync();

        // Assert
        upcomingEvents.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetEvents_WithVenueFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue1 = TestDataFactory.CreateVenue(name: "Venue 1");
        var venue2 = TestDataFactory.CreateVenue(name: "Venue 2");
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.AddRange(venue1, venue2);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var eventAtVenue1 = TestDataFactory.CreateEvent(eventTypeId: eventType.EventTypeId, venueId: venue1.VenueId, customerId: customer.CustomerId);
        var eventAtVenue2 = TestDataFactory.CreateEvent(eventTypeId: eventType.EventTypeId, venueId: venue2.VenueId, customerId: customer.CustomerId);

        DbContext.Events.AddRange(eventAtVenue1, eventAtVenue2);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var venue1Events = await queryContext.Events
            .Where(e => e.VenueId == venue1.VenueId)
            .ToListAsync();

        // Assert
        venue1Events.Should().HaveCount(1);
        venue1Events[0].EventId.Should().Be(eventAtVenue1.EventId);
    }

    [Fact]
    public async Task GetEvents_WithCustomerFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer1 = TestDataFactory.CreateCustomer(companyName: "Customer 1");
        var customer2 = TestDataFactory.CreateCustomer(companyName: "Customer 2");

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.AddRange(customer1, customer2);
        await DbContext.SaveChangesAsync();

        var customer1Event = TestDataFactory.CreateEvent(eventTypeId: eventType.EventTypeId, venueId: venue.VenueId, customerId: customer1.CustomerId);
        var customer2Event = TestDataFactory.CreateEvent(eventTypeId: eventType.EventTypeId, venueId: venue.VenueId, customerId: customer2.CustomerId);

        DbContext.Events.AddRange(customer1Event, customer2Event);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var customer1Events = await queryContext.Events
            .Where(e => e.CustomerId == customer1.CustomerId)
            .ToListAsync();

        // Assert
        customer1Events.Should().HaveCount(1);
        customer1Events[0].EventId.Should().Be(customer1Event.EventId);
    }

    [Fact]
    public async Task GetEvents_WithTitleSearch_ShouldReturnMatchingResults()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var event1 = TestDataFactory.CreateEvent(eventTypeId: eventType.EventTypeId, venueId: venue.VenueId, customerId: customer.CustomerId, title: "Annual Tech Summit");
        var event2 = TestDataFactory.CreateEvent(eventTypeId: eventType.EventTypeId, venueId: venue.VenueId, customerId: customer.CustomerId, title: "Tech Workshop");
        var event3 = TestDataFactory.CreateEvent(eventTypeId: eventType.EventTypeId, venueId: venue.VenueId, customerId: customer.CustomerId, title: "Business Meeting");

        DbContext.Events.AddRange(event1, event2, event3);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var techEvents = await queryContext.Events
            .Where(e => e.Title.Contains("Tech"))
            .ToListAsync();

        // Assert
        techEvents.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetEvents_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var events = Enumerable.Range(1, 10)
            .Select(i => TestDataFactory.CreateEvent(
                eventTypeId: eventType.EventTypeId,
                venueId: venue.VenueId,
                customerId: customer.CustomerId,
                title: $"Event {i:D2}"))
            .ToList();

        DbContext.Events.AddRange(events);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var page1 = await queryContext.Events
            .OrderBy(e => e.Title)
            .Skip(0)
            .Take(3)
            .ToListAsync();

        var page2 = await queryContext.Events
            .OrderBy(e => e.Title)
            .Skip(3)
            .Take(3)
            .ToListAsync();

        // Assert
        page1.Should().HaveCount(3);
        page2.Should().HaveCount(3);
        page1.Select(e => e.EventId).Should().NotIntersectWith(page2.Select(e => e.EventId));
    }

    #endregion

    #region Update (Nominal)

    [Fact]
    public async Task UpdateEvent_ShouldPersistChanges()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var evt = TestDataFactory.CreateEvent(
            eventTypeId: eventType.EventTypeId,
            venueId: venue.VenueId,
            customerId: customer.CustomerId,
            title: "Original Title"
        );
        DbContext.Events.Add(evt);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var eventToUpdate = await updateContext.Events
            .FirstAsync(e => e.EventId == evt.EventId);

        eventToUpdate.Title = "Updated Title";
        eventToUpdate.Description = "New description";
        eventToUpdate.ModifiedAt = DateTime.UtcNow;
        eventToUpdate.ModifiedBy = Guid.NewGuid();
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedEvent = await verifyContext.Events
            .FirstOrDefaultAsync(e => e.EventId == evt.EventId);

        updatedEvent.Should().NotBeNull();
        updatedEvent!.Title.Should().Be("Updated Title");
        updatedEvent.Description.Should().Be("New description");
        updatedEvent.ModifiedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateEventStatus_ShouldPersistNewStatus()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var evt = TestDataFactory.CreateEvent(
            eventTypeId: eventType.EventTypeId,
            venueId: venue.VenueId,
            customerId: customer.CustomerId,
            status: EventStatus.Planned
        );
        DbContext.Events.Add(evt);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var eventToUpdate = await updateContext.Events
            .FirstAsync(e => e.EventId == evt.EventId);

        eventToUpdate.Status = EventStatus.Confirmed;
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedEvent = await verifyContext.Events
            .FirstOrDefaultAsync(e => e.EventId == evt.EventId);

        updatedEvent!.Status.Should().Be(EventStatus.Confirmed);
    }

    [Fact]
    public async Task UpdateEventDate_ShouldPersistNewDate()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var originalDate = DateTime.UtcNow.AddMonths(1);
        var evt = TestDataFactory.CreateEvent(
            eventTypeId: eventType.EventTypeId,
            venueId: venue.VenueId,
            customerId: customer.CustomerId,
            eventDate: originalDate
        );
        DbContext.Events.Add(evt);
        await DbContext.SaveChangesAsync();

        // Act
        var newDate = DateTime.UtcNow.AddMonths(2);
        await using var updateContext = CreateNewDbContext();
        var eventToUpdate = await updateContext.Events
            .FirstAsync(e => e.EventId == evt.EventId);

        eventToUpdate.EventDate = newDate;
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedEvent = await verifyContext.Events
            .FirstOrDefaultAsync(e => e.EventId == evt.EventId);

        updatedEvent!.EventDate.Should().BeCloseTo(newDate, TimeSpan.FromSeconds(1));
    }

    #endregion

    #region Delete (Nominal)

    [Fact]
    public async Task SoftDeleteEvent_ShouldNotAppearInQueries()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var evt = TestDataFactory.CreateEvent(
            eventTypeId: eventType.EventTypeId,
            venueId: venue.VenueId,
            customerId: customer.CustomerId
        );
        DbContext.Events.Add(evt);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var eventToDelete = await deleteContext.Events
            .FirstAsync(e => e.EventId == evt.EventId);

        eventToDelete.IsDeleted = true;
        await deleteContext.SaveChangesAsync();

        // Assert - Query filter should exclude soft-deleted records
        await using var verifyContext = CreateNewDbContext();
        var result = await verifyContext.Events
            .FirstOrDefaultAsync(e => e.EventId == evt.EventId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task SoftDeleteEvent_ShouldStillExistWithIgnoreQueryFilters()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var evt = TestDataFactory.CreateEvent(
            eventTypeId: eventType.EventTypeId,
            venueId: venue.VenueId,
            customerId: customer.CustomerId
        );
        DbContext.Events.Add(evt);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var eventToDelete = await deleteContext.Events
            .FirstAsync(e => e.EventId == evt.EventId);

        eventToDelete.IsDeleted = true;
        await deleteContext.SaveChangesAsync();

        // Assert - Should still exist when ignoring query filters
        await using var verifyContext = CreateNewDbContext();
        var result = await verifyContext.Events
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(e => e.EventId == evt.EventId);

        result.Should().NotBeNull();
        result!.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task HardDeleteEvent_ShouldRemoveFromDatabase()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var evt = TestDataFactory.CreateEvent(
            eventTypeId: eventType.EventTypeId,
            venueId: venue.VenueId,
            customerId: customer.CustomerId
        );
        DbContext.Events.Add(evt);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var eventToDelete = await deleteContext.Events
            .FirstAsync(e => e.EventId == evt.EventId);

        deleteContext.Events.Remove(eventToDelete);
        await deleteContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var result = await verifyContext.Events
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(e => e.EventId == evt.EventId);

        result.Should().BeNull();
    }

    #endregion

    #region Off-Nominal Cases

    [Fact]
    public async Task GetEventById_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await DbContext.Events
            .FirstOrDefaultAsync(e => e.EventId == nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateEvent_WithDuplicateId_ShouldThrowException()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var eventId = Guid.NewGuid();
        var event1 = TestDataFactory.CreateEvent(eventTypeId: eventType.EventTypeId, venueId: venue.VenueId, customerId: customer.CustomerId);
        event1.EventId = eventId;

        DbContext.Events.Add(event1);
        await DbContext.SaveChangesAsync();

        // Act & Assert
        await using var context2 = CreateNewDbContext();
        var event2 = TestDataFactory.CreateEvent(eventTypeId: eventType.EventTypeId, venueId: venue.VenueId, customerId: customer.CustomerId);
        event2.EventId = eventId;

        context2.Events.Add(event2);

        await Assert.ThrowsAsync<DbUpdateException>(
            () => context2.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateEvent_WithNullTitle_ShouldThrowException()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var evt = TestDataFactory.CreateEvent(
            eventTypeId: eventType.EventTypeId,
            venueId: venue.VenueId,
            customerId: customer.CustomerId
        );
        evt.Title = null!;

        // Act & Assert
        DbContext.Events.Add(evt);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateEvent_WithTitleExceedingMaxLength_ShouldThrowException()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var evt = TestDataFactory.CreateEvent(
            eventTypeId: eventType.EventTypeId,
            venueId: venue.VenueId,
            customerId: customer.CustomerId
        );
        evt.Title = new string('T', 201); // Max is 200

        // Act & Assert
        DbContext.Events.Add(evt);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateEvent_WithDescriptionExceedingMaxLength_ShouldThrowException()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var evt = TestDataFactory.CreateEvent(
            eventTypeId: eventType.EventTypeId,
            venueId: venue.VenueId,
            customerId: customer.CustomerId
        );
        evt.Description = new string('D', 2001); // Max is 2000

        // Act & Assert
        DbContext.Events.Add(evt);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateEvent_WithInvalidEventTypeId_ShouldThrowException()
    {
        // Arrange
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var evt = TestDataFactory.CreateEvent(
            eventTypeId: Guid.NewGuid(), // Non-existent EventType
            venueId: venue.VenueId,
            customerId: customer.CustomerId
        );

        // Act & Assert
        DbContext.Events.Add(evt);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task UpdateNonExistentEvent_ShouldThrowException()
    {
        // Arrange
        var nonExistentEvent = TestDataFactory.CreateEvent();

        // Act & Assert
        DbContext.Events.Update(nonExistentEvent);
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task DeleteEventType_WithExistingEvents_ShouldThrowException()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        var venue = TestDataFactory.CreateVenue();
        var customer = TestDataFactory.CreateCustomer();

        DbContext.EventTypes.Add(eventType);
        DbContext.Venues.Add(venue);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        var evt = TestDataFactory.CreateEvent(
            eventTypeId: eventType.EventTypeId,
            venueId: venue.VenueId,
            customerId: customer.CustomerId
        );
        DbContext.Events.Add(evt);
        await DbContext.SaveChangesAsync();

        // Act & Assert
        await using var deleteContext = CreateNewDbContext();
        var typeToDelete = await deleteContext.EventTypes
            .FirstAsync(et => et.EventTypeId == eventType.EventTypeId);

        deleteContext.EventTypes.Remove(typeToDelete);

        // Should fail due to foreign key constraint
        await Assert.ThrowsAsync<DbUpdateException>(
            () => deleteContext.SaveChangesAsync());
    }

    #endregion
}
