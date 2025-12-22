// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.StaffAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementPlatform.Infrastructure.Configurations;

public class StaffMemberConfiguration : IEntityTypeConfiguration<StaffMember>
{
    public void Configure(EntityTypeBuilder<StaffMember> builder)
    {
        builder.ToTable("StaffMembers");
        builder.HasKey(x => x.StaffMemberId);
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
        builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(50);
        builder.Property(x => x.PhotoUrl).HasMaxLength(1000);
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.Role).IsRequired();
        builder.Property(x => x.HourlyRate).HasPrecision(18, 2);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.Role);
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
