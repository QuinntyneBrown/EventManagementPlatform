// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.EventAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementPlatform.Infrastructure.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");
        builder.HasKey(x => x.EventId);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.Status).IsRequired();
        builder.HasIndex(x => x.EventDate);
        builder.HasIndex(x => x.VenueId);
        builder.HasIndex(x => x.CustomerId);
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.HasOne(x => x.EventType)
            .WithMany(x => x.Events)
            .HasForeignKey(x => x.EventTypeId);
    }
}
