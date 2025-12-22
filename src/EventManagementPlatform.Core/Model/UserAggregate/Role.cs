// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace EventManagementPlatform.Core.Model.UserAggregate;

public class Role
{
    public Guid RoleId { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<Privilege> Privileges { get; set; } = new List<Privilege>();

    public ICollection<User> Users { get; set; } = new List<User>();
}
