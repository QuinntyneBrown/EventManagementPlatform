// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Venues.GetVenueById;

public class GetVenueByIdQueryHandler : IRequestHandler<GetVenueByIdQuery, GetVenueByIdResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public GetVenueByIdQueryHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<GetVenueByIdResponse> Handle(GetVenueByIdQuery request, CancellationToken cancellationToken)
    {
        var venue = await _context.Venues
            .FirstOrDefaultAsync(x => x.VenueId == request.VenueId, cancellationToken)
            ?? throw new KeyNotFoundException($"Venue with ID {request.VenueId} not found");

        return new GetVenueByIdResponse(new VenueDto(
            venue.VenueId,
            venue.Name,
            venue.Description,
            venue.Type,
            venue.Street,
            venue.City,
            venue.State,
            venue.PostalCode,
            venue.Country,
            venue.MaxCapacity,
            venue.SeatedCapacity,
            venue.StandingCapacity,
            venue.ContactName,
            venue.ContactEmail,
            venue.ContactPhone,
            venue.Status,
            venue.CreatedAt,
            venue.ModifiedAt));
    }
}
