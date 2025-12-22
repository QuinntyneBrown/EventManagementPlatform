// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.UserAggregate;
using EventManagementPlatform.Infrastructure.Tests.Fixtures;
using EventManagementPlatform.Infrastructure.Tests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Infrastructure.Tests.Entities;

[Collection("Database")]
public class UserTests : IntegrationTestBase
{
    public UserTests(SqlExpressDatabaseFixture fixture)
        : base(fixture)
    {
    }

    #region User Create (Nominal)

    [Fact]
    public async Task CreateUser_WithValidData_ShouldPersistToDatabase()
    {
        // Arrange
        var user = TestDataFactory.CreateUser(username: "testuser");

        // Act
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedUser = await verifyContext.Users
            .FirstOrDefaultAsync(u => u.UserId == user.UserId);

        savedUser.Should().NotBeNull();
        savedUser!.Username.Should().Be("testuser");
        savedUser.Password.Should().NotBeNullOrEmpty();
        savedUser.Salt.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateUser_WithRefreshToken_ShouldPersist()
    {
        // Arrange
        var user = TestDataFactory.CreateUser();
        user.RefreshToken = "refresh_token_12345";

        // Act
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedUser = await verifyContext.Users
            .FirstOrDefaultAsync(u => u.UserId == user.UserId);

        savedUser.Should().NotBeNull();
        savedUser!.RefreshToken.Should().Be("refresh_token_12345");
    }

    [Fact]
    public async Task CreateMultipleUsers_ShouldPersistAll()
    {
        // Arrange
        var users = new[]
        {
            TestDataFactory.CreateUser(username: "user1"),
            TestDataFactory.CreateUser(username: "user2"),
            TestDataFactory.CreateUser(username: "user3")
        };

        // Act
        DbContext.Users.AddRange(users);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedUsers = await verifyContext.Users.ToListAsync();
        savedUsers.Should().HaveCount(3);
    }

    #endregion

    #region User Read (Nominal)

    [Fact]
    public async Task GetUserById_WhenExists_ShouldReturnUser()
    {
        // Arrange
        var user = TestDataFactory.CreateUser(username: "querytest");
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var result = await queryContext.Users
            .FirstOrDefaultAsync(u => u.UserId == user.UserId);

        // Assert
        result.Should().NotBeNull();
        result!.Username.Should().Be("querytest");
    }

    [Fact]
    public async Task GetUserByUsername_ShouldReturnMatchingUser()
    {
        // Arrange
        var user = TestDataFactory.CreateUser(username: "unique_username");
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var result = await queryContext.Users
            .FirstOrDefaultAsync(u => u.Username == "unique_username");

        // Assert
        result.Should().NotBeNull();
        result!.UserId.Should().Be(user.UserId);
    }

    [Fact]
    public async Task GetUser_WithRolesIncluded_ShouldReturnWithRoles()
    {
        // Arrange
        var role = TestDataFactory.CreateRole(name: "Admin");
        var user = TestDataFactory.CreateUser(username: "adminuser");
        user.Roles.Add(role);

        DbContext.Roles.Add(role);
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var result = await queryContext.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.UserId == user.UserId);

        // Assert
        result.Should().NotBeNull();
        result!.Roles.Should().HaveCount(1);
        result.Roles.First().Name.Should().Be("Admin");
    }

    #endregion

    #region User Update (Nominal)

