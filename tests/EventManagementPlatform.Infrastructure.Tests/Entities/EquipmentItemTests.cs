// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.EquipmentAggregate;
using EventManagementPlatform.Infrastructure.Tests.Fixtures;
using EventManagementPlatform.Infrastructure.Tests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Infrastructure.Tests.Entities;

[Collection("Database")]
public class EquipmentItemTests : IntegrationTestBase
{
    public EquipmentItemTests(SqlExpressDatabaseFixture fixture)
        : base(fixture)
    {
    }

    #region Create (Nominal)

    [Fact]
    public async Task CreateEquipmentItem_WithValidData_ShouldPersistToDatabase()
    {
        // Arrange
        var equipment = TestDataFactory.CreateEquipmentItem(
            name: "Professional Sound System",
            category: EquipmentCategory.AudioVisual,
            condition: EquipmentCondition.Excellent,
            purchasePrice: 5000.00m);

        // Act
        DbContext.EquipmentItems.Add(equipment);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedEquipment = await verifyContext.EquipmentItems
            .FirstOrDefaultAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        savedEquipment.Should().NotBeNull();
        savedEquipment!.Name.Should().Be("Professional Sound System");
        savedEquipment.Category.Should().Be(EquipmentCategory.AudioVisual);
        savedEquipment.Condition.Should().Be(EquipmentCondition.Excellent);
        savedEquipment.PurchasePrice.Should().Be(5000.00m);
        savedEquipment.Status.Should().Be(EquipmentStatus.Available);
    }

    [Fact]
    public async Task CreateEquipmentItem_WithAllFields_ShouldPersistAllValues()
    {
        // Arrange
        var equipment = new EquipmentItem
        {
            EquipmentItemId = Guid.NewGuid(),
            Name = "LED Stage Lighting Set",
            Description = "Professional LED lighting for stage events",
            Category = EquipmentCategory.Lighting,
            Condition = EquipmentCondition.Good,
            Status = EquipmentStatus.Available,
            PurchaseDate = new DateTime(2023, 1, 15, 0, 0, 0, DateTimeKind.Utc),
            PurchasePrice = 3500.00m,
            CurrentValue = 2800.00m,
            Manufacturer = "ChauvetDJ",
            Model = "LED Par Pro",
            SerialNumber = "CDJ-2023-001234",
            WarehouseLocation = "Warehouse B, Shelf 12",
            IsActive = true,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid()
        };

        // Act
        DbContext.EquipmentItems.Add(equipment);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedEquipment = await verifyContext.EquipmentItems
            .FirstOrDefaultAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        savedEquipment.Should().NotBeNull();
        savedEquipment!.Description.Should().Be("Professional LED lighting for stage events");
        savedEquipment.Manufacturer.Should().Be("ChauvetDJ");
        savedEquipment.Model.Should().Be("LED Par Pro");
        savedEquipment.SerialNumber.Should().Be("CDJ-2023-001234");
        savedEquipment.WarehouseLocation.Should().Be("Warehouse B, Shelf 12");
        savedEquipment.CurrentValue.Should().Be(2800.00m);
    }

    [Fact]
    public async Task CreateMultipleEquipmentItems_ShouldPersistAll()
    {
        // Arrange
        var equipmentItems = new[]
        {
            TestDataFactory.CreateEquipmentItem(name: "Table Set A", category: EquipmentCategory.Table),
            TestDataFactory.CreateEquipmentItem(name: "Chair Set B", category: EquipmentCategory.Seating),
            TestDataFactory.CreateEquipmentItem(name: "Decoration Kit C", category: EquipmentCategory.Decoration)
        };

        // Act
        DbContext.EquipmentItems.AddRange(equipmentItems);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedItems = await verifyContext.EquipmentItems.ToListAsync();
        savedItems.Should().HaveCount(3);
    }

