// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Customers.GetCustomers;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, GetCustomersResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public GetCustomersQueryHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<GetCustomersResponse> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Customers.AsQueryable();

        if (request.Status.HasValue)
            query = query.Where(x => x.Status == request.Status.Value);

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(x => x.CompanyName.Contains(request.Search) || x.PrimaryEmail.Contains(request.Search));

        var totalCount = await query.CountAsync(cancellationToken);

        var customers = await query
            .OrderBy(x => x.CompanyName)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new CustomerDto(
                x.CustomerId,
                x.CompanyName,
                x.Type,
                x.PrimaryEmail,
                x.PrimaryPhone,
                x.Industry,
                x.Website,
                x.BillingStreet,
                x.BillingCity,
                x.BillingState,
                x.BillingZipCode,
                x.BillingCountry,
                x.Status,
                x.CreatedAt,
                x.ModifiedAt))
            .ToListAsync(cancellationToken);

        return new GetCustomersResponse(customers, totalCount, request.Page, request.PageSize);
    }
}
