// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.VenueAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementPlatform.Infrastructure.Configurations;

public class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.ToTable("Venues");
        builder.HasKey(x => x.VenueId);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.Street).HasMaxLength(200);
        builder.Property(x => x.City).HasMaxLength(100);
        builder.Property(x => x.State).HasMaxLength(100);
        builder.Property(x => x.PostalCode).HasMaxLength(20);
        builder.Property(x => x.Country).HasMaxLength(100);
        builder.Property(x => x.ContactName).HasMaxLength(200);
        builder.Property(x => x.ContactEmail).HasMaxLength(255);
        builder.Property(x => x.ContactPhone).HasMaxLength(50);
        builder.Property(x => x.Status).IsRequired();
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.City);
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
