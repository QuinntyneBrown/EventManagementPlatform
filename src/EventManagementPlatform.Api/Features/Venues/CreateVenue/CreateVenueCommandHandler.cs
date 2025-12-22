// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using EventManagementPlatform.Core.Model.VenueAggregate;
using MediatR;

namespace EventManagementPlatform.Api.Features.Venues.CreateVenue;

public class CreateVenueCommandHandler : IRequestHandler<CreateVenueCommand, CreateVenueResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public CreateVenueCommandHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<CreateVenueResponse> Handle(CreateVenueCommand request, CancellationToken cancellationToken)
    {
        var venue = new Venue
        {
            VenueId = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Type = request.Type,
            Street = request.Street,
            City = request.City,
            State = request.State,
            PostalCode = request.PostalCode,
            Country = request.Country,
            MaxCapacity = request.MaxCapacity,
            SeatedCapacity = request.SeatedCapacity,
            StandingCapacity = request.StandingCapacity,
            ContactName = request.ContactName,
            ContactEmail = request.ContactEmail,
            ContactPhone = request.ContactPhone,
            Status = VenueStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        _context.Venues.Add(venue);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateVenueResponse(new VenueDto(
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
