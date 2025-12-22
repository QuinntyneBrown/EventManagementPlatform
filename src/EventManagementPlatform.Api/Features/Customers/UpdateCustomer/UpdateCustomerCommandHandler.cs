// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Customers.UpdateCustomer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, UpdateCustomerResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public UpdateCustomerCommandHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<UpdateCustomerResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer with ID {request.CustomerId} not found");

        customer.CompanyName = request.CompanyName;
        customer.Type = request.Type;
        customer.PrimaryEmail = request.PrimaryEmail;
        customer.PrimaryPhone = request.PrimaryPhone;
        customer.Industry = request.Industry;
        customer.Website = request.Website;
        customer.BillingStreet = request.BillingStreet;
        customer.BillingCity = request.BillingCity;
        customer.BillingState = request.BillingState;
        customer.BillingPostalCode = request.BillingPostalCode;
        customer.BillingCountry = request.BillingCountry;
        customer.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateCustomerResponse(new CustomerDto(
            customer.CustomerId,
            customer.CompanyName,
            customer.Type,
            customer.PrimaryEmail,
            customer.PrimaryPhone,
            customer.Industry,
            customer.Website,
            customer.BillingStreet,
            customer.BillingCity,
            customer.BillingState,
            customer.BillingPostalCode,
            customer.BillingCountry,
            customer.Status,
            customer.CreatedAt,
            customer.ModifiedAt));
    }
}
