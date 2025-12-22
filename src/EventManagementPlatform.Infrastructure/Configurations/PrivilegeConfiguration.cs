// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementPlatform.Infrastructure.Configurations;

public class PrivilegeConfiguration : IEntityTypeConfiguration<Privilege>
{
    public void Configure(EntityTypeBuilder<Privilege> builder)
    {
        builder.ToTable("Privileges");
        builder.HasKey(x => x.PrivilegeId);
        builder.Property(x => x.Aggregate).IsRequired().HasMaxLength(100);
        builder.Property(x => x.AccessRight).IsRequired();
    }
}
