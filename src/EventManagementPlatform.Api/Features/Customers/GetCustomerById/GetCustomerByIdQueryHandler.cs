// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Customers.GetCustomerById;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, GetCustomerByIdResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public GetCustomerByIdQueryHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<GetCustomerByIdResponse> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer with ID {request.CustomerId} not found");

        return new GetCustomerByIdResponse(new CustomerDto(
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
