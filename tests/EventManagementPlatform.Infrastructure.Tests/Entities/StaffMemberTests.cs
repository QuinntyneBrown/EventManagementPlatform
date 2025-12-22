// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.StaffAggregate;
using EventManagementPlatform.Infrastructure.Tests.Fixtures;
using EventManagementPlatform.Infrastructure.Tests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Infrastructure.Tests.Entities;

[Collection("Database")]
public class StaffMemberTests : IntegrationTestBase
{
    public StaffMemberTests(SqlExpressDatabaseFixture fixture) : base(fixture)
    {
    }

    #region Create (Nominal)

    [Fact]
    public async Task CreateStaffMember_WithValidData_ShouldPersistToDatabase()
    {
        // Arrange
        var staffMember = TestDataFactory.CreateStaffMember(
            firstName: "John",
            lastName: "Smith",
            email: "john.smith@events.com",
            role: StaffRole.EventCoordinator
        );

        // Act
        DbContext.StaffMembers.Add(staffMember);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedStaff = await verifyContext.StaffMembers
            .FirstOrDefaultAsync(s => s.StaffMemberId == staffMember.StaffMemberId);

        savedStaff.Should().NotBeNull();
        savedStaff!.FirstName.Should().Be("John");
        savedStaff.LastName.Should().Be("Smith");
        savedStaff.Email.Should().Be("john.smith@events.com");
        savedStaff.Role.Should().Be(StaffRole.EventCoordinator);
        savedStaff.Status.Should().Be(StaffStatus.Active);
    }

    [Fact]
    public async Task CreateStaffMember_WithAllFields_ShouldPersistAllValues()
    {
        // Arrange
        var staffMember = new StaffMember
        {
            StaffMemberId = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@events.com",
            PhoneNumber = "555-123-4567",
            PhotoUrl = "https://example.com/photos/jane.jpg",
            Status = StaffStatus.Active,
            HireDate = new DateTime(2020, 1, 15, 0, 0, 0, DateTimeKind.Utc),
            Role = StaffRole.Manager,
            HourlyRate = 45.00m,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid()
        };

        // Act
        DbContext.StaffMembers.Add(staffMember);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedStaff = await verifyContext.StaffMembers
            .FirstOrDefaultAsync(s => s.StaffMemberId == staffMember.StaffMemberId);

        savedStaff.Should().NotBeNull();
        savedStaff!.PhotoUrl.Should().Be("https://example.com/photos/jane.jpg");
        savedStaff.HireDate.Should().Be(new DateTime(2020, 1, 15, 0, 0, 0, DateTimeKind.Utc));
        savedStaff.HourlyRate.Should().Be(45.00m);
        savedStaff.Role.Should().Be(StaffRole.Manager);
    }

    [Fact]
    public async Task CreateMultipleStaffMembers_ShouldPersistAll()
    {
        // Arrange
        var staffMembers = new[]
        {
            TestDataFactory.CreateStaffMember(firstName: "Alice", role: StaffRole.EventCoordinator),
            TestDataFactory.CreateStaffMember(firstName: "Bob", role: StaffRole.SetupCrew),
            TestDataFactory.CreateStaffMember(firstName: "Charlie", role: StaffRole.Server)
        };

        // Act
        DbContext.StaffMembers.AddRange(staffMembers);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedStaff = await verifyContext.StaffMembers.ToListAsync();
        savedStaff.Should().HaveCount(3);
    }

    [Fact]
    public async Task CreateStaffMember_WithNullHourlyRate_ShouldPersist()
    {
        // Arrange
        var staffMember = TestDataFactory.CreateStaffMember(hourlyRate: null);

        // Act
        DbContext.StaffMembers.Add(staffMember);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedStaff = await verifyContext.StaffMembers
            .FirstOrDefaultAsync(s => s.StaffMemberId == staffMember.StaffMemberId);

        savedStaff.Should().NotBeNull();
        savedStaff!.HourlyRate.Should().BeNull();
    }

    #endregion

    #region Read (Nominal)

    [Fact]
    public async Task GetStaffMemberById_WhenExists_ShouldReturnStaffMember()
    {
        // Arrange
        var staffMember = TestDataFactory.CreateStaffMember(firstName: "Query", lastName: "Test");
        DbContext.StaffMembers.Add(staffMember);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var result = await queryContext.StaffMembers
            .FirstOrDefaultAsync(s => s.StaffMemberId == staffMember.StaffMemberId);

        // Assert
        result.Should().NotBeNull();
        result!.FirstName.Should().Be("Query");
        result.LastName.Should().Be("Test");
    }

