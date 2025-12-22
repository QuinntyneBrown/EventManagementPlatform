// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.EquipmentAggregate;
using MediatR;

namespace EventManagementPlatform.Api.Features.Equipment.UpdateEquipmentItem;

public record UpdateEquipmentItemCommand(
    Guid EquipmentItemId,
    string Name,
    string? Description,
    EquipmentCategory Category,
    EquipmentCondition Condition,
    decimal? CurrentValue,
    string? Manufacturer,
    string? Model,
    string? SerialNumber,
    string? WarehouseLocation): IRequest<UpdateEquipmentItemResponse>;
