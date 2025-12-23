// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Infrastructure.Tests.Fixtures;

/// <summary>
/// Base class for integration tests that provides database context management
/// and automatic cleanup between tests.
/// </summary>
[Collection("Database")]
public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected IntegrationTestBase(SqlExpressDatabaseFixture fixture)
    {
        Fixture = fixture;
    }

    protected SqlExpressDatabaseFixture Fixture { get; }

    protected EventManagementPlatformDbContext DbContext { get; private set; } = null!;

    public virtual Task InitializeAsync()
    {
        DbContext = Fixture.CreateDbContext();
        return Task.CompletedTask;
    }

    public virtual async Task DisposeAsync()
    {
        // Clean up all data after each test to ensure test isolation
        await CleanupDatabaseAsync();
        await DbContext.DisposeAsync();
    }

    /// <summary>
    /// Cleans all data from the database tables while preserving the schema.
    /// Uses DELETE instead of TRUNCATE to handle foreign key constraints.
    /// </summary>
    /// <returns>A task representing the asynchronous cleanup operation.</returns>
    protected async Task CleanupDatabaseAsync()
    {
        // Delete in order to respect foreign key constraints
        // First delete junction tables and dependent entities
        await DbContext.Database.ExecuteSqlRawAsync("DELETE FROM [UserRoles]");
        await DbContext.Database.ExecuteSqlRawAsync("DELETE FROM [Privileges]");
        await DbContext.Database.ExecuteSqlRawAsync("DELETE FROM [Events]");

        // Then delete main entities
        await DbContext.Database.ExecuteSqlRawAsync("DELETE FROM [EventTypes]");
        await DbContext.Database.ExecuteSqlRawAsync("DELETE FROM [Users]");
        await DbContext.Database.ExecuteSqlRawAsync("DELETE FROM [Roles]");
        await DbContext.Database.ExecuteSqlRawAsync("DELETE FROM [Customers]");
        await DbContext.Database.ExecuteSqlRawAsync("DELETE FROM [Venues]");
        await DbContext.Database.ExecuteSqlRawAsync("DELETE FROM [StaffMembers]");
        await DbContext.Database.ExecuteSqlRawAsync("DELETE FROM [EquipmentItems]");
    }

    /// <summary>
    /// Creates a new DbContext instance for verification queries.
    /// Useful when you need to verify data was persisted correctly.
    /// </summary>
    /// <returns>A new instance of <see cref="EventManagementPlatformDbContext"/>.</returns>
    protected EventManagementPlatformDbContext CreateNewDbContext()
    {
        return Fixture.CreateDbContext();
    }
}
