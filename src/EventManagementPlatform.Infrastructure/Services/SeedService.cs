// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.UserAggregate;
using EventManagementPlatform.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventManagementPlatform.Infrastructure.Services;

public class SeedService : ISeedService
{
    private readonly EventManagementPlatformDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<SeedService> _logger;

    public SeedService(
        EventManagementPlatformDbContext context,
        IPasswordHasher passwordHasher,
        ILogger<SeedService> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting development seed...");

        await EnsureDatabaseCreatedAsync(cancellationToken);
        await SeedAdminUserAsync(cancellationToken);

        _logger.LogInformation("Development seed completed.");
    }

    private async Task EnsureDatabaseCreatedAsync(CancellationToken cancellationToken)
    {
        await _context.Database.EnsureCreatedAsync(cancellationToken);
    }

    private async Task SeedAdminUserAsync(CancellationToken cancellationToken)
    {
        const string adminUsername = "Admin";
        const string adminPassword = "P@ssw0rd";

        var existingAdmin = await _context.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Username == adminUsername, cancellationToken);

        if (existingAdmin != null)
        {
            _logger.LogInformation("Admin user already exists, skipping seed.");
            return;
        }

        var (hash, salt) = _passwordHasher.HashPassword(adminPassword);

        var adminUser = new User
        {
            UserId = Guid.NewGuid(),
            Username = adminUsername,
            Password = hash,
            Salt = salt,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false,
        };

        _context.Users.Add(adminUser);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Admin user created successfully.");
    }
}
