// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.EventAggregate;
using EventManagementPlatform.Infrastructure.Tests.Fixtures;
using EventManagementPlatform.Infrastructure.Tests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Infrastructure.Tests.Entities;

[Collection("Database")]
public class EventTypeTests : IntegrationTestBase
{
    public EventTypeTests(SqlExpressDatabaseFixture fixture)
        : base(fixture)
    {
    }

    #region Create (Nominal)

    [Fact]
    public async Task CreateEventType_WithValidData_ShouldPersistToDatabase()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType(
            name: "Corporate Conference",
            description: "Business and corporate conferences"
        );

        // Act
        DbContext.EventTypes.Add(eventType);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedEventType = await verifyContext.EventTypes
            .FirstOrDefaultAsync(et => et.EventTypeId == eventType.EventTypeId);

        savedEventType.Should().NotBeNull();
        savedEventType!.Name.Should().Be("Corporate Conference");
        savedEventType.Description.Should().Be("Business and corporate conferences");
        savedEventType.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task CreateEventType_WithMinimalData_ShouldPersist()
    {
        // Arrange
        var eventType = new EventType
        {
            EventTypeId = Guid.NewGuid(),
            Name = "Simple Event"
        };

        // Act
        DbContext.EventTypes.Add(eventType);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedEventType = await verifyContext.EventTypes
            .FirstOrDefaultAsync(et => et.EventTypeId == eventType.EventTypeId);

        savedEventType.Should().NotBeNull();
        savedEventType!.Name.Should().Be("Simple Event");
        savedEventType.Description.Should().BeNull();
        savedEventType.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task CreateMultipleEventTypes_ShouldPersistAll()
    {
        // Arrange
        var eventTypes = new[]
        {
            TestDataFactory.CreateEventType(name: "Wedding"),
            TestDataFactory.CreateEventType(name: "Birthday Party"),
            TestDataFactory.CreateEventType(name: "Corporate Event")
        };

        // Act
        DbContext.EventTypes.AddRange(eventTypes);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedEventTypes = await verifyContext.EventTypes.ToListAsync();
        savedEventTypes.Should().HaveCount(3);
    }

    [Fact]
    public async Task CreateEventType_WithInactiveStatus_ShouldPersist()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType(
            name: "Inactive Event Type",
            isActive: false
        );

        // Act
        DbContext.EventTypes.Add(eventType);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedEventType = await verifyContext.EventTypes
            .FirstOrDefaultAsync(et => et.EventTypeId == eventType.EventTypeId);

        savedEventType.Should().NotBeNull();
        savedEventType!.IsActive.Should().BeFalse();
    }

    #endregion

    #region Read (Nominal)

