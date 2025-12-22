// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Venues.UpdateVenue;

public class UpdateVenueCommandHandler : IRequestHandler<UpdateVenueCommand, UpdateVenueResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public UpdateVenueCommandHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<UpdateVenueResponse> Handle(UpdateVenueCommand request, CancellationToken cancellationToken)
    {
        var venue = await _context.Venues
            .FirstOrDefaultAsync(x => x.VenueId == request.VenueId, cancellationToken)
            ?? throw new KeyNotFoundException($"Venue with ID {request.VenueId} not found");

        venue.Name = request.Name;
        venue.Description = request.Description;
        venue.Type = request.Type;
        venue.Street = request.Street;
        venue.City = request.City;
        venue.State = request.State;
        venue.PostalCode = request.PostalCode;
        venue.Country = request.Country;
        venue.MaxCapacity = request.MaxCapacity;
        venue.SeatedCapacity = request.SeatedCapacity;
        venue.StandingCapacity = request.StandingCapacity;
        venue.ContactName = request.ContactName;
        venue.ContactEmail = request.ContactEmail;
        venue.ContactPhone = request.ContactPhone;
        venue.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateVenueResponse(new VenueDto(
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
