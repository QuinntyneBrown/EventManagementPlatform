// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.EquipmentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementPlatform.Infrastructure.Configurations;

public class EquipmentItemConfiguration : IEntityTypeConfiguration<EquipmentItem>
{
    public void Configure(EntityTypeBuilder<EquipmentItem> builder)
    {
        builder.ToTable("EquipmentItems");
        builder.HasKey(x => x.EquipmentItemId);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.Category).IsRequired();
        builder.Property(x => x.Condition).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.PurchasePrice).HasPrecision(18, 2);
        builder.Property(x => x.CurrentValue).HasPrecision(18, 2);
        builder.Property(x => x.Manufacturer).HasMaxLength(200);
        builder.Property(x => x.Model).HasMaxLength(200);
        builder.Property(x => x.SerialNumber).HasMaxLength(100);
        builder.Property(x => x.WarehouseLocation).HasMaxLength(100);
        builder.HasIndex(x => x.Category);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => new { x.Name, x.Category });
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