    [Fact]
    public async Task CreateEquipmentItem_WithNullOptionalFields_ShouldPersist()
    {
        // Arrange
        var equipment = new EquipmentItem
        {
            EquipmentItemId = Guid.NewGuid(),
            Name = "Basic Equipment",
            Category = EquipmentCategory.Other,
            Condition = EquipmentCondition.Good,
            Status = EquipmentStatus.Available,
            PurchaseDate = DateTime.UtcNow,
            PurchasePrice = 100.00m,

            // Optional fields left as null
            Description = null,
            Manufacturer = null,
            Model = null,
            SerialNumber = null,
            WarehouseLocation = null,
            CurrentValue = null,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid()
        };

        // Act
        DbContext.EquipmentItems.Add(equipment);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedEquipment = await verifyContext.EquipmentItems
            .FirstOrDefaultAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        savedEquipment.Should().NotBeNull();
        savedEquipment!.Description.Should().BeNull();
        savedEquipment.Manufacturer.Should().BeNull();
        savedEquipment.CurrentValue.Should().BeNull();
    }

    #endregion

    #region Read (Nominal)

    [Fact]
    public async Task GetEquipmentItemById_WhenExists_ShouldReturnEquipmentItem()
    {
        // Arrange
        var equipment = TestDataFactory.CreateEquipmentItem(name: "Query Test Equipment");
        DbContext.EquipmentItems.Add(equipment);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var result = await queryContext.EquipmentItems
            .FirstOrDefaultAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Query Test Equipment");
    }

    [Fact]
    public async Task GetEquipmentItems_WithCategoryFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var audioEquipment = TestDataFactory.CreateEquipmentItem(category: EquipmentCategory.AudioVisual);
        var lightingEquipment = TestDataFactory.CreateEquipmentItem(category: EquipmentCategory.Lighting);
        var tableEquipment = TestDataFactory.CreateEquipmentItem(category: EquipmentCategory.Table);

        DbContext.EquipmentItems.AddRange(audioEquipment, lightingEquipment, tableEquipment);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var audioItems = await queryContext.EquipmentItems
            .Where(e => e.Category == EquipmentCategory.AudioVisual)
            .ToListAsync();

