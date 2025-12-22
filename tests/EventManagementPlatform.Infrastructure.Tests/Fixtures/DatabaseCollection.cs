// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace EventManagementPlatform.Infrastructure.Tests.Fixtures;

/// <summary>
/// xUnit collection definition for database integration tests.
/// All tests in this collection share the same database fixture.
/// </summary>
[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<SqlExpressDatabaseFixture>
{
}
