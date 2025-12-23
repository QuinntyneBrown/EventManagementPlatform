// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.CustomerAggregate;
using EventManagementPlatform.Core.Model.EquipmentAggregate;
using EventManagementPlatform.Core.Model.EventAggregate;
using EventManagementPlatform.Core.Model.StaffAggregate;
using EventManagementPlatform.Core.Model.UserAggregate;
using EventManagementPlatform.Core.Model.VenueAggregate;

namespace EventManagementPlatform.Infrastructure.Tests.Helpers;

/// <summary>
/// Factory class for creating test entities with default values.
/// </summary>
public static class TestDataFactory
{
    public static Customer CreateCustomer(
        string? companyName = null,
        CustomerType type = CustomerType.Individual,
        CustomerStatus status = CustomerStatus.Active,
        string? email = null,
        bool isDeleted = false)
    {
        return new Customer
        {
            CustomerId = Guid.NewGuid(),
            CompanyName = companyName ?? $"Test Company {Guid.NewGuid():N}",
            Type = type,
            PrimaryEmail = email ?? $"test-{Guid.NewGuid():N}@example.com",
            PrimaryPhone = "555-123-4567",
            Industry = "Technology",
            Website = "https://example.com",
            BillingStreet = "123 Main St",
            BillingCity = "Toronto",
            BillingState = "ON",
            BillingPostalCode = "M5V 2H1",
            BillingCountry = "Canada",
            Status = status,
            IsDeleted = isDeleted,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid()
        };
    }

    public static Venue CreateVenue(
        string? name = null,
        VenueType type = VenueType.ConferenceCenter,
        VenueStatus status = VenueStatus.Active,
        string? city = null,
        int maxCapacity = 500,
        bool isDeleted = false)
    {
        return new Venue
        {
            VenueId = Guid.NewGuid(),
            Name = name ?? $"Test Venue {Guid.NewGuid():N}",
            Description = "A test venue for events",
            Type = type,
            Street = "456 Event Ave",
            City = city ?? "Toronto",
            State = "ON",
            PostalCode = "M5V 3A1",
            Country = "Canada",
            MaxCapacity = maxCapacity,
            SeatedCapacity = maxCapacity / 2,
            StandingCapacity = maxCapacity,
            ContactName = "John Doe",
            ContactEmail = "contact@venue.com",
            ContactPhone = "555-987-6543",
            Status = status,
            IsDeleted = isDeleted,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid()
        };
    }

    public static EventType CreateEventType(
        string? name = null,
        string? description = null,
        bool isActive = true)
    {
        return new EventType
        {
            EventTypeId = Guid.NewGuid(),
            Name = name ?? $"Event Type {Guid.NewGuid():N}",
            Description = description ?? "A test event type",
            IsActive = isActive
        };
    }

    public static Event CreateEvent(
        Guid? venueId = null,
        Guid? eventTypeId = null,
        Guid? customerId = null,
        string? title = null,
        DateTime? eventDate = null,
        EventStatus status = EventStatus.Planned,
        bool isDeleted = false)
    {
        return new Event
        {
            EventId = Guid.NewGuid(),
            Title = title ?? $"Test Event {Guid.NewGuid():N}",
            Description = "A test event description",
            EventDate = eventDate ?? DateTime.UtcNow.AddDays(30),
            VenueId = venueId ?? Guid.NewGuid(),
            EventTypeId = eventTypeId ?? Guid.NewGuid(),
            CustomerId = customerId ?? Guid.NewGuid(),
            Status = status,
            IsDeleted = isDeleted,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid()
        };
    }

    public static StaffMember CreateStaffMember(
        string? firstName = null,
        string? lastName = null,
        string? email = null,
        StaffRole role = StaffRole.EventCoordinator,
        StaffStatus status = StaffStatus.Active,
        decimal? hourlyRate = 25.00m,
        bool isDeleted = false)
    {
        return new StaffMember
        {
            StaffMemberId = Guid.NewGuid(),
            FirstName = firstName ?? "John",
            LastName = lastName ?? $"Doe_{Guid.NewGuid():N}",
            Email = email ?? $"staff-{Guid.NewGuid():N}@example.com",
            PhoneNumber = "555-456-7890",
            PhotoUrl = "https://example.com/photo.jpg",
            Status = status,
            HireDate = DateTime.UtcNow.AddYears(-1),
            Role = role,
            HourlyRate = hourlyRate,
            IsDeleted = isDeleted,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid()
        };
    }

    public static EquipmentItem CreateEquipmentItem(
        string? name = null,
        EquipmentCategory category = EquipmentCategory.AudioVisual,
        EquipmentCondition condition = EquipmentCondition.Good,
        EquipmentStatus status = EquipmentStatus.Available,
        decimal purchasePrice = 1000.00m,
        bool isActive = true,
        bool isDeleted = false)
    {
        return new EquipmentItem
        {
            EquipmentItemId = Guid.NewGuid(),
            Name = name ?? $"Equipment {Guid.NewGuid():N}",
            Description = "Test equipment item",
            Category = category,
            Condition = condition,
            Status = status,
            PurchaseDate = DateTime.UtcNow.AddMonths(-6),
            PurchasePrice = purchasePrice,
            CurrentValue = purchasePrice * 0.8m,
            Manufacturer = "Test Manufacturer",
            Model = "Model X",
            SerialNumber = $"SN-{Guid.NewGuid():N}",
            WarehouseLocation = "Warehouse A",
            IsActive = isActive,
            IsDeleted = isDeleted,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid()
        };
    }

    public static User CreateUser(
        string? username = null,
        bool isDeleted = false)
    {
        return new User
        {
            UserId = Guid.NewGuid(),
            Username = username ?? $"user_{Guid.NewGuid():N}",
            Password = "hashedpassword123",
            Salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 },
            RefreshToken = null,
            IsDeleted = isDeleted,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Role CreateRole(string? name = null)
    {
        return new Role
        {
            RoleId = Guid.NewGuid(),
            Name = name ?? $"Role_{Guid.NewGuid():N}"
        };
    }

    public static Privilege CreatePrivilege(
        Guid roleId,
        string aggregate = "Events",
        AccessRight accessRight = AccessRight.Read)
    {
        return new Privilege
        {
            PrivilegeId = Guid.NewGuid(),
            RoleId = roleId,
            Aggregate = aggregate,
            AccessRight = accessRight
        };
    }
}
