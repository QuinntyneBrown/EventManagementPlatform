// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Api.Features.Events;
using EventManagementPlatform.Api.Features.Events.CreateEvent;
using EventManagementPlatform.Api.Features.Events.DeleteEvent;
using EventManagementPlatform.Api.Features.Events.GetEventById;
using EventManagementPlatform.Api.Features.Events.GetEvents;
using EventManagementPlatform.Api.Features.Events.UpdateEvent;
using EventManagementPlatform.Api.Features.Events.UpdateEventStatus;
using EventManagementPlatform.Core.Model.EventAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetEventsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetEventsResponse>> GetEvents(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] EventStatus? status = null,
        [FromQuery] Guid? venueId = null,
        [FromQuery] Guid? customerId = null)
    {
        var response = await _mediator.Send(new GetEventsQuery(page, pageSize, status, venueId, customerId));
        return Ok(response);
    }

    [HttpGet("{eventId:guid}")]
    [ProducesResponseType(typeof(GetEventByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetEventByIdResponse>> GetEventById(Guid eventId)
    {
        try
        {
            var response = await _mediator.Send(new GetEventByIdQuery(eventId));
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateEventResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateEventResponse>> CreateEvent([FromBody] CreateEventCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetEventById), new { eventId = response.Event.EventId }, response);
    }

    [HttpPut("{eventId:guid}")]
    [ProducesResponseType(typeof(UpdateEventResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateEventResponse>> UpdateEvent(Guid eventId, [FromBody] UpdateEventCommand command)
    {
        if (eventId != command.EventId)
            return BadRequest("Event ID mismatch");

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

    [HttpDelete("{eventId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEvent(Guid eventId)
    {
        try
        {
            await _mediator.Send(new DeleteEventCommand(eventId));
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{eventId:guid}/status")]
    [ProducesResponseType(typeof(UpdateEventStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateEventStatusResponse>> UpdateEventStatus(Guid eventId, [FromBody] EventStatus status)
    {
        try
        {
            var response = await _mediator.Send(new UpdateEventStatusCommand(eventId, status));
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
