// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Equipment.UpdateEquipmentItem;

public class UpdateEquipmentItemCommandHandler : IRequestHandler<UpdateEquipmentItemCommand, UpdateEquipmentItemResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public UpdateEquipmentItemCommandHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<UpdateEquipmentItemResponse> Handle(UpdateEquipmentItemCommand request, CancellationToken cancellationToken)
    {
        var equipment = await _context.EquipmentItems
            .FirstOrDefaultAsync(x => x.EquipmentItemId == request.EquipmentItemId, cancellationToken)
            ?? throw new KeyNotFoundException($"Equipment item with ID {request.EquipmentItemId} not found");

        equipment.Name = request.Name;
        equipment.Description = request.Description;
        equipment.Category = request.Category;
        equipment.Condition = request.Condition;
        equipment.CurrentValue = request.CurrentValue;
        equipment.Manufacturer = request.Manufacturer;
        equipment.Model = request.Model;
        equipment.SerialNumber = request.SerialNumber;
        equipment.WarehouseLocation = request.WarehouseLocation;
        equipment.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateEquipmentItemResponse(new EquipmentItemDto(
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
