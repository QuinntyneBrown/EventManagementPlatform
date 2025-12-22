// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using EventManagementPlatform.Core.Model.StaffAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Staff.DeleteStaffMember;

public class DeleteStaffMemberCommandHandler : IRequestHandler<DeleteStaffMemberCommand, Unit>
{
    private readonly IEventManagementPlatformContext _context;

    public DeleteStaffMemberCommandHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteStaffMemberCommand request, CancellationToken cancellationToken)
    {
        var staffMember = await _context.StaffMembers
            .FirstOrDefaultAsync(x => x.StaffMemberId == request.StaffMemberId, cancellationToken)
            ?? throw new KeyNotFoundException($"Staff member with ID {request.StaffMemberId} not found");

        staffMember.Status = StaffStatus.Terminated;
        staffMember.TerminationDate = DateTime.UtcNow;
        staffMember.IsDeleted = true;
        staffMember.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
