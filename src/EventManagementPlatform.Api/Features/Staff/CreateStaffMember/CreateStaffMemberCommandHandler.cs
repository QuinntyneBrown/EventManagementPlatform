// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using EventManagementPlatform.Core.Model.StaffAggregate;
using MediatR;

namespace EventManagementPlatform.Api.Features.Staff.CreateStaffMember;

public class CreateStaffMemberCommandHandler : IRequestHandler<CreateStaffMemberCommand, CreateStaffMemberResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public CreateStaffMemberCommandHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<CreateStaffMemberResponse> Handle(CreateStaffMemberCommand request, CancellationToken cancellationToken)
    {
        var staffMember = new StaffMember
        {
            StaffMemberId = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            HireDate = request.HireDate,
            Role = request.Role,
            HourlyRate = request.HourlyRate,
            Status = StaffStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        _context.StaffMembers.Add(staffMember);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateStaffMemberResponse(new StaffMemberDto(
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
