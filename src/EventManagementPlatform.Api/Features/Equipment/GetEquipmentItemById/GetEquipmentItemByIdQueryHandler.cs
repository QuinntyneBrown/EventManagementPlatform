// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Equipment.GetEquipmentItemById;

public class GetEquipmentItemByIdQueryHandler : IRequestHandler<GetEquipmentItemByIdQuery, GetEquipmentItemByIdResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public GetEquipmentItemByIdQueryHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<GetEquipmentItemByIdResponse> Handle(GetEquipmentItemByIdQuery request, CancellationToken cancellationToken)
    {
        var equipment = await _context.EquipmentItems
            .FirstOrDefaultAsync(x => x.EquipmentItemId == request.EquipmentItemId, cancellationToken)
            ?? throw new KeyNotFoundException($"Equipment item with ID {request.EquipmentItemId} not found");

        return new GetEquipmentItemByIdResponse(new EquipmentItemDto(
            equipment.EquipmentItemId,
            equipment.Name,
            equipment.Description,
            equipment.Category,
            equipment.Condition,
            equipment.Status,
            equipment.PurchaseDate,
            equipment.PurchasePrice,
            equipment.CurrentValue,
            equipment.Manufacturer,
            equipment.Model,
            equipment.SerialNumber,
            equipment.WarehouseLocation,
            equipment.IsActive,
            equipment.CreatedAt,
            equipment.ModifiedAt));
    }
}
