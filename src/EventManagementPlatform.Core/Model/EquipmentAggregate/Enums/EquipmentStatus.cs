// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace EventManagementPlatform.Core.Model.EquipmentAggregate;

public enum EquipmentStatus
{
    Available = 0,
    Reserved = 1,
    InTransit = 2,
    AtVenue = 3,
    InMaintenance = 4,
    Retired = 5,
}
