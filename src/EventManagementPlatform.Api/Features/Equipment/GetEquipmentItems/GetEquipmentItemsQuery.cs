// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.EquipmentAggregate;
using MediatR;

namespace EventManagementPlatform.Api.Features.Equipment.GetEquipmentItems;

public record GetEquipmentItemsQuery(
    int Page = 1,
    int PageSize = 20,
    EquipmentCategory? Category = null,
    EquipmentStatus? Status = null,
    EquipmentCondition? Condition = null,
    bool? IsActive = null,
    string? Search = null) : IRequest<GetEquipmentItemsResponse>;
