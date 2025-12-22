// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Api.Features.Staff;
using EventManagementPlatform.Api.Features.Staff.CreateStaffMember;
using EventManagementPlatform.Api.Features.Staff.DeleteStaffMember;
using EventManagementPlatform.Api.Features.Staff.GetStaffMemberById;
using EventManagementPlatform.Api.Features.Staff.GetStaffMembers;
using EventManagementPlatform.Api.Features.Staff.UpdateStaffMember;
using EventManagementPlatform.Core.Model.StaffAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StaffController : ControllerBase
{
    private readonly IMediator _mediator;

    public StaffController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetStaffMembersResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetStaffMembersResponse>> GetStaffMembers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] StaffStatus? status = null,
        [FromQuery] StaffRole? role = null,
        [FromQuery] string? search = null)
    {
        var response = await _mediator.Send(new GetStaffMembersQuery(page, pageSize, status, role, search));
        return Ok(response);
    }

    [HttpGet("{staffMemberId:guid}")]
    [ProducesResponseType(typeof(GetStaffMemberByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetStaffMemberByIdResponse>> GetStaffMemberById(Guid staffMemberId)
    {
        try
        {
            var response = await _mediator.Send(new GetStaffMemberByIdQuery(staffMemberId));
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateStaffMemberResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateStaffMemberResponse>> CreateStaffMember([FromBody] CreateStaffMemberCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetStaffMemberById), new { staffMemberId = response.StaffMember.StaffMemberId }, response);
    }

    [HttpPut("{staffMemberId:guid}")]
    [ProducesResponseType(typeof(UpdateStaffMemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateStaffMemberResponse>> UpdateStaffMember(Guid staffMemberId, [FromBody] UpdateStaffMemberCommand command)
    {
        if (staffMemberId != command.StaffMemberId)
            return BadRequest("Staff member ID mismatch");

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

    [HttpDelete("{staffMemberId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteStaffMember(Guid staffMemberId)
    {
        try
        {
            await _mediator.Send(new DeleteStaffMemberCommand(staffMemberId));
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
