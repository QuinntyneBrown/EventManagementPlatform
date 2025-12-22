// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using EventManagementPlatform.Core.Model.CustomerAggregate;
using MediatR;

namespace EventManagementPlatform.Api.Features.Customers.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public CreateCustomerCommandHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<CreateCustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            CustomerId = Guid.NewGuid(),
            CompanyName = request.Company,
            Type = request.Type,
            PrimaryEmail = request.Email,
            PrimaryPhone = request.Phone,
            Industry = request.Industry,
            Website = request.Website,
            BillingStreet = request.Address,
            BillingCity = request.City,
            BillingState = request.State,
            BillingZipCode = request.ZipCode,
            BillingCountry = request.BillingCountry,
            Status = CustomerStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateCustomerResponse(new CustomerDto(
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
            customer.BillingZipCode,
            customer.BillingCountry,
            customer.Status,
            customer.CreatedAt,
            customer.ModifiedAt));
    }
}
