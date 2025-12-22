// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Api.Features.Venues;
using EventManagementPlatform.Api.Features.Venues.CreateVenue;
using EventManagementPlatform.Api.Features.Venues.DeleteVenue;
using EventManagementPlatform.Api.Features.Venues.GetVenueById;
using EventManagementPlatform.Api.Features.Venues.GetVenues;
using EventManagementPlatform.Api.Features.Venues.UpdateVenue;
using EventManagementPlatform.Core.Model.VenueAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VenuesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VenuesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetVenuesResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetVenuesResponse>> GetVenues(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] VenueStatus? status = null,
        [FromQuery] VenueType? type = null,
        [FromQuery] string? city = null,
        [FromQuery] int? minCapacity = null)
    {
        var response = await _mediator.Send(new GetVenuesQuery(page, pageSize, status, type, city, minCapacity));
        return Ok(response);
    }

    [HttpGet("{venueId:guid}")]
    [ProducesResponseType(typeof(GetVenueByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetVenueByIdResponse>> GetVenueById(Guid venueId)
    {
        try
        {
            var response = await _mediator.Send(new GetVenueByIdQuery(venueId));
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateVenueResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateVenueResponse>> CreateVenue([FromBody] CreateVenueCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetVenueById), new { venueId = response.Venue.VenueId }, response);
    }

    [HttpPut("{venueId:guid}")]
    [ProducesResponseType(typeof(UpdateVenueResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateVenueResponse>> UpdateVenue(Guid venueId, [FromBody] UpdateVenueCommand command)
    {
        if (venueId != command.VenueId)
            return BadRequest("Venue ID mismatch");

        try
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{venueId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteVenue(Guid venueId)
    {
        try
        {
            await _mediator.Send(new DeleteVenueCommand(venueId));
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
