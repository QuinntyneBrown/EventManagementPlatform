// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Api.Features.Customers;
using EventManagementPlatform.Api.Features.Customers.CreateCustomer;
using EventManagementPlatform.Api.Features.Customers.DeleteCustomer;
using EventManagementPlatform.Api.Features.Customers.GetCustomerById;
using EventManagementPlatform.Api.Features.Customers.GetCustomers;
using EventManagementPlatform.Api.Features.Customers.UpdateCustomer;
using EventManagementPlatform.Core.Model.CustomerAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetCustomersResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetCustomersResponse>> GetCustomers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] CustomerStatus? status = null,
        [FromQuery] string? search = null)
    {
        var response = await _mediator.Send(new GetCustomersQuery(page, pageSize, status, search));
        return Ok(response);
    }

    [HttpGet("{customerId:guid}")]
    [ProducesResponseType(typeof(GetCustomerByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetCustomerByIdResponse>> GetCustomerById(Guid customerId)
    {
        try
        {
            var response = await _mediator.Send(new GetCustomerByIdQuery(customerId));
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateCustomerResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateCustomerResponse>> CreateCustomer([FromBody] CreateCustomerCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCustomerById), new { customerId = response.Customer.CustomerId }, response);
    }

    [HttpPut("{customerId:guid}")]
    [ProducesResponseType(typeof(UpdateCustomerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateCustomerResponse>> UpdateCustomer(Guid customerId, [FromBody] UpdateCustomerCommand command)
    {
        if (customerId != command.CustomerId)
            return BadRequest("Customer ID mismatch");

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

    [HttpDelete("{customerId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCustomer(Guid customerId)
    {
        try
        {
            await _mediator.Send(new DeleteCustomerCommand(customerId));
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