        // Assert
        audioItems.Should().HaveCount(1);
        audioItems[0].EquipmentItemId.Should().Be(audioEquipment.EquipmentItemId);
    }

    [Fact]
    public async Task GetEquipmentItems_WithStatusFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var availableItem = TestDataFactory.CreateEquipmentItem(status: EquipmentStatus.Available);
        var reservedItem = TestDataFactory.CreateEquipmentItem(status: EquipmentStatus.Reserved);
        var maintenanceItem = TestDataFactory.CreateEquipmentItem(status: EquipmentStatus.InMaintenance);

        DbContext.EquipmentItems.AddRange(availableItem, reservedItem, maintenanceItem);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var availableItems = await queryContext.EquipmentItems
            .Where(e => e.Status == EquipmentStatus.Available)
            .ToListAsync();

        // Assert
        availableItems.Should().HaveCount(1);
        availableItems[0].EquipmentItemId.Should().Be(availableItem.EquipmentItemId);
    }

    [Fact]
    public async Task GetEquipmentItems_WithConditionFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var excellentItem = TestDataFactory.CreateEquipmentItem(condition: EquipmentCondition.Excellent);
        var goodItem = TestDataFactory.CreateEquipmentItem(condition: EquipmentCondition.Good);
        var needsRepairItem = TestDataFactory.CreateEquipmentItem(condition: EquipmentCondition.NeedsRepair);

        DbContext.EquipmentItems.AddRange(excellentItem, goodItem, needsRepairItem);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var itemsNeedingAttention = await queryContext.EquipmentItems
            .Where(e => e.Condition == EquipmentCondition.NeedsRepair || e.Condition == EquipmentCondition.Poor)
            .ToListAsync();

        // Assert
        itemsNeedingAttention.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetEquipmentItems_WithNameSearch_ShouldReturnMatchingResults()
    {
        // Arrange
        var item1 = TestDataFactory.CreateEquipmentItem(name: "Professional Audio Mixer");
        var item2 = TestDataFactory.CreateEquipmentItem(name: "Audio Speaker System");
        var item3 = TestDataFactory.CreateEquipmentItem(name: "LED Light Panel");

        DbContext.EquipmentItems.AddRange(item1, item2, item3);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var audioItems = await queryContext.EquipmentItems
            .Where(e => e.Name.Contains("Audio"))
            .ToListAsync();

        // Assert
        audioItems.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetEquipmentItems_WithValueRange_ShouldReturnFilteredResults()
    {
        // Arrange
        var cheapItem = TestDataFactory.CreateEquipmentItem(name: "Cheap", purchasePrice: 100.00m);
        var midItem = TestDataFactory.CreateEquipmentItem(name: "Mid", purchasePrice: 1000.00m);
        var expensiveItem = TestDataFactory.CreateEquipmentItem(name: "Expensive", purchasePrice: 10000.00m);

        DbContext.EquipmentItems.AddRange(cheapItem, midItem, expensiveItem);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var expensiveItems = await queryContext.EquipmentItems
            .Where(e => e.PurchasePrice >= 5000.00m)
            .ToListAsync();

        // Assert
        expensiveItems.Should().HaveCount(1);
        expensiveItems[0].Name.Should().Be("Expensive");
    }

    [Fact]
    public async Task GetEquipmentItems_WithWarehouseLocationFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var item1 = TestDataFactory.CreateEquipmentItem(name: "Item 1");
        item1.WarehouseLocation = "Warehouse A";
        var item2 = TestDataFactory.CreateEquipmentItem(name: "Item 2");
        item2.WarehouseLocation = "Warehouse B";
        var item3 = TestDataFactory.CreateEquipmentItem(name: "Item 3");
        item3.WarehouseLocation = "Warehouse A";

        DbContext.EquipmentItems.AddRange(item1, item2, item3);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var warehouseAItems = await queryContext.EquipmentItems
            .Where(e => e.WarehouseLocation == "Warehouse A")
            .ToListAsync();

        // Assert
        warehouseAItems.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetEquipmentItems_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var items = Enumerable.Range(1, 10)
            .Select(i => TestDataFactory.CreateEquipmentItem(name: $"Equipment {i:D2}"))
            .ToList();

        DbContext.EquipmentItems.AddRange(items);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var page1 = await queryContext.EquipmentItems
            .OrderBy(e => e.Name)
            .Skip(0)
            .Take(5)
            .ToListAsync();

        var page2 = await queryContext.EquipmentItems
            .OrderBy(e => e.Name)
            .Skip(5)
            .Take(5)
            .ToListAsync();

        // Assert
        page1.Should().HaveCount(5);
        page2.Should().HaveCount(5);
        page1.Select(e => e.EquipmentItemId).Should().NotIntersectWith(page2.Select(e => e.EquipmentItemId));
    }

    [Fact]
    public async Task GetActiveEquipmentItems_ShouldReturnOnlyActive()
    {
        // Arrange
        var activeItem = TestDataFactory.CreateEquipmentItem(name: "Active", isActive: true);
        var inactiveItem = TestDataFactory.CreateEquipmentItem(name: "Inactive", isActive: false);

        DbContext.EquipmentItems.AddRange(activeItem, inactiveItem);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var activeItems = await queryContext.EquipmentItems
            .Where(e => e.IsActive)
            .ToListAsync();

        // Assert
        activeItems.Should().HaveCount(1);
        activeItems[0].Name.Should().Be("Active");
    }

    #endregion

    #region Update (Nominal)

    [Fact]
    public async Task UpdateEquipmentItem_ShouldPersistChanges()
    {
        // Arrange
        var equipment = TestDataFactory.CreateEquipmentItem(name: "Original Name");
        DbContext.EquipmentItems.Add(equipment);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var itemToUpdate = await updateContext.EquipmentItems
            .FirstAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        itemToUpdate.Name = "Updated Name";
        itemToUpdate.Description = "New description";
        itemToUpdate.ModifiedAt = DateTime.UtcNow;
        itemToUpdate.ModifiedBy = Guid.NewGuid();
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedItem = await verifyContext.EquipmentItems
            .FirstOrDefaultAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        updatedItem.Should().NotBeNull();
        updatedItem!.Name.Should().Be("Updated Name");
        updatedItem.Description.Should().Be("New description");
        updatedItem.ModifiedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateEquipmentItemStatus_ShouldPersistNewStatus()
    {
        // Arrange
        var equipment = TestDataFactory.CreateEquipmentItem(status: EquipmentStatus.Available);
        DbContext.EquipmentItems.Add(equipment);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var itemToUpdate = await updateContext.EquipmentItems
            .FirstAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        itemToUpdate.Status = EquipmentStatus.Reserved;
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedItem = await verifyContext.EquipmentItems
            .FirstOrDefaultAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        updatedItem!.Status.Should().Be(EquipmentStatus.Reserved);
    }

    [Fact]
    public async Task UpdateEquipmentItemCondition_ShouldPersistNewCondition()
    {
        // Arrange
        var equipment = TestDataFactory.CreateEquipmentItem(condition: EquipmentCondition.Good);
        DbContext.EquipmentItems.Add(equipment);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var itemToUpdate = await updateContext.EquipmentItems
            .FirstAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        itemToUpdate.Condition = EquipmentCondition.NeedsRepair;
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedItem = await verifyContext.EquipmentItems
            .FirstOrDefaultAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        updatedItem!.Condition.Should().Be(EquipmentCondition.NeedsRepair);
    }

    [Fact]
    public async Task UpdateEquipmentItemCurrentValue_ShouldPersistNewValue()
    {
        // Arrange
        var equipment = TestDataFactory.CreateEquipmentItem(purchasePrice: 1000.00m);
        equipment.CurrentValue = 800.00m;
        DbContext.EquipmentItems.Add(equipment);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var itemToUpdate = await updateContext.EquipmentItems
            .FirstAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        itemToUpdate.CurrentValue = 600.00m;
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedItem = await verifyContext.EquipmentItems
            .FirstOrDefaultAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        updatedItem!.CurrentValue.Should().Be(600.00m);
    }

    [Fact]
    public async Task DeactivateEquipmentItem_ShouldPersist()
    {
        // Arrange
        var equipment = TestDataFactory.CreateEquipmentItem(isActive: true);
        DbContext.EquipmentItems.Add(equipment);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var itemToUpdate = await updateContext.EquipmentItems
            .FirstAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        itemToUpdate.IsActive = false;
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedItem = await verifyContext.EquipmentItems
            .FirstOrDefaultAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        updatedItem!.IsActive.Should().BeFalse();
    }

    #endregion

    #region Delete (Nominal)

    [Fact]
    public async Task SoftDeleteEquipmentItem_ShouldNotAppearInQueries()
    {
        // Arrange
        var equipment = TestDataFactory.CreateEquipmentItem();
        DbContext.EquipmentItems.Add(equipment);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var itemToDelete = await deleteContext.EquipmentItems
            .FirstAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        itemToDelete.IsDeleted = true;
        await deleteContext.SaveChangesAsync();

        // Assert - Query filter should exclude soft-deleted records
        await using var verifyContext = CreateNewDbContext();
        var result = await verifyContext.EquipmentItems
            .FirstOrDefaultAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task SoftDeleteEquipmentItem_ShouldStillExistWithIgnoreQueryFilters()
    {
        // Arrange
        var equipment = TestDataFactory.CreateEquipmentItem();
        DbContext.EquipmentItems.Add(equipment);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var itemToDelete = await deleteContext.EquipmentItems
            .FirstAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        itemToDelete.IsDeleted = true;
        await deleteContext.SaveChangesAsync();

        // Assert - Should still exist when ignoring query filters
        await using var verifyContext = CreateNewDbContext();
        var result = await verifyContext.EquipmentItems
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        result.Should().NotBeNull();
        result!.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task HardDeleteEquipmentItem_ShouldRemoveFromDatabase()
    {
        // Arrange
        var equipment = TestDataFactory.CreateEquipmentItem();
        DbContext.EquipmentItems.Add(equipment);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var itemToDelete = await deleteContext.EquipmentItems
            .FirstAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        deleteContext.EquipmentItems.Remove(itemToDelete);
        await deleteContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var result = await verifyContext.EquipmentItems
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        result.Should().BeNull();
    }

    #endregion

    #region Off-Nominal Cases

    [Fact]
    public async Task GetEquipmentItemById_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await DbContext.EquipmentItems
            .FirstOrDefaultAsync(e => e.EquipmentItemId == nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateEquipmentItem_WithDuplicateId_ShouldThrowException()
    {
        // Arrange
        var equipmentId = Guid.NewGuid();
        var item1 = TestDataFactory.CreateEquipmentItem(name: "Item 1");
        item1.EquipmentItemId = equipmentId;

        DbContext.EquipmentItems.Add(item1);
        await DbContext.SaveChangesAsync();

        // Act & Assert
        await using var context2 = CreateNewDbContext();
        var item2 = TestDataFactory.CreateEquipmentItem(name: "Item 2");
        item2.EquipmentItemId = equipmentId;

        context2.EquipmentItems.Add(item2);

        await Assert.ThrowsAsync<DbUpdateException>(
            () => context2.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateEquipmentItem_WithNullName_ShouldThrowException()
    {
        // Arrange
        var equipment = TestDataFactory.CreateEquipmentItem();
        equipment.Name = null!;

        // Act & Assert
        DbContext.EquipmentItems.Add(equipment);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateEquipmentItem_WithNameExceedingMaxLength_ShouldThrowException()
    {
        // Arrange
        var equipment = TestDataFactory.CreateEquipmentItem();
        equipment.Name = new string('E', 201); // Max is 200

        // Act & Assert
        DbContext.EquipmentItems.Add(equipment);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateEquipmentItem_WithDescriptionExceedingMaxLength_ShouldThrowException()
    {
        // Arrange
        var equipment = TestDataFactory.CreateEquipmentItem();
        equipment.Description = new string('D', 2001); // Max is 2000

        // Act & Assert
        DbContext.EquipmentItems.Add(equipment);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateEquipmentItem_WithSerialNumberExceedingMaxLength_ShouldThrowException()
    {
        // Arrange
        var equipment = TestDataFactory.CreateEquipmentItem();
        equipment.SerialNumber = new string('S', 101); // Max is 100

        // Act & Assert
        DbContext.EquipmentItems.Add(equipment);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateEquipmentItem_WithNegativePurchasePrice_ShouldPersist()
    {
        // Note: Database doesn't have a constraint for negative prices
        // This test verifies the current behavior (no constraint)
        // Arrange
        var equipment = TestDataFactory.CreateEquipmentItem(purchasePrice: -100.00m);

        // Act
        DbContext.EquipmentItems.Add(equipment);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedEquipment = await verifyContext.EquipmentItems
            .FirstOrDefaultAsync(e => e.EquipmentItemId == equipment.EquipmentItemId);

        savedEquipment.Should().NotBeNull();
        savedEquipment!.PurchasePrice.Should().Be(-100.00m);
    }

    [Fact]
    public async Task UpdateNonExistentEquipmentItem_ShouldThrowException()
    {
        // Arrange
        var nonExistentItem = TestDataFactory.CreateEquipmentItem();

        // Act & Assert
        DbContext.EquipmentItems.Update(nonExistentItem);
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task GetDeletedEquipmentItems_WithoutIgnoreQueryFilters_ShouldReturnEmpty()
    {
        // Arrange
        var deletedItem = TestDataFactory.CreateEquipmentItem(isDeleted: true);
        var activeItem = TestDataFactory.CreateEquipmentItem(isDeleted: false);

        // Need to bypass query filter for inserting deleted record
        await DbContext.Database.ExecuteSqlRawAsync(
            @"INSERT INTO EquipmentItems (EquipmentItemId, Name, Category, Condition, Status,
              PurchaseDate, PurchasePrice, IsActive, IsDeleted, CreatedAt, CreatedBy)
              VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})",
            deletedItem.EquipmentItemId, deletedItem.Name, (int)deletedItem.Category,
            (int)deletedItem.Condition, (int)deletedItem.Status, deletedItem.PurchaseDate,
            deletedItem.PurchasePrice, deletedItem.IsActive, true,
            deletedItem.CreatedAt, deletedItem.CreatedBy);

        DbContext.EquipmentItems.Add(activeItem);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var items = await queryContext.EquipmentItems.ToListAsync();

        // Assert
        items.Should().HaveCount(1);
        items[0].EquipmentItemId.Should().Be(activeItem.EquipmentItemId);
    }

    #endregion
}
