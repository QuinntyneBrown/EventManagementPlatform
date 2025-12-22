// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using EventManagementPlatform.Core.Model.CustomerAggregate;
using EventManagementPlatform.Core.Model.EquipmentAggregate;
using EventManagementPlatform.Core.Model.EventAggregate;
using EventManagementPlatform.Core.Model.StaffAggregate;
using EventManagementPlatform.Core.Model.UserAggregate;
using EventManagementPlatform.Core.Model.VenueAggregate;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Infrastructure;

public class EventManagementPlatformDbContext : DbContext, IEventManagementPlatformContext
{
    public EventManagementPlatformDbContext(DbContextOptions<EventManagementPlatformDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Privilege> Privileges => Set<Privilege>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventType> EventTypes => Set<EventType>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Venue> Venues => Set<Venue>();
    public DbSet<StaffMember> StaffMembers => Set<StaffMember>();
    public DbSet<EquipmentItem> EquipmentItems => Set<EquipmentItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventManagementPlatformDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
