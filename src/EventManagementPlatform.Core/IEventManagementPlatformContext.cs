// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.CustomerAggregate;
using EventManagementPlatform.Core.Model.EquipmentAggregate;
using EventManagementPlatform.Core.Model.EventAggregate;
using EventManagementPlatform.Core.Model.StaffAggregate;
using EventManagementPlatform.Core.Model.UserAggregate;
using EventManagementPlatform.Core.Model.VenueAggregate;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Core;

public interface IEventManagementPlatformContext
{
    DbSet<User> Users { get; }

    DbSet<Role> Roles { get; }

    DbSet<Privilege> Privileges { get; }

    DbSet<Event> Events { get; }

    DbSet<EventType> EventTypes { get; }

    DbSet<Customer> Customers { get; }

    DbSet<Venue> Venues { get; }

    DbSet<StaffMember> StaffMembers { get; }

    DbSet<EquipmentItem> EquipmentItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
