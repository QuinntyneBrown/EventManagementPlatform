// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Events.UpdateEventStatus;

public class UpdateEventStatusCommandHandler : IRequestHandler<UpdateEventStatusCommand, UpdateEventStatusResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public UpdateEventStatusCommandHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<UpdateEventStatusResponse> Handle(UpdateEventStatusCommand request, CancellationToken cancellationToken)
    {
        var @event = await _context.Events
            .FirstOrDefaultAsync(x => x.EventId == request.EventId, cancellationToken)
            ?? throw new KeyNotFoundException($"Event with ID {request.EventId} not found");

        @event.Status = request.Status;
        @event.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateEventStatusResponse(new EventDto(
            @event.EventId,
            @event.Title,
            @event.Description,
            @event.EventDate,
            @event.VenueId,
            @event.EventTypeId,
            @event.CustomerId,
            @event.Status,
            @event.CreatedAt,
            @event.ModifiedAt));
    }
}
