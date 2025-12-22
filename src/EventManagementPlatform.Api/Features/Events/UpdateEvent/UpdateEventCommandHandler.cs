// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Events.UpdateEvent;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, UpdateEventResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public UpdateEventCommandHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<UpdateEventResponse> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await _context.Events
            .FirstOrDefaultAsync(x => x.EventId == request.EventId, cancellationToken)
            ?? throw new KeyNotFoundException($"Event with ID {request.EventId} not found");

        @event.Title = request.Title;
        @event.Description = request.Description;
        @event.EventDate = request.EventDate;
        @event.VenueId = request.VenueId;
        @event.EventTypeId = request.EventTypeId;
        @event.CustomerId = request.CustomerId;
        @event.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateEventResponse(new EventDto(
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
