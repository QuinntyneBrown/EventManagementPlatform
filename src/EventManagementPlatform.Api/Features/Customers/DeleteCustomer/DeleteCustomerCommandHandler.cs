// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Customers.DeleteCustomer;

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Unit>
{
    private readonly IEventManagementPlatformContext _context;

    public DeleteCustomerCommandHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer with ID {request.CustomerId} not found");

        customer.IsDeleted = true;
        customer.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
