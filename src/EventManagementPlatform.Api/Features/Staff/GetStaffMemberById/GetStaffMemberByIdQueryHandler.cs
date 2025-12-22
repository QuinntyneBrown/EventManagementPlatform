// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Staff.GetStaffMemberById;

public class GetStaffMemberByIdQueryHandler : IRequestHandler<GetStaffMemberByIdQuery, GetStaffMemberByIdResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public GetStaffMemberByIdQueryHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<GetStaffMemberByIdResponse> Handle(GetStaffMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var staffMember = await _context.StaffMembers
            .FirstOrDefaultAsync(x => x.StaffMemberId == request.StaffMemberId, cancellationToken)
            ?? throw new KeyNotFoundException($"Staff member with ID {request.StaffMemberId} not found");

        return new GetStaffMemberByIdResponse(new StaffMemberDto(
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