    [Fact]
    public async Task GetEventTypeById_WhenExists_ShouldReturnEventType()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType(name: "Test Event Type");
        DbContext.EventTypes.Add(eventType);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var result = await queryContext.EventTypes
            .FirstOrDefaultAsync(et => et.EventTypeId == eventType.EventTypeId);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Event Type");
    }

    [Fact]
    public async Task GetEventTypes_WithActiveFilter_ShouldReturnActiveOnly()
    {
        // Arrange
        var activeType = TestDataFactory.CreateEventType(name: "Active Type", isActive: true);
        var inactiveType = TestDataFactory.CreateEventType(name: "Inactive Type", isActive: false);

        DbContext.EventTypes.AddRange(activeType, inactiveType);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var activeTypes = await queryContext.EventTypes
            .Where(et => et.IsActive)
            .ToListAsync();

        // Assert
        activeTypes.Should().HaveCount(1);
        activeTypes[0].Name.Should().Be("Active Type");
    }

    [Fact]
    public async Task GetEventTypes_WithNameSearch_ShouldReturnMatchingResults()
    {
        // Arrange
        var type1 = TestDataFactory.CreateEventType(name: "Wedding Ceremony");
        var type2 = TestDataFactory.CreateEventType(name: "Wedding Reception");
        var type3 = TestDataFactory.CreateEventType(name: "Birthday Party");

        DbContext.EventTypes.AddRange(type1, type2, type3);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var weddingTypes = await queryContext.EventTypes
            .Where(et => et.Name.Contains("Wedding"))
            .ToListAsync();

        // Assert
        weddingTypes.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetEventTypes_OrderedByName_ShouldReturnSorted()
    {
        // Arrange
        var typeC = TestDataFactory.CreateEventType(name: "Conference");
        var typeA = TestDataFactory.CreateEventType(name: "Anniversary");
        var typeB = TestDataFactory.CreateEventType(name: "Birthday");

        DbContext.EventTypes.AddRange(typeC, typeA, typeB);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var sortedTypes = await queryContext.EventTypes
            .OrderBy(et => et.Name)
            .ToListAsync();

        // Assert
        sortedTypes.Should().HaveCount(3);
        sortedTypes[0].Name.Should().Be("Anniversary");
        sortedTypes[1].Name.Should().Be("Birthday");
        sortedTypes[2].Name.Should().Be("Conference");
    }

    #endregion

    #region Update (Nominal)

    [Fact]
    public async Task UpdateEventType_ShouldPersistChanges()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType(name: "Original Name");
        DbContext.EventTypes.Add(eventType);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var typeToUpdate = await updateContext.EventTypes
            .FirstAsync(et => et.EventTypeId == eventType.EventTypeId);

        typeToUpdate.Name = "Updated Name";
        typeToUpdate.Description = "New description";
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedType = await verifyContext.EventTypes
            .FirstOrDefaultAsync(et => et.EventTypeId == eventType.EventTypeId);

        updatedType.Should().NotBeNull();
        updatedType!.Name.Should().Be("Updated Name");
        updatedType.Description.Should().Be("New description");
    }

    [Fact]
    public async Task DeactivateEventType_ShouldPersist()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType(isActive: true);
        DbContext.EventTypes.Add(eventType);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var typeToUpdate = await updateContext.EventTypes
            .FirstAsync(et => et.EventTypeId == eventType.EventTypeId);

        typeToUpdate.IsActive = false;
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedType = await verifyContext.EventTypes
            .FirstOrDefaultAsync(et => et.EventTypeId == eventType.EventTypeId);

        updatedType!.IsActive.Should().BeFalse();
    }

    #endregion

    #region Delete (Nominal)

    [Fact]
    public async Task DeleteEventType_WhenNoEvents_ShouldRemoveFromDatabase()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        DbContext.EventTypes.Add(eventType);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var typeToDelete = await deleteContext.EventTypes
            .FirstAsync(et => et.EventTypeId == eventType.EventTypeId);

        deleteContext.EventTypes.Remove(typeToDelete);
        await deleteContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var result = await verifyContext.EventTypes
            .FirstOrDefaultAsync(et => et.EventTypeId == eventType.EventTypeId);

        result.Should().BeNull();
    }

    #endregion

    #region Off-Nominal Cases

    [Fact]
    public async Task GetEventTypeById_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await DbContext.EventTypes
            .FirstOrDefaultAsync(et => et.EventTypeId == nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateEventType_WithDuplicateId_ShouldThrowException()
    {
        // Arrange
        var eventTypeId = Guid.NewGuid();
        var type1 = TestDataFactory.CreateEventType(name: "Type 1");
        type1.EventTypeId = eventTypeId;

        DbContext.EventTypes.Add(type1);
        await DbContext.SaveChangesAsync();

        // Act & Assert
        await using var context2 = CreateNewDbContext();
        var type2 = TestDataFactory.CreateEventType(name: "Type 2");
        type2.EventTypeId = eventTypeId;

        context2.EventTypes.Add(type2);

        await Assert.ThrowsAsync<DbUpdateException>(
            () => context2.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateEventType_WithDuplicateName_ShouldThrowException()
    {
        // Arrange
        var type1 = TestDataFactory.CreateEventType(name: "Unique Event Type");
        DbContext.EventTypes.Add(type1);
        await DbContext.SaveChangesAsync();

        // Act & Assert
        await using var context2 = CreateNewDbContext();
        var type2 = TestDataFactory.CreateEventType(name: "Unique Event Type"); // Same name
        context2.EventTypes.Add(type2);

        await Assert.ThrowsAsync<DbUpdateException>(
            () => context2.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateEventType_WithNullName_ShouldThrowException()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        eventType.Name = null!;

        // Act & Assert
        DbContext.EventTypes.Add(eventType);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateEventType_WithNameExceedingMaxLength_ShouldThrowException()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        eventType.Name = new string('E', 101); // Max is 100

        // Act & Assert
        DbContext.EventTypes.Add(eventType);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateEventType_WithDescriptionExceedingMaxLength_ShouldThrowException()
    {
        // Arrange
        var eventType = TestDataFactory.CreateEventType();
        eventType.Description = new string('D', 501); // Max is 500

        // Act & Assert
        DbContext.EventTypes.Add(eventType);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task UpdateEventTypeToExistingName_ShouldThrowException()
    {
        // Arrange
        var type1 = TestDataFactory.CreateEventType(name: "Existing Name");
        var type2 = TestDataFactory.CreateEventType(name: "Different Name");

        DbContext.EventTypes.AddRange(type1, type2);
        await DbContext.SaveChangesAsync();

        // Act & Assert
        await using var updateContext = CreateNewDbContext();
        var typeToUpdate = await updateContext.EventTypes
            .FirstAsync(et => et.EventTypeId == type2.EventTypeId);

        typeToUpdate.Name = "Existing Name"; // Try to change to existing name

        await Assert.ThrowsAsync<DbUpdateException>(
            () => updateContext.SaveChangesAsync());
    }

    [Fact]
    public async Task UpdateNonExistentEventType_ShouldThrowException()
    {
        // Arrange
        var nonExistentType = TestDataFactory.CreateEventType();

        // Act & Assert
        DbContext.EventTypes.Update(nonExistentType);
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
            () => DbContext.SaveChangesAsync());
    }

    #endregion
}
