// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.CustomerAggregate;
using EventManagementPlatform.Infrastructure.Tests.Fixtures;
using EventManagementPlatform.Infrastructure.Tests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Infrastructure.Tests.Entities;

[Collection("Database")]
public class CustomerTests : IntegrationTestBase
{
    public CustomerTests(SqlExpressDatabaseFixture fixture) : base(fixture)
    {
    }

    #region Create (Nominal)

    [Fact]
    public async Task CreateCustomer_WithValidData_ShouldPersistToDatabase()
    {
        // Arrange
        var customer = TestDataFactory.CreateCustomer(
            companyName: "Acme Corporation",
            type: CustomerType.Enterprise,
            email: "contact@acme.com"
        );

        // Act
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedCustomer = await verifyContext.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

        savedCustomer.Should().NotBeNull();
        savedCustomer!.CompanyName.Should().Be("Acme Corporation");
        savedCustomer.Type.Should().Be(CustomerType.Enterprise);
        savedCustomer.PrimaryEmail.Should().Be("contact@acme.com");
        savedCustomer.Status.Should().Be(CustomerStatus.Active);
    }

    [Fact]
    public async Task CreateCustomer_WithAllFields_ShouldPersistAllValues()
    {
        // Arrange
        var customer = new Customer
        {
            CustomerId = Guid.NewGuid(),
            CompanyName = "Full Data Corp",
            Type = CustomerType.NonProfit,
            PrimaryEmail = "full@data.org",
            PrimaryPhone = "555-111-2222",
            Industry = "Healthcare",
            Website = "https://fulldata.org",
            BillingStreet = "789 Charity Lane",
            BillingCity = "Vancouver",
            BillingState = "BC",
            BillingPostalCode = "V6B 1A1",
            BillingCountry = "Canada",
            Status = CustomerStatus.Active,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid()
        };

        // Act
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedCustomer = await verifyContext.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

        savedCustomer.Should().NotBeNull();
        savedCustomer!.Industry.Should().Be("Healthcare");
        savedCustomer.Website.Should().Be("https://fulldata.org");
        savedCustomer.BillingCity.Should().Be("Vancouver");
        savedCustomer.BillingState.Should().Be("BC");
    }

    [Fact]
    public async Task CreateMultipleCustomers_ShouldPersistAll()
    {
        // Arrange
        var customers = new[]
        {
            TestDataFactory.CreateCustomer(companyName: "Company A"),
            TestDataFactory.CreateCustomer(companyName: "Company B"),
            TestDataFactory.CreateCustomer(companyName: "Company C")
        };

        // Act
        DbContext.Customers.AddRange(customers);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedCustomers = await verifyContext.Customers.ToListAsync();
        savedCustomers.Should().HaveCount(3);
    }

    #endregion

    #region Read (Nominal)

    [Fact]
    public async Task GetCustomerById_WhenExists_ShouldReturnCustomer()
    {
        // Arrange
        var customer = TestDataFactory.CreateCustomer(companyName: "Query Test Corp");
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var result = await queryContext.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

        // Assert
        result.Should().NotBeNull();
        result!.CompanyName.Should().Be("Query Test Corp");
    }

    [Fact]
    public async Task GetCustomers_WithStatusFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var activeCustomer = TestDataFactory.CreateCustomer(status: CustomerStatus.Active);
        var inactiveCustomer = TestDataFactory.CreateCustomer(status: CustomerStatus.Inactive);
        var suspendedCustomer = TestDataFactory.CreateCustomer(status: CustomerStatus.Suspended);

        DbContext.Customers.AddRange(activeCustomer, inactiveCustomer, suspendedCustomer);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var activeCustomers = await queryContext.Customers
            .Where(c => c.Status == CustomerStatus.Active)
            .ToListAsync();

