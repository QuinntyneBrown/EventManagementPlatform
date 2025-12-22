// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Api.Features.Equipment;
using EventManagementPlatform.Api.Features.Equipment.CreateEquipmentItem;
using EventManagementPlatform.Api.Features.Equipment.DeleteEquipmentItem;
using EventManagementPlatform.Api.Features.Equipment.GetEquipmentItemById;
using EventManagementPlatform.Api.Features.Equipment.GetEquipmentItems;
using EventManagementPlatform.Api.Features.Equipment.UpdateEquipmentItem;
using EventManagementPlatform.Core.Model.EquipmentAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EquipmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public EquipmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetEquipmentItemsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetEquipmentItemsResponse>> GetEquipmentItems(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] EquipmentCategory? category = null,
        [FromQuery] EquipmentStatus? status = null,
        [FromQuery] EquipmentCondition? condition = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? search = null)
    {
        var response = await _mediator.Send(new GetEquipmentItemsQuery(page, pageSize, category, status, condition, isActive, search));
        return Ok(response);
    }

    [HttpGet("{equipmentItemId:guid}")]
    [ProducesResponseType(typeof(GetEquipmentItemByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetEquipmentItemByIdResponse>> GetEquipmentItemById(Guid equipmentItemId)
    {
        try
        {
            var response = await _mediator.Send(new GetEquipmentItemByIdQuery(equipmentItemId));
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateEquipmentItemResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateEquipmentItemResponse>> CreateEquipmentItem([FromBody] CreateEquipmentItemCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetEquipmentItemById), new { equipmentItemId = response.EquipmentItem.EquipmentItemId }, response);
    }

    [HttpPut("{equipmentItemId:guid}")]
    [ProducesResponseType(typeof(UpdateEquipmentItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateEquipmentItemResponse>> UpdateEquipmentItem(Guid equipmentItemId, [FromBody] UpdateEquipmentItemCommand command)
    {
        if (equipmentItemId != command.EquipmentItemId)
            return BadRequest("Equipment item ID mismatch");

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

    [HttpDelete("{equipmentItemId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEquipmentItem(Guid equipmentItemId)
    {
        try
        {
            await _mediator.Send(new DeleteEquipmentItemCommand(equipmentItemId));
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
