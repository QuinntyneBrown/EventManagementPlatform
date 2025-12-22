// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.VenueAggregate;
using EventManagementPlatform.Infrastructure.Tests.Fixtures;
using EventManagementPlatform.Infrastructure.Tests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Infrastructure.Tests.Entities;

[Collection("Database")]
public class VenueTests : IntegrationTestBase
{
    public VenueTests(SqlExpressDatabaseFixture fixture)
        : base(fixture)
    {
    }

    #region Create (Nominal)

    [Fact]
    public async Task CreateVenue_WithValidData_ShouldPersistToDatabase()
    {
        // Arrange
        var venue = TestDataFactory.CreateVenue(
            name: "Grand Convention Center",
            type: VenueType.ConferenceCenter,
            city: "Toronto",
            maxCapacity: 1000
        );

        // Act
        DbContext.Venues.Add(venue);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedVenue = await verifyContext.Venues
            .FirstOrDefaultAsync(v => v.VenueId == venue.VenueId);

        savedVenue.Should().NotBeNull();
        savedVenue!.Name.Should().Be("Grand Convention Center");
        savedVenue.Type.Should().Be(VenueType.ConferenceCenter);
        savedVenue.City.Should().Be("Toronto");
        savedVenue.MaxCapacity.Should().Be(1000);
    }

    [Fact]
    public async Task CreateVenue_WithAllFields_ShouldPersistAllValues()
    {
        // Arrange
        var venue = new Venue
        {
            VenueId = Guid.NewGuid(),
            Name = "Luxury Hotel Ballroom",
            Description = "An elegant ballroom for special events",
            Type = VenueType.Hotel,
            Street = "100 King Street",
            City = "Montreal",
            State = "QC",
            PostalCode = "H3B 1A1",
            Country = "Canada",
            MaxCapacity = 500,
            SeatedCapacity = 300,
            StandingCapacity = 500,
            ContactName = "Jane Smith",
            ContactEmail = "events@luxuryhotel.com",
            ContactPhone = "514-555-1234",
            Status = VenueStatus.Active,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid()
        };

        // Act
        DbContext.Venues.Add(venue);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedVenue = await verifyContext.Venues
            .FirstOrDefaultAsync(v => v.VenueId == venue.VenueId);

        savedVenue.Should().NotBeNull();
        savedVenue!.Description.Should().Be("An elegant ballroom for special events");
        savedVenue.SeatedCapacity.Should().Be(300);
        savedVenue.ContactName.Should().Be("Jane Smith");
        savedVenue.ContactEmail.Should().Be("events@luxuryhotel.com");
    }

    [Fact]
    public async Task CreateMultipleVenues_ShouldPersistAll()
    {
        // Arrange
        var venues = new[]
        {
            TestDataFactory.CreateVenue(name: "Venue A", type: VenueType.ConferenceCenter),
            TestDataFactory.CreateVenue(name: "Venue B", type: VenueType.Hotel),
            TestDataFactory.CreateVenue(name: "Venue C", type: VenueType.Outdoor)
        };

        // Act
        DbContext.Venues.AddRange(venues);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedVenues = await verifyContext.Venues.ToListAsync();
        savedVenues.Should().HaveCount(3);
    }

    #endregion

    #region Read (Nominal)

    [Fact]
    public async Task GetVenueById_WhenExists_ShouldReturnVenue()
    {
        // Arrange
        var venue = TestDataFactory.CreateVenue(name: "Query Test Venue");
        DbContext.Venues.Add(venue);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var result = await queryContext.Venues
            .FirstOrDefaultAsync(v => v.VenueId == venue.VenueId);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Query Test Venue");
    }

    [Fact]
    public async Task GetVenues_WithStatusFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var activeVenue = TestDataFactory.CreateVenue(status: VenueStatus.Active);
        var inactiveVenue = TestDataFactory.CreateVenue(status: VenueStatus.Inactive);
        var pendingVenue = TestDataFactory.CreateVenue(status: VenueStatus.PendingApproval);

