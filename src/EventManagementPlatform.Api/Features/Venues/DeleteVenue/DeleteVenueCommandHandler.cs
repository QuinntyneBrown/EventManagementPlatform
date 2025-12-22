// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Venues.DeleteVenue;

public class DeleteVenueCommandHandler : IRequestHandler<DeleteVenueCommand, Unit>
{
    private readonly IEventManagementPlatformContext _context;

    public DeleteVenueCommandHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteVenueCommand request, CancellationToken cancellationToken)
    {
        var venue = await _context.Venues
            .FirstOrDefaultAsync(x => x.VenueId == request.VenueId, cancellationToken)
            ?? throw new KeyNotFoundException($"Venue with ID {request.VenueId} not found");

        venue.IsDeleted = true;
        venue.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
