// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Infrastructure.Tests.Fixtures;

/// <summary>
/// xUnit collection fixture that manages a SQL Express database for integration tests.
/// Creates a unique database per test run and cleans up after all tests complete.
/// </summary>
public class SqlExpressDatabaseFixture : IAsyncLifetime
{
    private const string MasterConnectionString = "Server=.\\SQLEXPRESS;Database=master;Trusted_Connection=True;TrustServerCertificate=True";

    public SqlExpressDatabaseFixture()
    {
        // Create a unique database name for this test run to allow parallel test execution
        DatabaseName = $"EventManagementPlatform_Tests_{Guid.NewGuid():N}";
        ConnectionString = $"Server=.\\SQLEXPRESS;Database={DatabaseName};Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";
    }

    public string DatabaseName { get; }

    public string ConnectionString { get; }

    public async Task InitializeAsync()
    {
        // Create the test database
        await CreateDatabaseAsync();

        // Apply migrations / create schema
        await using var context = CreateDbContext();
        await context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        // Drop the test database
        await DropDatabaseAsync();
    }

    public EventManagementPlatformDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<EventManagementPlatformDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;

        return new EventManagementPlatformDbContext(options);
    }

    private async Task CreateDatabaseAsync()
    {
        await using var connection = new Microsoft.Data.SqlClient.SqlConnection(MasterConnectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = $"CREATE DATABASE [{DatabaseName}]";
        await command.ExecuteNonQueryAsync();
    }

    private async Task DropDatabaseAsync()
    {
        await using var connection = new Microsoft.Data.SqlClient.SqlConnection(MasterConnectionString);
        await connection.OpenAsync();

        // Force close all connections to the database
        var command = connection.CreateCommand();
        command.CommandText = $@"
            IF EXISTS (SELECT name FROM sys.databases WHERE name = N'{DatabaseName}')
            BEGIN
                ALTER DATABASE [{DatabaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                DROP DATABASE [{DatabaseName}];
            END";
        await command.ExecuteNonQueryAsync();
    }
}
