// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using EventManagementPlatform.Core.Model.EquipmentAggregate;
using MediatR;

namespace EventManagementPlatform.Api.Features.Equipment.CreateEquipmentItem;

public class CreateEquipmentItemCommandHandler : IRequestHandler<CreateEquipmentItemCommand, CreateEquipmentItemResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public CreateEquipmentItemCommandHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<CreateEquipmentItemResponse> Handle(CreateEquipmentItemCommand request, CancellationToken cancellationToken)
    {
        var equipment = new EquipmentItem
        {
            EquipmentItemId = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Category = request.Category,
            Condition = request.Condition,
            Status = EquipmentStatus.Available,
            PurchaseDate = request.PurchaseDate,
            PurchasePrice = request.PurchasePrice,
            CurrentValue = request.CurrentValue,
            Manufacturer = request.Manufacturer,
            Model = request.Model,
            SerialNumber = request.SerialNumber,
            WarehouseLocation = request.WarehouseLocation,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.EquipmentItems.Add(equipment);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateEquipmentItemResponse(new EquipmentItemDto(
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
