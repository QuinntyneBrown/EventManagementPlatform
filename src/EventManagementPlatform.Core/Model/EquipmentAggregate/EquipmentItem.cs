// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace EventManagementPlatform.Core.Model.EquipmentAggregate;

public class EquipmentItem
{
    public Guid EquipmentItemId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public EquipmentCategory Category { get; set; } = EquipmentCategory.Other;

    public EquipmentCondition Condition { get; set; } = EquipmentCondition.Good;

    public EquipmentStatus Status { get; set; } = EquipmentStatus.Available;

    public DateTime PurchaseDate { get; set; }

    public decimal PurchasePrice { get; set; }

    public decimal? CurrentValue { get; set; }

    public string? Manufacturer { get; set; }

    public string? Model { get; set; }

    public string? SerialNumber { get; set; }

    public string? WarehouseLocation { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }
}