    [Fact]
    public async Task UpdateUser_ShouldPersistChanges()
    {
        // Arrange
        var user = TestDataFactory.CreateUser(username: "original");
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var userToUpdate = await updateContext.Users
            .FirstAsync(u => u.UserId == user.UserId);

        userToUpdate.RefreshToken = "new_refresh_token";
        userToUpdate.ModifiedAt = DateTime.UtcNow;
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedUser = await verifyContext.Users
            .FirstOrDefaultAsync(u => u.UserId == user.UserId);

        updatedUser.Should().NotBeNull();
        updatedUser!.RefreshToken.Should().Be("new_refresh_token");
        updatedUser.ModifiedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateUserPassword_ShouldPersistNewPassword()
    {
        // Arrange
        var user = TestDataFactory.CreateUser();
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var userToUpdate = await updateContext.Users
            .FirstAsync(u => u.UserId == user.UserId);

        userToUpdate.Password = "new_hashed_password";
        userToUpdate.Salt = new byte[] { 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115 };
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedUser = await verifyContext.Users
            .FirstOrDefaultAsync(u => u.UserId == user.UserId);

        updatedUser!.Password.Should().Be("new_hashed_password");
    }

    [Fact]
    public async Task AddRoleToUser_ShouldPersist()
    {
        // Arrange
        var role = TestDataFactory.CreateRole(name: "NewRole");
        var user = TestDataFactory.CreateUser();

        DbContext.Roles.Add(role);
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var userToUpdate = await updateContext.Users
            .Include(u => u.Roles)
            .FirstAsync(u => u.UserId == user.UserId);

        var roleToAdd = await updateContext.Roles
            .FirstAsync(r => r.RoleId == role.RoleId);

        userToUpdate.Roles.Add(roleToAdd);
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedUser = await verifyContext.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.UserId == user.UserId);

        updatedUser!.Roles.Should().HaveCount(1);
    }

    #endregion

    #region User Delete (Nominal)

