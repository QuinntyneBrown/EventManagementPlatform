// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Events.DeleteEvent;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, Unit>
{
    private readonly IEventManagementPlatformContext _context;

    public DeleteEventCommandHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await _context.Events
            .FirstOrDefaultAsync(x => x.EventId == request.EventId, cancellationToken)
            ?? throw new KeyNotFoundException($"Event with ID {request.EventId} not found");

        @event.IsDeleted = true;
        @event.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