        DbContext.Venues.AddRange(activeVenue, inactiveVenue, pendingVenue);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var activeVenues = await queryContext.Venues
            .Where(v => v.Status == VenueStatus.Active)
            .ToListAsync();

        // Assert
        activeVenues.Should().HaveCount(1);
        activeVenues[0].VenueId.Should().Be(activeVenue.VenueId);
    }

    [Fact]
    public async Task GetVenues_WithTypeFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var conferenceVenue = TestDataFactory.CreateVenue(type: VenueType.ConferenceCenter);
        var hotelVenue = TestDataFactory.CreateVenue(type: VenueType.Hotel);
        var outdoorVenue = TestDataFactory.CreateVenue(type: VenueType.Outdoor);

        DbContext.Venues.AddRange(conferenceVenue, hotelVenue, outdoorVenue);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var conferenceVenues = await queryContext.Venues
            .Where(v => v.Type == VenueType.ConferenceCenter)
            .ToListAsync();

        // Assert
        conferenceVenues.Should().HaveCount(1);
        conferenceVenues[0].VenueId.Should().Be(conferenceVenue.VenueId);
    }

    [Fact]
    public async Task GetVenues_WithCityFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var torontoVenue = TestDataFactory.CreateVenue(city: "Toronto");
        var montrealVenue = TestDataFactory.CreateVenue(city: "Montreal");
        var vancouverVenue = TestDataFactory.CreateVenue(city: "Vancouver");

        DbContext.Venues.AddRange(torontoVenue, montrealVenue, vancouverVenue);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var torontoVenues = await queryContext.Venues
            .Where(v => v.City == "Toronto")
            .ToListAsync();

        // Assert
        torontoVenues.Should().HaveCount(1);
        torontoVenues[0].VenueId.Should().Be(torontoVenue.VenueId);
    }

    [Fact]
    public async Task GetVenues_WithCapacityFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var smallVenue = TestDataFactory.CreateVenue(name: "Small", maxCapacity: 100);
        var mediumVenue = TestDataFactory.CreateVenue(name: "Medium", maxCapacity: 500);
        var largeVenue = TestDataFactory.CreateVenue(name: "Large", maxCapacity: 2000);

        DbContext.Venues.AddRange(smallVenue, mediumVenue, largeVenue);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var largeVenues = await queryContext.Venues
            .Where(v => v.MaxCapacity >= 1000)
            .ToListAsync();

        // Assert
        largeVenues.Should().HaveCount(1);
        largeVenues[0].Name.Should().Be("Large");
    }

    [Fact]
    public async Task GetVenues_WithNameSearch_ShouldReturnMatchingResults()
    {
        // Arrange
        var venue1 = TestDataFactory.CreateVenue(name: "Downtown Conference Center");
        var venue2 = TestDataFactory.CreateVenue(name: "Uptown Conference Hall");
        var venue3 = TestDataFactory.CreateVenue(name: "Beach Resort");

        DbContext.Venues.AddRange(venue1, venue2, venue3);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var conferenceVenues = await queryContext.Venues
            .Where(v => v.Name.Contains("Conference"))
            .ToListAsync();

        // Assert
        conferenceVenues.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetVenues_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var venues = Enumerable.Range(1, 10)
            .Select(i => TestDataFactory.CreateVenue(name: $"Venue {i:D2}"))
            .ToList();

        DbContext.Venues.AddRange(venues);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var page1 = await queryContext.Venues
            .OrderBy(v => v.Name)
            .Skip(0)
            .Take(5)
            .ToListAsync();

        var page2 = await queryContext.Venues
            .OrderBy(v => v.Name)
            .Skip(5)
            .Take(5)
            .ToListAsync();

        // Assert
        page1.Should().HaveCount(5);
        page2.Should().HaveCount(5);
        page1.Select(v => v.VenueId).Should().NotIntersectWith(page2.Select(v => v.VenueId));
    }

    #endregion

    #region Update (Nominal)

    [Fact]
    public async Task UpdateVenue_ShouldPersistChanges()
    {
        // Arrange
        var venue = TestDataFactory.CreateVenue(name: "Original Venue Name");
        DbContext.Venues.Add(venue);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var venueToUpdate = await updateContext.Venues
            .FirstAsync(v => v.VenueId == venue.VenueId);

        venueToUpdate.Name = "Updated Venue Name";
        venueToUpdate.MaxCapacity = 750;
        venueToUpdate.ModifiedAt = DateTime.UtcNow;
        venueToUpdate.ModifiedBy = Guid.NewGuid();
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedVenue = await verifyContext.Venues
            .FirstOrDefaultAsync(v => v.VenueId == venue.VenueId);

        updatedVenue.Should().NotBeNull();
        updatedVenue!.Name.Should().Be("Updated Venue Name");
        updatedVenue.MaxCapacity.Should().Be(750);
        updatedVenue.ModifiedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateVenueStatus_ShouldPersistNewStatus()
    {
        // Arrange
        var venue = TestDataFactory.CreateVenue(status: VenueStatus.PendingApproval);
        DbContext.Venues.Add(venue);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var venueToUpdate = await updateContext.Venues
            .FirstAsync(v => v.VenueId == venue.VenueId);

        venueToUpdate.Status = VenueStatus.Active;
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedVenue = await verifyContext.Venues
            .FirstOrDefaultAsync(v => v.VenueId == venue.VenueId);

        updatedVenue!.Status.Should().Be(VenueStatus.Active);
    }

    [Fact]
    public async Task UpdateVenueContactInfo_ShouldPersistChanges()
    {
        // Arrange
        var venue = TestDataFactory.CreateVenue();
        DbContext.Venues.Add(venue);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var venueToUpdate = await updateContext.Venues
            .FirstAsync(v => v.VenueId == venue.VenueId);

        venueToUpdate.ContactName = "New Contact";
        venueToUpdate.ContactEmail = "newcontact@venue.com";
        venueToUpdate.ContactPhone = "555-999-8888";
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedVenue = await verifyContext.Venues
            .FirstOrDefaultAsync(v => v.VenueId == venue.VenueId);

        updatedVenue!.ContactName.Should().Be("New Contact");
        updatedVenue.ContactEmail.Should().Be("newcontact@venue.com");
        updatedVenue.ContactPhone.Should().Be("555-999-8888");
    }

    #endregion

    #region Delete (Nominal)

    [Fact]
    public async Task SoftDeleteVenue_ShouldNotAppearInQueries()
    {
        // Arrange
        var venue = TestDataFactory.CreateVenue();
        DbContext.Venues.Add(venue);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var venueToDelete = await deleteContext.Venues
            .FirstAsync(v => v.VenueId == venue.VenueId);

        venueToDelete.IsDeleted = true;
        await deleteContext.SaveChangesAsync();

        // Assert - Query filter should exclude soft-deleted records
        await using var verifyContext = CreateNewDbContext();
        var result = await verifyContext.Venues
            .FirstOrDefaultAsync(v => v.VenueId == venue.VenueId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task SoftDeleteVenue_ShouldStillExistWithIgnoreQueryFilters()
    {
        // Arrange
        var venue = TestDataFactory.CreateVenue();
        DbContext.Venues.Add(venue);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var venueToDelete = await deleteContext.Venues
            .FirstAsync(v => v.VenueId == venue.VenueId);

        venueToDelete.IsDeleted = true;
        await deleteContext.SaveChangesAsync();

        // Assert - Should still exist when ignoring query filters
        await using var verifyContext = CreateNewDbContext();
        var result = await verifyContext.Venues
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(v => v.VenueId == venue.VenueId);

        result.Should().NotBeNull();
        result!.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task HardDeleteVenue_ShouldRemoveFromDatabase()
    {
        // Arrange
        var venue = TestDataFactory.CreateVenue();
        DbContext.Venues.Add(venue);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var venueToDelete = await deleteContext.Venues
            .FirstAsync(v => v.VenueId == venue.VenueId);

        deleteContext.Venues.Remove(venueToDelete);
        await deleteContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var result = await verifyContext.Venues
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(v => v.VenueId == venue.VenueId);

        result.Should().BeNull();
    }

    #endregion

    #region Off-Nominal Cases

    [Fact]
    public async Task GetVenueById_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await DbContext.Venues
            .FirstOrDefaultAsync(v => v.VenueId == nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateVenue_WithDuplicateId_ShouldThrowException()
    {
        // Arrange
        var venueId = Guid.NewGuid();
        var venue1 = TestDataFactory.CreateVenue();
        venue1.VenueId = venueId;

        DbContext.Venues.Add(venue1);
        await DbContext.SaveChangesAsync();

        // Act & Assert
        await using var context2 = CreateNewDbContext();
        var venue2 = TestDataFactory.CreateVenue();
        venue2.VenueId = venueId;

        context2.Venues.Add(venue2);

        await Assert.ThrowsAsync<DbUpdateException>(
            () => context2.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateVenue_WithNullName_ShouldThrowException()
    {
        // Arrange
        var venue = TestDataFactory.CreateVenue();
        venue.Name = null!;

        // Act & Assert
        DbContext.Venues.Add(venue);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateVenue_WithNameExceedingMaxLength_ShouldThrowException()
    {
        // Arrange
        var venue = TestDataFactory.CreateVenue();
        venue.Name = new string('V', 201); // Max is 200

        // Act & Assert
        DbContext.Venues.Add(venue);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateVenue_WithDescriptionExceedingMaxLength_ShouldThrowException()
    {
        // Arrange
        var venue = TestDataFactory.CreateVenue();
        venue.Description = new string('D', 2001); // Max is 2000

        // Act & Assert
        DbContext.Venues.Add(venue);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateVenue_WithNegativeCapacity_ShouldPersist()
    {
        // Note: Database doesn't have a constraint for negative capacity
        // This test verifies the current behavior (no constraint)
        // Arrange
        var venue = TestDataFactory.CreateVenue(maxCapacity: -1);

        // Act
        DbContext.Venues.Add(venue);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedVenue = await verifyContext.Venues
            .FirstOrDefaultAsync(v => v.VenueId == venue.VenueId);

        savedVenue.Should().NotBeNull();
        savedVenue!.MaxCapacity.Should().Be(-1);
    }

    [Fact]
    public async Task GetDeletedVenues_WithoutIgnoreQueryFilters_ShouldReturnEmpty()
    {
        // Arrange
        var deletedVenue = TestDataFactory.CreateVenue(isDeleted: true);
        var activeVenue = TestDataFactory.CreateVenue(isDeleted: false);

        // Need to bypass query filter for inserting deleted record
        await DbContext.Database.ExecuteSqlRawAsync(
            @"INSERT INTO Venues (VenueId, Name, Type, Street, City, State, PostalCode, Country,
              MaxCapacity, Status, IsDeleted, CreatedAt, CreatedBy)
              VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12})",
            deletedVenue.VenueId, deletedVenue.Name, (int)deletedVenue.Type,
            deletedVenue.Street, deletedVenue.City, deletedVenue.State, deletedVenue.PostalCode,
            deletedVenue.Country, deletedVenue.MaxCapacity, (int)deletedVenue.Status,
            true, deletedVenue.CreatedAt, deletedVenue.CreatedBy);

        DbContext.Venues.Add(activeVenue);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var venues = await queryContext.Venues.ToListAsync();

        // Assert
        venues.Should().HaveCount(1);
        venues[0].VenueId.Should().Be(activeVenue.VenueId);
    }

    [Fact]
    public async Task UpdateNonExistentVenue_ShouldThrowException()
    {
        // Arrange
        var nonExistentVenue = TestDataFactory.CreateVenue();

        // Act & Assert
        DbContext.Venues.Update(nonExistentVenue);
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
            () => DbContext.SaveChangesAsync());
    }

    #endregion
}
