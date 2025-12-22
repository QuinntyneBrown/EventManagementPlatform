// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementPlatform.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.UserId);
        builder.Property(x => x.Username).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Password).IsRequired();
        builder.Property(x => x.Salt).IsRequired();
        builder.Property(x => x.RefreshToken).HasMaxLength(500);
        builder.HasIndex(x => x.Username).IsUnique();
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity("UserRoles");
    }
}
