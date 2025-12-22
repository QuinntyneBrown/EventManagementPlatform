// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.EquipmentAggregate;

namespace EventManagementPlatform.Api.Features.Equipment;

public record EquipmentItemDto(
    Guid EquipmentItemId,
    string Name,
    string? Description,
    EquipmentCategory Category,
    EquipmentCondition Condition,
    EquipmentStatus Status,
    DateTime PurchaseDate,
    decimal PurchasePrice,
    decimal? CurrentValue,
    string? Manufacturer,
    string? Model,
    string? SerialNumber,
    string? WarehouseLocation,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? ModifiedAt);
