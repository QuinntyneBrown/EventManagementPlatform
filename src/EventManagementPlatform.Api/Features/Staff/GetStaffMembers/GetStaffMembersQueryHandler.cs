// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Staff.GetStaffMembers;

public class GetStaffMembersQueryHandler : IRequestHandler<GetStaffMembersQuery, GetStaffMembersResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public GetStaffMembersQueryHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<GetStaffMembersResponse> Handle(GetStaffMembersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.StaffMembers.AsQueryable();

        if (request.Status.HasValue)
            query = query.Where(x => x.Status == request.Status.Value);

        if (request.Role.HasValue)
            query = query.Where(x => x.Role == request.Role.Value);

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(x => x.FirstName.Contains(request.Search) ||
                                     x.LastName.Contains(request.Search) ||
                                     x.Email.Contains(request.Search));

        var totalCount = await query.CountAsync(cancellationToken);

        var staffMembers = await query
            .OrderBy(x => x.LastName).ThenBy(x => x.FirstName)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new StaffMemberDto(
                x.StaffMemberId,
                x.FirstName,
                x.LastName,
                x.Email,
                x.PhoneNumber,
                x.PhotoUrl,
                x.Status,
                x.HireDate,
                x.TerminationDate,
                x.Role,
                x.HourlyRate,
                x.CreatedAt,
                x.ModifiedAt))
            .ToListAsync(cancellationToken);

        return new GetStaffMembersResponse(staffMembers, totalCount, request.Page, request.PageSize);
    }
}
