// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Equipment.DeleteEquipmentItem;

public class DeleteEquipmentItemCommandHandler : IRequestHandler<DeleteEquipmentItemCommand, Unit>
{
    private readonly IEventManagementPlatformContext _context;

    public DeleteEquipmentItemCommandHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteEquipmentItemCommand request, CancellationToken cancellationToken)
    {
        var equipment = await _context.EquipmentItems
            .FirstOrDefaultAsync(x => x.EquipmentItemId == request.EquipmentItemId, cancellationToken)
            ?? throw new KeyNotFoundException($"Equipment item with ID {request.EquipmentItemId} not found");

        equipment.IsActive = false;
        equipment.IsDeleted = true;
        equipment.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
