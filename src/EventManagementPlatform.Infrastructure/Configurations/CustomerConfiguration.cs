// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.CustomerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementPlatform.Infrastructure.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(x => x.CustomerId);
        builder.Property(x => x.CompanyName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.PrimaryEmail).IsRequired().HasMaxLength(255);
        builder.Property(x => x.PrimaryPhone).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Industry).HasMaxLength(100);
        builder.Property(x => x.Website).HasMaxLength(500);
        builder.Property(x => x.BillingStreet).HasMaxLength(200);
        builder.Property(x => x.BillingCity).HasMaxLength(100);
        builder.Property(x => x.BillingState).HasMaxLength(100);
        builder.Property(x => x.BillingZipCode).HasMaxLength(20);
        builder.Property(x => x.BillingCountry).HasMaxLength(100);
        builder.Property(x => x.Status).IsRequired();
        builder.HasIndex(x => x.PrimaryEmail);
        builder.HasIndex(x => x.CompanyName);
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
