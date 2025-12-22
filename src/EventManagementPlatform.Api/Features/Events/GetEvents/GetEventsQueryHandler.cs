// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Events.GetEvents;

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, GetEventsResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public GetEventsQueryHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<GetEventsResponse> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Events.AsQueryable();

        if (request.Status.HasValue)
            query = query.Where(x => x.Status == request.Status.Value);

        if (request.VenueId.HasValue)
            query = query.Where(x => x.VenueId == request.VenueId.Value);

        if (request.CustomerId.HasValue)
            query = query.Where(x => x.CustomerId == request.CustomerId.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var events = await query
            .OrderByDescending(x => x.EventDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new EventDto(
                x.EventId,
                x.Title,
                x.Description,
                x.EventDate,
                x.VenueId,
                x.EventTypeId,
                x.CustomerId,
                x.Status,
                x.CreatedAt,
                x.ModifiedAt))
            .ToListAsync(cancellationToken);

        return new GetEventsResponse(events, totalCount, request.Page, request.PageSize);
    }
}