    [Fact]
    public async Task GetStaffMembers_WithStatusFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var activeStaff = TestDataFactory.CreateStaffMember(status: StaffStatus.Active);
        var inactiveStaff = TestDataFactory.CreateStaffMember(status: StaffStatus.Inactive);
        var onLeaveStaff = TestDataFactory.CreateStaffMember(status: StaffStatus.OnLeave);

        DbContext.StaffMembers.AddRange(activeStaff, inactiveStaff, onLeaveStaff);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var activeStaffMembers = await queryContext.StaffMembers
            .Where(s => s.Status == StaffStatus.Active)
            .ToListAsync();

        // Assert
        activeStaffMembers.Should().HaveCount(1);
        activeStaffMembers[0].StaffMemberId.Should().Be(activeStaff.StaffMemberId);
    }

    [Fact]
    public async Task GetStaffMembers_WithRoleFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var coordinator = TestDataFactory.CreateStaffMember(role: StaffRole.EventCoordinator);
        var server = TestDataFactory.CreateStaffMember(role: StaffRole.Server);
        var bartender = TestDataFactory.CreateStaffMember(role: StaffRole.Bartender);

        DbContext.StaffMembers.AddRange(coordinator, server, bartender);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var coordinators = await queryContext.StaffMembers
            .Where(s => s.Role == StaffRole.EventCoordinator)
            .ToListAsync();

        // Assert
        coordinators.Should().HaveCount(1);
        coordinators[0].StaffMemberId.Should().Be(coordinator.StaffMemberId);
    }

    [Fact]
    public async Task GetStaffMembers_WithNameSearch_ShouldReturnMatchingResults()
    {
        // Arrange
        var staff1 = TestDataFactory.CreateStaffMember(firstName: "John", lastName: "Smith");
        var staff2 = TestDataFactory.CreateStaffMember(firstName: "Johnny", lastName: "Doe");
        var staff3 = TestDataFactory.CreateStaffMember(firstName: "Jane", lastName: "Johnson");

        DbContext.StaffMembers.AddRange(staff1, staff2, staff3);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var johnResults = await queryContext.StaffMembers
            .Where(s => s.FirstName.Contains("John") || s.LastName.Contains("John"))
            .ToListAsync();

        // Assert
        johnResults.Should().HaveCount(3); // John, Johnny, Johnson
    }

    [Fact]
    public async Task GetStaffMembers_WithHourlyRateRange_ShouldReturnFilteredResults()
    {
        // Arrange
        var lowPaidStaff = TestDataFactory.CreateStaffMember(firstName: "Low", hourlyRate: 15.00m);
        var midPaidStaff = TestDataFactory.CreateStaffMember(firstName: "Mid", hourlyRate: 30.00m);
        var highPaidStaff = TestDataFactory.CreateStaffMember(firstName: "High", hourlyRate: 50.00m);

        DbContext.StaffMembers.AddRange(lowPaidStaff, midPaidStaff, highPaidStaff);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var seniorStaff = await queryContext.StaffMembers
            .Where(s => s.HourlyRate >= 25.00m)
            .ToListAsync();

        // Assert
        seniorStaff.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetStaffMembers_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var staffMembers = Enumerable.Range(1, 10)
            .Select(i => TestDataFactory.CreateStaffMember(firstName: $"Staff{i:D2}"))
            .ToList();

        DbContext.StaffMembers.AddRange(staffMembers);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var page1 = await queryContext.StaffMembers
            .OrderBy(s => s.FirstName)
            .Skip(0)
            .Take(5)
            .ToListAsync();

        var page2 = await queryContext.StaffMembers
            .OrderBy(s => s.FirstName)
            .Skip(5)
            .Take(5)
            .ToListAsync();

        // Assert
        page1.Should().HaveCount(5);
        page2.Should().HaveCount(5);
        page1.Select(s => s.StaffMemberId).Should().NotIntersectWith(page2.Select(s => s.StaffMemberId));
    }

    [Fact]
    public async Task GetStaffMemberByEmail_ShouldReturnMatchingRecord()
    {
        // Arrange
        var staffMember = TestDataFactory.CreateStaffMember(email: "unique@events.com");
        DbContext.StaffMembers.Add(staffMember);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var result = await queryContext.StaffMembers
            .FirstOrDefaultAsync(s => s.Email == "unique@events.com");

        // Assert
        result.Should().NotBeNull();
        result!.StaffMemberId.Should().Be(staffMember.StaffMemberId);
    }

    #endregion

    #region Update (Nominal)

    [Fact]
    public async Task UpdateStaffMember_ShouldPersistChanges()
    {
        // Arrange
        var staffMember = TestDataFactory.CreateStaffMember(firstName: "Original", lastName: "Name");
        DbContext.StaffMembers.Add(staffMember);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var staffToUpdate = await updateContext.StaffMembers
            .FirstAsync(s => s.StaffMemberId == staffMember.StaffMemberId);

        staffToUpdate.FirstName = "Updated";
        staffToUpdate.LastName = "Person";
        staffToUpdate.ModifiedAt = DateTime.UtcNow;
        staffToUpdate.ModifiedBy = Guid.NewGuid();
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedStaff = await verifyContext.StaffMembers
            .FirstOrDefaultAsync(s => s.StaffMemberId == staffMember.StaffMemberId);

        updatedStaff.Should().NotBeNull();
        updatedStaff!.FirstName.Should().Be("Updated");
        updatedStaff.LastName.Should().Be("Person");
        updatedStaff.ModifiedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateStaffMemberStatus_ShouldPersistNewStatus()
    {
        // Arrange
        var staffMember = TestDataFactory.CreateStaffMember(status: StaffStatus.Active);
        DbContext.StaffMembers.Add(staffMember);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var staffToUpdate = await updateContext.StaffMembers
            .FirstAsync(s => s.StaffMemberId == staffMember.StaffMemberId);

        staffToUpdate.Status = StaffStatus.OnLeave;
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedStaff = await verifyContext.StaffMembers
            .FirstOrDefaultAsync(s => s.StaffMemberId == staffMember.StaffMemberId);

        updatedStaff!.Status.Should().Be(StaffStatus.OnLeave);
    }

    [Fact]
    public async Task TerminateStaffMember_ShouldSetTerminationDate()
    {
        // Arrange
        var staffMember = TestDataFactory.CreateStaffMember(status: StaffStatus.Active);
        DbContext.StaffMembers.Add(staffMember);
        await DbContext.SaveChangesAsync();

        // Act
        var terminationDate = DateTime.UtcNow;
        await using var updateContext = CreateNewDbContext();
        var staffToUpdate = await updateContext.StaffMembers
            .FirstAsync(s => s.StaffMemberId == staffMember.StaffMemberId);

        staffToUpdate.Status = StaffStatus.Terminated;
        staffToUpdate.TerminationDate = terminationDate;
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedStaff = await verifyContext.StaffMembers
            .FirstOrDefaultAsync(s => s.StaffMemberId == staffMember.StaffMemberId);

        updatedStaff!.Status.Should().Be(StaffStatus.Terminated);
        updatedStaff.TerminationDate.Should().BeCloseTo(terminationDate, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task UpdateStaffMemberHourlyRate_ShouldPersistNewRate()
    {
        // Arrange
        var staffMember = TestDataFactory.CreateStaffMember(hourlyRate: 25.00m);
        DbContext.StaffMembers.Add(staffMember);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var staffToUpdate = await updateContext.StaffMembers
            .FirstAsync(s => s.StaffMemberId == staffMember.StaffMemberId);

        staffToUpdate.HourlyRate = 35.00m;
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedStaff = await verifyContext.StaffMembers
            .FirstOrDefaultAsync(s => s.StaffMemberId == staffMember.StaffMemberId);

        updatedStaff!.HourlyRate.Should().Be(35.00m);
    }

    #endregion

    #region Delete (Nominal)

    [Fact]
    public async Task SoftDeleteStaffMember_ShouldNotAppearInQueries()
    {
        // Arrange
        var staffMember = TestDataFactory.CreateStaffMember();
        DbContext.StaffMembers.Add(staffMember);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var staffToDelete = await deleteContext.StaffMembers
            .FirstAsync(s => s.StaffMemberId == staffMember.StaffMemberId);

        staffToDelete.IsDeleted = true;
        await deleteContext.SaveChangesAsync();

        // Assert - Query filter should exclude soft-deleted records
        await using var verifyContext = CreateNewDbContext();
        var result = await verifyContext.StaffMembers
            .FirstOrDefaultAsync(s => s.StaffMemberId == staffMember.StaffMemberId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task SoftDeleteStaffMember_ShouldStillExistWithIgnoreQueryFilters()
    {
        // Arrange
        var staffMember = TestDataFactory.CreateStaffMember();
        DbContext.StaffMembers.Add(staffMember);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var staffToDelete = await deleteContext.StaffMembers
            .FirstAsync(s => s.StaffMemberId == staffMember.StaffMemberId);

        staffToDelete.IsDeleted = true;
        await deleteContext.SaveChangesAsync();

        // Assert - Should still exist when ignoring query filters
        await using var verifyContext = CreateNewDbContext();
        var result = await verifyContext.StaffMembers
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.StaffMemberId == staffMember.StaffMemberId);

        result.Should().NotBeNull();
        result!.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task HardDeleteStaffMember_ShouldRemoveFromDatabase()
    {
        // Arrange
        var staffMember = TestDataFactory.CreateStaffMember();
        DbContext.StaffMembers.Add(staffMember);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var staffToDelete = await deleteContext.StaffMembers
            .FirstAsync(s => s.StaffMemberId == staffMember.StaffMemberId);

        deleteContext.StaffMembers.Remove(staffToDelete);
        await deleteContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var result = await verifyContext.StaffMembers
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.StaffMemberId == staffMember.StaffMemberId);

        result.Should().BeNull();
    }

    #endregion

    #region Off-Nominal Cases

    [Fact]
    public async Task GetStaffMemberById_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await DbContext.StaffMembers
            .FirstOrDefaultAsync(s => s.StaffMemberId == nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateStaffMember_WithDuplicateId_ShouldThrowException()
    {
        // Arrange
        var staffId = Guid.NewGuid();
        var staff1 = TestDataFactory.CreateStaffMember(firstName: "First");
        staff1.StaffMemberId = staffId;

        DbContext.StaffMembers.Add(staff1);
        await DbContext.SaveChangesAsync();

        // Act & Assert
        await using var context2 = CreateNewDbContext();
        var staff2 = TestDataFactory.CreateStaffMember(firstName: "Second");
        staff2.StaffMemberId = staffId;

        context2.StaffMembers.Add(staff2);

        await Assert.ThrowsAsync<DbUpdateException>(
            () => context2.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateStaffMember_WithDuplicateEmail_ShouldThrowException()
    {
        // Arrange
        var staff1 = TestDataFactory.CreateStaffMember(email: "duplicate@events.com");
        DbContext.StaffMembers.Add(staff1);
        await DbContext.SaveChangesAsync();

        // Act & Assert
        await using var context2 = CreateNewDbContext();
        var staff2 = TestDataFactory.CreateStaffMember(email: "duplicate@events.com");
        context2.StaffMembers.Add(staff2);

        await Assert.ThrowsAsync<DbUpdateException>(
            () => context2.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateStaffMember_WithNullFirstName_ShouldThrowException()
    {
        // Arrange
        var staffMember = TestDataFactory.CreateStaffMember();
        staffMember.FirstName = null!;

        // Act & Assert
        DbContext.StaffMembers.Add(staffMember);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateStaffMember_WithNullLastName_ShouldThrowException()
    {
        // Arrange
        var staffMember = TestDataFactory.CreateStaffMember();
        staffMember.LastName = null!;

        // Act & Assert
        DbContext.StaffMembers.Add(staffMember);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateStaffMember_WithNullEmail_ShouldThrowException()
    {
        // Arrange
        var staffMember = TestDataFactory.CreateStaffMember();
        staffMember.Email = null!;

        // Act & Assert
        DbContext.StaffMembers.Add(staffMember);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateStaffMember_WithFirstNameExceedingMaxLength_ShouldThrowException()
    {
        // Arrange
        var staffMember = TestDataFactory.CreateStaffMember();
        staffMember.FirstName = new string('F', 101); // Max is 100

        // Act & Assert
        DbContext.StaffMembers.Add(staffMember);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateStaffMember_WithEmailExceedingMaxLength_ShouldThrowException()
    {
        // Arrange
        var staffMember = TestDataFactory.CreateStaffMember();
        staffMember.Email = new string('e', 246) + "@test.com"; // Max is 255

        // Act & Assert
        DbContext.StaffMembers.Add(staffMember);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task UpdateStaffMemberToExistingEmail_ShouldThrowException()
    {
        // Arrange
        var staff1 = TestDataFactory.CreateStaffMember(email: "existing@events.com");
        var staff2 = TestDataFactory.CreateStaffMember(email: "different@events.com");

        DbContext.StaffMembers.AddRange(staff1, staff2);
        await DbContext.SaveChangesAsync();

        // Act & Assert
        await using var updateContext = CreateNewDbContext();
        var staffToUpdate = await updateContext.StaffMembers
            .FirstAsync(s => s.StaffMemberId == staff2.StaffMemberId);

        staffToUpdate.Email = "existing@events.com"; // Try to change to existing email

        await Assert.ThrowsAsync<DbUpdateException>(
            () => updateContext.SaveChangesAsync());
    }

    [Fact]
    public async Task UpdateNonExistentStaffMember_ShouldThrowException()
    {
        // Arrange
        var nonExistentStaff = TestDataFactory.CreateStaffMember();

        // Act & Assert
        DbContext.StaffMembers.Update(nonExistentStaff);
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
            () => DbContext.SaveChangesAsync());
    }

    #endregion
}
