// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Venues.GetVenues;

public class GetVenuesQueryHandler : IRequestHandler<GetVenuesQuery, GetVenuesResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public GetVenuesQueryHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<GetVenuesResponse> Handle(GetVenuesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Venues.AsQueryable();

        if (request.Status.HasValue)
            query = query.Where(x => x.Status == request.Status.Value);

        if (request.Type.HasValue)
            query = query.Where(x => x.Type == request.Type.Value);

        if (!string.IsNullOrWhiteSpace(request.City))
            query = query.Where(x => x.City.Contains(request.City));

        if (request.MinCapacity.HasValue)
            query = query.Where(x => x.MaxCapacity >= request.MinCapacity.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var venues = await query
            .OrderBy(x => x.Name)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new VenueDto(
                x.VenueId,
                x.Name,
                x.Description,
                x.Type,
                x.Street,
                x.City,
                x.State,
                x.PostalCode,
                x.Country,
                x.MaxCapacity,
                x.SeatedCapacity,
                x.StandingCapacity,
                x.ContactName,
                x.ContactEmail,
                x.ContactPhone,
                x.Status,
                x.CreatedAt,
                x.ModifiedAt))
            .ToListAsync(cancellationToken);

        return new GetVenuesResponse(venues, totalCount, request.Page, request.PageSize);
    }
}