        // Assert
        activeCustomers.Should().HaveCount(1);
        activeCustomers[0].CustomerId.Should().Be(activeCustomer.CustomerId);
    }

    [Fact]
    public async Task GetCustomers_WithTypeFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var enterpriseCustomer = TestDataFactory.CreateCustomer(type: CustomerType.Enterprise);
        var individualCustomer = TestDataFactory.CreateCustomer(type: CustomerType.Individual);
        var governmentCustomer = TestDataFactory.CreateCustomer(type: CustomerType.Government);

        DbContext.Customers.AddRange(enterpriseCustomer, individualCustomer, governmentCustomer);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var enterpriseCustomers = await queryContext.Customers
            .Where(c => c.Type == CustomerType.Enterprise)
            .ToListAsync();

        // Assert
        enterpriseCustomers.Should().HaveCount(1);
        enterpriseCustomers[0].CustomerId.Should().Be(enterpriseCustomer.CustomerId);
    }

    [Fact]
    public async Task GetCustomers_WithSearchByCompanyName_ShouldReturnMatchingResults()
    {
        // Arrange
        var customer1 = TestDataFactory.CreateCustomer(companyName: "ABC Technologies");
        var customer2 = TestDataFactory.CreateCustomer(companyName: "XYZ Tech Solutions");
        var customer3 = TestDataFactory.CreateCustomer(companyName: "Global Industries");

        DbContext.Customers.AddRange(customer1, customer2, customer3);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var techCustomers = await queryContext.Customers
            .Where(c => c.CompanyName.Contains("Tech"))
            .ToListAsync();

        // Assert
        techCustomers.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetCustomers_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var customers = Enumerable.Range(1, 10)
            .Select(i => TestDataFactory.CreateCustomer(companyName: $"Company {i:D2}"))
            .ToList();

        DbContext.Customers.AddRange(customers);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var page1 = await queryContext.Customers
            .OrderBy(c => c.CompanyName)
            .Skip(0)
            .Take(3)
            .ToListAsync();

        var page2 = await queryContext.Customers
            .OrderBy(c => c.CompanyName)
            .Skip(3)
            .Take(3)
            .ToListAsync();

        // Assert
        page1.Should().HaveCount(3);
        page2.Should().HaveCount(3);
        page1.Select(c => c.CustomerId).Should().NotIntersectWith(page2.Select(c => c.CustomerId));
    }

    #endregion

    #region Update (Nominal)

    [Fact]
    public async Task UpdateCustomer_ShouldPersistChanges()
    {
        // Arrange
        var customer = TestDataFactory.CreateCustomer(companyName: "Original Name");
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var customerToUpdate = await updateContext.Customers
            .FirstAsync(c => c.CustomerId == customer.CustomerId);

        customerToUpdate.CompanyName = "Updated Name";
        customerToUpdate.ModifiedAt = DateTime.UtcNow;
        customerToUpdate.ModifiedBy = Guid.NewGuid();
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedCustomer = await verifyContext.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

        updatedCustomer.Should().NotBeNull();
        updatedCustomer!.CompanyName.Should().Be("Updated Name");
        updatedCustomer.ModifiedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateCustomerStatus_ShouldPersistNewStatus()
    {
        // Arrange
        var customer = TestDataFactory.CreateCustomer(status: CustomerStatus.Active);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var customerToUpdate = await updateContext.Customers
            .FirstAsync(c => c.CustomerId == customer.CustomerId);

        customerToUpdate.Status = CustomerStatus.Suspended;
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedCustomer = await verifyContext.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

        updatedCustomer!.Status.Should().Be(CustomerStatus.Suspended);
    }

    #endregion

    #region Delete (Nominal)

    [Fact]
    public async Task SoftDeleteCustomer_ShouldNotAppearInQueries()
    {
        // Arrange
        var customer = TestDataFactory.CreateCustomer();
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var customerToDelete = await deleteContext.Customers
            .FirstAsync(c => c.CustomerId == customer.CustomerId);

        customerToDelete.IsDeleted = true;
        await deleteContext.SaveChangesAsync();

        // Assert - Query filter should exclude soft-deleted records
        await using var verifyContext = CreateNewDbContext();
        var result = await verifyContext.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task SoftDeleteCustomer_ShouldStillExistWithIgnoreQueryFilters()
    {
        // Arrange
        var customer = TestDataFactory.CreateCustomer();
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var customerToDelete = await deleteContext.Customers
            .FirstAsync(c => c.CustomerId == customer.CustomerId);

        customerToDelete.IsDeleted = true;
        await deleteContext.SaveChangesAsync();

        // Assert - Should still exist when ignoring query filters
        await using var verifyContext = CreateNewDbContext();
        var result = await verifyContext.Customers
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

        result.Should().NotBeNull();
        result!.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task HardDeleteCustomer_ShouldRemoveFromDatabase()
    {
        // Arrange
        var customer = TestDataFactory.CreateCustomer();
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var customerToDelete = await deleteContext.Customers
            .FirstAsync(c => c.CustomerId == customer.CustomerId);

        deleteContext.Customers.Remove(customerToDelete);
        await deleteContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var result = await verifyContext.Customers
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

        result.Should().BeNull();
    }

    #endregion

    #region Off-Nominal Cases

    [Fact]
    public async Task GetCustomerById_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await DbContext.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateCustomer_WithDuplicateId_ShouldThrowException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer1 = TestDataFactory.CreateCustomer();
        customer1.CustomerId = customerId;

        DbContext.Customers.Add(customer1);
        await DbContext.SaveChangesAsync();

        // Act & Assert
        await using var context2 = CreateNewDbContext();
        var customer2 = TestDataFactory.CreateCustomer();
        customer2.CustomerId = customerId;

        context2.Customers.Add(customer2);

        await Assert.ThrowsAsync<DbUpdateException>(
            () => context2.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateCustomer_WithNullCompanyName_ShouldThrowException()
    {
        // Arrange
        var customer = TestDataFactory.CreateCustomer();
        customer.CompanyName = null!;

        // Act & Assert
        DbContext.Customers.Add(customer);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateCustomer_WithCompanyNameExceedingMaxLength_ShouldThrowException()
    {
        // Arrange
        var customer = TestDataFactory.CreateCustomer();
        customer.CompanyName = new string('A', 201); // Max is 200

        // Act & Assert
        DbContext.Customers.Add(customer);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateCustomer_WithNullEmail_ShouldThrowException()
    {
        // Arrange
        var customer = TestDataFactory.CreateCustomer();
        customer.PrimaryEmail = null!;

        // Act & Assert
        DbContext.Customers.Add(customer);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateCustomer_WithEmailExceedingMaxLength_ShouldThrowException()
    {
        // Arrange
        var customer = TestDataFactory.CreateCustomer();
        customer.PrimaryEmail = new string('a', 246) + "@test.com"; // Max is 255

        // Act & Assert
        DbContext.Customers.Add(customer);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task GetDeletedCustomers_WithoutIgnoreQueryFilters_ShouldReturnEmpty()
    {
        // Arrange
        var deletedCustomer = TestDataFactory.CreateCustomer(isDeleted: true);
        var activeCustomer = TestDataFactory.CreateCustomer(isDeleted: false);

        // Need to bypass query filter for inserting deleted record
        await DbContext.Database.ExecuteSqlRawAsync(
            @"INSERT INTO Customers (CustomerId, CompanyName, Type, PrimaryEmail, PrimaryPhone,
              BillingStreet, BillingCity, BillingState, BillingPostalCode, BillingCountry,
              Status, IsDeleted, CreatedAt, CreatedBy)
              VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13})",
            deletedCustomer.CustomerId, deletedCustomer.CompanyName, (int)deletedCustomer.Type,
            deletedCustomer.PrimaryEmail, deletedCustomer.PrimaryPhone, deletedCustomer.BillingStreet,
            deletedCustomer.BillingCity, deletedCustomer.BillingState, deletedCustomer.BillingPostalCode,
            deletedCustomer.BillingCountry, (int)deletedCustomer.Status, true,
            deletedCustomer.CreatedAt, deletedCustomer.CreatedBy);

        DbContext.Customers.Add(activeCustomer);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var customers = await queryContext.Customers.ToListAsync();

        // Assert
        customers.Should().HaveCount(1);
        customers[0].CustomerId.Should().Be(activeCustomer.CustomerId);
    }

    [Fact]
    public async Task UpdateNonExistentCustomer_ShouldThrowException()
    {
        // Arrange
        var nonExistentCustomer = TestDataFactory.CreateCustomer();

        // Act & Assert
        DbContext.Customers.Update(nonExistentCustomer);
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
            () => DbContext.SaveChangesAsync());
    }

    #endregion
}