    [Fact]
    public async Task SoftDeleteUser_ShouldNotAppearInQueries()
    {
        // Arrange
        var user = TestDataFactory.CreateUser();
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var userToDelete = await deleteContext.Users
            .FirstAsync(u => u.UserId == user.UserId);

        userToDelete.IsDeleted = true;
        await deleteContext.SaveChangesAsync();

        // Assert - Query filter should exclude soft-deleted records
        await using var verifyContext = CreateNewDbContext();
        var result = await verifyContext.Users
            .FirstOrDefaultAsync(u => u.UserId == user.UserId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task SoftDeleteUser_ShouldStillExistWithIgnoreQueryFilters()
    {
        // Arrange
        var user = TestDataFactory.CreateUser();
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var userToDelete = await deleteContext.Users
            .FirstAsync(u => u.UserId == user.UserId);

        userToDelete.IsDeleted = true;
        await deleteContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var result = await verifyContext.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.UserId == user.UserId);

        result.Should().NotBeNull();
        result!.IsDeleted.Should().BeTrue();
    }

    #endregion

    #region Role Tests

    [Fact]
    public async Task CreateRole_WithValidData_ShouldPersistToDatabase()
    {
        // Arrange
        var role = TestDataFactory.CreateRole(name: "Administrator");

        // Act
        DbContext.Roles.Add(role);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedRole = await verifyContext.Roles
            .FirstOrDefaultAsync(r => r.RoleId == role.RoleId);

        savedRole.Should().NotBeNull();
        savedRole!.Name.Should().Be("Administrator");
    }

    [Fact]
    public async Task GetRole_WithPrivilegesIncluded_ShouldReturnWithPrivileges()
    {
        // Arrange
        var role = TestDataFactory.CreateRole(name: "RoleWithPrivileges");
        var privilege1 = TestDataFactory.CreatePrivilege(role.RoleId, "Events", AccessRight.Read);
        var privilege2 = TestDataFactory.CreatePrivilege(role.RoleId, "Customers", AccessRight.Write);

        role.Privileges.Add(privilege1);
        role.Privileges.Add(privilege2);

        DbContext.Roles.Add(role);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var result = await queryContext.Roles
            .Include(r => r.Privileges)
            .FirstOrDefaultAsync(r => r.RoleId == role.RoleId);

        // Assert
        result.Should().NotBeNull();
        result!.Privileges.Should().HaveCount(2);
    }

    [Fact]
    public async Task DeleteRole_ShouldCascadeDeletePrivileges()
    {
        // Arrange
        var role = TestDataFactory.CreateRole(name: "RoleToDelete");
        var privilege = TestDataFactory.CreatePrivilege(role.RoleId, "Events", AccessRight.Read);
        role.Privileges.Add(privilege);

        DbContext.Roles.Add(role);
        await DbContext.SaveChangesAsync();

        // Act
        await using var deleteContext = CreateNewDbContext();
        var roleToDelete = await deleteContext.Roles
            .FirstAsync(r => r.RoleId == role.RoleId);

        deleteContext.Roles.Remove(roleToDelete);
        await deleteContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var deletedRole = await verifyContext.Roles
            .FirstOrDefaultAsync(r => r.RoleId == role.RoleId);
        var deletedPrivilege = await verifyContext.Privileges
            .FirstOrDefaultAsync(p => p.PrivilegeId == privilege.PrivilegeId);

        deletedRole.Should().BeNull();
        deletedPrivilege.Should().BeNull();
    }

    #endregion

    #region Privilege Tests

    [Fact]
    public async Task CreatePrivilege_WithValidData_ShouldPersistToDatabase()
    {
        // Arrange
        var role = TestDataFactory.CreateRole(name: "PrivilegeTestRole");
        DbContext.Roles.Add(role);
        await DbContext.SaveChangesAsync();

        var privilege = TestDataFactory.CreatePrivilege(role.RoleId, "Venues", AccessRight.Create);

        // Act
        DbContext.Privileges.Add(privilege);
        await DbContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var savedPrivilege = await verifyContext.Privileges
            .FirstOrDefaultAsync(p => p.PrivilegeId == privilege.PrivilegeId);

        savedPrivilege.Should().NotBeNull();
        savedPrivilege!.Aggregate.Should().Be("Venues");
        savedPrivilege.AccessRight.Should().Be(AccessRight.Create);
    }

    [Fact]
    public async Task GetPrivilege_WithRoleIncluded_ShouldReturnWithRole()
    {
        // Arrange
        var role = TestDataFactory.CreateRole(name: "IncludedRole");
        var privilege = TestDataFactory.CreatePrivilege(role.RoleId, "Staff", AccessRight.Delete);
        role.Privileges.Add(privilege);

        DbContext.Roles.Add(role);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var result = await queryContext.Privileges
            .Include(p => p.Role)
            .FirstOrDefaultAsync(p => p.PrivilegeId == privilege.PrivilegeId);

        // Assert
        result.Should().NotBeNull();
        result!.Role.Should().NotBeNull();
        result.Role!.Name.Should().Be("IncludedRole");
    }

    #endregion

    #region Off-Nominal Cases

    [Fact]
    public async Task GetUserById_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await DbContext.Users
            .FirstOrDefaultAsync(u => u.UserId == nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateUser_WithDuplicateUsername_ShouldThrowException()
    {
        // Arrange
        var user1 = TestDataFactory.CreateUser(username: "duplicateuser");
        DbContext.Users.Add(user1);
        await DbContext.SaveChangesAsync();

        // Act & Assert
        await using var context2 = CreateNewDbContext();
        var user2 = TestDataFactory.CreateUser(username: "duplicateuser");
        context2.Users.Add(user2);

        await Assert.ThrowsAsync<DbUpdateException>(
            () => context2.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateUser_WithNullUsername_ShouldThrowException()
    {
        // Arrange
        var user = TestDataFactory.CreateUser();
        user.Username = null!;

        // Act & Assert
        DbContext.Users.Add(user);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateUser_WithUsernameExceedingMaxLength_ShouldThrowException()
    {
        // Arrange
        var user = TestDataFactory.CreateUser();
        user.Username = new string('u', 101); // Max is 100

        // Act & Assert
        DbContext.Users.Add(user);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateUser_WithNullPassword_ShouldThrowException()
    {
        // Arrange
        var user = TestDataFactory.CreateUser();
        user.Password = null!;

        // Act & Assert
        DbContext.Users.Add(user);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateRole_WithDuplicateName_ShouldThrowException()
    {
        // Arrange
        var role1 = TestDataFactory.CreateRole(name: "UniqueRole");
        DbContext.Roles.Add(role1);
        await DbContext.SaveChangesAsync();

        // Act & Assert
        await using var context2 = CreateNewDbContext();
        var role2 = TestDataFactory.CreateRole(name: "UniqueRole");
        context2.Roles.Add(role2);

        await Assert.ThrowsAsync<DbUpdateException>(
            () => context2.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateRole_WithNullName_ShouldThrowException()
    {
        // Arrange
        var role = TestDataFactory.CreateRole();
        role.Name = null!;

        // Act & Assert
        DbContext.Roles.Add(role);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreateRole_WithNameExceedingMaxLength_ShouldThrowException()
    {
        // Arrange
        var role = TestDataFactory.CreateRole();
        role.Name = new string('R', 101); // Max is 100

        // Act & Assert
        DbContext.Roles.Add(role);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreatePrivilege_WithInvalidRoleId_ShouldThrowException()
    {
        // Arrange
        var privilege = TestDataFactory.CreatePrivilege(
            Guid.NewGuid(), // Non-existent role
            "Events",
            AccessRight.Read);

        // Act & Assert
        DbContext.Privileges.Add(privilege);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreatePrivilege_WithNullAggregate_ShouldThrowException()
    {
        // Arrange
        var role = TestDataFactory.CreateRole();
        DbContext.Roles.Add(role);
        await DbContext.SaveChangesAsync();

        var privilege = TestDataFactory.CreatePrivilege(role.RoleId, "Events", AccessRight.Read);
        privilege.Aggregate = null!;

        // Act & Assert
        DbContext.Privileges.Add(privilege);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task CreatePrivilege_WithAggregateExceedingMaxLength_ShouldThrowException()
    {
        // Arrange
        var role = TestDataFactory.CreateRole();
        DbContext.Roles.Add(role);
        await DbContext.SaveChangesAsync();

        var privilege = TestDataFactory.CreatePrivilege(role.RoleId, "Events", AccessRight.Read);
        privilege.Aggregate = new string('A', 101); // Max is 100

        // Act & Assert
        DbContext.Privileges.Add(privilege);
        await Assert.ThrowsAsync<DbUpdateException>(
            () => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task UpdateNonExistentUser_ShouldThrowException()
    {
        // Arrange
        var nonExistentUser = TestDataFactory.CreateUser();

        // Act & Assert
        DbContext.Users.Update(nonExistentUser);
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
            () => DbContext.SaveChangesAsync());
    }

    #endregion

    #region Many-to-Many Relationship Tests

    [Fact]
    public async Task User_CanHaveMultipleRoles()
    {
        // Arrange
        var role1 = TestDataFactory.CreateRole(name: "Role1");
        var role2 = TestDataFactory.CreateRole(name: "Role2");
        var role3 = TestDataFactory.CreateRole(name: "Role3");

        var user = TestDataFactory.CreateUser(username: "multiroleuser");
        user.Roles.Add(role1);
        user.Roles.Add(role2);
        user.Roles.Add(role3);

        DbContext.Roles.AddRange(role1, role2, role3);
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var result = await queryContext.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.UserId == user.UserId);

        // Assert
        result.Should().NotBeNull();
        result!.Roles.Should().HaveCount(3);
    }

    [Fact]
    public async Task Role_CanHaveMultipleUsers()
    {
        // Arrange
        var role = TestDataFactory.CreateRole(name: "SharedRole");
        var user1 = TestDataFactory.CreateUser(username: "user_a");
        var user2 = TestDataFactory.CreateUser(username: "user_b");
        var user3 = TestDataFactory.CreateUser(username: "user_c");

        user1.Roles.Add(role);
        user2.Roles.Add(role);
        user3.Roles.Add(role);

        DbContext.Roles.Add(role);
        DbContext.Users.AddRange(user1, user2, user3);
        await DbContext.SaveChangesAsync();

        // Act
        await using var queryContext = CreateNewDbContext();
        var result = await queryContext.Roles
            .Include(r => r.Users)
            .FirstOrDefaultAsync(r => r.RoleId == role.RoleId);

        // Assert
        result.Should().NotBeNull();
        result!.Users.Should().HaveCount(3);
    }

    [Fact]
    public async Task RemoveRoleFromUser_ShouldNotDeleteRole()
    {
        // Arrange
        var role = TestDataFactory.CreateRole(name: "PersistentRole");
        var user = TestDataFactory.CreateUser();
        user.Roles.Add(role);

        DbContext.Roles.Add(role);
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        // Act
        await using var updateContext = CreateNewDbContext();
        var userToUpdate = await updateContext.Users
            .Include(u => u.Roles)
            .FirstAsync(u => u.UserId == user.UserId);

        userToUpdate.Roles.Clear();
        await updateContext.SaveChangesAsync();

        // Assert
        await using var verifyContext = CreateNewDbContext();
        var updatedUser = await verifyContext.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.UserId == user.UserId);

        var persistentRole = await verifyContext.Roles
            .FirstOrDefaultAsync(r => r.RoleId == role.RoleId);

        updatedUser!.Roles.Should().BeEmpty();
        persistentRole.Should().NotBeNull(); // Role should still exist
    }

    #endregion
}
