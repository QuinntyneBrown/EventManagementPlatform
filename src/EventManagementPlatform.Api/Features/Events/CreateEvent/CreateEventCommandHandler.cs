// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using EventManagementPlatform.Core.Model.EventAggregate;
using MediatR;

namespace EventManagementPlatform.Api.Features.Events.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, CreateEventResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public CreateEventCommandHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<CreateEventResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var @event = new Event
        {
            EventId = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            EventDate = request.EventDate,
            VenueId = request.VenueId,
            EventTypeId = request.EventTypeId,
            CustomerId = request.CustomerId,
            Status = EventStatus.Planned,
            CreatedAt = DateTime.UtcNow
        };

        _context.Events.Add(@event);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateEventResponse(new EventDto(
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
