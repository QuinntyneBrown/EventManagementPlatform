// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Staff.UpdateStaffMember;

public class UpdateStaffMemberCommandHandler : IRequestHandler<UpdateStaffMemberCommand, UpdateStaffMemberResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public UpdateStaffMemberCommandHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<UpdateStaffMemberResponse> Handle(UpdateStaffMemberCommand request, CancellationToken cancellationToken)
    {
        var staffMember = await _context.StaffMembers
            .FirstOrDefaultAsync(x => x.StaffMemberId == request.StaffMemberId, cancellationToken)
            ?? throw new KeyNotFoundException($"Staff member with ID {request.StaffMemberId} not found");

        staffMember.FirstName = request.FirstName;
        staffMember.LastName = request.LastName;
        staffMember.PhoneNumber = request.PhoneNumber;
        staffMember.Role = request.Role;
        staffMember.HourlyRate = request.HourlyRate;
        staffMember.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateStaffMemberResponse(new StaffMemberDto(
            staffMember.StaffMemberId,
            staffMember.FirstName,
            staffMember.LastName,
            staffMember.Email,
            staffMember.PhoneNumber,
            staffMember.PhotoUrl,
            staffMember.Status,
            staffMember.HireDate,
            staffMember.TerminationDate,
            staffMember.Role,
            staffMember.HourlyRate,
            staffMember.CreatedAt,
            staffMember.ModifiedAt));
    }
}
