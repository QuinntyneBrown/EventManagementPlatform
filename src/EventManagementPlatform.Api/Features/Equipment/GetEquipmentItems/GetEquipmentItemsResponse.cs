// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace EventManagementPlatform.Api.Features.Equipment.GetEquipmentItems;

public record GetEquipmentItemsResponse(IEnumerable<EquipmentItemDto> EquipmentItems, int TotalCount, int Page, int PageSize);
